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
            foreach(DataRow item in data.Rows)
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
            bool value = false;
            int result = (int)DataProvider.Instance.ExcuteScarar("select count(*) from dbo.passb+ook where passbook_type=" + idType + "and passbook_balance >0");
            if (result != 0)
                value = true;
            return value;
        }
        public void DeleteType(int idType)
        {
            //co the phai xoa account thuoc type truoc, sau do moi xoa type, dung trigger     
        }
    }
}
