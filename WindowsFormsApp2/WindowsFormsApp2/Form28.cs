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
	public partial class Form28 : Form
	{
		public string ReceivedText4 { get; set; }
		public Form28()
		{
			InitializeComponent();
		}
		public void put_p_c_ids1()
		{
			List<string> classIds = new List<string>();
			using (OracleConnection conn = new OracleConnection("DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;"))
			{
				conn.Open();
				string query3 = "select t_id from teacher where email=:ReceivedText4";
				OracleCommand command = new OracleCommand(query3, conn);
				command.Parameters.Add(new OracleParameter("ReceivedText4", ReceivedText4 ?? string.Empty));
				object result = command.ExecuteScalar();
				int t_id = result != null ? Convert.ToInt32(result) : 0;
				using (OracleCommand cmd = new OracleCommand("SELECT c_id FROM class where t_id=:t_id", conn))
				{
					cmd.Parameters.Add(new OracleParameter("t_id", t_id));
					using (OracleDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							classIds.Add(reader.GetString(0));
						}
					}
				}
			}
			comboBox1.Items.Clear();
			comboBox1.Items.AddRange(classIds.ToArray());
		}
		public DataTable GetClasses()
		{
			DataTable table = new DataTable();
			using (OracleConnection conn = new OracleConnection("DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;"))
			{
				conn.Open();
				using (OracleCommand cmd = new OracleCommand("SELECT DISTINCT C_ID FROM attendance", conn))
				{
					using (OracleDataReader reader = cmd.ExecuteReader())
					{
						table.Load(reader);
					}
				}
			}
			return table;
		}
		//public DataTable Get_Student_Attendance_InClass(string c_id)
		//{
		//	DataTable table = new DataTable();
		//	using (OracleConnection conn = new OracleConnection("DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;"))
		//	{
		//		conn.Open();
		//		using (OracleCommand cmd = new OracleCommand("SELECT S_ID, COUNT(*) AS ClassCount FROM attendance WHERE C_ID = :c_id GROUP BY S_ID", conn))
		//		{
		//			cmd.Parameters.Add(new OracleParameter("c_id", c_id));

		//			using (OracleDataReader reader = cmd.ExecuteReader())
		//			{
		//				table.Load(reader);
		//			}
		//		}
		//	}

		//	return table;
		//}

		public DataTable Get_Student_Attendance_InClass(string c_id)
		{
			DataTable table = new DataTable();

			using (OracleConnection conn = new OracleConnection("DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;"))
			{
				conn.Open();

				string query = @"
            SELECT A.C_ID, 
                   A.S_ID, 
                   B.TotalClasses,
                   COUNT(DISTINCT A.CLASS_DATE) AS ClassesAttended,
                   (COUNT(DISTINCT A.CLASS_DATE) / B.TotalClasses) * 100 AS AttendanceRate
            FROM ATTENDANCE A
            INNER JOIN (
                SELECT C_ID, COUNT(DISTINCT CLASS_DATE) AS TotalClasses
                FROM ATTENDANCE 
                GROUP BY C_ID
            ) B ON A.C_ID = B.C_ID
            WHERE A.STATUS = 'Present' AND A.C_ID = :c_id
            GROUP BY A.C_ID, A.S_ID, B.TotalClasses";

				using (OracleCommand cmd = new OracleCommand(query, conn))
				{
					cmd.Parameters.Add(new OracleParameter("c_id", c_id));

					using (OracleDataReader reader = cmd.ExecuteReader())
					{
						table.Load(reader);
					}
				}
			}

			return table;
		}




		private void Form28_Load(object sender, EventArgs e)
		{
			put_p_c_ids1();
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedClass = comboBox1.SelectedItem.ToString();
			DataTable dt = Get_Student_Attendance_InClass(selectedClass);
			dataGridView1.DataSource = dt;
		}


	}
}
