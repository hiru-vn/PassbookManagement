using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
