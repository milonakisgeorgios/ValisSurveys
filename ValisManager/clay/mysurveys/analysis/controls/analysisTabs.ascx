<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="analysisTabs.ascx.cs" Inherits="ValisManager.clay.mysurveys.analysis.controls.analysisTabs" %>
<style type="text/css">
    table.analysis-tabs { border-collapse: collapse; border-spacing: 0; margin: 12px 0px 0px 0px; }
    .tab 
    { 
        padding: 4px; 
        width: 112px; height: 48px; 
        background-color: #c7c7c7; text-align: center;
        border-top-left-radius: 5px;
        border-top-right-radius: 5px;
    }
    .selected { background-color: #fff; }
    .tab a { text-decoration: none; color: #fff; font-size: 14px; display: inline-block; width: 90px; float: right; margin-top: 8px;}
    .tab.selected a { color: #000; font-weight: bold; }
    .tab[class=noselected]:hover { background-color: #b7b7b7; cursor: pointer;}

    .summary{
        background-image: url(/content/images/chart.png);
        background-repeat: no-repeat;
        background-position: left, top;
    }
    .responses{
        background-image: url(/content/images/person.png);
        background-repeat: no-repeat;
        background-position: left, top;
        width: 122px
    }
</style>
<table class="analysis-tabs">
    <tbody>
        <tr>
            <td class="<%=SummaryTabClass %> summary">
                <a href="<%=SummaryLink %>">Survey Summary</a>
            </td>
            <td class="<%=ResponsesTabClass %> responses">
                <a href="<%=ResponsesLink %>">Individual Responses</a>
            </td>
        </tr>
    </tbody>
</table>