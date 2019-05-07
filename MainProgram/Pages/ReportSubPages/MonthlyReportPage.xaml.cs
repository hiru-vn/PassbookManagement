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

namespace MainProgram.Pages.ReportSubPages
{
    /// <summary>
    /// Interaction logic for MonthlyReportPage.xaml
    /// </summary>
    public partial class MonthlyReportPage : Page
    {
        public MonthlyReportPage()
        {
            InitializeComponent();
            SetMonthYear();
            SetTypeBox();
        }
        void SetMonthYear()
        {
            int thisyear = DateTime.Now.Year;
            List<int> listyear = new List<int>();
            List<int> listmonth = new List<int>();
            for (int i= thisyear - 30; i<= thisyear; i++)
            {
                listyear.Add(i);
            }
            for (int i = 1; i<=12;i++)
            {
                listmonth.Add(i);
            }
            this.Combobox_year.ItemsSource = listyear;
            this.Combobox_year.SelectedItem = thisyear;
            this.Combobox_Month.ItemsSource = listmonth;
            this.Combobox_Month.SelectedItem = DateTime.Now.Month;
        }
        void SetTypeBox()
        {
            List<TypePassbook> listtype = TypePassbookDAO.Instance.GetListType();
            this.Combobox_type.ItemsSource = listtype;
            this.Combobox_type.DisplayMemberPath = "Typename";
            if (this.Combobox_type.Items.Count>0) this.Combobox_type.SelectedIndex = 0;
        }
        private void GetReport(object sender, SelectionChangedEventArgs e)
        {
            if (this.Combobox_Month.Items.Count > 0 && this.Combobox_type.Items.Count > 0 && this.Combobox_year.Items.Count > 0)
            {
                int month = (int)this.Combobox_Month.SelectedItem;
                int year = (int)this.Combobox_year.SelectedItem;
                int typeid = (this.Combobox_type.SelectedItem as TypePassbook).Id;
                this.ListView.ItemsSource = ReportDAO.Instance.GetMonthlyReport(month, year, typeid).DefaultView;
            }
        }
    }
}
