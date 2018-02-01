namespace MaestroPanel.Api.Entity
{
    using System;

    public class DomainListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DomainStatuses Status { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string OwnerName { get; set; }
    }
}
