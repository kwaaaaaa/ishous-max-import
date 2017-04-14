using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

class MaxscriptFileManager
{
    public static string CURRENT_VERSION = "1-3-1";

    public class MaxFolders
    {
        public string location { set; get; }
        public string project_path { set; get; }
        public string name { set; get; }
        public string version { set; get; }
    }

    public static void UpdateEntireDirectory(string mainFolder) {
        var folders = ListFolders(mainFolder);
        foreach (var m in folders)
        {
            Console.WriteLine(m.location);
            Console.WriteLine(m.project_path);
            Console.WriteLine(m.name);
            Console.WriteLine(m.version);
            UpdateSingleFolder(m);
        };
    }

    public static void UpdateOneFolder(string oneFolder)
    {
        MaxFolders mf = new MaxFolders();
        mf.location = oneFolder;
        mf.name = oneFolder.Substring(oneFolder.LastIndexOf(@"\") + 1).Replace(" ", "_");
        mf.project_path = GetProjectPath(oneFolder);
        mf.version = SearchVersion(oneFolder);
        Console.WriteLine(mf.location);
        Console.WriteLine(mf.project_path);
        Console.WriteLine(mf.name);
        Console.WriteLine(mf.version);

        UpdateSingleFolder(mf);
    }

    public static void UpdateSingleFolder(MaxFolders mf)
    {
        // check if current version matches the version in the folder
        if (mf.version == CURRENT_VERSION)
        {
            Console.WriteLine("Current Version is a Match, no need to update Max Script");
        } else {
            // if not current version, delete old versions
            string[] copyTypes = new string[1];
            copyTypes[0] = "ms";
            DirectoryInfo source = new DirectoryInfo(mf.location);
            List<string> files = ListFiles(source, copyTypes);
            foreach (string f in files)
            {
                File.Delete(f);
            };

            // if not current version, add new version into folder
            string path = mf.location + @"\" + "_" + mf.name + CURRENT_VERSION + ".ms";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    MaxScriptWrite(sw, mf.name, mf.project_path);
                }
            }
        };
    }

    public static List<MaxFolders> ListFolders (string mainFolder) {
        string[] subFolders = Directory.GetDirectories(mainFolder);
        List<MaxFolders> folders = new List<MaxFolders>();
        foreach (string sub in subFolders)
        {
            MaxFolders mf = new MaxFolders();
            mf.location = sub;
            mf.name = sub.Substring(sub.LastIndexOf(@"\") + 1).Replace(" ","_");
            mf.project_path = GetProjectPath(sub);
            mf.version = SearchVersion(sub);
            folders.Add(mf);
        };

        return folders;
    }

    public static string SearchVersion (string path)
    {
        // list each file in each folder
        string[] copyTypes = new string[1];
        copyTypes[0] = "ms";
        DirectoryInfo source = new DirectoryInfo(path);
        List<string> files = ListFiles(source, copyTypes);
        string scriptVersion = "";
        foreach (string f in files)
        {
            string scriptFileName = f.Substring(f.LastIndexOf(@"\") + 1);
            scriptVersion = scriptFileName.Substring(scriptFileName.LastIndexOf("v") + 1).Replace(".ms","");
            Console.WriteLine(f);
        }

        return scriptVersion;
    }

    public static string GetProjectPath(string path)
    {
        string project_path = path + @"\" + "新しいフォルダ";
        // get sub folders
        string[] subFolders = Directory.GetDirectories(path);
        if (subFolders.Count() == 0)
        {
            // there are no subfolders, use the path
            return path;
        } else if (subFolders.Count() == 1) {
            // theres only one folder so might as well use it
            return subFolders[0];
        } else {
            // theres more than one file! just use the default path
            for (int i=0; i < subFolders.Count(); i++)
            {
                return project_path;
            };
        }
        return project_path;
    }

    public static List<string> ListFiles(DirectoryInfo source, string[] copyTypes)
    {
        List<string> requestedFiles = new List<string>();

        // Get each file into the new directory.
        foreach (FileInfo fi in source.GetFiles())
        {
            string ext = "";
            try
            {
                ext = fi.Extension.Remove(0, 1);
            }
            catch { };
            if (copyTypes.Contains(ext)) requestedFiles.Add(fi.FullName);
        }

        // Copy each subdirectory using recursion.
        foreach (DirectoryInfo sub1 in source.GetDirectories())
        {
            List<string> moreFiles = ListFiles(sub1, copyTypes);
            foreach (string f in moreFiles) requestedFiles.Add(f);
        }

        return requestedFiles;
    }

    public static void MaxScriptWrite(StreamWriter sw, string projectName, string projectPath)
    {
        sw.WriteLine("-- vars (RECOMMENDED)");
        sw.WriteLine("-- プロジェクトの名前は何でもいいです");
        sw.WriteLine("-- プロジェクトのパス");
        sw.WriteLine("-- MAXファイルの名前（このファイルをインポートします）");
        sw.WriteLine(@"projectName = """ + projectName + @"""");
        sw.WriteLine(@"projectDir = @""" + projectPath + @"""");
        sw.WriteLine(@"maxFN = """"");
        string main_code = @"
-- セッチング
doAutoRenameDups = true
logLocation = @""\\IMPORT-3\share1\importLog.txt""

-- set projectName if not set manually
if projectName == """" then (
	print sysInfo.currentdir
	print ""Project Name not Specified""
	cd = filterString sysInfo.currentdir @""\""
	projectName = cd[cd.count]
	print projectName
)

-- set projectDir if not set manually
if projectDir == """" then (
	print ""Project Directory not Specified""
	childFolders = getDirectories(sysInfo.currentdir + @""\*"")
	if childFolders.count == 0 then (
		projectDir = sysInfo.currentdir
	) else if childFolders.count > 1 then (
		hasNewFolder = findItem childFolders (sysInfo.currentdir + ""新しいフォルダー"")
		if hasNewFolder == true then (
			projectDir = sysInfo.currentdir + ""新しいフォルダー""
		) else (
			projectDir = childFolders[1]
		)
	) else (
		projectDir = childFolders[1]
	)
	print projectDir
)

-- set maxFN if not set manually
maxFileDiffLocation = """"
if maxFN == """" then (
	print ""Max File Name not Specified""
	maxFiles = #()
	join maxFiles (getFiles(projectDir + @""\*.max""))
	print maxFiles.count
	if maxFiles.count == 0 then (
		print ""INITIAL MAX FILE COUNT IS ZERO SO CHECK PARENT""
		projectParentDir = pathConfig.removePathLeaf projectDir
		print projectParentDir
		join maxFiles (getFiles(projectParentDir + @""\*.max""))
		maxFileDiffLocation = projectParentDir
		print maxFiles.count
	)
	if maxFiles.count > 0 then(
		firstMaxFilePath = maxFiles[1]
		firstMaxFilePath_array = filterString firstMaxFilePath @""\""
		maxFN = firstMaxFilePath_array[firstMaxFilePath_array.count]
		print maxFN
	) else (
		maxFN = """"
	)
)

-- static vars
scriptTitle = ""意匠計画MAXScript""
introMsg = ""["" + (substituteString projectDir @""\\IMPORT-3\share1"" """") + ""] からスクリプトがダウンロードされ実行されます。よろしいですか？""
finishMsgFail = ""インポートに失敗しました。""
finishMsgSuccess = ""インポートに成功しました。""
cancelMsg = ""スクリプトは停止されました。""
invalidPathMsg = ""ERROR: 先MAXファイルを保存して下さい""
noMaxFileMsg = ""ERROR: インポートフォだーはMAXファイル無しです""

-- project vars
--localPath = @""C:\Users\"" + sysInfo.username + @""\Documents\3dsMax\import""
localPath = maxFilePath + @""\import""
imgFileNames = #()
imgLocalPaths = #()
missingPaths = #()

-- check if the local path exists
makeDir localPath

-- ask if user would like to continue
prompt = true
if maxFilePath == """" or maxFN == """" then (
	prompt = false
) else (
	prompt = queryBox introMsg title: scriptTitle
)

if prompt == true then (
	-- load remote project
	if maxFileDiffLocation == """" then (
		mergeDir = projectDir + @""\"" + maxFN
	) else (
		mergeDir = maxFileDiffLocation + @""\"" + maxFN
	)
	print mergeDir
	importTest = false
	if doAutoRenameDups == true then (
		importTest = mergemaxfile mergeDir #select #noRedraw #autoRenameDups
	) else (
		importTest = mergemaxfile mergeDir #select #noRedraw 
	)

	-- if able to import then
	if importTest == true then (
		
		-- copy mapping files locally to this folder
		localDir = localPath + @""\"" + projectName
		makeDir localDir
		
		-- get each mapping file off network directory
		wildcardDir = projectDir + @""\*""
		networkDirs = getDirectories(wildcardDir)
		for d in networkDirs do (
			join networkDirs (GetDirectories (d as string + ""*""))
		)
		imgFiles = #()
		join imgFiles (getFiles(projectDir + @""\*.png""))
		join imgFiles (getFiles(projectDir + @""\*.jpg""))
		join imgFiles (getFiles(projectDir + @""\*.gif""))
		join imgFiles (getFiles(projectDir + @""\*.bmp""))
		join imgFiles (getFiles(projectDir + @""\*.tif""))
		for f in networkDirs do (
			join imgFiles (getFiles(f + @""*.png""))
			join imgFiles (getFiles(f + @""*.jpg""))
			join imgFiles (getFiles(f + @""*.gif""))
			join imgFiles (getFiles(f + @""*.bmp""))
			join imgFiles (getFiles(f + @""*.tif""))
		)
		if imgFiles.count == 0 then (
			projectParentDir = pathConfig.removePathLeaf projectDir
			join imgFiles (getFiles(projectParentDir + @""\*.png""))
			join imgFiles (getFiles(projectParentDir + @""\*.jpg""))
			join imgFiles (getFiles(projectParentDir + @""\*.gif""))
			join imgFiles (getFiles(projectParentDir + @""\*.bmp""))
			join imgFiles (getFiles(projectParentDir + @""\*.tif""))
		)
		
		-- copy each file
		for m in imgFiles do(
			paths = filterString m ""\\""
			imgName = """"
			for p in paths do (
				imgName = p as string
			)
			append imgFileNames imgName
			imgLocalPath = @""import\"" + projectName + @""\"" + imgName
			append imgLocalPaths imgLocalPath
			new_path = localPath + @""\"" + projectName + @""\"" + imgName
			print ""copying file...""
			print m
			print new_path
			copyFile m new_path
		)
		
		-- do something for each mapping reference
		refFiles = #()
		for bmt in getClassInstances bitmaptex do
		(
			-- get reference info
			-- 3DSMaxのMappingパスの情報をくれます
			fpath = bmt.filename
			print fpath
			if fpath == undefined then (
				print ""this path is not defined""
			) else (
				fname = """"
				paths = filterString fpath ""\\""
				for p in paths do (
					fname = p as string
				)
				print fname
				append refFiles fname
				
				-- check if reference is local or on network
				-- このMappingパスはどんなパスですか？
				lstatus = ""LOCAL PATH""
				if fpath[1] == @""/"" then (
					lstatus = ""NETWORK PATH""
				) else if (substring fpath 1 6) == ""import"" then (
					lstatus = ""IMPORT PATH""
				)
				print lstatus
				
				-- check if reference is valid
				-- このMappingパスがあるですか？
				fpath_exists = doesFileExist fpath
				fstatus = ""FOUND""
				if fpath_exists == false then (
					fpath_exists = doesFileExist (maxFilePath + fpath)
					if fpath_exists == false then (
						fstatus = ""MISSING""
					)
				)
				print fstatus
				
				-- check if reference was one of the files that we copied earlier
				refStatus = ""EXCLUDED""
				fref = findItem imgFileNames fname
				if fref > 0 then (				
					refStatus = ""INCLUDED""
					-- if reference is like import/ then we dont need to reset the path
					if lstatus == ""IMPORT PATH"" then (
						refStatus = ""PRIOR INCLUDED""
					) else (
						bmt.filename = imgLocalPaths[fref]
					)					
				) else (
				
					if fstatus == ""MISSING"" then (
						-- this means the file mapping path is missing
						append missingPaths bmt.filename
					)
				)
				print refStatus
			)
		)
		finishMsgSuccess += ""\n "" + (refFiles.count as string) + "" マッピングファイルがあります。""
		finishMsgSuccess += ""\n "" + (missingPaths.count as string) + "" マッピングファイルのパスはダメです。\n""
		
		print ""--- CHECK COPIED FILE WAS REFERENCED, DELETE IF NOT ---""
		for img in imgFileNames do(
			print img
			copiedRef = findItem refFiles img
			if copiedRef > 0 then (
				print ""this file was used""
			) else (
				--deleteFile copiedRef
				delPath = localPath + @""\"" + projectName + @""\"" + img
				deleteFile delPath
				print ""this file wasnt used -> delete""
			)
		)
			
		-- log list of missing file paths
		if missingPaths.count > 0 then (
			finishMsgSuccess += ""\n以下のパスはダメです：""
			for m in missingPaths do(
				finishMsgSuccess += ""\n "" + m
			)
		) else (
			finishMsgSuccess += ""\n 全部のMappingPathsは大丈夫です！""
		)
		messageBox finishMsgSuccess
		print finishMsgSuccess
		
		-- log results to file		
		f = (openFile logLocation mode:""a"")
		format ""-- -------------------\n"" to:f
        format ""%\n"" localTime to:f
		format ""%\n"" sysInfo.username to:f
		format ""%\n"" projectName to:f
		format ""%\n"" ""Success"" to:f
		format ""% mapping files\n"" (refFiles.count as string) to:f
		format ""% mapping errors\n"" (missingPaths.count as string) to:f
		format ""%\n"" projectDir to:f
		format ""%\n"" maxFilePath to:f
		close f
	) else (
		messageBox finishMsgFail
		print finishMsgFail
		
		-- log results to file		
		f = (openFile logLocation mode:""a"")
		format ""-- -------------------\n"" to:f
        format ""%\n"" localTime to:f
		format ""%\n"" sysInfo.username to:f
		format ""%\n"" projectName to:f
		format ""%\n"" ""Failed"" to:f
		format ""% mapping files\n"" (0 as string) to:f
		format ""% mapping errors\n"" (0 as string) to:f
		format ""%\n"" projectDir to:f
		format ""%\n"" maxFilePath to:f
		close f
	)

) else (
	-- error list
	-- エラー
	if maxFilePath == """" then (
		-- filepath is invalid
		messageBox invalidPathMsg
		print invalidPathMsg
	) else if maxFN == """" then (
		-- noMaxFile
		messageBox noMaxFileMsg
		print noMaxFileMsg		
	) else (
		-- user cancelled
		messageBox cancelMsg
		print cancelMsg
	)
)
";
        sw.WriteLine(main_code);

    }
}
