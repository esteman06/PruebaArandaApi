using System;

namespace PruebaAranda.Entitis
{
    public class InformationUserView
    {
        public Guid? InformationUserId { get; set; }
        public Guid? IdentityUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public Guid RolsId { get; set; }
        public string Password { get; set; }
    }
}
