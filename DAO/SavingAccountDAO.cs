using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class SavingAccountDAO
    {
        private static SavingAccountDAO instance;

        public static SavingAccountDAO Instance
        {
            get { if (instance == null) instance = new SavingAccountDAO(); return instance; }
            private set { instance = value; }
        }
        private SavingAccountDAO() { }
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
    }
}
