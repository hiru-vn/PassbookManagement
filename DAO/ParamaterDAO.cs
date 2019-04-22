using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class ParamaterDAO
    {
        private static ParamaterDAO instance;

        public static ParamaterDAO Instance
        {
            get { if (instance == null) instance = new ParamaterDAO(); return instance; }
            private set { instance = value; }
        }
        private ParamaterDAO() { }
    }
}
