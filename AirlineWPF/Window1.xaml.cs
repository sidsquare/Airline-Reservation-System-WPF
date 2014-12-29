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

namespace AirlineWPF
{
    
    public partial class Window1 : Window
    {
        public static String uid = "", pw = "";
        public Window1()
        {
            InitializeComponent();
        }

        private void bt1_Click(object sender, RoutedEventArgs e)
        {
            string oradb = "server=127.0.0.1;User Id=root;port=3306;database=flight;Persist Security Info=True;Password=qwerty";
            MySqlConnection conn = new MySqlConnection(oradb);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection=conn;
            cmd.CommandText = "SELECT COUNT(UD) FROM flight.LOGIN WHERE UD='"+tb1.Text+"' AND PWD='"+pb1.Password+"'";
            cmd.CommandType = CommandType.Text;
            MySqlDataReader ro = cmd.ExecuteReader(); int t = 0;
            while (ro.Read())
            {
                t = (int)ro.GetDecimal(0); 
            }
            conn.Dispose();
            if(t==1)
            {
                uid = tb1.Text;
                pw = pb1.Password;
                conn.Dispose();
                this.IsEnabled = false;
                MainWindow mw=new MainWindow();
                mw.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login Information Incorrect .");
            }
        }

        private void bt2_Click(object sender, RoutedEventArgs e)
        {
            Window2 we = new Window2();
            we.Show();
            this.IsEnabled = false;
            this.Close();
        }

    }
}
