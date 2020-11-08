<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditSurveyTabs.ascx.cs" Inherits="ValisManager.clay.controls.EditSurveyTabs" %>
<style type="text/css">
    table.edit-survey-tabs { border-collapse: collapse; border-spacing: 0; margin: 12px 0px 0px 0px; }
    .tab { padding: 4px; width: 9em; background-color: #fff; text-align: center;}
    .selected { background-color: #f1f1f1; }
    .tab a { text-decoration: none; color: #0077b5; font-size: .9em; }
    .tab.selected a { color: #0049a4; font-weight: bold; }
    .tab:hover { color: #1388d8; background-color: #c7c7c7; }
</style>
<table class="edit-survey-tabs">
    <tbody>
        <tr>
            <td class="<%=DesignTabClass %>">
                <a href="<%=DesignLink %>">Design Survey</a>
            </td>
            <td class="<%=EditTabClass %>">
                <a href="<%=EditLink %>">Survey Options</a>
            </td>
        </tr>
    </tbody>
</table>