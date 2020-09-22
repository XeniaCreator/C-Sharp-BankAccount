using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccount
{
    public class CreditAccount : Account
    {
     //кредитный счет
        public CreditAccount()
        {

        }
        public CreditAccount(decimal sum) : base(sum, 14)
        {
            Date = DateTime.Now;
            EndSum = 2*sum+sum/100*Percentage;
        }
       
        //возможность положить деньги
        public override bool CanPut(decimal sum)
        {
            if (EndSum == 0)
            {
                OnAdded(new AccountEventArgs("Кредитный счет " + Id + " выплачен ", sum));
                throw new Exception("Кредитный счет " + Id + " выплачен ");
                return false;
            }
            else return true;
        }

        //положить деньги
        public override void Put(decimal sum)
        {
            if(CanPut(sum))
            {
                EndSum -= sum;
            }
        }
        // открытие счета
        public override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Открыт новый кредитный счет! Id счета: {this.Id}", this.Sum));
        }
        // закрытие счета
        public override void Close()
        {
            if (EndSum==0 && Sum == 0)
            {
                    OnClosed(new AccountEventArgs($"Счет {Id} закрыт.  Итоговая сумма: {Sum}", Sum));
            }

            else
            {
                base.OnWithdrawed(new AccountEventArgs("Закрыть счет можно только после полного погашения кредита и вывод всех средств ",  Sum));
                throw new Exception("Закрыть счет можно только после полного погашения кредита и вывод всех средств");
            }
        }
        // начисление процентов
        public override void Calculate()
        {
            OnCalculated(new AccountEventArgs($"Для полного погашения счета {Id} нужно выплатить {EndSum}", 0));
        }
    }
}
