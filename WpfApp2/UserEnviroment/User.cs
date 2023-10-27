using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.UserEnviroment;

namespace WpfApp2.User_Enviroment
{
    public class User
    {
        public Guid id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public User_card User_Card { get; set; } = null;
    }
}
