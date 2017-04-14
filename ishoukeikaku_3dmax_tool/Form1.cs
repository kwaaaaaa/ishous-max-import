using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//using Autodesk.Max;

namespace ishoukeikaku_3dmax_tool
{
    public partial class Form1 : Form
    {
        //static IGlobal max_global = GlobalInterface.Instance;
        //static IInterface13 Interface = max_global.COREInterface13;

        public Form1()
        {
            InitializeComponent();
        }

        public class MaxImports
        {
            public string time_stamp { set; get; }
            public string user { set; get; }
            public int result { set; get; }
            public int mapping_files { set; get; }
            public int mapping_errors { set; get; }
            public string obj_full_path { set; get; }
            public string obj_category { set; get; }
            public string obj_subcatgeory { set; get; }
            public string obj_name { set; get; }
            public string karte_full_path { set; get; }
            public int karte_id { set; get; }
            public string karte_name { set; get; }
        }

        public static string IMPORTLOG_LOCATION = @"\\IMPORT-3\share1\importLog.txt";

        public static string[] FOLDERS_TO_UPDATE = new string[] {
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\カーテン ブラインド_curtain",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\クッション",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\テーブル_table",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\ベッド_bed",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\ラグ",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\収納",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\子供部屋　baby room",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\暖炉_fireplace",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\椅子_chair",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\椅子_chair_High",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\椅子_chair_Sofa",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\水周り_Utility Rooms",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\窓・ドア・建具_windows door\ドア_doors",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\窓・ドア・建具_windows door\ドアノブ",
        @"\\IMPORT-3\share1\01_Object\家具_Furniture\窓・ドア・建具_windows door\窓",
        @"\\IMPORT-3\share1\01_Object\アミューズメント",
        @"\\IMPORT-3\share1\01_Object\エクステリア_exterior\その他",
        @"\\IMPORT-3\share1\01_Object\エクステリア_exterior\メールボックス",
        @"\\IMPORT-3\share1\01_Object\エクステリア_exterior\住宅関連_house stuff",
        @"\\IMPORT-3\share1\01_Object\エクステリア_exterior\岩・石・飛び石_rock stone stepping stone",
        @"\\IMPORT-3\share1\01_Object\エクステリア_exterior\設備関係＿屋外_outdoor facilities",
        @"\\IMPORT-3\share1\01_Object\エクステリア_exterior\道路_外溝",
        @"\\IMPORT-3\share1\01_Object\キッチン_Kicthen\コーヒーメーカー",
        @"\\IMPORT-3\share1\01_Object\キッチン_Kicthen\システムキッチン",
        @"\\IMPORT-3\share1\01_Object\キッチン_Kicthen\その他",
        @"\\IMPORT-3\share1\01_Object\キッチン_Kicthen\ボトル_Bottle",
        @"\\IMPORT-3\share1\01_Object\キッチン_Kicthen\食べ物、飲み物_food and drinks",
        @"\\IMPORT-3\share1\01_Object\キッチン_Kicthen\食器、調味料_tableware",
        @"\\IMPORT-3\share1\01_Object\ジム・フィットネスマシン_fitness equipments",
        @"\\IMPORT-3\share1\01_Object\スポーツ_趣味_sports",
        @"\\IMPORT-3\share1\01_Object\その他_other",
        @"\\IMPORT-3\share1\01_Object\モールディング・アイアンワーク_Decorative",
        @"\\IMPORT-3\share1\01_Object\乗り物_vehicle\バイク_bike",
        @"\\IMPORT-3\share1\01_Object\乗り物_vehicle\働く車",
        @"\\IMPORT-3\share1\01_Object\乗り物_vehicle\自転車_bicycle",
        @"\\IMPORT-3\share1\01_Object\乗り物_vehicle\車_cars",
        @"\\IMPORT-3\share1\01_Object\家電・機械_\PC",
        @"\\IMPORT-3\share1\01_Object\家電・機械_\エアコン_ aircon",
        @"\\IMPORT-3\share1\01_Object\家電・機械_\シーリングファン",
        @"\\IMPORT-3\share1\01_Object\家電・機械_\その他",
        @"\\IMPORT-3\share1\01_Object\家電・機械_\テレビ_TV",
        @"\\IMPORT-3\share1\01_Object\家電・機械_\冷蔵庫",
        @"\\IMPORT-3\share1\01_Object\家電・機械_\電話_tel",
        @"\\IMPORT-3\share1\01_Object\小物　雑貨_accessories\その他",
        @"\\IMPORT-3\share1\01_Object\小物　雑貨_accessories\ディスプレイ",
        @"\\IMPORT-3\share1\01_Object\小物　雑貨_accessories\トランク",
        @"\\IMPORT-3\share1\01_Object\小物　雑貨_accessories\ﾌﾞﾗｲﾀﾞﾙ小物_bridal",
        @"\\IMPORT-3\share1\01_Object\小物　雑貨_accessories\ほ_本_books",
        @"\\IMPORT-3\share1\01_Object\小物　雑貨_accessories\花器.つぼ_vase and plant pot",
        @"\\IMPORT-3\share1\01_Object\小物　雑貨_accessories\額縁、フォトフレーム_photo frame",
        @"\\IMPORT-3\share1\01_Object\店舗関連_shop related\カート",
        @"\\IMPORT-3\share1\01_Object\店舗関連_shop related\ゴンドラ",
        @"\\IMPORT-3\share1\01_Object\店舗関連_shop related\スーパー冷ケース_supermarket cold storage",
        @"\\IMPORT-3\share1\01_Object\店舗関連_shop related\その他",
        @"\\IMPORT-3\share1\01_Object\店舗関連_shop related\マネキン　トルソー ハンガー",
        @"\\IMPORT-3\share1\01_Object\店舗関連_shop related\化粧品",
        @"\\IMPORT-3\share1\01_Object\店舗関連_shop related\厨房機器_kitchen appliances",
        @"\\IMPORT-3\share1\01_Object\店舗関連_shop related\衣料品",
        @"\\IMPORT-3\share1\01_Object\店舗関連_shop related\設備",
        @"\\IMPORT-3\share1\01_Object\植栽_Plants\3D",
        @"\\IMPORT-3\share1\01_Object\楽器",
        @"\\IMPORT-3\share1\01_Object\照明_lighting",
        @"\\IMPORT-3\share1\01_Object\隣地_house proxies"};

        private void btnTest_Click(object sender, EventArgs e)
        {
            // 例えば ) Import Max File for ALL folders 
            foreach (string f in FOLDERS_TO_UPDATE)
            {
                MaxscriptFileManager.UpdateEntireDirectory(f);
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 例えば ) Import Max File for NEW KARTE
            string new_karte_folder = textBox1.Text;
            MaxscriptFileManager.UpdateOneFolder(new_karte_folder);
        }

		private void Form1_Load(object sender, EventArgs e)
		{

		}

        private void btnUpdateDB_Click(object sender, EventArgs e)
        {
            var import_list = new List<MaxImports>();

            // open and read file
            var lines = File.ReadLines(IMPORTLOG_LOCATION);
            int n = 0;
            while (n < lines.Count())
            {
                // create class per line
                // first line is divider
                string time_stamp = lines.ElementAt(n + 1);
                string computer_name = lines.ElementAt(n + 2);
                string obj_name = lines.ElementAt(n + 3);
                string result_string = lines.ElementAt(n + 4);
                string map_count_string = lines.ElementAt(n + 5);
                string map_errors_string = lines.ElementAt(n + 6);
                string obj_loc_string = lines.ElementAt(n + 7);
                string karte_loc_string = lines.ElementAt(n + 8);

                var separators = new char[] {
                  Path.DirectorySeparatorChar,  
                  Path.AltDirectorySeparatorChar  
                };

                string[] obj_loc_split = obj_loc_string
                    .Trim()
                    .Replace(@"\\IMPORT-3\share1\01_Object\", "")
                    .Split(separators);
                string category = obj_loc_split[0];
                string subcat = obj_loc_split[1];

                string[] karte_loc_split = karte_loc_string
                    .Trim()
                    .Split(separators);
                int d = karte_loc_split.Length - 1;
                string karte_folder = "";
                while (d > 0 && karte_folder == "") {
                    string path = karte_loc_split[d];
                    if (path.Length > 5) {
                        bool first_lines_are_int = StringPlay.FirstCharsAreInt(path, 5);
                        if (first_lines_are_int) {
                            karte_folder = path;
                        };
                    };
                    d--;
                };
                string karte_name = karte_folder;
                string karte_id_string = "0";
                if (karte_name.Length > 5) {
                    karte_id_string = (StringPlay.FirstCharsAreInt(karte_folder, 6)) ? karte_folder.Substring(0, 6) : karte_folder.Substring(0, 5);
                };
                int karte_id = Convert.ToInt32(karte_id_string);

                MaxImports mi = new MaxImports {
                    time_stamp = time_stamp,
                    user = computer_name,
                    result = (result_string == "Success") ? 1 : 0,
                    mapping_files = Convert.ToInt32(map_count_string.Replace(" mapping files","")),
                    mapping_errors = Convert.ToInt32(map_errors_string.Replace(" mapping errors", "")),
                    obj_full_path = obj_loc_string,
                    obj_category = category,
                    obj_subcatgeory = subcat,
                    obj_name = obj_name,
                    karte_full_path = karte_loc_string,
                    karte_id = karte_id,
                    karte_name = karte_name
                };
                import_list.Add(mi);

                n += 9;
            };

            // create sql string for data
            string sb = "Replace into orbit.maximports VALUES \n";
            int list_count = 0;
            foreach (var mi in import_list) {
                string max_sql = "(" + StringPlay.SQuoteWrap(mi.time_stamp) + "," +
                    StringPlay.SQLSafeString(mi.user) + "," +
                    StringPlay.SQLSafeString(mi.obj_name) + "," +
                    mi.result.ToString() + "," + 
                    mi.mapping_files.ToString() + "," +
                    mi.mapping_errors.ToString() + "," +
                    StringPlay.SQLSafeString(mi.obj_full_path) + "," +
                    StringPlay.SQLSafeString(mi.obj_category) + "," +
                    StringPlay.SQLSafeString(mi.obj_subcatgeory) + "," +
                    StringPlay.SQLSafeString(mi.karte_full_path) + "," +
                    mi.karte_id.ToString() + "," +
                    StringPlay.SQLSafeString(mi.karte_name) + ")\n";
                sb += (list_count > 0) ? "," : "";
                sb += max_sql;
                list_count++;
            };
            //Console.WriteLine(sb.ToString());
            
            // upload data to db
            MysqlDB db = new MysqlDB();
            string connection_param = Properties.Settings.Default.orbit_mysql_conn;
            db.ConnectToDB(connection_param);
            db.Operation(sb);
            db.CloseConnection();
        }
	}
}
