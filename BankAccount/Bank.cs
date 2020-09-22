using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BankAccount
{

    public class Bank
    {
        public List<User> users;
        public List<Transaction> transaction;
        // регистрация пользователя
        public Bank()
        {
           
        }
        //  генерация id пользователя
        private string GenerateID()
        {
            string id;
            do
            {
                id = ((ulong)(int.MaxValue + new Random().Next(1000000, 2000000))).ToString();
            }
            while (users.Find(item => item.Id == id)!=null);
            return id.ToString();
        }
        // создание пользователя
        public bool Create(string Name, string Surname, string Email, string Phone,
            string Password, string ConfirmPassword)
        {
            User newuser = new User(Name, Surname, Email, Phone,
                Password, ConfirmPassword);

            var results = new List<ValidationResult>();
            var context = new ValidationContext(newuser);
            if (!Validator.TryValidateObject(newuser, context, results, true))
            {
                foreach (var error in results)
                {
                    MessageBox.Show(error.ErrorMessage);
                }
            }
            // добавляем нового пользователя в массив счетов      
            else
            {
                if (users == null)
                {
                    users = new List<User>();
                }
                    newuser.Id = GenerateID();
                     users.Add(newuser);
                AddFrirstAccountForNewUser(newuser);
                    MessageBox.Show("Успешно зарегистрирован");
                    Serialize();
                    return true;
            }
            return false;
        }
        //первый счет пользователя при регистрации
         public void AddFrirstAccountForNewUser(User newus)
        {
          Account newac=newus.Open(AccountType.Ordinary, 0, null, null, null, null, null);
                //бонусные
         //   newus.DoTransaction(null, 1000, newac, "Зачисленно бонусная 1000!");

        }
        //удаление пользователя
        public void Close(string id)
        {
            int index;
            User user = FindUser(id, out index);
            if (user == null)
                throw new Exception("Счет не найден");
            users.RemoveAt(index);
        }

        // поиск пользователя по id 
        public User FindUser(string id, out int index)
        {
            if (users != null)
            {
                index = users.FindIndex(item => item.Id == id);
                return users.Find(item => item.Id == id);
            }
            index = -1;
            return null;
        }

        // поиск пользователя по id
        public User FindUser(string id)
        {
            if (users != null)
            {
                return users.Find(item => item.Id == id);
            }
            return null;
        }
        // поиск пользователя по id счета
        public User FindUserByAccount(string Accountid)
        {
            if (users != null)
            {
                foreach (var us in users)
                {
                    if (us.FindAccount(Accountid) != null)
                    {
                        return us;
                    }
                }
            }
            return null;
        }
       //поиск счета по id
        public Account FindAccount(string id)
        {
            if (users != null)
            {
                foreach (var us in users)
                {
                   Account findedaccount = us.FindAccount(id);
                    if (findedaccount != null)
                    {
                        return findedaccount;
                    }
                }
            }
            return null;
        }
        //авторизация пользователя
        public User LogInUser(string email, string password)
        {
            User foundUser = users?.Find(item => item.Email == email && item.Password == password);
            if (users==null || foundUser == null )
                MessageBox.Show("Такого пользователя не существует");
            return foundUser;
        }
        //сериализация
        public void Serialize()
        {
            SerializeP();
            SerializeT();
        }
        //десериализация
        public void Deserialize()
        {
            DeserializeP();
            DeserializeT();
        }
        //сериализация пользователей
        public void SerializeP()
        {
            using (FileStream fs = new FileStream("people.xml", FileMode.OpenOrCreate))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<User>));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                formatter.Serialize(fs, users);
            }
        }
        //десериализация пользователей
        public void DeserializeP()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<User>));
            using (FileStream fs = new FileStream("people.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    users = (List<User>)formatter.Deserialize(fs);
                }
                catch(Exception ex)
                {
                    users = null;
                }
            }
        }

        //сериализация транзакций
        public void SerializeT()
        {
            using (FileStream fs = new FileStream("transactions.xml", FileMode.OpenOrCreate))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<Transaction>));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                formatter.Serialize(fs, transaction);
            }
        }
        //десериализация транзакций
        public void DeserializeT()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Transaction>));
            using (FileStream fs = new FileStream("transactions.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    transaction = (List<Transaction>)formatter.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    transaction = new List<Transaction>();
                }
            }
        }
    }
}
