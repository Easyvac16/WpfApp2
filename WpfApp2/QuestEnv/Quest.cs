using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.QuestEnv
{
    [Serializable]
    public class Quest
    {
        public Guid id { get; set; }
        
        public string Text { get; set; }

        public DificultyLevels Level { get; set; }
    }
}
