﻿using System;
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
    public partial class MainWindow : Window
    {
        SqlConnection conn;
        SqlConnectionStringBuilder connString;
        string idLekarza;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            logow();
        }

        private void UserTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                logow();
            }
        }

        private void logow()
        {
            try
            {
                connString = new SqlConnectionStringBuilder();
                connString.InitialCatalog = "Firma";

                //connString.DataSource = "localhost";
                connString.DataSource = @"DESKTOP-SLTS1AQ\SQLEXPRESS";

                connString.IntegratedSecurity = true;
                conn = new SqlConnection(connString.ConnectionString);
                conn.Open();

                
                SqlCommand query = new SqlCommand();
                string polecenie = "Select Stanowisko,id from Pracownicy where login=@login and haslo=@password";

                query.Parameters.AddWithValue("@login", userTxt.Text);
                query.Parameters.AddWithValue("@password", passwordTxt.Password);
                query.CommandText = polecenie;
                query.Connection = conn;

                SqlDataReader reader = query.ExecuteReader();

                int ilosc = 0;
                string stanowisko = "";

                while (reader.Read())
                {
                    ilosc++;
                    stanowisko = reader.GetString(0).Replace(" ", "");
                    idLekarza = reader.GetInt32(1).ToString();
                }
                reader.Close();

                if (ilosc >= 1)
                {
                    if (stanowisko == "Recepcjonistka" || stanowisko == "Recepcja")
                    {
                        Recepcja recepcjaOkno = new Recepcja(conn, userTxt.Text);
                        recepcjaOkno.Show();
                        this.Close();
                    }
                    else if (stanowisko == "Lekarz")
                    {
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
                    else
                    {
                        conn.Close();

                    }
                }

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
