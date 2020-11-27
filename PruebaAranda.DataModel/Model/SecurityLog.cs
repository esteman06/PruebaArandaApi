using System;

namespace PruebaAranda.DataModel.Model
{
    public partial class SecurityLog
    {
        public Guid SecurityLogId { get; set; }
        public DateTime? Date { get; set; }
        public string Activity { get; set; }
        public string RemoteIpaddress { get; set; }
        public Guid? IdentityUserId { get; set; }
    }
}
