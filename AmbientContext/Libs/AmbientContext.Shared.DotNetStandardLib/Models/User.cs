using System;

namespace AmbientContext.Shared.DotNetStandardLib.Models
{
    public class User
    {
        public Guid UserGuid { get; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public User(string userName) : this(userName, Guid.NewGuid())
        {
        }

        public User(string userName, Guid guid)
        {
            UserName = userName;   
            UserGuid = guid;
        }
    }
}
