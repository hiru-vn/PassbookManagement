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
using DTO;
using DAO;

namespace MainProgram.Pages.ManagePassbookSubPages
{
    /// <summary>
    /// Interaction logic for SendMoneyPage.xaml
    /// </summary>
    public partial class SendMoneyPage : Page
    {
        public SendMoneyPage()
        {
            InitializeComponent();
        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            int customerID;
            if (int.TryParse(this.Txt_CustomerID.Text, out customerID) == true)
            {
                customerID = int.Parse(this.Txt_CustomerID.Text);
                bool exist_ID = CustomerDAO.Instance.CheckExistID(customerID);
                if (exist_ID)
                {
                    this.Txt_CustomerName.Text = CustomerDAO.Instance.GetCustomerName(customerID);
                    this.Txt_CustomerCard.Text = CustomerDAO.Instance.GetCustomerCardNumber(customerID);
                    this.Txt_CustomerAddress.Text = CustomerDAO.Instance.GetCustomerAddress(customerID);
                    this.DatePicker_Time.SelectedDate = DateTime.Now;
                }
                else
                {
                    MessageBox.Show("CustomerID is not available");
                }
            }
        }

        private void Numberic_txtbox(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
                if (!Char.IsDigit(ch))
                    e.Handled = true;
        }

        private void BtnSMoney_Click(object sender, RoutedEventArgs e)
        {
            String PassbookID = this.Txt_PassbookID.Text;
            string AccountType = this.Cb_TypePassbook.SelectedValue.ToString();
            PassbookDAO.Instance.SendMoney(PassbookID, AccountType, int.Parse(this.Money.Text));
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            //CaptureUIElement.Instance.SaveFrameworkElementToPng(Grid_BillInfo, 200, 200, "MyImage.png");
        }
    }
}
