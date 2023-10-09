using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Forum_Website
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=mysql");
        MySqlCommand cmd = new MySqlCommand();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["User"].ToString()))
                Response.Redirect("Login.aspx");

            connection.Open();
        }

        protected void btnChangePass_Click(object sender, EventArgs e)
        {
            if (txtCurrentPass.Text == "" || txtNewPass.Text == "" || txtConfirmNewPass.Text == "")
            {
                MsgBox("Please fill in all fields.", "Change Password", MessageBoxIcon.Error);

                //If field is empty, make border red
                if (txtCurrentPass.Text == "")
                    txtCurrentPass.Style.Add("Border", "1px solid red");
                else
                    txtCurrentPass.Style.Add("Border", "1px solid black");

                if (txtNewPass.Text == "")
                    txtNewPass.Style.Add("Border", "1px solid red");
                else
                    txtNewPass.Style.Add("Border", "1px solid black");

                if (txtConfirmNewPass.Text == "")
                    txtConfirmNewPass.Style.Add("Border", "1px solid red");
                else
                    txtConfirmNewPass.Style.Add("Border", "1px solid black");
            }

            else if (txtNewPass.Text != txtConfirmNewPass.Text)
            {
                MsgBox("New passwords do not match.", "Change Password", MessageBoxIcon.Error);
            }

            else
            {
                if (txtCurrentPass.Text == Session["Password"].ToString())
                {
                    string changePassword = "USE Forum; UPDATE LoginInfo SET Password = @password WHERE Username = '" + Session["User"].ToString() + "'";
                    cmd = new MySqlCommand(changePassword, connection);

                    MySqlParameter[] param = new MySqlParameter[1];
                    param[0] = new MySqlParameter("@password", txtConfirmNewPass.Text);
                    cmd.Parameters.Add(param[0]);

                    cmd.ExecuteNonQuery();

                    Session["Password"] = txtNewPass.Text;
                    MsgBox("Password successfully changed.", "Change Password", MessageBoxIcon.Information);

                    Response.Redirect("HomePage.aspx");
                }
                
                else
                {
                    MsgBox("Current password is incorrect.", "Change Password", MessageBoxIcon.Error);
                }
            }
        }

        private void MsgBox(string message, string caption, MessageBoxIcon icon)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, caption, buttons, icon);
        }
    }
}