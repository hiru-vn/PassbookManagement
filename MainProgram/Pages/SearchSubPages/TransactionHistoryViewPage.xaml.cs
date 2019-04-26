using DAO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainProgram.Pages.SearchSubPages
{
    /// <summary>
    /// Interaction logic for TransactionHistoryViewPage.xaml
    /// </summary>
    public partial class TransactionHistoryViewPage : Page
    {
        public TransactionHistoryViewPage()
        {
            InitializeComponent();
        }
        private void TextBox_Search_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty((sender as TextBox).Text))
            {
                this.Button_ClearText.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Button_ClearText.Visibility = Visibility.Visible;
            }
        }

        private void TextBox_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string content = this.Textbox_Search.Text;
                if (!string.IsNullOrEmpty(content))
                {
                    //search query
                }
            }
        }

        private void Button_ClearText_Click(object sender, RoutedEventArgs e)
        {
            this.Textbox_Search.Clear();
            (sender as Button).Visibility = Visibility.Hidden;
        }
        private void Textbox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_LoadList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cus_name = this.Textbox_Search.Text.Trim();
                this.ListView.ItemsSource = TransactionDAO.Instance.GetListTransaction(cus_name, this.DatePicker.SelectedDate);
            }
            catch
            {
                this.ListView.Items.Clear();
            }
        }
    }
}
