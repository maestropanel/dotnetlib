namespace MaestroPanel.Api.Entity
{
    using Newtonsoft.Json.Converters;

    public class Whoami
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UserType { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organization { get; set; }
    }   
}
