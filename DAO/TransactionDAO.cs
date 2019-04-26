using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class TransactionDAO
    {
        private static TransactionDAO instance;

        public static TransactionDAO Instance
        {
            get { if (instance == null) instance = new TransactionDAO(); return instance; }
            private set { instance = value; }
        }
        private TransactionDAO() { }
        public List<Transaction> GetListTransaction(string cusname, DateTime? date)
        {
            List<Transaction> list = new List<Transaction>();
            if (date == null)
            {
                List<CollectBill> collectBills = CollectBillDAO.Instance.GetListBill(cusname, date);
                List<WithdrawBill> withdrawBills = WithdrawBillDAO.Instance.GetListBill(cusname, date);
                foreach (CollectBill collectBill in collectBills)
                {
                    Transaction transaction = new Transaction(collectBill);
                    transaction.Cusname = CustomerDAO.Instance.GetCustomerNameByCollectBillID(transaction.Id);
                    transaction.Balancemoney = PassbookDAO.Instance.GetBalanceMoneyByCollectBillID(transaction.Id);
                    transaction.Passbooktype = PassbookDAO.Instance.GetPassbookTypeNameByCollectBillID(transaction.Id);
                    list.Add(transaction);
                }
                foreach (WithdrawBill withdrawBill in withdrawBills)
                {
                    Transaction transaction = new Transaction(withdrawBill);
                    transaction.Cusname = CustomerDAO.Instance.GetCustomerNameByWithdrawBillID(transaction.Id);
                    transaction.Balancemoney = PassbookDAO.Instance.GetBalanceMoneyByWithdrawBillID(transaction.Id);
                    transaction.Passbooktype = PassbookDAO.Instance.GetPassbookTypeNameByWithdrawBillID(transaction.Id);
                    list.Add(transaction);
                }
            }
            return list;
        }
    }
}
