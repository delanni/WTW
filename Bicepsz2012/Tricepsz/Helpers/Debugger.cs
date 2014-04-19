using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tricepsz.Helpers
{
    public partial class Debugger : Form
    {
        private static int Ordinal=0;
        private Timer timer;
        public static List<string> Messages;
        public static void Log(string message)
        {
            Messages.Add(Ordinal++.ToString() + " [" + DateTime.Now.ToShortTimeString() + "] " + message);
        }

        public Debugger()
        {
            InitializeComponent();
            Debugger.Messages = new List<string>();
            timer = new Timer();
            timer.Interval = 500;
            timer.Enabled = true;
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            listBox1.DataSource = Messages.Where(x => x.Contains(textBox1.Text)).ToList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Messages.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
