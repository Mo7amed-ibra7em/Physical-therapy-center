using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physiotherapy_Center
{
    public partial class F_Manager_6 : Form
    {
        public F_Manager_6()
        {
            InitializeComponent();
        }

        private void F_Manager_6_Load(object sender, EventArgs e)
        {
            F_Employee_7 f_Employee_7 = new F_Employee_7();
            f_Employee_7.TopLevel = false;
            pnl_load_6.Controls.Add(f_Employee_7);
            f_Employee_7.Show();
            f_Employee_7.BringToFront();
        }

        private void b_employee_6_Click(object sender, EventArgs e)
        {
            F_Employee_7 f_Employee_7 = new F_Employee_7();
            f_Employee_7.TopLevel = false;
            pnl_load_6.Controls.Add(f_Employee_7);
            f_Employee_7.Show();
            f_Employee_7.BringToFront();
        }

        private void b_Patient_record_6_Click(object sender, EventArgs e)
        {
            F_Patient_Record_10 f_Patient_Record_10 = new F_Patient_Record_10();
            f_Patient_Record_10.TopLevel = false;
            pnl_load_6.Controls.Add(f_Patient_Record_10);
            f_Patient_Record_10.Show();
            f_Patient_Record_10.BringToFront();
        }

        private void b_privacy_6_Click(object sender, EventArgs e)
        {
            F_Privacy_11 f_Privacy_11 = new F_Privacy_11();
            f_Privacy_11.ShowDialog();
        }

        private void b_close_6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void b_about_6_Click(object sender, EventArgs e)
        {
            F_About_12 f_About = new F_About_12();
            f_About.TopLevel = false;
            pnl_load_6.Controls.Add(f_About);
            f_About.Show();
            f_About.BringToFront();
        }
    }
}
