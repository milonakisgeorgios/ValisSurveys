<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="remove.aspx.cs" Inherits="ValisManager.clay.addressbook.remove" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            margin: 0 0 30px 0;
            padding: 0px;
            margin-left: 32px;
        }
        .form-paragraph
        {

        }
        .form-paragraph-title
        {
            font-size: 16px;
            font-weight: bold;
            margin: 0 0 6px;
            padding: 0;
        }

        .form-paragraph-line
        {
            line-height: 1.4em;
            font-size: 12px;
            color: #333333;
        }

        .indent1
        {
            margin-left: 20px;
        }

        .rightButton
        {
            float: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="pageTitle">
        <h1>Remove Contacts from “<%: this.SelectedClientList.Name %>”</h1>
        <a class="greybutton rightButton" id="returnButton" href="~/clay/addressbook/addressbook.aspx" runat="server">&lt;&lt;&nbsp;Back to Address Book</a>
    </div>

    <div id="optionsWrapper">
        <div class="optionsTitle">Which Contacts Should be Removed?</div>
    </div>
    
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <asp:RadioButton ID="rdbtnAll" runat="server" GroupName="TypeOfRemove" ClientIDMode="Static" /><asp:Label ID="lblbtnAll" runat="server" Text="Remove All Contacts" AssociatedControlID="rdbtnAll"></asp:Label>
            </div>
            <div class="form-paragraph-line indent1">Clear out all the contacts in your list.</div>
        </div>
    </div>
    
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <asp:RadioButton ID="rdbtnOptOut" runat="server" GroupName="TypeOfRemove" ClientIDMode="Static" /><asp:Label ID="lblbtnOptOut" runat="server" Text="Remove All Opted-Out Contacts" AssociatedControlID="rdbtnOptOut"></asp:Label>
            </div>
            <div class="form-paragraph-line indent1">Remove all opted-out contacts from this list.</div>
        </div>
    </div>
    
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <asp:RadioButton ID="rdbtnBounced" runat="server" GroupName="TypeOfRemove" ClientIDMode="Static" /><asp:Label ID="lblbtnBounced" runat="server" Text="Remove All Bounced Email Recipients" AssociatedControlID="rdbtnBounced"></asp:Label>
            </div>
            <div class="form-paragraph-line indent1">Remove all non-deliverable contacts from this list.</div>
        </div>
    </div>
    
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <asp:RadioButton ID="rdbtnDomain" runat="server" GroupName="TypeOfRemove" ClientIDMode="Static" /><asp:Label ID="lblbtnDomain" runat="server" Text="Remove All Contacts by Domain Name" AssociatedControlID="rdbtnDomain"></asp:Label>
            </div>
            <div class="form-paragraph-line indent1">Remove all contacts that match the domain name you enter the textbox below.</div>
            <div class="form-paragraph-line indent1"><span style="font-size: 14px; font-weight: bold; padding-right: 5px;">Emails@</span><asp:TextBox ID="txtDomainName" runat="server" Width="266px" onclick="document.getElementById('rdbtnDomain').checked = true;"></asp:TextBox></div>
        </div>
    </div>

    
        <div class="form-paragraph-wrapper">
            <div class="form-paragraph">
                <a class="greybutton" id="btnCancel" href="~/clay/addressbook/addressbook.aspx" runat="server">Cancel</a>
                <asp:Button ID="btnRemoveContacts" runat="server" CssClass="greenbutton" Text="Remove Contacts" OnClick="btnRemoveContacts_Click" />
            </div>
        </div>

</asp:Content>
