using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankAccount
{
   public static class GlobalBank
    {
        public static Bank ourbank = new Bank();

        public static void CloseApplication()
        {
            ourbank.Serialize();
            Application.Exit();
        }

    }
}
