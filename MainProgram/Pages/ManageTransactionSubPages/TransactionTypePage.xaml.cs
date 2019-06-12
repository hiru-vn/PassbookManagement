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
using MainProgram.CustomControls;
using MaterialDesignThemes.Wpf;

namespace MainProgram.Pages.ManageTransactionSubPages
{
    /// <summary>
    /// Interaction logic for TransactionTypePage.xaml
    /// </summary>
    public partial class TransactionTypePage : Page
    {
        public TransactionTypePage()
        {
            InitializeComponent();
            ShowTreeItem();
        }
        #region functions
        void LoadListTreeItem()
        {
            //get list typepassbook and show them on listview
            this.ListView_TransactionType.Items.Clear();
            List<TypePassbook> list = new List<TypePassbook>();
            list = TypePassbookDAO.Instance.GetListType();
            foreach (TypePassbook type in list)
            {
                StackPanel stackPanel = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Orientation = Orientation.Horizontal
                };
                PackIcon icon = new PackIcon
                {
                    Kind = PackIconKind.Notebook,
                    Margin = new Thickness(-5, 0, 5, 0),
                    Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF31577E"))
                };
                TextBlock textBlock = new TextBlock
                {
                    Text = type.Typename
                };
                stackPanel.Children.Add(icon);
                stackPanel.Children.Add(textBlock);
                ListViewItem item = new ListViewItem
                {
                    Content = stackPanel,
                    Tag = type
                };

                this.ListView_TransactionType.Items.Add(item);
            }
        }
        void Look_Type_Mode()
        {
            //set view for user to look
            this.Button_Fix.Visibility = Visibility.Visible;
            this.Texblock_title.Text = "Thông tin";
            if (this.ListView_TransactionType.Items.Count > 0)
                this.ListView_TransactionType.SelectedIndex = 0;
        }
        void ShowTreeItem()
        {
            //default view
            LoadListTreeItem();
            Look_Type_Mode();
        }
        void SetReadOnly(bool flag)
        {
            //change textbox to prevent user change unpermission properties in database
            if (flag)
            {
                this.TextBox_MinBalanceMoney.IsReadOnly = true;
                this.TextBox_MinWithdrawDay.IsReadOnly = true;
                this.TextBox_MinCollectMoney.IsReadOnly = true;
                this.TextBox_InterestRate.IsReadOnly = true;
            }
            else
            {
                this.TextBox_MinBalanceMoney.IsReadOnly = false;
                this.TextBox_MinWithdrawDay.IsReadOnly = false;
                this.TextBox_MinCollectMoney.IsReadOnly = false;
                this.TextBox_InterestRate.IsReadOnly = false;
            }
        }
        void SetTermTypeMode(bool flag)
        {
            //view for term typepassbook or not
            try
            {
                if (flag)
                {
                    this.Stackpanel_term.Visibility = Visibility.Visible;
                    this.Stackpanel_MinWithdrawday.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.Stackpanel_term.Visibility = Visibility.Collapsed;
                    this.Stackpanel_MinWithdrawday.Visibility = Visibility.Visible;
                }
            }
            catch { }
        }
        void enableRadioButton(bool flag)
        {
            //enable radiobutton to prevent user change unpermission properties in database
            if (flag)
            {
                this.RadioButton_Noterm.IsHitTestVisible = true;
                this.RadioButton_Yesterm.IsHitTestVisible = true;
            }
            else
            {
                this.RadioButton_Noterm.IsHitTestVisible = false;
                this.RadioButton_Yesterm.IsHitTestVisible = false;
            }
        }
        #endregion

        #region events
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
        private void Listview_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (this.ListView_TransactionType.Items.Count > 0)
            {
                TypePassbook typepassbook = (TypePassbook)(this.ListView_TransactionType.SelectedItem as ListViewItem).Tag;
                this.TextBox_MinBalanceMoney.Text = typepassbook.Min_passbookblance.ToString();
                this.TextBox_MinWithdrawDay.Text = typepassbook.Withdrawterm.ToString();
                this.TextBox_MinCollectMoney.Text = typepassbook.Min_collectmoney.ToString();
                this.TextBox_InterestRate.Text = (typepassbook.Interest_rate * 100).ToString();
                this.TextBox_Term.Text = typepassbook.Term.ToString();
                this.Button_Add.Visibility = Visibility.Collapsed;
                this.Button_Save.Visibility = Visibility.Collapsed;
                this.Button_Fix.Visibility = Visibility.Visible;
                if (typepassbook.Term > 0)
                {
                    this.RadioButton_Yesterm.IsChecked = true;
                    SetTermTypeMode(true);
                }
                else
                {
                    this.RadioButton_Noterm.IsChecked = true;
                    SetTermTypeMode(false);
                }
                this.Texblock_title.Text = "Thông tin";
                SetReadOnly(true);
                enableRadioButton(false);
            }
        }


        private void Add_Type_Mode(object sender, RoutedEventArgs e)
        {
            SetReadOnly(false);
            enableRadioButton(true);
            this.TextBox_Term.IsReadOnly = false;
            this.Texblock_title.Text = "Thêm mới loại tiết kiệm";
            this.Button_Add.Visibility = Visibility.Visible;
            this.Button_Fix.Visibility = Visibility.Collapsed;
            this.Button_Save.Visibility = Visibility.Collapsed;
            this.RadioButton_Noterm.IsChecked = true;
            this.RadioButton_Yesterm.IsChecked = true;

        }
        private void Add_Type(object sender, RoutedEventArgs e)
        {
            TypePassbook type = new TypePassbook
            {
                Min_passbookblance = int.Parse(this.TextBox_MinBalanceMoney.Text),
                Interest_rate = float.Parse(this.TextBox_InterestRate.Text) / 100
            };
            if (this.RadioButton_Noterm.IsChecked == true)
            {
                type.Term = 0;
                type.Withdrawterm = int.Parse(this.TextBox_MinWithdrawDay.Text.ToString());
            }
            else
            {
                type.Term = int.Parse(this.TextBox_Term.Text);
            }
            type.Min_collectmoney = int.Parse(this.TextBox_MinCollectMoney.Text);
            try { TypePassbookDAO.Instance.InsertType(type); }
            catch { MessageBoxCustom.setContent("Trùng loại tiết kiệm").ShowDialog(); }
            SetReadOnly(true);
            enableRadioButton(false);
            this.TextBox_Term.IsReadOnly = true;
            ShowTreeItem();
        }
        private void Fix_Type(object sender, RoutedEventArgs e)
        {
            this.Button_Save.Visibility = Visibility.Visible;
            this.Button_Fix.Visibility = Visibility.Collapsed;
            SetReadOnly(false);
        }

        private void Save_Type(object sender, RoutedEventArgs e)
        {
            this.Button_Save.Visibility = Visibility.Collapsed;
            this.Button_Fix.Visibility = Visibility.Visible;
            SetReadOnly(true);

            TypePassbook type;
            try { type = (this.ListView_TransactionType.SelectedItem as ListViewItem).Tag as TypePassbook; } catch { type = new TypePassbook(); }
            if (type != null)
            {
                long minBalanceMoney = long.Parse(this.TextBox_MinBalanceMoney.Text);
                long minCollectMoney = long.Parse(this.TextBox_MinCollectMoney.Text);
                float interestRate = float.Parse(this.TextBox_InterestRate.Text) / 100;
                int minWithDrawDay = int.Parse(this.TextBox_MinWithdrawDay.Text);
                TypePassbookDAO.Instance.UpdateType(type.Id, minCollectMoney, minBalanceMoney, interestRate, minWithDrawDay);
            }
            LoadListTreeItem();
        }

        private void Delete_type(object sender, RoutedEventArgs e)
        {
            if (MessageBoxCustom.setContent("Bạn thực sự muốn xóa loại tiết kiệm này?").ShowDialog() == true)
            {
                if (this.ListView_TransactionType.Items.Count > 0)
                {
                    TypePassbook typepassbook = (TypePassbook)(this.ListView_TransactionType.SelectedItem as ListViewItem).Tag;
                    if (!TypePassbookDAO.Instance.CheckIfExistActivePassbookInType(typepassbook.Id))
                    {
                        TypePassbookDAO.Instance.DeleteType(typepassbook.Id);
                        ShowTreeItem();
                    }
                    else
                    {
                        MessageBoxCustom.setContent("Loại tiết kiệm này vẫn còn sổ mở").ShowDialog();
                    }
                }
            }
        }

        private void Change_Term_Type(object sender, RoutedEventArgs e)
        {
            if (this.RadioButton_Noterm.IsChecked == true)
            {
                SetTermTypeMode(false);
            }
            else if (this.RadioButton_Yesterm.IsChecked == true)
            {
                SetTermTypeMode(true);
            }
        }
        #endregion
    }
}
