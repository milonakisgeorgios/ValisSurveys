<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CollectorNavigation.ascx.cs" Inherits="ValisManager.clay.controls.CollectorNavigation" %>
<style type="text/css">
    
    .sidebar {
        font-size: 14px;
        width: 200px;
        height: 360px;
    }
    .sidebar .menu {
        font-weight: bold;
        margin: 12px 0px 0px 20px;
    }
    
    .sidebar .menu li {
        border: 0 none;
        margin-bottom: 12px;
        padding: 0;    
        list-style-position: outside;
        list-style-type: none;
    }
    
    .sidebar a, .sidebar a:link, .sidebar a:visited, .sidebar a:active {
        color: #00898A;
    }
    
    .sidebar a.selected, .sidebar a.selected:active, .sidebar a.selected:link, .sidebar a.selected:visited {
        color: #333333;
        text-decoration: none;
    }

    .sidebar span.disabled
    {
        color: #c7c7c7;
    }
</style>
<div class="sidebar">
    <ul class="menu">
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
    </ul>
</div>