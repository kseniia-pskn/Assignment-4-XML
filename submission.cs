using System;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1
{
    public class Program
    {
        // URLs to be updated with your ASU web-hosted files
        public static string xmlURL = "https://www.public.asu.edu/kpiskun/Hotels.xml"; //Q1.2
        public static string xmlErrorURL = "https://www.public.asu.edu/kpiskun/HotelsErrors.xml"; //Q1.3
        public static string xsdURL = "https://www.public.asu.edu/kpiskun/Hotels.xsd"; //Q1.1

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

        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add("", xsdUrl);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(schema);
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += (sender, e) =>
                {
                    throw new Exception(e.Message);
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
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

            string jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, true);
            JObject jsonObject = JObject.Parse(jsonText);

            // Optional: Remove empty fields like "_Rating" if not present in XML
            jsonObject.Descendants()
                .OfType<JProperty>()
                .Where(attr => attr.Name.StartsWith("_") && attr.Value.Type == JTokenType.Null)
                .ToList()
                .ForEach(attr => attr.Remove());

            return jsonObject.ToString();
        }
    }
}
