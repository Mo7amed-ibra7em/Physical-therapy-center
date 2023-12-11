using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Physiotherapy_Center
{
    public partial class F_Doctor_3 : Form
    {
        #region
        string connstr = "Data Source=M-A-IBRAHEM; Initial Catalog=DB Physical Treatment;Integrated Security = True";
        bool Btnsave = false;
        string[] Data;
        #endregion

        public F_Doctor_3()
        {
            InitializeComponent();
        }
        private string[] DataReport()
        {
            int IDReport = Convert.ToInt32(dgv_report_3.CurrentRow.Cells[0].Value);
            int IDPatient = Convert.ToInt32(dgv_report_3.CurrentRow.Cells[1].Value);
            string Name = dgv_report_3.CurrentRow.Cells[2].Value.ToString();
            string data = dgv_report_3.CurrentRow.Cells[3].Value.ToString();
            string complaint = dgv_report_3.CurrentRow.Cells[4].Value.ToString();
            string Injury_1 = dgv_report_3.CurrentRow.Cells[5].Value.ToString();
            string Injury_2 = dgv_report_3.CurrentRow.Cells[6].Value.ToString();
            string Clinical_story = dgv_report_3.CurrentRow.Cells[7].Value.ToString();
            string surveys = dgv_report_3.CurrentRow.Cells[8].Value.ToString();
            string Clinical_examination = dgv_report_3.CurrentRow.Cells[9].Value.ToString();
            string treatment_plan = dgv_report_3.CurrentRow.Cells[10].Value.ToString();

            string[] DataPatient = new string[] { IDReport.ToString(), IDPatient.ToString(), Name, data, complaint, Injury_1, Injury_2, Clinical_story, surveys, Clinical_examination, treatment_plan };
            return DataPatient;
        }
        private void b_add_report_3_Click(object sender, EventArgs e)
        {
            if (dgv_report_3.DataSource == null || ((DataTable)dgv_report_3.DataSource).Rows.Count == 0)
            {
                NotificationForm notificationForm = new NotificationForm("لا يوجد مرضى");
                notificationForm.ShowDialog();
            }
            else
            {
                string[] Data = DataReport();
                Btnsave = false;
                F_AddReport_5 addReport_5 = new F_AddReport_5(Data, Btnsave);
                addReport_5.ShowDialog();
                Refresh();
            }
        }
        private void Refresh()
        {
            try
            {
                DataTable reportsData = new DataTable();

                using (SqlConnection sqlconn = new SqlConnection(connstr))
                using (SqlCommand sqlcmd = new SqlCommand("GetReports", sqlconn))
                using (SqlDataAdapter sqladap = new SqlDataAdapter(sqlcmd))
                {
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlconn.Open();
                    sqladap.Fill(reportsData);
                }

                dgv_report_3.DataSource = reportsData; // dgv_reports هو اسم DataGridView الذي ستعرض فيه التقارير
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void b_edit_3_Click(object sender, EventArgs e)
        {
            if (dgv_report_3.DataSource == null || ((DataTable)dgv_report_3.DataSource).Rows.Count == 0)
            {
                NotificationForm notificationForm = new NotificationForm("لا يوجد مرضى");
                notificationForm.ShowDialog();
            }
            else
            {
                string[] Data = DataReport();
                Btnsave = true;
                F_AddReport_5 addReport_5 = new F_AddReport_5(Data, Btnsave);
                addReport_5.ShowDialog();
                Refresh();
                t_search_3.Text = "";
            }
        }
        private void F_Doctor_3_Load(object sender, EventArgs e)
        {
            Refresh();
        }
        private void t_search_3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(connstr))
                {
                    sqlconn.Open();

                    using (SqlCommand sqlcmd = new SqlCommand("SearchReports", sqlconn))
                    {
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.Parameters.AddWithValue("@searchText", t_search_3.Text);

                        DataTable reportData = new DataTable();

                        using (SqlDataAdapter sqladap = new SqlDataAdapter(sqlcmd))
                        {
                            sqladap.Fill(reportData);
                        }

                        dgv_report_3.DataSource = reportData;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void b_delete_3_Click(object sender, EventArgs e)
        {
            if (dgv_report_3.DataSource == null || ((DataTable)dgv_report_3.DataSource).Rows.Count == 0)
            {
                NotificationForm notificationForm = new NotificationForm("لا يوجد مرضى");
                notificationForm.ShowDialog();
            }
            else
            {
                using (SqlConnection sqlconn = new SqlConnection(connstr))
                using (SqlCommand sqlcmd = new SqlCommand("DeleteReport", sqlconn))
                {
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@patient_id", dgv_report_3.CurrentRow.Cells[1].Value);
                    sqlcmd.Parameters.AddWithValue("@report_id", dgv_report_3.CurrentRow.Cells[0].Value);

                    // إضافة معامل إخراج للحصول على قيمة القدرة على الحذف
                    SqlParameter canDeleteParameter = new SqlParameter("@can_delete", SqlDbType.Bit);
                    canDeleteParameter.Direction = ParameterDirection.Output;
                    sqlcmd.Parameters.Add(canDeleteParameter);

                    sqlconn.Open();
                    sqlcmd.ExecuteNonQuery();

                    bool canDelete = (bool)canDeleteParameter.Value;

                    if (canDelete)
                    {
                        NotificationForm notificationForm = new NotificationForm("تم حذف التقرير بنجاح.");
                        notificationForm.ShowDialog();
                        Refresh();
                    }
                    else
                    {
                        NotificationForm notificationForm = new NotificationForm("لا يمكن حذف التقرير لأنه مرتبط بجلسة");
                        notificationForm.ShowDialog();
                    }
                }
            }
        }
        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void b_savephoto_3_Click(object sender, EventArgs e)
        {
            int reportID = Convert.ToInt32(dgv_report_3.CurrentRow.Cells[0].Value);
            int patientID = Convert.ToInt32(dgv_report_3.CurrentRow.Cells[1].Value);
            GetPatientData(patientID, reportID);
        }
        private void GetPatientData(int patientID, int reportID)
        {
            using (SqlConnection connection = new SqlConnection(connstr))
            using (SqlCommand cmd = new SqlCommand("GetPatientDataByID", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PatientID", patientID);
                cmd.Parameters.AddWithValue("@reportID", reportID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string ID = reader["PatientID"].ToString();
                            string name = reader["Name"].ToString();
                            string age = reader["Age"].ToString();
                            string phone = reader["Phone"].ToString();
                            string dept = reader["Dept"].ToString();
                            string currentAddress = reader["CurrentAddress"].ToString();
                            string admissionDate = reader["AdmissionDate"].ToString().Substring(0, 10);
                            string dateOfInjury = reader["DateOfInjury"].ToString().Substring(0, 10);
                            string[] dataPatient = new string[]{ID, name, age, phone, dept, currentAddress, admissionDate, dateOfInjury};

                            F_Report_13 f_Report_13 = new F_Report_13(dataPatient,DataReport());
                            f_Report_13.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("لم يتم العثور على بيانات المريض.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
