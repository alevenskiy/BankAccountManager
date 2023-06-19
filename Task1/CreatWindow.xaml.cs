using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Task1
{
    /// <summary>
    /// Логика взаимодействия для CreatWindow.xaml
    /// </summary>
    public partial class CreatWindow : Window
    {
        public CreatWindow(Window parent)
        {
            InitializeComponent();
            this.Owner = parent;

        }

        private void butt_add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void butt_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
