using DAO;
using DevExpress.Xpf.Charts;
using DTO;
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

namespace MainProgram.CustomControls
{
    /// <summary>
    /// Interaction logic for ChartDailyReport.xaml
    /// </summary>
    public partial class ChartDailyReport : UserControl
    {
        private DateTime _currentCharDate = DateTime.Now;
        public ChartDailyReport()
        {
            InitializeComponent();
            GetListType();
            setDefault();
        }

        #region functions
        void setDefault()
        {
            DateTime flagtime = _currentCharDate;
            for (int i = 0; i < 8; i++)
            {
                SeriesPoint pointOpen = new SeriesPoint
                {
                    Argument = "Tháng " + flagtime.Month.ToString() + "/" + flagtime.Year.ToString(),
                    Value = 0
                };
                this.Incom.Points.Insert(0, pointOpen);
                SeriesPoint pointClose = new SeriesPoint
                {
                    Argument = "Tháng " + flagtime.Month.ToString() + "/" + flagtime.Year.ToString(),
                    Value = 0
                };
                this.Outcome.Points.Insert(0, pointClose);
                flagtime = flagtime.AddMonths(-1);
            }
        }
        void GetChart()
        {
            if (this.Combobox_type.SelectedItem != null)
            {
                this.Outcome.Points.Clear();
                this.Incom.Points.Clear();
                DateTime flagtime = _currentCharDate;
                for (int i = 0; i < 8; i++)
                {
                    Tuple<int, int> pair = ReportDAO.Instance.GetIncomeOutcomeMoney(flagtime.Month, flagtime.Year, (this.Combobox_type.SelectedItem as TypePassbook).Id);
                    SeriesPoint pointIn = new SeriesPoint
                    {
                        Argument = "Tháng " + flagtime.Month.ToString() + "/" + flagtime.Year.ToString(),
                        Value = pair.Item1
                    };
                    this.Incom.Points.Insert(0, pointIn);
                    SeriesPoint pointOut = new SeriesPoint
                    {
                        Argument = "Tháng " + flagtime.Month.ToString() + "/" + flagtime.Year.ToString(),
                        Value = pair.Item2
                    };
                    this.Outcome.Points.Insert(0, pointOut);
                    flagtime = flagtime.AddMonths(-1);
                }
            }
        }
        void GetListType()
        {
            List<TypePassbook> listtype = TypePassbookDAO.Instance.GetListType();
            this.Combobox_type.ItemsSource = listtype;
            this.Combobox_type.DisplayMemberPath = "Typename";
        }
        #endregion

        #region events
        private void MoveChartBackward(object sender, RoutedEventArgs e)
        {
            this._currentCharDate = this._currentCharDate.AddMonths(-1);
            GetChart();
            this.Button_MoveForward.IsEnabled = true;
        }

        private void MoveChartForward(object sender, RoutedEventArgs e)
        {
            this._currentCharDate = this._currentCharDate.AddMonths(1);
            GetChart();
            if (_currentCharDate > DateTime.Now) this.Button_MoveForward.IsEnabled = false;
        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            GetChart();
        }
        #endregion
    }
}
