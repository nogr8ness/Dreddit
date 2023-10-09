<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="Forum_Website.WebForm3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>

    </title>
    <style>
        #logo {
            display: block;
            margin-left: auto;
            margin-right: auto;
            width: 50%;
        }

        #menubar {
            background-color: orangered;
            height: 50px;
        }

        #btnMenuButton {
            height: 50px;
            background-color: orangered;
            color: white;
            border-left: 1px solid white;
            border-right: 1px solid white;
            border-top-color: orangered;
            border-bottom-color: orangered;
            position: relative;
            bottom: 18px;
            left: 1300px;
            width: 100px;
            font-size: 20px;
            cursor: pointer;
        }

        #btnMenuButton:hover, #btnMenuButton:focus {
            background-color: orange;
        }

        #dropdown {
            display: none;
            position: absolute;
            width: 100px;
            top: 370px;
            height: 33px;
            left: 1308px;
            z-index: 1;
        }

        #dropdown a {
            text-align: center;
            color: black;
            padding: 12px 16px;
            text-decoration: none;
            display: block;
            border: 0.5px solid black;
            background-color: antiquewhite;
        }

        #changepass {
            position: inherit;
            top: -5px;
        }

        #logout {
            border-top: 1px solid black;
            position: absolute;
            top: 57px;
            width: 66px;
        }

        #dropdown a:hover {
            background-color: orange;
        }

        .createpost {
            width: 125px;
            height: 40px;
            background-image: linear-gradient(to bottom right, cornflowerblue, blue);
            color: white;
            font-size: 12px;
            font-family: 'Times New Roman';
        }

        #btnCreateReply, #submitEditPost, #submitEditSubPost {
            width: 75px;
        }

        #back, #RepBack, #btnReturn, .cancel {
            width: 75px;
            height: 40px;
            font-size: 12px;
            font-family: 'Times New Roman';
        }

        #btnSubmitPost, #btnSubmitReply {
            position: relative;
            bottom: 40px;
            left: 100px;
        }

        #reply {
            bottom: 2010px;
            left: 230px;
        }

        #txtDesc, #txtReply {
            width: 400px;
            height: 250px;
        }

        .startvisible, .substartvisible {
            display: block;
        }

        .starthidden, .substarthidden {
            display: none;
        }

        .container {
            display: inline-block;
            width: fit-content;
            position: relative;
            text-align: center;
        }

        .posttitle {
            font-size: 36px;
            color: deepskyblue;
            font-weight: bolder;
            text-decoration: none;
        }

        .posttitle:hover {
            color: blue;
            cursor: pointer;
        }

        .userdate {
            position: relative;
            bottom: 11px;
            text-align: center;
            border-top: 1px dotted black;
            border-bottom: 1px dotted black;
            width: 500px;
        }

        .center {
            margin-left: auto;
            margin-right: auto;
        }

        .sidebar {
            background-color: orangered;
            width: 100px;
            /*height: 1000px;*/
            position: relative;
        }

        #rightsidebar {
            left: 1547px;
            /*bottom: 2750px;*/
        }

        .positionpost {
            position: relative; 
            bottom: 1970px;
            left: 130px;
        }

        .position {
            bottom: 2000px;
            left: 550px;
        }

        .description {
            position: relative; 
            bottom: 2025px;
            font-size: 20px;
            width: 500px;
            text-align: center;
        }

        .subcontainer {
            position: relative;
            left: 550px;
            width: 500px;
        }

        .subuserdate {
            position: relative;
            bottom: 13px;
        }

        .subtext {
            font-size: 27px;
            width: 500px;
            background-color: lightgray;
            position: relative;
            bottom: 30px;
        }

        .delete {
            position: absolute;
            top: 0px;
            right: 0px;
            display: none;
        }

        .subdelete {
            position: absolute;
            top: -15px;
            right: 0px;
            display: none;
        }

        .edit {
            position: absolute;
            top: 0px;
            right: 22px;
            display: block;
            width: 16px;
            height: 16px;
            display: none;
        }

        .subedit {
            position: absolute;
            top: -16px;
            right: 22px;
            display: block;
            width: 16px;
            height: 16px;
            display: none;
        }
    </style>
</head>
<body style="overflow-x:hidden" onload="window.history.forward()">
    
    <script>
        function dropdown() {
            if (document.getElementById("hidden").getAttribute("value") != "show")
                document.getElementById("hidden").setAttribute("Value", "show");
            else
                document.getElementById("hidden").setAttribute("Value", "hide");
        }

        function createpost() {
            var visible = document.getElementsByClassName("startvisible");
            for (let i = 0; i < visible.length; i++) {
                visible[i].style.display = "none";
            }

            var invisible = document.getElementsByClassName("starthidden");
            for (let j = 0; j < invisible.length; j++) {
                invisible[j].style.display = "block";
            }
        }

        function createreply() {
            var visible = document.getElementsByClassName("substartvisible");
            for (let i = 0; i < visible.length; i++) {
                visible[i].style.display = "none";
            }

            var invisible = document.getElementsByClassName("substarthidden");
            for (let j = 0; j < invisible.length; j++) {
                invisible[j].style.display = "block";
            }

            document.getElementById("reply").style = "left:130px; bottom:1990px";
        }

        function postback() {
            document.forms[0].submit();
        }

        function delbutton() {
            parent = event.target;
            child = parent.children[2];
            child2 = parent.children[3];

            document.getElementById(child.id).style.display = "block";
            document.getElementById(child2.id).style.display = "block";
        }

        function delremove() {
            parent = event.target;
            child = parent.children[2];
            child2 = parent.children[3];

            document.getElementById(child.id).style.display = "none";
            document.getElementById(child2.id).style.display = "none";
        }
    </script>

    <form id="form1" runat="server" autocomplete="off">
        <asp:HiddenField ID="hidden" Value="hide" runat="server"/>

        <img src="reddit_logo.png" id="logo" />
        <div id="menubar">
            <br />
            <asp:Button id="btnMenuButton" Text="test" OnClientClick="dropdown()" runat="server" />
            <asp:Panel ID="dropdown" runat="server">
                <asp:LinkButton href="ChangePassword.aspx" id="changepass" runat="server">Change Password</asp:LinkButton><br />
                <asp:LinkButton id="logout" OnClick="logout_Click" runat="server">Log Out</asp:LinkButton>
            </asp:Panel>           
        </div>

        <div class="sidebar" id="leftsidebar" runat="server"></div>
        <div class="sidebar" id="rightsidebar" runat="server"></div>

        <div id="posts" style="position:relative; bottom:900px" runat="server">
            <div id="createpost" class="positionpost">
                <asp:Button ID="btnCreatePost" class="startvisible createpost" OnClientClick="createpost(); return false;" Text="Create New Post +" runat="server"/>
                <asp:TextBox ID="txtTitle" class="starthidden" placeholder="Title (required)" runat="server"></asp:TextBox><br />
                <asp:TextBox ID="txtDesc" class="starthidden" MaxLength="500" TextMode="MultiLine" placeholder="Description (optional)" runat="server"></asp:TextBox>
                <asp:Button ID="back" class="starthidden" OnClientClick="document.forms[0].submit()" Text="Go Back" runat="server" />
                <asp:Button ID="btnSubmitPost" CssClass="starthidden createpost" style="width:75px" Text="Submit" runat="server" OnClick="btnSubmitPost_Click" />
            </div>

            <div class="positionpost">
                <asp:Button ID="btnReturn" class="substartvisible" Text="<< Go Back" OnClick="btnReturn_Click" runat="server" />
            </div>
            
            <div id="reply" class="positionpost">
                <asp:Button ID="btnCreateReply" class="substartvisible createpost" OnClientClick="createreply(); return false;" Text="Reply +" runat="server" />
                <asp:TextBox ID="txtReply" class="substarthidden" placeholder="Insert text here..." TextMode="MultiLine" runat="server"></asp:TextBox>
                <asp:Button ID="RepBack" class="substarthidden" OnClientClick="document.forms[0].submit()" Text="Go Back" runat="server" />
                <asp:Button ID="btnSubmitReply" CssClass="substarthidden createpost" OnClick="btnSubmitReply_Click" style="width:75px" Text="Submit" runat="server" />
            </div>

            <div id="defaultPost" class="center" runat="server">
                <div id="Post2" class="container startvisible position" onmouseover="delbutton()" onmouseleave="delremove()">
                    <asp:LinkButton id="Post1" class="posttitle" Text="Welcome to Dreddit!" OnClick="PostClick" runat="server" />
                    <p class="userdate">Posted by Dreddit on 12/8/2021 at 12:00 AM</p>
                </div>
                <p class="startvisible position description">Learn how to become a dredditor <a href="https://www.youtube.com/watch?v=dQw4w9WgXcQ&ab_channel=RickAstley" 
                    target="_blank" style="color:blue">here.</a></p><br /><br /><br />
            </div>
            
            <!-- <div class="subcontainer startvisible position" onmouseover="delbutton()" onmouseleave="delremove()">
                <p class="subuserdate">Posted by William Never on 1/1/2001</p>
                <p class="subtext">Lorem ipsum dolor sit amet...</p>
                <asp:ImageButton id="delReply1" class="subdelete" OnClick="DeleteSubPost" ImageUrl="~/trash.svg" runat="server" />
            </div> -->
        </div>        
    </form>
</body>
</html>
