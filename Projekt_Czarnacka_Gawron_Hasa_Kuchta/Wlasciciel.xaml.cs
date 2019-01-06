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

using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.ComponentModel;

namespace Projekt_Czarnacka_Gawron_Hasa_Kuchta
{
    public partial class Wlasciciel : Window
    {
        int i;
        SqlDataAdapter dataAdapterWl, dataAdapterPracownicy, dataAdapterExcel;
        DataSet dswl;
        DataTable dt, excel_dt;
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

            cb_okres.Items.Add("Wybierz Miesiąc");
            cb_okres.Items.Add("Styczeń");
            cb_okres.Items.Add("Luty");
            cb_okres.Items.Add("Marzec");
            cb_okres.Items.Add("Kwiecień");
            cb_okres.Items.Add("Maj");

        }
        private void nowyLekarz_Click(object sender, RoutedEventArgs e)
        {
            PanelDodawanieLekarza.Visibility = Visibility.Visible;
            PanelPracownikow.Visibility = Visibility.Collapsed;
            PanelPracownikow.Visibility = Visibility.Collapsed;
            this.Width = 352.594;
            this.Height = 450;
        }

        private void Raport_Click(object sender, RoutedEventArgs e)
        {
            PanelPracownikow.Visibility = Visibility.Collapsed;
            PanelRaportów.Visibility = Visibility.Visible;
            PanelDodawanieLekarza.Visibility = Visibility.Collapsed;
            this.Width = 352.594;
            this.Height = 450;
        }

        private void Pracownicy_Click(object sender, RoutedEventArgs e)
        {
            fill_DataGrid_Pracownicy();
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

        private void PressEnter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                DodawaniePracownika();
            }
        }

        void SprawdzeniePracownika()
        {
            i = 0;
            SqlCommand querySprawdzeniePracownika = new SqlCommand();
            string polecenieSprawdzeniePracownika = "select login,pesel from Pracownicy where pesel=@pesel OR login=@login";
            querySprawdzeniePracownika.Parameters.AddWithValue("@pesel",peselLekarzaTxt.Text);
            querySprawdzeniePracownika.Parameters.AddWithValue("@login",loginLekarzaTxt.Text);
            querySprawdzeniePracownika.CommandText = polecenieSprawdzeniePracownika;
            querySprawdzeniePracownika.Connection = conn;

            SqlDataReader readerSprawdzeniePracownika= querySprawdzeniePracownika.ExecuteReader();

            while (readerSprawdzeniePracownika.Read() && i == 0)
            {
                if(readerSprawdzeniePracownika.GetString(0) == loginLekarzaTxt.Text)
                {
                    MessageBox.Show("Już istnieje taki pracownik o takim loginie");
                }
                if(readerSprawdzeniePracownika.GetString(1) == peselLekarzaTxt.Text)
                {
                    MessageBox.Show("Już istnieje taki pracownik o taki peselu");
                }
                i++;

            }
            readerSprawdzeniePracownika.Close();

        }

        private void Btn_generuj_Click(object sender, RoutedEventArgs e)
        {
            /*
             * Muszę tutaj dodać jeszcze odnośniki do poszczególnych miesięcy 
             * ale muszę przemyśleć zapytanie sql 
             * w senie zrobić użytek z tego comboboxa 
            */

            //zamiana danych z sql na obiekt datatable
            string excel_command_string = "Select Pac.imie + ' ' + Pac.nazwisko as 'Pacjent', Pac.pesel as 'Pesel', P.imie + ' ' + P.nazwisko as 'Lekarz'," +
                                            " W.data as 'Data', W.godzina as 'Godzina', W.status as 'Status wizyty'" +
                                            "from Wizyty W JOIN Pracownicy P ON W.id_lekarza = P.id Join Pacjenci Pac ON w.id_pacjenta = Pac.id";
            SqlCommand excel_command = new SqlCommand(excel_command_string, conn);
            dataAdapterExcel = new SqlDataAdapter(excel_command);
            excel_dt = new DataTable();
            //wypełnienie obiektu datatable
            excel_dt.Load(excel_command.ExecuteReader());

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

            //SaveCopyAs bo SaveAs sprawia że nie mogę otworzyć pliku
            //Environment.UserName pobiera nawzę użytkownika
            excel_object.ActiveWorkbook.SaveCopyAs("C:\\Users\\"+ Environment.UserName + "\\Desktop\\Raport z dnia " + DateTime.Now.ToShortDateString() + ".xlsx");
            MessageBox.Show("Utworzono nowy raport na Twoim pulpicie");

            //myślę nad innym spodobem na to ale jeszcze nie wiem
            foreach (Process proc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
            {
                proc.Kill();
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
            }

        }

        
    }
}
