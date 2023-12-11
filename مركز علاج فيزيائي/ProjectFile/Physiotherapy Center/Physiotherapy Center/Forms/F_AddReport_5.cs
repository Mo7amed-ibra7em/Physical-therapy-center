using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physiotherapy_Center
{
    public partial class F_AddReport_5 : Form
    {
        #region
        string connstr = "Data Source=M-A-IBRAHEM; Initial Catalog=DB Physical Treatment;Integrated Security = True";
        string[] TopDept = { "عطف كتف", "بسط كتف", "تبعيد كتف", "تقريب كتف", "عطف أصابع", "بسط أصابع", "دوران داخلي للكتف", "دوران خارجي للكتف", "عطف مرفق", "بسط مرفق", "عطف رسغ", "بسط رسغ", "استلقاء الساعد", "كب الساعد", "باسطات الجذع" };
        string[] ButtomDept = { "عطف ورك", "بسط ورك", "تبعيد ورك", "تقريب ورك", "عطف أصابع", "بسط أصابع", "دوران داخلي للورك", "دوران خارجي للورك", "عطف ركبة", "بسط ركبة", "عطف ظهري للقدم", "بسط أخمصي للقدم" };
        string [] data;
        bool Btnsave = false;
        #endregion
        public F_AddReport_5(string [] Data, bool Btnsave)
        {
            InitializeComponent();
            this.Btnsave = Btnsave;
            this.data = Data;
        }
        private void cb_type_Injury_1_5_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_type_Injury_2_5.Items.Clear();
            if (cb_type_Injury_1_5.SelectedIndex == 0 || cb_type_Injury_1_5.SelectedIndex == 1)
            {
                cb_type_Injury_2_5.Items.AddRange(TopDept);
            }
            else
            {
                cb_type_Injury_2_5.Items.AddRange(ButtomDept);
            }

        }
        private void F_AddReport_5_Load(object sender, EventArgs e)
        {
            cb_type_Injury_2_5.Items.AddRange(TopDept);
            t_name_sick_5.Text = data[2];
            t_data_5.Text = data[3].Substring(0,10);
            if (Btnsave == true)
            {
                b_add_report_3.Text = "تعـديل";
                rtb_main_complaint_5.Text = data[4];
                cb_type_Injury_1_5.SelectedItem = data[5];
                cb_type_Injury_2_5.SelectedItem = data[6];
                rtb_Clinical_story_5.Text = data[7];
                rtb_surveys_5.Text = data[8];
                rtb_Clinical_examination_.Text = data[9];
                rtb_treatment_plan_.Text = data[10];
            }
            if(Btnsave == false)
            {
                b_add_report_3.Text = "إضافة";
                cb_type_Injury_2_5.SelectedIndex = 0;
            }
        }
        private void AddReport()
        {
            if (Btnsave == true)
            {
                try
                {
                    using (SqlConnection sqlconn = new SqlConnection(connstr))
                    using (SqlCommand sqlcmd = new SqlCommand("UpdateReport", sqlconn))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;

                        // تعبئة المعاملات
                        sqlcmd.Parameters.AddWithValue("@report_id", data[0]);
                        sqlcmd.Parameters.AddWithValue("@patient_id", data[1]);
                        sqlcmd.Parameters.AddWithValue("@main_complaint", rtb_main_complaint_5.Text);
                        sqlcmd.Parameters.AddWithValue("@side_of_injury", cb_type_Injury_1_5.SelectedItem);
                        sqlcmd.Parameters.AddWithValue("@type_of_injury", cb_type_Injury_2_5.SelectedItem);
                        sqlcmd.Parameters.AddWithValue("@clinical_story", rtb_Clinical_story_5.Text);
                        sqlcmd.Parameters.AddWithValue("@surveys", rtb_surveys_5.Text);
                        sqlcmd.Parameters.AddWithValue("@clinical_examination", rtb_Clinical_examination_.Text);
                        sqlcmd.Parameters.AddWithValue("@treatment_plan", rtb_treatment_plan_.Text);

                        sqlconn.Open();
                        sqlcmd.ExecuteNonQuery();
                        NotificationForm notificationForm = new NotificationForm("تم تعديل التقرير بنجاح");
                        notificationForm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            if (Btnsave == false)
            {
                try
                {
                    using (SqlConnection sqlconn = new SqlConnection(connstr))
                    using (SqlCommand sqlcmd = new SqlCommand("InsertReport", sqlconn))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;

                        // تعبئة المعاملات
                        sqlcmd.Parameters.AddWithValue("@patient_id", data[1]);
                        sqlcmd.Parameters.AddWithValue("@main_complaint", rtb_main_complaint_5.Text);
                        sqlcmd.Parameters.AddWithValue("@side_of_injury", cb_type_Injury_1_5.SelectedItem);
                        sqlcmd.Parameters.AddWithValue("@type_of_injury", cb_type_Injury_2_5.SelectedItem);
                        sqlcmd.Parameters.AddWithValue("@clinical_story", rtb_Clinical_story_5.Text);
                        sqlcmd.Parameters.AddWithValue("@surveys", rtb_surveys_5.Text);
                        sqlcmd.Parameters.AddWithValue("@clinical_examination", rtb_Clinical_examination_.Text);
                        sqlcmd.Parameters.AddWithValue("@treatment_plan", rtb_treatment_plan_.Text);

                        sqlconn.Open();
                        sqlcmd.ExecuteNonQuery();

                        NotificationForm notificationForm = new NotificationForm("تم إضافة التقرير بنجاح");
                        notificationForm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void b_add_report_3_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(rtb_main_complaint_5.Text) || string.IsNullOrEmpty(rtb_Clinical_story_5.Text) || string.IsNullOrEmpty(rtb_Clinical_examination_.Text) || string.IsNullOrEmpty(rtb_surveys_5.Text) || string.IsNullOrEmpty(rtb_treatment_plan_.Text) || string.IsNullOrEmpty(cb_type_Injury_2_5.Text))
            {
                NotificationForm notificationForm = new NotificationForm("أدخل جميع البيانات");
                notificationForm.ShowDialog();
            }
            else
            {
                AddReport();
                this.Close();
            }
        }
    }
}
