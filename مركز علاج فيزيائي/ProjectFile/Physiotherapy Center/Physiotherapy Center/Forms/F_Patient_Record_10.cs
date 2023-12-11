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
    public partial class F_Patient_Record_10 : Form
    {
        string connstr = "Data Source=M-A-IBRAHEM; Initial Catalog=DB Physical Treatment;Integrated Security = True";

        public F_Patient_Record_10()
        {
            InitializeComponent();
        }

        private void F_Patient_Record_10_Load(object sender, EventArgs e)
        {
            using (SqlConnection sqlconn = new SqlConnection(connstr))
            using (SqlCommand sqlcmd = new SqlCommand("GetPatientInformation_Manager", sqlconn))
            {
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlconn.Open();

                using (SqlDataReader reader = sqlcmd.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dgv_report_10.DataSource = dataTable;
                }
            }
        }
    }
}
