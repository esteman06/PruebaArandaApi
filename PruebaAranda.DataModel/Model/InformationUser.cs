using System;

namespace PruebaAranda.DataModel.Model
{
    public partial class InformationUser
    {
        public Guid InformationUserId { get; set; }
        public Guid IdentityUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public Guid RolsId { get; set; }
    }
}
