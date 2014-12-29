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
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace AirlineWPF
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Window1 w = new Window1();
            w.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool i = false;
            String oradb = "server=127.0.0.1;User Id=root;port=3306;database=flight;Persist Security Info=True;Password=qwerty";
            MySqlConnection conn = new MySqlConnection(oradb);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select COUNT(UD) from flight.LOGIN WHERE UD='" + b7.Text + "'";
            cmd.CommandType = CommandType.Text;
            MySqlDataReader drj = cmd.ExecuteReader();
            int exist = 0;
            while (drj.Read())
            {
                exist = (int)drj.GetDecimal(0);
            }
            conn.Dispose();
            if (Regex.IsMatch(b1.Text, @"Dd") == true || Regex.IsMatch(b2.Text, @"Dd")==true)
            {
                i = true;
            }
            //MessageBox.Show(i.ToString());
            if (exist == 0 && i==false)
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = "INSERT INTO flight.LOGIN VALUES('" + b7.Text + "','" + b8.Password + "','" + b1.Text + "','" + b2.Text + "','" + b3.Text + "','" + b5.Text + "','" + b6.Text + "','" + b4.Text + "')";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User " + b7.Text + " created");
                    this.Close();
                }
                catch (MySqlException e1)
                {
                    String sMessageBoxText = "Only Numbers allowed in Phone field";
                    string sCaption = "Airline Reservation System";
                    MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                }
                catch (Exception e1)
                {
                    String sMessageBoxText = "Error "+e1.ToString();
                    string sCaption = "Airline Reservation System";
                    MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                }
            }
            else if (i == true)
            {
                //MessageBox.Show("Only alphabets allowed in First Name and Last Name");
                string sMessageBoxText = "Only alphabets allowed in First Name and Last Name";
                string sCaption = "Airline Reservation System";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
            else
            {
                //MessageBox.Show("Enter a different User Name .");
                String sMessageBoxText = "Enter a different User Name";
                string sCaption = "Airline Reservation System";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            }
            conn.Dispose();
        }
    }
}
