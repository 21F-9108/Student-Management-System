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
    public partial class Form19 : Form
    {
        OracleConnection con;

        public Form19()
        {
            InitializeComponent();
        }
        string selected_cid;
        public Form19(string c_id)
        {
            InitializeComponent();
             selected_cid = c_id;
        }

        private void Form19_Load(object sender, EventArgs e)
        {
            string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
            con = new OracleConnection(conStr);
            updateGrid();
        }
        private void updateGrid()
        {
            con.Open();
            OracleCommand display = con.CreateCommand();
            display.CommandText = "select s_id,fname,lname from student where s_id= ANY(select s_id from enrollment where c_id= :c_id)";
            display.Parameters.Add(":c_id", OracleDbType.Varchar2).Value = selected_cid;
            display.CommandType = CommandType.Text;
            OracleDataReader dr = display.ExecuteReader();
            DataTable table= new DataTable();
            table.Load(dr);
            dataGridView1.DataSource = table;
            con.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
