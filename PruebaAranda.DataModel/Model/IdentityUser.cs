using System;

namespace PruebaAranda.DataModel.Model
{
    public partial class IdentityUser
    {
        public Guid IdentityUserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsTempPassword { get; set; }
    }
}
