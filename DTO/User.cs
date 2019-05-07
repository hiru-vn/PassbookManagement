using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    class User
    {
        string UserName;
        string Password;
        int priority;
        //Mã hóa và giải mã UTF8
        public static string EncodeServerName(string serverName)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serverName));
        }

        public static string DecodeServerName(string encodedServername)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedServername));
        }
        //Password đúng
        public bool PasswordIsRight(string Password)
        {
            string NPassword = null;
            //get Password trong xml
            NPassword = DecodeServerName(NPassword);
            if (string.Compare(Password, NPassword, false) == 0)
            {
                return true;
            }
            return false;
        }
        public bool UserNameRight(string UserName)
        {
            string NUserName = null;
            //Get user Name trong xml;
            if (string.Compare(UserName, NUserName, false) == 0)
            {
                return true;
            }
            return false;
        }
        //public int GetPriority(){ }
    }
}
