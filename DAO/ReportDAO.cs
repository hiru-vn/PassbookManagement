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
            // ý nghĩa: stt, loại tiết kiệm, tiền nộp vô trong ngày, tiền rút ra trong ngày, chênh lệch lượng tiền tính bằng số dươngy
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;
            string query = "exec usp_ReportTypePassbookDay " + day + ", " + month + ", " + year;
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;
        }
        public DataTable GetMonthlyReport(int month, int year, int typeid)
        {
            //trả về bảng gồm các cột được đặt tên: STT, TypePassbook, MoneyIncome, MoneyOutcome, Difference
            // ý nghĩa: stt, loại tiết kiệm, tiền nộp vô trong ngày, tiền rút ra trong ngày, chênh lệch lượng tiền tính bằng số dương của tháng {0} năm {1} và có typeid là {2}
            string query = "exec usp_ReportTypePassbookMonth " + month + ", " + year + ", " + typeid;
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;
        }
    }
}
