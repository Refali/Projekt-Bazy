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
using System.Data;

namespace Projekt_Czarnacka_Gawron_Hasa_Kuchta
{
    public partial class MainWindow : Window
    {
        SqlConnection conn;
        SqlConnectionStringBuilder connString;
        string idLekarza;

        public MainWindow()
        {
            InitializeComponent();
            userTxt.Focus();
        }
        
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Logow();
        }

        private void UserTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Logow();
            }
        }

        private void Logow()
        {
            try
            {
                //utworzenie obiektu połączeniowego
                connString = new SqlConnectionStringBuilder
                {
                    InitialCatalog = "Firma",

                    //DataSource = "localhost",
                    DataSource = @"DESKTOP-SLTS1AQ\SQLEXPRESS",

                    IntegratedSecurity = true
                };
                conn = new SqlConnection(connString.ConnectionString);

                
                SqlCommand query = new SqlCommand();
                //wybranie pracownika o danym loginie i haśle
                string polecenie = "Select Stanowisko,id from Pracownicy where login=@login and haslo=@password";

                query.Parameters.AddWithValue("@login", userTxt.Text);
                query.Parameters.AddWithValue("@password", passwordTxt.Password);
                query.CommandText = polecenie;
                query.Connection = conn;

                //pobranie wybranych danych do obiektu DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(query);
                DataTable Pracownicy = new DataTable();
                adapter.Fill(Pracownicy);

                int ilosc = Pracownicy.Rows.Count;
                string stanowisko = "";
                
               //Sprawdzenie jaki użytkownik będzie będzie zalogowany, oraz wybranie okna dla niego przeznaczonego
                if (ilosc == 1)
                {
                    stanowisko = Pracownicy.Rows[0]["stanowisko"].ToString();
                    if (stanowisko == "Recepcja")
                    {
                        Recepcja recepcjaOkno = new Recepcja(conn, userTxt.Text);
                        recepcjaOkno.Show();
                        this.Close();
                    }
                    else if (stanowisko == "Lekarz")
                    {
                        idLekarza = Pracownicy.Rows[0]["id"].ToString();
                        Lekarz lekarzOkno = new Lekarz(conn, userTxt.Text, idLekarza);
                        lekarzOkno.Show();
                        this.Close();
                    }
                    else if (stanowisko == "Wlasciciel")
                    {
                        Wlasciciel bossOkno = new Wlasciciel(conn, userTxt.Text);
                        bossOkno.Show();
                        this.Close();
                    }                   
                }

                lbNieprawidloweDane.Visibility = Visibility.Visible;
                userTxt.Clear();
                passwordTxt.Clear();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

    }
}
