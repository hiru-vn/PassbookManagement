using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class WithdrawBill
    {
        string _id;
        int _withdraw_passbook;
        long _withdrawmoney;
        DateTime? withdrawdate;
        public WithdrawBill() { }
        public WithdrawBill(DataRow row)
        {
            this.Id = row["id"].ToString();
            this.Withdrawmoney = long.Parse(row["withdrawmoney"].ToString());
            this.Withdraw_passbook = int.Parse(row["withdrawpassbook"].ToString());
            this.Withdrawdate = (DateTime?)row["withdrawdate"];

        }

        public string Id { get => _id; set => _id = value; }
        public int Withdraw_passbook { get => _withdraw_passbook; set => _withdraw_passbook = value; }
        public long Withdrawmoney { get => _withdrawmoney; set => _withdrawmoney = value; }
        public DateTime? Withdrawdate { get => withdrawdate; set => withdrawdate = value; }
    }
}
