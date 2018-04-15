using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Controllers;

namespace Pharmacy.Views
{
    public partial class AdmisitratorView : Form
    {
        private int userId;

        private int lastMedicationId = -1;
        private int lastUserId = -1;

        public AdmisitratorView(int userId)
        {
            this.userId = userId;

            InitializeComponent();

            RefreshTab1List();
        }

        #region Tab1

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == checkBox1)
            {
                textBox1.ReadOnly = !checkBox1.Checked;
            }
            else if (sender == checkBox2)
            {
                textBox2.ReadOnly = !checkBox2.Checked;
            }
            else if (sender == checkBox3)
            {
                textBox3.ReadOnly = !checkBox3.Checked;
            }

            RefreshTab1List();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            RefreshTab1List();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                lastMedicationId = ((Medication)listBox1.Items[listBox1.SelectedIndex]).Id;
                RefreshTab1Fields();
            }
        }

        private void RefreshTab1List()
        {
            listBox1.Items.Clear();

            foreach (var medication in AdministratorController.SearchMedication((checkBox1.Checked ? textBox1.Text : ""),
                (checkBox2.Checked ? textBox2.Text : ""), (checkBox3.Checked ? textBox3.Text : "")))
            {
                listBox1.Items.Add(medication);
            }
        }

        private void RefreshTab1Fields()
        {
            if (lastMedicationId != -1)
            {
                //update
                var medication = AdministratorController.GetMedication(lastMedicationId);

                textBox4.Text = medication.Id.ToString();
                textBox5.Text = medication.Name;
                textBox6.Text = medication.Manufacturer;
                textBox7.Text = medication.Ingredients;
                numericUpDown1.Value = medication.Price;
                numericUpDown2.Value = medication.Stock;

            }
            else
            {
                //new

                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lastMedicationId = AdministratorController.AddOrUpdateMedication(
                (lastMedicationId == -1 ? 0 : lastMedicationId), textBox5.Text, textBox6.Text,
                textBox7.Text, (int) numericUpDown1.Value, (int) numericUpDown2.Value).Id;

            RefreshTab1List();
            RefreshTab1Fields();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lastMedicationId = -1;
            RefreshTab1Fields();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lastMedicationId != -1)
            {
                AdministratorController.DeleteMedication(Convert.ToInt32(textBox4.Text));
                lastMedicationId = -1;

                RefreshTab1List();
                RefreshTab1Fields();
            }
        }

        #endregion

        #region Tab2

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            RefreshTab2List();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                lastUserId = ((User)listBox2.Items[listBox2.SelectedIndex]).Id;
                RefreshTab2Fields();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lastUserId = AdministratorController.AddOrUpdateUser((lastUserId == -1 ? 0 : lastUserId), textBox10.Text,
                textBox11.Text, textBox12.Text, UserType.Chemist).Id;

            RefreshTab2List();
            RefreshTab2Fields();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lastUserId = -1;
            RefreshTab2Fields();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (lastUserId != -1)
            {
                AdministratorController.DeleteUser(Convert.ToInt32(textBox9.Text));
                lastUserId = -1;

                RefreshTab2List();
                RefreshTab2Fields();
            }
        }

        private void RefreshTab2List()
        {
            listBox2.Items.Clear();

            foreach (var chemist in AdministratorController.SearchChemists(textBox8.Text.NormalizeString(),
                textBox8.Text.NormalizeString()))
            {
                listBox2.Items.Add(chemist);
            }
        }

        private void RefreshTab2Fields()
        {
            if (lastUserId != -1)
            {
                //update
                var user = AdministratorController.GetUser(lastUserId);

                textBox9.Text = user.Id.ToString();
                textBox10.Text = user.Username;
                textBox11.Text = user.Name;
                textBox12.Text = user.Password;
            }
            else
            {
                //new

                textBox9.Text = "";
                textBox10.Text = "";
                textBox11.Text = "";
                textBox12.Text = "";
            }
        }

        #endregion

        #region Tab3

        private void button7_Click(object sender, EventArgs e)
        {
            AdministratorController.GenerateReport(radioButton1.Checked ? "CSV" : "PDF");
            Process.Start("explorer.exe", "/select,"+Path.GetFullPath(radioButton1.Checked ? "report.csv" : "report.pdf"));
            MessageBox.Show("Success!");
        }

        #endregion

        #region Tab4

        private void button8_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                AdministratorController.ImportMedication(openFileDialog1.FileName);
                MessageBox.Show("Success!");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AdministratorController.ExportMedication();
            Process.Start("explorer.exe", "/select," + Path.GetFullPath("medication.xml"));
        }

        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
