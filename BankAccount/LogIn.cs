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
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Regist().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            User us=GlobalBank.ourbank.LogInUser(textBox1.Text, textBox2.Text);
            if (us != null)
            {
                new Profile(us).Show();
            }
        }
    }
}
