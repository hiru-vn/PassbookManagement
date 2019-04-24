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

namespace MainProgram.Pages.ReportSubPages
{
    /// <summary>
    /// Interaction logic for DailyReportPage.xaml
    /// </summary>
    public partial class DailyReportPage : Page
    {
        public DailyReportPage()
        {
            InitializeComponent();
        }

        private void GetReport(object sender, SelectionChangedEventArgs e)
        {
            Calendar calendar = sender as Calendar;
            DateTime date = calendar.SelectedDate??DateTime.MinValue;
            this.ListView.ItemsSource = ReportDAO.Instance.GetDailyReport(date).DefaultView;
        }
    }
}
