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
    public partial class OpenDepositAccount : Form
    {
        User user;
        public OpenDepositAccount(User user)
        {
            InitializeComponent();
            this.user = user;

            comboBox1.Items.Clear();
            if (user.accounts?.Count > 0)
            {
                foreach (var acc in user.accounts)
                {
                    comboBox1.Items.Add(acc);
                    comboBox1.DisplayMember = "IdAndSum";
                    comboBox1.ValueMember = "Id";
                }
            }
            else
            {
                comboBox1.Items.Add("У вас пока что нету счетов");
            }
            comboBox1.SelectedIndex = 0;

            textBox3.Text = DateTime.Now.AddYears(1).ToShortDateString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Account sid = null;
            Account newac=null;
            try
            {
                    if (comboBox1.SelectedItem?.ToString() != "У вас пока что нету счетов" &&
                  textBox1.Text != "")
                    {
                        sid = ((Account)comboBox1.Items[comboBox1.SelectedIndex]);
                        int sum;

                    if (sid != null && int.TryParse(textBox1.Text, out sum) && sum > 0)
                    {
                        if (sid.Sum >= sum)
                        {
                           newac= user.Open(AccountType.Deposit, sum, null, null, null, null, null);
                            user.DoTransaction(sid, sum,newac, "Открыт депозит");
                            this.Close();
                        }
                        else MessageBox.Show("Не достаточно средств на вашем счете");
                    }
                    else MessageBox.Show("Не корректный ввод данных");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                user.Close(newac?.Id);
            }
        }
    }
    
}
