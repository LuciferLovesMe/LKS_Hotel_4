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
    public partial class MainLogin : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public MainLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = '\0';
            else
                textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.TextLength < 1 || textBox2.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                command = new SqlCommand("select * from employee where username = '" + textBox1.Text.Replace("'", "`") + "' and password = @pass", connection);
                command.Parameters.AddWithValue("@pass", Encrypt.enc(textBox2.Text));
                connection.Open();
                reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    Model.id = reader.GetInt32(0);
                    Model.name = reader.GetString(3);
                    Model.jobId = reader.GetInt32(6);
                    connection.Close();
                    if(Model.jobId == 1)
                    {
                        MainFrontOffice main = new MainFrontOffice();
                        this.Hide();
                        main.ShowDialog();
                    }
                    else if (Model.jobId == 2)
                    {
                        MainAdmin main = new MainAdmin();
                        this.Hide();
                        main.ShowDialog();
                    }
                }
                else
                {
                    connection.Close();
                    MessageBox.Show("Can't find user", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
