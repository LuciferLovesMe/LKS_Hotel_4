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
    public partial class RequestAdd : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        
        public RequestAdd()
        {
            InitializeComponent();
            loadroom();
            loaditem();

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            lblname.Text = Model.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        void loadroom()
        {
            string com = "select reservationRoom.id, room.roomNumber from reservationRoom join room on room.id = reservationRoom.roomId where room.status = 'unavail'";
            comboBox1.DisplayMember = "roomNumber";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = Command.getdata(com);
        }

        void loaditem()
        {
            string com = "select * from item";
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "name";
            comboBox2.DataSource = Command.getdata(com);
            getPrice();
        }

        void getPrice()
        {
            command = new SqlCommand("select requestPrice from item where id = " + comboBox2.SelectedValue, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            textBox1.Text = reader.GetInt32(0).ToString();
            textBox2.Text = (reader.GetInt32(0) * numericUpDown1.Value).ToString();
            connection.Close();
        }

        int countTotal()
        {
            int t = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                t += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);
            }

            return t;
        }

        void clear()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            lbltotal.Text = countTotal().ToString();
            numericUpDown1.Value = 0;
            getPrice();
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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            getPrice();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(comboBox2.Text.Length > 0 && numericUpDown1.Value > 0)
            {
                int rows = dataGridView1.Rows.Add();
                dataGridView1.Rows[rows].Cells[0].Value = comboBox2.SelectedValue;
                dataGridView1.Rows[rows].Cells[1].Value = comboBox2.Text;
                dataGridView1.Rows[rows].Cells[2].Value = textBox1.Text;
                dataGridView1.Rows[rows].Cells[3].Value = numericUpDown1.Value;
                dataGridView1.Rows[rows].Cells[4].Value = textBox2.Text;

                lbltotal.Text = countTotal().ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);

                lbltotal.Text = countTotal().ToString();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount > 0)
            {
                try
                {
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        command = new SqlCommand("insert into reservationRequestItem values(" + comboBox1.SelectedValue + ", " + dataGridView1.Rows[i].Cells[0].Value + ", " + dataGridView1.Rows[i].Cells[3].Value + ", " + dataGridView1.Rows[i].Cells[4].Value + ")", connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex);
                }
                finally
                {
                    connection.Close();
                }
                
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            getPrice();
        }
    }
}
