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

namespace Projekt_Czarnacka_Gawron_Hasa_Kuchta
{
    public partial class Wlasciciel : Window
    {
        public Wlasciciel()
        {
            InitializeComponent();
        }

        private void nowyLekarz_Click(object sender, RoutedEventArgs e)
        {
            PanelDodawanieLekarza.Visibility = Visibility.Visible;
        }

        private void Raport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Za takie pieniądze raportów nie będzie!");
        }
    }
}
