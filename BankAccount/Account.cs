using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankAccount
{
    [XmlInclude(typeof(DemandAccount)), XmlInclude(typeof(DepositAccount)), XmlInclude(typeof(CreditAccount))]
    [Serializable]
    public abstract class Account : IAccount
        {
        //Событие, возникающее при выводе денег
        [field: NonSerialized]
        public event AccountStateHandler Withdrawed;
        // Событие возникающее при добавление на счет
        [field: NonSerialized]
        public event AccountStateHandler Added;
        // Событие возникающее при открытии счета
        [field: NonSerialized]
        public event AccountStateHandler Opened;
        // Событие возникающее при закрытии счета
        [field: NonSerialized]
        public  event AccountStateHandler Closed;
        // Событие возникающее при начислении процентов
        [field: NonSerialized]
        public  event AccountStateHandler Calculated;
        
            //Дата
            public DateTime Date { get; set; }
            // Текущая сумма на счету
            public decimal Sum { get; set; }
            // Процент начислений
            public int Percentage { get; set; }
            // Уникальный идентификатор счета
            public string Id { get; set; }
        // Окончательная сумма для закрытия счета
        public decimal EndSum { get; set; }
        
        //поля для ввыода информации о счете
        public string IdAndSum
        {
            get
            { return Id + "\t  " + Sum+"у.е."; }
            set { }
        }
          public Account()
            {

            }
        //конструктор
            public Account(decimal sum, int percentage)
            {
                Sum = sum;
                Percentage = percentage;
                Date = DateTime.Now;
            }
         //вызов событий
            private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
            {
                if (e != null)
                    handler?.Invoke(this, e);
             GlobalBank.ourbank.Serialize();
            }
            // вызов отдельных событий. Для каждого события определяется свой витуальный метод
            protected virtual void OnOpened(AccountEventArgs e)
            {
                CallEvent(e, Opened);
            }
            protected virtual void OnWithdrawed(AccountEventArgs e)
            {
                CallEvent(e, Withdrawed);
            }
            protected virtual void OnAdded(AccountEventArgs e)
            {
                CallEvent(e, Added);
            }
            protected virtual void OnClosed(AccountEventArgs e)
            {
                CallEvent(e, Closed);
            }
            protected virtual void OnCalculated(AccountEventArgs e)
            {
                CallEvent(e, Calculated);
            }

        //метод ввода средст
            public virtual void Put(decimal sum)
            {
                Sum += sum;
                OnAdded(new AccountEventArgs("На счет "+Id+" поступило " + sum, sum));
            }
            // метод снятия со счета, возвращает сколько снято со счета
            public virtual decimal Withdraw(decimal sum)
            {
                decimal result = 0;
                if (Sum >= sum)
                {
                    Sum -= sum;
                    result = sum;
                    OnWithdrawed(new AccountEventArgs($"Сумма {sum} снята со счета {Id}", sum));
                }
                else
                {
                    OnWithdrawed(new AccountEventArgs($"Недостаточно денег на счете {Id}", 0));
                    throw new Exception("Недостаточно денег на счете");
                }
                return result;
            }

        //проверка возможности положить деньги
        public virtual bool CanPut(decimal sum)
        {
            return true;
        }
        //проверка возможности снять деньги
        public virtual bool CanWithdraw(decimal sum)
        {
            if (Sum >= sum)
            {
                return true;
            }
            else
            {
                OnWithdrawed(new AccountEventArgs($"Недостаточно денег на счете {Id}", 0));
                return false;
            }
        }
        // открытие счета
        public virtual void Open()
            {
                OnOpened(new AccountEventArgs($"Открыт новый счет! Id счета: {Id}", Sum));
            }
        // закрытие счета
        public virtual void Close()
            {
            if (this.Sum == 0)
            {
                OnClosed(new AccountEventArgs($"Счет {Id} закрыт.  Итоговая сумма: {Sum}", Sum));
            }
            else
            {
                OnClosed(new AccountEventArgs("Сначала нужно вывести средства", this.Sum));
                throw new Exception("Сначала нужно вывести средства");
            }
            }
        // начисление процентов
        public virtual void Calculate()
            {
                OnCalculated(new AccountEventArgs($"Начислены проценты в размере 0 на счет {Id}", 0));
            }
        }
    }
