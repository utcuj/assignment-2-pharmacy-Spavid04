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
using Pharmacy.Models;

namespace Pharmacy.Views
{
    public partial class LoginView : Form
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (LoginController.TryLogin(textBox1.Text, textBox2.Text))
            {
                var userData = LoginController.GetUserLevelAndId(textBox1.Text);

                Form f = (userData.Item2 == UserType.Chemist
                    ? (Form)new RegularUserView(userData.Item1)
                    : (Form)new AdmisitratorView(userData.Item1));

                this.Visible = false;
                f.ShowDialog();
                Environment.Exit(0);
            }
            else
            {
                MessageBox.Show("Invalid credentials!");
            }
        }
    }
}
