using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

using Iswenzz.AION.utility;

namespace Iswenzz.AION.Merger
{
    public static class Program
    {
        public static string option;
        public static string path_1;
        public static string path_2;

        private static List<string> list_option = new List<string>
        {
            // Server
            "-server_npc_trade_list",
            "-server_npc_trade_in_list",
            "-server_npc_trade_purchase",
            "-server_npc_templates",
            "-server_goods_list",
            "-server_goods_in_list",
            "-server_goods_purchase_list",
            "-server_npc_skills",
            "-server_skill",
            "-server_weather",
            "-server_teleport_location",
            "-server_teleporter",
            "-server_hotspot",
            "-server_flypath",
            "-server_zones_quest",
            "-server_zones_weather",
            "-server_staticdoors",
            "-server_windstreams",
            "-server_quest",

            // Client
            "-client_npc_goods_list",
            "-client_npc_goods_in_list",
            "-client_npc_goods_purchase_list",
            "-client_abyss_icon",
            "-client_waypoint",
            "-client_source_sphere",
            "-client_skill",
            "-client_zonemap",
            "-client_instance_cooltime",
            "-client_instance_cooltime2",
            "-client_artifact",
            "-client_airline",
            "-client_airport",
            "-client_fly_path",
            "-client_quest",
            "-client_quest_data_driven",
            "-client_quest_monster",
            "-client_quest_script_monster"
        };

        private static void Main(string[] args)
        {
            if (args.Length == 0 || args.Length < 3)
            {
                Console.WriteLine
                (
                    "This Software will merge XML Files without duplicating elements,\n" +
                    "both xml need to have the same structure.\n" +
                    "i.e: path 1 = quest data 4.7, path 2 = quest data 5.8\n\n" +
                    "(!) Don't forget to remove the Read Only properties on each file.\n\n" +
                    "Usage:\n\n\t" +
                    "template_merger.exe [options] <path 1> <path 2>\n\n" +
                    "Options:\n"
                );
                foreach (string opt in list_option)
                    Console.WriteLine("\t" + opt);
                return;
            }
            option = args[0];
            path_1 = args[1];
            path_2 = args[2];

            Template.Init();
            Console.WriteLine("\nDone!");
        }
    }

    public static class Template
    {
        public static void Init()
        {
            switch (Program.option)
            {
                // Server
                case "-server_npc_trade_list": Xml.MergeXmlServer(Program.path_1, Program.path_2, "tradelist_template", "npc_id"); break;
                case "-server_npc_trade_in_list": Xml.MergeXmlServer(Program.path_1, Program.path_2, "trade_in_list_template", "npc_id"); break;
                case "-server_npc_trade_purchase": Xml.MergeXmlServer(Program.path_1, Program.path_2, "purchase_template", "npc_id"); break;
                case "-server_goods_list": Xml.MergeXmlServer(Program.path_1, Program.path_2, "list", "id"); break;
                case "-server_goods_in_list": Xml.MergeXmlServer(Program.path_1, Program.path_2, "in_list", "id"); break;
                case "-server_goods_purchase_list": Xml.MergeXmlServer(Program.path_1, Program.path_2, "purchase_list", "id"); break;
                case "-server_npc_templates": Xml.MergeXmlServer(Program.path_1, Program.path_2, "npc_template", "npc_id"); break;
                case "-server_npc_skills": Xml.MergeXmlServer(Program.path_1, Program.path_2, "npcskills", "npcid"); break;
                case "-server_skill": Xml.MergeXmlServer(Program.path_1, Program.path_2, "skill_template", "skill_id"); break;
                case "-server_weather": Xml.MergeXmlServer(Program.path_1, Program.path_2, "map", "id"); break;
                case "-server_teleport_location": Xml.MergeXmlServer(Program.path_1, Program.path_2, "teleloc_template", "loc_id"); break;
                case "-server_teleporter": Xml.MergeXmlServer(Program.path_1, Program.path_2, "teleporter_template", "npc_ids"); break;
                case "-server_hotspot": Xml.MergeXmlServer(Program.path_1, Program.path_2, "hotspot_template", "id"); break;
                case "-server_quest": Xml.MergeXmlServer(Program.path_1, Program.path_2, "quest", "id"); break;
                case "-server_windstreams": Xml.MergeXmlServer(Program.path_1, Program.path_2, "windstream", "mapid"); break;
                case "-server_staticdoors": Xml.MergeXmlServer(Program.path_1, Program.path_2, "world", "world"); break;
                case "-server_zones_weather": Xml.MergeXmlServer(Program.path_1, Program.path_2, "zone", "name"); break;
                case "-server_zones_quest": Xml.MergeXmlServer(Program.path_1, Program.path_2, "zone", "name"); break;
                case "-server_flypath": Xml.MergeXmlServer(Program.path_1, Program.path_2, "flypath_location", "id"); break;

                // Client
                case "-client_skill": Xml.MergeXmlClient(Program.path_1, Program.path_2, "skill_base_client", "id"); break;
                case "-client_source_sphere": Csv.MergeCSV(Program.path_1, Program.path_2); break;
                case "-client_waypoint": Csv.MergeCSV(Program.path_1, Program.path_2); break;
                case "-client_npc_goods_list": Xml.MergeXmlClient(Program.path_1, Program.path_2, "client_npc_goodslist", "id"); break;
                case "-client_npc_goods_in_list": Xml.MergeXmlClient(Program.path_1, Program.path_2, "client_npc_trade_in_list", "id"); break;
                case "-client_npc_goods_purchase_list": Xml.MergeXmlClient(Program.path_1, Program.path_2, "client_npc_purchase_list", "id"); break;
                case "-client_zonemap": Xml.MergeXmlClient(Program.path_1, Program.path_2, "zonemap", "id"); break;
                case "-client_abyss_icon": Xml.MergeXmlClient(Program.path_1, Program.path_2, "abyss_icon", "pos_x"); break;
                case "-client_instance_cooltime": Xml.MergeXmlClient(Program.path_1, Program.path_2, "client_instance_cooltime", "id"); break;
                case "-client_instance_cooltime2": Xml.MergeXmlClient(Program.path_1, Program.path_2, "client_instance_cooltime2", "id"); break;
                case "-client_quest": Xml.MergeXmlClient(Program.path_1, Program.path_2, "quest", "id"); break;
                case "-client_artifact": Xml.MergeXmlClient(Program.path_1, Program.path_2, "client_artifact", "id"); break;
                case "-client_fly_path": Xml.MergeXmlClient(Program.path_1, Program.path_2, "path_group", "group_id"); break;
                case "-client_airline": Xml.MergeXmlClient(Program.path_1, Program.path_2, "client_airline", "id"); break;
                case "-client_airport": Xml.MergeXmlClient(Program.path_1, Program.path_2, "client_airport", "id"); break;
                case "-client_quest_data_driven": Xml.MergeXmlClient(Program.path_1, Program.path_2, "quest_data_driven", "id"); break;
                case "-client_quest_monster": Csv.MergeCSV(Program.path_1, Program.path_2, "progress", 1); break;
                case "-client_quest_script_monster": Csv.MergeCSV(Program.path_1, Program.path_2, "progress", 1); break;
                default: break;
            }
        }
    }
}
