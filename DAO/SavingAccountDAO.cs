using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
