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

            string query = "select ROW_NUMBER() over(order by typename) STT , typename TypePassbook , collect MoneyIncome , withdraw MoneyOutcome, abs(collect-withdraw) Difference from (select typepassbook.id as idc,case when collectmoney1 is null then 0 else collectmoney1 end as collect,case when withdrawmoney1 is null then 0 else withdrawmoney1 end as 'withdraw' from (select id1, collectmoney1, withdrawmoney1 from (select typepassbook.id as 'id1',sum(collectmoney) 'collectmoney1' from dbo.collectbill, dbo.typepassbook, dbo.passbook where day(collectdate)=day(" + date + ") and month(collectdate)=month(" + date + ") and year(collectdate)=year(" + date + ") and passbook.id=collect_passbook and passbook_type=typepassbook.id group by typepassbook.id) as a full join (select typepassbook.id 'id2',sum(withdrawmoney) 'withdrawmoney1' from dbo.withdrawbill, dbo.typepassbook, dbo.passbook where day(withdrawdate)=day(" + date + ") and month(withdrawdate)=month(" + date + ") and year(withdrawdate)=year(" + date + ") and passbook.id=withdraw_passbook and passbook_type=typepassbook.id group by typepassbook.id) as b on (id1=id2)) c full join dbo.typepassbook on (typepassbook.id=id1)) as d, dbo.typepassbook where idc=typepassbook.id";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;
        }
        public DataTable GetMonthlyReport(int month, int year, int typeid)
        {
            //trả về bảng gồm các cột được đặt tên: STT, TypePassbook, MoneyIncome, MoneyOutcome, Difference
            // ý nghĩa: stt, loại tiết kiệm, tiền nộp vô trong ngày, tiền rút ra trong ngày, chênh lệch lượng tiền tính bằng số dương của tháng {0} năm {1} và có typeid là {2}
            string query = "select ROW_NUMBER() over(order by typename) STT , typename TypePassbook , collect MoneyIncome , withdraw MoneyOutcome, abs(collect-withdraw) Difference from (select typepassbook.id as idc,case when collectmoney1 is null then 0 else collectmoney1 end as collect,case when withdrawmoney1 is null then 0 else withdrawmoney1 end as 'withdraw' from (select id1, collectmoney1, withdrawmoney1 from (select typepassbook.id as 'id1',sum(collectmoney) 'collectmoney1' from dbo.collectbill, dbo.typepassbook, dbo.passbook where month(collectdate)=" + month + " and year(collectdate)=" + year + " and passbook.id=collect_passbook and passbook_type=typepassbook.id group by typepassbook.id) as a full join (select typepassbook.id 'id2',sum(withdrawmoney) 'withdrawmoney1' from dbo.withdrawbill, dbo.typepassbook, dbo.passbook where month(withdrawdate)=" + month + " and year(withdrawdate)=" + year + " and passbook.id=withdraw_passbook and passbook_type=typepassbook.id group by typepassbook.id) as b on (id1=id2)) c full join dbo.typepassbook on (typepassbook.id=id1)) as d, dbo.typepassbook where idc=typepassbook.id and typepassbook.id=" + typeid;
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;
        }
    }
}
