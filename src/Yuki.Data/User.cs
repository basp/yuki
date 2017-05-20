namespace Yuki.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;

    public class User : IEntity
    {
        public User()
        {
            this.LastUpdated = DateTime.UtcNow;
        }

        public int Id
        {
            get;
            set;
        }

        [NotMapped]
        public string Subject
        {
            get => this.Email;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
        }

        public DateTime LastUpdated
        {
            get;
            private set;
        }

        public IEnumerable<Claim> GetClaims()
        {
            return new List<Claim>
            {
                new Claim("email", this.Email),
                new Claim("full_name", this.FullName),
            };
        }
    }
}
