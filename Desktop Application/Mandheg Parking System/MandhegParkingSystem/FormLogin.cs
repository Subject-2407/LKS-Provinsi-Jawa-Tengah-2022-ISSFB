using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace MandhegParkingSystem
{
    public partial class FormLogin : Form
    {

        ParkingDataContext context = new ParkingDataContext();
        public FormLogin()
        {
            InitializeComponent();
        }

        static string computeHash(string text)
        {
            var crypt = new SHA256Managed();
            var stringBuilder = new StringBuilder();
            byte[] Bytes = crypt.ComputeHash(Encoding.UTF8.GetBytes(text));
            foreach(byte b in Bytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(empIdBox.Text) && String.IsNullOrEmpty(passBox.Text))
            {
                MessageBox.Show("Please fill the Employee ID and passwsord!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int tryParse;
            if(!int.TryParse(empIdBox.Text, out tryParse ))
            {
                MessageBox.Show("ID can only be numbers!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var p = context.Employees.Where(q => q.id.ToString() == empIdBox.Text && q.password == computeHash(passBox.Text)).FirstOrDefault();
            if (p == null)
            {
                MessageBox.Show("Invalid login data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Login successful! Your Employee ID is : " + p.id, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                storedEmployee.name = p.name;
                this.Hide();
                var newForm = new FormMain();
                newForm.FormClosed += (s, args) => this.Close();
                newForm.Show();
                
            }

        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
