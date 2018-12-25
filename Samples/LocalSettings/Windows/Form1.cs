using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample.LocalSettings.Windows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.checkBox1.Checked = Settings.CheckBox;
            this.dateTimePicker1.Value = Settings.DateTimePicker;
            this.textBox1.Text = Settings.Text;
            this.numericUpDown1.Value = Settings.Number;
            this.textBox2.Text = Settings.Password; 
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Settings.CheckBox = this.checkBox1.Checked;
            Settings.DateTimePicker = this.dateTimePicker1.Value;
            Settings.Text = this.textBox1.Text;
            Settings.Number = this.numericUpDown1.Value;
            Settings.Password = this.textBox2.Text;

            base.OnFormClosing(e);
        }
    }
}
