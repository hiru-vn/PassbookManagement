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
using DAO;
using DTO;
using MainProgram.CustomControls;

namespace MainProgram.Pages.ManageTransactionSubPages
{
    /// <summary>
    /// Interaction logic for ManageTransactionRecordsPage.xaml
    /// </summary>
    public partial class ManageTransactionRecordsPage : Page
    {
        bool IsWithdrawBill = false;
        public ManageTransactionRecordsPage()
        {
            InitializeComponent();
        }
        void ClearPage()
        {
            foreach(TextBlock control in this.PanelForm.Children)
            {
                control.Text = "";
            }
        }
        private void Delete_Transaction(object sender, RoutedEventArgs e)
        {
            if (MessageBoxCustom.setContent("bạn muốn xóa giao dịch này?").ShowDialog() == true)
            {

                //cap nhat giao dich
                ClearPage();
            }
        }

        private void Update_Transaction(object sender, RoutedEventArgs e)
        {
            //cap nhat giao dich
            if (!string.IsNullOrEmpty(TextBox_SavingBookID.Text.Trim())) {
                string idBill = TextBox_SavingBookID.Text.Trim();

                if (CollectBillDAO.Instance.CheckIfExistBillID(idBill) == true)
                {

                }
                else if (WithdrawBillDAO.Instance.CheckIfExistBillID(idBill) == true)
                {

                }
            }
            //cap nhat khach hang
            if (!string.IsNullOrEmpty(this.TextBox_CustomerID.Text.Trim()))
            {
                Customer cus = new Customer();
                cus.Id = int.Parse(this.TextBox_CustomerID.Text);
                cus.Name = this.TextBox_CustomerName.Text.Trim();
                cus.Cmnd = this.TextBox_CustomerIDcard.Text.Trim();
                cus.Cus_address = this.TextBox_CustomerAddress.Text.Trim();
                CustomerDAO.Instance.UpdateCustomer(cus);
            }

        }

        private void Search_Transaction(object sender, RoutedEventArgs e)
        {
            Frame frame = Application.Current.MainWindow.FindName("FramePage") as Frame;
            frame.Source = new System.Uri("Pages/SearchPage.xaml", UriKind.Relative);
            frame.Content = new SearchPage();
            SearchPage page = frame.Content as SearchPage;
            page.TabControl.SelectedIndex = 2;
        }

        private void View_Transaction(object sender, RoutedEventArgs e)
        {
            string idBill = this.Textbox_Search.Text;
            if (CollectBillDAO.Instance.CheckIfExistBillID(idBill) == true)
            {
                IsWithdrawBill = false;
                CollectBill bill = CollectBillDAO.Instance.GetBill(idBill);
                //SavingAccount acc = SavingAccountDAO.Instance.GetAccount(id);
                //Customer customerInfo = CustomerDAO.Instance.GetCustomer(id);
                //update form

            }
            else if (WithdrawBillDAO.Instance.CheckIfExistBillID(idBill) == true)
            {
                IsWithdrawBill = true;
                WithdrawBill bill = WithdrawBillDAO.Instance.GetBill(idBill);
                //SavingAccount acc = SavingAccountDAO.Instance.GetAccount(id);
                //Customer customerInfo = CustomerDAO.Instance.GetCustomer(id);
                //update form
            }
            else
            {
                MessageBoxCustom.setContent("Không tìm thấy mã giao dịch này.").ShowDialog();
            }
        }
        private void Money_TextBox(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
                if (!Char.IsDigit(ch) || ch == ',')
                    e.Handled = true;
        }
    }
}
