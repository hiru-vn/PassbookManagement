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
      
            int result = (int)DataProvider.Instance.ExcuteScarar("select count(*) from dbo.withdrawbill where id=" + idBill);
            if (result != 0)
                return true;
            return false;

        }
        public WithdrawBill GetBill(string id)
        {
            string query = string.Format("select * from dbo.withdrawbill where id = '{0}'", id);
            DataRow row = DataProvider.Instance.ExcuteQuery(query).Rows[0];
            WithdrawBill bill = new WithdrawBill(row);
            return bill;
        }
        public List<WithdrawBill> GetListBill(string cusname, DateTime? date)
        {
            if (date == null)
            {
                //get list by cusname
                //truy van gan dung voi %cusname%
            }
            else
            {
                //get list by cus name and transaction date
                //truy van gan dung voi %cusname%
            }
            return null;
        }
    }
}
