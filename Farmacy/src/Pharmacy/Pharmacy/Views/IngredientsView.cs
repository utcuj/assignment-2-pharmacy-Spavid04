using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Pharmacy.Views
{
    public partial class IngredientsView : Form
    {
        public string ingredients;

        public IngredientsView(string ingredients)
        {
            InitializeComponent();
            this.Closing += IngredientsView_Closing;

            textBox1.Text = String.Join(Environment.NewLine,
                ingredients.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        }

        private void IngredientsView_Closing(object sender, CancelEventArgs e)
        {
            ingredients = textBox1.Text.Replace(Environment.NewLine, ",");
        }
    }
}
