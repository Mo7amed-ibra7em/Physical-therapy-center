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
using System.Xml.Linq;

namespace Physiotherapy_Center
{
    public partial class F_Privacy_11 : Form
    {
        string connstr = "Data Source=M-A-IBRAHEM; Initial Catalog=DB Physical Treatment;Integrated Security = True";

        public F_Privacy_11()
        {
            InitializeComponent();
        }

        private void F_Privacy_11_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connstr))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("GetManagerData", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            t_name_manager_11.Text = reader["Name"].ToString();
                            dtp_age_11.Value = Convert.ToDateTime(reader["age"]);
                            string Gender = reader["Gender"].ToString();
                            rb_male_11.Checked = Gender == "ذكر";
                            rb_female_11.Checked = Gender == "أنثى";
                            t_phone_num_11.Text = reader["Phone"].ToString();
                            t_username_11.Text = reader["UserName"].ToString();
                            t_password_11.Text = reader["Password"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void b_edit_11_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(t_name_manager_11.Text) || string.IsNullOrEmpty(t_phone_num_11.Text) || string.IsNullOrEmpty(t_username_11.Text) || string.IsNullOrEmpty(t_password_11.Text))
            {
                NotificationForm notificationForm = new NotificationForm("أدخل جميع بياناتك");
                notificationForm.ShowDialog();
            }
            else
            {
                UpdateData();
            }
            this.Close();
        }
        private void UpdateData()
        {
            using (SqlConnection connection = new SqlConnection(connstr))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UpdateManagerData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@managerID", 1);
                    command.Parameters.AddWithValue("@name", t_name_manager_11.Text);
                    command.Parameters.AddWithValue("@birthdate", dtp_age_11.Value);
                    string gender;
                    if (rb_male_11.Checked)
                    {
                        gender = rb_male_11.Text;
                    }
                    else
                    {
                        gender = rb_female_11.Text;
                    }
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@phone", t_phone_num_11.Text);
                    command.Parameters.AddWithValue("@userName", t_username_11.Text);
                    command.Parameters.AddWithValue("@password", t_password_11.Text);

                    command.ExecuteNonQuery();
                    NotificationForm notificationForm = new NotificationForm("تم التعديل بنجاح");
                    notificationForm.ShowDialog();
                }
                connection.Close();
            }
        }
        private void t_name_manager_11_KeyPress(object sender, KeyPressEventArgs e)
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
        private void t_phone_num_11_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)Keys.Back)
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}
