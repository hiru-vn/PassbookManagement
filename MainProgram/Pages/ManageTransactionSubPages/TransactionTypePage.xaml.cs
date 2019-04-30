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
            showTreeItem();
        }
        void showTreeItem()
        {
            this.ListView_TransactionType.Items.Clear();
            List<TypePassbook> list = new List<TypePassbook>();
            list = TypePassbookDAO.Instance.GetListType();
            foreach (TypePassbook type in list)
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                PackIcon icon = new PackIcon();
                icon.Kind = PackIconKind.Notebook;
                icon.Margin = new Thickness(-5, 0, 5, 0);
                TextBlock textBlock = new TextBlock();
                textBlock.Text = type.Typename;
                stackPanel.Children.Add(icon);
                stackPanel.Children.Add(textBlock);
                TreeViewItem item = new TreeViewItem();
                item.Header = stackPanel;
                item.Tag = type;

                this.ListView_TransactionType.Items.Add(item);
            }
        }
        void Look_Type_Mode()
        {
            this.Texblock_title.Text = "Thông tin";

        }

        private void Add_Type_Mode(object sender, RoutedEventArgs e)
        {
            this.Texblock_title.Text = "Thêm mới loại tiết kiệm";
            this.Button_Add.Visibility = Visibility.Visible;
        }

        private void Delete_type(object sender, RoutedEventArgs e)
        {
            if (MessageBoxCustom.setContent("Bạn thực sự muốn xóa loại tiết kiệm này?").ShowDialog() == true)
            {
                if (this.ListView_TransactionType.Items.Count > 0)
                {
                    TypePassbook typepassbook =(TypePassbook) (this.ListView_TransactionType.SelectedItem as TreeViewItem).Tag;
                    if (!TypePassbookDAO.Instance.CheckIfExistActivePassbookInType(typepassbook.Id))
                    {
                        TypePassbookDAO.Instance.DeleteType(typepassbook.Id);
                        showTreeItem();
                    }
                }
            }
        }

        private void Listview_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (this.ListView_TransactionType.Items.Count > 0)
            {
                TypePassbook typepassbook = (TypePassbook)(this.ListView_TransactionType.SelectedItem as TreeViewItem).Tag;
                this.TextBox_MinMoney.Text = typepassbook.Min_passbookblance.ToString();
                this.TextBox_MinCollectDay.Text = typepassbook.Min_collectmoney.ToString();
                this.TextBox_InterestRate.Text = (typepassbook.Interest_rate*100).ToString();
                this.TextBox_Term.Text = typepassbook.Term.ToString();
            }
        }

        private void Add_Type(object sender, RoutedEventArgs e)
        {
            TypePassbook type = new TypePassbook();
            type.Min_passbookblance = int.Parse(this.TextBox_MinMoney.Text);
            type.Interest_rate = float.Parse(this.TextBox_InterestRate.Text)/100;
            if (this.RadioButton_Noterm.IsChecked==true)
                type.Term = 0;
            else
            {
                type.Term = int.Parse(this.TextBox_Term.Text);
            }
            type.Min_collectmoney = int.Parse(this.TextBox_MinCollectDay.Text);
            TypePassbookDAO.Instance.InsertType(type);
            Look_Type_Mode();
        }

        private void Change_Term_Type(object sender, RoutedEventArgs e)
        {
            if (this.RadioButton_Noterm.IsChecked == true)
            {
                this.Stackpanel_term.Visibility = Visibility.Collapsed;
            }
            else if (this.RadioButton_Yesterm.IsChecked == false)
            {
                this.Stackpanel_term.Visibility = Visibility.Visible;
            }
        }
    }
}
