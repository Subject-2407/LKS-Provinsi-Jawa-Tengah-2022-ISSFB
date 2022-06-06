using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MandhegParkingSystem
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            empNamePlaceholder.Text = "Welcome, " + storedEmployee.name;
            dayTimePlaceholder.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy, HH:mm:ss");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dayTimePlaceholder.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy, HH:mm:ss");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var newForm = new FormMember();
            newForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var newForm = new FormVehicle();
            newForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var newForm = new FormPayment();
            newForm.Show();
        }
    }
}
