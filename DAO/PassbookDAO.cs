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
        public int GetMaxID()
        {
            int value=0;
            return value;
        }
        public Passbook GetAccount(int ID)
        {
            string query = string.Format("select * from dbo.savingaccount where id = {0}", ID);
            DataRow row = DataProvider.Instance.ExcuteQuery(query).Rows[0];
            Passbook acc = new Passbook(row);
            return acc;
        }
        public DataTable GetPassInfoByCusName(string CusName)
        {
            //tìm kiếm gần đúng %CusName%
            //trả về bảng gồm các hàng STT, IDPassbook, CustomerName, PassbookType, Balance, DateOpenPassbook, WithDrawDate (WithDrawDate là ngày mở + thời hạn rút, nếu không có thời hạn trả về null)
            string query = "select ROW_NUMBER() over(order by passbook.id), passbook.id,cus_name,typename,passbook_balance,opendate,withdrawday from dbo.passbook, dbo.customer, dbo.typepassbook where passbook_customer = customer.id and passbook_type = typepassbook.id and cus_name like '%" + CusName + "%'";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;    
        }
        public DataTable GetPassInfoByPassID(int PassbookID)
        {
            //tìm kiếm chính xác ID
            //trả về bảng gồm 1 hàng STT, IDPassbook, CustomerName, PassbookType, Balance, DateOpenPassbook, WithDrawDate (WithDrawDate là ngày mở + thời hạn rút, nếu không có thời hạn thì ghi là --)
            string query = "select ROW_NUMBER() over(order by passbook.id), passbook.id,cus_name,typename,passbook_balance,opendate,withdrawday from dbo.passbook, dbo.customer, dbo.typepassbook where passbook_customer = customer.id and passbook_type = typepassbook.id and passbook.id like '%" + PassbookID + "%'";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            return data;
        }
        public long GetBalanceMoneyByCollectBillID(string BillID)
        {
            return 0;
        }
        public string GetPassbookTypeNameByCollectBillID(string BillID)
        {
            return "";
        }
        public long GetBalanceMoneyByWithdrawBillID(string BillID)
        {
            return 0;
        }
        public string GetPassbookTypeNameByWithdrawBillID(string BillID)
        {
            return "";
        }
        public long GetBalanceMoneyByCollectBillID(string BillID)
        {
            return 0;
        }
        public string GetPassbookTypeNameByCollectBillID(string BillID)
        {
            return "";
        }
        public long GetBalanceMoneyByWithdrawBillID(string BillID)
        {
            return 0;
        }
        public string GetPassbookTypeNameByWithdrawBillID(string BillID)
        {
            return "";
        }
        public long GetBalanceMoneyByCollectBillID(string BillID)
        {
            return 0;
        }
        public string GetPassbookTypeNameByCollectBillID(string BillID)
        {
            return "";
        }
        public long GetBalanceMoneyByWithdrawBillID(string BillID)
        {
            return 0;
        }
        public string GetPassbookTypeNameByWithdrawBillID(string BillID)
        {
            return "";
        }
    }
}
