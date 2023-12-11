using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physiotherapy_Center
{
    public partial class F_Report_13 : Form
    {
        string[] DataPatient;
        string[] DataReport;
        public F_Report_13(string[] DataPatient, string[] DataReport)
        {
            InitializeComponent();
            this.DataPatient = DataPatient;
            this.DataReport = DataReport;
        }
        private void F_Report_13_Load(object sender, EventArgs e)
        {
            //{ Id, name, age, phone, dept, address, Acceptance, data_injury }
            t_number.Text = DataPatient[0];
            t_fullname.Text= DataPatient[1];
            t_age.Text= DataPatient[2];
            t_phone.Text= DataPatient[3];
            t_dept.Text= DataPatient[4];
            t_address.Text= DataPatient[5];
            t_Acceptance.Text= DataPatient[6];
            t_data_injury.Text= DataPatient[7];
            //{ IDReport.ToString(), IDPatient.ToString(), Name, data, complaint, Injury_1, Injury_2, Clinical_story, surveys, Clinical_examination, treatment_plan }
            rtb_main_complaint.Text= DataReport[4];
            t_SideOfTheInjury.Text = DataReport[5];
            t_TypeOfInjury.Text = DataReport[6];
            rtb_Clinical_story.Text = DataReport[7];
            rtb_surveys.Text = DataReport[8];
            rtb_Clinical_examination.Text = DataReport[9];
            rtb_treatment_plan.Text = DataReport[10];
        }
        private void F_Report_13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // التحقق ما إذا كان المفتاح المضغوط هو Enter
            {
                ScreenShot screenshot = new ScreenShot(t_fullname.Text);
                screenshot.CaptureScreen();
                this.Close();
            }
            else if(e.KeyCode== Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
