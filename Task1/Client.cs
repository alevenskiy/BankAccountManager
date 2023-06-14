using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Task1
{
    public class Pair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Pair() { }

        public Pair(TKey key, TValue val)
        {
            this.Key = key;
            this.Value = val;
        }
    }

    internal class Client
    {
        public string Name { get; }
        public ObservableCollection<Pair<string, int>> Account { get; set; }

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
                if(pair.Key == account)
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
                if (pair.Key == account)
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
                if (Account[i].Key == account)
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
            if (Account[ind1].Value < value)
                return false;

            Account[ind1].Value -= value;
            Account[ind2].Value += value;
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
            if (Account[ind].Value < value)
                return false;

            Account[ind].Value -= value;
            toClient.Account[0].Value += value;
            return true;
        }
    }
}
