using System;

namespace PruebaAranda.DataModel.Model
{
    public partial class Authorization
    {
        public Guid AuthorizationId { get; set; }
        public string AuthorizationNameSystem { get; set; }
        public string AuthorizationNameApplication { get; set; }
    }
}
