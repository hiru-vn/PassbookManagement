using System;
using System.Collections.Generic;
using System.Data;
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
using DevExpress.Charts;
using DevExpress.Xpf.Charts;
using DAO;

namespace MainProgram.Pages.ReportSubPages
{
    /// <summary>
    /// Interaction logic for ChartPage.xaml
    /// </summary>
    public partial class ChartPage : Page
    {
        public ChartPage()
        {
            InitializeComponent();
        }

        void GetDailyChartData()
        {
            XYDiagram2D diagram2D = new XYDiagram2D();
            BarSideBySideSeries2D barIn = new BarSideBySideSeries2D();
            BarSideBySideSeries2D barOut = new BarSideBySideSeries2D();

            //List<DataTable> dataTables = new List<DataTable>();
            DateTime recent = DateTime.Now;
            for (int i=0;i<5;i++)
            {
                DataTable table = ReportDAO.Instance.GetDailyReport(recent);
                recent = recent.AddDays(-1);
                foreach (DataRow row in table.Rows)
                {

                }
            }

        }
        void GetMonthlyChartData()
        {
            BarSideBySideSeries2D barIn = new BarSideBySideSeries2D();
            BarSideBySideSeries2D barOut = new BarSideBySideSeries2D();

            DateTime recent = DateTime.Now;
            for (int i = 0; i <5;i++)
            {

            }
        }

        private void SetChartDailyReport(object sender, RoutedEventArgs e)
        {
            this.ChartDailyReport.Visibility = Visibility.Visible;
            this.ChartMonthlyReport.Visibility = Visibility.Collapsed;
        }

        private void SetChartMonthlyReport(object sender, RoutedEventArgs e)
        {
            this.ChartMonthlyReport.Visibility = Visibility.Visible;
            this.ChartDailyReport.Visibility = Visibility.Collapsed;
        }
    }
}
