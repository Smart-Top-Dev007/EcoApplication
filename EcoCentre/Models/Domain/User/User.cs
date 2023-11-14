using System;
using System.Web.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EcoCentre.Models.Domain.User
{
	[BsonIgnoreExtraElements]
	public class User : Entity
    {
        public string Login { get; protected set; }
        public string LoginLower { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime LastLoginAt { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsGlobalAdmin { get; set; }
        public string Email { get; set; }
        public string EmailResetKey { get; set; }
        public bool IsReadOnly { get; set; }
        public string Name { get; set; }
	    public DateTime? LastSeenOnline { get; set; }
	    public DateTime? LogoutDate { get; set; }
	    public string LastSeenOnlineOnHub { get; set; }
	    public bool IsLoginAlertEnabled { get; set; }


	    public static User Create(string login, string password, string email)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (password == null) throw new ArgumentNullException(nameof(password));
            var result = new User
                {
                    Login = login.Trim(),
                    IsAdmin = false,
                    IsGlobalAdmin = false,
                    Email = email,
                    CreatedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow
                };
            result.LoginLower = result.Login.ToLower();
            result.UpdatePassword(password);
            return result;
        }
        public void MakeAdmin()
        {
            IsAdmin = true;
        }
        public void MakeGlobalAdmin()
        {
            IsGlobalAdmin = true;
        }
        public void UpdatePassword(string password)
        {
            Salt = Crypto.GenerateSalt();
            Password = Crypto.HashPassword(password + Salt);
        }

        public bool VerifyPassword(string password )
        {
            return Crypto.VerifyHashedPassword(Password, password + Salt);
        }

        public void UpdateLogin(string login)
        {
            Login = login.Trim();
            LoginLower = Login.ToLowerInvariant();
        }

        public void ResetPasswordRequest()
        {
            EmailResetKey = Crypto.GenerateSalt();
        }

        public void ResetPassword(string key, string password)
        {
            if(EmailResetKey != key)
                throw new Exception("Invalid reset key");
            UpdatePassword(password);
            EmailResetKey = null;
        }
    }
}