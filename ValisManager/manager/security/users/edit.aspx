<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="ValisManager.manager.security.users.edit" %>
<%@ Register src="usersform.ascx" tagname="usersform" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {
            $('#newPswdForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, resizable: false, width: 380, height: 235, buttons: { 'Change': { text: 'Change Password', id: 'formChangeButton', click: OnChangePasswordButton }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'formChangeCancelButton', click: function () { $(this).dialog("close"); } } } });
                
        });
        function OnUnlockUser()
        {
            showQuestion("Are you sure you want to unlock this user?", function () {

            var _data = 'UserId=<%=this.SelectedUser.UserId%>';
            var _url = theManagerPath + 'services/api/SystemUser/UnlockUser?ScId=' + theAccessToken;


            $.ajax({
                url: _url, data: _data, async: false,
                complete: function (jqXHR, textStatus) {
                    window.location.reload(true);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showException(jqXHR, textStatus, errorThrown);
                }
            });


            }, "Are you sure?");
        }
        function OnChangePassword()
        {
            $("#newPswdForm").dialog("option", "title", 'Change Password').dialog({ position: { my: "top", at: "top", of: window } }).dialog("open");
        }
        function OnChangePasswordButton()
        {
            var value = $('#newPswd').val();
            if (value == '' || value == null || value == undefined) {
                alert('Πρέπει να ορίσετε νέο password!');
                return;
            }

            var _data = 'npt=' + escape($('#newPswd').val());
            _data = _data + '&UserId=<%=this.SelectedUser.UserId%>';
            var _url = theManagerPath + 'services/api/SystemUser/SetNewPswd?ScId=' + theAccessToken;


            $.ajax({
                url: _url, data: _data, async: false,
                complete: function (jqXHR, textStatus) {
                    $('#newPswdForm').dialog('close');
                    showInfoBand('Password Changed Succesfully!');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    showException(jqXHR, textStatus, errorThrown);
                }
            });
        }
        function OnDelete()
        {
            showDelete("Are you sure you want to delete this item?", function () {
                <%=GetDeleteButtonHandler%>;
            }, "Are you sure?");

            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-header">
        <span class="form-header-title">Edit '<%: this.SelectedUserName %>'</span>
        <a class="form-header-link" href="list.aspx">(back to users list)</a>
        <asp:Button ID="deleteBtn" runat="server" CssClass="form-button form-right-button delete-button" Text="Delete" OnClick="deleteBtn_Click" OnClientClick="return OnDelete();" formnovalidate="formnovalidate" />
        <asp:Button ID="saveBtn" runat="server" CssClass="form-button form-right-button save-button" Text="Save" OnClick="saveBtn_Click" />
        <%if(this.SelectedCredentials.IsLockedOut == true){ %>
            <input id="unlockUserBtn" type="button" value="Unlock User" class="form-button form-right-button unlock-button" onclick="OnUnlockUser()"/>
        <%} %>
        <input id="changePswdBtn" type="button" value="Change Password" class="form-button form-right-button pswd-button" onclick="OnChangePassword()"/>
    </div>

    <uc1:usersform ID="usersform1" runat="server" />

    <div id="newPswdForm" class="inputForm" style="display: none">
        <div class="formWrapper">
            <div class="formRow" id="PositionWrapper">
                <div><label for="newPswd">New Password:</label><input type="text" id="newPswd" name="newPswd" required/></div>
            </div>
        </div>
    </div>

</asp:Content>
