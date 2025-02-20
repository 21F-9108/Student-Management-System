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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsApp2
{

	public partial class Form25 : Form
	{
		public string ReceivedText { get; set; }
		public Form25()
		{
			InitializeComponent();
		}

		private void Form25_Load(object sender, EventArgs e)
		{
			DataTable dt = GetStudents();
			dataGridView1.DataSource = dt;
		}
		public DataTable GetStudents()
		{
			string teacher_email_to_find_teacher_id;
			////OracleConnection represents a connection to an Oracle Database. 
			////you initialize it with a connection string, which contains information about how to connect to the database.
			using (OracleConnection conn = new OracleConnection("DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;"))
			{
				// Open the connection to the database.
				conn.Open();
				teacher_email_to_find_teacher_id = ReceivedText;


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

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

			DataTable dt = GetStudents();

			/////Bind the DataTable to the DataGridView.
			dataGridView1.DataSource = dt;
		}



		
	}
}
