using System;

namespace PruebaAranda.DataModel.Model
{
    public partial class RolAuthorizations
    {
        public Guid RolAuthorizationsId { get; set; }
        public Guid RolId { get; set; }
        public Guid AuthorizationId { get; set; }
    }
}
