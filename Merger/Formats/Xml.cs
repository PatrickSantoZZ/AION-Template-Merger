using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Iswenzz.AION.Merger.Format
{
    public static class Xml
    {
        /// <summary>
        /// Append a XML file, if the file doesn't exist it will create a new one.
        /// </summary>
        /// <param name="fs">filepath</param>
        /// <param name="root">root element name</param>
        /// <returns>return a XDocument instance</returns>
        public static XDocument AppendXml(string fs, string root)
        {
            if (!File.Exists(fs))
            {
                return new XDocument
                (
                    new XDeclaration("1.0", "ISO-8859-1", null),
                    new XComment("Generated on " + DateTime.Now),
                    new XElement(root)
                );
            }
            return XDocument.Load(fs);
        }

        /// <summary>
        /// Merge all missing line from doc1 to doc2
        /// </summary>
        /// <remarks>
        ///  -1 Root with everything inside
        ///  -Each element in root get a different ID
        ///  -Each element have that id defined in the attribute collection 
        /// </remarks>
        /// <param name="path_1">doc1</param>
        /// <param name="path_2">doc2</param>
        /// <param name="element_name">Element name i.e: quest</param>
        /// <param name="attribute_name">Attribute name i.e: id</param>
        public static void MergeXmlServer(string path_1, string path_2, string element_name, string attribute_name)
        {
            XDocument doc = XDocument.Load(path_1);
            XDocument doc_final = XDocument.Load(path_2);
            doc_final.Declaration = new XDeclaration("1.0", "ISO-8859-1", null);
            List<string> existing_id = new List<string>();

            foreach (XElement element in doc_final.Descendants(element_name))
                existing_id.Add(element.Attribute(attribute_name).Value);

            foreach (XElement element in doc.Descendants(element_name))
            {
                if (existing_id.Contains(element.Attribute(attribute_name).Value))
                    continue;

                Console.WriteLine(element.Attribute(attribute_name).Value);
                doc_final.Root.Add(element);
            }
            doc_final.Save(path_2);
        }

        /// <summary>
        /// Merge all missing line from doc1 to doc2
        /// </summary>
        /// <remarks>
        /// -1 Root with everything inside
        /// -Each element in root get a different ID
        /// -Each element have that id defined in some node (innerText) 
        /// </remarks>
        /// <param name="path_1">doc1</param>
        /// <param name="path_2">doc2</param>
        /// <param name="element_name">Element name i.e: string</param>
        /// <param name="childnode_name">Childnode name i.e: id</param>
        public static void MergeXmlClient(string path_1, string path_2, string element_name, string childnode_name)
        {
            XDocument doc = XDocument.Load(path_1);
            XDocument doc_final = XDocument.Load(path_2);
            doc_final.Declaration = new XDeclaration("1.0", "ISO-8859-1", null);
            List<string> existing_id = new List<string>();

            foreach (XElement element in doc_final.Descendants(element_name))
                existing_id.Add(element.Element(childnode_name).Value);

            foreach (XElement element in doc.Descendants(element_name))
            {
                if (existing_id.Contains(element.Element(childnode_name).Value))
                    continue;

                Console.WriteLine(element.Element(childnode_name).Value);
                doc_final.Root.Add(element);
            }
            doc_final.Save(path_2);
        }

        /// <summary>
        /// Add a new element from doc1 to doc2
        /// Client + Server
        /// </summary>
        /// <param name="path_1">doc1</param>
        /// <param name="path_2">doc2</param>
        public static void AddElementDoc1toDoc2(string path_1, string path_2)
        {
            Dictionary<string, string> array = new Dictionary<string, string>();
            XDocument doc_39 = XDocument.Load(path_1);
            XDocument doc_56 = XDocument.Load(path_2);
            int index = 0;

            foreach (XElement element in doc_39.Descendants("npc_template"))
            {
                if (element.Element("npc_id") != null && element.Element("name_desc") != null)
                    array.Add(element.Element("npc_id").Value, element.Element("name_desc").Value);
            }

            foreach (XElement element in doc_56.Descendants("npc_template"))
            {
                if (element.Element("npc_id") != null && element.Element("name_desc") == null)
                {
                    if (!array.ContainsKey(element.Element("npc_id").Value)) continue;
                    if (string.IsNullOrEmpty(array[element.Element("npc_id").Value])) continue;

                    element.SetElementValue("name_desc", array[element.Element("npc_id").Value]);
                    Console.WriteLine(element.Element("npc_id").Value + " " + array[element.Element("npc_id").Value]);
                    index++;
                }
            }

            Console.WriteLine("\n" + index);
            doc_56.Declaration = new XDeclaration("1.0", "ISO-8859-1", null);
            doc_56.Save("./56/npc_templates" + "_new.xml");
        }

        /// <summary>
        /// Replace Element Value of doc2 to doc1
        /// Client + Server
        /// </summary>
        /// <param name="path_2">doc2</param>
        /// <param name="path_1">doc1</param> 
        public static void ReplaceElementValueDoc2toDoc1(string path_2, string path_1)
        {
            Dictionary<string, string> array = new Dictionary<string, string>();
            XDocument doc_39 = XDocument.Load(path_1);
            XDocument doc_56 = XDocument.Load(path_2);
            int index = 0;

            foreach (XElement element in doc_39.Descendants("npc_client"))
            {
                if (element.Element("id") != null && element.Element("hpgauge_level") != null)
                    array.Add(element.Element("id").Value, element.Element("hpgauge_level").Value);
            }

            foreach (XElement element in doc_56.Descendants("npc_client") ?? Enumerable.Empty<XElement>())
            {
                if (element.Element("id") != null && element.Element("hpgauge_level") != null)
                {
                    if (!array.ContainsKey(element.Element("id").Value)) continue;
                    if (element.Element("hpgauge_level").Value == array[element.Element("id").Value]) continue;

                    element.Element("hpgauge_level").SetValue(array[element.Element("id").Value]);
                    Console.WriteLine(element.Element("id").Value + " " + array[element.Element("id").Value]);
                    index++;
                }
            }

            Console.WriteLine("\n" + index);
            doc_56.Declaration = new XDeclaration("1.0", "ISO-8859-1", null);
            doc_56.Save("./56/client_npcs_monster" + "_new.xml");
        }

        /// <summary>
        /// Add Attribute of doc1 to doc2
        /// Client + Server
        /// </summary>
        /// <param name="path_1">doc1</param>
        /// <param name="path_2">doc2</param>
        public static void AddAttributeDoc1toDoc2(string path_1, string path_2)
        {
            Dictionary<string, string> array = new Dictionary<string, string>();
            XDocument doc_39 = XDocument.Load(path_1);
            XDocument doc_56 = XDocument.Load(path_2);
            int index = 0;

            foreach (XElement element in doc_39.Descendants("npc_template"))
            {
                if (element.Attribute("npc_id") != null && element.Attribute("name_desc") != null)
                    array.Add(element.Attribute("npc_id").Value, element.Attribute("name_desc").Value);
            }

            foreach (XElement element in doc_56.Descendants("npc_template"))
            {
                if (element.Attribute("npc_id") != null && element.Attribute("name_desc") == null)
                {
                    if (!array.ContainsKey(element.Attribute("npc_id").Value)) continue;
                    if (string.IsNullOrEmpty(array[element.Attribute("npc_id").Value])) continue;

                    element.SetAttributeValue("name_desc", array[element.Attribute("npc_id").Value]);
                    Console.WriteLine(element.Attribute("npc_id").Value + " " + array[element.Attribute("npc_id").Value]);
                    index++;
                }
            }

            Console.WriteLine("\n" + index);
            doc_56.Declaration = new XDeclaration("1.0", "ISO-8859-1", null);
            doc_56.Save("./56/npc_templates" + "_new.xml");
        }

        /// <summary>
        /// Replace Attribute Value of doc2 to doc1
        /// Client + Server
        /// </summary>
        /// <param name="path_2">doc2</param>
        /// <param name="path_1">doc1</param>
        public static void ReplaceAttributeValueDoc2toDoc1(string path_2, string path_1)
        {
            Dictionary<string, string> array = new Dictionary<string, string>();
            XDocument doc_39 = XDocument.Load(path_1);
            XDocument doc_56 = XDocument.Load(path_2);
            int index = 0;

            foreach (XElement element in doc_39.Descendants("npc_template"))
            {
                if (element.Attribute("npc_id") != null && element.Attribute("ai") != null)
                    array.Add(element.Attribute("npc_id").Value, element.Attribute("ai").Value);
            }

            foreach (XElement element in doc_56.Descendants("npc_template"))
            {
                if (element.Attribute("npc_id") != null && element.Attribute("ai") != null)
                {
                    if (!array.ContainsKey(element.Attribute("npc_id").Value)) continue;
                    if (string.IsNullOrEmpty(array[element.Attribute("npc_id").Value])) continue;
                    if (element.Attribute("ai").Value == array[element.Attribute("npc_id").Value]) continue;

                    element.Attribute("ai").SetValue(array[element.Attribute("npc_id").Value]);
                    Console.WriteLine(element.Attribute("npc_id").Value + " " + array[element.Attribute("npc_id").Value]);
                    index++;
                }
            }

            Console.WriteLine("\n" + index);
            doc_56.Declaration = new XDeclaration("1.0", "ISO-8859-1", null);
            doc_56.Save("./56/npc_templates" + "_new.xml");
        }
    }
}
