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

        void Default()
        {
            this.TextBox_PassbookID.Text = (SavingAccountDAO.Instance.GetMaxID() + 1).ToString();
            this.Combobox_TypePassbook.ItemsSource = TypeAccountDAO.Instance.GetListTypeName();
            this.DatePicker_DateOpen.SelectedDate = DateTime.Now;
        }
        // new customer/ old customer change => change form format
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.RadioButton_NewCustomer.IsChecked == false)
            {
                this.TextBox_CustomerID.IsEnabled = true;
                this.TextBox_CustomerID.Text = "";
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
            CaptureUIElement.Instance.SaveFrameworkElementToPng(Grid_BillInfo, 200, 200, "MyImage.png");
        }
        //check if the customer has had an account type before, if not, create new passbook, if yes, show messagebox warning
        private void Button_OpenPassbook(object sender, RoutedEventArgs e)
        {
            int IDcustomer = int.Parse(this.TextBox_CustomerID.Text);
            if (this.RadioButton_NewCustomer.IsChecked == true)
            {
                //CustomerDAO.Instance.InsertCustomer();
            }
            string AccountType = this.Combobox_TypePassbook.SelectedValue.ToString();
            bool Check = CustomerDAO.Instance.CheckCustomerHasAccountType(IDcustomer, AccountType);
            if (Check)
            {
                //SavingAccountDAO.Instance.InsertAccount();
            }
            else
            {
                MessageBoxCustom.setContent("Lỗi, khách hàng này đã có tài khoản thuộc loại " + this.Combobox_TypePassbook.SelectedValue.ToString() + " còn thời hạn.").ShowDialog();
            }
        }
    }
}
