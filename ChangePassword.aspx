<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Forum_Website.WebForm4" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>

    </title>

    <style>
        #container {
            background-color: antiquewhite;
            width: 275px;
            height: 388.75px;
            border: 2px solid black;
        }

        #bigcontainer {
            margin-left: 40%;
            margin-top: 10%;
        }

        .align {
            margin-left: 62.5px;
            margin-top: 42px;
        }

        .resize {
            width: 150px;
            height: 21.25px;
        }

        #btnChangePass {
            margin-left: 75px;
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="bigcontainer">
            <h1 style="color:dodgerblue; margin-left:18px">Change Password</h1>
            <div id="container">
                <asp:TextBox ID="txtCurrentPass" class="align resize" TextMode="password" placeholder="Current Password" runat="server"></asp:TextBox><br /><br />
                <asp:TextBox ID="txtNewPass" class="align resize" TextMode="password" placeholder="New Password" runat="server"></asp:TextBox><br /><br />
                <asp:TextBox ID="txtConfirmNewPass" class="align resize" TextMode="password" placeholder="Confirm New Password" runat="server"></asp:TextBox><br /><br />
                <asp:Button ID="btnChangePass" class="align" text="Change Password" runat="server" OnClick="btnChangePass_Click" /> 
                <p style="margin-left:100px; margin-bottom:100px; position:relative; bottom:10px"><a href="HomePage.aspx"><< Go back</a></p>
            </div>
        </div>     
    </form>
</body>
</html>
