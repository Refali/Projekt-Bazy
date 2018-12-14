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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt_Czarnacka_Gawron_Hasa_Kuchta
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (userTxt.Text == "recepcja")
            {
                Recepcja recepcjaOkno = new Recepcja();
                recepcjaOkno.Show();
                this.Close();
            }
            else if (userTxt.Text == "lekarz")
            {
                Lekarz lekarzOkno = new Lekarz();
                lekarzOkno.Show();
                this.Close();
            }
            else if (userTxt.Text == "boss")
            {
                Wlasciciel bossOkno = new Wlasciciel();
                bossOkno.Show();
                this.Close();
            }

        }
    }
}
