using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using MainProgram.Converter;
using MainProgram.CustomControls;

namespace MainProgram.Pages.ManagePassbookSubPages
{
    /// <summary>
    /// Interaction logic for CreatePassbookPage.xaml
    /// </summary>
    public partial class CreatePassbookPage : Page
    {
        public CreatePassbookPage()
        {
            InitializeComponent();
            Default();
        }
        #region functions
        void Default()
        {
            this.TextBox_PassbookID.Text = (PassbookDAO.Instance.GetMaxID() + 1).ToString();
            this.Combobox_TypePassbook.ItemsSource = TypePassbookDAO.Instance.GetListTypeName();
            this.DatePicker_DateOpen.SelectedDate = DateTime.Now;
            this.Combobox_TypePassbook.ItemsSource = TypePassbookDAO.Instance.GetListType();
            this.Combobox_TypePassbook.DisplayMemberPath = "Typename";
        }
        private void Clearall()
        {
            this.TextBox_CustomerID.Text = null;
            this.TextBox_CustomerName.Text = null;
            this.TextBox_CardID.Text = null;
            this.TextBox_Address.Text = null;
            this.TextBox_Money.Text = null;
            this.TextBox_CustomerID.Clear();
            this.TextBox_CustomerName.Clear();
            this.TextBox_CardID.Clear();
            this.TextBox_Address.Clear();
            this.TextBox_Money.Clear();
            this.Combobox_TypePassbook.ItemsSource = TypePassbookDAO.Instance.GetListType();
            this.Combobox_TypePassbook.DisplayMemberPath = "Typename";
        }
        public bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }
        #endregion

        #region events
        // new customer/ old customer change => change form format
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.RadioButton_NewCustomer.IsChecked == false)
            {
                this.TextBox_CustomerID.IsEnabled = true;
                this.TextBox_CustomerID.Text = "";
                Clearall();
            }
            else
            {
                this.TextBox_CustomerID.IsEnabled = false;
                this.TextBox_CustomerID.Text = (CustomerDAO.Instance.GetCurrentMaxCustomerID() + 1).ToString();
            }
        }
        // show customer info if old customer radio button is check and textboxcustomerID is valid
        // if textboxcustomerID is not valid, show warning
        private void TextBox_CustomerID_LostFocus(object sender, RoutedEventArgs e)
        {
            int customerID;
            if (int.TryParse(this.TextBox_CustomerID.Text,out customerID) == true)
            {
                customerID = int.Parse(this.TextBox_CustomerID.Text);
                bool exist_ID = CustomerDAO.Instance.CheckExistID(customerID);
                if (exist_ID)
                {
                    this.TextBox_warning_1.Visibility = Visibility.Collapsed;
                    this.TextBox_CustomerName.Text = CustomerDAO.Instance.GetCustomerName(customerID);
                    this.TextBox_CardID.Text = CustomerDAO.Instance.GetCustomerCardNumber(customerID);
                    this.TextBox_Address.Text = CustomerDAO.Instance.GetCustomerAddress(customerID);
                }
                else
                {
                    this.TextBox_warning_1.Visibility = Visibility.Visible;
                }
            }
        }
        // apply for numberic textbox
        private void Numberic_TextBox(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
                if (!Char.IsDigit(ch))
                    e.Handled = true;
        }
        // apply for money textbox
        private void Money_TextBox(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
                if (!Char.IsDigit(ch) || ch == ',')
                    e.Handled = true;
        }
        //capture the grid
        private void Button_Print_Clicked(object sender, RoutedEventArgs e)
        {
            string name = "Bill" + CollectBillDAO.Instance.GetLastBillID() + ".png";
            CaptureUIElement.Instance.SaveFrameworkElementToPng(Panel_Bill, (int)Panel_Bill.ActualWidth, (int)Panel_Bill.ActualHeight, name);
            MessageBoxCustom.setContent("phiếu lưu tại: "+ System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).ShowDialog();
        }
        //check if the customer has had an account type before, if not, create new passbook, if yes, show messagebox warning
        private void Button_OpenPassbook(object sender, RoutedEventArgs e)
        {
            int IDcustomer = int.Parse(this.TextBox_CustomerID.Text);
            Customer customer = new Customer
            {
                Cus_address = TextBox_Address.Text.Trim(),
                Cus_name = TextBox_CustomerName.Text.Trim(),
                Cmnd = TextBox_CardID.Text.Trim()
            };
            if (this.RadioButton_NewCustomer.IsChecked == true)
            {               
                if (string.IsNullOrEmpty(customer.Cus_name) || string.IsNullOrEmpty(customer.Cmnd))
                {
                    MessageBoxCustom.setContent("Chưa điền đầy đủ thông tin khách hàng");
                    return;
                }
                if (PassbookDAO.Instance.CheckBalance(long.Parse(this.TextBox_Money.Text.ToString()), (this.Combobox_TypePassbook.SelectedItem as TypePassbook).Typename))
                {
                    MessageBoxCustom.setContent("Số tiền gởi ban đầu không hợp lệ ").ShowDialog();
                    Clearall();
                    this.TextBox_CustomerID.Text = (CustomerDAO.Instance.GetCurrentMaxCustomerID() + 1).ToString();
                    return;
                }
                CustomerDAO.Instance.InsertCustomer(customer);
            }
            int? idType = (this.Combobox_TypePassbook.SelectedItem as TypePassbook).Id;
            bool Check = CustomerDAO.Instance.CheckCustomerHasAccountType(IDcustomer, idType);
            if (!Check) //cus dont have any active passbook of this kind
            {
                if (idType != null)
                {
                    Passbook pass = new Passbook
                    {
                        Passbook_customer = IDcustomer,
                        Opendate = this.DatePicker_DateOpen.SelectedDate ?? DateTime.Now,
                        Passbooktype = idType ?? -1,
                        Passbook_balance = 0
                    };
                    PassbookDAO.Instance.InsertPassbook(pass);
                    CollectBill bill = new CollectBill
                    {
                        Id = 1.ToString(),
                        Collect_passbook = int.Parse(this.TextBox_PassbookID.Text.ToString()),
                        Collect_money = long.Parse(this.TextBox_Money.Text.ToString()),
                        Collectdate = this.DatePicker_DateOpen.SelectedDate ?? DateTime.Now
                    };
                    CollectBillDAO.Instance.InsertCollectBill(bill);
                    MessageBoxCustom.setContent("Thêm sổ thành công").ShowDialog();
                    Clearall();
                    this.TextBox_PassbookID.Text = (PassbookDAO.Instance.GetMaxID() + 1).ToString();
                    this.TextBox_Money.Clear();
                    if (this.RadioButton_NewCustomer.IsChecked == true) this.TextBox_CustomerID.Text = (CustomerDAO.Instance.GetCurrentMaxCustomerID() + 1).ToString();


                }
            }
            else
            {
                MessageBoxCustom.setContent("Lỗi, khách hàng này đã có tài khoản thuộc loại " + (this.Combobox_TypePassbook.SelectedItem as TypePassbook).Typename + " còn thời hạn.").ShowDialog();
            }
        }
       
        private void TextBox_CustomerID_GotFocus(object sender, RoutedEventArgs e)
        {
            Clearall();
        }

        private void RadioButton_NewCustomer_GotFocus(object sender, RoutedEventArgs e)
        {
            Clearall();
        }
        private void TextBox_CardID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TextBox_CardID.Text))
                return;
            else
            {
                if (this.TextBox_CardID.Text.Length == 9)
                {
                    if (IsNumber(this.TextBox_CardID.Text))
                        if (CustomerDAO.Instance.CheckCardIDexist(this.TextBox_CardID.Text))
                            return;
                        else
                        {
                            MessageBoxCustom.setContent("Số CMND đã tồn tại. Vui lòng nhập lại").ShowDialog();
                            this.TextBox_CardID.Clear();
                            return;
                        }   
                    
                    
                }
                MessageBoxCustom.setContent("Số CMND chưa đúng, Vui lòng nhập lại").ShowDialog();
               this.TextBox_CardID.Clear();
            }
            
        }
        #endregion
    }
}
