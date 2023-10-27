using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace WpfApp2.QuestEnv
{
    [Serializable]
    public class QuestData
    {
        public DificultyLevels Level { get; set; }
        public string Text { get; set; }
    }
    class QuestControler
    {
        private Dictionary<DificultyLevels, List<Quest>> _quests;
        public DificultyLevels CurentDificultiLevel { get; set; } = DificultyLevels.Norm;



        public QuestControler()
        {
            _quests = new Dictionary<DificultyLevels, List<Quest>>();
            _quests.Add(DificultyLevels.Just, new List<Quest>());
            _quests.Add(DificultyLevels.Norm, new List<Quest>());
            _quests.Add(DificultyLevels.Hard, new List<Quest>());

            LoadQuest();
        }

        private void LoadQuest()
        {
            string json = File.ReadAllText("questData.json");
            List<QuestData> questDataList = JsonConvert.DeserializeObject<List<QuestData>>(json);

            foreach (var questData in questDataList)
            {
                _quests[questData.Level].Add(new Quest
                {
                    id = Guid.NewGuid(),
                    Text = questData.Text,
                    Level = questData.Level
                }
                );
            }
        }
        public void UpdateQuests(DificultyLevels selectedDificulty)
        {
            CurentDificultiLevel = selectedDificulty;
            
        }

        public List<Quest> Quests
        {
            get { return _quests[CurentDificultiLevel]; }
        }

        public Quest GetQuestByDificultyLevel(int index, DificultyLevels dificultyLevels)
        {

            return _quests[dificultyLevels][index];

        }

    }
}
