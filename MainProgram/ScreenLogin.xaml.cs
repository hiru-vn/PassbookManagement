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
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
namespace MainProgram
{
    /// <summary>
    /// Interaction logic for ScreenLogin.xaml
    /// </summary>
    public partial class ScreenLogin : Window
    {
        public ScreenLogin()
        {
            InitializeComponent();
        }
      
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string Pass = Password.Password;
            if (!System.IO.File.Exists("user.txt"))
            {
                System.IO.File.Create("user.txt");
                return;
            }
            string[] user_pass_strs = System.IO.File.ReadAllLines("user.txt");
            bool is_ok = false;
            foreach (var line in user_pass_strs)
            {
                string[] sub = line.Split('|');
                using (MD5 md5 = MD5.Create())
                {
                    bool a = ScreenSignUp.VerifyMd5Hash(md5, Pass, sub[1]);
                    if (username.CompareTo(sub[0]) == 0 && ScreenSignUp.VerifyMd5Hash(md5, Pass, sub[1]))
                    {
                        is_ok = true;
                        break;
                    }
                }
            }
            if (is_ok)
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo");
                this.Hide();
                MainWindow Main = new MainWindow();
                Main.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng! Vui lòng nhập lại!", "Thông báo");
            }
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            
            ScreenSignUp SU = new ScreenSignUp();
            SU.ShowDialog();
            this.Hide();
        }
    }
}
