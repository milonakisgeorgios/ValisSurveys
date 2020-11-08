<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="export.aspx.cs" Inherits="ValisManager.clay.addressbook.export" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .rightButton
        {
            float: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">    
    <div class="pageTitle">
        <h1>Download Contacts from “<%: this.SelectedClientList.Name %>”</h1>
        <a class="greybutton rightButton" id="returnButton" href="~/clay/addressbook/addressbook.aspx" runat="server">&lt;&lt;&nbsp;Back to Address Book</a>
    </div>
</asp:Content>
