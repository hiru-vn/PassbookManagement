using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class CollectBillDAO
    {
        private static CollectBillDAO instance;

        public static CollectBillDAO Instance
        {
            get { if (instance == null) instance = new CollectBillDAO(); return instance; }
            private set { instance = value; }
        }
        private CollectBillDAO() { }
        public bool CheckIfExistBillID(string idBill)
        {
            bool check = false;
            int result = (int)DataProvider.Instance.ExcuteScarar("select count(*) from dbo.collectbill where id=" + idBill);
            if (result != 0)
                check = true;
            return check;
        }
        public CollectBill GetBill(string id)
        {
            string query = string.Format("select * from dbo.collectbill where id = '{0}'", id);
            DataRow row = DataProvider.Instance.ExcuteQuery(query).Rows[0];
            CollectBill bill = new CollectBill(row);
            return bill;
        }
    }
}
