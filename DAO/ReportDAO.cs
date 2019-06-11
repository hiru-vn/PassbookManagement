using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class ReportDAO
    {
        private static ReportDAO instance;

        public static ReportDAO Instance
        {
            get { if (instance == null) instance = new ReportDAO(); return instance; }
            private set { instance = value; }
        }
        private ReportDAO() { }
        public DataTable GetDailyReport(DateTime date)
        {
            //trả về bảng gồm các cột được đặt tên: STT, TypePassbook, MoneyIncome, MoneyOutcome, Difference
            // ý nghĩa: stt, loại tiết kiệm, tiền nộp vô trong ngày, tiền rút ra trong ngày, chênh lệch lượng tiền tính bằng số dương
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;
            string query = "usp_ReportTypePassbookDay " + day + ", " + month + ", " + year;
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;
        }
        public DataTable GetMonthlyReport(int month, int year, int typeid)
        {
            //trả về bảng gồm các cột được đặt tên: STT, PassIn, PassOut, Difference
            // ý nghĩa: stt, loại tiết kiệm, tiền nộp vô trong ngày, tiền rút ra trong ngày, chênh lệch lượng tiền tính bằng số dương của tháng {0} năm {1} và có typeid là {2}
            string query = "usp_ReportTypePassbookMonth " + month + ", " + year + ", " + typeid;
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;
        }
        public Tuple<int,int> GetCountOpenClosePassbook(int month, int year, int typeid)
        {
            string query = "usp_ReportChartTypePassbookMonth " + month + ", " + year + ", " + typeid;
            DataRow data = DataProvider.Instance.ExcuteQuery(query).Rows[0];
            int open =int.Parse(data["openP"].ToString());
            int close = int.Parse(data["closeP"].ToString());
            Tuple<int, int> result = new Tuple<int, int>(open,close);

            return result;
        }
        public Tuple<int,int> GetIncomeOutcomeMoney(int month, int year, int typeid)
        {
            string query = "usp_ReportChartTypePassbookDay " + month + ", " + year + ", " + typeid;
            DataRow data = DataProvider.Instance.ExcuteQuery(query).Rows[0];
            int Collect = int.Parse(data["Collect"].ToString());
            int Withdraw = int.Parse(data["Withdraw"].ToString());
            Tuple<int, int> result = new Tuple<int, int>(Collect, Withdraw);

            return result;
        }
    }
}
