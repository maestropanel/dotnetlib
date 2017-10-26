namespace MaestroPanelApi
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Collections.Generic;

    public class Client
    {
        private string _apiKey;
        private string _apiUri;

        public Client(string ApiKey, string apiHostdomain, int port = 9715, bool ssl = false)
        {
            _apiKey = ApiKey;
            _apiUri = String.Format("{2}://{0}:{1}/Api/v1", apiHostdomain, port, ssl ? "https" : "http");
        }

        public ApiResult DomainDelete(string name)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);

            return SendApi("Domain/Delete", "DELETE", _args);
        }

        public ApiResult DomainStart(string name)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);

            return SendApi("Domain/Start", "POST", _args);
        }

        public ApiResult DomainStop(string name)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);

            return SendApi("Domain/Stop", "POST", _args);
        }

        public ApiResult DomainCreate(string name, string planAlias, string username, string password, bool activedomainuser, string firstName = "", string lastName = "", string email = "")
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("planAlias", planAlias);
            _args.Add("username", username);
            _args.Add("password", password);
            _args.Add("activedomainuser", activedomainuser.ToString());
            _args.Add("firstname", firstName);
            _args.Add("lastname", lastName);
            _args.Add("email", email);

            return SendApi("Domain/Create", "POST", _args);
        }

        public ApiResult DomainChangePassword(string name, string newpassword)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("newpassword", newpassword);

            return SendApi("Domain/Password", "POST", _args);
        }

        public ApiResult AddDomainAlias(string name, string aliasname)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("alias", aliasname);

            return SendApi("Domain/AddDomainAlias", "POST", _args);
        }

        public ApiResult AddSubDomain(string name, string subdomain, string username, string password)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("subdomain", subdomain);
            _args.Add("username", username);
            _args.Add("password", password);

            return SendApi("Domain/AddSubDomain", "POST", _args);
        }

        public ApiResult ChangeIpAddr(string name, string newip)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("newip", newip);

            return SendApi("Domain/ChangeIpAddr", "POST", _args);
        }

        public List<DomainListItem> GetDomainList()
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);            
            _args.Add("format", "XML");

            var xmlString = GetData("Domain/GetList", "GET", _args);            
            return ApiResult.DeSerializeObject<List<DomainListItem>>(xmlString);
        }

        public ApiResult SetWriteAccess(string name, string path)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("path", path);            

            return SendApi("Domain/SetWriteAccess", "POST", _args);
        }

        public ApiResult AddMailBox(string name, string account, string password, int quota, string redirect, string remail)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("account", account);
            _args.Add("password", password);
            _args.Add("quota", quota.ToString());
            _args.Add("redirect", redirect);
            _args.Add("remail", remail);

            return SendApi("Domain/AddMailBox", "POST", _args);
        }

        public ApiResult DeleteMailBox(string name, string account)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("account", account);

            return SendApi("Domain/DeleteMailBox", "POST", _args);
        }

        public ApiResult ChangeMailBoxQuota(string name, string account, int quota)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("account", account);
            _args.Add("quota", quota.ToString());

            return SendApi("Domain/ChangeMailBoxQuota", "POST", _args);
        }

        public ApiResult ChangeMailBoxPassword(string name, string account, string newpassword)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("account", account);
            _args.Add("newpassword", newpassword);

            return SendApi("Domain/ChangeMailBoxPassword", "POST", _args);
        }

        public ExportPostOffice GetMailList(string name)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);

            var xmlString = GetData("Domain/GetMailList", "GET", _args);
            return ApiResult.DeSerializeObject<ExportPostOffice>(xmlString);
        }

        public ApiResult AddDatabase(string name, string dbtype, string database, string username, string password, int quota)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("dbtype", dbtype);
            _args.Add("database", database);
            _args.Add("username", username);
            _args.Add("password", password);
            _args.Add("quota", quota.ToString());

            return SendApi("Domain/AddDatabase", "POST", _args);
        }

        public ApiResult AddDatabaseUser(string name, string dbtype, string database, string username, string password)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("dbtype", dbtype);
            _args.Add("database", database);
            _args.Add("username", username);
            _args.Add("password", password);

            return SendApi("Domain/AddDatabaseUser", "POST", _args);
        }

        public ApiResult DeleteDatabase(string name, string dbtype, string database)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("dbtype", dbtype);
            _args.Add("database", database);

            return SendApi("Domain/DeleteDatabase", "POST", _args);
        }

        public ApiResult AddFtpAccount(string name, string account, string password, string homePath, bool ronly)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("account", account);
            _args.Add("password", password);
            _args.Add("homePath", homePath);
            _args.Add("ronly", ronly.ToString());

            return SendApi("Domain/AddFtpAccount", "POST", _args);
        }

        public ApiResult DeleteFtpAccount(string name, string account)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("account", account);

            return SendApi("Domain/DeleteFtpAccount", "POST", _args);
        }
        
        public ApiResult ChangeFtpPassword(string name, string account, string newpassword)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("account", account);
            _args.Add("newpassword", newpassword);

            return SendApi("Domain/ChangeFtpPassword", "POST", _args);
        }

        public ApiResult ChangeDatabaseUserPassword(string name, string dbtype, string database, string username, string newpassword)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("name", name);
            _args.Add("dbtype", dbtype);
            _args.Add("database", database);
            _args.Add("username", username);
            _args.Add("newpassword", newpassword);

            return SendApi("Domain/ChangeDatabaseUserPassword", "POST", _args);
        }

        public ApiResult ResellerCreate(string username, string password, string planAlias,
            string firstName, string lastName, string email, string country, string organization, string address1, string address2, string city,
            string province, string postalcode, string phone, string fax)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("username", username);
            _args.Add("password", password);
            _args.Add("planAlias", planAlias);
            _args.Add("firstName", firstName);
            _args.Add("lastName", lastName);
            _args.Add("email", email);
            _args.Add("country", country);
            _args.Add("organization", organization);
            _args.Add("address1", address1);
            _args.Add("address2", address2);
            _args.Add("city", city);
            _args.Add("province", province);
            _args.Add("postalcode", postalcode);
            _args.Add("phone", phone);
            _args.Add("fax", fax);

            return SendApi("Reseller/Create", "POST", _args);
        }

        public ApiResult ResellerStop(string username)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);            
            _args.Add("username", username);            

            return SendApi("Reseller/Stop", "POST", _args);
        }

        public ApiResult ResellerStart(string username)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);            
            _args.Add("username", username);

            return SendApi("Reseller/Start", "POST", _args);
        }

        public ApiResult ResellerChangePassword(string username, string newpassword)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);            
            _args.Add("username", username);
            _args.Add("newpassword", newpassword);

            return SendApi("Reseller/ChangePassword", "POST", _args);
        }

        public ApiResult ResellerAddDomain(string username, string domainName, string planAlias, string domainUsername, string domainPassword,
                                bool activedomainuser, string firstName, string lastName, string email, string expiration)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);            
            _args.Add("username", username);
            _args.Add("domainName",domainName );
            _args.Add("planAlias", planAlias);
            _args.Add("domainUsername", domainUsername);
            _args.Add("domainPassword", domainPassword);
            _args.Add("activedomainuser", activedomainuser.ToString());
            _args.Add("firstName", firstName);
            _args.Add("lastName", lastName);
            _args.Add("email", email);
            _args.Add("expiration", expiration);

            return SendApi("Reseller/AddDomain", "POST", _args);
        }

        public ApiResult ResellerDeleteDomain(string username, string domainName)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("username", username);
            _args.Add("domainName", domainName);

            return SendApi("Reseller/DeleteDomain", "DELETE", _args);
        }

        public List<DomainListItem> ResellerGetDomains(string username)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);
            _args.Add("username", username);
            _args.Add("format", "XML");

            var xmlString = GetData("Reseller/GetDomains", "GET", _args);
            return ApiResult.DeSerializeObject<List<DomainListItem>>(xmlString);
        }

        public List<LoginListItem> GetResellers()
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);

            var xmlString = GetData("Reseller/GetResellers", "GET", _args);
            return ApiResult.DeSerializeObject<List<LoginListItem>>(xmlString);
        }

        public ApiResult SetDnsZone(string name, string soa_expired, string soa_ttl, 
                                                string soa_refresh, string soa_email, 
                                                string soa_retry, string soa_serial, string primaryServer, 
                                                bool suppress_host_ip, params string[] records)
        {
            var _args = new NameValueCollection();
            _args.Add("key", _apiKey);            
            _args.Add("name", name);
            _args.Add("soa_expired", soa_expired);
            _args.Add("soa_ttl", soa_ttl);
            _args.Add("soa_refresh", soa_refresh);
            _args.Add("soa_email", soa_email);
            _args.Add("soa_serial", soa_serial);
            _args.Add("primaryServer", primaryServer);
            _args.Add("suppress_host_ip", suppress_host_ip.ToString().ToLowerInvariant());
            _args.Add("suppress_response_codes", "true");
            
            return SendApi("Domain/SetDnsZone", "POST", _args, "record", records);
        }

        #region Privates
        private string GetData(string action, string method, NameValueCollection _parameters)
        {
            var _result = "";
            var _uri = new Uri(String.Format("{0}/{1}", _apiUri, action));

            try
            {
                HttpWebRequest request = WebRequest.Create(_uri) as HttpWebRequest;
                request.Method = method;
                request.Timeout = 120 * 1000;
                request.ContentType = "application/x-www-form-urlencoded";

                WriteData(ref request, _parameters);
                _result = GetData(request);
                
            }
            catch (Exception ex)
            {                
                throw new Exception("API Error", ex);
            }

            return _result;
        }

        private ApiResult SendApi(string action, string method, NameValueCollection _parameters)
        {
            var _result = new ApiResult();
            var _uri = new Uri(String.Format("{0}/{1}?suppress_response_codes=true", _apiUri, action));

            try
            {
                HttpWebRequest request = WebRequest.Create(_uri) as HttpWebRequest;
                request.Method = method;
                request.Timeout = 120 * 1000;
                request.ContentType = "application/x-www-form-urlencoded";

                WriteData(ref request, _parameters);
                var _responseText = GetData(request);

                _result = ApiResult.DeSerializeObject<ApiResult>(_responseText);
            }
            catch (Exception ex)
            {
                _result.Code = -1;
                _result.Message = ex.Message;
                _result.OperationResultString = ex.StackTrace;
            }

            return _result;
        }

        private ApiResult SendApi(string action, string method, NameValueCollection _parameters, string name, string[] values)
        {
            var _result = new ApiResult();
            var _uri = new Uri(String.Format("{0}/{1}?suppress_response_codes=true", _apiUri, action));

            var _sb = new StringBuilder();

            foreach (var item in _parameters.AllKeys)
                _sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(item), HttpUtility.UrlEncode(_parameters[item]));

            foreach (var item in values)
                _sb.AppendFormat("{0}={1}&", name, item);

            _sb.Length -= 1;

            try
            {

                using (var c = new WebClient())
                {
                    c.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    var _responseText = c.UploadString(_uri, _sb.ToString());

                    _result = ApiResult.DeSerializeObject<ApiResult>(_responseText);
                }                
                
            }
            catch (Exception ex)
            {
                _result.Code = -1;
                _result.Message = ex.Message;
                _result.OperationResultString = ex.StackTrace;
            }

            return _result;
        }

        private void WriteData(ref HttpWebRequest _request, NameValueCollection _parameters, string name, string[] values)
        {
            byte[] byteData = CreateParameters(_parameters, name, values);
            _request.ContentLength = byteData.Length;

            using (Stream postStream = _request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
        }

        private void WriteData(ref HttpWebRequest _request, NameValueCollection _parameters)
        {
            byte[] byteData = CreateParameters(_parameters);
            _request.ContentLength = byteData.Length;

            using (Stream postStream = _request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
        }

        private string GetData(HttpWebRequest _request)
        {
            var _response = String.Empty;
            using (HttpWebResponse response = _request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                _response = reader.ReadToEnd();
            }

            return _response;
        }

        private byte[] CreateParameters(NameValueCollection _parameters)
        {
            var _sb = new StringBuilder(_parameters.Count);

            foreach (var item in _parameters.AllKeys)
                _sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(item), HttpUtility.UrlEncode(_parameters[item]));

            _sb.Length -= 1;

            return UTF8Encoding.UTF8.GetBytes(_sb.ToString());
        }

        private byte[] CreateParameters(NameValueCollection _parameters, string name, string[] values)
        {
            var _sb = new StringBuilder();

            foreach (var item in _parameters.AllKeys)
                _sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(item), HttpUtility.UrlEncode(_parameters[item]));

            foreach (var item in values)
                _sb.AppendFormat("{0}={1}&", name, item);

            return UTF8Encoding.UTF8.GetBytes(_sb.ToString());
        }

        private string GeneratePassword(int Length)
        {
            return System.Web.Security.Membership.GeneratePassword(8, 2);
        }
        #endregion
    }
}
