<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="login.aspx.cs" Inherits="ValisManager.clay.login.login" %>
<!DOCTYPE html>
<html lang="el">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1, maximum-scale=1, user-scalable=no" />
    <title>The Survey System</title>
    <link href="/favicon.ico" rel="icon" type="image/x-icon" />
    <link href="/content/login.css" rel="stylesheet" />
    <noscript>
	    <style type="text/css" media="all">form {display:none;}</style>
	    <div style="padding: 50px 25%;">
		    <div class="ErrorMessage">
			    Javascript is required for this site to function, please enable.
		    </div>
	    </div>
    </noscript>
</head>
<body>
    <div class="header">

    </div>
    <form id="theForm" runat="server">

        <div class="loginform">
            <div class="title">
                <p><%=ValisManager.Globals.BrandName %></p>
            </div>
            <div class="content">
                <div class="field">
                    <label>UserName:</label>
                    <asp:TextBox ID="username" runat="server" autocomplete="off" MaxLength="48"></asp:TextBox>
                </div>
                <div class="field">
                    <label>Password:</label>
                    <asp:TextBox ID="password" runat="server" autocomplete="off" MaxLength="48" TextMode="Password"></asp:TextBox>
                </div>
                <div class="buttons">
                    <asp:Button ID="loginButton" runat="server" Text="Go" CssClass="loginButton" OnClick="loginButton_Click" />
                </div>
                <asp:PlaceHolder ID="AutoLoginPlaceHolder" runat="server">
                    <div class="buttons">
                        <asp:DropDownList ID="AutoLogin" runat="server" AutoPostBack="True" OnSelectedIndexChanged="AutoLogin_SelectedIndexChanged">
                            <asp:ListItem Text="Account..." Value="0"></asp:ListItem>
                            <asp:ListItem Text="SystemAdmin (system)" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Developer (system)" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Admin (system)" Value="3"></asp:ListItem>
                            <asp:ListItem Text="PowerClient (Flovela)" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Client (Flovela)" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>
    </form>



        
      <div class="footer">
        <p>&copy;XCompany Ltd 2015</p>
      </div>
</body>
</html>
