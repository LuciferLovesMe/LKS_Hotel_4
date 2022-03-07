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
    public partial class CheckIn : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int idCust, cond;

        public CheckIn()
        {
            InitializeComponent();
            lblname.Text = Model.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        bool val()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || dateTimePicker1.Value == null || !radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (radioButton1.Checked && radioButton2.Checked)
            {
                MessageBox.Show("Please select a gender!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Please isert a correct date of birth!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox5.TextLength != 16)
            {
                MessageBox.Show("NIK must be 16 characters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            command = new SqlCommand("select * from customer where nik = '" + textBox5.Text + "'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("NIK was already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();

            command = new SqlCommand("select * from customer where phoneNumber = '" + textBox2.Text + "'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Phone number was already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();
            return true;
        }

        bool val_up()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || dateTimePicker1.Value == null || !radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (radioButton1.Checked && radioButton2.Checked)
            {
                MessageBox.Show("Please select a gender!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Please isert a correct date of birth!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox5.TextLength != 16)
            {
                MessageBox.Show("NIK must be 16 characters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            command = new SqlCommand("select * from customer where nik = '" + textBox5.Text + "'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows && idCust != reader.GetInt32(0))
            {
                connection.Close();
                MessageBox.Show("NIK was already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();

            command = new SqlCommand("select * from customer where phoneNumber = '" + textBox2.Text + "'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows && idCust != reader.GetInt32(0))
            {
                connection.Close();
                MessageBox.Show("Phone number was already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            connection.Close();
            return true;
        }

        void cust()
        {
            if(cond == 1)
            {
                if (val())
                {
                    string g = "";
                    if (radioButton1.Checked)
                    {
                        g = "Male";
                    }
                    else if (radioButton2.Checked)
                    {
                        g = "Female";
                    }
                    int i = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                    command = new SqlCommand("insert into customer values('" + textBox3.Text.Replace("'", "`") + "', '" + textBox5.Text + "', '" + textBox4.Text.Replace("'", "`") + "', '" + g + "', '" + textBox2.Text + "', '" + dateTimePicker1.Value.Date + "', " + i + ")", connection);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Reservation reservation = new Reservation();
                        this.Hide();
                        reservation.ShowDialog();
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
            else if (val_up() && cond == 2)
            {
                string g = "";
                if (radioButton1.Checked)
                {
                    g = "Male";
                }
                else if (radioButton2.Checked)
                {
                    g = "Female";
                }
                int i = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                command = new SqlCommand("update customer set name = '" + textBox3.Text.Replace("'", "`") + "', nik = '" + textBox5.Text + "', email = '" + textBox4.Text.Replace("'", "`") + "', gender = '" + g + "', dateOfBirth = '" + dateTimePicker1.Value.Date + "', age = " + i + ", phoneNumber = '" + textBox2.Text + "' where id = " + idCust, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Reservation reservation = new Reservation();
                    this.Hide();
                    reservation.ShowDialog();
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == 8);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string com = "select * from reservation join reservationRoom on reservation.id = reservationRoom.reservationId where checkInDateTime is null and bookingCode = '"+textBox1.Text+"'";

            command = new SqlCommand("select * from reservation where bookingCode = '" + textBox1.Text + "'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (!reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Wrong booking code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                connection.Close();
                dataGridView1.DataSource = Command.getdata(com);
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount > 0)
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    string com = "update reservationRoom set checkInDateTime = getDate() where id = " + dataGridView1.Rows[i].Cells[7].Value;
                    try
                    {
                        Command.exec(com);
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
                if (textBox2.TextLength > 0)
                {
                    cust();
                }
            }
            else
            {
                if(textBox2.TextLength > 0)
                {
                    cust();
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            command = new SqlCommand("select top(1) * from customer where phoneNumber like '%" + textBox2.Text + "%'", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                cond = 1;
                idCust = reader.GetInt32(0);
                textBox3.Text = reader.GetString(1);
                textBox4.Text = reader.GetString(3);
                if (reader.GetString(4) == "Male")
                    radioButton1.Checked = true;
                else
                    radioButton2.Checked = true;
                textBox5.Text = reader.GetString(2);
                dateTimePicker1.Value = reader.GetDateTime(6);
                connection.Close();
            }
            else
            {
                connection.Close();
                cond = 2;
            }
        }
    }
}
