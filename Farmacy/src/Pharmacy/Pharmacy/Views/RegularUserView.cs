using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pharmacy.Controllers;
using Pharmacy.Models;

namespace Pharmacy.Views
{
    public partial class RegularUserView : Form
    {
        private int userId;

        private int lastId = -1;
        private Dictionary<int, int> cart = new Dictionary<int, int>(); //med id - count

        public RegularUserView(int userId)
        {
            this.userId = userId;

            InitializeComponent();

            toolTip1.SetToolTip(checkBox3, "Comma separated list of ingredients.");

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
                lastId = ((Medication) listBox1.Items[listBox1.SelectedIndex]).Id;
                RefreshTab1Fields();
            }
        }

        private void RefreshTab1List()
        {
            listBox1.Items.Clear();

            foreach (var medication in RegularUserController.SearchMedication((checkBox1.Checked ? textBox1.Text : ""),
                (checkBox2.Checked ? textBox2.Text : ""), (checkBox3.Checked ? textBox3.Text : "")))
            {
                listBox1.Items.Add(medication);
            }
        }

        private void RefreshTab1Fields()
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

                numericUpDown3.Maximum = medication.Stock;
                numericUpDown3.Value = (cart.ContainsKey(medication.Id) ? cart[medication.Id] : 0);

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
                numericUpDown3.Value = numericUpDown3.Maximum = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lastId != -1 && numericUpDown3.Value > 0)
            {
                cart[lastId] = (int) numericUpDown3.Value;
            }
        }

        #endregion

        #region Tab2
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox8.Text, @"\w\w\d{8}"))
            {
                MessageBox.Show("Invalid client identifier.");
                return;
            }

            RegularUserController.SaveOrder(userId, textBox8.Text, cart);
            MessageBox.Show("Success");
            cart.Clear();
            tabControl1.SelectedIndex = 0;
            RefreshTab1Fields();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cart.Clear();
            tabControl1.SelectedIndex = 0;
        }

        private void RefreshTab2Fields()
        {
            listBox2.Items.Clear();
            textBox8.Clear();

            label8.Text = "Total: " + cart.Select(x => RegularUserController.GetMedication(x.Key).Price * x.Value).Sum();

            foreach (var item in cart)
            {
                var medication = RegularUserController.GetMedication(item.Key);

                listBox2.Items.Add($"{medication.Name}  -  x{item.Value}  -  ¤{item.Value * medication.Price}");
            }
        }

        #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                if (cart.Count == 0)
                {
                    tabControl1.SelectedIndex = 0;
                    MessageBox.Show("Please add at least one type of medication first.");
                }
                else
                {
                    RefreshTab2Fields();
                }
            }
        }
    }
}
