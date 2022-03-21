using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Hotel_4
{
    public partial class ReportGuess : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        string[] bulan = { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public ReportGuess()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy";
            dateTimePicker1.ShowUpDown = true;
            lblname.Text = Model.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        private void panel_reservation_Click(object sender, EventArgs e)
        {
            Reservation reservation = new Reservation();
            this.Hide();
            reservation.ShowDialog();
        }

        private void panel_checkin_Click(object sender, EventArgs e)
        {
            CheckIn check = new CheckIn();
            this.Hide();
            check.ShowDialog();
        }

        private void panel_item_Click(object sender, EventArgs e)
        {
            RequestAdd request = new RequestAdd();
            this.Hide();
            request.ShowDialog();
        }

        private void panel_checkout_Click(object sender, EventArgs e)
        {
            CheckOut check = new CheckOut();
            this.Hide();
            check.ShowDialog();
        }

        private void panel_repcheck_Click(object sender, EventArgs e)
        {
            ReportCheckIn report = new ReportCheckIn();
            this.Hide();
            report.ShowDialog();
        }

        private void panel_guest_Click(object sender, EventArgs e)
        {
            ReportGuess report = new ReportGuess();
            this.Hide();
            report.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                this.Hide();
                main.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            chart1.ChartAreas[0].AxisX.Interval = 1;
            for (int i = 0; i < 12; i++)
            {
                int x = i + 1;
                int d = 0;
                for (int j = 0; j < 12; j++)
                {
                    connection.Open();
                    command = new SqlCommand("select count(id) as num from reservation where year(datetime) = " + dateTimePicker1.Value.ToString("yyyy") + " and month(datetime) = " + x + " group by month", connection);
                    reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        d = reader.GetInt32(0);
                    }
                    else
                    {
                        d = 0;
                    }
                    connection.Close();
                }

                chart1.Series["Guess(es)"].Points.Add(d);
                chart1.Series["Guess(es)"].Points[i].AxisLabel = bulan[i];
                chart1.Series["Guess(es)"].Points[i].LegendText = bulan[i];
                chart1.Series["Guess(es)"].Points[i].Label = d.ToString();
            }
        }
    }
}
