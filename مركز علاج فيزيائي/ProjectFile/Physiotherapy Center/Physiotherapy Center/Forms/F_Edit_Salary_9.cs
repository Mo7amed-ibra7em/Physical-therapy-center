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
    public partial class F_Edit_Salary_9 : Form
    {
        string connstr = "Data Source=M-A-IBRAHEM; Initial Catalog=DB Physical Treatment;Integrated Security = True";

        public F_Edit_Salary_9()
        {
            InitializeComponent();
        }
        private void GetSalary()
        {
            using (SqlConnection sqlconn = new SqlConnection(connstr))
            using (SqlCommand sqlcmd = new SqlCommand("GetSalary", sqlconn))
            {
                sqlcmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] outputParams = 
                    {
                        new SqlParameter("@doctor_salary", SqlDbType.Float) { Direction = ParameterDirection.Output },
                        new SqlParameter("@therapist_salary", SqlDbType.Float) { Direction = ParameterDirection.Output },
                        new SqlParameter("@data_entry_salary", SqlDbType.Float) { Direction = ParameterDirection.Output }
                    };

                sqlcmd.Parameters.AddRange(outputParams);

                sqlconn.Open();
                sqlcmd.ExecuteNonQuery();

                t_doctor_sal_9.Text = Convert.ToSingle(sqlcmd.Parameters["@doctor_salary"].Value).ToString();
                t_therapist_sal_9.Text = Convert.ToSingle(sqlcmd.Parameters["@therapist_salary"].Value).ToString();
                t_data_entry_sal_9.Text = Convert.ToSingle(sqlcmd.Parameters["@data_entry_salary"].Value).ToString();
            }
        }
        private void F_Edit_Salary_9_Load(object sender, EventArgs e)
        {
            GetSalary();
        }
        private void b_edit_9_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlconn = new SqlConnection(connstr))
            {
                sqlconn.Open();

                // Update Doctor Salary
                using (SqlCommand sqlcmd = new SqlCommand("UpdateSalary", sqlconn))
                {
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@salary_id", 1);
                    sqlcmd.Parameters.AddWithValue("@salary_amount", float.Parse(t_doctor_sal_9.Text));

                    sqlcmd.ExecuteNonQuery();
                }

                // Update Therapist Salary
                using (SqlCommand sqlcmd = new SqlCommand("UpdateSalary", sqlconn))
                {
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@salary_id", 2);
                    sqlcmd.Parameters.AddWithValue("@salary_amount", float.Parse(t_therapist_sal_9.Text));

                    sqlcmd.ExecuteNonQuery();
                }

                // Update Data Entry Salary
                using (SqlCommand sqlcmd = new SqlCommand("UpdateSalary", sqlconn))
                {
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@salary_id", 3);
                    sqlcmd.Parameters.AddWithValue("@salary_amount", float.Parse(t_data_entry_sal_9.Text));

                    sqlcmd.ExecuteNonQuery();
                }

                NotificationForm notificationForm = new NotificationForm("تم تحديث البيانات بنجاح.");
                notificationForm.ShowDialog();
            }
            GetSalary();
            this.Close();
        }
        private void t_doctor_sal_9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)Keys.Back)
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}
