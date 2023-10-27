using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.QuestEnv;
using WpfApp2.User_Enviroment;

namespace WpfApp2.UserEnviroment
{
    public class User_card
    {
        public Guid id { get; set; }

        public User User { get; set; }

        public List<Result> results { get; set; }  = new List<Result>();

        public User_card()
        {
            results = new List<Result>();
            id = Guid.NewGuid();
        }

        public Result GetResult(Quest quest)
        {
            var found = results.Where(res => res.Equals(quest));
            if (found.Count() == 0)
            {
                return null;
            }
            return found.ElementAt(0);
        }

        public bool TryAddResult(Result result)
        {
            if (results.Where(res => res.Quest.Equals(result.Quest)).Count() != 0)
            {
                return false;
            }
            results.Add(result);
            return true;
        }

        public bool TryUpdateResult(Result result)
        {
            if (results.Where(res => res.Quest.Equals(result.Quest)).Count() == 0)
            {
                return false;
            }
            int previousRes = results.FindIndex(res => res.Quest.Equals(result.Quest));
            if (result.Efective > results[previousRes].Efective)
            {
                results[previousRes] = result;
            }
            return true;
        }
    }
}
