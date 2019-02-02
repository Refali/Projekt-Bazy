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
    
    public partial class Lekarz : Window
    {
        SqlConnection conn;
        string userName,idLekarza;
        SqlDataAdapter dataAdapterEdycjaWizyt;
        DataTable dt;
        public Lekarz()
        {
            InitializeComponent();
        }
        public Lekarz(SqlConnection conn, string userName,string idLekarza)
        {
            InitializeComponent();

            this.conn = conn;
            this.userName = userName;
            this.idLekarza = idLekarza;
            Status();
            Fill_DataGrid_Wizyty();
        }
        void Fill_DataGrid_Wizyty()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                //string polecenie = "select * from Wizyty where id_lekarza=@idlekarza ";
                string polecenie = "select W.id, P.imie + ' ' + P.nazwisko as 'Lekarz', Pa.imie + ' ' + Pa.nazwisko as 'Pacjent', W.data, W.godzina, w.status, w.uwagi from Wizyty W JOIN Pracownicy P on W.id_lekarza = P.id Join Pacjenci PA on PA.id = W.id_pacjenta where id_lekarza = @idlekarza ";
                
                cmd.CommandText = polecenie;
                cmd.Parameters.AddWithValue("@idlekarza",idLekarza);
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

        private void ZapiszEdycje_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable zmiany = dt.GetChanges();
                if (zmiany != null)
                {
                    for(int i = 0; i < zmiany.Rows.Count; i++)
                    {
                        SqlCommand command = new SqlCommand
                        {
                            CommandText = "UPDATE wizyty Set Status = @Status, Uwagi=@Uwagi where id=@id",
                            Connection = conn
                        };
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Status()
        {
          
            userLbl.Content = userName;
        }

        private void WizytyView_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Label_WizytyZmiany.Visibility = Visibility.Visible;
        }

        private void TxtWyszukaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtWyszukaj.Text != "")
            {
                
                dt.DefaultView.RowFilter = "Pacjent LIKE '%" + txtWyszukaj.Text + "%'";
                WizytyView.ItemsSource = dt.DefaultView;
            }
            else
            {
                dt.DefaultView.RowFilter = "Pacjent LIKE '%'";
                WizytyView.ItemsSource = dt.DefaultView;
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
