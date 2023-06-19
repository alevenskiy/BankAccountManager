using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /**/ClientList clients = new ClientList
        {
            new Client("client1", "acc1"),
            new Client("client2", "acc2"),
            new Client("client3", "acc3")
        };/**/


        public MainWindow()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            /**/clients[1].Account[0].Balance = 100;
            clients[2].Account[0].Balance = 1000;
            clients[2].AddAccount("acc33");
            clients[2].Account[1].Balance = 10;/**/

            lv_clients.ItemsSource = clients;
        }

        private void lv_clients_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void lv_clients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            butt_openClient_Click(butt_openClient, e);
        }

        private void butt_openClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void butt_addClient_Click(object sender, RoutedEventArgs e)
        {
            CreatWindow creatWindow = new CreatWindow(this);
            creatWindow.ShowDialog();
        }
    }
}
