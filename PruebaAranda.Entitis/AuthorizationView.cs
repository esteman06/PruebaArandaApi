using System;

namespace PruebaAranda.Entitis
{
    public class AuthorizationView
    {
        public Guid? AuthorizationId { get; set; }
        public string AuthorizationNameSystem { get; set; }
        public string AuthorizationNameApplication { get; set; }
    }
}
