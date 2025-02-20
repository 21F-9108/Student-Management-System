using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
/////add attendence form 
namespace WindowsFormsApp2
{

	public partial class Form26 : Form
	{
		OracleConnection con;
		public string ReceivedText2 { get; set; }
		public Form26()
		{
			InitializeComponent();
			dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);

		}
		string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";

		public void AddAttendance(string c_id, int s_id, DateTime class_date, string status)
		{
			using (OracleConnection conn = new OracleConnection("DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;"))
			{
				conn.Open();

				using (OracleCommand command = new OracleCommand("ADD_ATTENDANCE", conn))
				{
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.Add("p_c_id", OracleDbType.Varchar2, ParameterDirection.Input).Value = c_id;
					command.Parameters.Add("p_s_id", OracleDbType.Int32, ParameterDirection.Input).Value = s_id;
					command.Parameters.Add("p_class_date", OracleDbType.Date, ParameterDirection.Input).Value = class_date;
					command.Parameters.Add("p_status", OracleDbType.Varchar2, ParameterDirection.Input).Value = status;
					command.ExecuteNonQuery();
				}
			}
		}
		public DataTable GetStudents()
		{
			string teacher_email_to_find_teacher_id;
			////oracleconnection represents a connection to an Oracle Database. 
			////you initialize it with a connection string, which contains information about how to connect to the database.
			using (OracleConnection conn = new OracleConnection("DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;"))
			{
				// Open the connection to the database.
				conn.Open();
				teacher_email_to_find_teacher_id = ReceivedText2;


				//// OracleCommand represents a SQL statement or a stored procedure to execute against a database.
				//// In this case we are going to call a stored procedure.
				using (OracleCommand cmd = new OracleCommand("GET_STUDENTS", conn))
				{

					string query = "SELECT t_id FROM teacher WHERE Email = :Email"; //sqlquery to check if the email exists in the Students table
					OracleCommand command = new OracleCommand(query, conn);//// Create a OracleCommand object
					command.Parameters.Add(new OracleParameter("Email", teacher_email_to_find_teacher_id));//// add a parameter to the query to avoid SQL injection attacks
					object result = command.ExecuteScalar();
					int t_id = result != null ? Convert.ToInt32(result) : 0; // We're calling a stored procedure, not executing a plain text SQL query.
					cmd.CommandType = CommandType.StoredProcedure;


					///// OracleDbType.Int32 specifies the data type of the parameter, 
					////ParameterDirection.Input means it's an input parameter.
					cmd.Parameters.Add("p_t_id", OracleDbType.Int32, ParameterDirection.Input).Value = t_id;

					// Add a parameter to hold the cursor that will be returned by the stored procedure. 
					// ParameterDirection.Output means it's a parameter that will hold data coming out of the stored procedure.
					OracleParameter outParam = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
					cmd.Parameters.Add(outParam);
					cmd.ExecuteNonQuery();
					// Get the returned cursor and cast it to an OracleDataReader.
					OracleDataReader dr = ((OracleRefCursor)outParam.Value).GetDataReader();
					// Create a DataTable to hold the returned rows.
					DataTable dt = new DataTable();
					// Load the DataReaders data into the DataTable.
					dt.Load(dr);
					////returning DataTable.
					return dt;
				}
			}
		}

		public DataTable GetStudents(string c_id)
		{
			using (OracleConnection conn = new OracleConnection("DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;"))
			{
				conn.Open();

				using (OracleCommand cmd = new OracleCommand("GET_STUDENTS_FROM_CLASS", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("p_c_id", OracleDbType.Varchar2).Value = c_id; // Ensure the datatype matches your database
					OracleParameter outParam = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
					cmd.Parameters.Add(outParam);

					cmd.ExecuteNonQuery();

					OracleDataReader dr = ((OracleRefCursor)outParam.Value).GetDataReader();
					DataTable dt = new DataTable();
					dt.Load(dr);

					return dt;
				}
			}
		}





		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

			DataTable dt = GetStudents();
			///combining the grid with the data table
			dataGridView1.DataSource = dt;

		}
		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			// If the 'Add Attendance' button is clicked.
			string c_temp = comboBox1.SelectedItem.ToString();
			if (dataGridView1.Columns[e.ColumnIndex].Name == "AddAttendanceButton")
			{
				// Get the student id from the 's_id' cell of the same row.
				int s_id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["s_id"].Value);

				// Show a MessageBox asking if the student is present.
				DialogResult dialogResult = MessageBox.Show("Is the student present?", "Attendance", MessageBoxButtons.YesNo);

				// If 'Yes' (Present) is clicked, call the AddAttendance method with "present".
				if (dialogResult == DialogResult.Yes)
				{
					AddAttendance(c_temp, s_id, dateTimePicker1.Value, "present");
				}
				// If 'No' (Absent) is clicked, call the AddAttendance method with "absent".
				else if (dialogResult == DialogResult.No)
				{
					AddAttendance(c_temp, s_id, dateTimePicker1.Value, "absent");
				}
			}
		}




		private void Form26_Load_1(object sender, EventArgs e)
		{
			string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
			con = new OracleConnection(conStr);
			DataTable dt = GetStudents();
			put_p_c_ids();
			// Add a new button column.
			DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
			comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);

			buttonColumn.HeaderText = "Add Attendance";
			buttonColumn.Name = "AddAttendanceButton";
			buttonColumn.Text = "Add Attendance";
			buttonColumn.UseColumnTextForButtonValue = true;

			dataGridView1.Columns.Add(buttonColumn);

			dataGridView1.DataSource = dt;
		}

		public void put_p_c_ids()
		{
			//creating a list to hold the resultant query c_ids
			List<string> classIds = new List<string>();

			// Create a new connection.
			using (OracleConnection conn = new OracleConnection("DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;"))
			{
				// Open the connection.
				conn.Open();

				// Create a new command.
				string query3 = "select t_id from teacher where email=:ReceivedText2";
				OracleCommand command = new OracleCommand(query3, conn);
				command.Parameters.Add(new OracleParameter("ReceivedText2", ReceivedText2));
				object result = command.ExecuteScalar();
				int t_id = result != null ? Convert.ToInt32(result) : 0;
				using (OracleCommand cmd = new OracleCommand("SELECT c_id FROM class where t_id=:t_id", conn))
				{
					cmd.Parameters.Add(new OracleParameter("t_id", t_id));
					// Execute the command and retrieve a data reader.
					using (OracleDataReader reader = cmd.ExecuteReader())
					{
						// Loop through the results.
						while (reader.Read())
						{
							// Add the class ID to the list.
							classIds.Add(reader.GetString(0));
						}
					}
				}
			}

			// Populate the ComboBox with the class IDs.
			comboBox1.Items.Clear();
			comboBox1.Items.AddRange(classIds.ToArray());
		}


		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedClassId = comboBox1.SelectedItem.ToString();

			// Fetch the list of students in the selected class.
			DataTable dt = GetStudents(selectedClassId);

			// Display the list of students in the DataGridView.
			dataGridView1.DataSource = dt;
		}

		private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
		{
            string selectedClassId = comboBox1.SelectedItem.ToString();
          
            con.Open();
            OracleCommand getSTD = new OracleCommand();
            getSTD = con.CreateCommand();
			DateTime formattedDate = dateTimePicker1.Value.Date;
            string formattedDateString = formattedDate.ToString("MM/dd/yyyy");
            MessageBox.Show(formattedDateString);
			MessageBox.Show(selectedClassId);
            getSTD.CommandText = " select COUNT(*)from ATTENDANCE where class_date =TO_DATE(:formattedDate, 'MM/dd/yyyy') and c_id=:c_id";
            getSTD.Parameters.Add(":c_id", OracleDbType.Varchar2).Value = selectedClassId;
            getSTD.Parameters.Add(":formattedDate", OracleDbType.Varchar2).Value = formattedDateString;
            //getSTD.Parameters.Add(":formateddate", OracleDbType.Date).Value = formattedDate;

            int count1 = Convert.ToInt32(getSTD.ExecuteScalar());

            con.Close();
            if (count1 > 0)
            {
                MessageBox.Show("Already exists");
            }
            else
            {
                MessageBox.Show(formattedDateString);


            }


            // Fetch the list of students in the selected class.
            DataTable dt = GetStudents(selectedClassId);

            // Display the list of students in the DataGridView.
            dataGridView1.DataSource = dt;
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
		{

		}
	}
}



