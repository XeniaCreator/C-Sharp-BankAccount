using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccount
{
    //счет до востребования
   public class DemandAccount : Account
    {
        public DemandAccount() : base(0, 0)
        {

        }
        // переопределение Открытия счета
        public override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Открыт новый счет до востребования! Id счета: {this.Id}", this.Sum));
        }
    }
}
