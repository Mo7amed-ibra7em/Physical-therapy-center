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
    public partial class F_Therapist_4 : Form
    {
        string connstr = "Data Source=M-A-IBRAHEM; Initial Catalog=DB Physical Treatment;Integrated Security = True";

        public F_Therapist_4()
        {
            InitializeComponent();
        }

        private void F_Therapist_4_Load(object sender, EventArgs e)
        {
            RefreshData();
        }
        private void AddSession()
        {
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(connstr))
                using (SqlCommand sqlcmd = new SqlCommand("InsertSession", sqlconn))
                {
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@report_id", dgv_session_4.CurrentRow.Cells[0].Value);
                    sqlcmd.Parameters.AddWithValue("@patient_id", dgv_session_4.CurrentRow.Cells[1].Value);
                    sqlcmd.Parameters.AddWithValue("@session_date", dtp_data_session_4.Value);
                    sqlcmd.Parameters.AddWithValue("@Session_details", rtb_session_details_4.Text);
                    sqlconn.Open();
                    sqlcmd.ExecuteNonQuery();

                    NotificationForm notificationForm = new NotificationForm("تم إضافة جلسة جديدة بنجاح");
                    notificationForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void b_add_session_4_Click(object sender, EventArgs e)
        {
            if (dgv_session_4.DataSource == null || ((DataTable)dgv_session_4.DataSource).Rows.Count == 0)
            {
                NotificationForm notificationForm = new NotificationForm("لا يوجد مرضى");
                notificationForm.ShowDialog();
            }
            else
            {
                if (string.IsNullOrEmpty(rtb_session_details_4.Text))
                {
                    NotificationForm notificationForm = new NotificationForm("أضف تفاصيل الجلسة");
                    notificationForm.ShowDialog();
                }
                else
                {
                    AddSession();
                    rtb_session_details_4.Text = "";
                }
                RefreshData();
            }
        }
        private void RefreshData()
        {
            try
            {
                DataTable SessionsData = new DataTable();

                using (SqlConnection sqlconn = new SqlConnection(connstr))
                using (SqlCommand sqlcmd = new SqlCommand("GetSessions", sqlconn))
                using (SqlDataAdapter sqladap = new SqlDataAdapter(sqlcmd))
                {
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlconn.Open();
                    sqladap.Fill(SessionsData);
                }

                dgv_session_4.DataSource = SessionsData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void t_search_4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(connstr))
                using (SqlCommand sqlcmd = new SqlCommand("SearchSessions", sqlconn))
                {
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@searchText", t_search_4.Text);

                    sqlconn.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlcmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgv_session_4.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void b_edit_4_Click(object sender, EventArgs e)
        {
            if (dgv_session_4.DataSource == null || ((DataTable)dgv_session_4.DataSource).Rows.Count == 0)
            {
                NotificationForm notificationForm = new NotificationForm("لا يوجد مرضى");
                notificationForm.ShowDialog();
            }
            else
            {
                Update();
            }
        }
        private void Update()
        {
            int ReportID = Convert.ToInt32(dgv_session_4.CurrentRow.Cells[0].Value);
            int PatientID = Convert.ToInt32(dgv_session_4.CurrentRow.Cells[1].Value);
            int SessionID = Convert.ToInt32(dgv_session_4.CurrentRow.Cells[6].Value);
            if (b_edit_4.Text == "تعــديل")
            {
                if (Convert.ToString(dgv_session_4.CurrentRow.Cells[7].Value) != "" && Convert.ToString(dgv_session_4.CurrentRow.Cells[8].Value) != "")
                {
                    rtb_session_details_4.Text = dgv_session_4.CurrentRow.Cells[7].Value.ToString();
                    dtp_data_session_4.Value = Convert.ToDateTime(dgv_session_4.CurrentRow.Cells[8].Value.ToString());
                }
                b_edit_4.Text = "حفـظ";
            }
            else
            {
                if (rtb_session_details_4.Text == "")
                {
                    NotificationForm notificationForm = new NotificationForm("أدخل تفاصيل الجلسة ");
                    notificationForm.ShowDialog();
                }
                else
                {
                    try
                    {
                        using (SqlConnection sqlconn = new SqlConnection(connstr))
                        using (SqlCommand sqlcmd = new SqlCommand("UpdateSession", sqlconn))
                        {
                            sqlcmd.CommandType = CommandType.StoredProcedure;
                            sqlcmd.Parameters.AddWithValue("@Report_id", ReportID);
                            sqlcmd.Parameters.AddWithValue("@patient_id", PatientID);
                            sqlcmd.Parameters.AddWithValue("@session_id", SessionID);
                            sqlcmd.Parameters.AddWithValue("@session_date", dtp_data_session_4.Value);
                            sqlcmd.Parameters.AddWithValue("@session_details", rtb_session_details_4.Text);

                            sqlconn.Open();
                            sqlcmd.ExecuteNonQuery();

                            NotificationForm notificationForm = new NotificationForm("تم التعديل بنجاح ");
                            notificationForm.ShowDialog();
                            RefreshData();
                            b_edit_4.Text = "تعــديل";
                            dtp_data_session_4.Value = DateTime.Now;
                            rtb_session_details_4.Text = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void b_delete_4_Click(object sender, EventArgs e)
        {
            if (dgv_session_4.DataSource == null || ((DataTable)dgv_session_4.DataSource).Rows.Count == 0)
            {
                NotificationForm notificationForm = new NotificationForm("لا يوجد مرضى");
                notificationForm.ShowDialog();
            }
            else
            {
                DeleteSession();
                RefreshData();
            }
        }
        private void DeleteSession()
        {
            using (SqlConnection connection = new SqlConnection(connstr))
            {
                using (SqlCommand command = new SqlCommand("DeleteSession", connection))
                {
                    connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@report_id", dgv_session_4.CurrentRow.Cells[0].Value);
                    command.Parameters.AddWithValue("@patient_id", dgv_session_4.CurrentRow.Cells[1].Value);
                    command.Parameters.AddWithValue("@session_id", dgv_session_4.CurrentRow.Cells[6].Value);

                    command.ExecuteNonQuery();
                    NotificationForm notificationForm = new NotificationForm("تم حذف الجلسة بنجاح");
                    notificationForm.ShowDialog();
                }
            }
        }
    }
}
