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
    public partial class Reservation : Form
    {
        int idReser, idCust, idRoom;
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public Reservation()
        {
            InitializeComponent();
            loadroomtype();
            loadcustomer("");
            loaditems();
            loadprice();

            textBox3.Enabled = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            lblcode.Text = loadcode();
            lblname.Text = Model.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        void loadroomtype()
        {
            string com = "select * from roomType";
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = Command.getdata(com);
        }

        void loadcustomer(string s)
        {
            string com = "select * from customer" + s;
            dataGridView1.DataSource = Command.getdata(com);
            dataGridView1.Columns[0].Visible = false;
        }

        void loaditems()
        {
            string com = "select * from item";
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";
            comboBox2.DataSource = Command.getdata(com);
        }

        string loadcode()
        {
            command = new SqlCommand("select top(1) id, bookingcode from reservation order by id desc", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (!reader.HasRows)
            {
                connection.Close();
                return "BK0001";
            }
            else
            {
                string read = reader.GetString(1);
                int i = reader.GetInt32(0) + 1;
                string s = read.Substring(0,5);
                return s + i.ToString() ;
            } 
        }

        int counttotal()
        {
            int room = 0, item = 0;
            for (int i = 0; i < dataGridView3.RowCount; i++)
            {
                room += Convert.ToInt32(dataGridView3.Rows[i].Cells[6].Value); 
            }

            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                item += Convert.ToInt32(dataGridView4.Rows[i].Cells[4].Value);
            }

            return room + item;
        }

        void loadprice()
        {
            command = new SqlCommand("select requestPrice from item where id=" + comboBox2.SelectedValue, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            textBox3.Text = reader.GetInt32(0).ToString();
            connection.Close();
        }

        bool valRoom()
        {
            if (textBox2.TextLength < 1)
            {
                MessageBox.Show("Fill the staying column", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (dateTimePicker1.Value.Date > dateTimePicker2.Value.Date)
            {
                MessageBox.Show("Check in must be less than check out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        void clear()
        {
            idCust = 0;
            idReser = 0;
            idRoom = 0;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            numericUpDown1.Value = 0;
            textBox2.Text = "";
            dataGridView2.DataSource = null;
            dataGridView3.DataSource = null;
            dataGridView4.DataSource = null;
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadcustomer(" where name like '%" + textBox1.Text + "%'");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            idCust = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string com = "select * from avail_view where roomTypeId = "+comboBox1.SelectedValue;
            dataGridView2.DataSource = Command.getdata(com);
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
            textBox4.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
            textBox5.Text = dataGridView2.SelectedRows[0].Cells[3].Value.ToString();
            textBox6.Text = dataGridView2.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow.Selected == true)
            {
                if (valRoom())
                {
                    int rows = dataGridView3.Rows.Add();
                    dataGridView3.Rows[rows].Cells[0].Value = textBox4.Text;
                    dataGridView3.Rows[rows].Cells[1].Value = textBox5.Text;
                    dataGridView3.Rows[rows].Cells[2].Value = textBox2.Text;
                    dataGridView3.Rows[rows].Cells[3].Value = textBox6.Text;
                    dataGridView3.Rows[rows].Cells[4].Value = dateTimePicker1.Value.Date;
                    dataGridView3.Rows[rows].Cells[5].Value = dateTimePicker2.Value.Date;
                    dataGridView3.Rows[rows].Cells[6].Value = Convert.ToInt32(textBox2.Text) * Convert.ToInt32(textBox6.Text);
                    lbltotal.Text = counttotal().ToString();
                }
            }
            else
            {
                MessageBox.Show("Please select a room!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView3.CurrentRow.Selected = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow.Selected)
            {
                dataGridView3.Rows.Remove(dataGridView3.SelectedRows[0]);
                lbltotal.Text = counttotal().ToString();
            }
            else
            {
                MessageBox.Show("Please select a room to delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(numericUpDown1.Value < 1)
            {
                MessageBox.Show("Insert quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int row = dataGridView4.Rows.Add();
                dataGridView4.Rows[row].Cells[0].Value = comboBox2.SelectedValue;
                dataGridView4.Rows[row].Cells[1].Value = comboBox2.Text;
                dataGridView4.Rows[row].Cells[2].Value = textBox3.Text;
                dataGridView4.Rows[row].Cells[3].Value = numericUpDown1.Value;
                dataGridView4.Rows[row].Cells[4].Value = numericUpDown1.Value * Convert.ToInt32(textBox3.Text);
                lbltotal.Text = counttotal().ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddCustomer add = new AddCustomer();
            add.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if(idCust == 0)
            {
                MessageBox.Show("Please select a customer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (dataGridView3.RowCount < 1)
            {
                MessageBox.Show("Please select a room", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string com = "insert into reservation values (getdate(), " + Model.id + ", " + idCust + ", '" + lblcode.Text + "', '" + DateTime.Now.ToString("MMMM") + "', '" + DateTime.Now.ToString("yyyy") + "')";
                try
                {
                    Command.exec(com);
                    connection.Close();

                    command = new SqlCommand("select top(1) id from reservation order by id desc", connection);
                    connection.Open();
                    reader = command.ExecuteReader();
                    reader.Read();
                    idReser = reader.GetInt32(0);
                    connection.Close();

                    for(int i = 0; i < dataGridView3.RowCount; i++)
                    {
                        command = new SqlCommand("insert into reservationroom values(" + idReser + ", " + dataGridView3.Rows[i].Cells[0].Value + ", getdate(), " + dataGridView3.Rows[i].Cells[2].Value + ", '" + dataGridView3.Rows[i].Cells[3].Value + "', '" + dataGridView3.Rows[i].Cells[4].Value + "', '" + dataGridView3.Rows[i].Cells[4].Value + "')", connection);
                        connection.Open();
                        try
                        {
                            command.ExecuteNonQuery();
                            connection.Close();

                            string comm = "update room set status = 'unavail' where id = " + dataGridView3.Rows[i].Cells[0].Value;
                            try
                            {
                                Command.exec(comm);
                                connection.Close();
                            }
                            catch (Exception exx)
                            {
                                MessageBox.Show(exx.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show("room" + exc);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }

                    if(dataGridView4.RowCount > 0)
                    {
                        command = new SqlCommand("select top(1) id from reservationRoom order by id desc", connection);
                        connection.Open();
                        reader = command.ExecuteReader();
                        reader.Read();
                        idRoom = reader.GetInt32(0);
                        connection.Close();

                        for (int i = 0; i < dataGridView4.RowCount; i++)
                        {
                            command = new SqlCommand("insert into reservationRequestItem values(" + idRoom + ", " + dataGridView4.Rows[i].Cells[0].Value + ", " + dataGridView4.Rows[i].Cells[3].Value + ", " + dataGridView4.Rows[i].Cells[4].Value + ")", connection);
                            connection.Open();
                            try
                            {
                                command.ExecuteNonQuery();
                                MessageBox.Show("Successfully add new reservation", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                clear();
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show("req" + exc);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Successfully add new reservation", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("res" + ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
