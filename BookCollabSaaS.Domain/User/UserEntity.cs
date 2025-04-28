using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using BookCollabSaaS.Domain.Subscription;

namespace BookCollabSaaS.Domain.User
{
    public class UserEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string? PasswordHash { get; private set; }
        public string? PasswordSalt { get; private set; }
        public SubscriptionEntity? Subscription { get; private set; }
        public List<RoleEntity> Roles { get; private set; } = new();

        private UserEntity() { }

        public UserEntity(string name, string email, string password)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            SetPassword(password);
        }

        public void UpdateName(string newName)
        {
            Name = newName ?? throw new ArgumentNullException(nameof(newName));
        }

        public void UpdateEmail(string newEmail)
        {
            Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            PasswordSalt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
            PasswordHash = HashPassword(password, PasswordSalt);
        }

        public bool VerifyPassword(string password)
        {
            if (string.IsNullOrEmpty(PasswordSalt) || string.IsNullOrEmpty(PasswordHash))
                return false;

            var hash = HashPassword(password, PasswordSalt);
            return hash == PasswordHash;
        }

        private static string HashPassword(string password, string salt)
        {
            var combined = password + salt;
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(combined);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public void AddRole(RoleEntity role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            if (Roles.Any(r => r.Name == role.Name))
                return; // Already has the role, no need to add again

            Roles.Add(role);
        }

        public void SetSubscription(SubscriptionEntity subscription)
        {
            Subscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
        }
    }
}
