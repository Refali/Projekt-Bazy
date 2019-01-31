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
        SqlDataAdapter dataAdapterP,dataAdapterW, dataAdapterUsuwaniePacjenta, dataAdapterEdycjaWizyt;
        DataSet dsp, dsw;
        DataTable dt,dt2;

        string userName;
        int id_pacjenta, id_lekarza;
        int ilosc = 0;
        int iloscPacjenta = 0;

        List<ComboBoxItem> lista = new List<ComboBoxItem>(); //combobox
        

        public enum Myenum
        {
            Zarezerwowana,
            Anulowana,
            Wykonana,
        }

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
            fill_DataGrid_Wizyty();
            fill_DataGrid_Pacjenci();
            fill_PacjentListBox(); //uzupełnienie listy Pacjentów w dodawaniu wizyty - wywołanie
            fill_LekarzListBox(); //uzupełnienie listy Lekarzy w dodawaniu wizyty - wywołanie
        }
        private void ZapiszPacjenta_Click(object sender, RoutedEventArgs e)
        {
            DodawaniePacjenta(); //dodawanie pacjenta 
        }

        private void ZapiszWizyte_Click(object sender, RoutedEventArgs e)
        {
            DodawanieWizyty();   //dodawanie wizyty          
        }

        private void UsuńBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable zmiany2 = dt2.GetChanges();
                if (zmiany2 != null)
                {
                    SqlCommandBuilder zmianyUpdate2 = new SqlCommandBuilder(dataAdapterUsuwaniePacjenta);
                    int zmienioneWiersze2 = dataAdapterUsuwaniePacjenta.Update(zmiany2);
                    dt2.AcceptChanges();
                    Label_PacjenciZmiany.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błędnie wpisane dane"+ex.Message);
            }

        }

        //edycja wizyt
        private void ZapiszEdycje_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable zmiany = dt.GetChanges();
                if (zmiany != null)
                {
                    for (int i = 0; i < zmiany.Rows.Count; i++)
                    {
                        SqlCommand command = new SqlCommand();
                        command.CommandText = "UPDATE wizyty Set Status = @Status, Uwagi=@Uwagi where id=@id";
                        command.Connection = conn;
                        command.Parameters.AddWithValue("@Status", zmiany.Rows[i]["Status"]);
                        command.Parameters.AddWithValue("@Uwagi", zmiany.Rows[i]["Uwagi"]);
                        command.Parameters.AddWithValue("@id", zmiany.Rows[i]["id"]);

                        //SqlCommandBuilder zmianyUpdate = new SqlCommandBuilder(dataAdapterEdycjaWizyt);
                        dataAdapterEdycjaWizyt.UpdateCommand = command;
                        dataAdapterEdycjaWizyt.Update(zmiany);
                    }
                    dt.AcceptChanges();
                    Label_WizytyZmiany.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        //-----------------------------------------------------------//
        // --------------------------METODY--------------------------//
        //-----------------------------------------------------------//

        //sprawdzanie czy dana wizyta już istnieje
        void SprawdzanieWizyty()
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

                //sprawdzenie wizyty na podstawie dnia godziny i lekarza
                ilosc = 0;
                SqlCommand querySprawdzenieWizyt = new SqlCommand();
                string polecenieSprawdzenieWizyt = "select data,godzina,id_lekarza,id_pacjenta from Wizyty where (data=@data and godzina=@godzina and id_lekarza=@id_lekarz) or (data=@data and godzina=@godzina and id_pacjenta=@id_pacjent)";
                querySprawdzenieWizyt.Parameters.AddWithValue("@data", DataTxt.SelectedDate.Value.ToString("yyyy-MM-dd"));
                querySprawdzenieWizyt.Parameters.AddWithValue("@godzina", GodzinaListBox.SelectionBoxItem);
                querySprawdzenieWizyt.Parameters.AddWithValue("@id_pacjent", id_pacjenta);
                querySprawdzenieWizyt.Parameters.AddWithValue("@id_lekarz", id_lekarza);
                querySprawdzenieWizyt.CommandText = polecenieSprawdzenieWizyt;
                querySprawdzenieWizyt.Connection = conn;

                SqlDataReader readerSprawdzenieWizyt = querySprawdzenieWizyt.ExecuteReader();

                while (readerSprawdzenieWizyt.Read())
                {
                    ilosc++;

                }
                readerSprawdzenieWizyt.Close();
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        //sprawdzenie istnienia pacjenta na podstawie peselu
        void SprawdzaniePacjenta()
        {
            iloscPacjenta = 0;
            SqlCommand querySprawdzeniePacjenta = new SqlCommand();
            string polecenieSprawdzeniePacjenta = "select pesel from Pacjenci where pesel=@pesel";
            querySprawdzeniePacjenta.Parameters.AddWithValue("@pesel", peselPacjentaTxt.Text);
            querySprawdzeniePacjenta.CommandText = polecenieSprawdzeniePacjenta;
            querySprawdzeniePacjenta.Connection = conn;

            SqlDataReader readerSprawdzeniePacjenta = querySprawdzeniePacjenta.ExecuteReader();

            while (readerSprawdzeniePacjenta.Read())
            {
                iloscPacjenta++;

            }
            readerSprawdzeniePacjenta.Close();
        }

        //dodanie wizyty
        void DodawanieWizyty()
        {
           
            SprawdzanieWizyty();
            try
            {

                if (ilosc == 0)
                {

                    DataRow drw = dsw.Tables["Wizyty"].NewRow();
                    drw["id_pacjenta"] = id_pacjenta;
                    drw["id_lekarza"] = id_lekarza;
                    drw["data"] = DataTxt.SelectedDate.Value.ToString("yyyy-MM-dd"); 
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
                    MessageBox.Show("Jest już utworzona wizyta na tą datę i godzinę");
                    
                }
                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }
        //dodanie pacjenta
        void DodawaniePacjenta()
        {
            SprawdzaniePacjenta();
            if (iloscPacjenta == 0)
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
            else
            {
                MessageBox.Show("Taki pacjent już istnieje");
            }

        }
        //statusy
        void status()
        {
            statusLbl.Content = conn.State.ToString(); 
            userLbl.Content = userName;
        }
        //selecty
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

        }
        //wypełnienie grida z pacjentami
        void fill_DataGrid_Pacjenci()
        {
            try
            {
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "select * from Pacjenci";
                cmd2.Connection = conn;
                dataAdapterUsuwaniePacjenta = new SqlDataAdapter(cmd2);
                dt2 = new DataTable("Pacjenci");
                dataAdapterUsuwaniePacjenta.Fill(dt2);

                UsuwanieView.ItemsSource = dt2.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //wypełnienie grida z wizytami
        void fill_DataGrid_Wizyty()
        {
            try
            {
                //combobox
                lista.Add(new ComboBoxItem() { Content = "Anulowana" });
                lista.Add(new ComboBoxItem() { Content = "Zarezerwowana" });
                eee.ItemsSource = lista;

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select W.id, P.imie + ' ' + P.nazwisko as 'Lekarz', Pa.imie + ' ' + Pa.nazwisko as 'Pacjent', W.data, W.godzina, w.status, w.uwagi from Wizyty W JOIN Pracownicy P on W.id_lekarza = P.id Join Pacjenci PA on PA.id = W.id_pacjenta";
                cmd.Connection = conn;
                dataAdapterEdycjaWizyt = new SqlDataAdapter(cmd);
                dt = new DataTable("Wizyty");
                dataAdapterEdycjaWizyt.Fill(dt);
                WizytyView.ItemsSource = dt.DefaultView;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void PressEnterdod(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                DodawaniePacjenta();
            }
        }



        //uzupełnienie listy Pacjentów w dodawaniu wizyty
        void fill_LekarzListBox() 
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

        private void WizytyView_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Label_WizytyZmiany.Visibility = Visibility.Visible;
        }

        private void UsuwanieView_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Label_PacjenciZmiany.Visibility = Visibility.Visible;
        }







        //uzupełnienie listy Lekarzy w dodawaniu wizyty
        void fill_PacjentListBox() 
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
            this.Width = 352.594;
            this.Height = 471;
        }

        private void usunPacjenta_Click(object sender, RoutedEventArgs e)
        {
            fill_DataGrid_Pacjenci();
            PanelDodawaniePacjenta.Visibility = Visibility.Collapsed;
            PanelDodawanieWizyty.Visibility = Visibility.Collapsed;
            PanelEdycjaWizyty.Visibility = Visibility.Collapsed;
            PanelUsuwaniaPacjenta.Visibility = Visibility.Visible;
            this.Width = 652.594;
            this.Height = 471;
        }
        private void nowaWizyta_Click(object sender, RoutedEventArgs e)
        {
            PanelDodawanieWizyty.Visibility = Visibility.Visible;
            PanelDodawaniePacjenta.Visibility = Visibility.Collapsed;
            PanelEdycjaWizyty.Visibility = Visibility.Collapsed;
            PanelUsuwaniaPacjenta.Visibility = Visibility.Collapsed;
            this.Width = 352.594;
            this.Height = 471;
        }

        private void edycjaWizyty_Click(object sender, RoutedEventArgs e)
        {
            fill_DataGrid_Wizyty();
            PanelDodawanieWizyty.Visibility = Visibility.Collapsed;
            PanelDodawaniePacjenta.Visibility = Visibility.Collapsed;
            PanelEdycjaWizyty.Visibility = Visibility.Visible;
            PanelUsuwaniaPacjenta.Visibility = Visibility.Collapsed;
            this.Width = 652.594;
            this.Height = 471;
        }

        private void Wyloguj_Click(object sender, RoutedEventArgs e)
        {
          if(MessageBoxResult.Yes ==   MessageBox.Show("Czy na pewno?", "Wyloguj", MessageBoxButton.YesNo))
           {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
           }
            

        }
        
        
        
    }
}
