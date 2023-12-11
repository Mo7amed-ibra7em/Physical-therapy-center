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
    public partial class F_Employee_7 : Form
    {
        #region
        string connstr = "Data Source=M-A-IBRAHEM; Initial Catalog=DB Physical Treatment;Integrated Security = True";
        bool Btnsave = false;
        string[] DataEmp;
        #endregion
        public F_Employee_7()
        {
            InitializeComponent();
        }

        private string[] DataEmployee()
        {
            int EmployeeID = Convert.ToInt32(dgv_employee_7.CurrentRow.Cells[0].Value);
            string NameEmp = dgv_employee_7.CurrentRow.Cells[1].Value.ToString();
            string Jop = dgv_employee_7.CurrentRow.Cells[2].Value.ToString();
            string Age = dgv_employee_7.CurrentRow.Cells[3].Value.ToString();
            string Gender = dgv_employee_7.CurrentRow.Cells[4].Value.ToString();
            string Email = dgv_employee_7.CurrentRow.Cells[6].Value.ToString();
            string PhoneNumber = dgv_employee_7.CurrentRow.Cells[5].Value.ToString();
            string Username = dgv_employee_7.CurrentRow.Cells[7].Value.ToString();
            string Password = dgv_employee_7.CurrentRow.Cells[8].Value.ToString();

            string[] DataEmployee = new string[] { EmployeeID.ToString(), NameEmp, Jop, Age, Gender, Email, PhoneNumber, Username, Password };
            return DataEmployee;
        }
        public void GetDataEmp()
        {
            using (SqlConnection sqlconn = new SqlConnection(connstr))
            {
                sqlconn.Open();

                using (SqlCommand sqlcmd = new SqlCommand("GetDataEmployee", sqlconn))
                {
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dgv_employee_7.DataSource = dataTable;
                }
            }
        }
        private void b_add_emp_7_Click(object sender, EventArgs e)
        {
            Btnsave = false;
            F_Add_Edit_Emp_8 f_Add_Edit_Emp_8 = new F_Add_Edit_Emp_8( Btnsave,DataEmp);
            f_Add_Edit_Emp_8.ShowDialog();
            GetDataEmp();
        }
        private void t_salary_7_Click(object sender, EventArgs e)
        {
            F_Edit_Salary_9 f_Edit_Salary_9 = new F_Edit_Salary_9();
            f_Edit_Salary_9.ShowDialog();
        }
        private void b_edit_7_Click(object sender, EventArgs e)
        {
            if (dgv_employee_7.DataSource == null || ((DataTable)dgv_employee_7.DataSource).Rows.Count == 0)
            {
                NotificationForm notificationForm = new NotificationForm("لا يوجد موظفين");
                notificationForm.ShowDialog();
            }
            else
            {
                DataEmp = DataEmployee();
                Btnsave = true;
                F_Add_Edit_Emp_8 f_Add_Edit_Emp_8 = new F_Add_Edit_Emp_8(Btnsave, DataEmp);
                f_Add_Edit_Emp_8.ShowDialog();
            }
            GetDataEmp();
        }
        private void F_Employee_7_Load(object sender, EventArgs e)
        {
            GetDataEmp();
        }
        private void b_delete_7_Click(object sender, EventArgs e)
        {
            if (dgv_employee_7.DataSource == null || ((DataTable)dgv_employee_7.DataSource).Rows.Count == 0)
            {
                NotificationForm notificationForm = new NotificationForm("لا يوجد موظفين");
                notificationForm.ShowDialog();
            }
            else
            {
                DeleteEmp();
                GetDataEmp();
            }
        }
        private void DeleteEmp()
        {
            using (SqlConnection connection = new SqlConnection(connstr))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DeleteEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@employee_id", dgv_employee_7.CurrentRow.Cells[0].Value);
                    command.ExecuteNonQuery();
                    NotificationForm notificationForm = new NotificationForm("تم الحذف بنجاح");
                    notificationForm.ShowDialog();
                }
            }
        }
        private void t_search_7_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connstr))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SearchEmployee", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@searchText", t_search_7.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dgv_employee_7.DataSource = dataTable;
            }
        }
    }
}
