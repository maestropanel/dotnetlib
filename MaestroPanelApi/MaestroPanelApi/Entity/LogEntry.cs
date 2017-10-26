namespace MaestroPanel.Api.Entity
{
    using System.Collections.Generic;

    public class LogEntry
    {
        public string Url { get; set; }
        public string Method { get; set; }
        public List<KeyValuePair<string, string>> Parameters { get; set; }

        public string Response { get; set; }
    }
}
