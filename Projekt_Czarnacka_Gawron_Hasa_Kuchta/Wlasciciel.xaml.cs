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
using System.Data.SqlClient;
using System.Data;
namespace Projekt_Czarnacka_Gawron_Hasa_Kuchta
{
    public partial class Wlasciciel : Window
    {
        int liczbaPracownikow,i;
        SqlDataAdapter dataAdapterWl, dataAdapterPracownicy;
        DataSet dswl;
        DataTable dt;
        SqlConnection conn;
        string userName;
        
        public Wlasciciel()
        {
            InitializeComponent();
        }
        public Wlasciciel(SqlConnection conn, string userName)
        {
            InitializeComponent();

            this.conn = conn;
            this.userName = userName;
            status();

            string querySelectPracownicy = "Select * from Pracownicy";
            dataAdapterWl = new SqlDataAdapter(querySelectPracownicy, conn);
            dswl = new DataSet();
            dataAdapterWl.Fill(dswl, "Pracownicy");

        }
        private void nowyLekarz_Click(object sender, RoutedEventArgs e)
        {
            PanelDodawanieLekarza.Visibility = Visibility.Visible;
            PanelPracownikow.Visibility = Visibility.Collapsed;
            PanelPracownikow.Visibility = Visibility.Collapsed;
        }

        private void Raport_Click(object sender, RoutedEventArgs e)
        {
            PanelPracownikow.Visibility = Visibility.Collapsed;
            PanelRaportów.Visibility = Visibility.Visible;
            PanelDodawanieLekarza.Visibility = Visibility.Collapsed;
        }

        private void Pracownicy_Click(object sender, RoutedEventArgs e)
        {
            fill_DataGrid_Pracownicy();
            PanelPracownikow.Visibility = Visibility.Visible;
            PanelRaportów.Visibility = Visibility.Collapsed;
            PanelDodawanieLekarza.Visibility = Visibility.Collapsed;
        } 

        private void ZapiszLekarza_Click(object sender, RoutedEventArgs e)
        {
            idLekarza();
            DodawaniePracownika();
        }
        void status()
        {
            statusLblW.Content = conn.State.ToString();
            userLblW.Content = userName;
        }
        void fill_DataGrid_Pracownicy()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from Pracownicy";
                cmd.Connection = conn;
                dataAdapterPracownicy = new SqlDataAdapter(cmd);
                dt = new DataTable("Pracownicy");
                dataAdapterPracownicy.Fill(dt);

                PracownicyView.ItemsSource = dt.DefaultView;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void SprawdzeniePracownika()
        {
            i = 0;
            SqlCommand querySprawdzeniePracownika = new SqlCommand();
            string polecenieSprawdzeniePracownika = "select pesel from Pracownicy where pesel=@pesel";
            querySprawdzeniePracownika.Parameters.AddWithValue("@pesel",peselLekarzaTxt.Text);
            querySprawdzeniePracownika.CommandText = polecenieSprawdzeniePracownika;
            querySprawdzeniePracownika.Connection = conn;

            SqlDataReader readerSprawdzeniePracownika= querySprawdzeniePracownika.ExecuteReader();

            while (readerSprawdzeniePracownika.Read())
            {
                i++;

            }
            readerSprawdzeniePracownika.Close();

        }
        void idLekarza()
        {
            try
            {
                SqlCommand query = new SqlCommand();
                string polecenie = "Select count(*) from Pracownicy";
                query.CommandText = polecenie;
                query.Connection = conn;
                liczbaPracownikow = (int)query.ExecuteScalar();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void DodawaniePracownika()
        {
            SprawdzeniePracownika();
            if (i == 0)
            {
                try
                {
                    DataRow drwl = dswl.Tables["Pracownicy"].NewRow();
                    drwl["imie"] = imieLekarzaTxt.Text;
                    drwl["nazwisko"] = nazwiskoLekarzaTxt.Text;
                    drwl["pesel"] = peselLekarzaTxt.Text;
                    drwl["adres"] = adresLekarzaTxt.Text;
                    drwl["telefon"] = telefonLekarza.Text;
                    switch (StanowiskoBox.SelectionBoxItem)
                    {
                        case "Lekarz":
                            idLekarza();
                            liczbaPracownikow++;
                            drwl["stanowisko"] = "Lekarz";
                            drwl["login"] = "lek" + liczbaPracownikow; //unikalne logowanie potrzebne do logowania 
                            drwl["haslo"] = "lek" + liczbaPracownikow;
                            break;
                        case "Recepcja":
                            drwl["stanowisko"] = "Recepcjonistka";
                            drwl["login"] = "rec";
                            drwl["haslo"] = "rec";
                            break;
                    }


                    dswl.Tables["Pracownicy"].Rows.Add(drwl);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                try
                {
                    SqlCommandBuilder queryUpdatePracownikow = new SqlCommandBuilder(dataAdapterWl);
                    int updateIloscPracownikow = dataAdapterWl.Update(dswl, "Pracownicy");
                    MessageBox.Show("Dodano pracownika");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
            else
            {
                MessageBox.Show("Już istnieje taki pracownik");
            }

        }

        
    }
}
