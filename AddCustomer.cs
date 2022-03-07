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
    public partial class AddCustomer : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public AddCustomer()
        {
            InitializeComponent();
        }

        bool val()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || dateTimePicker1.Value == null || !radioButton1.Checked && !radioButton2.Checked)
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
            else if (textBox2.TextLength != 16)
            {
                MessageBox.Show("NIK must be 16 characters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            command = new SqlCommand("select * from customer where nik = '" + textBox2.Text + "'", connection);
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

            command = new SqlCommand("select * from customer where phoneNumber = '" + textBox3.Text + "'", connection);
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reservation reservation = new Reservation();
            this.Hide();
            reservation.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
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
                command = new SqlCommand("insert into customer values('" + textBox2.Text.Replace("'", "`") + "', '" + textBox1.Text + "', '" + textBox3.Text.Replace("'", "`") + "', '" + g + "', '" + textBox4.Text + "', '" + dateTimePicker1.Value.Date + "', " + i + ")", connection);
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsSymbol(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}
