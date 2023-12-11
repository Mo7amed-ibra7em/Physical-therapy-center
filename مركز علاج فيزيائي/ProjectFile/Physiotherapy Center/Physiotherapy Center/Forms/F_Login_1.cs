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
    public partial class F_Login_1 : Form
    {
        string connstr = "Data Source=M-A-IBRAHEM; Initial Catalog=DB Physical Treatment;Integrated Security = True";

        public F_Login_1()
        {
            InitializeComponent();
        }
        private void F_Login_Load(object sender, EventArgs e)
        {

        }
        private int TestLogin()
        {
            int loginID = 0;
            using (SqlConnection connection = new SqlConnection(connstr))
            {
                using (SqlCommand command = new SqlCommand("TestLogin", connection))
                {
                    connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@userName", t_username_1.Text);
                    command.Parameters.AddWithValue("@password", t_password_1.Text);

                    SqlParameter IDParam = new SqlParameter("@IDJOP", SqlDbType.Int);
                    IDParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(IDParam);

                    command.ExecuteNonQuery();

                    if (!Convert.IsDBNull(IDParam.Value))
                    {
                        loginID = Convert.ToInt32(IDParam.Value);
                    }
                    else
                    {
                        loginID = 10000;
                    }
                }
            }
            return loginID;
        }
        private void b_Login_1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(t_password_1.Text) || string.IsNullOrEmpty(t_username_1.Text))
            {
                l_test_login_1.Text = "!! تحقق من بياناتك";
            }
            else
            {
                int Idemp = TestLogin();
                if (Idemp == 0)
                {
                    F_Manager_6 f_Manager_6 = new F_Manager_6();
                    f_Manager_6.Show();
                }
                else if (Idemp == 1)
                {
                    F_Doctor_3 f_Doctor_3 = new F_Doctor_3();
                    f_Doctor_3.Show();
                }
                else if (Idemp == 2)
                {
                    F_Therapist_4 f_Therapist_4 = new F_Therapist_4();
                    f_Therapist_4.Show();
                }
                else if(Idemp == 3)
                {
                    F_DataEntry_2 f_DataEntry_2 = new F_DataEntry_2();
                    f_DataEntry_2.Show();
                }
                else
                {
                    l_test_login_1.Text = "!! تحقق من بياناتك";
                }
            }
        }
    }
}
