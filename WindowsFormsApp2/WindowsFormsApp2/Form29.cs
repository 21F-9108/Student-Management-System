using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
	public partial class Form29 : Form
	{
		int s_id;
		public Form29(int id)
		{
			InitializeComponent();
			s_id = id;
			
		}

		public void put_p_c_ids1()
		{
			List<string> classIds = new List<string>();
			using (OracleConnection conn = new OracleConnection("DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;"))
			{
				conn.Open();

				using (OracleCommand cmd = new OracleCommand("SELECT c_id FROM enrollment WHERE s_id = :s_id", conn))
				{
					cmd.Parameters.Add(new OracleParameter("s_id", s_id));
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

		public DataTable Get_Student_Attendance_InClass(string c_id, int s_id)
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
        WHERE A.STATUS = 'Present' AND A.C_ID = :c_id AND A.S_ID = :s_id
        GROUP BY A.C_ID, A.S_ID, B.TotalClasses";

				using (OracleCommand cmd = new OracleCommand(query, conn))
				{
					cmd.Parameters.Add(new OracleParameter("c_id", c_id));
					cmd.Parameters.Add(new OracleParameter("s_id", s_id));

					using (OracleDataReader reader = cmd.ExecuteReader())
					{
						table.Load(reader);
					}
				}
			}

			return table;
		}



		private void Form29_Load(object sender, EventArgs e)
		{
			put_p_c_ids1();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedClass = comboBox1.SelectedItem.ToString();
			DataTable dt = new DataTable();

		     dt = Get_Student_Attendance_InClass(selectedClass, s_id);

			dataGridView1.DataSource = dt;
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}
	}
}
