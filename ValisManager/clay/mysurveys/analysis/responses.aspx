<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/analysis/Analysis.master" AutoEventWireup="false" CodeBehind="responses.aspx.cs" Inherits="ValisManager.clay.mysurveys.analysis.responses" %>
<%@ Register src="controls/analysisTabs.ascx" tagname="analysisTabs" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style>
        div.pageTools {
        margin: 0px 12px 0px 12px;
        }
        div.pageBody {
            margin: 0px 12px 12px 12px;
            min-height: 500px;
        }
    </style>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainAnalysisPanelHolder" runat="server">

    <div class="pageTools">
        <uc1:analysisTabs ID="analysisTabs1" runat="server" />
    </div>
    <div class="pageBody">
        <p>Test</p>
    </div>
</asp:Content>
