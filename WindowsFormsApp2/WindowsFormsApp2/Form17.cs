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
    public partial class Form17 : Form
    {
        OracleConnection con;
        public Form17()
        {
            InitializeComponent();
        }
        string selected;
        public Form17(string var) //selected class id is being passed from form16 to this form
        {
            InitializeComponent();
            selected = var;

        }

        private void Form17_Load(object sender, EventArgs e)
        {
            string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
            con = new OracleConnection(conStr);
            updateGrid();
        }
        private void updateGrid()
        {
            con.Open();
            OracleCommand getEmps = con.CreateCommand();
            getEmps.CommandText = "select s_id from enrollment where c_id= :selected ";
            getEmps.Parameters.Add(":selected", OracleDbType.Varchar2).Value = selected;

            getEmps.CommandType = CommandType.Text;
            OracleDataReader empDR = getEmps.ExecuteReader();
            DataTable empDT = new DataTable();
            empDT.Load(empDR);
            dataGridView1.DataSource = empDT;
            con.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow new_rec = dataGridView1.SelectedRows[0];
                DataGridViewCell cell = new_rec.Cells["s_id"];
                if (cell.Value==null || string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    MessageBox.Show("Student id cant be null");
                    return;
                }
                else
                {
                    //check if student enters exist in student table or not
                    con.Open();
                    OracleCommand getSTD = con.CreateCommand();
                    getSTD.CommandText = "select count(*) from student where s_id= :s_id ";
                    getSTD.Parameters.Add(":s_id", OracleDbType.Int32).Value = cell.Value;
                    string count = getSTD.ExecuteScalar().ToString();
                    int s_count = Convert.ToInt32(count);
                    con.Close();
                    if (s_count > 0)
                    {
                        //check if student is already enrolled in the same class
                        con.Open();
                        getSTD=con.CreateCommand();
                        getSTD.CommandText = " select count(*) from enrollment where s_id= :s_id AND c_id=:c_id";
                        getSTD.Parameters.Add(":s_id", OracleDbType.Int32).Value = cell.Value;
                        getSTD.Parameters.Add(":c_id", OracleDbType.Varchar2).Value = selected;
                        string count1 = getSTD.ExecuteScalar().ToString();
                        int s_count1 = Convert.ToInt32(count1);
                        con.Close();
                        
                        if (s_count1 < 0)
                        {
                            MessageBox.Show(count1);
                            MessageBox.Show("Student already enrolled in this class");
                            return;
                        }
                        else
                        {
                            con.Open();
                            getSTD =con.CreateCommand();
                            int s_id;
                            if (int.TryParse(cell.Value.ToString(), out s_id)) //just to convert cell value to integer
                            {
                                getSTD.CommandText = "insert into enrollment (c_id,s_id) values (:c_id,:s_id)";
                                getSTD.Parameters.Add(":c_id", OracleDbType.Varchar2).Value = selected;
                                getSTD.Parameters.Add(":s_id", OracleDbType.Int32).Value = int.Parse(cell.Value.ToString());
                                getSTD.CommandType = CommandType.Text;

                                getSTD.ExecuteNonQuery();

                                con.Close();
                            }
                            else
                            {
                                MessageBox.Show("error");
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Student doesnt exist");
                        return;
                    }
                }

            }
            else
            {
                MessageBox.Show("Please select a record");
            }
        }
    }

}
