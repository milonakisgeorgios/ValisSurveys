<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/analysis/Analysis.master" AutoEventWireup="false" CodeBehind="summary.aspx.cs" Inherits="ValisManager.clay.mysurveys.analysis.summary" %>
<%@ Register src="controls/analysisTabs.ascx" tagname="analysisTabs" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <link href="summary.css" rel="stylesheet" />
    <link href="/External/paulkinzett-toolbar/jquery.toolbars.css" rel="stylesheet" />
    <link href="/External/paulkinzett-toolbar/bootstrap.icons.css" rel="stylesheet" />
    <script src="/scripts/highcharts/js/highcharts.js"></script>
    <script src="/External/paulkinzett-toolbar/jquery.toolbar.min.js"></script>
    <script src="summary.js"></script>
    <style>
        .icon-barschart
        {
            width: 16px;
            height: 16px;
            line-height: 16px;
            margin-top: 8px;
            background-position: 0      0;
            background-image: url("/content/images/bars-chart-16.png");
        }
        .icon-columnschart
        {
            width: 16px;
            height: 16px;
            line-height: 16px;
            margin-top: 8px;
            background-position: 0      0;
            background-image: url("/content/images/column-chart-16.png");
        }
        .icon-piechart
        {
            width: 16px;
            height: 16px;
            line-height: 16px;
            margin-top: 8px;
            background-position: 0      0;
            background-image: url("/content/images/pie-16.png");
        }
        .icon-percent
        {
            width: 16px;
            height: 16px;
            line-height: 16px;
            margin-top: 8px;
            background-position: 0      0;
            background-image: url("/content/images/percent-16.png");
        }
        .icon-datatable
        {
            width: 16px;
            height: 16px;
            line-height: 16px;
            margin-top: 8px;
            background-position: 0      0;
            background-image: url("/content/images/datatable-16.png");
        }
        .icon-hidezero
        {
            width: 16px;
            height: 16px;
            line-height: 16px;
            margin-top: 8px;
            background-position: 0      0;
            background-image: url("/content/images/HideZeroResponseOptions-16.png");
        }
    </style>
    <script>


    </script>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainAnalysisPanelHolder" runat="server">

    <div class="pageTools">
        <uc1:analysisTabs ID="analysisTabs1" runat="server" />
    </div>
    <div class="pageBody">
        
        <div id="summaryReport">


        </div>

    </div>
    <div id="user-toolbar-options1" style="display:none">
	    <a href="#" command="1" ><i class="icon-barschart"></i></a>
	    <a href="#" command="2" ><i class="icon-columnschart"></i></a>
	    <a href="#" command="3" ><i class="icon-piechart"></i></a>
	    <a href="#" command="4" ><i class="icon-percent"></i></a>
	    <a href="#" command="5" ><i class="icon-datatable"></i></a>
	    <a href="#" command="6" ><i class="icon-hidezero"></i></a>
    </div>
    <div id="user-toolbar-options2" style="display:none">
	    <a href="#" command="1" ><i class="icon-barschart"></i></a>
	    <a href="#" command="2" ><i class="icon-columnschart"></i></a>
	    <a href="#" command="4" ><i class="icon-percent"></i></a>
	    <a href="#" command="5" ><i class="icon-datatable"></i></a>
	    <a href="#" command="6" ><i class="icon-hidezero"></i></a>
    </div>
</asp:Content>
