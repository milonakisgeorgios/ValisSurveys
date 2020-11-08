<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="recipientsDownload.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.recipientsDownload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Download recipients</span>
    </div>
    <a class="backToList rightButton" id="btnBackToList" href="recipients.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&textslanguage=<%=this.TextsLanguage %>"><< Back to Recipients</a>


</asp:Content>
