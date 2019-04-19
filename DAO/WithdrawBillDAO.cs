using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class WithdrawBillDAO
    {
        private static WithdrawBillDAO instance;

        public static WithdrawBillDAO Instance
        {
            get { if (instance == null) instance = new WithdrawBillDAO(); return instance; }
            private set { instance = value; }
        }
        private WithdrawBillDAO() { }
        public bool CheckIfExistBillID(string idBill)
        {
            bool check = false;
            //code
            return check;
        }
        public WithdrawBill GetBill(string id)
        {
            string query = string.Format("select * from dbo.withdrawbill where id = '{0}'", id);
            DataRow row = DataProvider.Instance.ExcuteQuery(query).Rows[0];
            WithdrawBill bill = new WithdrawBill(row);
            return bill;
        }
    }
}
