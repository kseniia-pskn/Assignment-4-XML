using System;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    public class Program
    {
        public static string xmlURL = "https://www.public.asu.edu/~kpiskun/Hotels.xml";
        public static string xmlErrorURL = "https://www.public.asu.edu/~kpiskun/HotelsErrors.xml";
        public static string xsdURL = "https://www.public.asu.edu/~kpiskun/Hotels.xsd";

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
public static string Xml2Json(string xmlUrl)
{
    XmlDocument doc = new XmlDocument();
    doc.Load(xmlUrl);

    // Convert XML to JSON string
    string jsonText = JsonConvert.SerializeXmlNode(doc);

    // Clean up JSON output
    jsonText = jsonText.Replace("{\"?xml\":{\"@version\":\"1.0\",\"@encoding\":\"UTF-8\"},", "{");
    jsonText = jsonText.Replace("@", ""); // Remove "@" prefixes
    jsonText = jsonText.Replace("?_declaration", ""); // Remove potential XML declaration artifacts

    // Return cleaned JSON text
    return jsonText;
}
    }
}
