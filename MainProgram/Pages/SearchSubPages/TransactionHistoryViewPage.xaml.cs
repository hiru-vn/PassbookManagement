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
        #region function

        #endregion

        #region events
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
                string name = this.Textbox_Search.Text.Trim();
                if (this.DatePicker.SelectedDate == null)
                {
                    this.ListView.ItemsSource = TransactionDAO.Instance.GetSearchTransactionByCustomerName(name).DefaultView;
                }
                else
                {
                    this.ListView.ItemsSource = TransactionDAO.Instance.GetSearchTransactionByCustomerNameAndDate(name,this.DatePicker.SelectedDate??DateTime.MinValue).DefaultView;
                }
            }
        }

        private void Button_ClearText_Click(object sender, RoutedEventArgs e)
        {
            this.Textbox_Search.Clear();
            (sender as Button).Visibility = Visibility.Hidden;
        }

        private void Button_LoadList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string cus_name = this.Textbox_Search.Text.Trim();
                //this.ListView.ItemsSource = TransactionDAO.Instance.GetListTransaction(cus_name, this.DatePicker.SelectedDate);
                string name = this.Textbox_Search.Text.Trim();
                if (this.DatePicker.SelectedDate == null)
                {
                    this.ListView.ItemsSource = TransactionDAO.Instance.GetSearchTransactionByCustomerName(name).DefaultView;
                }
                else
                {
                    this.ListView.ItemsSource = TransactionDAO.Instance.GetSearchTransactionByCustomerNameAndDate(name, this.DatePicker.SelectedDate ?? DateTime.MinValue).DefaultView;
                }
            }
            catch
            {
                this.ListView.Items.Clear();
            }
        }
        #endregion
    }
}
