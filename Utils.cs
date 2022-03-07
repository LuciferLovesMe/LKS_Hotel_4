using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LKS_Hotel_4
{
    class Utils
    {
        public static string conn = @"Data Source=desktop-00eposj;Initial Catalog=LKS_Hotel_4;Integrated Security=True";
    }

    class Model
    {
        public static int id { set; get; }
        public static string name { set; get; }
        public static int jobId { set; get; }
    }

    class Encrypt
    {
        public static string enc(string data)
        {
            using (SHA256Managed managed = new SHA256Managed())
            {
                byte[] b = managed.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(b);
            }
        }

        public static byte[] encode(Image img)
        {
            ImageConverter converter = new ImageConverter();
            byte[] image = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return image;
        }
    }

    class Command
    {
        public static DataTable getdata(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlDataAdapter adapter = new SqlDataAdapter(com, connection);
            DataTable data = new DataTable();
            adapter.Fill(data);
            return data;
        }

        public static void exec(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlCommand command = new SqlCommand(com, connection);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
