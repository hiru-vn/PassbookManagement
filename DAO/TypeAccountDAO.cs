using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class TypeAccountDAO
    {
        private static TypeAccountDAO instance;

        public static TypeAccountDAO Instance
        {
            get { if (instance == null) instance = new TypeAccountDAO(); return instance; }
            private set { instance = value; }
        }
        private TypeAccountDAO() { }
        public List<string> GetListType()
        {
            List<string> list = new List<string>();
            return list;
        }
    }
}
