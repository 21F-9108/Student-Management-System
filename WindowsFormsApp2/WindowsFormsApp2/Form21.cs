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
    public partial class Form21 : Form
    {
        OracleConnection con;

        public Form21()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void updateGrid()
        {
            con.Open();
            OracleCommand getEmps = con.CreateCommand();
            getEmps.CommandText = "select  * from class";
            getEmps.CommandType = CommandType.Text;
            OracleDataReader empDR = getEmps.ExecuteReader();
            DataTable empDT = new DataTable();
            empDT.Load(empDR);
            dataGridView1.DataSource = empDT;
            con.Close();

        }

        private void Form21_Load(object sender, EventArgs e)
        {
            string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
            con = new OracleConnection(conStr);
            updateGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selected = dataGridView1.SelectedRows[0];

                DataGridViewCell cell1= selected.Cells["c_id"];
                DataGridViewCell cell2 = selected.Cells["t_id"];
                if (cell2.Value == null || string.IsNullOrEmpty(cell2.Value.ToString()))
                {
                    MessageBox.Show("Enter teacher you want to assign", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                string t_id = selected.Cells["t_id"].Value.ToString();
                con.Open();
                string query2 = "SELECT count(*) FROM teacher  WHERE t_id = :t_id";
                OracleCommand command2 = new OracleCommand(query2, con);
                command2.Parameters.Add(new OracleParameter("t_id", t_id));
                string count2 = command2.ExecuteScalar().ToString();
                int t_count = Convert.ToInt32(count2);
                con.Close();
                if (t_count > 0)
                {
                    con.Open();
                    OracleCommand command = new OracleCommand("assign_teacher", con);
                    command.CommandType = CommandType.StoredProcedure;


                    command.Parameters.Add("t_id", OracleDbType.Int32).Value = cell2.Value.ToString();
                    command.Parameters.Add("c_id", OracleDbType.Varchar2).Value = cell1.Value.ToString();

                    // Execute the procedure
                    command.ExecuteNonQuery();
                    con.Close();
                }


                if (t_count <= 0)
                {
                    MessageBox.Show("Teacher doesn't exist", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
            }
            else
            {
                MessageBox.Show("Select a record.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
