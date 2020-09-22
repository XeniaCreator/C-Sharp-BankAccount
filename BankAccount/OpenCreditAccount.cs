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
    public partial class OpenCreditAccount : Form
    {
        User user;
        public OpenCreditAccount(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Account newac = null;
            int sum;
            try
            {
                    if (int.TryParse(textBox1.Text, out sum) && sum > 0)
                    {
                            newac = user.Open(AccountType.Credit, sum, null, null, null, null, null);
                            user.DoTransaction(null, sum, newac, "Взят кредит");
                            this.Close();
                    }
                    else MessageBox.Show("Не корректный ввод данных");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                user.Close(newac?.Id);
            }
        }
    
    }
}
