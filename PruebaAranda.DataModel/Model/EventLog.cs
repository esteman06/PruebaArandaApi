using System;

namespace PruebaAranda.DataModel.Model
{
    public partial class EventLog
    {
        public Guid EventLogId { get; set; }
        public Guid IdentityUserId { get; set; }
        public DateTime Date { get; set; }
        public string ObjectType { get; set; }
        public bool IsSuccessfull { get; set; }
        public string Response { get; set; }
        public string Parameters { get; set; }
        public string Action { get; set; }

        public IdentityUser IdentityUser { get; set; }
    }
}
