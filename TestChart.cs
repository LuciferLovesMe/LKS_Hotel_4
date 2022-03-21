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
    public partial class TestChart : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        string[] bulan = { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public TestChart()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy";
            dateTimePicker1.ShowUpDown = true;
            loadchart();
            comboBox1.DataSource = bulan;


        }

        void loadchart()
        {
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
                
                chart1.Series["Total"].Points.Add(d);
                chart1.Series["Total"].Points[i].AxisLabel = bulan[i];
                chart1.Series["Total"].Points[i].LegendText = bulan[i];
                chart1.Series["Total"].Points[i].Label = d.ToString() + " Guesses";
            }


//            chart1.Series["Total"].Points.Add(1);
            //chart1.Series["Total"].Points[0].AxisLabel = "Jan";
            //chart1.Series["Total"].Points[0].LegendText = "Jan";
            //chart1.Series["Total"].Points[0].Label = "1 Guesses";

            //chart1.Series["Total"].Points.Add(3);
            //chart1.Series["Total"].Points[1].AxisLabel = "Feb";
            //chart1.Series["Total"].Points[1].LegendText = "Feb";
            //chart1.Series["Total"].Points[1].Label = "3 Guesses";

            //chart1.Series["Total"].Points.Add(reader.GetInt32(0));
            //chart1.Series["Total"].Points[2].AxisLabel = "Mar";
            //chart1.Series["Total"].Points[2].LegendText = "Mar";
            //chart1.Series["Total"].Points[2].Label = reader.GetInt32(0).ToString() + " Guesses";
        }
    }
}
