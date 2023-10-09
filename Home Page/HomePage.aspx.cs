using MySql.Data.MySqlClient;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Web.UI;

namespace Forum_Website
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=mysql");
        MySqlCommand cmd = new MySqlCommand();
        int increment = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["User"].ToString()))
                Response.Redirect("Login.aspx");

            connection.Open();

            if (!IsPostBack)
            {
                btnMenuButton.Text = Session["User"].ToString() + " ▼";

                if (btnMenuButton.Text.Length > 5 && btnMenuButton.Text.Length <= 10)
                {
                    btnMenuButton.Style.Add("font-size", "15px");
                }
                if (btnMenuButton.Text.Length > 10 && btnMenuButton.Text.Length <= 17)
                {
                    btnMenuButton.Style.Add("font-size", "10px");
                }

                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:postback(); ", true);
            }

            if (ViewState["EditPostID"] == null)
                ViewState["EditPostID"] = "";
            if (ViewState["EditSubPostID"] == null)
                ViewState["EditSubPostID"] = "";

            if (ViewState["PostID"] == null || ViewState["PostID"].ToString() == "")
                LoadPosts();
            else
                LoadSubPost(ViewState["PostID"].ToString());

            if (hidden.Value == "show")
            {
                dropdown.Attributes.Add("style", "display: block");
            }

            if (hidden.Value == "hide")
            {
                dropdown.Attributes.Add("style", "display: none");
            }

            
        }

        protected void logout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
            Session.Clear();
        }



        //Create a post



        protected void btnSubmitPost_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text == "")
            {
                MessageBox.Show("Please create a title for your post.");
            }

            else
            {
                string titleString = txtTitle.Text;
                string userString = Session["User"].ToString();
                string dateString = DateTime.Now.ToShortDateString();
                string descriptionString = txtDesc.Text;
                string timeString = DateTime.Now.ToShortTimeString();

                if (descriptionString != "")
                    CreatePost(titleString, userString, dateString, descriptionString, timeString);
                else
                    CreatePost(titleString, userString, dateString, timeString);

                LoadPosts();
            }
        }

        private void CreatePost(string title, string user, string date, string time)
        {
            string dataString = "USE Forum; INSERT INTO Posts (Title, User, Date, Time) VALUES (@title, '" + user + "','" + date + "','" + time + "')";
            cmd = new MySqlCommand(dataString, connection);

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@title", title);
            cmd.Parameters.Add(param[0]);

            cmd.ExecuteNonQuery();

            CreateTable();
        }

        private void CreatePost(string title, string user, string date, string desc, string time)
        {
            string dataString = "USE Forum; INSERT INTO Posts (Title, User, Date, Description, Time) VALUES (@title, '" + user + "','" + 
                date + "', @desc, '" + time + "')";

            cmd = new MySqlCommand(dataString, connection);

            MySqlParameter[] param = new MySqlParameter[2];
            param[0] = new MySqlParameter("@title", title);
            param[1] = new MySqlParameter("@desc", desc);
            cmd.Parameters.Add(param[0]);
            cmd.Parameters.Add(param[1]);
            
            cmd.ExecuteNonQuery();

            CreateTable();
        }

        private void CreateTable()
        {
            string getID = "USE Forum; SELECT PostID FROM Posts ORDER BY PostID DESC LIMIT 1";
            cmd = new MySqlCommand(getID, connection);
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            string postID = "Post" + dr.GetValue(0).ToString();
            dr.Close();

            string table = "CREATE TABLE " + postID + 
                " (SubPostID int auto_increment PRIMARY KEY, Text varchar(500), User varchar(25), Date varchar(30), Time varchar(30))";

            cmd = new MySqlCommand(table, connection);
            cmd.ExecuteNonQuery();

            Server.Transfer("HomePage1.aspx");
        }



        //Load posts (both during Page_Load and after post is created)



        private void LoadPosts()
        {
            string getRows = "USE Forum; SELECT COUNT(*) FROM Posts";
            cmd = new MySqlCommand(getRows, connection);
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int maxPosts = Convert.ToInt32(dr.GetValue(0));

            dr.Close();

            if (!string.IsNullOrEmpty(ViewState["EditPostID"].ToString()))
            {
                string getPost = "USE Forum; SELECT * FROM Posts WHERE PostID = " + ViewState["EditPostID"].ToString().Substring(4);
                cmd = new MySqlCommand(getPost, connection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();

                string title = rdr.GetValue(1).ToString();
                string desc = rdr.GetValue(4).ToString();

                System.Web.UI.WebControls.Panel container = new System.Web.UI.WebControls.Panel()
                {
                    CssClass = "container startvisible"
                };
                container.Attributes.Add("style", "bottom:2000px; left:600px");
                posts.Controls.Add(container);

                HtmlGenericControl edit = new HtmlGenericControl("h2")
                {
                    InnerText = "Edit Post"
                };
                edit.Attributes.Add("class", "posttitle");
                container.Controls.Add(edit);

                System.Web.UI.WebControls.TextBox txtTitle = new System.Web.UI.WebControls.TextBox()
                {
                    ID = "editTitle",
                    Text = title,
                };
                txtTitle.Attributes.Add("placeholder", "Title (required)");
                txtTitle.Style.Add("width", "400px");
                container.Controls.Add(txtTitle);

                container.Controls.Add(new LiteralControl("<br />"));
                container.Controls.Add(new LiteralControl("<br />"));

                System.Web.UI.WebControls.TextBox txtDesc = new System.Web.UI.WebControls.TextBox()
                {
                    ID = "editDesc",
                    Text = desc,
                    TextMode = TextBoxMode.MultiLine
                };
                txtDesc.Attributes.Add("placeholder", "Description (optional)");
                txtDesc.Attributes.Add("style", "width:400px; height:250px");
                container.Controls.Add(txtDesc);

                container.Controls.Add(new LiteralControl("<br />"));

                System.Web.UI.WebControls.Button cancelEdit = new System.Web.UI.WebControls.Button()
                {
                    ID = "cancelEditPost",
                    Text = "Cancel",
                    CssClass = "cancel"
                };
                cancelEdit.Style.Add("position", "relative");
                cancelEdit.Style.Add("right", "128px");
                cancelEdit.Click += CancelEdit_Click;
                container.Controls.Add(cancelEdit);

                System.Web.UI.WebControls.Button submitEdit = new System.Web.UI.WebControls.Button()
                {
                    ID = "submitEditPost",
                    Text = "Submit",
                    CssClass = "createpost"
                };
                submitEdit.Style.Add("position", "relative");
                submitEdit.Style.Add("right", "100px");
                submitEdit.Click += SubmitEdit_Click;
                container.Controls.Add(submitEdit);

                defaultPost.Visible = false;
                btnCreatePost.Visible = false;

                rdr.Close();
            }
            else
            {
                defaultPost.Visible = true;
                btnCreatePost.Visible = true;

                for (int postNum = 1; postNum <= maxPosts; postNum++)
                {
                    dr.Close();

                    string getPost = "USE Forum; SELECT * FROM Posts LIMIT 1 OFFSET " + (postNum - 1);
                    cmd = new MySqlCommand(getPost, connection);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();

                    string postID = "Post" + Convert.ToInt32(rdr.GetValue(0));
                    string title = rdr.GetValue(1).ToString();
                    string user = rdr.GetValue(2).ToString();
                    string date = rdr.GetValue(3).ToString();
                    string desc = rdr.GetValue(4).ToString();
                    string time = rdr.GetValue(5).ToString();

                    System.Web.UI.WebControls.Panel container = new System.Web.UI.WebControls.Panel()
                    {
                        CssClass = "container startvisible position"
                    };
                    container.Attributes.Add("onmouseover", "delbutton()");
                    container.Attributes.Add("onmouseleave", "delremove()");
                    posts.Controls.Add(container);

                    LinkButton postTitle = new LinkButton()
                    {
                        ID = postID,
                        Text = title,
                    };
                    postTitle.Attributes.Add("class", "posttitle");
                    postTitle.Click += PostClick;
                    container.Controls.Add(postTitle);


                    HtmlGenericControl userdate = new HtmlGenericControl("p")
                    {
                        InnerText = "Posted by " + user + " on " + date + " at " + time
                    };
                    userdate.Attributes.Add("class", "userdate");
                    container.Controls.Add(userdate);


                    if (user == Session["User"].ToString() || Session["User"].ToString() == "admin")
                    {
                        ImageButton delete = new ImageButton()
                        {
                            ID = "del" + postID,
                            CssClass = "delete",
                            ImageUrl = "~/trash.svg"
                        };
                        delete.Click += DeletePost;
                        container.Controls.Add(delete);
                    }

                    if (user == Session["User"].ToString())
                    {
                        ImageButton edit = new ImageButton()
                        {
                            ID = "edit" + postID,
                            CssClass = "edit",
                            ImageUrl = "~/Pencil_edit_icon.jpg"
                        };
                        edit.Click += EditPost;
                        container.Controls.Add(edit);
                    }

                    HtmlGenericControl description = new HtmlGenericControl("p")
                    {
                        InnerText = desc
                    };
                    description.Attributes.Add("class", "startvisible position description");
                    posts.Controls.Add(description);

                    posts.Controls.Add(new LiteralControl("<br />"));
                    posts.Controls.Add(new LiteralControl("<br />"));
                    posts.Controls.Add(new LiteralControl("<br />"));

                    rdr.Close();

                    increment += 30;
                }
            }

            int sideBarHeight = 1000 + 200 * maxPosts + increment;
            leftsidebar.Attributes.Add("style", "height: " + sideBarHeight.ToString() + "px");
            rightsidebar.Attributes.Add("style", "bottom: " + sideBarHeight.ToString() + "px; height: " + sideBarHeight.ToString() + "px");
            
            double movePostsUp = 400 * maxPosts + 2 * increment;
            posts.Attributes.Add("style", "position: relative; bottom: " + movePostsUp + "px");

            dr.Close();

            btnCreateReply.Visible = false;
            btnReturn.Visible = false;
        }




        //Delete post




        protected void DeletePost(object sender, EventArgs e)
        {
            string msg = "Are you sure you want to delete this post? \n" +
                "This action cannot be undone.";
            string caption = "Delete Post";
            MessageBoxButtons btns = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;
            MessageBoxDefaultButton defaultBtn = MessageBoxDefaultButton.Button2;
            DialogResult result = MessageBox.Show(msg, caption, btns, icon, defaultBtn);

            if (result == DialogResult.Yes)
            {
                string btnID = ((ImageButton)sender).ID;
                string table = btnID.Substring(3);
                string postNum = btnID.Substring(7);

                string delPost = "DELETE FROM Posts WHERE PostID = " + postNum;
                cmd = new MySqlCommand(delPost, connection);
                cmd.ExecuteNonQuery();

                string delTable = "DROP TABLE " + table;
                cmd = new MySqlCommand(delTable, connection);
                cmd.ExecuteNonQuery();

                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:postback(); ", true);
            }
        }




        //Edit post




        protected void EditPost(object sender, ImageClickEventArgs e)
        {
            ViewState["EditPostID"] = ((ImageButton)sender).ID.Substring(4);
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:postback(); ", true);
        }

        private void CancelEdit_Click(object sender, EventArgs e)
        {
            ViewState["EditPostID"] = "";
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:postback(); ", true);
        }

        private void SubmitEdit_Click(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.TextBox txtTitle = FindControl("editTitle") as System.Web.UI.WebControls.TextBox;
            string title = txtTitle.Text;

            System.Web.UI.WebControls.TextBox txtDesc = FindControl("editDesc") as System.Web.UI.WebControls.TextBox;
            string desc = txtDesc.Text;

            string update = "UPDATE Posts SET Title = @title, Description = @desc WHERE PostID = '" +
                ViewState["EditPostID"].ToString().Substring(4) + "'";
            cmd = new MySqlCommand(update, connection);

            MySqlParameter[] param = new MySqlParameter[2];
            param[0] = new MySqlParameter("@title", title);
            param[1] = new MySqlParameter("@desc", desc);
            cmd.Parameters.Add(param[0]);
            cmd.Parameters.Add(param[1]);

            cmd.ExecuteNonQuery();

            ViewState["EditPostID"] = "";

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:postback(); ", true);
        }



        //Load all replies for the specific post



        protected void PostClick(object sender, EventArgs e)
        {
            string postID = ((LinkButton)sender).ID;
            ViewState["PostID"] = postID;
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:postback(); ", true);
        }

        private void LoadSubPost(string postID)
        {
            if (string.IsNullOrEmpty(ViewState["EditSubPostID"].ToString()))
            {
                if (postID == "Post1")
                {
                    defaultPost.Visible = true;
                    defaultPost.Attributes.Add("style", "position:relative; bottom:60px");
                }
                else
                {
                    defaultPost.Visible = false;

                    string getPost = "USE Forum; SELECT * FROM Posts WHERE PostID = '" + postID.Substring(4) + "'";
                    cmd = new MySqlCommand(getPost, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    string postTitle = reader.GetValue(1).ToString();
                    string postUser = reader.GetValue(2).ToString();
                    string postDate = reader.GetValue(3).ToString();
                    string postDesc = reader.GetValue(4).ToString();
                    string postTime = reader.GetValue(5).ToString();
                    reader.Close();

                    System.Web.UI.WebControls.Panel container = new System.Web.UI.WebControls.Panel()
                    {
                        CssClass = "container substartvisible position"
                    };
                    container.Style.Add("bottom", "2100px");
                    posts.Controls.Add(container);

                    HtmlGenericControl title = new HtmlGenericControl("h2")
                    {
                        ID = postID,
                        InnerText = postTitle,
                    };
                    title.Attributes.Add("class", "posttitle");
                    container.Controls.Add(title);


                    HtmlGenericControl userdate = new HtmlGenericControl("p")
                    {
                        InnerText = "Posted by " + postUser + " on " + postDate + " at " + postTime
                    };
                    userdate.Attributes.Add("class", "userdate");
                    userdate.Style.Add("bottom", "20px");
                    container.Controls.Add(userdate);

                    HtmlGenericControl description = new HtmlGenericControl("p")
                    {
                        InnerText = postDesc
                    };
                    description.Attributes.Add("class", "substartvisible position description");
                    description.Style.Add("bottom", "2130px");
                    posts.Controls.Add(description);

                    //Gets info about parent post using PostID and dynamically generates it
                }
            }



            string getRows = "USE Forum; SELECT COUNT(*) FROM " + postID;
            cmd = new MySqlCommand(getRows, connection);
            MySqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            int maxPosts = Convert.ToInt32(dr.GetValue(0));

            dr.Close();

            if (maxPosts > 0)
            {
                if (!string.IsNullOrEmpty(ViewState["EditSubPostID"].ToString()))
                {
                    string getSubPost = "USE Forum; SELECT * FROM " + postID + " WHERE SubPostID = " + ViewState["EditSubPostID"].ToString();
                    cmd = new MySqlCommand(getSubPost, connection);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    rdr.Read();

                    string text = rdr.GetValue(1).ToString();

                    System.Web.UI.WebControls.Panel container = new System.Web.UI.WebControls.Panel()
                    {
                        CssClass = "container startvisible"
                    };
                    container.Attributes.Add("style", "bottom:2000px; left:600px");
                    posts.Controls.Add(container);

                    HtmlGenericControl edit = new HtmlGenericControl("h2")
                    {
                        InnerText = "Edit Reply"
                    };
                    edit.Attributes.Add("class", "posttitle");
                    container.Controls.Add(edit);

                    System.Web.UI.WebControls.TextBox txtContent = new System.Web.UI.WebControls.TextBox()
                    {
                        ID = "editContent",
                        Text = text,
                        TextMode = TextBoxMode.MultiLine
                    };
                    txtContent.Attributes.Add("placeholder", "Insert text here...");
                    txtContent.Attributes.Add("style", "width:400px; height:250px");
                    container.Controls.Add(txtContent);

                    container.Controls.Add(new LiteralControl("<br />"));

                    System.Web.UI.WebControls.Button cancelEdit = new System.Web.UI.WebControls.Button()
                    {
                        ID = "cancelEditSubPost",
                        Text = "Cancel",
                        CssClass = "cancel"
                    };
                    cancelEdit.Style.Add("position", "relative");
                    cancelEdit.Style.Add("right", "128px");
                    cancelEdit.Click += CancelSubEdit;
                    container.Controls.Add(cancelEdit);

                    System.Web.UI.WebControls.Button submitEdit = new System.Web.UI.WebControls.Button()
                    {
                        ID = "submitEditSubPost",
                        Text = "Submit",
                        CssClass = "createpost"
                    };
                    submitEdit.Style.Add("position", "relative");
                    submitEdit.Style.Add("right", "100px");
                    submitEdit.Click += SubmitSubEdit;
                    container.Controls.Add(submitEdit);

                    defaultPost.Visible = false;
                    btnReturn.Visible = false;
                    btnCreateReply.Visible = false;

                    rdr.Close();
                }

                else
                {
                    btnCreateReply.Visible = true;
                    btnReturn.Visible = true;

                    for (int postNum = 1; postNum <= maxPosts; postNum++)
                    {
                        dr.Close();

                        string getSubPost = "USE Forum; SELECT * FROM " + postID + " LIMIT 1 OFFSET " + (postNum - 1);
                        cmd = new MySqlCommand(getSubPost, connection);
                        MySqlDataReader rdr = cmd.ExecuteReader();
                        rdr.Read();

                        string subPostID = postID + "Reply" + Convert.ToInt32(rdr.GetValue(0));
                        string user = rdr.GetValue(2).ToString();
                        string date = rdr.GetValue(3).ToString();
                        string text = rdr.GetValue(1).ToString();
                        string time = rdr.GetValue(4).ToString();

                        System.Web.UI.WebControls.Panel subContainer = new System.Web.UI.WebControls.Panel()
                        {
                            CssClass = "subcontainer substartvisible position",
                            ID = subPostID
                        };
                        subContainer.Style.Add("bottom", "2070px");
                        subContainer.Attributes.Add("onmouseover", "delbutton()");
                        subContainer.Attributes.Add("onmouseleave", "delremove()");
                        posts.Controls.Add(subContainer);

                        HtmlGenericControl subUserDate = new HtmlGenericControl("p")
                        {
                            InnerText = "Posted by " + user + " on " + date + " at " + time
                        };
                        subUserDate.Attributes.Add("class", "subuserdate");
                        subContainer.Controls.Add(subUserDate);

                        HtmlGenericControl content = new HtmlGenericControl("p")
                        {
                            InnerText = text
                        };
                        content.Attributes.Add("class", "subtext");
                        subContainer.Controls.Add(content);

                        if (user == Session["User"].ToString() || Session["User"].ToString() == "admin")
                        {
                            string subPostNum = "Reply" + Convert.ToInt32(rdr.GetValue(0));

                            ImageButton delete = new ImageButton()
                            {
                                ID = "delete" + subPostNum,
                                CssClass = "subdelete",
                                ImageUrl = "~/trash.svg"
                            };
                            delete.Click += DeleteSubPost;
                            subContainer.Controls.Add(delete);
                        }

                        if (user == Session["User"].ToString())
                        {
                            ImageButton edit = new ImageButton()
                            {
                                ID = "editReply" + Convert.ToInt32(rdr.GetValue(0)),
                                CssClass = "subedit",
                                ImageUrl = "~/Pencil_edit_icon.jpg"
                            };
                            edit.Click += EditSubPost;
                            subContainer.Controls.Add(edit);
                        }

                        rdr.Close();
                    }
                }     
            }
            else
            {
                btnCreateReply.Visible = true;
                btnReturn.Visible = true;

                HtmlGenericControl noReply = new HtmlGenericControl("p")
                {
                    InnerText = "No one has replied to this post yet.",
                };
                noReply.Attributes.Add("class", "subText substartvisible subcontainer position");
                noReply.Attributes.Add("style", "font-size:30px; bottom:2100px");
                posts.Controls.Add(noReply);
            }

            dr.Close();
            btnCreatePost.Visible = false;
            
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Server.Transfer("HomePage1.aspx");
        }



        //Edit Reply



        private void EditSubPost(object sender, ImageClickEventArgs e)
        {
            ViewState["EditSubPostID"] = ((ImageButton)sender).ID.Substring(9);
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:postback(); ", true);
        }

        private void CancelSubEdit(object sender, EventArgs e)
        {
            ViewState["EditSubPostID"] = "";
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:postback(); ", true);
        }

        private void SubmitSubEdit(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.TextBox txtContent = FindControl("editContent") as System.Web.UI.WebControls.TextBox;
            string content = txtContent.Text;

            string update = "UPDATE " + ViewState["PostID"] + " SET Text = @content WHERE SubPostID = " + ViewState["EditSubPostID"].ToString();
            cmd = new MySqlCommand(update, connection);

            MySqlParameter[] param = new MySqlParameter[1];
            param[0] = new MySqlParameter("@content", content);
            cmd.Parameters.Add(param[0]);

            cmd.ExecuteNonQuery();

            ViewState["EditSubPostID"] = "";

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:postback(); ", true);
        }



        //Create new subpost



        protected void btnSubmitReply_Click(object sender, EventArgs e)
        {
            if (txtReply.Text == "")
            {
                MessageBox.Show("Reply cannot be empty.");
            }

            else
            {
                string content = txtReply.Text;
                string user = Session["User"].ToString();
                string date = DateTime.Now.ToShortDateString();
                string time = DateTime.Now.ToShortTimeString();

                string dataString = "USE Forum; INSERT INTO " + ViewState["PostID"] + " (Text, User, Date, Time) VALUES " +
                    "(@content, '" + user + "','" + date + "','" + time + "')";

                cmd = new MySqlCommand(dataString, connection);
                MySqlParameter[] param = new MySqlParameter[1];
                param[0] = new MySqlParameter("@content", content);
                cmd.Parameters.Add(param[0]);

                cmd.ExecuteNonQuery();

                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:postback(); ", true);
            }
        }

        

        //Delete Subpost 



        protected void DeleteSubPost(object sender, EventArgs e)
        {
            string msg = "Are you sure you want to delete this reply? \n" +
                "This action cannot be undone.";
            string caption = "Delete Post";
            MessageBoxButtons btns = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Exclamation;
            MessageBoxDefaultButton defaultBtn = MessageBoxDefaultButton.Button2;
            DialogResult result = MessageBox.Show(msg, caption, btns, icon, defaultBtn);

            if (result == DialogResult.Yes)
            {
                string btnID = ((ImageButton)sender).ID;
                string table = ViewState["PostID"].ToString();
                string replyNum = btnID.Substring(11);

                string delReply = "DELETE FROM " + table + " WHERE SubPostID = " + replyNum;
                cmd = new MySqlCommand(delReply, connection);
                cmd.ExecuteNonQuery();

                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:postback(); ", true);
            }
        }

        
    }
}