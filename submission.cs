using System;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Program
    {
        // Local file paths for macOS
        public static string xmlURL = "/Users/kseniia.piskun/Desktop/Assignment4_XMLProject/Hotels.xml";
        public static string xmlErrorURL = "/Users/kseniia.piskun/Desktop/Assignment4_XMLProject/HotelsErrors.xml";
        public static string xsdURL = "/Users/kseniia.piskun/Desktop/Assignment4_XMLProject/Hotels.xsd";

        public static void Main(string[] args)
        {
            // Test Validation with correct XML
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine("Validation Result (Hotels.xml): " + result);

            // Test Validation with erroneous XML
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine("Validation Result (HotelsErrors.xml): " + result);

            // Test XML to JSON conversion
            result = Xml2Json(xmlURL);
            Console.WriteLine("JSON Output (Hotels.xml): " + result);
        }

        public static string Verification(string xmlFilePath, string xsdFilePath)
        {
            try
            {
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add("", xsdFilePath);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas = schema;
                settings.ValidationType = ValidationType.Schema;

                settings.ValidationEventHandler += (sender, e) =>
                {
                    throw new Exception(e.Message);
                };

                using (XmlReader reader = XmlReader.Create(xmlFilePath, settings))
                {
                    while (reader.Read()) { }
                }

                return "No Error";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public static string Xml2Json(string xmlFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);

            // Convert XML to JSON string using Newtonsoft.Json
            string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, true);

            // Manually remove empty "Rating" fields if necessary (JSON.NET does not handle this in .NET 4.5)
            if (jsonText.Contains("\"_Rating\": null"))
            {
                jsonText = jsonText.Replace("\"_Rating\": null,", "");
            }

            return jsonText;
        }
    }
}
