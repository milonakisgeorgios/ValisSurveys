<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="recipientsRemove.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.recipientsRemove" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style type="text/css">

        .optionsTitle
        {
            color: #87A32E;
            font-size: 16px;
            font-weight: bold;
            margin: 0 0 18px;
            padding: 0;
        }

        .form-paragraph-wrapper
        {
            margin-left: 32px;
        }

        .form-paragraph-line
        {
            font-size: 12px;
            color: #333333;
        }
    </style>
    <script>
        function OnRemoveRecipients()
        {
            showQuestion('Are you sure you want to remove the recipients?', function ()
            {
                <%=GetRemoveButtonHandler%>;
            }, 'Are you sure?');

            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Remove recipients</span>
    </div>
    <a class="backToList rightButton" id="btnBackToList" href="recipients.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&textslanguage=<%=this.TextsLanguage %>"><< Back to Recipients</a>

    <div id="optionsWrapper">
        <div class="optionsTitle">Which Recipients to Remove? </div>

        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnUnsent" runat="server" GroupName="TypeOfRemove" ClientIDMode="Static" /><asp:Label ID="Label1" runat="server" Text="Remove All Unsent/New Recipients" AssociatedControlID="rdbtnUnsent"></asp:Label>
                </div>
                <div class="form-paragraph-line indent1">Remove all recipients that have not been sent a message yet.</div>
            </div>
        </div>
        
        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnOptOut" runat="server" GroupName="TypeOfRemove" ClientIDMode="Static" /><asp:Label ID="Label2" runat="server" Text=" 	Remove All Opted-Out Recipients" AssociatedControlID="rdbtnOptOut"></asp:Label>
                </div>
                <div class="form-paragraph-line indent1">Remove all recipients that have declined to receive further mailings. (Responded recipients will remain in list.)</div>
            </div>
        </div>
        
        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnBounced" runat="server" GroupName="TypeOfRemove" ClientIDMode="Static" /><asp:Label ID="Label3" runat="server" Text="Remove All Bounced Email Recipients" AssociatedControlID="rdbtnBounced"></asp:Label>
                </div>
                <div class="form-paragraph-line indent1">Remove all recipients with bounced email address. (Responded recipients will remain in list.)</div>
            </div>
        </div>
        
        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <div class="form-paragraph-title">
                    <asp:RadioButton ID="rdbtnDomain" runat="server" GroupName="TypeOfRemove" ClientIDMode="Static" /><asp:Label ID="Label4" runat="server" Text="Remove All Contacts by Domain Name" AssociatedControlID="rdbtnDomain"></asp:Label>
                </div>
                <div class="form-paragraph-line indent1">Remove all contacts that match the domain name you enter the textbox below. (Responded recipients will remain in list.)</div>
                <div class="form-paragraph-line indent1"><span style="font-size: 14px; font-weight: bold; padding-right: 5px;">Emails@</span><asp:TextBox ID="txtDomainName" runat="server" Width="266px" onclick="document.getElementById('rdbtnDomain').checked = true;"></asp:TextBox></div>
            </div>
        </div>

        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <a class="greybutton" id="btnCancel" href="recipients.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&textslanguage=<%=this.TextsLanguage %>">Cancel</a>
                <asp:Button ID="btnRemoveRecipients" runat="server" CssClass="greenbutton" Text="Remove Recipients" OnClick="btnRemoveRecipients_Click" OnClientClick="return OnRemoveRecipients();"/>
            </div>
        </div>
    </div>


</asp:Content>
