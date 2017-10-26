namespace MaestroPanel.Api
{
    using MaestroPanel.Api.Entity;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;

    public class LogHelper: IDisposable
    {
        private string LogFilePath;
        private List<string> LogRows = new List<string>();

        public LogHelper()
        {
            LogFilePath = Path.Combine(Environment.CurrentDirectory, "mpimport_log.html");

            if (!File.Exists(LogFilePath))
                File.WriteAllText(LogFilePath, "");            
        }        

        public void WriteLog(string url, string method, NameValueCollection _parameters, ApiResult<DomainOperationsResult> result)
        {            
            AddRow(url, method, Explode(_parameters), result.ErrorCode, result.Message, result.StatusCode);
        }

        public void WriteLog(string url, string method, NameValueCollection _parameters, ApiResult<ResellerOperationResult> result)
        {
            AddRow(url, method, Explode(_parameters), result.ErrorCode, result.Message, result.StatusCode);
        }

        public void WriteLog(string url, string method, List<KeyValuePair<string, string>> _parameters, ApiResult<DomainOperationsResult> result)
        {
            AddRow(url, method, Explode(_parameters), result.ErrorCode, result.Message, result.StatusCode);
        }

        public void WriteLog(string url, string method, List<KeyValuePair<string, string>> _parameters, ApiResult<ResellerOperationResult> result)
        {            
            AddRow(url, method, Explode(_parameters), result.ErrorCode, result.Message, result.StatusCode);
        }

        public void WriteLog(string url, string method, List<KeyValuePair<string, string>> _parameters, string result)
        {
            AddRow(url, method, Explode(_parameters), -1, result, -1);
        }

        public void WriteLog(string url, string method, NameValueCollection _parameters, string result)
        {
            AddRow(url, method, Explode(_parameters), -1, result, -1);
        }

        private void AddRow(string url, string method, string request, int errorCode, string message, int statusCode)
        {
            var status = errorCode == 0 ? "OK" : "Error";
            var bgcolor = errorCode == 0 ? "#d0f6c0" : "#fabfbf";

            var row = String.Format(@"	<table style=""width:98%;border:1px solid #ECECEC;border-spacing:0;border-collapse:collapse;"">
                                        <tr>
		                                    <td style=""width:10%;text-align:center;vertical-align:middle;background-color:{7}"">{6}</td>
		                                    <td>		
			                                    <strong>Request</strong> - {8}<br/>
			                                    {1} {0}
                                                <br/>
                                                <strong>Parameters</strong>
                                                <br/>
                                                <blockquote>
                                                    {2}
                                                </blockquote>
			                                    <hr>
			                                    <strong>Response</strong><br/>
                                                Http Status Code: {5} <br/>
			                                    Error Code: {3} <br/>
                                                Message: {4} <br/>
			                                    </td>
	                                    </tr>
                                            </table><br/>", url, method, request, errorCode, message, statusCode, status, bgcolor, DateTime.Now);

            File.AppendAllText(LogFilePath, row);            
        }

        private string Explode(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key) ?? new List<string>().ToArray()
                         select string.Format("{0}: {1}", key, value))
                .ToArray();

            return string.Join("<br/>", array);
        }

        private string Explode(List<KeyValuePair<string, string>> nvc)
        {
            var prms = new List<string>();

            foreach (var item in nvc)
                prms.Add(String.Format("{0}: {1}", item.Key, item.Value));

            return String.Join("<br/>", prms.ToArray());
        }

        public void Dispose()
        {

        }
    }
}
