using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace WindowsFormsApp2
{
    public partial class Form15 : Form
    {
        OracleConnection con;

        public Form15()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("C_id cant be null");

            }
            else if (textBox1.Text.Length > 5)
            {
                MessageBox.Show("C_id length exceeded");

            }

            else
            {
                //check if class already exists
                con.Open();
                string query1 = "SELECT count(*) FROM class WHERE c_id = :c_id";
                OracleCommand command1 = new OracleCommand(query1, con);
                command1.Parameters.Add(new OracleParameter("t_id", textBox1.Text));
                string count1 = command1.ExecuteScalar().ToString();
                int c_count = Convert.ToInt32(count1);
                con.Close();
                if (c_count > 0)
                {
                    MessageBox.Show("Class already exists");
                    return;
                }
           

                //check if teacher exists or not
                if (!string.IsNullOrEmpty(textBox2.Text))
                {
                    con.Open();
                    string query = "SELECT count(*) FROM teacher WHERE t_id = :t_id";
                    OracleCommand command = new OracleCommand(query, con);
                    command.Parameters.Add(new OracleParameter("t_id", textBox2.Text));
                    string count = command.ExecuteScalar().ToString();
                    int t_count = Convert.ToInt32(count);
                    con.Close();
                    if (t_count <= 0)
                    {
                        MessageBox.Show("Teacher doesnt exist");
                        return;
                    }
                }

                //insery
                con.Open();
                OracleCommand add = con.CreateCommand();
                add.CommandText = "INSERT INTO class (c_id, t_id) " + "VALUES (:c_id, :t_id)";
                add.Parameters.Add(":c_id", OracleDbType.Varchar2).Value = textBox1.Text;
                if (!string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    add.Parameters.Add(":t_id", OracleDbType.Int32).Value = textBox2.Text;
                }
                else
                {
                    add.Parameters.Add(":t_id", DBNull.Value).Value=null;
                }

                add.CommandType = CommandType.Text;
                add.ExecuteNonQuery();
                con.Close();
            }
            
        }

        private void Form15_Load(object sender, EventArgs e)
        {
            string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
            con = new OracleConnection(conStr);
        }
    }
}
