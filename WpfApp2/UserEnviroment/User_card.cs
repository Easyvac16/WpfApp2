using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.User_Enviroment;

namespace WpfApp2.UserEnviroment
{
    public class User_card
    {
        public Guid id { get; set; }

        public User User { get; set; }

        public List<Result> results { get; set; }  = new List<Result>();
    }
}
