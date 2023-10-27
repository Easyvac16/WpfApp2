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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private UserEnviroment.UserController _userController;
        
        public AuthWindow(UserEnviroment.UserController userC)
        {
            _userController = userC;
            InitializeComponent();
        }

        private void Button_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_auth_Click(object sender, RoutedEventArgs e)
        {
            if(TextBox_Email.Text.Length <= 8)
            {
                Text_block_info.Text = "Insert Email";
                return;
            }
            if(PasswordBox_Password.Password.Length <= 7)
            {
                Text_block_info.Text = "Insert password";
                return;
            }
            User_Enviroment.User user = _userController.GetUser(TextBox_Email.Text,PasswordBox_Password.Password);
            if(user == null) 
            {
                Text_block_info.Text = "User info is not valid";
                return;
            }    
            _userController.CurrentUser = user;

            Close();
        }
    }
}
