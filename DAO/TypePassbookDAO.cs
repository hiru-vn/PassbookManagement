using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Data;

namespace DAO
{
    public class TypePassbookDAO
    {
        private static TypePassbookDAO instance;

        public static TypePassbookDAO Instance
        {
            get { if (instance == null) instance = new TypePassbookDAO(); return instance; }
            private set { instance = value; }
        }
        private TypePassbookDAO() { }
        public List<string> GetListTypeName()
        {
            List<string> list = new List<string>();
            string query = "select * from dbo.typepassbook";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                TypePassbook type = new TypePassbook(item);
                list.Add(type.Typename);
            }
            return list;
        }
        public List<TypePassbook> GetListType()
        {
            List<TypePassbook> list = new List<TypePassbook>();
            string query = "select * from dbo.typepassbook";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                TypePassbook type = new TypePassbook(item);
                list.Add(type);
            }
            return list;
        }
        public bool CheckIfExistActivePassbookInType(int idType)
        {
            //kiem tra xem trong bang tai khoan co cai nao co Type = idType ma van con tien trong tai khoan hay khong?
            int result = (int)DataProvider.Instance.ExcuteScarar("select count(*) from dbo.passbook where passbook_type=" + idType + " and passbook_balance >0");
            if (result != 0)
                return true;
            return false;
        }
        public void DeleteType(int idType)
        {
            //co the phai xoa account thuoc type truoc, sau do moi xoa type, dung trigger    
        }
        public void InsertType(TypePassbook type)
        {
            float Interset_rate = type.Interest_rate;
            int term = type.Term;
            long min_passbookblance = type.Min_passbookblance;
            long min_collectmoney = type.Min_collectmoney;
            string query1 = string.Format("usp_InsertTypePassbook {0}, {1}, {2}, {3} ", Interset_rate, term, min_passbookblance, min_collectmoney);
            DataProvider.Instance.ExcuteNonQuery(query1);
            string query = string.Format("update dbo.typepassbook set withdrawterm = {0} where interest_rate = {1} and term = {2} and min_balance = {3} and min_collectmoney = {4} ", type.Withdrawterm, Interset_rate, term, min_passbookblance, min_collectmoney);
            if (type.Term == 0) DataProvider.Instance.ExcuteNonQuery(query);
        }
        public void UpdateType(int idtype, long minmoney, long minbalance, float rate, int mindaywithdraw)
        {
            string query = string.Format("update dbo.typepassbook set min_collectmoney = {0}, min_passbookbalance = {1}, interest_rate = {2}, withdrawterm = case when term = 0 then {3} else withdrawterm end where id = {4}", minmoney, minbalance, rate, mindaywithdraw,  idtype);
            DataProvider.Instance.ExcuteNonQuery(query);
        }
    }
}
