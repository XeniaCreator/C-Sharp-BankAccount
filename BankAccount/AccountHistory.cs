using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankAccount
{
    public partial class AccountHistory : Form
    {
        public AccountHistory(List<Transaction> lt, User user, string acc)
        {
            InitializeComponent();
            label2.Text = acc;
            if(lt!=null)
            {
                foreach(var trans in lt)
                {
                    if (trans.Sender?.Id == acc)
                    {
                        listBox1.Items.Add(trans.Date.ToShortDateString() + trans.Date.ToShortTimeString()
                            + ":\n" + "-" + trans.Sum + " на счет:"
                          + trans.Recipient.Id + "\nКоментарий:" + trans.Message);
                    }
                    else
                    {
                        listBox1.Items.Add(trans.Date.ToShortDateString() + trans.Date.ToShortTimeString()
                            + ":" + "+" + trans.Sum + " со счета:"
                     + trans.Sender?.Id + " Коментарий:" + trans.Message);

                    }
                }
            }
        }
    }
}
