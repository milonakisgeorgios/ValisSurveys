<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="remove.aspx.cs" Inherits="ValisManager.commands.recipient.remove" %>
<!DOCTYPE html>
<html lang="el">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1, maximum-scale=1, user-scalable=no" />
    <title>The Survey System - Email Preferences</title>
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
    <style>
        .greenbutton
        {
            display:inline-block;
            text-decoration: none;
            font-size: 12px;
            font-weight: normal;
            line-height: 1.8em;
            text-align: center;
            vertical-align: middle;
            color: #f1f1f1;
            padding: 0px 8px 0px 8px;
            border-radius: 3px;
            background: url("/content/images/btnMain.gif") repeat-x scroll 0 0 #2eacaa;
            cursor: pointer;
            min-width: 70px;
        }
        .mylink
        {
            outline: medium none;
             color: #00898b;
            text-decoration: none;

        }
    </style>
</head>
<body>
    <div class="header">

    </div>

    <form id="theForm" runat="server">
        <asp:Panel ID="QuestionPanel" runat="server" Visible="false">

            <div style="width: 100%; font-size: 32px; font-weight: bold; margin-top: 50px; margin-right: auto; margin-left: auto;text-align: center;">
                Do You Want to Receive Surveys from Us?
            </div>
            <div style="width: 540px; font-size: 16px; font-weight: bold; margin-top: 50px; margin-right: auto; margin-left: auto;text-align: center;">
                Are you sure you want to block surveys from XCompany surveys being sent to this email address: 
                <span style="font-style: italic; color: #00105e;"><%:this.Recipient.Email %></span>?
            </div>

            <div class="center" id="divConfirmBtns">

                <div style="margin: 50px auto 0px; width: 700px;">
                    <div style="text-align: center; margin-right: auto; margin-left: 180px; float: left;">
                        <div class="headerTitle" style="text-align: center; white-space: nowrap;">
                            Yes, block surveys from this person only.
                        </div>
                        <div style="width: 300px; margin-bottom: 37px;">
                            If you'd like to block surveys sent to you from <b>just this particular person</b>, please click the button below.
                        </div>
                        <asp:Button ID="btnLocalOptOut" runat="server" Text="Block This Person" CssClass="greenbutton" OnClick="btnLocalOptOut_Click" UseSubmitBehavior="False"/>
                    </div>
                </div>

                <div style="text-align: center; padding-top: 200px;">
                    <asp:HyperLink ID="lnkSurvey" runat="server" CssClass="mylink">No, I just want to take the survey</asp:HyperLink>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="ResultPanel" runat="server" Visible="false">
            
            <div style="width: 100%; font-size: 32px; font-weight: bold; margin-top: 50px; margin-right: auto; margin-left: auto;text-align: center;">
                Do You Want to Receive Surveys from Us?
            </div>

            <div style="width:600px; margin:50px auto 100px auto; font-size:14px; font-weight:bold">
                Sending to the email address: <span style="font-style: italic; color: #00105e;"><%:this.Recipient.Email %></span> has been blocked.<br><br>
                You will no longer receive surveys sent by SurveyMonkey or Research.net on behalf of this person.
            </div>
        </asp:Panel>    
        <asp:Panel ID="ErrorPanel" runat="server" Visible="false">
            <div style="width: 100%; font-size: 18px; font-weight: bold; margin-top: 50px; margin-right: auto; margin-left: auto; margin-bottom: 150px; text-align: center; padding: 12px;background-color: #feef48; color: #ff0000">
                <%:this.ErrorMessage %>
            </div>
        </asp:Panel>    
    </form>
    
    <div class="footer">
        <p>&copy;XCompany Ltd 2015</p>
    </div>
</body>
</html>
