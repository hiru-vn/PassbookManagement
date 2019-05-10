using MainProgram.Pages;
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

namespace MainProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TransactionDAO.Instance.UpdatePassbookBalance();
        }

        private void Close_Application(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "PageSystem":
                    FramePage.Source = new Uri("Pages/SystemPage.xaml", UriKind.Relative);
                    break; 
                case "PageCreate":
                    FramePage.Source = new Uri("Pages/ManagePassbookPage.xaml", UriKind.Relative);
                    break;
                case "PageManage":
                    FramePage.Source = new Uri("Pages/ManageTransactionPage.xaml", UriKind.Relative);
                    break;
                case "PageSearch":
                    FramePage.Source = new Uri("Pages/SearchPage.xaml", UriKind.Relative);
                    break;
                case "PageReport":
                    FramePage.Source = new Uri("Pages/ReportPage.xaml", UriKind.Relative);
                    break;
                case "PageHelp":
                    FramePage.Source = new Uri("Pages/HelpPage.xaml", UriKind.Relative);
                    break;
                default:
                    break;
            }
        }
    }
}
