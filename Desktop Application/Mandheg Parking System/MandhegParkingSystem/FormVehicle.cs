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
    public partial class FormVehicle : Form
    {
        private FormState _formState;
        private Button currentBtn;

        public FormState FormState
        {
            get { return _formState; }
            set
            {
                _formState = value;
                switch (value)
                {
                    case FormState.Read:
                        vehicleBox.Enabled = false;
                        ownerBox.Enabled = false;
                        vehicletypeBox.Enabled = false;
                        notesBox.Enabled = false;
                        insertBtn.Enabled = true;
                        deleteBtn.Enabled = false;
                        submitBtn.Enabled = false;
                        cancelBtn.Enabled = false;
                        updBtn.Enabled = false;
                        updBtn.BackColor = Color.FromName("IndianRed");
                        insertBtn.BackColor = Color.FromName("IndianRed");
                        break;
                    case FormState.Insert:
                        vehicleBox.Enabled = true;
                        ownerBox.Enabled = true;
                        vehicletypeBox.Enabled = true;
                        notesBox.Enabled = true;
                        submitBtn.Enabled = true;
                        cancelBtn.Enabled = true;
                        updBtn.Enabled = false;
                        deleteBtn.Enabled = false;
                        insertBtn.BackColor = Color.FromName("Salmon");
                        insertBtn.ForeColor = Color.FromName("White");
                        updBtn.BackColor = Color.FromName("IndianRed");
                        break;
                    case FormState.Update:
                        vehicleBox.Enabled = true;
                        ownerBox.Enabled = true;
                        vehicletypeBox.Enabled = true;
                        notesBox.Enabled = true;
                        insertBtn.Enabled = false;
                        deleteBtn.Enabled = false;
                        submitBtn.Enabled = true;
                        cancelBtn.Enabled = true;
                        updBtn.BackColor = Color.FromName("Salmon");
                        updBtn.ForeColor = Color.FromName("White");
                        insertBtn.BackColor = Color.FromName("IndianRed");
                        break;

                }
            }
        }

                        ParkingDataContext context = new ParkingDataContext();
        public FormVehicle()
        {
            InitializeComponent();
        }


        private void FormVehicle_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'mandhegParkingSystem3DataSet.VehicleType' table. You can move, or remove it, as needed.
            this.vehicleTypeTableAdapter.Fill(this.mandhegParkingSystem3DataSet.VehicleType);

            var x = (from p in context.Vehicles
                     where p.deleted_at == null
                     join q in context.Members
                     on p.member_id equals q.id
                     select new
                     {
                         Id = p.id,
                         Member = q.name,
                         LicensePlate = p.license_plate,
                         Notes = p.notes,
                     }).ToList();

       

            dataGridView1.DataSource = x;
            
            dataGridView1.Refresh();
            dataGridView1.ClearSelection();
            vehicletypeBox.SelectedValue = 0;
            dataGridView1.Columns[2].HeaderText = "License Plate";

            var names = context.Members.Select(y => y.name).Distinct().ToArray();
            AutoCompleteStringCollection ownerNames = new AutoCompleteStringCollection();
            ownerNames.AddRange(names);
            ownerBox.AutoCompleteCustomSource = ownerNames;

        }

        public void refreshData()
        {
            var x = (from p in context.Vehicles
                     where p.deleted_at == null
                     join q in context.Members
                     on p.member_id equals q.id
                     select new
                     {
                         Id = p.id,
                         Member = q.name,
                         LicensePlate = p.license_plate,
                         Notes = p.notes,
                     }).ToList();



            dataGridView1.DataSource = x;

            dataGridView1.Refresh();
            dataGridView1.ClearSelection();
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            if(searchBy.Text == "Owner Name")
            {
                var x = (from p in context.Vehicles
                         where p.deleted_at == null
                         join q in context.Members
                         on p.member_id equals q.id
                         select new
                         {
                             Id = p.id,
                             Member = q.name,
                             LicensePlate = p.license_plate,
                             Notes = p.notes,
                         }).Where(p => p.Member.Contains(searchBox.Text)).ToList();

                dataGridView1.DataSource = x;
                dataGridView1.Refresh();
            }
            if (searchBy.Text == "License Plate")
            {
                var x = (from p in context.Vehicles
                         where p.deleted_at == null
                         join q in context.Members
                         on p.member_id equals q.id
                         select new
                         {
                             Id = p.id,
                             Member = q.name,
                             LicensePlate = p.license_plate,
                             Notes = p.notes,
                         }).Where(p => p.LicensePlate.Contains(searchBox.Text)).ToList();

                dataGridView1.DataSource = x;
                dataGridView1.Refresh();
            }
        }

        public void clearData()
        {
            vehicleBox.Text = "";
            ownerBox.Text = "";
            vehicletypeBox.SelectedValue = 0;
            notesBox.Text = "";
        }
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            var p = context.Vehicles.Single(q => q.id == storedVehicles.id);
            p.deleted_at = DateTime.Now;
            context.SubmitChanges();
            MessageBox.Show("Successfully deleted data with ID : " + storedVehicles.id);
            refreshData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            storedVehicles.id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            updBtn.Enabled = true;
            deleteBtn.Enabled = true;
            var p = context.Vehicles.Where(q => q.id == storedVehicles.id).FirstOrDefault();
            vehicleBox.Text = p.license_plate;
            ownerBox.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            notesBox.Text = p.notes;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            if (FormState == FormState.Insert)
            {
                clearData();
            }
            FormState = FormState.Read;
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            if (FormState == FormState.Insert)
            {
                if (String.IsNullOrEmpty(vehicleBox.Text)
                    || Convert.ToInt32(vehicletypeBox.SelectedValue) == 0
                    || String.IsNullOrEmpty(ownerBox.Text)
                    || String.IsNullOrEmpty(notesBox.Text)                 )
                {
                    MessageBox.Show("Please fill all the fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    Vehicle insertVehicle = new Vehicle();
                    var p = context.Members.Where(q => q.name == ownerBox.Text).FirstOrDefault();
                    insertVehicle.vehicle_type_id = Convert.ToInt32(vehicletypeBox.SelectedValue);
                    insertVehicle.member_id = p.id;
                    insertVehicle.license_plate = vehicleBox.Text;
                    insertVehicle.notes = notesBox.Text;
                    insertVehicle.created_at = DateTime.Now;
                    context.Vehicles.InsertOnSubmit(insertVehicle);
                    context.SubmitChanges();
                    MessageBox.Show("Successfully inserted new vehicle data with ID : " + insertVehicle.id, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refreshData();
                    clearData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (FormState == FormState.Update)
            {
                if (String.IsNullOrEmpty(vehicleBox.Text)
                    || Convert.ToInt32(vehicletypeBox.SelectedValue) == 0
                    || String.IsNullOrEmpty(ownerBox.Text)
                    || String.IsNullOrEmpty(notesBox.Text))
                {
                    MessageBox.Show("Please fill all the fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    var x = context.Members.Where(q => q.name == ownerBox.Text).FirstOrDefault();
                    var updateVeh = context.Vehicles.Single(p => p.id == storedVehicles.id);
                    updateVeh.vehicle_type_id = Convert.ToInt32(vehicletypeBox.SelectedValue);
                    updateVeh.member_id = x.id;
                    updateVeh.license_plate = vehicleBox.Text;
                    updateVeh.notes = notesBox.Text;
                    updateVeh.last_updated_at = DateTime.Now;
                    context.SubmitChanges();
                    MessageBox.Show("Successfully updated a vehicle data with ID : " + updateVeh.id, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void updBtn_Click(object sender, EventArgs e)
        {
            FormState = FormState.Update;
        }
    }
}
