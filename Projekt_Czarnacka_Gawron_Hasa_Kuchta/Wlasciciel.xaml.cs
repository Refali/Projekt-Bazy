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
using System.IO;

using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Projekt_Czarnacka_Gawron_Hasa_Kuchta
{
    public partial class Wlasciciel : Window
    {
        SqlDataAdapter dataAdapterWl, dataAdapterPracownicy, dataAdapterExcel;
        DataSet dswl;
        DataTable dt, excel_dt;
        SqlConnection conn;
        string userName;

        //combobox
        List<ComboBoxItem> lista = new List<ComboBoxItem>
                {
                    new ComboBoxItem() { Content = "Lekarz" },
                    new ComboBoxItem() { Content = "Recepcja" },
                    new ComboBoxItem() { Content = "Wlasciciel" },
                };


        public Wlasciciel()
        {
            InitializeComponent();
        }
        public Wlasciciel(SqlConnection conn, string userName)
        {
            InitializeComponent();
            Title = "Wlasciciel - Raporty";

            this.conn = conn;
            this.userName = userName;
            Status();
            stanowisko_combo.ItemsSource = lista;
            string querySelectPracownicy = "Select * from Pracownicy";
            dataAdapterWl = new SqlDataAdapter(querySelectPracownicy, conn);
            dswl = new DataSet();
            dataAdapterWl.Fill(dswl, "Pracownicy");

            cb_okres.Items.Add("Wybierz Miesiąc");
            cb_okres.Items.Add("Styczeń");
            cb_okres.Items.Add("Luty");
            cb_okres.Items.Add("Marzec");
            cb_okres.Items.Add("Kwiecień");
            cb_okres.Items.Add("Maj");

        }
        private void NowyLekarz_Click(object sender, RoutedEventArgs e)
        {
            Title = "Wlasciciel - Nowy pracownik";
            PanelDodawanieLekarza.Visibility = Visibility.Visible;
            PanelPracownikow.Visibility = Visibility.Collapsed;
            PanelPracownikow.Visibility = Visibility.Collapsed;
            this.Width = 352.594;
            this.Height = 450;
        }

        private void Raport_Click(object sender, RoutedEventArgs e)
        {
            Title = "Wlasciciel - Raporty";
            PanelPracownikow.Visibility = Visibility.Collapsed;
            PanelRaportów.Visibility = Visibility.Visible;
            PanelDodawanieLekarza.Visibility = Visibility.Collapsed;
            this.Width = 352.594;
            this.Height = 450;
        }

        private void Pracownicy_Click(object sender, RoutedEventArgs e)
        {
            Title = "Wlasciciel - Pracownicy";
            Fill_DataGrid_Pracownicy();
            PanelPracownikow.Visibility = Visibility.Visible;
            PanelRaportów.Visibility = Visibility.Collapsed;
            PanelDodawanieLekarza.Visibility = Visibility.Collapsed;
            this.Width = 652.594;
            this.Height = 450;
        } 

        private void ZapiszLekarza_Click(object sender, RoutedEventArgs e)
        {
            DodawaniePracownika();
        }
        void Status()
        {
            statusLblW.Content = conn.State.ToString();
            userLblW.Content = userName;
        }
        void Fill_DataGrid_Pracownicy()
        {
            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "select * from Pracownicy",
                    Connection = conn
                };
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

        private void PressEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                DodawaniePracownika();
            }
        }

        int SprawdzeniePracownika()
        {
            SqlCommand querySprawdzeniePracownika = new SqlCommand();
            string polecenieSprawdzeniePracownika = "select login,pesel from Pracownicy where pesel=@pesel OR login=@login";
            querySprawdzeniePracownika.Parameters.AddWithValue("@pesel",peselLekarzaTxt.Text);
            querySprawdzeniePracownika.Parameters.AddWithValue("@login",loginLekarzaTxt.Text);
            querySprawdzeniePracownika.CommandText = polecenieSprawdzeniePracownika;
            querySprawdzeniePracownika.Connection = conn;

            SqlDataAdapter adapter = new SqlDataAdapter(querySprawdzeniePracownika);
            DataTable pracownicy = new DataTable();
            adapter.Fill(pracownicy);

            if(pracownicy.Rows.Count > 0)
            {
                if (pracownicy.Rows[0]["login"].ToString() == loginLekarzaTxt.Text)
                {
                    MessageBox.Show("Już istnieje taki pracownik o takim loginie");
                }
                if (pracownicy.Rows[0]["pesel"].ToString() == peselLekarzaTxt.Text)
                {
                    MessageBox.Show("Już istnieje taki pracownik o taki peselu");
                }
            }

            return pracownicy.Rows.Count;
        }

        private void EdytujPracownikaBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable zmiany = dt.GetChanges();
                if (zmiany != null)
                {
                    for (int i = 0; i < zmiany.Rows.Count; i++)
                    {
                        SqlCommand command = new SqlCommand
                        {
                            CommandText = "UPDATE Pracownicy Set imie = @imie, nazwisko=@nazwisko, adres=@adres, telefon=@telefon, stanowisko=@stanowisko, login=@login, haslo=@haslo where id=@id",
                            Connection = conn
                        };
                        command.Parameters.AddWithValue("@imie", zmiany.Rows[i]["imie"]);
                        command.Parameters.AddWithValue("@nazwisko", zmiany.Rows[i]["nazwisko"]);
                        command.Parameters.AddWithValue("@adres", zmiany.Rows[i]["adres"]);
                        command.Parameters.AddWithValue("@telefon", zmiany.Rows[i]["telefon"]);
                        command.Parameters.AddWithValue("@stanowisko", zmiany.Rows[i]["stanowisko"]);
                        command.Parameters.AddWithValue("@login", zmiany.Rows[i]["login"]);
                        command.Parameters.AddWithValue("@haslo", zmiany.Rows[i]["haslo"]);
                        command.Parameters.AddWithValue("@id", zmiany.Rows[i]["id"]);

                        //SqlCommandBuilder zmianyUpdate = new SqlCommandBuilder(dataAdapterEdycjaWizyt);
                        dataAdapterPracownicy.UpdateCommand = command;
                        dataAdapterPracownicy.Update(zmiany);
                    }
                    dt.AcceptChanges();
                    Label_PracownicyZmiany.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PracownicyView_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Label_PracownicyZmiany.Visibility = Visibility.Visible;
        }

        private void Btn_generuj_Click(object sender, RoutedEventArgs e)
        {
           
            string excel_file = "C:\\Users\\" + Environment.UserName + "\\Desktop\\Raport z dnia " + DateTime.Now.ToShortDateString() + ".xlsx";
            try
            {
                if (File.Exists(excel_file) == false)
                {
                    //zamiana danych z sql na obiekt datatable
                    string excel_command_string = "Select Pac.imie + ' ' + Pac.nazwisko as 'Pacjent', Pac.pesel as 'Pesel', P.imie + ' ' + P.nazwisko as 'Lekarz'," +
                                                    " W.data as 'Data', W.godzina as 'Godzina', W.status as 'Status wizyty'" +
                                                    "from Wizyty W JOIN Pracownicy P ON W.id_lekarza = P.id Join Pacjenci Pac ON w.id_pacjenta = Pac.id";
                    SqlCommand excel_command = new SqlCommand(excel_command_string, conn);
                    dataAdapterExcel = new SqlDataAdapter(excel_command);
                    excel_dt = new DataTable();
                    //wypełnienie obiektu datatable
                    dataAdapterExcel.Fill(excel_dt);

                    Excel.Application excel_object;
                    Excel.Workbook excel_workbook;
                    Excel.Worksheet excel_worksheet;
                    Excel.Range excel_range;
                    Excel.Borders excel_border, excel_rows_border;

                    //utworzenie(wystartowanie) nowego "arkusza" excela
                    excel_object = new Excel.Application();

                    //nowy plik zestaw excela
                    excel_workbook = excel_object.Workbooks.Add(Type.Missing);

                    excel_worksheet = (Excel.Worksheet)excel_workbook.ActiveSheet;
                    excel_worksheet.Name = "Raport";

                    //wypełnianie i formatowanie komórek
                    for (int i = 1; i < excel_dt.Columns.Count + 1; i++)
                    {
                        excel_range = (Excel.Range)excel_object.Cells[1, i];
                        excel_border = excel_range.Borders;
                        excel_border.LineStyle = Excel.XlLineStyle.xlContinuous;
                        excel_border.Weight = 3;
                        excel_range.Font.Bold = true;
                        excel_range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        excel_object.Cells[1, i] = excel_dt.Columns[i - 1].ColumnName;
                    }

                    for (int i = 0; i < excel_dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < excel_dt.Columns.Count; j++)
                        {
                            if (excel_dt.Rows[i][j] != null)
                            {
                                excel_range = (Excel.Range)excel_object.Cells[i + 2, j + 1];
                                excel_rows_border = excel_range.Borders;
                                excel_rows_border.LineStyle = Excel.XlLineStyle.xlContinuous;
                                excel_rows_border.Weight = 2;

                                excel_object.Cells[i + 2, j + 1] = excel_dt.Rows[i][j].ToString();
                            }
                        }
                    }
                    //automatyczne dopasowanie do tekstu
                    excel_object.Columns.AutoFit();

                
                    //zapis do pliku z podaną ścieżką
                    excel_object.ActiveWorkbook.SaveAs(excel_file);

                    //zakmniecie excela oraz jego procesu 
                    excel_workbook.Close(false);
                    excel_object.Quit();
                    Marshal.ReleaseComObject(excel_object);

                    MessageBox.Show("Utworzono nowy raport na Twoim pulpicie");
                }
                else
                {
                    MessageBox.Show("Raport w danym dniu został już utworzony");
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Wystąpił błąd");
            }

        }

        

        void DodawaniePracownika()
        {
            
            if (SprawdzeniePracownika() == 0)
            {
                try
                {
                    DataRow drwl = dswl.Tables["Pracownicy"].NewRow();
                    drwl["imie"] = imieLekarzaTxt.Text;
                    drwl["nazwisko"] = nazwiskoLekarzaTxt.Text;
                    drwl["pesel"] = peselLekarzaTxt.Text;
                    drwl["adres"] = adresLekarzaTxt.Text;
                    drwl["telefon"] = telefonLekarza.Text;
                    drwl["stanowisko"] = StanowiskoBox.SelectionBoxItem;
                    drwl["login"] = loginLekarzaTxt.Text;
                    drwl["haslo"] = hasloLekarzaTxt.Text;
                    

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
                imieLekarzaTxt.Clear();
                nazwiskoLekarzaTxt.Clear();
                peselLekarzaTxt.Clear();
                adresLekarzaTxt.Clear();
                telefonLekarza.Clear();
                loginLekarzaTxt.Clear();
                hasloLekarzaTxt.Clear();
            }

            
        }

        private void Wyloguj_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Czy na pewno?", "Wyloguj", MessageBoxButton.YesNo))
            {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
        }
    }
}
