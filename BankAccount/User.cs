using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace BankAccount
{
    // тип счета
    public enum AccountType
    {
        Ordinary,
        Deposit,
        Credit
    }
    public class User
    {
        public string Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get;  set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Surname { get;  set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Phone]
        public string Phone { get; set; }
        public DateTime Date { get; set; }
        public List<Account> accounts { get; set; }

        public User()
        {

        }
        public User(string Name, string Surname, string Email, string Phone,
            string Password, string ConfirmPassword)
        {
            this.Name = Name;
            this.Surname = Surname;
            this.Email = Email;
            this.Password = Password;
            this.ConfirmPassword = ConfirmPassword;
            this.Phone = Phone;
            this.Date = DateTime.Now;
            this.Id = Id;
            this.accounts = new List<Account>();
            
        }

        //генерация номер счета
        private string GenerateId()
        {
            int i = 1;
            string id;
            do
            {
                id = this.Id + (this.accounts.Count + i++).ToString();
            }
            while (accounts.Find(item => item.Id == id) != null);
            return id;
    }

    //открытие счета
    public Account Open(AccountType accountType, decimal sum,
      AccountStateHandler addSumHandler, AccountStateHandler withdrawSumHandler,
      AccountStateHandler calculationHandler, AccountStateHandler closeAccountHandler,
      AccountStateHandler openAccountHandler)
        {
            Account newAccount = null;
            string newid =  GenerateId();

            switch (accountType)
            {
                case AccountType.Ordinary:
                    newAccount = new DemandAccount();
                    newAccount.Id = newid;
                    break;
                case AccountType.Deposit:
                    newAccount = new DepositAccount(sum);
                    newAccount.Id = newid;
                    break;
                case AccountType.Credit:
                    newAccount = new CreditAccount(sum);
                    newAccount.Id = newid;
                    break;
            }
            if (newAccount == null)
                throw new Exception("Ошибка создания счета");
            // добавляем новый счет в массив счетов      
            if (accounts == null)
            {
                accounts = new List<Account>();
                
                accounts.Add(newAccount);
            }
            else
            {
                accounts.Add(newAccount);
            }
            // установка обработчиков событий счета
            newAccount.Added += addSumHandler;
            newAccount.Withdrawed += withdrawSumHandler;
            newAccount.Closed += closeAccountHandler;
            newAccount.Opened += openAccountHandler;
            newAccount.Calculated += calculationHandler;

            newAccount.Open();
            return newAccount;
        }

        // закрытие счета
        public void Close(string id)
        {
            int index;
            Account account = FindAccount(id, out index);
            if (account == null)
                throw new Exception("Счет не найден");
            try
            {
                account.Close();
                accounts.RemoveAt(index);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // перегруженная версия поиск счета по id
        public Account FindAccount(string id, out int index)
        {
            if (accounts != null)
            {
                index = accounts.FindIndex(item => item.Id == id);
                return accounts.Find(item => item.Id == id);
            }
            index = -1;
            return null;
        }

        // поиск счета по id
        public Account FindAccount(string id)
        {
            if (accounts != null)
            {
                return accounts.Find(item => item.Id == id);
            }
            return null;
        }

        //проведение транзакции
        public void DoTransaction(Account sender,decimal sum, string recipientId,string mes)
        {
            Account recipient = GlobalBank.ourbank.FindAccount(recipientId);
            if (recipient == null)
            {
                MessageBox.Show("Счет не найдено");
            }
            else
            {
                Transaction trans = new Transaction(sender, recipient, sum, mes);
                trans.Activate();
            }
        }

        //перегруженный метод проведения транзакции
        public void DoTransaction(Account sender, decimal sum, Account recipient, string mes)
        {
            Transaction trans = new Transaction(sender, recipient, sum, mes);
            trans.Activate();
        }
        //поиск транзакций по id счета
        public List<Transaction> FindTransactionByIdAccount(string id)
        {
            List<Transaction> lt;
            if (FindAccount(id)!=null)
            {
                lt = new List<Transaction>();
                foreach(var trans in GlobalBank.ourbank.transaction)
                {
                    if(trans.Sender?.Id == id)
                    {
                        lt.Add(trans);
                    }
                    else if(trans.Recipient?.Id == id)
                    {
                        lt.Add(trans);
                    }
                }
                if (lt.Count > 0)
                    return lt;
                else return null;
            }
            else
            {
                return null;
            }
        }

        public void OpenDemandAccount()
        {
        }
    }
}
