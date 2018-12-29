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
        int id_pacjenta, id_lekarza;
        int ilosc = 0;
        public Recepcja()
        {
            InitializeComponent();
        }
        public Recepcja(SqlConnection conn,string userName)
        {
            InitializeComponent();
             
            this.conn = conn;
            this.userName = userName;

            status(); //status i zalogowano jako

            selecty(); //wszystkie potrzebne selecty

            fill_PacjentListBox(); //uzupełnienie listy Pacjentów w dodawaniu wizyty - wywołanie
            fill_LekarzListBox(); //uzupełnienie listy Lekarzy w dodawaniu wizyty - wywołanie
        }
        private void ZapiszPacjenta_Click(object sender, RoutedEventArgs e)
        {
            DodawaniePacjenta(); //dodawanie pacjenta xd
        }

        private void ZapiszWizyte_Click(object sender, RoutedEventArgs e)
        {
            DodawanieWizyty();   //dodawanie wizyty xd         
        }

        //-----------------------------------------------------------//
        // --------------------------METODY--------------------------//
        //-----------------------------------------------------------//

        void SprawdzanieWizyty()
        {
            SqlCommand querySprawdzenieWizyt = new SqlCommand();
            string polecenieSprawdzenieWizyt = "select data,godzina from Wizyty where data=@data and godzina=@godzina";
            querySprawdzenieWizyt.Parameters.AddWithValue("@data", DataTxt.SelectedDate.Value.ToShortDateString());
            querySprawdzenieWizyt.Parameters.AddWithValue("@godzina", GodzinaListBox.SelectionBoxItem);
            querySprawdzenieWizyt.CommandText = polecenieSprawdzenieWizyt;
            querySprawdzenieWizyt.Connection = conn;

            SqlDataReader readerSprawdzenieWizyt = querySprawdzenieWizyt.ExecuteReader();
            
            while (readerSprawdzenieWizyt.Read())
            {
                ilosc++;

            }
            readerSprawdzenieWizyt.Close();

            MessageBox.Show(ilosc.ToString());
        }
        void DodawanieWizyty()
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
            }
            catch (SqlException exc)
            {
                MessageBox.Show(exc.Message);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            SprawdzanieWizyty();
            try
            {
                
                MessageBox.Show(ilosc.ToString());
                if (ilosc == 0)
                {

                    DataRow drw = dsw.Tables["Wizyty"].NewRow();
                    drw["id_pacjenta"] = id_pacjenta;
                    drw["id_lekarza"] = id_lekarza;
                    drw["data"] = DataTxt.SelectedDate.Value.ToShortDateString();
                    drw["godzina"] = GodzinaListBox.SelectionBoxItem;
                    drw["status"] = "Zarezerwowana";
                    drw["uwagi"] = "Brak";

                    dsw.Tables["Wizyty"].Rows.Add(drw);

                    try
                    {
                        SqlCommandBuilder queryUpdate = new SqlCommandBuilder(dataAdapterW);
                        int updateIloscWizyt = dataAdapterW.Update(dsw, "Wizyty");

                        MessageBox.Show("Utworzono wizytę");
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }

                }
                else
                {
                    MessageBox.Show("Jest już utworzona wizyta na tą datę");
                }
                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }

        void DodawaniePacjenta()
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

        void status()
        {
            statusLbl.Content = conn.State.ToString(); 
            userLbl.Content = userName;
        }

        void selecty()
        {
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


        //Widzialność paneli

        private void nowyPacjent_Click(object sender, RoutedEventArgs e)
        {
            PanelDodawaniePacjenta.Visibility = Visibility.Visible;
            PanelDodawanieWizyty.Visibility = Visibility.Collapsed;
            PanelEdycjaWizyty.Visibility = Visibility.Collapsed;
            PanelUsuwaniaPacjenta.Visibility = Visibility.Collapsed;
        }
        private void usunPacjenta_Click(object sender, RoutedEventArgs e)
        {
            PanelDodawaniePacjenta.Visibility = Visibility.Collapsed;
            PanelDodawanieWizyty.Visibility = Visibility.Collapsed;
            PanelEdycjaWizyty.Visibility = Visibility.Collapsed;
            PanelUsuwaniaPacjenta.Visibility = Visibility.Visible;
        }
        private void nowaWizyta_Click(object sender, RoutedEventArgs e)
        {
            PanelDodawanieWizyty.Visibility = Visibility.Visible;
            PanelDodawaniePacjenta.Visibility = Visibility.Collapsed;
            PanelEdycjaWizyty.Visibility = Visibility.Collapsed;
            PanelUsuwaniaPacjenta.Visibility = Visibility.Collapsed;
        }

        private void edycjaWizyty_Click(object sender, RoutedEventArgs e)
        {
            PanelDodawanieWizyty.Visibility = Visibility.Collapsed;
            PanelDodawaniePacjenta.Visibility = Visibility.Collapsed;
            PanelEdycjaWizyty.Visibility = Visibility.Visible;
            PanelUsuwaniaPacjenta.Visibility = Visibility.Collapsed;
        }

        
        
        
    }
}
