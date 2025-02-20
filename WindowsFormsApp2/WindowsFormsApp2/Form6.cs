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
    public partial class Form6 : Form
    {
        OracleConnection con;

        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
            con = new OracleConnection(conStr);
            updateGrid();

        }

        private void updateGrid()
        {
            con.Open();
            OracleCommand getEmps = con.CreateCommand();
            getEmps.CommandText = "select s_id, fname,lname,email, contact_no,address from student order by s_id asc";
            getEmps.CommandType = CommandType.Text;
            OracleDataReader empDR = getEmps.ExecuteReader();
            DataTable empDT = new DataTable();
            empDT.Load(empDR);
            dataGridView1.DataSource = empDT;

            // listBox1.Items.Add(empDT);
            con.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                con.Open();
                DataGridViewRow selected = dataGridView1.SelectedRows[0];
                //MessageBox.Show(selected.Cells["fname"].Value.ToS());
                string student_id = selected.Cells["s_id"].Value.ToString();
                OracleCommand update = con.CreateCommand();
                string fname = selected.Cells["fname"].Value.ToString();
                string lname = selected.Cells["lname"].Value.ToString();
                string address = selected.Cells["address"].Value.ToString();
                string contact_no = selected.Cells["contact_no"].Value.ToString();
                string email= selected.Cells["email"].Value.ToString();

                update.CommandText = "update student set fname= :fname, lname=:lname , address=:address, contact_no=:contact_no, email=:email where s_id=:student_id";
                update.Parameters.Add(":fname", OracleDbType.Varchar2).Value = fname;
                update.Parameters.Add(":lname", OracleDbType.Varchar2).Value = lname;
                update.Parameters.Add(":address", OracleDbType.Varchar2).Value = address;
                update.Parameters.Add(":contact_no", OracleDbType.Varchar2).Value = contact_no;
                update.Parameters.Add(":email", OracleDbType.Varchar2).Value = email;
                update.Parameters.Add(":student_id", OracleDbType.Int32).Value = Convert.ToInt32(student_id);

                update.CommandType = CommandType.Text;

                update.ExecuteNonQuery();

                con.Close();
            }
            else
            {
                MessageBox.Show("Select the record you have edited");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
