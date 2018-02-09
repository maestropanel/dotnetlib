using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MaestroPanelApi.Test
{
    [TestClass]
    public class UnitTest1
    {
        private string S = "";
        private string H = "";
        private string D = "{\"StatusCode\":200,\"ErrorCode\":0,\"Message\":\"\",\"Details\":[{\"Id\":25,\"Name\":\"a.com.tr\",\"Status\":0,\"ExpirationDate\":\"\\/Date(1547200447000)\\/\",\"OwnerName\":\"aaa\",\"Email\":0,\"Disk\":0,\"IpAddr\":null},{\"Id\":26,\"Name\":\"b.com\",\"ExpirationDate\":\"\\/Date(1547200460000)\\/\",\"Status\":0,\"OwnerName\":\"b\",\"Email\":0,\"Disk\":0,\"IpAddr\":null},{\"Id\":27,\"Name\":\"c.com.tr\",\"ExpirationDate\":\"\\/Date(1547200473000)\\/\",\"Status\":0,\"OwnerName\":\"c\",\"Email\":0,\"Disk\":0,\"IpAddr\":null},{\"Id\":90,\"Name\":\"d.com.tr\",\"ExpirationDate\":\"\\/Date(1547203463000)\\/\",\"Status\":0,\"OwnerName\":\"d\",\"Email\":0,\"Disk\":0,\"IpAddr\":null}]}";

        [TestMethod]
        public void SerializeGetDomainList()
        {
            var c = new MaestroPanel.Api.MaestroPanelClient(S, H);

            var list = c.GetDomainList();

            Assert.IsNotNull(list);
            Assert.AreEqual(true, list.Any());
        }
    }
}
