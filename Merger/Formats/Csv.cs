using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Iswenzz.AION.Merger.Format
{
    public static class Csv
    {
        /// <summary>
        /// Append a CSV file, if the file doesn't exist it will create a new one.
        /// </summary>
        /// <param name="fs">filepath</param>
        /// <returns>return a generic list of strings</returns>
        public static List<string> AppendCSV(string fs)
        {
            if (!File.Exists(fs))
                return new List<string>();
            else
                return File.ReadAllLines(fs).ToList();
        }

        /// <summary>
        /// Merge all missing line from doc1 to doc2
        /// </summary>
        /// <param name="path_1">doc1</param>
        /// <param name="path_2">doc2</param>
        public static void MergeCSV(string path_1, string path_2)
        {
            MergeCSV(path_1, path_2, "", 0);
        }

        /// <summary>
        /// Merge all missing line from doc1 to doc2 with some edits
        /// </summary>
        /// <param name="path_1">doc1</param>
        /// <param name="path_2">doc2</param>
        /// <param name="exception">exception name (default empty)</param>
        public static void MergeCSV(string path_1, string path_2, string exception)
        {
            MergeCSV(path_1, path_2, exception, 0);
        }

        /// <summary>
        /// Merge all missing line from doc1 to doc2 with some edits
        /// </summary>
        /// <param name="path_1">doc1</param>
        /// <param name="path_2">doc2</param>
        /// <param name="exception">exception name (default empty)</param>
        /// <param name="column">column (index base 0)</param>
        public static void MergeCSV(string path_1, string path_2, string exception, int column)
        {
            if (exception == "progress")
            {
                AddProgress(path_1, column);
                AddProgress(path_2, column);
            }

            StreamReader reader = new StreamReader(path_1);
            StreamReader reader_final = new StreamReader(path_2);
            List<string> existing_id = new List<string>();
            string reader_text = reader.ReadToEnd();
            string reader_final_text = reader_final.ReadToEnd();

            foreach (string line in reader_final_text.Split('\n'))
                existing_id.Add(line);

            reader_final.Close();
            reader_final.Dispose();

            foreach (string line in reader_text.Split('\n'))
            {
                if (existing_id.Contains(line))
                {
                    Console.WriteLine("Line already exist!");
                    continue;
                }

                Console.Write(line);
                File.AppendAllText(path_2, line);
            }

            reader.Close();
            reader.Dispose();
        }

        /// <summary>
        /// Remove all ids of a csv.
        /// </summary>
        /// <param name="path_csv">csv to edit</param>
        /// <param name="path_id_remove">txt file that contains each id to remove</param>
        public static void RemoveIDs(string path_csv, string path_id_remove)
        {
            // path_csv = @"D:\Users\Wenzz\source\repos\Aion EU\Aion SDK\bin\Debug\wip\_Sorted_Maps\56\waypoint_new.csv";
            // path_id_remove = @"D:\Users\Wenzz\source\repos\Aion EU\Aion SDK\bin\Debug\wip\_Sorted_Maps\47\id.txt";
            Console.WriteLine("This file MUST have its original name.");
            string[] csv_lines = File.ReadAllLines(path_csv);
            string csv_new_lines = "";
            List<string> id_remove = new List<string>();

            foreach (string id in File.ReadAllLines(path_id_remove)) id_remove.Add(id);
            foreach (string line in csv_lines)
            {
                switch (true)
                {
                    case true when Path.GetFileName(path_csv).Contains("source"):
                        if (Source_Check(line, id_remove)) { Console.WriteLine(line); continue; } break;
                    case true when Path.GetFileName(path_csv).Contains("waypoint"):
                        if (Waypoint_Check(line, id_remove)) { Console.WriteLine(line); continue; } break;
                    case true when Path.GetFileName(path_csv).Contains("quest"):
                        if (Quest_Check(line, id_remove)) { Console.WriteLine(line); continue; } break;
                }
                csv_new_lines += line + Environment.NewLine;
            }
            File.WriteAllText(path_csv.Replace(".csv", "_new.csv"), csv_new_lines);
        }

        private static bool Quest_Check(string line, List<string> id_remove)
        {
            foreach (string id in id_remove)
            {
                if (line.Contains(id.ToLower() + ","))
                    return true;
            }
            return false;
        }

        private static bool Source_Check(string line, List<string> id_remove)
        {
            foreach (string id in id_remove)
            {
                if (line.Contains("," + id.ToLower() + ","))
                    return true;
            }
            return false;
        }

        private static bool Waypoint_Check(string line, List<string> id_remove)
        {
            foreach (string id in id_remove)
            {
                if (line.Contains(id.ToLower() + "_"))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Add Progress method in CSV.
        /// </summary>
        /// <param name="path">csv path</param>
        /// <param name="progress_column">which column (index base 0)</param>
        public static void AddProgress(string path, int progress_column)
        {
            string csv = File.ReadAllText(path);
            bool skip_first = true;

            File.WriteAllText(path, "questId, questProgress, item, sourceType, sourceName, num, monsters_gathers_npcs_list" + Environment.NewLine);

            foreach (string line in csv.Split('\n'))
            {
                if (skip_first)
                {
                    skip_first = false;
                    continue;
                }

                string new_line = "";
                string[] column = line.Split(',');

                for (int i = 0; i < column.Length; i++)
                {
                    if (!string.IsNullOrEmpty(column[i]))
                    {
                        if (i == column.Length - 1)
                        {
                            new_line += column[i];
                            continue;
                        }

                        if (i == progress_column)
                        {
                            if (column[i][0] == 'P')
                            {
                                new_line += column[i] + ",";
                                continue;
                            }

                            new_line += "Progress(" + column[i] + "),";
                            continue;
                        }
                        new_line += column[i] + ",";
                    }
                    else if (i == column.Length - 1) continue;
                    else new_line += ",";
                }
                if (new_line.Length > 0)
                    File.AppendAllText(path, new_line.Remove(new_line.Length - 1) + Environment.NewLine);
            }
        }
    }
}
