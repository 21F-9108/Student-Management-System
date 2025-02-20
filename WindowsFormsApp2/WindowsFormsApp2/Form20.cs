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
using System.Globalization;
namespace WindowsFormsApp2
{


    
    public partial class Form20 : Form
    {
        OracleConnection con;

        public Form20()
        {

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selected = dataGridView1.SelectedRows[0];

                //check if cid left null or not
                DataGridViewCell cell1 = selected.Cells["c_id"];
                if (cell1.Value == null || string.IsNullOrEmpty(cell1.Value.ToString()))
                {
                    MessageBox.Show("Class ID cannot be null.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (cell1.Value.ToString().Length > 5)
                {
                    MessageBox.Show("Class ID length exceeded.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //check if entered class exists or not
                string c_id = selected.Cells["c_id"].Value.ToString();
                con.Open();
                string query1 = "SELECT count(*) FROM class WHERE c_id = :c_id";
                OracleCommand command1 = new OracleCommand(query1, con);
                command1.Parameters.Add(new OracleParameter("c_id", c_id));
                string count1 = command1.ExecuteScalar().ToString();
                int c_count = Convert.ToInt32(count1);
                con.Close();
                if (c_count <= 0)
                {
                    MessageBox.Show("Class does not exist.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                //check if entered teacher exists or not
                //DataGridViewCell cell2 = selected.Cells["t_id"];
                //if (cell2.Value != null || !string.IsNullOrEmpty(cell2.Value.ToString()))
                //{

                //    string t_id = selected.Cells["t_id"].Value.ToString();
                //    con.Open();
                //    string query2 = "SELECT count(*) FROM class WHERE t_id = :t_id";
                //    OracleCommand command2 = new OracleCommand(query2, con);
                //    command1.Parameters.Add(new OracleParameter("c_id", c_id));
                //    string count2 = command1.ExecuteScalar().ToString();
                //    int t_count = Convert.ToInt32(count2);
                //    con.Close();
                //    if (c_count <= 0)
                //    {
                //        MessageBox.Show("Teacher does not exist");
                //        return;
                //    }
                //}

                DataGridViewCell cell2 = selected.Cells["day"];
                if (cell2.Value == null || string.IsNullOrEmpty(cell2.Value.ToString()))
                {
                    MessageBox.Show("Day cannot be null.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (cell2.Value.ToString().Length > 10)
                {
                    MessageBox.Show("Day length exceeded");
                    return;
                }
                if (cell2.Value.ToString().ToUpper() != "MONDAY" && cell2.Value.ToString().ToUpper() != "TUESDAY" && cell2.Value.ToString().ToUpper() != "WEDNESDAY"
                    && cell2.Value.ToString().ToUpper() != "THURSDAY" && cell2.Value.ToString().ToUpper() != "FRIDAY" && cell2.Value.ToString().ToUpper() != "SATURDAY" && cell2.Value.ToString().ToUpper() != "SUNDAY")
                {
                    MessageBox.Show("Invalid day entered", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                con.Open();
                string query2 = "SELECT count(*) FROM class_schedule WHERE c_id = :c_id and day= :day";
                OracleCommand command2 = new OracleCommand(query2, con);
                command2.Parameters.Add(new OracleParameter("c_id", c_id));
                command2.Parameters.Add(new OracleParameter("day", cell2.Value.ToString()));
                string count2 = command2.ExecuteScalar().ToString();
                int cs_count = Convert.ToInt32(count2);
                con.Close();
                if (cs_count > 0)
                {
                    MessageBox.Show("Class Schedule already exists.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                DataGridViewCell cell3 = selected.Cells["start_time"];
                if (cell3.Value == null || string.IsNullOrEmpty(cell3.Value.ToString()))
                {
                    MessageBox.Show("Start time cannot be null.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }

                bool isValidTime = DateTime.TryParse(cell3.FormattedValue.ToString(), out DateTime timeValue);
                if (isValidTime==false)
                {
                    MessageBox.Show("Invalid time format. Please enter a valid time.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

               

                DataGridViewCell cell4 = selected.Cells["end_time"];
                if (cell4.Value == null || string.IsNullOrEmpty(cell4.Value.ToString()))
                {
                    MessageBox.Show("Start time cant be null");
                    return;
                }
                bool isValidTime1 = DateTime.TryParse(cell4.FormattedValue.ToString(), out DateTime timeValue1);
                if (isValidTime==false)
                {
                    MessageBox.Show("Invalid time format. Please enter a valid time.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return;
                }


                DataGridViewCell cell5 = selected.Cells["location"];
                if (cell5.Value == null || string.IsNullOrEmpty(cell5.Value.ToString()))
                {
                    MessageBox.Show("Location cannot be null.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (cell5.Value.ToString().Length > 20)
                {
                    MessageBox.Show("Location length.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                con.Open();
                OracleCommand add = con.CreateCommand();
                add.CommandText = "INSERT INTO class_schedule(c_id, day, start_time, end_time,location) " 
                    + "VALUES (:c_id, :day, :start_time, :end_time, :location)";
                add.Parameters.Add(":c_id", OracleDbType.Varchar2).Value = cell1.Value.ToString();
                add.Parameters.Add(":day", OracleDbType.Varchar2).Value = cell2.Value.ToString();


                DateTime start_time,end_time;
                if (DateTime.TryParse(cell3.FormattedValue.ToString(), out start_time))
                {
                    add.Parameters.Add(":start_time", OracleDbType.TimeStamp).Value = start_time;
                }

                if (DateTime.TryParse(cell4.FormattedValue.ToString(), out end_time))
                {
                    add.Parameters.Add(":end_time", OracleDbType.TimeStamp).Value = end_time;
                }


                add.Parameters.Add(":location", OracleDbType.Varchar2).Value = cell5.Value.ToString();

                add.CommandType = CommandType.Text;
                add.ExecuteNonQuery();
                con.Close();





            }
            else
            {
                MessageBox.Show("Please select a record");
            }
        }

        private void Form20_Load(object sender, EventArgs e)
        {

            string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
            con = new OracleConnection(conStr);
            updateGrid();
        }

        private void updateGrid()
        {
            con.Open();
            OracleCommand getEmps = con.CreateCommand();
            getEmps.CommandText = "select c_id, day, start_time, end_time,location from class_schedule";

            getEmps.CommandType = CommandType.Text;
            OracleDataReader empDR = getEmps.ExecuteReader();
            DataTable empDT = new DataTable();
            empDT.Load(empDR);

            dataGridView1.DataSource = empDT;
            dataGridView1.Columns["start_time"].DefaultCellStyle.Format = " HH:mm:ss";
            dataGridView1.Columns["end_time"].DefaultCellStyle.Format = " HH:mm:ss";


            con.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form21 f21=new Form21();
            this.Hide();
            f21.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
