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
        public List<CollectBill> GetListBill(string cusname, DateTime? date)
        {
            if (date == null)
            {
                //truy van gan dung voi %cusname%
                List<CollectBill> list = new List<CollectBill>();
                string query = "select * from dbo.collectbill where collect_passbook in(select passbook.id from dbo.passbook, dbo.customer where passbook.passbook_customer=customer.id and cus_name like '%" + cusname + "%')";
                DataTable data = DataProvider.Instance.ExcuteQuery(query);
                foreach (DataRow item in data.Rows)
                {
                    CollectBill result = new CollectBill(item);
                    list.Add(result);
                }
                return list;
                
            }
            else
            {
                //get list by cus name and transaction date
                //truy van gan dung voi %cusname%
                List<CollectBill> list = new List<CollectBill>();
                string query = "select * from dbo.collectbill where collect_passbook in(select passbook.id from dbo.passbook, dbo.customer where passbook.passbook_customer=customer.id and cus_name like '%" + cusname + "%') and day(collectdate)=day(" + date + ") and month(collectdate)=month(" + date + ") and year(collectdate)=year(" + date + ")";
                DataTable data = DataProvider.Instance.ExcuteQuery(query);
                foreach (DataRow item in data.Rows)
                {
                    CollectBill result = new CollectBill(item);
                    list.Add(result);
                }
                return list;
                
            }
        }   
    }
}
