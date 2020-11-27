using System;

namespace PruebaAranda.Entitis
{
    public class EventLogView
    {
        public Guid? EventLogId { get; set; }
        public Guid IdentityUserId { get; set; }
        public string Email { get; set; }
        public string Date { get; set; }
        public string ObjectType { get; set; }
        public bool IsSuccessfull { get; set; }
        public string Response { get; set; }
        public string Parameters { get; set; }
        public string Action { get; set; }
    }
}
