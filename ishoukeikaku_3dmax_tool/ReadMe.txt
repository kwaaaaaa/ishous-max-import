****************************************************
Autodesk Docs
http://docs.autodesk.com/3DSMAX/16/ENU/3ds-Max-SDK-Programmer-Guide/index.html?url=files/GUID-4B706997-C946-4C1D-8A69-0A0AC14A1D9B.htm,topicNumber=d30e50630
****************************************************



****************************************************
3DSMax SDK uses the Autodesk.Max.dll file
dll was found on my computer here:
C:\Program Files\Autodesk\3ds Max 2016
Add this to references and then call it from using Autodesk.Max;

Interact with 3dsmax classes (also called interface):
static IGlobal i_global = GlobalInterface.Instance;
static IInterface13 Interface = i_global.COREInterface13;
****************************************************



****************************************************
Max Import MySQL Table Struct
----------------------------------------------------

CREATE TABLE orbit.maximports (
`time_stamp` datetime NOT NULL,
`user` varchar(255) NOT NULL default '',
`obj_name` varchar(255) NOT NULL default '',
`result` bit NOT NULL DEFAULT 0,
`mapping_files` smallint unsigned NOT NULL DEFAULT 0,
`mapping_errors` smallint unsigned NOT NULL DEFAULT 0,
`obj_full_path` varchar(255) NULL,
`obj_category` varchar(255) NULL,
`obj_subcatgeory` varchar(255) NULL,
`karte_full_path` varchar(255) NULL,
`karte_id` bigint NULL,
`karte_name` varchar(255) NULL,
PRIMARY KEY (time_stamp,user,obj_name),
KEY (user),
KEY (obj_name),
KEY (result),
KEY (mapping_files),
KEY (mapping_errors),
KEY (karte_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
