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
    public partial class Recepcja : Window
    {
        SqlConnection conn;
        SqlDataAdapter dataAdapterP,dataAdapterW,dataAdapterPr;
        DataSet dsp,dsw,dsl;
        string userName;
        public Recepcja()
        {
            InitializeComponent();
        }
        public Recepcja(SqlConnection conn,string userName)
        {
            InitializeComponent();
             
            this.conn = conn;
            this.userName = userName;
            
            statusLbl.Content = conn.State.ToString(); //status i zalogowano jako
            userLbl.Content = userName;

            string querySelectPacjenci = "Select * from Pacjenci";
            dataAdapterP = new SqlDataAdapter(querySelectPacjenci, conn);
            dsp = new DataSet();
            dataAdapterP.Fill(dsp, "Pacjenci");

            string querySelectWizyty = "Select * from Wizyty";
            dataAdapterW = new SqlDataAdapter(querySelectWizyty, conn);
            dsw = new DataSet();
            dataAdapterW.Fill(dsw, "Wizyty");

            string querySelectLekarze = "Select * from Pracownicy";
            dataAdapterPr = new SqlDataAdapter(querySelectLekarze, conn);
            dsl = new DataSet();
            dataAdapterPr.Fill(dsl, "Wizyty");

            fill_PacjentListBox(); //uzupełnienie listy Pacjentów w dodawaniu wizyty - wywołanie
            fill_LekarzListBox(); //uzupełnienie listy Lekarzy w dodawaniu wizyty - wywołanie
        }
        void fill_LekarzListBox() //uzupełnienie listy Pacjentów w dodawaniu wizyty
        {
            try
            {
                string query = "select * from Pracownicy where stanowisko='lekarz'";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    string pesel = dr.GetString(3);
                    string nazwisko = dr.GetString(2);
                    LekarzListBox.Items.Add("PESEL: "+pesel+ " Nazwisko: " +nazwisko);
                }
                dr.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        void fill_PacjentListBox() //uzupełnienie listy Lekarzy w dodawaniu wizyty
        {
            try
            {
                string query = "select * from Pacjenci";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    string pesel = dr.GetString(3);
                    string nazwisko = dr.GetString(2);
                    PacjentListBox.Items.Add("PESEL: " + pesel + " Nazwisko: " + nazwisko);
                }
                dr.Close();
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
                
        }
        private void nowyPacjent_Click(object sender, RoutedEventArgs e)
        {
            PanelDodawaniePacjenta.Visibility = Visibility.Visible;
            PanelDodawanieWizyty.Visibility = Visibility.Collapsed;
            PanelEdycjaWizyty.Visibility = Visibility.Collapsed;
        }

        private void nowaWizyta_Click(object sender, RoutedEventArgs e)
        {
            PanelDodawanieWizyty.Visibility = Visibility.Visible;
            PanelDodawaniePacjenta.Visibility = Visibility.Collapsed;
            PanelEdycjaWizyty.Visibility = Visibility.Collapsed;

        }

        private void edycjaWizyty_Click(object sender, RoutedEventArgs e)
        {
            PanelDodawanieWizyty.Visibility = Visibility.Collapsed;
            PanelDodawaniePacjenta.Visibility = Visibility.Collapsed;
            PanelEdycjaWizyty.Visibility = Visibility.Visible;
        }

        private void ZapiszPacjenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRow dr = dsp.Tables["Pacjenci"].NewRow();
                dr["imie"] = imiePacjentaTxt.Text;
                dr["nazwisko"] = nazwiskoPacjentaTxt.Text;
                dr["pesel"] = peselPacjentaTxt.Text;
                dr["adres"] = adresPacjentaTxt.Text;
                dr["telefon"] = telefonPacjenta.Text;

                dsp.Tables["Pacjenci"].Rows.Add(dr);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                SqlCommandBuilder queryUpdate = new SqlCommandBuilder(dataAdapterP);
                int updateIlosc = dataAdapterP.Update(dsp, "Pacjenci");
                MessageBox.Show("Dodano pacjenta");
                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }
        int id_pacjenta,id_lekarza;
        private void ZapiszWizyte_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                
                //znalezienie id pacjenta w bazie
                SqlCommand queryPacjenta = new SqlCommand();
                string polecenie = "Select id from Pacjenci where pesel like @peselPacjenta";
                string peselpacjenta = PacjentListBox.SelectedValue.ToString().Substring(7, 11);
                queryPacjenta.Parameters.AddWithValue("@peselPacjenta", peselpacjenta);
                queryPacjenta.CommandText = polecenie;
                queryPacjenta.Connection = conn;

                SqlDataReader readerPac = queryPacjenta.ExecuteReader();
                if (readerPac.HasRows)
                {
                    while (readerPac.Read())
                    {
                        id_pacjenta = readerPac.GetInt32(0);
                    }
                }
                readerPac.Close();
                //znalezienie id lekarza
                SqlCommand queryLekarza = new SqlCommand();
                string polecenie2 = "Select id from Pracownicy where pesel like @peselPracownika and stanowisko like 'Lekarz'";
                string peselLekarza = LekarzListBox.SelectedValue.ToString().Substring(7, 11);
                queryLekarza.Parameters.AddWithValue("@peselPracownika", peselLekarza);
                queryLekarza.CommandText = polecenie2;
                queryLekarza.Connection = conn;

                SqlDataReader readerPrac = queryLekarza.ExecuteReader();
                if (readerPrac.HasRows)
                {
                    while (readerPrac.Read())
                    {
                        id_lekarza = readerPrac.GetInt32(0);
                    }
                }
                readerPrac.Close();
                //status ustawi się sam na zarezerwowano

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            /*
            try
            {
                DataRow drw = dsp.Tables["Wizyty"].NewRow();
                drw["id_pacjenta"] = id_pacjenta;
                drw["id_lekarza"] = id_lekarza;
                drw["data"] = DataTxt.SelectedDate.ToString();
                drw["godzina"] = GodzinaListBox.SelectedValue.ToString();
                drw["status"] = "Zarezerwowana";
                drw["uwagi"] = "Brak";


                dsw.Tables["Wizyty"].Rows.Add(drw);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                SqlCommandBuilder queryUpdate = new SqlCommandBuilder(dataAdapterW);
                int updateIlosc = dataAdapterW.Update(dsw, "Wizyty");

                MessageBox.Show("Utworzono wizytę");

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }*/
        }
    }
}
