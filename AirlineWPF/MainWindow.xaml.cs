using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace AirlineWPF
{
    public partial class MainWindow : Window
    {
        public static int numup = 0, glob = 0, num = 0, tick, t = 1, eco = 1, ui = 0; public String day_val; public static String s = "", g = "", date = "";
        public static String uid = Window1.uid;
        public static string oradb = "server=127.0.0.1;User Id=root;port=3306;database=flight;Persist Security Info=True;Password=qwerty";
        public static bool check = false,com=false;
        public MainWindow()
        {
            InitializeComponent();
            com = false;
            //MessageBox.Show(com.ToString());
            uid = Window1.uid;
           // MessageBox.Show(uid + "       ");
            MySqlConnection conn = new MySqlConnection(oradb);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            dateSel.SelectedDate = DateTime.Today;
            dateSel.DisplayDateStart = DateTime.Today;
            conn.Open();
            ConnectionState s = new ConnectionState();
            s = conn.State;
            // MessageBox.Show(s.ToString());
            cmd.Connection = conn;

            //COMBOBOX1 DATA
            cmd.CommandText = "select DISTINCT(fro) from flight.flight";
            cmd.CommandType = CommandType.Text;
            MySqlDataReader dr6 = cmd.ExecuteReader();

            while (dr6.Read())
            {
                combo1.Items.Add(dr6.GetString(0).ToString());
            }
            conn.Close();
            conn.Open();


            // GETTING TICKET COUNT
            cmd.CommandText = "SELECT MAX(TICKET) FROM flight.PERSONAL";
            cmd.CommandType = CommandType.Text;
            MySqlDataReader dro1 = cmd.ExecuteReader();
            while (dro1.Read())
            {
                try
                { tick = (int)dro1.GetDecimal(0); }
                catch
                { tick = 100000; }
            }
            conn.Dispose();
        }


        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { }
        private void listView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { }

        public int frak(String g)
        {
            // CHECK FOR SEAT AVAILIBILITY
            numup = (int)numUpDown.Value;
            DateTime dt = new System.DateTime();
            dt = dateSel.SelectedDate.Value;
            day_val = String.Format("{0:yyyy-MM-dd}", dt);
            if (rad1.IsChecked == true)
            {
                eco = 1;
            }
            else
            {
                eco = 2;
            }
            try
             {
            num = int.Parse(g);
             
            MySqlConnection conn = new MySqlConnection(oradb);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select flight_no,dates,BOOKED_SEATS from flight.BOOK where dates='" + day_val + "' and flight_no=" + num;
            cmd.CommandType = CommandType.Text;
            MySqlDataReader tin = cmd.ExecuteReader();
            int iter = 0, seat_val = 0, e_seat_val = 0;
            while (tin.Read())
            {
                iter++;
            }
            conn.Close();
            if (iter == 0)
            {
                conn.Open();
                cmd.CommandText = "select econ_seat,seat from flight.flight where flight_no=" + num;
                cmd.CommandType = CommandType.Text;
                MySqlDataReader tin2 = cmd.ExecuteReader();
                while (tin2.Read())
                {
                    e_seat_val = (int)tin2.GetDecimal(0);
                    seat_val = (int)tin2.GetDecimal(1);
                }
                conn.Close();
                conn.Open();
                cmd.CommandText = "insert into flight.BOOK values(" + num + ",'" + day_val + "'," + e_seat_val + "," + seat_val + ")";
                cmd.ExecuteNonQuery();
            }
            else
            { }
            if (eco == 1)
            {
                cmd.Connection = conn;
                conn.Close();
                conn.Open();
                cmd.CommandText = "select ECON_BOOKED_SEATS from flight.book where flight_no=" + num + " and dates='" + day_val + "'";
                cmd.CommandType = CommandType.Text;
                MySqlDataReader dr = cmd.ExecuteReader();
                int seat = 0;
                while (dr.Read())
                {
                    seat = (int)dr.GetDecimal(0);
                }

                //CHECKING FOR SEAT AVAILABILITY(ON FLIGHT)
                if (seat >= numup && ui==0)
                {
                    
                        //INSERTING PERSONAL DETAILS
                        tick += t; 
                        conn.Close();
                        conn.Open();
                        cmd.CommandText = "INSERT INTO flight.PERSONAL VALUES(" + num + ",'" + day_val + "'," + numup + "," + tick + ",'" + uid + "',0)";
                        int rowsUpdated = cmd.ExecuteNonQuery();

                        //UPDATING BOOKING STATUS
                        conn.Close();
                        conn.Open();
                        cmd.CommandText = "update flight.book set ECON_BOOKED_SEATS=ECON_BOOKED_SEATS-" + numup + " where flight_no=" + num + " and dates='" + day_val + "'";
                        cmd.CommandType = CommandType.Text;
                        MySqlDataReader dro = cmd.ExecuteReader();
                        conn.Dispose();
                        return 2;
                }
                else
                {
                    String sMessageBoxText = "Seat/s not available on Flight No. " + num;
                    string sCaption = "Airline Reservation System";
                    MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    return 1; ui = 1;
                }
            }
            else
            {
                cmd.Connection = conn;
                conn.Close();
                conn.Open();
                cmd.CommandText = "select BOOKED_SEATS from flight.book where flight_no=" + num + " and dates='" + day_val + "'";
                cmd.CommandType = CommandType.Text;
                MySqlDataReader dr = cmd.ExecuteReader();
                int seat = 0;
                while (dr.Read())
                {
                    seat = (int)dr.GetDecimal(0);
                }

                //CHECKING FOR SEAT AVAILABILITY(ON FLIGHT)
                if (seat >= numup)
                {
                    
                    //INSERTING PERSONAL DETAILS
                    
                        tick += t;
                        conn.Close();
                        conn.Open();
                        cmd.CommandText = "INSERT INTO flight.PERSONAL VALUES(" + num + ",'" + day_val + "',0," + tick + ",'" + uid + "'," + numup + ")";
                        int rowsUpdated = cmd.ExecuteNonQuery();

                        //UPDATING BOOKING STATUS
                        conn.Close();
                        conn.Open();
                        cmd.CommandText = "update flight.book set BOOKED_SEATS=BOOKED_SEATS-" + numup + " where flight_no=" + num + " and dates='" + day_val + "'";
                        cmd.CommandType = CommandType.Text;
                        MySqlDataReader dro = cmd.ExecuteReader();
                        conn.Dispose();
                        return 2;
                }
                else
                {
                    String sMessageBoxText = "Seat/s not available on Flight No. " + num;
                    string sCaption = "Airline Reservation System";
                    MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Error;
                    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                    return 1;
                }
            }
        }
             catch
             {
                 MessageBox.Show("Please select a flight .");
                 return 0;
             }
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            glob = 0; ui = 0;
            listView1.Items.Clear();
            numup = (int)numUpDown.Value;
            DateTime dt = new System.DateTime();
            dt = dateSel.SelectedDate.Value;
            day_val = String.Format("{0:ddd}", dt);
            String day_val2 = String.Format("{0:yyyy-MM-dd}", dt);
             
            MySqlConnection conn = new MySqlConnection(oradb);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            if (check == true)
            {
                cmd.CommandText = "SELECT DISTINCT FLIGHT.FLIGHT_NO FROM FLIGHT,DAYS WHERE FLIGHT.FLIGHT_NO=DAYS.FLIGHT_NO AND FRO='" + combo1.Text + "' AND " + day_val + " IS NOT NULL";
                cmd.CommandType = CommandType.Text;
                MySqlDataReader der = cmd.ExecuteReader();
                int[] fl = new int[20];
                int c = 0;
                while (der.Read())
                {
                    fl[c] = (int)der.GetDecimal(0);
                    c++;
                }
                for (int x = 0; x <= c; x++)
                {
                    conn.Close();
                    conn.Open();
                    cmd.CommandText = "SELECT COUNT(FLIGHT_NO) FROM BOOK WHERE FLIGHT_NO=" + fl[x] + " AND DATES='" + day_val2 + "'";
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader to = cmd.ExecuteReader();
                    int yu = 0; String econ = "", comb = "";
                    while (to.Read())
                    {
                        yu = (int)to.GetDecimal(0);
                    }
                    if (yu == 0)
                    {
                        econ = "ECON_SEAT,SEAT FROM"; comb = "FLIGHT.FLIGHT_NO=" + fl[x];
                    }
                    else
                    {
                        econ = "ECON_BOOKED_SEATS,BOOKED_SEATS FROM BOOK,"; comb = " BOOK.FLIGHT_NO=FLIGHT.FLIGHT_NO AND BOOK.FLIGHT_NO=" + fl[x] + " AND DATES='" + day_val2 + "'";
                    }
                    conn.Close();
                    conn.Open();
                    String flig = "";
                    cmd.CommandText = "SELECT DISTINCT FLIGHT.FLIGHT_NO,CARRIER,FRO,DESTIN,DEP_TIME,ARR_TIME," + econ + " FLIGHT,DAYS WHERE " + comb;
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader dere = cmd.ExecuteReader();
                    while (dere.Read())
                    {
                        flig = "1.      " + dere.GetDecimal(0).ToString() + "      " + dere.GetString(1) + "      " + dere.GetString(2) + "      " + dere.GetString(3) + "      " + dere.GetString(4) + "      " + dere.GetString(5) + "      " + dere.GetDecimal(6).ToString() + "      " + dere.GetDecimal(7).ToString() + "      ";
                    }
                    conn.Close();
                    conn.Open();
                    cmd.CommandText = "SELECT COUNT(FLIGHT_NO) FROM BOOK WHERE FLIGHT_NO=" + fl[x] + " AND DATES='" + day_val2 + "'";
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader too = cmd.ExecuteReader();
                    yu = 0; econ = ""; comb = "";
                    while (too.Read())
                    {
                        yu = (int)too.GetDecimal(0);
                    }
                    if (yu == 0)
                    {
                        econ = "ECON_SEAT,SEAT FROM"; comb = "";
                    }
                    else
                    {
                        econ = "ECON_BOOKED_SEATS,BOOKED_SEATS FROM BOOK,"; comb = " BOOK.FLIGHT_NO=FLIGHT.FLIGHT_NO AND DATES='" + day_val2 + "' AND";
                    }
                    conn.Close();
                    conn.Open();
                    cmd.CommandText = "SELECT DISTINCT FLIGHT.FLIGHT_NO,CARRIER,FRO,DESTIN,DEP_TIME,ARR_TIME," + econ + " FLIGHT,days WHERE " + comb + " FRO=(SELECT DESTIN FROM FLIGHT WHERE FLIGHT_NO=" + fl[x] + ")AND DESTIN='" + combo2.Text + "'AND " + day_val + "=1 AND TIMEDIFF(DEP_TIME,'01:00:00')>(SELECT ARR_TIME FROM FLIGHT WHERE FLIGHT_NO=" + fl[x] + ")";
                    cmd.CommandType = CommandType.Text;
                    MySqlDataReader dereo = cmd.ExecuteReader();
                    while (dereo.Read())
                    {
                        glob++;
                        flig = String.Concat(flig, "2.      " + dereo.GetDecimal(0).ToString() + "      " + dereo.GetString(1) + "      " + dereo.GetString(2) + "      " + dereo.GetString(3) + "      " + dereo.GetString(4) + "      " + dereo.GetString(5) + "      " + dereo.GetDecimal(6).ToString() + "      " + dereo.GetDecimal(7).ToString() + "      \n");
                        String[] fg = Regex.Split(flig, "      ");
                        listView1.Items.Add(new { Col1 = glob.ToString(), Col2 = fg[1] + "\n" + fg[10], Col3 = fg[2] + "\n" + fg[11], Col4 = fg[3] + "\n" + fg[12], Col5 = fg[4] + "\n" + fg[13], Col6 = fg[5] + "\n" + fg[14], Col7 = fg[6] + "\n" + fg[15], Col8 = fg[7] + "\n" + fg[16], Col9 = fg[8] + "\n" + fg[17] });
                        flig = "";
                        break;
                    }
                }
            }
            else
            {
                glob = 0;

                //CLEARING TEMP TABLES
                cmd.CommandText = "truncate table flight.TEMP2";
                cmd.CommandType = CommandType.Text;
                MySqlDataReader dr = cmd.ExecuteReader();
                conn.Close();
                conn.Open();
                cmd.CommandText = "truncate table flight.TEMP";
                cmd.CommandType = CommandType.Text;
                dr = cmd.ExecuteReader();


                //QUERYING FOR AVAILABLE FLIGHTS
                conn.Close();
                conn.Open();
                cmd.CommandText = "INSERT INTO flight.TEMP(FLIGHT_NO,CARRIER,FRO,DESTIN,DEP_TIME,ARR_TIME,ECON_SEATING,SEATING,FARE)SELECT FLIGHT.FLIGHT_NO,CARRIER,FRO,DESTIN,DEP_TIME,ARR_TIME,ECON_SEAT,SEAT,FARE FROM flight.FLIGHT,flight.DAYS WHERE FLIGHT.FLIGHT_NO=DAYS.FLIGHT_NO AND FRO='" + combo1.Text + "' AND DESTIN='" + combo2.Text + "'AND " + day_val + " IS NOT NULL";
                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Open();
                cmd.CommandText = "INSERT INTO flight.TEMP2(SELECT TEMP.FLIGHT_NO,CARRIER,FRO,DESTIN,DEP_TIME,ARR_TIME,ECON_BOOKED_SEATS,BOOKED_SEATS,FARE FROM flight.BOOK,flight.TEMP WHERE DATES='" + day_val2 + "' AND BOOK.FLIGHT_NO=TEMP.FLIGHT_NO)";
                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Open();
                cmd.CommandText = "INSERT INTO flight.TEMP2 (SELECT DISTINCT TEMP.FLIGHT_NO,TEMP.CARRIER,TEMP.FRO,TEMP.DESTIN,TEMP.DEP_TIME,TEMP.ARR_TIME,ECON_SEATING,SEATING,TEMP.FARE FROM flight.TEMP LEFT JOIN flight.TEMP2 ON TEMP.FLIGHT_NO=TEMP2.FLIGHT_NO WHERE TEMP2.FLIGHT_NO IS NULL)";
                cmd.ExecuteNonQuery();
                conn.Close();
                conn.Open();
                cmd.CommandText = "SELECT FLIGHT_NO,CARRIER,FRO,DESTIN,DEP_TIME,ARR_TIME,ECON_SEATS,SEATS,FARE FROM flight.TEMP2 ORDER BY FLIGHT_NO";
                cmd.CommandType = CommandType.Text;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    glob++;
                    listView1.Items.Add(new { Col1 = glob.ToString(), Col2 = dr.GetDecimal(0).ToString(), Col3 = dr.GetString(1), Col4 = dr.GetString(2), Col5 = dr.GetString(3), Col6 = dr.GetString(4), Col7 = dr.GetString(5), Col8 = dr.GetDecimal(6).ToString(), Col9 = dr.GetDecimal(7).ToString() });
                }
                conn.Dispose();
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                String sel = listView1.SelectedValue.ToString();
                String[] fgo = Regex.Split(sel, "Col2 = ");
                String[] golo = new String[2];
                string sMessageBoxText = "Do you want to continue with the Booking ?";
                string sCaption = "Airline Reservation System";
                MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
                MessageBoxImage icnMessageBox = MessageBoxImage.Question;
                MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
                btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage mi = MessageBoxImage.Information;
                if (rsltMessageBox == MessageBoxResult.Yes)
                {
                    if (check == true)
                    {
                        golo[0] = fgo[1].Substring(0, 4);
                        golo[1] = fgo[1].Substring(5, 4);
                        int r = frak(golo[0]);
                        r = r + frak(golo[1]);
                        if (r == 4)
                        {
                            sMessageBoxText = "Tickets Booked \n Safe Travels !!";
                            MessageBoxResult rs = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, mi);
                        }
                    }
                    else
                    {
                        golo[0] = fgo[1].Substring(0, 4);
                        int r = frak(golo[0]);
                        if (r == 2)
                        {
                            sMessageBoxText = "Ticket Booked                 \n Safe Travels !!";
                            MessageBoxResult rs = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, mi);
                        }
                    }
                    listView1.Items.Clear();
                }
                else
                {
                    sMessageBoxText = "Ticket not Booked";
                    MessageBoxResult rs = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, mi);
                }
            }
            catch
            {

                string sMessageBoxText = "Please Select a Flight first .... ";
                string sCaption = "Airline Reservation System";
                MessageBoxButton btnMessageBox = MessageBoxButton.OK;
                MessageBoxImage mi = MessageBoxImage.Stop;
                MessageBoxResult rs = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, mi);
            }

        }

        private void combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listView1.Items.Clear();
            refresh();
            com = true;
        }

        private void ckb1_Checked(object sender, RoutedEventArgs e)
        {
            check = true;
            if(com==true)
            {
                listView1.Items.Clear();
                refresh();
            }
        }

        private void ckb1_Un_Checked(object sender, RoutedEventArgs e)
        {
            check = false;
            if (com == true)
            {
                listView1.Items.Clear();
                refresh();
            }
        }

        private void refresh()
        {

            //COMBOBOX2 DATA
            combo2.Items.Clear();
             
            MySqlConnection conn = new MySqlConnection(oradb);
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            conn.Open();
            if (check == true)
            {
                cmd.CommandText = "select DISTINCT(destin) from flight.flight WHERE NOT DESTIN='" + combo1.SelectedValue.ToString() + "'";
                cmd.CommandType = CommandType.Text;
                MySqlDataReader dr33 = cmd.ExecuteReader();
                while (dr33.Read())
                {
                    combo2.Items.Add(dr33.GetString(0).ToString());
                }
            }
            else
            {
                cmd.CommandText = "select DISTINCT(DESTIN) from flight.flight WHERE FRO='" + combo1.SelectedItem.ToString() + "'";
                cmd.CommandType = CommandType.Text;
                MySqlDataReader dr6 = cmd.ExecuteReader();

                while (dr6.Read())
                {
                    combo2.Items.Add(dr6.GetString(0).ToString());
                }
            }
            conn.Close();
            conn.Open();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Window1 w = new Window1();
            w.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listView2.Items.Clear();
             
            MySqlConnection conn = new MySqlConnection(oradb);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT DISTINCT TICKET,PERSONAL.FLIGHT_NO,FLIGHT.CARRIER,DAT,FRO,DESTIN,DEP_TIME,ARR_TIME,ECON_NO_SEATS,NO_SEATS FROM FLIGHT.BOOK,FLIGHT.LOGIN,FLIGHT.PERSONAL,FLIGHT.FLIGHT WHERE PERSONAL.UD='" + uid + "'AND PERSONAL.FLIGHT_NO=FLIGHT.FLIGHT_NO";
            cmd.CommandType = CommandType.Text;
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listView2.Items.Add(new { Col1 = dr.GetDecimal(0).ToString(), Col2 = dr.GetDecimal(1).ToString(), Col3 = dr.GetString(2), Col4 = dr.GetDateTime(3).ToShortDateString(), Col5 = dr.GetString(4), Col6 = dr.GetString(5), Col7 = dr.GetString(6), Col8 = dr.GetString(7), Col9 = dr.GetDecimal(8).ToString(), Col10 = dr.GetDecimal(9).ToString() });
            }
            conn.Close();
            conn.Open();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Do you want to continue with the cancellation ?";
            string sCaption = "Airline Reservation System";
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;
            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            if (rsltMessageBox == MessageBoxResult.Yes)
            {

                MySqlConnection conn = new MySqlConnection(oradb);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                String sel = listView2.SelectedItem.ToString();
                String[] golo = new String[12];
                golo = Regex.Split(sel, "Col");
                golo[1] = golo[1].Substring(4, 6);
                golo[2] = golo[2].Substring(4, 4);
                golo[4] = golo[4].Substring(4, 10);
                golo[9] = golo[9].Substring(4, 1);
                golo[10] = golo[10].Substring(5, 1);
                String[] a = new string[3];
                a = golo[4].Split('-');
                date = ""; date = a[2] + "-" + a[1] + "-" + a[0];
                cmd.CommandText = "DELETE FROM PERSONAL WHERE TICKET=" + golo[1];
                cmd.CommandType = CommandType.Text;
                MySqlDataReader dr = cmd.ExecuteReader();
                conn.Close();
                conn.Open();
                cmd.CommandText = "update flight.book set ECON_BOOKED_SEATS=ECON_BOOKED_SEATS+" + golo[9] + ", BOOKED_SEATS=BOOKED_SEATS+" + golo[10] + " where flight_no=" + golo[2] + " and dates='" + date + "'";
                cmd.CommandType = CommandType.Text;
                MySqlDataReader drq = cmd.ExecuteReader();
                MessageBox.Show("Ticket Cancelled");
                conn.Close();
                listView2.Items.Clear();
            }
        }
        private void bt3_Click_1(object sender, RoutedEventArgs e)
        {
            listView3.Items.Clear();
            MySqlConnection conn = new MySqlConnection(oradb);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT DISTINCT TICKET,PERSONAL.FLIGHT_NO,FLIGHT.CARRIER,DAT,FRO,DESTIN,DEP_TIME,ARR_TIME,ECON_NO_SEATS,NO_SEATS FROM FLIGHT.BOOK,FLIGHT.LOGIN,FLIGHT.PERSONAL,FLIGHT.FLIGHT WHERE PERSONAL.UD='" + uid + "'AND PERSONAL.FLIGHT_NO=FLIGHT.FLIGHT_NO";
            cmd.CommandType = CommandType.Text;
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listView3.Items.Add(new { Col1 = dr.GetDecimal(0).ToString(), Col2 = dr.GetDecimal(1).ToString(), Col3 = dr.GetString(2), Col4 = dr.GetDateTime(3).ToShortDateString(), Col5 = dr.GetString(4), Col6 = dr.GetString(5), Col7 = dr.GetString(6), Col8 = dr.GetString(7), Col9 = dr.GetDecimal(8).ToString(), Col10 = dr.GetDecimal(9).ToString() });
            }
            conn.Close();
            conn.Open();
        }
    }
}