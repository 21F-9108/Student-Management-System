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
using System.Security.Cryptography;

namespace WindowsFormsApp2
{
	//dfghjfghjdfghertyuiortyuiorty
    public partial class Form1 : Form
    {
        OracleConnection con;
		int logged_sid;
		public string hashing_tec(string password)
		{
			SHA256 hash_1 = SHA256.Create();
			//this will take the string of password and change it to byte type since we need it to be byte type for computehash function
			//var is the datatype that converts to the type being assigend to it
			var password_in_bytes = Encoding.Default.GetBytes(password);
			///hash_1.ComputeHash(password_in_bytes);//this will return an array of bytes that we will later convert back to string

			var hashed_password = hash_1.ComputeHash(password_in_bytes);
			return BitConverter.ToString(hashed_password).Replace("-", "");//this bit converter will convert the byte to string and since it places hypehn automatically replace is used to remove them
		}

		public string salting_tec(string password)
		{
			var salt_1 = DateTime.Now.ToString();
			return salt_1;


		}
		public Form1()
        {
            InitializeComponent();
        }

		private void editing_admin_pass()
		{
			
			con.Open();
			using (OracleConnection connection = new OracleConnection(connectionString))
			{
				string insertQuery = "INSERT INTO ADMINISTRATOR (Admin_Id,email, Password,salt, Role) " +
					"VALUES (:adminId, :email, :password,:salt,:role)";
				string[] pass_arr = { "123", "321", "321", "111", "333", "444", "555", "222", "999", "1010" };
				string[] email_arr = { "admin1@gmail.com", "admin2@gmail.com", "admin3@gmail.com", "admin4@gmail.com", "admin5@gmail.com", "admin5@gmail.com", "admin6@gmail.com", "admin7@gmail.com", "admin8@gmail.com", "admin9@gmail.com", "admin10@gmail.com" };
				int[] admin_ids = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
				string[] role_arr = { "Manager", "Admin", "Dean", "Aids", "HRD", "Cheif of information", "HOD", "Registrar",
				"Provost","DeptChair"};
				for (int i = 0; i < 10; i++)
				{
					string final_salt = salting_tec(pass_arr[i]);
					string final_hash = hashing_tec(pass_arr[i]);
					string final_pass = final_hash + final_salt;
					using (OracleCommand command = new OracleCommand(insertQuery, con))
					{

						command.Parameters.Add(new OracleParameter("adminId", admin_ids[i]));
						command.Parameters.Add(new OracleParameter("email", email_arr[i]));
						command.Parameters.Add(new OracleParameter("password", final_pass));
						command.Parameters.Add(new OracleParameter("salt", final_salt));
						command.Parameters.Add(new OracleParameter("role", role_arr[i]));


						command.ExecuteNonQuery();
						///MessageBox.Show("Data Inserted Successfully");
					}
				}

			}
			con.Close();
		}
		private void Form1_Load(object sender, EventArgs e)
        {
			string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
			con = new OracleConnection(conStr);
			string connectionString = @"DATA SOURCE= localhost:1521/XE; USER ID=test2;PASSWORD=123";
			//editing_admin_pass();

		}
		string connectionString = @"DATA SOURCE= localhost:1521/XE; USER ID=F219108;PASSWORD=123";

		private string CheckUserType(string email)
		{

			using (OracleConnection connection = new OracleConnection(connectionString)) // Create a connection to the database
			{
				connection.Open(); // Open the database connection

				/////checking in Student table
				string query = "SELECT 'Student' FROM Student WHERE Email = :Email"; //sqlquery to check if the email exists in the Students table
				OracleCommand command = new OracleCommand(query, connection);//// Create a OracleCommand object
				command.Parameters.Add(new OracleParameter("Email", email));//// add a parameter to the query to avoid SQL injection attacks

				string userType = command.ExecuteScalar() as string; /////execute the query and get the user as a string

				if (userType != null)
				///// If the user type is not null, it means the email exists in the Students table
				{
					return userType;
					////return the user type
				}

				///// Checking in teacher table
				query = "SELECT 'Teacher' FROM Teacher WHERE Email = :Email"; // SQL query to check if the email exists in the Teachers table
				command.CommandText = query; // Update the SQL command with the new query
				userType = command.ExecuteScalar() as string; // Execute the query and get the user type (if exists) as a string

				if (userType != null)
				///// If the user type is not null, it means the email exists in the Teachers table
				{
					return userType;
					///// Return the user type
				}

				///// Check in Administrators table
				query = "SELECT 'Administrator' FROM Administrator WHERE Email = :Email"; // SQL query to check if the email exists in the Administrators table
				command.CommandText = query;
				//// Update the SQL command with the new query
				userType = command.ExecuteScalar() as string; // Execute the query and get the user type (if exists) as a string

				return userType;
				///// Return the user type

			}


		}

		private bool check_password(string usertype, string password, string email)
		{
			//con.Open();
			string saved_password;
			string g_password;
			string h_pass;
			string saved_salt;
			using (OracleConnection connection = new OracleConnection(connectionString)) // Create a connection to the database
			{
				connection.Open(); // Open the database connection
								   /////checking in Student table

				if (usertype == "Student")
				{

					string query = "SELECT password FROM Student WHERE Email = :Email"; //sqlquery to check if the email exists in the Students table
					OracleCommand command = new OracleCommand(query, connection);
					command.Parameters.Add(new OracleParameter("Email", email));//// add a parameter to the query to avoid SQL injection attacks
					saved_password = command.ExecuteScalar() as string; /////execute the query and get the user as a string
					if (saved_password != null)
					///// If the password is not null, it means the email exists in the Students table
					{
						string query2 = "SELECT salt FROM Student WHERE Email = :Email"; //sqlquery to check if the email exists in the Students table
						command = new OracleCommand(query2, connection);
						command.Parameters.Add(new OracleParameter("Email", email));//// add a parameter to the query to avoid SQL injection attacks
						saved_salt = command.ExecuteScalar() as string;
						h_pass = hashing_tec(password);
						g_password = h_pass + saved_salt;
						if (saved_password == g_password)
						{
							con.Open();
							OracleCommand command1 = con.CreateCommand();
							command1.CommandText = "SELECT s_id FROM Student WHERE Email = :Email";
							command1.Parameters.Add(":Email", OracleDbType.Varchar2).Value = email ;
							string s_id = command1.ExecuteScalar().ToString();
							logged_sid = Convert.ToInt32(s_id);
							con.Close();
							return true;
						}

						////return the user type
					}
				}
				else if (usertype == "Teacher")
				{
					string query = "SELECT password FROM teacher WHERE Email = :Email"; //sqlquery to check if the email exists in the Students table
					OracleCommand command = new OracleCommand(query, connection);
					command.Parameters.Add(new OracleParameter("Email", email));//// add a parameter to the query to avoid SQL injection attacks
					saved_password = command.ExecuteScalar() as string; /////execute the query and get the user as a string
					if (saved_password != null)
					///// If the password is not null, it means the email exists in the Students table
					{
						string query2 = "SELECT salt FROM teacher WHERE Email = :Email"; //sqlquery to check if the email exists in the Students table
						command = new OracleCommand(query2, connection);
						command.Parameters.Add(new OracleParameter("Email", email));//// add a parameter to the query to avoid SQL injection attacks
						saved_salt = command.ExecuteScalar() as string;
						h_pass = hashing_tec(password);
						g_password = h_pass + saved_salt;
						if (saved_password == g_password)
						{
							return true;
						}


					}

				}

				else if (usertype == "Administrator")
				{
                    string query = "SELECT password FROM administrator WHERE Email = :Email"; //sqlquery to check if the email exists in the Students table
                    OracleCommand command = new OracleCommand(query, connection);
                    command.Parameters.Add(new OracleParameter("Email", email));//// add a parameter to the query to avoid SQL injection attacks
                    saved_password = command.ExecuteScalar() as string; /////execute the query and get the user as a string
                    if (saved_password != null)
                    ///// If the password is not null, it means the email exists in the Students table
                    {
                        string query2 = "SELECT salt FROM administrator WHERE Email = :Email"; //sqlquery to check if the email exists in the Students table
                        command = new OracleCommand(query2, connection);
                        command.Parameters.Add(new OracleParameter("Email", email));//// add a parameter to the query to avoid SQL injection attacks
                        saved_salt = command.ExecuteScalar() as string;
                        h_pass = hashing_tec(password);
                        g_password = h_pass + saved_salt;
                        if (saved_password == g_password)
                        {
                            return true;
                        }


                    }

     //               //you can commnent the follwoing lines after hashing admin ka password
     //               string query = "SELECT password FROM administrator WHERE Email = :Email"; //sqlquery to check if the email exists in the Students table
					//OracleCommand command = new OracleCommand(query, connection);
					//command.Parameters.Add(new OracleParameter("Email", email));//// add a parameter to the query to avoid SQL injection attacks
					//saved_password = command.ExecuteScalar() as string;
					//if (saved_password == password)
     //               {
					//	return true;
     //               }

				}



				return false;



			}

		}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
			string email = textBox1.Text.Trim(); // Get the email address from the text box
			string password = textBox2.Text.ToString();
			if (string.IsNullOrEmpty(email)) // Check if the email is empty or null
			{
				MessageBox.Show("Please enter an email address."); // Display an error message if the email is not provided
				return; // Exit the method
			}

			string userType = CheckUserType(email); // Call the method to check the user type based on the email

			if (userType == null) // If the user type is null, it means the email does not exist in any table
			{
				MessageBox.Show("No such email address exists."); // Display a message indicating that the email does not exist
			}
			else
			{
				MessageBox.Show("email found.");
				if (check_password(userType, password, email))
				{
					MessageBox.Show("password is correct, logged in");
					if (userType == "Administrator")
					{
						Form2 f2 = new Form2();
						this.Hide();
						f2.Show();
					}

					if (userType == "Teacher")
                    {

						Form24 f24 = new Form24();
						this.Hide();
						f24.ReceivedText1 = email;
						f24.Show();

					}
					if (userType == "Student")
                    {
						Form22 f22=new Form22(logged_sid);
						this.Hide();
						f22.Show();
                    }
				}
				else
				{
					MessageBox.Show("password is not correct");
				}

			}
		}
    }
}
