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
    public partial class Form14 : Form
    {
        OracleConnection con;

        public Form14()
        {
            InitializeComponent();
        }

        private void Form14_Load(object sender, EventArgs e)
        {
            string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
            con = new OracleConnection(conStr);
            updateGrid();
        }
        private void updateGrid()
        {

            con.Open();
            OracleCommand display = new OracleCommand("display_classes", con);
            display.CommandType = CommandType.StoredProcedure;

            OracleParameter outParameter = new OracleParameter("output_cursor", OracleDbType.RefCursor);
            outParameter.Direction = ParameterDirection.Output;
            display.Parameters.Add(outParameter);

            OracleDataAdapter adapter = new OracleDataAdapter(display);

            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;


            con.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form15 f15=new Form15();
            this.Hide();
            f15.Show();
         
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
