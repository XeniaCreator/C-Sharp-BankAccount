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
    public partial class Profile : Form
    {
        private User user;
        public Profile(User user)
        {
            InitializeComponent();
            this.user = user;
            this.label2.Text = user.Name;
          

        }
        private void UserEvents(object sender, AccountEventArgs e)
        {
            statusStrip1.Items.Clear();
            statusStrip1.Items.Add(e.Message);
        }
        private void Profile_Load(object sender, EventArgs e)
        {
            RefreshData();
        }
        private void RefreshData()
        {
            GetAccounts();
            GetTransactions();
            foreach (var acc in user.accounts)
            {
                acc.Added += UserEvents;
                acc.Calculated += UserEvents;
                acc.Closed += UserEvents;
                acc.Opened += UserEvents;
                acc.Withdrawed += UserEvents;

            }
        }
        private void GetAccounts()
        {
            comboBox3.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            if (user.accounts?.Count > 0)
            {
                foreach (var acc in user.accounts)
                {
                    comboBox3.Items.Add(acc);
                    comboBox3.DisplayMember = "IdAndSum";
                    comboBox3.ValueMember = "Id";

                    comboBox1.Items.Add(acc);
                    comboBox1.DisplayMember = "IdAndSum";
                    comboBox1.ValueMember = "Id";

                    comboBox2.Items.Add(acc);
                    comboBox2.DisplayMember = "IdAndSum";
                    comboBox2.ValueMember = "Id";
                }
            }
            else
            {
                comboBox3.Items.Add("У вас пока что нету счетов");
                comboBox1.Items.Add("У вас пока что нету счетов");
                comboBox2.Items.Add("У вас пока что нету счетов");
            }
            comboBox3.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
        }

        private void GetTransactions()
        {
            listBox1.Items.Clear();
            
            foreach (var trans in GlobalBank.ourbank.transaction)
            {
                foreach(var acc in user.accounts)
                {
                    if (trans.Sender?.Id == acc.Id)
                    {
                        listBox1.Items.Add(trans.Date.ToShortDateString() + trans.Date.ToShortTimeString()
                       + ":" + "-" + trans.Sum + " со счета "+trans.Sender?.Id +" на счет: "
                     + trans.Recipient.Id + " Коментарий: " + trans.Message);
                    }
                    else if(trans.Recipient?.Id == acc.Id)
                    {
                        listBox1.Items.Add(trans.Date.ToShortDateString() + trans.Date.ToShortTimeString()
                            + ":" + "+" + trans.Sum +" на счет " + trans.Recipient?.Id + " со счета: "
                     + trans.Sender?.Id + " Коментарий: " + trans.Message);

                    }
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true)
            {
                textBox1.Enabled = true;
                comboBox2.Enabled = false;
            }
            else
            {
                textBox1.Enabled = false;
                comboBox2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox3.SelectedItem?.ToString() != "У вас пока что нету счетов")
            {
                new AccountHistory(user.FindTransactionByIdAccount(((Account)comboBox3.Items[comboBox3.SelectedIndex]).Id),user,
                    ((Account)comboBox3.Items[comboBox3.SelectedIndex]).Id).Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem?.ToString() != "У вас пока что нету счетов")
            {
                user.FindAccount(((Account)comboBox3.Items[comboBox3.SelectedIndex]).Id).Calculate();
             }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            user.Open(AccountType.Ordinary, 0, UserEvents, UserEvents, UserEvents, UserEvents, UserEvents);
            RefreshData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.SelectedItem?.ToString() != "У вас пока что нету счетов")
                {
                    user.Close(((Account)comboBox3.Items[comboBox3.SelectedIndex]).Id);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            RefreshData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string rid="";
            Account sid = null;
          
            try
            {
                if (checkBox1.Checked == false)
                {
                    if (comboBox1.SelectedItem?.ToString() != "У вас пока что нету счетов" &&
                    comboBox2.SelectedItem?.ToString() != "У вас пока что нету счетов")
                    {
                        sid = ((Account)comboBox1.Items[comboBox1.SelectedIndex]);
                        rid = ((Account)comboBox2.Items[comboBox2.SelectedIndex]).Id;
                    }
                }
                else
                {
                    if (comboBox1.SelectedItem?.ToString() != "У вас пока что нету счетов" &&
                  textBox1.Text != "")
                    {
                        sid = ((Account)comboBox1.Items[comboBox1.SelectedIndex]);
                        rid = textBox1.Text;
                    }
                }
                int sum;

                if (sid != null && rid != "" && int.TryParse(textBox2.Text, out sum) && sid.Id!=rid && sum>0)
                {
                    user.DoTransaction(sid, sum, rid, textBox3.Text);
                    RefreshData();
                }
                else MessageBox.Show("Не корректный ввод данных");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            RefreshData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new OpenDepositAccount(user).Show();
        }

        private void Profile_Activated(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new OpenCreditAccount(user).Show();
        }
    }
}
