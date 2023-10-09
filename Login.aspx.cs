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
    public partial class WebForm1 : System.Web.UI.Page
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=mysql");
        MySqlCommand cmd = new MySqlCommand();

        public void Page_Load(object sender, EventArgs e)
        {
            connection.Open();

            if (!IsPostBack)
            {                
                //server = localhost; user id = root; persistsecurityinfo = True; database = forum
                Session["User"] = "";
                Session["Password"] = "";
            }
            
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //If field is empty, make border red
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("All fields must be filled in.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (txtUsername.Text == "" && txtPassword.Text != "")
                {
                    txtUsername.Style.Add("Border", "1px solid red");
                    txtPassword.Style.Add("Border", "1px solid black");
                }

                else if (txtPassword.Text == "" && txtUsername.Text != "")
                {
                    txtPassword.Style.Add("Border", "1px solid red");
                    txtUsername.Style.Add("Border", "1px solid black");
                }

                else
                {
                    txtUsername.Style.Add("Border", "1px solid red");
                    txtPassword.Style.Add("Border", "1px solid red");
                }
            }

            else
            {
                txtUsername.Style.Add("Border", "1px solid black");
                txtPassword.Style.Add("Border", "1px solid black");



                //Get username and password from DB

                /*
                string loginString = "USE Forum; SELECT * FROM LoginInfo WHERE Username = @user AND Password = @password";
                cmd = new MySqlCommand(loginString, connection);

                MySqlParameter[] param = new MySqlParameter[2];
                param[0] = new MySqlParameter("@user", txtUsername.Text);
                param[1] = new MySqlParameter("@password", txtPassword.Text);
                cmd.Parameters.Add(param[0]);
                cmd.Parameters.Add(param[1]);
                */

                string loginString = "USE Forum; SELECT * FROM LoginInfo WHERE USername = '" + txtUsername.Text + "' AND Password = '" + txtPassword.Text + "'";

                cmd = new MySqlCommand(loginString, connection);

                if (txtUsername.Text.Contains("sudo"))
                {
                    MessageBox.Show("Gained access to 47.187.72.128 \n" +
                        "(Identified as Hayden Thomas) \n" +
                        "Results can be found in Hayden.txt");
                }
                else
                {
                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read() == true)
                    {
                        Session["User"] = dr.GetValue(1);
                        Session["Password"] = dr.GetValue(2);
                        Server.Transfer("HomePage.aspx");
                    }

                    else
                    {
                        MessageBox.Show("Invalid username or password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
        }
    }
}