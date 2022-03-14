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
    public partial class CheckOut : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int idFd, reserId;
        
        public CheckOut()
        {
            InitializeComponent();
            loadroom();
            loadstatus();

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            lblname.Text = Model.name;
            lbltime.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        void loadroom()
        {
            string com = "select * from room where status = 'unavail'";
            comboBox1.DisplayMember = "roomNumber";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = Command.getdata(com);

            command = new SqlCommand("select top(1) id from reservationRoom where roomId = " + comboBox1.SelectedValue + " order by id desc", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            reserId = reader.GetInt32(0);
            connection.Close();
            
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            if (comboBox1.Text.Length > 0 || comboBox1.SelectedValue != null)
            {
                loaditem();
                loadfd();

                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
            }
        }

        void loaditem()
        {
            string com = "select item.id, item.name from reservationRequestItem join item on reservationRequestItem.itemid = item.id where reservationRoomId = " + reserId;
            comboBox2.DisplayMember = "name";
            comboBox2.ValueMember = "id";
            comboBox2.DataSource = Command.getdata(com);

            if (comboBox2.Text.Length > 0 || comboBox2.SelectedValue != null)
            {
                getPrice();
            }
        }

        void loadfd()
        {
            string com = "select foodsAndDrinks.name, FDCheckOut.* from FDCheckout join foodsandDrinks on FDCheckout.fdid = foodsanddrinks.id where reservationRoomId = " + reserId;
            dataGridView2.DataSource = Command.getdata(com);

            lbltotalfd.Text = countsubFd().ToString();
            lbltotal.Text = (countSubItem() + countsubFd()).ToString();
        }

        void getPrice()
        {
            command = new SqlCommand("select requestPrice, compensationFee from item where id = " + comboBox2.SelectedValue, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                textBox1.Text = reader.GetInt32(0).ToString();
                textBox2.Text = reader.GetInt32(1).ToString();
                textBox3.Text = (reader.GetInt32(0) * numericUpDown1.Value).ToString();
                connection.Close();
            }
            connection.Close();
        }

        void loadstatus()
        {
            string com = "select * from itemStatus";
            comboBox3.DisplayMember = "name";
            comboBox3.ValueMember = "id";
            comboBox3.DataSource = Command.getdata(com);
        }

        int countSubItem()
        {
            int t = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                t += Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);
            }

            return t;
        }

        int countsubFd()
        {
            int t = 0;
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                t += Convert.ToInt32(dataGridView2.Rows[i].Cells[5].Value);
            }

            return t;
        }

        void clear()
        {
            dataGridView1.Rows.Clear();
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            loadroom();
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
            if(result == DialogResult.Yes)
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
            if(comboBox2.Text.Length > 0 && numericUpDown1.Value > 0 && comboBox3.Text.Length > 0)
            {
                int rows = dataGridView1.Rows.Add();
                dataGridView1.Rows[rows].Cells[0].Value = comboBox2.SelectedValue;
                dataGridView1.Rows[rows].Cells[1].Value = comboBox2.Text;
                dataGridView1.Rows[rows].Cells[2].Value = comboBox3.Text;
                dataGridView1.Rows[rows].Cells[3].Value = textBox1.Text;
                dataGridView1.Rows[rows].Cells[4].Value = numericUpDown1.Value;
                if(comboBox3.Text != "Good")
                {
                    dataGridView1.Rows[rows].Cells[5].Value = Convert.ToInt32(textBox2.Text) * numericUpDown1.Value;
                    dataGridView1.Rows[rows].Cells[6].Value = (Convert.ToInt32(textBox2.Text) * numericUpDown1.Value) + Convert.ToInt32(textBox3.Text);
                }
                else
                {
                    dataGridView1.Rows[rows].Cells[5].Value = 0;
                    dataGridView1.Rows[rows].Cells[6].Value = (Convert.ToInt32(textBox3.Text));

                }

                lbltotalitem.Text = countSubItem().ToString();
                lbltotal.Text = (countSubItem() + countsubFd()).ToString();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);

                lbltotalfd.Text = countSubItem().ToString();
                lbltotal.Text = (countSubItem() + countsubFd()).ToString();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loaditem();
            loadfd();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
            idFd = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[1].Value);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow.Selected)
            {
                string com = "delete from fdCheckOut where id = " + idFd;
                try
                {
                    Command.exec(com);
                    MessageBox.Show("Successfully Removed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadfd();

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

        private void button6_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt32(comboBox1.SelectedValue) != 0)
            {
                int s = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    try
                    {

                        if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Good")
                        {
                            s = 1;
                        }
                        else
                        {
                            s = 2;
                        }

                        command = new SqlCommand("insert into reservationCheckOut values(" + comboBox1.SelectedValue + ", " + dataGridView1.Rows[i].Cells[0].Value + ", " + s + ", " + dataGridView1.Rows[i].Cells[4].Value + ", " + dataGridView1.Rows[i].Cells[5].Value + ")", connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                command = new SqlCommand("update room set status = 'avail' where roomNumber = " + comboBox1.Text, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                command = new SqlCommand("update reservationRoom set checkOutDatetime = getdate() where id = " + comboBox1.SelectedValue, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Room Successfully check outed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
                
            }
        }
    }
}
