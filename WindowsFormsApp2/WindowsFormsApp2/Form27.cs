using Oracle.ManagedDataAccess.Client;
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

namespace WindowsFormsApp2
{
	public partial class Form27 : Form
	{

		public string ReceivedText3 { get; set; }
		public Form27()
		{
			InitializeComponent();
		}

		private void Form27_Load(object sender, EventArgs e)
		{
			put_p_c_ids();
		}


		public void put_p_c_ids()
		{
			//creating a list to hold the resultant query c_ids
			List<string> classIds = new List<string>();

			using (OracleConnection conn = new OracleConnection("DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;"))
			{
				conn.Open();

				string query3 = "select t_id from teacher where email=:ReceivedText3";
				OracleCommand command = new OracleCommand(query3, conn);
				command.Parameters.Add(new OracleParameter("ReceivedText3", ReceivedText3));
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

			// Create a DataTable from the list
			DataTable table = new DataTable();
			table.Columns.Add("Class IDs", typeof(string));

			foreach (string id in classIds)
			{
				table.Rows.Add(id);
			}

			// Bind the DataTable to the DataGridView
			dataGridView1.DataSource = table;
		}



		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
		{

		}
	}
}
