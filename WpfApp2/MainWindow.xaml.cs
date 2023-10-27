using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using WpfApp2.Language;
using WpfApp2.QuestEnv;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>  
    public partial class MainWindow : Window
    {
        private LanguageSwitcher _languageSwitcher;
        private QuestControler _questControler;
        private UserEnviroment.UserController _userController;
        private int _indexQuest = -1;
        private int _indexCurrentLetter = -1;
        private int _mistakesCount = 0;
        private int _typedCharactersCount;

        private DispatcherTimer _taskTimer;
        private DateTime _startTimeRound;
        private DateTime _finishTimeRound;
        private DateTime _typingStartTime;
        private TimeSpan _elapsedTimeRound;

        private List<Border> _buttons;

        private List<DificultyLevels> dificultyLevels = new List<DificultyLevels>
        {
            DificultyLevels.Just,
            DificultyLevels.Norm,
            DificultyLevels.Hard
        };
        public MainWindow()
        {
            InitializeComponent();
            _userController = new UserEnviroment.UserController();
            _questControler = new QuestControler();
            _taskTimer = new DispatcherTimer();
            _taskTimer.Interval = new TimeSpan(0, 0, 1);
            /*_languageSwitcher = new LanguageSwitcher(Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location
                ));*/

            _languageSwitcher = new LanguageSwitcher(@"..\..\Language\");
            foreach(var oneLang in _languageSwitcher.Langs)
            {
                ComboBog_Language.Items.Add(oneLang);
            }

            _buttons = new List<Border>();

            foreach (var OneChild in Grid_Keyboard.Children)
            {
                if (OneChild is Border)
                {

                    _buttons.Add((Border)OneChild);

                }
            }
            //ComboBox1.ItemsSource = Enum.GetValues(typeof(DificultyLevels)).Cast<DificultyLevels>();

           

            //ComboBox1.ItemsSource = dificultyLevels;
            ComboBox1.DisplayMemberPath = "ToString";
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            if (_taskTimer.IsEnabled) return;
            _taskTimer.Start();
            _startTimeRound = DateTime.Now;
            _typingStartTime = DateTime.Now;
            _typedCharactersCount = 0;
            Button_Start.IsEnabled = false;
            Button_Stop.IsEnabled = true;
            _indexQuest++;
            _indexCurrentLetter = 0;
            RichTextBox_Quest.Document.Blocks.Clear();

            DificultyLevels selectedDificulty = (DificultyLevels)ComboBox1.SelectedIndex;

            _questControler.UpdateQuests(selectedDificulty);
            RichTextBox_Quest.Document.Blocks.Add(new Paragraph(new Run(_questControler.GetQuestByDificultyLevel(_indexQuest, selectedDificulty).Text)));


        }


        private void Button_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (!_taskTimer.IsEnabled) return;

            _taskTimer.Stop();
            _finishTimeRound = DateTime.Now;
            _elapsedTimeRound = _finishTimeRound - _startTimeRound;
            Title = _elapsedTimeRound.TotalSeconds.ToString();
            Button_Start.IsEnabled = true;
            Button_Stop.IsEnabled = false;
            _indexQuest = -1;
            RichTextBox_Answer.Document.Blocks.Clear();

            _mistakesCount = 0;
            Label_Mistakes.Content = "0";
        }

        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_taskTimer.IsEnabled) return;
            int keyCode = Convert.ToInt32(e.Key);
            string keySymbol = e.Key.ToString();

            Title = keyCode.ToString();
            Title += " " + keySymbol;

            foreach (var button in _buttons)
            {
                if (button.Child is TextBlock)
                {
                    if (((TextBlock)button.Child).Text.ToLower().Equals(keySymbol.ToLower()))
                    {

                        if (button.Background != Brushes.FloralWhite)
                        {
                            Brush tempB = button.Background;
                            button.Background = Brushes.FloralWhite;
                            await Task.Delay(75);
                            button.Background = tempB;
                        }
                    }
                }
            }


            if (Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.None)
            {
                keySymbol = keySymbol.ToLower();
            }

            if (keySymbol.Length == 1 || keyCode == 18)
            {
                var endPos = RichTextBox_Quest.CaretPosition.GetNextInsertionPosition(LogicalDirection.Forward);

                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.CapsLock))
                {
                    keySymbol = keySymbol.ToUpper();
                }
                if (keyCode == 18)
                {
                    keySymbol = " ";
                }
                TextRange textRange = new TextRange(RichTextBox_Quest.CaretPosition, endPos);
                if (_questControler.GetQuestByDificultyLevel(_indexQuest, (DificultyLevels)ComboBox1.SelectedIndex).Text[_indexCurrentLetter] == keySymbol[0])
                {
                    textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Green);
                    RichTextBox_Quest.CaretPosition = endPos;
                    RichTextBox_Answer.AppendText(keySymbol);
                    _indexCurrentLetter++;
                }
                else
                {
                    textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Red);
                    _mistakesCount++;
                    Label_Mistakes.Content = $"{_mistakesCount}";
                }
                _typedCharactersCount++;

                TimeSpan elapsedTime = DateTime.Now - _typingStartTime;

                double cpm = (_typedCharactersCount / elapsedTime.TotalMinutes);

                Laleb_Speed.Content = $"{Math.Round(cpm)}";

            }
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DificultyLevels selectedDificulty = (DificultyLevels)ComboBox1.SelectedIndex;

            _questControler.UpdateQuests(selectedDificulty);

        }

        private void ButtonAuth_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow(_userController);
            authWindow.Owner = this;
            authWindow.ShowDialog();
            if(_userController.CurrentUser != null)
            {
                Label_user_info.Content = _userController.CurrentUser.Email.Split('@')[0];
                User_Results.IsEnabled = true;
                return;
            }
        }

        private void User_Results_Click(object sender, RoutedEventArgs e)
        {
            ResultWindow resultWindow = new ResultWindow(_userController);
            resultWindow.Owner = this;
            resultWindow.WindowStartupLocation = WindowStartupLocation.Manual;

            var locPoint = this.PointToScreen(new Point(0, 0));
            resultWindow.Left = locPoint.X+ this.Width - 23;
            resultWindow.Top = locPoint.Y - 31;

            resultWindow.Show();
        }

        private void ComboBog_Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(ComboBog_Language.SelectedItem.ToString());
            UpdateUILocale();
        }
        private void UpdateUILocale()
        {
            this.Title = Strings.MainWindow_Title;
            ButtonAuth.Content = Strings.ButtonAuth;
            User_Results.Content = Strings.User_Results;
            Button_Result.Content = Strings.Button_Result;
            Button_Start.Content = Strings.Button_Start;
            LabelSpeed_unit.Content = Strings.LabelSpeed_unit;
            Label_speed.Content = Strings.Label_speed;
            LabelStatus_Info.Content = Strings.LabelStatus_Info;
            Label_Fails.Content = Strings.Label_Fails;
        }
    }
}
