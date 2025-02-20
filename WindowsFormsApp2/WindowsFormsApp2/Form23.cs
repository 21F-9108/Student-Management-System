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
    public partial class Form23 : Form
    {
        OracleConnection con;

        int s_id;
        public Form23()
        {
            InitializeComponent();
        }
        public Form23(int sid)
        {
            InitializeComponent();
            s_id = sid;
        }

        private void Form23_Load(object sender, EventArgs e)
        {
            string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
            con = new OracleConnection(conStr);
            updateGrid();
        }
        private void updateGrid()
        {
            con.Open();
            OracleCommand getEmps = con.CreateCommand();
            getEmps.CommandText = "select * from class_schedule where c_id=ANY(select c_id from enrollment where s_id= :s_id )";
            getEmps.Parameters.Add(":s_id", OracleDbType.Int32).Value = s_id;

            getEmps.CommandType = CommandType.Text;
            OracleDataReader empDR = getEmps.ExecuteReader();
            DataTable empDT = new DataTable();
            empDT.Load(empDR);
            dataGridView1.DataSource = empDT;
            con.Close();

        }
    }
}
