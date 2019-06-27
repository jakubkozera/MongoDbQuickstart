using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;
using Project.Common.Types;


namespace Project.Core
{
    public class User : IIdentifiable
    {
        public User(string name, IEnumerable<string> roles)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Username cannot be empty");

            Name = name;
            Email = name;
            EntityId = Guid.NewGuid();
            Roles = roles.Where(Role.IsValid).ToArray();
        }

        [BsonElement("roles")]
        public string[] Roles { get; set; }

        [BsonElement("entityId")]
        public Guid EntityId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }

        public void SetPassword(string password, IPasswordHasher<User> passwordHasher)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Password can not be empty.");
            }

            PasswordHash = passwordHasher.HashPassword(this, password);
        }
    }
}
