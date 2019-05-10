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
            Default();
        }

        void Default()
        {
            this.DatePicker_Time.SelectedDate = DateTime.Now;
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
            if (string.IsNullOrEmpty(this.Money.Text))
            {
                MessageBox.Show("Người dùng nhập thiếu dữ liệu");
                return;
            }
            else
            {
                CollectBill bill = new CollectBill();
                bill.Id = 1.ToString().Trim();
                bill.Collect_passbook = int.Parse(Txt_PassbookID.Text);
                bill.Collect_money = long.Parse(Money.Text);
                bill.Collectdate = this.DatePicker_Time.SelectedDate ?? DateTime.Now;
                CollectBillDAO.Instance.InsertCollectBill(bill);
                MessageBox.Show("Thêm phiếu gởi thành công");
            }



            //    String PassbookID = this.Txt_PassbookID.Text;
            //    string AccountType = this.Cb_TypePassbook.SelectedValue.ToString();
            //    PassbookDAO.Instance.SendMoney(PassbookID, AccountType, int.Parse(this.Money.Text));
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            //CaptureUIElement.Instance.SaveFrameworkElementToPng(Grid_BillInfo, 200, 200, "MyImage.png");

        }

        private void Txt_CustomerID_LostFocus(object sender, RoutedEventArgs e)
        {
            int customerID;
            if (int.TryParse(this.Txt_CustomerID.Text, out customerID) == true)
            {
                customerID = int.Parse(this.Txt_CustomerID.Text);
                bool exist_ID = CustomerDAO.Instance.CheckExistID(customerID);
                if (exist_ID)
                {

                    this.TextBox_warning_1.Visibility = Visibility.Collapsed;
                    this.Txt_CustomerName.Text = CustomerDAO.Instance.GetCustomerName(customerID);
                    this.Txt_CustomerCard.Text = CustomerDAO.Instance.GetCustomerCardNumber(customerID);
                    this.Txt_CustomerAddress.Text = CustomerDAO.Instance.GetCustomerAddress(customerID);
                    this.Money.Clear();
                    this.Txt_PassbookID.Clear();
                    this.Cb_TypePassbook.ItemsSource = null;
                    this.Cb_TypePassbook.Items.Clear();
                    this.Cb_TypePassbook.ItemsSource = TypePassbookDAO.Instance.GetListTypeByCusID(customerID);
                    this.Cb_TypePassbook.DisplayMemberPath = "Typename";

                }
                else
                {
                    this.TextBox_warning_1.Visibility = Visibility.Visible;
                    MessageBox.Show("Mã khách hàng này không tồn tại!");
                    this.Txt_CustomerID.Clear();
                }
            }

        }

        private void Cb_TypePassbook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int customerID;
            if (int.TryParse(this.Txt_CustomerID.Text, out customerID) == true)
            {
                if (this.Cb_TypePassbook.ItemsSource == null)
                    return;
                else
                {
                    ComboBox cb = sender as ComboBox;
                    if (cb.SelectedItem != null)
                    {
                        customerID = int.Parse(this.Txt_CustomerID.Text);
                        TypePassbook type = cb.SelectedItem as TypePassbook;
                        string name = type.Typename;

                        this.Txt_PassbookID.Text = PassbookDAO.Instance.GetPassbookIDbyCusIDandidType(customerID, name).ToString();
                    }
                }
            }
        }

    }

}

