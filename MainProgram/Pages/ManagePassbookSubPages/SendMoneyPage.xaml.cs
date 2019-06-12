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
using MainProgram.CustomControls;
using DTO;
using DAO;
using MainProgram.Converter;
using System.Reflection;

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
        #region function
        void Default()
        {
            this.DatePicker_Time.SelectedDate = DateTime.Now;
        }
        private void Clearall()
        {
            this.Txt_CustomerID.Text = null;
            this.Txt_CustomerName.Text = null;
            this.Txt_CustomerCard.Text = null;
            this.Txt_CustomerAddress.Text = null;
            this.Cb_TypePassbook.ItemsSource = null;
            this.Txt_PassbookID.Text = null;
            this.Money.Text = null;
            this.Txt_CustomerID.Clear();
            this.Txt_CustomerName.Clear();
            this.Txt_CustomerCard.Clear();
            this.Txt_CustomerAddress.Clear();
            this.Cb_TypePassbook.Items.Clear();
            this.Txt_PassbookID.Clear();
            this.Money.Clear();
        }
        #endregion

        #region events
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
                    MessageBoxCustom.setContent("CustomerID is not available").ShowDialog();
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
                MessageBoxCustom.setContent("Người dùng nhập thiếu dữ liệu").ShowDialog();
                return;
            }
            else
            {
                if (CollectBillDAO.Instance.CheckCollectMoney(long.Parse(this.Money.Text.ToString()), (this.Cb_TypePassbook.SelectedItem as TypePassbook).Typename))
                {
                    MessageBoxCustom.setContent("Số tiên gởi không hợp lê").ShowDialog();
                    Clearall();
                    return;
                }
                if (CollectBillDAO.Instance.CheckCollectdate(this.DatePicker_Time.SelectedDate, int.Parse(Txt_PassbookID.Text.ToString())))
                {
                    MessageBoxCustom.setContent("Chưa đến ngày đáo hạn sổ, Ngày đáo hạn là: " + (PassbookDAO.Instance.GetWithdrawday(int.Parse(this.Txt_PassbookID.Text.ToString()))).Value.ToString("dd/MM/yyyy")).ShowDialog();
                    return;
                }
                CollectBill bill = new CollectBill
                {
                    Id = 1.ToString().Trim(),
                    Collect_passbook = int.Parse(Txt_PassbookID.Text),
                    Collect_money = long.Parse(Money.Text),
                    Collectdate = this.DatePicker_Time.SelectedDate ?? DateTime.Now
                };
                CollectBillDAO.Instance.InsertCollectBill(bill);
                MessageBoxCustom.setContent("Thêm phiếu gởi thành công").ShowDialog();   
                Clearall();
            }
        }
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            string name = "Bill"+ CollectBillDAO.Instance.GetLastBillID() + ".png";
            CaptureUIElement.Instance.SaveFrameworkElementToPng(Panel_Bill, (int)Panel_Bill.ActualWidth, (int)Panel_Bill.ActualHeight, name);
            MessageBoxCustom.setContent("phiếu lưu tại: " + System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).ShowDialog();
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
                    MessageBoxCustom.setContent("Mã khách hàng này không tồn tại!").ShowDialog();
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
        
        private void Txt_CustomerID_GotFocus(object sender, RoutedEventArgs e)
        {
            Clearall();
        }
        #endregion
    }
}

