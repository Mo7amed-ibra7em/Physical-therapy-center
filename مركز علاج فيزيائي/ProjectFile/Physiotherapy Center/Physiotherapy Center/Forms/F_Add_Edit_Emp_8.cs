using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physiotherapy_Center
{
    public partial class F_Add_Edit_Emp_8 : Form
    {
        #region
        string connstr = "Data Source=M-A-IBRAHEM; Initial Catalog=DB Physical Treatment;Integrated Security = True";
        string[] Data;
        bool Btnsave;
        string Gender;
        #endregion
        public F_Add_Edit_Emp_8(bool Btnsave,string[] Data)
        {
            InitializeComponent();
            this.Btnsave = Btnsave;
            this.Data = Data;
        }
        private void AddEmp()
        {
            string Gender = rb_male_8.Checked ? "ذكر" : "أنثى";
            using (SqlConnection sqlconn = new SqlConnection(connstr))
            using (SqlCommand sqlcmd = new SqlCommand("InsertEmployee", sqlconn) { CommandType = CommandType.StoredProcedure })
            {
                sqlcmd.Parameters.AddWithValue("@job_number", cb_jop_8.SelectedIndex);
                sqlcmd.Parameters.AddWithValue("@name", t_name_emp_8.Text);
                sqlcmd.Parameters.AddWithValue("@birthdate", dtp_age_8.Value);
                sqlcmd.Parameters.AddWithValue("@gender", Gender);
                sqlcmd.Parameters.AddWithValue("@phone", t_phone_num_8.Text);
                sqlcmd.Parameters.AddWithValue("@email", t_email_8.Text);
                sqlcmd.Parameters.AddWithValue("@username", t_username_8.Text);
                sqlcmd.Parameters.AddWithValue("@password", t_password_8.Text);

                sqlconn.Open();
                sqlcmd.ExecuteNonQuery();

                NotificationForm notificationForm = new NotificationForm("تم الاضافة بنجاح");
                notificationForm.ShowDialog();
            }
        }
        private void UpdateEmp()
        {
            string Gender = rb_male_8.Checked ? "ذكر" : "أنثى";
            using (SqlConnection sqlconn = new SqlConnection(connstr))
            using (SqlCommand sqlcmd = new SqlCommand("UpdateEmployee", sqlconn))
            {
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@employee_id", int.Parse(Data[0]));
                sqlcmd.Parameters.AddWithValue("@job_number", cb_jop_8.SelectedIndex);
                sqlcmd.Parameters.AddWithValue("@name", t_name_emp_8.Text);
                sqlcmd.Parameters.AddWithValue("@birthdate", dtp_age_8.Value);
                sqlcmd.Parameters.AddWithValue("@gender", Gender);
                sqlcmd.Parameters.AddWithValue("@phone", t_phone_num_8.Text);
                sqlcmd.Parameters.AddWithValue("@email", t_email_8.Text);
                sqlcmd.Parameters.AddWithValue("@username", t_username_8.Text);
                sqlcmd.Parameters.AddWithValue("@password", t_password_8.Text);

                sqlconn.Open();
                sqlcmd.ExecuteNonQuery();

                NotificationForm notificationForm = new NotificationForm("تم تحديث البيانات بنجاح");
            }
        }
        private void b_add_emp_8_Click(object sender, EventArgs e)
        {
            bool Test = TestInput();
            if (Btnsave == false)
            {
                if (Test)
                {
                    AddEmp();
                    b_add_emp_8.Text = "إضافة";
                }
            }
            else
            {
                if (Test)
                {
                    UpdateEmp();
                    b_add_emp_8.Text = "تعـديل";
                }
            }
            this.Close();
        }
        private bool TestInput()
        {
            if (string.IsNullOrEmpty(t_name_emp_8.Text) || string.IsNullOrEmpty(t_email_8.Text) || string.IsNullOrEmpty(t_phone_num_8.Text) || string.IsNullOrEmpty(t_username_8.Text) || string.IsNullOrEmpty(t_password_8.Text))
            {
                NotificationForm notificationForm = new NotificationForm("أدخل جميع البيانات");
                notificationForm.ShowDialog();
                return false;
            }
            else
            {
                return true;
            }
        }
        private void F_Add_Edit_Emp_8_Load(object sender, EventArgs e)
        {
            if (Btnsave == false)
            {
                b_add_emp_8.Text = "إضافة";
            }
            else
            {
                t_name_emp_8.Text = Data[1];
                cb_jop_8.SelectedItem = Data[2];
                DateTime currentDate = DateTime.Now; // التاريخ الحالي
                dtp_age_8.Value = currentDate.AddYears(-Convert.ToInt32(Data[3]));
                if (Data[4] == rb_male_8.Text)
                {
                    rb_male_8.Checked = true;
                }
                else
                {
                    rb_female_8.Checked = true;
                }
                t_email_8.Text = Data[5];
                t_name_emp_8.Text = Data[1];
                t_phone_num_8.Text = Data[6];
                t_username_8.Text = Data[7];
                t_password_8.Text = Data[8];
                b_add_emp_8.Text = "تعـديل";
            }
        }
        private void t_name_emp_8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back)
            {
                return;
            }
            if (!Regex.IsMatch(e.KeyChar.ToString(), @"^[\p{L}\p{M}\s]+$"))
            {
                e.Handled = true;
            }
        }
        private void t_phone_num_8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)Keys.Back)
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}
