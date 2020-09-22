using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccount
{
    public class DepositAccount : Account
    {
        //депозитный счет
        public DepositAccount()
        {

        }
        public DepositAccount(decimal sum) : base(0, 14)
        {
            Date = DateTime.Now.AddYears(1);
        }
        //возможность положить деньги
        public override bool CanPut(decimal sum)
        {
            if (Sum > 0)
            {
                OnAdded(new AccountEventArgs("На депозитный счет " + Id + " положить деньги не возможно " + sum, sum));
                throw new Exception("Для депозитного счета эта операция не доступна ");
                return false;
            }
            else return true;
        }
        // //проверка возможности снять деньги
        public override bool CanWithdraw(decimal sum)
        {
            if (Sum >= sum)
            {
                return true;
            }
            else
            {
                if (DateTime.Now >= Date)
                    return true;
                else
                {
                    base.OnWithdrawed(new AccountEventArgs("Вывести средства можно только после даты " +
                        Date.ToShortDateString(), 0));
                    throw new Exception("Вывести средства можно только после даты " +
                         Date.ToShortDateString());
                    return false;
                }
            }
        }

        //положить деньги
        public override void Put(decimal sum)
        {
            if (this.Sum > 0)
            {
                OnAdded(new AccountEventArgs("На депозитный счет " + Id + " положить деньги не возможно " + sum, sum));
                throw new Exception("Для депозитного счета эта операция не доступна ");
            }
            else
            {
                this.Sum = sum+ sum/100*Percentage;
            }
        }
        // открытие счета
        public override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Открыт новый депозитный счет! Id счета: {this.Id} до:"
                +Date.ToShortDateString() , this.Sum));
        }
        // закрытие счета
        public override void Close()
        {
            if ((DateTime.Now >= Date && Sum==0) || Sum==0)
            {
                if(this.Sum==0)
                {
                    OnClosed(new AccountEventArgs($"Счет {Id} закрыт.  Итоговая сумма: {Sum}", Sum));
                }
                else
                {
                    base.OnClosed(new AccountEventArgs("Сначала нужно вывести средства", this.Sum));
                   throw new Exception("Сначала нужно вывести средства" );
                }
            }
               
            else
            {
                base.OnWithdrawed(new AccountEventArgs("Вывести средства можно только после даты " + Date, 0));
                throw new Exception("Вывести средства можно только после даты " + Date);
            }
        }
        
        //снятие денег
        public override decimal Withdraw(decimal sum)
        {
            if (DateTime.Now >= Date)
                return base.Withdraw(sum);
            else
            {
                base.OnWithdrawed(new AccountEventArgs("Вывести средства можно только после даты "+
                    Date.ToShortDateString(), 0));
               throw new Exception("Вывести средства можно только после даты " +
                    Date.ToShortDateString());
                return 0;
            }
        }
        // начисление процентов
       public override void Calculate()
        {
                base.Calculate();
        }
    }
}
