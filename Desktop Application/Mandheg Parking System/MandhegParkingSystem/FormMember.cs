using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MandhegParkingSystem
{
    public partial class FormMember : Form
    {
        private FormState _formState;
        private Button currentBtn;

        public FormState FormState
        {
            get { return _formState; }
            set
            {
                _formState = value;
                switch(value)
                {
                    case FormState.Read:
                        nameBox.Enabled = false;
                        insertBtn.Enabled = true;
                        deleteBtn.Enabled = false;
                        submitBtn.Enabled = false;
                        cancelBtn.Enabled = false;
                        updBtn.Enabled = false;
                        updBtn.BackColor = Color.FromName("IndianRed");
                        insertBtn.BackColor = Color.FromName("IndianRed");
                        membershipBox.Enabled = false;
                        emailBox.Enabled = false;
                        phoneBox.Enabled = false;
                        addressBox.Enabled = false;
                        birthDatePicker.Enabled = false;
                        maleRadio.Enabled = false;
                        femaleRadio.Enabled = false;
                        break;
                    case FormState.Insert:
                        nameBox.Enabled = true;
                        submitBtn.Enabled = true;
                        cancelBtn.Enabled = true;
                        updBtn.Enabled = false;
                        deleteBtn.Enabled = false;
                        insertBtn.BackColor = Color.FromName("Salmon");
                        insertBtn.ForeColor = Color.FromName("White");
                        updBtn.BackColor = Color.FromName("IndianRed");
                        membershipBox.Enabled = true;
                        emailBox.Enabled = true;
                        phoneBox.Enabled = true;
                        addressBox.Enabled = true;
                        birthDatePicker.Enabled = true;
                        maleRadio.Enabled = true;
                        femaleRadio.Enabled = true;
                        break;
                    case FormState.Update:
                        nameBox.Enabled = true;
                        insertBtn.Enabled = false;
                        deleteBtn.Enabled = false;
                        submitBtn.Enabled = true;
                        cancelBtn.Enabled = true;
                        updBtn.BackColor = Color.FromName("Salmon");
                        updBtn.ForeColor = Color.FromName("White");
                        insertBtn.BackColor = Color.FromName("IndianRed");
                        membershipBox.Enabled = true;
                        emailBox.Enabled = true;
                        phoneBox.Enabled = true;
                        addressBox.Enabled = true;
                        birthDatePicker.Enabled = true;
                        maleRadio.Enabled = true;
                        femaleRadio.Enabled = true;
                        break;
                }
            }
        }
        ParkingDataContext context = new ParkingDataContext();
        public FormMember()
        {
            InitializeComponent();
        }

        public void disableBtn() {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromName("IndianRed");
                currentBtn.ForeColor = Color.FromName("White");
            }
        }

        public void activateBtn(object sender)
        {
                      
            currentBtn = (Button)sender;
            currentBtn.BackColor = Color.FromName("Salmon");
            currentBtn.ForeColor = Color.FromName("Black");
        }

        private void FormMember_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'mandhegParkingSystem3DataSet.Membership' table. You can move, or remove it, as needed.
            this.membershipTableAdapter.Fill(this.mandhegParkingSystem3DataSet.Membership);
            // TODO: This line of code loads data into the 'mandhegParkingSystem3DataSet.Member' table. You can move, or remove it, as needed.
            this.memberTableAdapter.Fill(this.mandhegParkingSystem3DataSet.Member);

            var x = (from p in context.Members
                     where p.deleted_at == null
                     select new
                     {
                         Id = p.id,
                         Name = p.name,
                         PhoneNumber = p.phone_number,
                         Address = p.address,
                         DateofBirth = p.date_of_birth,
                         Gender = p.gender
                     }).ToList();

            dataGridView1.DataSource = x;
            dataGridView1.Refresh();
            dataGridView1.ClearSelection();
            membershipBox.SelectedValue = 0;

        }

        public void refreshData()
        {
            var x = (from p in context.Members
                     where p.deleted_at == null
                     select new
                     {
                         Id = p.id,
                         MembershipId = p.membership_id,
                         Name = p.name,
                         PhoneNumber = p.phone_number,
                         Address = p.address,
                         DateofBirth = p.date_of_birth,
                         Gender = p.gender
                     }).ToList();

            dataGridView1.DataSource = x;
            dataGridView1.Refresh();
            dataGridView1.ClearSelection();
        }

        public void clearData()
        {
            nameBox.Text = "";
            membershipBox.SelectedValue = 0;
            emailBox.Text = "";
            phoneBox.Text = "";
            addressBox.Text = "";
            maleRadio.Checked = false;
            femaleRadio.Checked = false;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {    
            if(FormState == FormState.Insert)
            {
                return;
            }
            storedMember.id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            updBtn.Enabled = true;
            deleteBtn.Enabled = true;
            var p = context.Members.Where(q => q.id== storedMember.id).FirstOrDefault();
            nameBox.Text = p.name;
            membershipBox.SelectedValue = p.membership_id;
            emailBox.Text = p.email;
            phoneBox.Text = p.phone_number;
            addressBox.Text = p.address;
            birthDatePicker.Value = p.date_of_birth;
            if (p.gender == "Male")
            {
                maleRadio.Checked = true;
            }
            else femaleRadio.Checked = true;
            
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this data?","Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            var p = context.Members.Single(q => q.id == storedMember.id);
            p.deleted_at = DateTime.Now;
            context.SubmitChanges();
            MessageBox.Show("Successfully deleted data with ID : " + storedMember.id);
            refreshData();

        }

        private void updBtn_Click(object sender, EventArgs e)
        {
            FormState = FormState.Update;
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            if(FormState == FormState.Insert)
            {
                if(String.IsNullOrEmpty(nameBox.Text)
                    || Convert.ToInt32(membershipBox.SelectedValue) == 0
                    || String.IsNullOrEmpty(emailBox.Text)
                    || String.IsNullOrEmpty(phoneBox.Text)
                    || String.IsNullOrEmpty(addressBox.Text)
                    || (maleRadio.Checked == false && femaleRadio.Checked == false)
                    )
                {
                    MessageBox.Show("Please fill all the fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Regex regEmail = new Regex(@"^[a-z0-9._]+@[a-z]+\.[a-z]{2,4}$");
                Regex regPhone = new Regex(@"^[0-9]{1}-[0-9]{3}-[0-9]{3}-[0-9]{4}$");

                if (!regEmail.IsMatch(emailBox.Text))
                {
                    MessageBox.Show("Incorrect email format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!regPhone.IsMatch(phoneBox.Text))
                {
                    MessageBox.Show("Incorrect email format! Format must be x-xxx-xxx-xxxx", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    Member insertMem = new Member();
                    insertMem.name = nameBox.Text;
                    insertMem.membership_id = Convert.ToInt32(membershipBox.SelectedValue);
                    insertMem.email = emailBox.Text;
                    insertMem.phone_number = phoneBox.Text;
                    insertMem.address = addressBox.Text;
                    insertMem.date_of_birth = birthDatePicker.Value;
                    insertMem.created_at = DateTime.Now;

                    if (maleRadio.Checked == true)
                    {
                        insertMem.gender = "Male";
                    }
                    else insertMem.gender = "Female";
                    context.Members.InsertOnSubmit(insertMem);
                    context.SubmitChanges();
                    MessageBox.Show("Successfully inserted new member with ID : " + insertMem.id, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refreshData();
                    clearData();
                } catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if(FormState == FormState.Update)
            {
                if(String.IsNullOrEmpty(nameBox.Text)
                    || Convert.ToInt32(membershipBox.SelectedValue) == 0
                    || String.IsNullOrEmpty(emailBox.Text)
                    || String.IsNullOrEmpty(phoneBox.Text)
                    || String.IsNullOrEmpty(addressBox.Text)
                    || (maleRadio.Checked == false && femaleRadio.Checked == false)
                    )
                {
                    MessageBox.Show("Please fill all the fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Regex regEmail = new Regex(@"^[a-z0-9._]+@[a-z]+\.[a-z]{2,4}$");
                Regex regPhone = new Regex(@"^[0-9]{1}-[0-9]{3}-[0-9]{3}-[0-9]{4}$");

                if (!regEmail.IsMatch(emailBox.Text))
                {
                    MessageBox.Show("Incorrect email format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!regPhone.IsMatch(phoneBox.Text))
                {
                    MessageBox.Show("Incorrect email format! Format must be x-xxx-xxx-xxxx", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    var updateMem = context.Members.Single(p => p.id == storedMember.id);
                    updateMem.name = nameBox.Text;
                    updateMem.membership_id = Convert.ToInt32(membershipBox.SelectedValue);
                    updateMem.email = emailBox.Text;
                    updateMem.phone_number = phoneBox.Text;
                    updateMem.address = addressBox.Text;
                    updateMem.date_of_birth = birthDatePicker.Value;
                    updateMem.last_updated_at = DateTime.Now;

                    if (maleRadio.Checked == true)
                    {
                        updateMem.gender = "Male";
                    }
                    else updateMem.gender = "Female";
                    context.SubmitChanges();
                    MessageBox.Show("Successfully updated a member with ID : " + updateMem.id, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refreshData();
                    clearData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
            FormState = FormState.Read;
        }

        private void insertBtn_Click(object sender, EventArgs e)
        {
            clearData();
            FormState = FormState.Insert;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            if(FormState == FormState.Insert)
            {
                clearData();
            }
            FormState = FormState.Read;
        }
    }
}
