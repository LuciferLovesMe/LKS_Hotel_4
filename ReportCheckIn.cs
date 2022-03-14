using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Hotel_4
{
    public partial class ReportCheckIn : Form
    {
        Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();

        public ReportCheckIn()
        {
            InitializeComponent();
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
            if(dataGridView1.RowCount > 0)
            {
                application.Workbooks.Add(Type.Missing);
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    application.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        application.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }

                application.Columns.AutoFit();
                application.Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(dateTimePicker1.Value.Date > dateTimePicker2.Value.Date)
            {
                MessageBox.Show("Insert a correct date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string com;
                if (checkBox1.Checked)
                {
                    com = "select reservation.id, reservation.bookingCode, reservationRoom.startDateTime, reservationRoom.durationNights, reservationRoom.checkInDateTime, reservationRoom.checkOutDateTime, reservationRoom.roomPrice, room.roomNumber, room.roomFloor, room.description from reservation join reservationRoom on reservation.id = reservationRoom.reservationId join room on reservationroom.roomId = room.id where reservationRoom.checkInDatetime = '"+DateTime.Today+"'";
                }
                else
                {
                    com = "select reservation.id, reservation.bookingCode, reservationRoom.startDateTime, reservationRoom.durationNights, reservationRoom.checkInDateTime, reservationRoom.checkOutDateTime, reservationRoom.roomPrice, room.roomNumber, room.roomFloor, room.description from reservation join reservationRoom on reservation.id = reservationRoom.reservationId join room on reservationroom.roomId = room.id where reservationRoom.checkInDatetime >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and reservationRoom.checkoutdatetime <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'";
                }
                dataGridView1.DataSource = Command.getdata(com);
            }
        }
    }
}
