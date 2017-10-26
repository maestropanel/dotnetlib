namespace MaestroPanelApi
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public enum UserTypes
    {
        Admin,
        Reseller,
        DomainUser,
        EmailUser,
        FtpUser,
        None
    }

    public enum DomainStatuses
    {
        Start,
        Stop,
        inProcess
    }

    [Serializable]
    [XmlRoot("Result")]
    public class ApiResult
    {
        private XmlNode cDataAttributeField;

        [XmlElement(ElementName="ErrorCode")]
        public int Code { get; set; }

        [XmlElement(ElementName = "StatusCode")]
        public int StatusCode { get; set; }

        [XmlElement]
        public string Message { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public XmlNode OperationResult
        {
            get
            {
                return this.cDataAttributeField;
            }
            set
            {
                this.cDataAttributeField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore()]
        public String OperationResultString
        {
            get
            {
                return cDataAttributeField.Value;
            }

            set
            {
                XmlDocument xmlDocument = new XmlDocument();
                this.OperationResult = xmlDocument.CreateCDataSection(value);
            }
        }

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

    [Serializable]
    [XmlRoot("Domain")]
    public class ExportPostOffice
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public long Quota { get; set; }

        [XmlArray("Accounts")]
        [XmlArrayItem("Account")]
        public ExportPostOfficeAccount[] Accounts { get; set; }
    }

    public class ExportPostOfficeAccount
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool Status { get; set; }

        [XmlAttribute]
        public long Quota { get; set; }

        [XmlAttribute]
        public int Usage { get; set; }
    }

    public class DomainListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DomainStatuses Status { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string OwnerName { get; set; }
    }

    public class LoginListItem
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserTypes LoginType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Status { get; set; }
        public bool ApiAccess { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Organization { get; set; }
    }

}
