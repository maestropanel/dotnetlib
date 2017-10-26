namespace MaestroPanel.Api.Entity
{
    using System.Collections.Generic;

    public class DomainOperationsResult
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool DomainUser { get; set; }
        public string IpString { get; set; }

        public List<DomainOperationModuleResult> ModuleResults { get; set; }

        public DomainOperationsResult()
        {
            ModuleResults = new List<DomainOperationModuleResult>();
        }
    }


    public class DomainOperationModuleResult
    {
        public bool Status { get; set; }
        public string Msg { get; set; }
        public string Name { get; set; }
        public string PType { get; set; }
    }
}
