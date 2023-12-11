using System;
using System.Drawing;
using System.Windows.Forms;

namespace Physiotherapy_Center
{
    public partial class NotificationForm : Form
    {
        public NotificationForm(string message)
        {
            InitializeUI(message);
        }

        private void InitializeUI(string message)
        {
            this.Text = "إشعار";
            this.FormBorderStyle = FormBorderStyle.None; // تم تغيير هنا
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(329, 124); // تم تغيير هنا

            Label messageLabel = new Label();
            messageLabel.Text = message;
            messageLabel.TextAlign = ContentAlignment.MiddleCenter;
            messageLabel.Font = new Font("Arial", 14, FontStyle.Regular);
            messageLabel.Dock = DockStyle.Fill;

            Button okButton = new Button();
            okButton.Text = "موافق";
            okButton.Font = new Font("Arial", 12, FontStyle.Bold);
            okButton.BackColor = Color.FromArgb(64, 104, 130);
            okButton.ForeColor = Color.White;
            okButton.FlatStyle = FlatStyle.Flat;
            okButton.FlatAppearance.BorderSize = 0;
            okButton.Dock = DockStyle.Bottom;
            okButton.Height = 40;
            okButton.Click += OkButton_Click;

            this.Controls.Add(messageLabel);
            this.Controls.Add(okButton);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NotificationForm
            // 
            this.ClientSize = new System.Drawing.Size(329, 124);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NotificationForm";
            this.ResumeLayout(false);

        }
    }
}
