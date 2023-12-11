using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace Physiotherapy_Center
{
    public partial class F_DataEntry_2 : Form
    {
        string connstr = "Data Source=M-A-IBRAHEM; Initial Catalog=DB Physical Treatment;Integrated Security = True";

        int ID;
        public F_DataEntry_2()
        {
            InitializeComponent();
        }

        private void AddSick()
        {
            try
            {
                string Gender = rb_male_2.Checked ? "ذكر" : "أنثى";
                string Dept = ch_men_2.Checked ? "رجال" : "نسائية وأطفال";

                using (SqlConnection sqlconn = new SqlConnection(connstr))
                {
                    sqlconn.Open();

                    using (SqlCommand sqlcmd = new SqlCommand("EXEC [dbo].[InsertPatient] @patient_name, @birthdate, @patient_gender, @patient_phone, @patient_dept, @patient_address, @admission_date, @date_of_injury", sqlconn))
                    {
                        sqlcmd.Parameters.AddWithValue("@patient_name", t_name_sick_2.Text);
                        sqlcmd.Parameters.AddWithValue("@birthdate", dtp_age_2.Value);
                        sqlcmd.Parameters.AddWithValue("@patient_gender", Gender);
                        sqlcmd.Parameters.AddWithValue("@patient_phone", t_phone_Num_2.Text);
                        sqlcmd.Parameters.AddWithValue("@patient_dept", Dept);
                        sqlcmd.Parameters.AddWithValue("@patient_address", t_address_2.Text);
                        sqlcmd.Parameters.AddWithValue("@admission_date", dtp_Acceptance_2.Value);
                        sqlcmd.Parameters.AddWithValue("@date_of_injury", dtp_data_injury_2.Value);

                        sqlcmd.ExecuteNonQuery();

                        NotificationForm notificationForm = new NotificationForm("تم إدخال بيانات المريض بنجاح.");
                        notificationForm.ShowDialog();

                    }
                }

                DataTable patientData = new DataTable();

                using (SqlConnection sqlconn = new SqlConnection(connstr))
                using (SqlCommand sqlcmd1 = new SqlCommand("GetPatientData", sqlconn))
                using (SqlDataAdapter sqladap = new SqlDataAdapter(sqlcmd1))
                {
                    sqlcmd1.CommandType = CommandType.StoredProcedure;
                    sqlconn.Open();
                    sqladap.Fill(patientData);
                }

                dgv_sick_2.DataSource = patientData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            t_address_2.Text = "";
            t_name_sick_2.Text = "";
            t_phone_Num_2.Text = "";
            dtp_Acceptance_2.Value = DateTime.Now;
            dtp_age_2.Value = DateTime.Now;
            dtp_data_injury_2.Value = DateTime.Now;
        }
        private void GetDataPatient()
        {
            try
            {
                DataTable patientData = new DataTable();

                using (SqlConnection sqlconn = new SqlConnection(connstr))
                using (SqlCommand sqlcmd1 = new SqlCommand("GetPatientData", sqlconn))
                using (SqlDataAdapter sqladap = new SqlDataAdapter(sqlcmd1))
                {
                    sqlcmd1.CommandType = CommandType.StoredProcedure; // تحديد نوع الاستعلام إلى إجراء مخزن
                    sqlconn.Open();
                    sqladap.Fill(patientData);
                }

                dgv_sick_2.DataSource = patientData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void F_DataEntry_2_Load(object sender, EventArgs e)
        {
            dtp_Acceptance_2.Value = DateTime.Now;
            dtp_Acceptance_2.Format = DateTimePickerFormat.Custom;
            dtp_Acceptance_2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            GetDataPatient();
        }
        private void b_add_sick_2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(t_name_sick_2.Text) || string.IsNullOrEmpty(t_phone_Num_2.Text) || string.IsNullOrEmpty(t_address_2.Text))
            {
                NotificationForm notificationForm = new NotificationForm("أدخل جميع البيانات");
                notificationForm.ShowDialog();
            }
            else
            {
                AddSick();
            }
        }
        private void b_edit_2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(connstr))
                {
                    sqlconn.Open();

                    if (b_edit_2.Text == "تعــديل")
                    {
                        ID = Convert.ToInt32(dgv_sick_2.CurrentRow.Cells[0].Value);
                        using (SqlCommand sqlcmd1 = new SqlCommand("GetUpdateDataPatient", sqlconn))
                        {
                            sqlcmd1.CommandType = CommandType.StoredProcedure;
                            sqlcmd1.Parameters.AddWithValue("@patientID", ID);

                            using (SqlDataReader reader = sqlcmd1.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    t_name_sick_2.Text = reader.GetString(0);
                                    string Gender = reader.GetString(1);
                                    t_phone_Num_2.Text = reader.GetString(2);
                                    string Dept = reader.GetString(3);
                                    t_address_2.Text = reader.GetString(4);
                                    dtp_Acceptance_2.Value = reader.GetDateTime(5);
                                    dtp_data_injury_2.Value = reader.GetDateTime(6);
                                    dtp_age_2.Value = reader.GetDateTime(7);

                                    rb_male_2.Checked = Gender == "ذكر";
                                    rb_female_2.Checked = Gender == "أنثى";
                                    ch_men_2.Checked = Dept == "رجال";
                                    ch_women_2.Checked = Dept == "نسائية وأطفال";
                                }
                                else
                                {
                                    NotificationForm notificationForm = new NotificationForm("لم يتم العثور على مريض بهذا الرقم");
                                    notificationForm.ShowDialog();
                                }
                            }
                        }

                        b_edit_2.Text = "حفــظ";
                    }
                    else
                    {
                        using (SqlCommand sqlcmd = new SqlCommand("UpdateDataPatient", sqlconn))
                        {
                            sqlcmd.CommandType = CommandType.StoredProcedure;
                            sqlcmd.Parameters.AddWithValue("@patient_id", ID);
                            sqlcmd.Parameters.AddWithValue("@patient_name", t_name_sick_2.Text);
                            sqlcmd.Parameters.AddWithValue("@birthdate", dtp_age_2.Value);

                            string gender = rb_male_2.Checked ? "ذكر" : "أنثى";
                            sqlcmd.Parameters.AddWithValue("@patient_gender", gender);

                            sqlcmd.Parameters.AddWithValue("@patient_phone", t_phone_Num_2.Text);

                            string dept = ch_men_2.Checked ? "رجال" : "نسائية وأطفال";
                            sqlcmd.Parameters.AddWithValue("@patient_dept", dept);

                            sqlcmd.Parameters.AddWithValue("@patient_address", t_address_2.Text);
                            sqlcmd.Parameters.AddWithValue("@admission_date", dtp_Acceptance_2.Value);
                            sqlcmd.Parameters.AddWithValue("@date_of_injury", dtp_data_injury_2.Value);

                            sqlcmd.ExecuteNonQuery();
                        }
                        NotificationForm notificationForm = new NotificationForm("تم التعديل بنجاح.");
                        notificationForm.ShowDialog();
                        //////
                        t_address_2.Text = "";
                        t_name_sick_2.Text = "";
                        t_phone_Num_2.Text = "";
                        dtp_Acceptance_2.Value = DateTime.Now;
                        dtp_age_2.Value = DateTime.Now;
                        dtp_data_injury_2.Value = DateTime.Now;
                        b_edit_2.Text = "تعــديل";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ////تحديث البيانات
            try
            {
                DataTable patientData = new DataTable();

                using (SqlConnection sqlconn = new SqlConnection(connstr))
                using (SqlCommand sqlcmd1 = new SqlCommand("GetPatientData", sqlconn))
                using (SqlDataAdapter sqladap = new SqlDataAdapter(sqlcmd1))
                {
                    sqlcmd1.CommandType = CommandType.StoredProcedure; // تحديد نوع الاستعلام إلى إجراء مخزن
                    sqlconn.Open();
                    sqladap.Fill(patientData);
                }

                dgv_sick_2.DataSource = patientData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void b_close_tool_2_Click(object sender, EventArgs e)
        {
            t_address_2.Text = "";
            t_name_sick_2.Text = "";
            t_phone_Num_2.Text = "";
            dtp_Acceptance_2.Value = DateTime.Now;
            dtp_age_2.Value = DateTime.Now;
            dtp_data_injury_2.Value = DateTime.Now;
            b_edit_2.Text = "تعــديل";
        }
        private void ch_women_2_Click(object sender, EventArgs e)
        {
            ch_men_2.Checked = false;
            ch_women_2.Checked = true;
        }
        private void ch_men_2_Click(object sender, EventArgs e)
        {
            ch_men_2.Checked = true;
            ch_women_2.Checked = false;
        }
        private void b_delete_2_Click(object sender, EventArgs e)
        {
            Delete();
            GetDataPatient();
        }
        private void Delete()
        {
            int ID = Convert.ToInt32(dgv_sick_2.CurrentRow.Cells[0].Value);
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(connstr))
                using (SqlCommand sqlcmd = new SqlCommand("DeletePatient", sqlconn))
                {
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@patientID", ID );

                    // إضافة معامل إخراج للتحقق مما إذا كان بإمكان حذف المريض أم لا
                    SqlParameter canDeleteParameter = new SqlParameter("@can_delete", SqlDbType.Bit);
                    canDeleteParameter.Direction = ParameterDirection.Output;
                    sqlcmd.Parameters.Add(canDeleteParameter);

                    sqlconn.Open();
                    sqlcmd.ExecuteNonQuery();

                    bool canDelete = (bool)canDeleteParameter.Value;

                    if (canDelete)
                    {
                        NotificationForm notificationForm = new NotificationForm("تم حذف المريض بنجاح");
                        notificationForm.ShowDialog();
                    }
                    else
                    {
                        NotificationForm notificationForm = new NotificationForm("لا يمكن حذف المريض لأنه مرتبط بتقرير");
                        notificationForm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void t_search_2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable searchResults = new DataTable();

                using (SqlConnection sqlconn = new SqlConnection(connstr))
                using (SqlCommand sqlcmd = new SqlCommand("SearchPatientData", sqlconn))
                using (SqlDataAdapter sqladap = new SqlDataAdapter(sqlcmd))
                {
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@searchText", t_search_2.Text);
                    sqlconn.Open();
                    sqladap.Fill(searchResults);
                }
                dgv_sick_2.DataSource = searchResults;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void t_name_sick_2_KeyPress(object sender, KeyPressEventArgs e)
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
        private void t_phone_Num_2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)Keys.Back)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void close_2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}