using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Controllers;

namespace Pharmacy.Views
{
    public partial class RegularUserView : Form
    {
        private int lastId = -1;

        public RegularUserView()
        {
            InitializeComponent();

            toolTip1.SetToolTip(checkBox3, "Comma separated list of ingredients.");
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == checkBox1)
            {
                textBox1.ReadOnly = checkBox1.Checked;
            }
            else if (sender == checkBox2)
            {
                textBox2.ReadOnly = checkBox2.Checked;
            }
            else if (sender == checkBox3)
            {
                textBox3.ReadOnly = checkBox3.Checked;
            }

            RefreshList();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                lastId = listBox1.SelectedIndex;
                RefreshFields();
            }
        }

        private void RefreshList()
        {
            listBox1.Items.Clear();

            foreach (var medication in RegularUserController.SearchMedication((checkBox1.Checked ? textBox1.Text : ""),
                (checkBox2.Checked ? textBox2.Text : ""), (checkBox3.Checked ? textBox3.Text : "")))
            {
                listBox1.Items.Add(medication);
            }
        }

        private void RefreshFields()
        {
            if (lastId != -1)
            {
                //update
                var medication = RegularUserController.GetMedication(lastId);

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
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lastId != -1)
            {
                
            }

            lastId = -1;
            RefreshList();
            RefreshFields();
        }
    }
}
