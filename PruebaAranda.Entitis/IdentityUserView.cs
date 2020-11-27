using System;

namespace PruebaAranda.Entitis
{
    public class IdentityUserView
    {
        public Guid? IdentityUserId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsTempPassword { get; set; }
        public string Token { get; set; }
        public Guid RolsId { get; set; }
        public string RolName { get; set; }
        public bool AuthEdit { get; set; }
        public bool AuthQuery { get; set; }
        public bool AuthCreate { get; set; }
        public bool AuthDelete { get; set; }
    }
}
