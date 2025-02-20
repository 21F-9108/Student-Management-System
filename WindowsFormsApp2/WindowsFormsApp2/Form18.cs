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
    public partial class Form18 : Form
    {
        OracleConnection con;
        public Form18()
        {
            InitializeComponent();
        }

        private void Form18_Load(object sender, EventArgs e)
        {
            string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
            con = new OracleConnection(conStr);
            updateGrid();

        }
        private void updateGrid()
        {
            con.Open();
            OracleCommand display = new OracleCommand("display_classes", con);
            display.CommandType=CommandType.StoredProcedure;

            OracleParameter outParameter = new OracleParameter("output_cursor", OracleDbType.RefCursor);
            outParameter.Direction = ParameterDirection.Output;
            display.Parameters.Add(outParameter);

            OracleDataAdapter adapter = new OracleDataAdapter(display);

            DataTable table=new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;

            
            con.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selected = dataGridView1.SelectedRows[0];
                string c_id = selected.Cells["c_id"].Value.ToString();
                Form19 f19 = new Form19(c_id);
                this.Hide();
                f19.Show();
            }
            else
            {
                MessageBox.Show("Please select a record");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
