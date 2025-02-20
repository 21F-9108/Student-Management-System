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
    public partial class Form10 : Form
    {
        OracleConnection con;
        Form1 obj = new Form1();

        public Form10()
        {
            InitializeComponent();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            string conStr = "DATA SOURCE= localhost:1521/XE;  USER ID=F219108; password=123; pooling=false;";
            con = new OracleConnection(conStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("First name cant be null");

            }

            else if (textBox1.Text.Length > 20)
            {
                MessageBox.Show("first name length exceeded");

            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Last name cant be null");
            }
            else if (textBox2.Text.Length > 20)
            {
                MessageBox.Show("last name length exceeded");

            }
            else if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Email cant be null");


            }
            else if (textBox3.Text.Length > 30)
            {
                MessageBox.Show("Email length exceeded");

            }
            else if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Password cant be null");


            }
            else if (textBox4.Text.Length > 100)
            {
                MessageBox.Show("password length exceeded");

            }
            else if (string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Gender cant be null");

            }
            else if (textBox6.Text.Length > 1)
            {
                MessageBox.Show("Gender length exceeded");

            }
            else if (string.IsNullOrEmpty(textBox7.Text))
            {
                MessageBox.Show("Contact no cant be null");


            }
            else if (textBox7.Text.Length > 12)
            {
                MessageBox.Show("contact length exceeded");

            }
           
            

            else if (!string.IsNullOrEmpty(textBox8.Text) && textBox8.Text.Length > 20)
            {

                {
                    MessageBox.Show("blood group length exceeded");

                }
            }
            else if (string.IsNullOrEmpty(textBox9.Text))
            {
                MessageBox.Show("Address cant be null");

            }
            else if (textBox9.Text.Length > 20)
            {
                MessageBox.Show("address length exceeded");

            }
            
           
            else
            {
               
                if (string.IsNullOrEmpty(textBox8.Text))
                {
                    textBox8.Text = null;

                }
                con.Open();


                OracleCommand check_email = con.CreateCommand();
                check_email.CommandText = " select count(*) from student where email=:email";
                check_email.Parameters.Add(":email", OracleDbType.Varchar2).Value = textBox3.Text;
                check_email.CommandType = CommandType.Text;
                string count = check_email.ExecuteScalar().ToString();
                int e_count = Convert.ToInt32(count);
                con.Close();

                con.Open();
                OracleCommand check_email2 = con.CreateCommand();
                check_email2.CommandText = " select count(*) from administrator where email=:email";
                check_email2.Parameters.Add(":email", OracleDbType.Varchar2).Value = textBox3.Text;
                check_email2.CommandType = CommandType.Text;
                string count1 = check_email2.ExecuteScalar().ToString();
                e_count += Convert.ToInt32(count1);
                con.Close();

                con.Open();
                OracleCommand check_email3 = con.CreateCommand();
                check_email3.CommandText = " select count(*) from teacher where email=:email";
                check_email3.Parameters.Add(":email", OracleDbType.Varchar2).Value = textBox3.Text;
                check_email3.CommandType = CommandType.Text;
                string count2 = check_email3.ExecuteScalar().ToString();
                e_count += Convert.ToInt32(count2);
                con.Close();
                if (e_count > 0)
                {
                    MessageBox.Show("Email already exists");
                    return;
                }
                con.Open();
                OracleCommand insertEmp = con.CreateCommand();
                string selectedDate = dateTimePicker1.Value.ToString("DD-MM-YYYY");

                string final_salt = obj.salting_tec(textBox4.Text);
                string final_hash = obj.hashing_tec(textBox4.Text);
                string final_pass = final_hash + final_salt;
                insertEmp.CommandText = "INSERT INTO teacher (fname, lname, reg_date, gender, contact_no, email, password, blood_group, address,salt) " +
                   "VALUES (:firstName, :lastName, :reg_date, :gender, :contact_no, :email, :password, :blood_group, :address,:salt)";
                insertEmp.Parameters.Add(":firstName", OracleDbType.Varchar2).Value = textBox1.Text;
                insertEmp.Parameters.Add(":lastName", OracleDbType.Varchar2).Value = textBox2.Text;
                insertEmp.Parameters.Add(":reg_date", OracleDbType.Date).Value = dateTimePicker1.Value;
                insertEmp.Parameters.Add(":gender", OracleDbType.Varchar2).Value = textBox6.Text;
                insertEmp.Parameters.Add(":contact_no", OracleDbType.Varchar2).Value = textBox7.Text;
                insertEmp.Parameters.Add(":email", OracleDbType.Varchar2).Value = textBox3.Text;
                insertEmp.Parameters.Add(":password", OracleDbType.Varchar2).Value =final_pass;
                insertEmp.Parameters.Add(":blood_group", OracleDbType.Varchar2).Value = textBox8.Text;
                insertEmp.Parameters.Add(":address", OracleDbType.Varchar2).Value = textBox9.Text;
				insertEmp.Parameters.Add(":salt", OracleDbType.Varchar2).Value = final_salt;
				insertEmp.CommandType = CommandType.Text;
                int rows = insertEmp.ExecuteNonQuery();
                MessageBox.Show("data inserted successfully");
                con.Close();
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

		private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{

		}
	}
}
