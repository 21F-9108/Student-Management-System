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
    public partial class Form13 : Form
    {
        public Form13()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form14 f14=new Form14();
            this.Hide();
            f14.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form16 f16= new Form16();
            this.Hide();
            f16.Show();
        }

        private void Form13_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form18 f18=new Form18();
            this.Hide();
            f18.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form20 f20 = new Form20();
            this.Hide();
            f20.Show();
        }
    }
}
