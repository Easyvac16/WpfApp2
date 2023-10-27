using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp2.UserEnviroment;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for ResultUserWindow.xaml
    /// </summary>
    public partial class ResultUserWindow : Window
    {
        private UserController _userController;
        public ResultUserWindow(UserController userController)
        {
            InitializeComponent();
            _userController = userController;
            _initResults();
        }

        private void _initResults()
        {
            List<Result> results = _userController.CurrentUser.User_Card.results.ToList();
            foreach (Result result in results)
            {
                switch (result.Quest.Level)
                {
                    case QuestEnv.DificultyLevels.Just:
                        ListBox_Fails.Items.Add(result.FailCount);
                        ListBox_Speed.Items.Add(result.SpeedTest);
                        ListBox_Time.Items.Add(result.Efective);
                        break;
                    case QuestEnv.DificultyLevels.Norm:
                        ListBox_Speed.Items.Add(result);
                        break;
                    case QuestEnv.DificultyLevels.Hard:
                        ListBox_Fails.Items.Add(result.FailCount);
                        break;
                }
            }
        }
    }
}
