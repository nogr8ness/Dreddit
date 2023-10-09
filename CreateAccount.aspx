<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAccount.aspx.cs" Inherits="Forum_Website.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #container {
            background-color: antiquewhite;
            width: 275px;
            height: 388.75px;
            border: 2px solid black;
        }

        #bigcontainer {
            margin-left: 40%;
            margin-top: 5%;
        }

        .align {
            margin-left: 68.75px;
            margin-top: 42px;
        }

        .resize {
            width: 137.5px;
            height: 21.25px;
        }

        #btnCreateAccount {
            margin-left: 85px;
            margin-bottom: 25px;
            height: 30px;
            position: relative;
            bottom: 10px;
        }

        #logo {
            font-size: 100px;
            text-align: center;
        }

        #logo_pic {
            position: relative;
            top: 50px;
        }

        #ddlSecurity {
            margin-left: 50px;
        }
    </style>
</head>
<body>
    <form id="CreateAccount" autocomplete="off" runat="server">
        <h1 id="logo">Welcome to <img src="reddit_logo.png" id="logo_pic" /></h1>
        <div id="bigcontainer">
            <h1 style="color:dodgerblue; margin-left: 35.25px">Create Account</h1>
            <div id="container">
                <asp:TextBox ID="txtCreateUser" class="align resize" placeholder="Username" runat="server"></asp:TextBox><br /><br />
                <asp:TextBox ID="txtCreatePass" class="align resize" TextMode="Password" placeholder="Password" runat="server"></asp:TextBox><br /><br />
                <asp:TextBox ID="txtConfirmPassword" class="align resize" TextMode="Password" placeholder="Confirm Password" runat="server"></asp:TextBox><br /><br />
                <asp:Button ID="btnCreateAccount" class="align" text="Create Account" runat="server" OnClick="btnCreateAccount_Click" /> 
                <asp:DropDownList ID="ddlSecurity" runat="server">
                    <asp:ListItem Selected="True" Value="(Select Security Question)">(Select Security Question)</asp:ListItem>
                </asp:DropDownList>
                <p style="margin-left:15px; margin-bottom:85px; position:relative; bottom:10px;">Already have an account? <a href="Login.aspx">Login here.</a></p>
            </div>
        </div>     
    </form>
</body>
</html>
