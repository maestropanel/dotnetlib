namespace MaestroPanel.Api
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlHelper
    {
        public static string SerializeObjectToXmlString<T>(T TModel)
        {
            string xmlData = String.Empty;

            XmlSerializerNamespaces EmptyNameSpace = new XmlSerializerNamespaces();
            EmptyNameSpace.Add("", "");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlWriter = new XmlTextWriter(memoryStream, new System.Text.UTF8Encoding(false));
            xmlWriter.Formatting = Formatting.Indented;

            xmlSerializer.Serialize(xmlWriter, TModel, EmptyNameSpace);

            memoryStream = (MemoryStream)xmlWriter.BaseStream;
            xmlData = UTF8ByteArrayToString(memoryStream.ToArray());

            return xmlData;
        }

        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        public static T DeSerializeObject<T>(string xmlData = "", string filePath = "")
        {
            T deSerializeObject = default(T);

            if (!String.IsNullOrEmpty(filePath))
                xmlData = File.ReadAllText(filePath);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            StringReader stringReader = new StringReader(xmlData);

            XmlReader XR = new XmlTextReader(stringReader);

            if (xmlSerializer.CanDeserialize(XR))
            {
                deSerializeObject = (T)xmlSerializer.Deserialize(XR);
            }

            return deSerializeObject;
        }
    }
}
