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


namespace WindowsFormsApp2
{
	public partial class Form24 : Form
	{
		public string ReceivedText1 { get; set; }
		public Form24()
		{
			InitializeComponent();
		}


		private void Form24_Load(object sender, EventArgs e)
		{

		}

		
		



		private void button1_Click(object sender, EventArgs e)
		{
			Form25 f25 = new Form25();
			this.Hide();
			f25.ReceivedText = ReceivedText1;
			f25.Show();
		}

		//private void button2_Click(object sender, EventArgs e)
		//{
			
		//}

		private void button3_Click(object sender, EventArgs e)
		{
			Form27 f27 = new Form27();
			this.Hide();
			f27.ReceivedText3 = ReceivedText1;
			f27.Show();
		}

		private void button4_Click(object sender, EventArgs e)
		{

			Form28 f28 = new Form28();
			f28.ReceivedText4 = ReceivedText1;
			this.Hide();
			f28.Show();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Form26 f26 = new Form26();
			this.Hide();
			f26.ReceivedText2 = ReceivedText1;
			f26.Show();
		}

		//private void button2_Click_1(object sender, EventArgs e)
		//{
		//	Form26 f26 = new Form26();
		//	this.Hide();
		//	f26.ReceivedText2 = ReceivedText1;
		//	f26.Show();
		//}


	}
}
