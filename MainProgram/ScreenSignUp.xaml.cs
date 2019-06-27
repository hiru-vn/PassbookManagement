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
    /// Interaction logic for ScreenSignUp.xaml
    /// </summary>
    public partial class ScreenSignUp : Window
    {
        public ScreenSignUp()
        {
            InitializeComponent();
        }
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string Pass = Password.Password;
            string RePass = RePassword.Password;
            //Kiem tra username co ton tai
            if (!System.IO.File.Exists("user.txt"))
                System.IO.File.Create("user.txt");
            string[] user_pass_strs = System.IO.File.ReadAllLines("user.txt");
            bool is_ok = true;
            foreach (var line in user_pass_strs)
            {
                string[] sub = line.Split('|');
                if (username.CompareTo(sub[0]) == 0)
                {
                    is_ok = false;
                    break;
                }
            }
            if (!is_ok)
            {
                MessageBox.Show("Tài khoản đã tồn tại ", "Lỗi!");
            }
            else
            {
                if (Pass.CompareTo(RePass) == 0)
                {
                    using (MD5 md5 = MD5.Create())
                    {
                        string hash = GetMd5Hash(md5, Pass);
                        System.IO.StreamWriter str = System.IO.File.AppendText("user.txt");
                        str.WriteLine(username + '|' + hash);
                        str.Close();
                        MessageBox.Show("Đăng kí thành công", "Thông báo!");
                        this.Hide();
                        ScreenLogin Scrlogin = new ScreenLogin();
                        Scrlogin.ShowDialog();
                        this.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Password không giống nhau! Vui lòng nhập lại!");
                }

            }

        }
    }
    
}
