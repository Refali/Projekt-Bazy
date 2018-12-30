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
            status();
            fill_DataGrid_Wizyty();
        }
        void fill_DataGrid_Wizyty()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                string polecenie = "select * from Wizyty where id_lekarza=@idlekarza";
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
                    SqlCommandBuilder zmianyUpdate = new SqlCommandBuilder(dataAdapterEdycjaWizyt);
                    int zmienioneWiersze = dataAdapterEdycjaWizyt.Update(zmiany);
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void status()
        {
            statusLbl.Content = conn.State.ToString();
            userLbl.Content = userName;
        }
    }
}
