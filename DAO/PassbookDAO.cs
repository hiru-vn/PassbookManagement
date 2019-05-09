using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class PassbookDAO
    {
        private static PassbookDAO instance;

        public static PassbookDAO Instance
        {
            get { if (instance == null) instance = new PassbookDAO(); return instance; }
            private set { instance = value; }
        }
        private PassbookDAO() { }

        public Passbook GetAccount(int ID)
        {
            string query = string.Format("select * from dbo.passbook where id = {0}", ID);
            DataRow row = DataProvider.Instance.ExcuteQuery(query).Rows[0];
            Passbook acc = new Passbook(row);
            return acc;
        }
        public DataTable GetPassInfoByCusName(string CusName)
        {
            //tìm kiếm gần đúng %CusName%
            //trả về bảng gồm các hàng STT, IDPassbook, CustomerName, PassbookType, Balance, DateOpenPassbook, WithDrawDate (WithDrawDate là ngày mở + thời hạn rút, nếu không có thời hạn trả về null)
            string query = "select ROW_NUMBER() over(order by passbook.id) STT, passbook.id IDPassbook, cus_name CustomerName, typename PassbookType, passbook_balance Balance, opendate DateOpenPassbook, withdrawday WithDrawDate from dbo.passbook, dbo.customer, dbo.typepassbook where passbook_customer = customer.id and passbook_type = typepassbook.id and status=1 and cus_name like '%" + CusName + "%'";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;    
        }
        public DataTable GetPassInfoByPassID(int PassbookID)
        {
            //tìm kiếm chính xác ID
            //trả về bảng gồm 1 hàng STT, IDPassbook, CustomerName, PassbookType, Balance, DateOpenPassbook, WithDrawDate (WithDrawDate là ngày mở + thời hạn rút, nếu không có thời hạn thì ghi là --)
            string query = "select ROW_NUMBER() over(order by passbook.id) STT, passbook.id IDPassbook, cus_name CustomerName, typename PassbookType, passbook_balance Balance, opendate DateOpenPassbook, withdrawday WithDrawDate from dbo.passbook, dbo.customer, dbo.typepassbook where passbook_customer = customer.id and passbook_type = typepassbook.id and status=1 and passbook.id like '%" + PassbookID + "%'";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;
        }
        public long GetBalanceMoneyByCollectBillID(string BillID)
        {
            string query = "select passbook_balance from dbo.passbook, dbo.collectbill where collect_passbook =passbook.id and collectbill.id='" + BillID + "'";
            long result = (long)DataProvider.Instance.ExcuteScarar(query);
            return result;
        }
        public string GetPassbookTypeNameByCollectBillID(string BillID)
        {


            string query = "select typename from dbo.typepassbook, dbo.passbook, dbo.collectbill where collect_passbook= passbook.id and passbook.passbook_type = typepassbook.id and collectbill.id='" + BillID + "'";
            string  result =(string)DataProvider.Instance.ExcuteScarar(query).ToString();
            return result;
        }
        public long GetBalanceMoneyByWithdrawBillID(string BillID)
        {
            string query = "select passbook_balance from dbo.passbook, dbo.withdrawbill where withdraw_passbook =passbook.id and withdrawbill.id='" + BillID + "'";
            long result = (long)DataProvider.Instance.ExcuteScarar(query);
            return result;
        }
        public string GetPassbookTypeNameByWithdrawBillID(string BillID)
        {
            string query = "select typename from dbo.typepassbook, dbo.passbook, dbo.withdrawbill where withdraw_passbook= passbook.id and passbook.passbook_type = typepassbook.id and withdrawbill.id='" + BillID + "'";
            string result = (string)DataProvider.Instance.ExcuteScarar(query).ToString();
            return result;
        }
        public void InsertPassbook(Passbook passbook)
        {
            int type = passbook.Passbooktype;
            long balance = passbook.Passbook_balance;
            int cusid = passbook.Passbook_customer;
            DateTime? opendate = passbook.Opendate;
            if (opendate != null)
            {
                string query = string.Format("exec usp_InsertPassbook {0} , {1} , {2} , '{3}' ", type, balance, cusid, opendate.Value.ToString("yyyy/MM/dd"));
                DataProvider.Instance.ExcuteNonQuery(query);
            }
            else
                DataProvider.Instance.ExcuteNonQuery("usp_InsertPassbook1 @type , @balance , @cusid", new object[] { type, balance, cusid });
            
            //opendate,type,balance,customer

        }
        public int GetMaxID()
        {
            int max = 0;
            max = (int)DataProvider.Instance.ExcuteScarar("select max(id) from dbo.Passbook ");
            return max;
        }
        public void SendMoney(string s, string ss, int sss)
        {
            //do not code in here
        }
        public int GetPassbookIDbyCusIDandidType(int cusID, string Typename)
        {
            string query = "select passbook.id from dbo.passbook where passbook_customer=" + cusID + " and passbook_type in(select id from dbo.typepassbook where typename=N'" + Typename + "')";
            int value = (int)DataProvider.Instance.ExcuteScarar(query);
            return value;
        }
    }
}
