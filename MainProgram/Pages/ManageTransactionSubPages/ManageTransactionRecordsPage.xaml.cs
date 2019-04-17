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

namespace MainProgram.Pages.ManageTransactionSubPages
{
    /// <summary>
    /// Interaction logic for ManageTransactionRecordsPage.xaml
    /// </summary>
    public partial class ManageTransactionRecordsPage : Page
    {
        public ManageTransactionRecordsPage()
        {
            InitializeComponent();
        }

        private void Delete_Transaction(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("bạn muốn xóa giao dịch này?","",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                //xoa giao dich
                //cap nhat giao dich
            }
        }

        private void Update_Transaction(object sender, RoutedEventArgs e)
        {
            //cap nhat giao dich
        }

        private void Search_Transaction(object sender, RoutedEventArgs e)
        {
            Frame frame = Application.Current.MainWindow.FindName("FramePage") as Frame;
            frame.Source = new System.Uri("Pages/SearchPage.xaml", UriKind.Relative);
            frame.Content = new SearchPage();
            SearchPage page = frame.Content as SearchPage;
            page.TabControl.SelectedIndex = 2;
        }
    }
}
