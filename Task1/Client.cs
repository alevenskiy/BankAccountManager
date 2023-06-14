using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Task1
{
    public class Pair<TKey, TValue> : INotifyPropertyChanged
    {
        private TKey _key;
        private TValue _value;

        public TKey AccountName { 
            get 
            { 
                return _key;
            }
            set
            {
                _key = value;
                OnPropertyChanged("AccountName");
            }
        }
        public TValue Balance 
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("Balance");
            }
        }

        public Pair() { }

        public Pair(TKey AccountName, TValue Balance)
        {
            this.AccountName = AccountName;
            this.Balance = Balance;
        }

        public JObject Serialize()
        {
            JObject jObj = new JObject();
            jObj["AccountName"] = AccountName.ToString();
            jObj["Balance"] = Balance.ToString();
            return jObj;
        }

        #region INotifyPropertyChanged inheritance 

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }

    internal class Client
    {
        public string Name { get; }
        public ObservableCollection<Pair<string, int>> Account { get; set; }

        public Client() { }

        public Client(string Name, string account)
        {
            this.Name = Name;
            Account = new ObservableCollection<Pair<string, int>>()
            {
                new Pair<string, int>(account, 0)
            };
        }

        /// <summary>
        /// Открывает новый счет, если имя счета не занято
        /// </summary>
        /// <param name="account">Имя нового счета</param>
        /// <returns>true - если счет открыт, false - если счет с таким именем уже существует</returns>
        public bool AddAccount(string account)
        {
            foreach (Pair<string, int> pair in Account)
            {
                if(pair.AccountName == account)
                {
                    return false;
                }
            }
            Account.Add(new Pair<string, int>(account, 0));
            return true;
        }

        /// <summary>
        /// Закрывает счет
        /// </summary>
        /// <param name="account">Имя счета</param>
        /// <returns>true - если счет закрыт, false - если счета с таким именем нет</returns>
        public bool RemoveAccount(string account)
        {
            foreach(Pair<string, int> pair in Account)
            {
                if (pair.AccountName == account)
                {
                    Account.Remove(pair);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Закрывает счет
        /// </summary>
        /// <param name="pair">Пара (имя счета, баланс)</param>
        /// <returns>true - если счет закрыт, false - если такого счета нет</returns>
        public bool RemoveAccount(Pair<string, int> pair)
        {
            if (Account.Contains(pair))
            {
                Account.Remove(pair);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Возвращает индекс элемента по имени счета
        /// </summary>
        /// <param name="account">Имя счета</param>
        /// <returns>Индекс элемента или -1 если такого нет</returns>
        public int IndexOf(string account)
        {
            for(int i = 0; i < Account.Count; i++)
            {
                if (Account[i].AccountName == account)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Перевод с счета на счет
        /// </summary>
        /// <param name="fromAcc">Имя счета отправителя</param>
        /// <param name="toAcc">Имя счета получателя</param>
        /// <param name="value">Размер перевода</param>
        /// <returns>true - если перевод произошел, false - если переданные параметры некорректные</returns>
        public bool Transfer(string fromAcc, string toAcc, int value)
        {
            int ind1 = IndexOf(fromAcc);
            int ind2 = IndexOf(toAcc);
            if(ind1 == -1) 
                return false;
            if(ind2 == -1) 
                return false;
            if (Account[ind1].Balance < value)
                return false;

            Account[ind1].Balance -= value;
            Account[ind2].Balance += value;
            return true;
        }

        /// <summary>
        /// Перевод с счета на первый счет Клиента
        /// </summary>
        /// <param name="fromAcc">Имя счета отправителя</param>
        /// <param name="toClient">Имя получателя</param>
        /// <param name="value">Размер перевода</param>
        /// <returns>true - если перевод произошел, false - если переданные параметры некорректные</returns>
        public bool Transfer(string fromAcc, Client toClient, int value)
        {
            int ind = IndexOf(fromAcc);
            if (ind == -1)
                return false;
            if (Account[ind].Balance < value)
                return false;

            Account[ind].Balance -= value;
            toClient.Account[0].Balance += value;
            return true;
        }

        public JObject Serialize()
        {
            JObject obj = new JObject();
            obj["Name"] = Name;

            JArray array = new JArray();

            foreach (Pair<string, int> pair in Account)
            {
                array.Add(pair.Serialize());
            }

            obj["Account"] = array;

            return obj;
        }
    }

    internal class ClientList : ObservableCollection<Client>
    {
        public ClientList Deserialize(string path)
        {
            string str = "";

            try
            {
                using (Stream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    StreamReader streamReader = new StreamReader(stream);
                    str = streamReader.ReadToEnd();
                }
            }
            catch
            {
                MessageBox.Show("File does not open");
            }

            if (str != "")
                return JsonConvert.DeserializeObject<ClientList>(str);
            else
                return null;
        }

        public void Serialize(string path)
        {
            JArray array = new JArray();

            foreach (Client client in this)
                array.Add(client.Serialize());

            string str = array.ToString();

            try
            {
                File.WriteAllText(path, str);
            }
            catch
            {
                MessageBox.Show("File does not open");
            }
        }
    }
}
