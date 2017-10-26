namespace MaestroPanel.Api
{
    using MaestroPanel.Api.Entity;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;

    public class MaestroPanelClient
    {
        private string _apiKey;
        private string _apiUri;
        private string _format;
        private bool _SuppressResponse;
        private bool _SuppressDnsZoneIp;
        private bool _GeneratePassword;

        private LogHelper _log;

        public const int DOMAIN_OPERATION_ONERROR = -1;
        public const int DOMAIN_OPERATION_SUCCESS = 0;
        public const int API_AUTHENTICATION_ERROR = 5;
        public const int API_PARAMETER_ERROR = 6;
        public const int API_ACCESS_DENIED = 7;
        public const int API_DOMAIN_NOT_FOUND = 8;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApiKey"></param>
        /// <param name="apiHostdomain"></param>
        /// <param name="port"></param>
        /// <param name="ssl"></param>
        /// <param name="format">Varsayılan Değer: Xml
        ///Alabileceği Değerler: Xml, Json
        ///Tip: String
        ///
        ///format parametresi MaestroPanel API'nin hangi tipte cevap vereceğini belirler. Xml ve Json tipinde cevap verebilir.
        ///
        ///* Büyük küçük harf fark etmez
        ///</param>
        /// <param name="suppressResponse">
        ///Varsayılan Değer: true
        ///Alabileceği Değerler: true, false
        ///Tip: Boolean
        ///
        ///MaestroPanel API'nin http durum kodu ile cevap vermesini pasivize eder veya aktif eder. False değeri verildiğinde HTTP durum kodlarını verilen cevap içinde alabilirsiniz.
        ///* Büyük küçük harf fark etmez
        ///</param>
        /// <param name="suppressDnsZoneIP"></param>
        /// <param name="generatePassword"></param>
        public MaestroPanelClient(string ApiKey, 
                string apiHostdomain, 
                int port = 9715, 
                bool ssl = false, 
                string format = "JSON", 
                bool suppressResponse = true, 
                bool suppressDnsZoneIP = false, 
                bool generatePassword = false)
        {
            this._format = format;
            this._apiKey = ApiKey;
            this._apiUri = String.Format("{2}://{0}:{1}/Api/v1", apiHostdomain, port, ssl ? "https" : "http");
            this._SuppressResponse = suppressResponse;
            this._SuppressDnsZoneIp = suppressDnsZoneIP;
            this._GeneratePassword = generatePassword;

            _log = new LogHelper();
        }

        public ApiResult<DomainOperationsResult> DomainDelete(string name)
        {
            var _args = new NameValueCollection();            
            _args.Add("name", name);

            return ExecuteDomainOperation("Domain/Delete", "DELETE", _args);
        }

        public ApiResult<DomainOperationsResult> DomainStart(string name)
        {
            var _args = new NameValueCollection();            
            _args.Add("name", name);
            
            
            return ExecuteDomainOperation("Domain/Start", "POST", _args);
        }

        public ApiResult<DomainOperationsResult> DomainStop(string name)
        {
            var _args = new NameValueCollection();            
            _args.Add("name", name);

            return ExecuteDomainOperation("Domain/Stop", "POST", _args);
        }

        private string GeneratePassword(int Length)
        {
            return System.Web.Security.Membership.GeneratePassword(8, 2);
        }

        public string SetPassword(string password)
        {
            return _GeneratePassword ? GeneratePassword(8) : password;
        }

        public ApiResult<DomainOperationsResult> DomainCreate(string name, string planAlias, string username, string password, bool activedomainuser,
                                        string firstName = "", string lastName = "", string email = "", DateTime? expiration = null)
        {
            var _args = new NameValueCollection();            
            _args.Add("name", name);
            _args.Add("planAlias", planAlias);
            _args.Add("username", username);
            _args.Add("password", SetPassword(password));
            _args.Add("activedomainuser", activedomainuser.ToString());
            _args.Add("firstname", firstName);
            _args.Add("lastname", lastName);
            _args.Add("email", email);

            if (expiration.HasValue)
                _args.Add("expiration", expiration.Value.ToString("yyyy-MM-dd"));
                        
            return ExecuteDomainOperation("Domain/Create", "POST", _args);
        }
 
        public ApiResult<DomainOperationsResult> AddMailBox(string name, string account, string password, double quota, string redirect, string redirectEmail)
        {
            var _args = new NameValueCollection();            
            _args.Add("name", name);
            _args.Add("account", account);
            _args.Add("password", SetPassword(password));
            _args.Add("quota", quota.ToString());
            _args.Add("redirect", redirect);
            _args.Add("remail", redirectEmail);

            
            return ExecuteDomainOperation("Domain/AddMailBox", "POST", _args);
            
        }

        public ApiResult<DomainOperationsResult> AddDatabase(string name, string dbtype, string database, string username, string password, int quota)
        {
            var _args = new NameValueCollection();            
            _args.Add("name", name);
            _args.Add("dbtype", dbtype);
            _args.Add("database", database);
            _args.Add("username", username);
            _args.Add("password", SetPassword(password));
            _args.Add("quota", quota.ToString());

            return ExecuteDomainOperation("Domain/AddDatabase", "POST", _args);
        }

        public ApiResult<DomainOperationsResult> AddDatabase(string name, string dbtype, string database, int quota)
        {
            var _args = new NameValueCollection();
            _args.Add("name", name);
            _args.Add("dbtype", dbtype);
            _args.Add("database", database);
            _args.Add("username", "");
            _args.Add("password", "");
            _args.Add("quota", quota.ToString());

            return ExecuteDomainOperation("Domain/AddDatabase", "POST", _args);
        }

        public ApiResult<DomainOperationsResult> AddDatabaseUser(string name, string dbtype, string database, string username, string password)
        {
            var _args = new NameValueCollection();            
            _args.Add("name", name);
            _args.Add("dbtype", dbtype);
            _args.Add("database", database);
            _args.Add("username", username);
            _args.Add("password", SetPassword(password));
            
            return ExecuteDomainOperation("Domain/AddDatabaseUser", "POST", _args);
        }

        public ApiResult<DomainOperationsResult> AddSubDomain(string name, string subdomain, string username, string password)
        {
            var _args = new NameValueCollection();            
            _args.Add("name", name);
            _args.Add("subdomain", subdomain);
            _args.Add("ftpuser", username);

            return ExecuteDomainOperation("Domain/AddSubDomain", "POST", _args);
        }

        public ApiResult<DomainOperationsResult> AddAlias(string name, string alias)
        {
            var _args = new NameValueCollection();            
            _args.Add("name", name);
            _args.Add("alias", alias);

            return ExecuteDomainOperation("Domain/AddDomainAlias", "POST", _args);
        }

        public ApiResult<DomainOperationsResult> AddFtpUser(string name, string account, string password, string homePath = "/", bool ronly = false)
        {
            var _args = new NameValueCollection();            
            _args.Add("name", name);
            _args.Add("account", account);
            _args.Add("password", SetPassword(password));
            _args.Add("homePath", homePath);
            _args.Add("ronly", ronly.ToString());

            return ExecuteDomainOperation("Domain/AddFtpAccount", "POST", _args);
        }

        public ApiResult<DomainOperationsResult> SetForwarding(string name, bool enabled, string destination, bool exacDestination, bool childOnly, string statusCode)
        {
            var _args = new NameValueCollection();
            _args.Add("name", name);
            _args.Add("enabled", enabled.ToString());
            _args.Add("destination", destination);
            _args.Add("exacDestination", exacDestination.ToString());
            _args.Add("childOnly", childOnly.ToString());
            _args.Add("statusCode", statusCode);

            return ExecuteDomainOperation("Domain/Forwarding", "POST", _args);
        }

        public ApiResult<ResellerOperationResult> ResellerCreate(string username, string password, string planAlias,
            string firstName, string lastName, string email, string country, string organization,
                string address1, string address2, string city, string province, string postalcode,
                    string phone, string fax)
        {
            var _args = new NameValueCollection();            
            _args.Add("username", username);
            _args.Add("password", SetPassword(password));
            _args.Add("planAlias", planAlias);
            _args.Add("firstName", firstName);
            _args.Add("lastname", lastName);
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

            return ExecuteResellerOperation("Reseller/Create", "POST", _args);
        }

        public ApiResult<ResellerOperationResult> ResellerChangePassword(string username, string newpassword)
        {
            var _args = new NameValueCollection();
            _args.Add("username", username);
            _args.Add("newpassword", newpassword);

            return ExecuteResellerOperation("Reseller/ChangePassword", "POST", _args);
        }

        public ApiResult<ResellerOperationResult> ResellerStop(string username)
        {
            var _args = new NameValueCollection();
            _args.Add("username", username);

            return ExecuteResellerOperation("Reseller/Stop", "POST", _args);
        }

        public ApiResult<ResellerOperationResult> ResellerSetLimit(string username,
            int maxdomain,
            int maxdiskspace, int maxmailbox,
            int maxftpuser, int maxsubdomain,
            int maxdomainalias, int totalwebtraffic,
            int totalmailspace, int maxwebtraffic,
            int maxftptraffic, int maxmailtraffic,
            int maxmysql, int maxmysqluser,
            int maxmysqlspace, int maxmssql,
            int maxmssqluser, int maxmssqlspace)
        {
            var _args = new NameValueCollection();
            _args.Add("username", username);
            _args.Add("maxdomain", maxdomain.ToString());
            _args.Add("maxdiskspace", maxdiskspace.ToString());
            _args.Add("maxmailbox", maxmailbox.ToString());
            _args.Add("maxftpuser", maxftpuser.ToString());
            _args.Add("maxsubdomain", maxsubdomain.ToString());
            _args.Add("maxdomainalias", maxdomainalias.ToString());
            _args.Add("totalmailspace", totalmailspace.ToString());
            _args.Add("maxftptraffic", maxftptraffic.ToString());
            _args.Add("maxmailtraffic", maxmailtraffic.ToString());

            _args.Add("maxmysql", maxmysql.ToString());
            _args.Add("maxmysqluser", maxmysqluser.ToString());
            _args.Add("maxmysqlspace", maxmysqlspace.ToString());

            _args.Add("maxmssql", maxmssql.ToString());
            _args.Add("maxmssqluser", maxmssqluser.ToString());
            _args.Add("maxmssqlspace", maxmssqlspace.ToString());

            return ExecuteResellerOperation("Reseller/SetLimits", "POST", _args);
        }

        public ApiResult<DomainOperationsResult> ChangeReseller(string name, string newResellerName)
        {
            var _args = new NameValueCollection();
            _args.Add("name", name);
            _args.Add("resellerName", newResellerName);

            return ExecuteDomainOperation("Domain/ChangeReseller", "POST", _args);
        }
        
        public ApiResult<Whoami> Whoami()
        {
            var requestUrl = String.Empty;

            var _args = new NameValueCollection();
            return SendApi<ApiResult<Whoami>>("User/Whoami", "GET", _args, new[] { typeof(Whoami) }, out requestUrl);
        }


        public ApiResult<DomainOperationsResult> SetDnsZone(string name, int soa_expired, int soa_ttl, int soa_refresh, string soa_email, int soa_retry, int soa_serial,
            string primaryServer, List<DnsZoneRecordItem> records)
        {
            var _args = new List<KeyValuePair<string, string>>();
            _args.Add(new KeyValuePair<string, string>("name", name));            
            _args.Add(new KeyValuePair<string, string>("soa_expired", soa_expired.ToString()));
            _args.Add(new KeyValuePair<string, string>("soa_ttl", soa_ttl.ToString()));
            _args.Add(new KeyValuePair<string, string>("soa_refresh", soa_refresh.ToString()));
            _args.Add(new KeyValuePair<string, string>("soa_email", soa_email));
            _args.Add(new KeyValuePair<string, string>("soa_retry", soa_retry.ToString()));
            _args.Add(new KeyValuePair<string, string>("soa_serial", soa_serial.ToString()));
            _args.Add(new KeyValuePair<string, string>("primaryServer", primaryServer));
            _args.Add(new KeyValuePair<string, string>("suppress_host_ip", _SuppressDnsZoneIp.ToString()));

            foreach (var item in records)
                _args.Add(new KeyValuePair<string, string>("record", String.Format("{0},{1},{2},{3}", item.name, item.type, item.value, item.priority)));                        

            return ExecuteDomainOperation("Domain/SetDnsZone", "POST", _args);
        }

        public ApiResult<DomainOperationsResult> SetLimits(string name, 
            int maxdiskspace, 
            int maxmailbox, 
            int maxftpuser, 
            int maxsubdomain,
            int maxdomainalias, 
            int totalwebtraffic, 
            int totalmailspace, 
            int maxwebtraffic, 
            int maxftptraffic, 
            int maxmailtraffic,
            int maxmysql, 
            int maxmysqluser, 
            int maxmysqlspace, 
            int maxmssql, 
            int maxmssqluser, 
            int maxmssqlspace)
        {            
            var _args = new NameValueCollection();
            _args.Add("name", name);
            _args.Add("maxdiskspace", maxdiskspace.ToString());
            _args.Add("maxmailbox", maxmailbox.ToString());
            _args.Add("maxftpuser", maxftpuser.ToString());
            _args.Add("maxsubdomain", maxsubdomain.ToString());
            _args.Add("maxdomainalias", maxdomainalias.ToString());
            _args.Add("totalwebtraffic", totalwebtraffic.ToString());
            _args.Add("totalmailspace", totalmailspace.ToString());
            _args.Add("maxwebtraffic", maxwebtraffic.ToString());
            _args.Add("maxftptraffic", maxftptraffic.ToString());
            _args.Add("maxmailtraffic", maxmailtraffic.ToString());

            _args.Add("maxmysql", maxmysql.ToString());
            _args.Add("maxmysqluser", maxmysqluser.ToString());
            _args.Add("maxmysqlspace", maxmysqlspace.ToString());
            
            _args.Add("maxmssql", maxmssql.ToString());
            _args.Add("maxmssqlspace", maxmssqlspace.ToString());
            _args.Add("maxmssqluser", maxmssqluser.ToString());

            return ExecuteDomainOperation("Domain/SetLimits", "POST", _args);
        }
        

        public ApiResult<DomainOperationsResult> AddDnsRecord(string name, string rec_type, string rec_name, string rec_value, int priority)
        {
            var _args = new NameValueCollection();        
            _args.Add("name", name);
            _args.Add("rec_type", rec_type);
            _args.Add("rec_name", rec_name);
            _args.Add("rec_value", rec_value);
            _args.Add("priority", priority.ToString());            
            
            return ExecuteDomainOperation("Domain/AddDnsRecord", "POST", _args);
        }

        public ApiResult<DomainOperationsResult> DeleteDnsRecord(string name, string rec_type, string rec_name, string rec_value, int priority)
        {
            var _args = new NameValueCollection();
            _args.Add("name", name);
            _args.Add("rec_type", rec_type);
            _args.Add("rec_name", rec_name);
            _args.Add("rec_value", rec_value);
            _args.Add("priority", priority.ToString());

            return ExecuteDomainOperation("Domain/DeleteDnsRecord", "POST", _args);
        }

        public ApiResult<DomainOperationsResult> ChangeEmailPassword(string name, string account, string newpassword)
        {
            var _args = new NameValueCollection();
            _args.Add("name", name);
            _args.Add("account", account);
            _args.Add("newpassword", newpassword);

            return ExecuteDomainOperation("Domain/ChangeMailBoxPassword", "POST", _args);
        }


        #region Privates
        private void WriteData(ref HttpWebRequest _request, NameValueCollection _parameters)
        {
            byte[] byteData = CreateParameters(_parameters);
            _request.ContentLength = byteData.Length;

            using (Stream postStream = _request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
        }

        private void WriteData(ref HttpWebRequest _request, List<KeyValuePair<string, string>> _parameters)
        {
            byte[] byteData = CreateParameters(_parameters);
            _request.ContentLength = byteData.Length;

            using (Stream postStream = _request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
        }

        private string GetData(HttpWebRequest _request, out string contentType)
        {
            contentType = String.Empty;
            var _response = String.Empty;

            using (HttpWebResponse response = _request.GetResponse() as HttpWebResponse)
            {                                
                contentType = response.ContentType;
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

        private byte[] CreateParameters(List<KeyValuePair<string, string>> _parameters)
        {
            var _sb = new StringBuilder(_parameters.Count);

            foreach (var item in _parameters)
                _sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(item.Key), HttpUtility.UrlEncode(item.Value));

            _sb.Length -= 1;

            return UTF8Encoding.UTF8.GetBytes(_sb.ToString());
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return string.Join("&", array);
        }

        private string ToQueryString(List<KeyValuePair<string, string>> nvc)
        {
            var prms = new List<string>();

            foreach (var item in nvc)            
                prms.Add(String.Format("{0}={1}", HttpUtility.UrlEncode(item.Key), HttpUtility.UrlEncode(item.Value)));
            
            return String.Join("&", prms.ToArray());
        }
        
        private ApiResult<DomainOperationsResult> ExecuteDomainOperation(string action, string method, NameValueCollection args)
        {
            var requestUri = String.Empty;
            var result = new ApiResult<DomainOperationsResult>();

            try
            {
                result = SendApi<ApiResult<DomainOperationsResult>>(action, method, args,
                        new[] { typeof(DomainOperationsResult), typeof(DomainOperationModuleResult) }, out requestUri);

                _log.WriteLog(requestUri, method, args, result);
            }
            catch (Exception ex)
            {
                result = new ApiResult<DomainOperationsResult>() { ErrorCode = 500, Message = ex.Message, StatusCode = 200 };
            }

            return result;
        }

        private ApiResult<DomainOperationsResult> ExecuteDomainOperation(string action, string method, List<KeyValuePair<string, string>> args)
        {
            var requestUri = String.Empty;
            var result = new ApiResult<DomainOperationsResult>();

            try
            {
                result = SendApi<ApiResult<DomainOperationsResult>>(action, method, args,
                    new[] { typeof(DomainOperationsResult), typeof(DomainOperationModuleResult) }, out requestUri);

                _log.WriteLog(requestUri, method, args, result);
            }
            catch (Exception ex)
            {
                result = new ApiResult<DomainOperationsResult>() { ErrorCode = 500, Message = ex.Message, StatusCode = 200 };
            }

            return result;
        }

        private ApiResult<ResellerOperationResult> ExecuteResellerOperation(string action, string method, NameValueCollection args)
        {
            var requestUri = String.Empty;

            var result = new ApiResult<ResellerOperationResult>();

            try
            {

                result = SendApi<ApiResult<ResellerOperationResult>>(action, method, args,
                        new[] { typeof(ResellerOperationResult) }, out requestUri);

                _log.WriteLog(requestUri, method, args, result);

            }
            catch (Exception ex)
            {
                result = new ApiResult<ResellerOperationResult>() { ErrorCode = 500, Message = ex.Message, StatusCode = 200 };
            }

            return result;
        }

        private T SendApi<T>(string action, string method, NameValueCollection _parameters, Type[] extraTypes, out string RequestUri)
        {
            var _result = default(T);

            var contentType = String.Empty;

            if (method == "GET")
            {
                _parameters.Add("key", _apiKey);
                _parameters.Add("format", _format);

                if (_SuppressResponse)
                    _parameters.Add("suppress_response_codes", "true");
            }

            var _uri =
                method == "GET" ?
                new Uri(String.Format("{0}/{1}?{2}", _apiUri, action, ToQueryString(_parameters))) :
                new Uri(String.Format("{0}/{1}?key={2}&format={3}&suppress_response_codes={4}", _apiUri, action, _apiKey, _format, _SuppressResponse ? "true" : "false"));

            RequestUri = _uri.ToString();

            try
            {
                HttpWebRequest request = WebRequest.Create(_uri) as HttpWebRequest;
                request.Method = method;
                request.Timeout = 240 * 1000;
                request.ContentType = "application/x-www-form-urlencoded";

                if (method != "GET")
                    WriteData(ref request, _parameters);

                var _responseText = GetData(request, out contentType);

                if (_format == "JSON")
                    _result = JsonConvert.DeserializeObject<T>(_responseText, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                else
                    _result = XmlHelper.DeSerializeObject<T>(_responseText);

            }
            catch (Exception ex)
            {
                _log.WriteLog(RequestUri, method, _parameters, String.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
            }

            return _result;
        }

        private T SendApi<T>(string action, string method, List<KeyValuePair<string, string>> _parameters, Type[] extraTypes, out string RequestUri)
        {
            var _result = default(T);
            var contentType = String.Empty;

            if (method == "GET")
            {
                _parameters.Add(new KeyValuePair<string,string>("key", _apiKey));
                _parameters.Add(new KeyValuePair<string,string>("format", _format));

                if (_SuppressResponse)
                    _parameters.Add(new KeyValuePair<string,string>("suppress_response_codes", "true"));
            }

            var _uri =
                method == "GET" ?
                new Uri(String.Format("{0}/{1}?{2}", _apiUri, action, ToQueryString(_parameters))) :
                new Uri(String.Format("{0}/{1}?key={2}&format={3}&suppress_response_codes={4}", _apiUri, action, _apiKey, _format, _SuppressResponse ? "true" : "false"));


            RequestUri = _uri.ToString();

            try
            {           
                HttpWebRequest request = WebRequest.Create(_uri) as HttpWebRequest;
                request.Method = method;
                request.Timeout = 240 * 1000;
                request.ContentType = "application/x-www-form-urlencoded";
            
                if (method != "GET")
                    WriteData(ref request, _parameters);

                var _responseText = GetData(request, out contentType);

                if (_format == "JSON")
                    _result = JsonConvert.DeserializeObject<T>(_responseText, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                else
                    _result = XmlHelper.DeSerializeObject<T>(_responseText);

            }
            catch (Exception ex)
            {
                _log.WriteLog(RequestUri, method, _parameters, String.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
            }

            return _result;
        }
        #endregion
    }
}
