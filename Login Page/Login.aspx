<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Forum_Website.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        #container {
            background-color: antiquewhite;
            width: 275px;
            height: 343.75px;
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

        #btnLogin {
            margin-left: 106.25px;
            text-align: center;
            width: 64px;
            height: 30px;
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
    <form id="Login" autocomplete="off" runat="server">
        <h1 id="logo">Welcome to <img src="reddit_logo.png" id="logo_pic" /></h1>
        <div id="bigcontainer">
            <h1 style="color:dodgerblue; margin-left:100px">Login</h1>
            <div id="container">
                <asp:TextBox ID="txtUsername" class="align resize" placeholder="Username" runat="server"></asp:TextBox><br /><br />
                <asp:TextBox ID="txtPassword" class="align resize" TextMode="password" placeholder="Password" runat="server"></asp:TextBox><br /><br />
                <asp:Button ID="btnLogin" class="align" text="Login" runat="server" OnClick="btnLogin_Click" /> 
                <p class="align" style="margin-left:11px">Don't have an account? <a href="CreateAccount.aspx">Create one here.</a></p>
            </div>
        </div>     
    </form>
</body>
</html>
