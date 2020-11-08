<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="true" CodeBehind="myaccount.aspx.cs" Inherits="ValisManager.clay.myaccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

        
        div.myaccount-wrapper
        {
            padding: 12px;
            margin-left: auto;
            margin-right: auto;
            width: 974px;
        }
        div.pageTitle {
            background-image: url(/content/images/user.png);
            background-position: 0px 6px;
            background-repeat: no-repeat;
            padding-left: 38px;
        }
        div.infoPanel {
            font-size: 1.2em;
        }
        div.infoPanel h2{
            color: #87a32e;
            font-size: 16px;
            font-weight: bold;
        }
        div.infoPanel label{
            line-height: 1.8em;
            display: inline-block;
            width: 100px;
        }
        div.infoPanel span{
            line-height: 1.8em;
            margin-left: 12px;
            color: #04005b;
        }
        div.infoPanel input[type=text]{
            width: 260px;
        }
        
        div.infoPanel-company label {
        }
        div.infoPanel-company span {
        }
        div.infoPanel-account label {
        }
        
        input.btnSaveProfile
        {
            background: url("/content/images/btnMain.gif") repeat-x scroll 0 0 #2eacaa;
            border: 1px solid #999999;
            color: #FFFFFF;
            font-size: .82em;
            padding: 4px 8px;
            margin-top: 24px;
            float: right;
            cursor: pointer;
        }
        input.btnChangePswd
        {
            background: url("/content/images/btnMain2.gif") repeat-x scroll 0 0 #2eacaa;
            border: 1px solid #999999;
            color: #FFFFFF;
            font-size: .78em;
            padding: 4px 8px;
            margin-top: 24px;
            float: right;
            cursor: pointer;
        }

        div.changePasswordPanel
        {

        }
        div.changePasswordPanel h2
        {
            color: #87a32e;
            font-size: 16px;
            font-weight: bold;
        }

        div.changePasswordPanel label {
            line-height: 1.8em;
            display: inline-block;
            width: 160px;
            text-align: right;
            padding-right: 8px;
        }
        div.changePasswordPanel label:after{
            content: ':';
        }

        div.changePasswordPanel input[type=text], div.changePasswordPanel input[type=password] {
            width: 220px;
        }
    </style>
    <script>
        function changePassword()
        {
            $('#logontoken').val('');
            $('#oldPass').val('');
            $('#newPass1').val('');
            $('#newPass2').val('');
            $("#changePassForm").dialog({ position: { my: "center", at: "center", of: window } }).dialog("open");
        }
        function OnChangePsswdBtn()
        {
            var logontoken = $('#logontoken').val();
            if (logontoken == '' || logontoken == null || logontoken == undefined) {
                $("#logontoken").animateHighlight("#fd2525", 400);
                $('#logontoken').focus();
                return;
            }

            var oldPass = $('#oldPass').val();
            if (oldPass == '' || oldPass == null || oldPass == undefined) {
                $("#oldPass").animateHighlight("#fd2525", 400);
                $('#oldPass').focus();
                return;
            }

            var newPass1 = $('#newPass1').val();
            if (newPass1 == '' || newPass1 == null || newPass1 == undefined) {
                $("#newPass1").animateHighlight("#fd2525", 400);
                $('#newPass1').focus();
                return;
            }

            var newPass2 = $('#newPass2').val();
            if (newPass2 == '' || newPass2 == null || newPass2 == undefined) {
                $("#newPass2").animateHighlight("#fd2525", 400);
                $('#newPass2').focus();
                return;
            }

            if(newPass1 != newPass2)
            {
                $('#newPass1').val('');
                $('#newPass2').val('');
                alert('Enter again new password!!');
                return;
            }

            var _data = 'logontoken=' + escape($('#logontoken').val());
            _data = _data + '&oldPass=' + escape($('#oldPass').val());
            _data = _data + '&newPass=' + escape($('#newPass1').val());

            var _url = theManagerPath + 'services/api/SystemUser/ChangePassword?ScId=' + theAccessToken;

            $.ajax({
                url: _url, data: _data, type: 'POST', async: false,
                success: function (data) {
                    //$('#changePassForm').dialog('close');
                    window.location.replace("/logoff.aspx");
                }
            });

        }

        $(document).ready(function () {
            $('#changePassForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, draggable: false, width: 500, buttons: { 'ChangePsswd': { class: 'lightgreenbutton', text: 'Change Password', id: 'ChangePsswdBtn', click: OnChangePsswdBtn }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'CancelChangePsswdBtn', click: function () { $(this).dialog("close"); } } } });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="myaccount-wrapper">
        <div class="pageTitle">
            <h1>My Account</h1>
        </div>

        <div class="infoPanel">
            <table style="width: 800px;">
                <tr><td style="width: 395px;"><h2>Company Info</h2></td><td style="width:10px;"></td><td class="width: 395px;"><h2>Account Info</h2></td></tr>
                <tr><td>
                    <div class="infoPanel-company">
                        <div><label>Name</label><span><%: this.SelectedClient.Name %></span></div>
                        <div><label>Profile</label><span><%: this.ClientProfile.Name %></span></div>
                        <div><label>Timezone</label><span><%: this.SelectedClient.TimeZoneId %></span></div>
                        <div><label>Country</label><span><%: this.CountryName %></span></div>
                        <div><label>Town</label><span><%: this.SelectedClient.Town %></span></div>
                        <div><label>Telephone 1</label><span><%:this.SelectedClient.Telephone1 %></span></div>
                        <div><label>Telephone 2</label><span><%:this.SelectedClient.Telephone2 %></span></div>
                    </div>
                </td><td>
                     </td><td>
                    <div class="infoPanel-account" style="clear: both; position: relative;">
                        <div><label>FirstName</label><span><asp:TextBox ID="FirstName" runat="server" required="required" ValidationGroup="a1"></asp:TextBox></span></div>
                        <div><label>LastName</label><span><asp:TextBox ID="LastName" runat="server" required="required" ValidationGroup="a1"></asp:TextBox></span></div>
                        <div><label>UserName</label><span><asp:TextBox ID="UserName" runat="server" required="required" ValidationGroup="a1"></asp:TextBox></span></div>
                        <div><label>Email</label><span><asp:TextBox ID="Email" runat="server" required="required" ValidationGroup="a1"></asp:TextBox></span></div>
                    
                        <asp:Button ID="btnSaveProfile" CssClass="btnSaveProfile" runat="server" Text="Update Account" OnClick="btnSaveProfile_Click" ValidationGroup="a1" />
                    </div>
                    <div style="clear: both; position: relative;">
                        <h2>Password Info</h2>
                        <input type="button" id="btnChangePswd" class="btnChangePswd" onclick="changePassword()" value="Change Password"/>
                    </div>
                </td></tr>
            </table>
        </div>

        <div id="changePassForm" title="Change Password" style="display: none">
            <div class="changePasswordPanel">
            <h2>Change Password</h2>
            <div><label for="logontoken">Username</label><input type="text" id="logontoken"/><span class="requiredFieldAsterisk">*</span></div>
            <div><label for="oldPass">Old Password</label><input type="password" id="oldPass"/><span class="requiredFieldAsterisk">*</span></div>
            <div><label for="newPass1">New Password</label><input type="password" id="newPass1"/><span class="requiredFieldAsterisk">*</span></div>
            <div><label for="newPass2">Repeat New Password</label><input type="password" id="newPass2"/><span class="requiredFieldAsterisk">*</span></div>
            </div>
        </div>
    </div>
</asp:Content>
