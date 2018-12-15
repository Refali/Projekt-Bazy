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
using System.Data.SqlClient;
namespace Projekt_Czarnacka_Gawron_Hasa_Kuchta
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn;
        SqlConnectionStringBuilder connString;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (userTxt.Text == "r")
            {
                connString = new SqlConnectionStringBuilder();
                connString.InitialCatalog = "Firma";
                connString.DataSource = "DESKTOP-70K18SL\\SQLEXPRESS";
                connString.IntegratedSecurity = true;
                conn = new SqlConnection(connString.ConnectionString);
                conn.Open();
                Recepcja recepcjaOkno = new Recepcja(conn, userTxt.Text);
                recepcjaOkno.Show();
                this.Close();
            }
            else if (userTxt.Text == "l")
            {
                connString = new SqlConnectionStringBuilder();
                connString.InitialCatalog = "Firma";
                connString.DataSource = "DESKTOP-70K18SL\\SQLEXPRESS";
                connString.IntegratedSecurity = true;
                conn = new SqlConnection(connString.ConnectionString);
                conn.Open();
                Lekarz lekarzOkno = new Lekarz();
                lekarzOkno.Show();
                this.Close();
            }
            else if (userTxt.Text == "w")
            {
                connString = new SqlConnectionStringBuilder();
                connString.InitialCatalog = "Firma";
                connString.DataSource = "DESKTOP-70K18SL\\SQLEXPRESS";
                connString.IntegratedSecurity = true;
                conn = new SqlConnection(connString.ConnectionString);
                conn.Open();
                Wlasciciel bossOkno = new Wlasciciel();
                bossOkno.Show();
                this.Close();
                
            }
            

        }
    }
}
