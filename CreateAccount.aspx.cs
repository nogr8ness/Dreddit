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
    public partial class WebForm2 : System.Web.UI.Page
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=mysql");
        MySqlCommand cmd = new MySqlCommand();

        protected void Page_Load(object sender, EventArgs e)
        {
            connection.Open();
        }

        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            if (txtCreateUser.Text == "" || txtCreatePass.Text == "" || txtConfirmPassword.Text == "")
            {
                MsgBox("Please fill in all fields.", "Create Account", MessageBoxIcon.Error);

                //If field is empty, make border red
                if (txtCreateUser.Text == "")
                    txtCreateUser.Style.Add("Border", "1px solid red");
                else
                    txtCreateUser.Style.Add("Border", "1px solid black");

                if (txtCreatePass.Text == "")
                    txtCreatePass.Style.Add("Border", "1px solid red");
                else
                    txtCreatePass.Style.Add("Border", "1px solid black");

                if (txtConfirmPassword.Text == "")
                    txtConfirmPassword.Style.Add("Border", "1px solid red");
                else
                    txtConfirmPassword.Style.Add("Border", "1px solid black");
            }

            else if (txtCreatePass.Text != txtConfirmPassword.Text)
            {
                MsgBox("New passwords do not match.", "Create Account", MessageBoxIcon.Error);

                txtCreatePass.Style.Add("Border", "1px solid red");
                txtConfirmPassword.Style.Add("Border", "1px solid red");
                txtCreateUser.Style.Add("Border", "1px solid black");
            }

            else if (txtCreateUser.Text.Length > 20)
            {
                MsgBox("Username cannot be longer than 20 characters.", "Create Account", MessageBoxIcon.Error);
                txtCreateUser.Style.Add("Border", "1px solid red");
            }

            else if (txtCreateUser.Text.Length < 3)
            {
                MsgBox("Username must be at least 3 characters long.", "Create Account", MessageBoxIcon.Error);
                txtCreateUser.Style.Add("Border", "1px solid red");
            }

            else
            {
                string duplicate = "USE Forum; SELECT * FROM LoginInfo WHERE Username= '" + txtCreateUser.Text + "'";
                cmd = new MySqlCommand(duplicate, connection);

                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read() == false)
                {
                    dr.Close();

                    string createAccount = "USE Forum; INSERT INTO LoginInfo (Username, Password) VALUES (@user, @password)";
                    cmd = new MySqlCommand(createAccount, connection);

                    MySqlParameter[] param = new MySqlParameter[2];
                    param[0] = new MySqlParameter("@user", txtCreateUser.Text);
                    param[1] = new MySqlParameter("@password", txtCreatePass.Text);
                    cmd.Parameters.Add(param[0]);
                    cmd.Parameters.Add(param[1]);

                    cmd.ExecuteNonQuery();

                    MsgBox("Account creation successful.", "Create Account", MessageBoxIcon.Information);
                    Response.Redirect("Login.aspx");
                }
                
                else
                {
                    dr.Close();
                    MsgBox("Username already taken. Please try another.", "Create Account", MessageBoxIcon.Error);
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