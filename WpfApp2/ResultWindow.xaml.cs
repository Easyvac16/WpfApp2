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
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        private UserController _userController;

        public ResultWindow(UserController userController)
        {
            InitializeComponent();
            _userController = userController;

            _initAVGs();
            _initResults();
        }

        private void _initAVGs()
        {
            List<Result> results = _userController.CurrentUser.User_Card.results.ToList();
            int countJust = 0, countNorm = 0, countDiff = 0;
            int failsJust = 0, failsNorm = 0, failsDiff = 0;
            double speedJust = 0, speedNorm = 0, speedDiff = 0;
            double effectiveJust = 0, effectiveNorm = 0, effectiveDiff = 0;
            foreach (Result result in results)
            {
                switch (result.Quest.Level)
                {
                    case QuestEnv.DificultyLevels.Just:
                        countJust++;
                        failsJust += result.FailCount;
                        speedJust += result.SpeedTest;
                        effectiveJust += result.Efective;
                        break;
                    case QuestEnv.DificultyLevels.Norm:
                        countNorm++;
                        failsNorm += result.FailCount;
                        speedNorm += result.SpeedTest;
                        effectiveNorm += result.Efective;
                        break;
                    case QuestEnv.DificultyLevels.Hard:
                        countDiff++;
                        failsDiff += result.FailCount;
                        speedNorm += result.SpeedTest;
                        effectiveDiff += result.Efective;
                        break;
                }
            }
            if (countJust > 0)
            {
                failsJust = (int)(failsJust / (double)countJust);
                speedJust = (int)(speedJust / (double)countJust);
                effectiveJust /= countJust;
            }
            if (countNorm > 0)
            {
                failsNorm = (int)(failsNorm / (double)countNorm);
                speedNorm = (int)(speedNorm / (double)countNorm);
                effectiveNorm /= countNorm;
            }
            if (countDiff > 0)
            {
                failsDiff = (int)(failsDiff / (double)countDiff);
                speedDiff = (int)(speedDiff / (double)countDiff);
                effectiveDiff /= countDiff;
            }
            ListBox_JustAVG.Items.Add($"Fails: {failsJust}");
            ListBox_JustAVG.Items.Add($"Speed: {speedJust}");
            ListBox_JustAVG.Items.Add($"Effective: {Math.Round(effectiveJust, 2)}");

            ListBox_NormAVG.Items.Add($"Fails: {failsNorm}");
            ListBox_NormAVG.Items.Add($"Speed: {speedNorm}");
            ListBox_NormAVG.Items.Add($"Effective: {Math.Round(effectiveNorm, 2)}");

            ListBox_DifficultAVG.Items.Add($"Fails: {failsDiff}");
            ListBox_DifficultAVG.Items.Add($"Speed: {speedDiff}");
            ListBox_DifficultAVG.Items.Add($"Effective: {Math.Round(effectiveDiff, 2)}");
        }

        private void _initResults()
        {
            List<Result> results = _userController.CurrentUser.User_Card.results.ToList();
            foreach (Result result in results)
            {
                switch (result.Quest.Level)
                {
                    case QuestEnv.DificultyLevels.Just:
                        ListBox_Just.Items.Add(result);
                        break;
                    case QuestEnv.DificultyLevels.Norm:
                        ListBox_Norm.Items.Add(result);
                        break;
                    case QuestEnv.DificultyLevels.Hard:
                        ListBox_Difficult.Items.Add(result);
                        break;
                }
            }
        }
    }
}
