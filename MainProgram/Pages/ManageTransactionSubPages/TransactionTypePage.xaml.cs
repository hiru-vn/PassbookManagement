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
        }
        void showTreeItem()
        {
            this.ListView_TransactionType.Items.Clear();
            List<TypeAccount> list = new List<TypeAccount>();
            list = TypeAccountDAO.Instance.GetListType();
            foreach (TypeAccount acc in list)
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                PackIcon icon = new PackIcon();
                icon.Kind = PackIconKind.Notebook;
                icon.Margin = new Thickness(-5, 0, 5, 0);
                TextBlock textBlock = new TextBlock();
                //textBlock.Text = acc;
                stackPanel.Children.Add(icon);
                stackPanel.Children.Add(textBlock);
                TreeViewItem item = new TreeViewItem();
                item.Header = stackPanel;
                item.Tag = acc;

                //this.Account_treeview.Items.Add(item);
            }

            //this.AccountFrame.Content = new AccountPage1();
        }

        private void Add_Type_Mode(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_type(object sender, RoutedEventArgs e)
        {
            if (MessageBoxCustom.setContent("Bạn thực sự muốn xóa loại tiết kiệm này?").ShowDialog() == true)
            {
                if (SavingAccountDAO.Instance.CheckIfExist)
            }
        }
    }
}
