using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BankAccount
{
    public class Transaction
    {
        public Account Sender { get;  set; }
        public Account Recipient { get;  set; }
        public decimal Sum { get;  set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public Transaction()
        {

        }
        public Transaction(Account Sender, Account Recipient, decimal Sum, string mes)
        {
            this.Sender = Sender;
            this.Recipient = Recipient;
            this.Sum = Sum;
            this.Message = mes;
            Date = DateTime.Now;
        } 
        public void Activate()
        {
            if(Sender == null)
            {
                Sender?.Withdraw(Sum);
                Recipient?.Put(Sum);
                GlobalBank.ourbank.transaction.Add(this);
                GlobalBank.ourbank.Serialize();
            }
            else if (Sender.CanWithdraw(Sum) && Recipient.CanPut(Sum))
            {
                Sender?.Withdraw(Sum);
                Recipient?.Put(Sum);
                GlobalBank.ourbank.transaction.Add(this);
                GlobalBank.ourbank.Serialize();
            }
        }

        //добавление средств на счет
        public void Put()
        {
           Recipient?.Put(Sum);
        }
        // вывод средств
        public void Withdraw()
        {
            Sender?.Withdraw(Sum);
        }
    }
}
