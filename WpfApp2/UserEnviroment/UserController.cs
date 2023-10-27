using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using WpfApp2.User_Enviroment;

namespace WpfApp2.UserEnviroment
{
    public class UserController
    {
        private List<User> _users;
        public string EncryptedPassword;
        public User CurrentUser { get; set; }
        public UserController()
        {
            _users = new List<User>();
            LoadUsers();
        }
        private void LoadUsers()
        {
            string json = File.ReadAllText("users.json");
            _users = JsonConvert.DeserializeObject<List<User>>(json);
        }
        public User GetUser(string email, string enteredPassword)
        {
            string encryptedEnteredPassword = EncryptPassword(enteredPassword);
            return _users.Find(us => us.Email.ToLower() == email.ToLower() && us.Password == encryptedEnteredPassword);
        }
        private string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
