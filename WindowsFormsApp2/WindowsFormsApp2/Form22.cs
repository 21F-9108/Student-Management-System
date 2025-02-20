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
    public partial class Form22 : Form
    {
        int s_id;
        public Form22()
        {
            InitializeComponent();
        }
        public Form22(int sid)
        {
            InitializeComponent();
            s_id = sid;
        }
        private void Form22_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form23 f23=new Form23(s_id);
            this.Hide();
            f23.Show();
        }

		private void button2_Click(object sender, EventArgs e)
		{
			Form29 f29 = new Form29(s_id);
			this.Hide();
			f29.Show();
		}
	}
}
