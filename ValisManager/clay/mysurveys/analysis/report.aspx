<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="report.aspx.cs" Inherits="ValisManager.clay.mysurveys.analysis.report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/scripts/jquery-2.1.1.min.js"></script>
    <script src="/scripts/highcharts/js/highcharts.js"></script>
    <link href="report.css" rel="stylesheet" />
    <script>
        var theViewId = '<%=this.ViewId %>';
        var theSurveyId = '<%=this.Surveyid %>';
        var theTextsLanguage = '<%=this.TextsLanguage %>';
        
    </script>
    <script src="report.js"></script>
</head>
<body>
    <form id="form1" runat="server">

    <div class="pageBody">
        <div class="PageHeaderFront"><span class="brandName">XCompany Surveys</span></div>

        <div id="summaryInfo">
            <h2>Survey</h2>
            <h1 class="title"><%=this.SelectedSurvey.Title %></h1>
            <p class="stamp">Report generated at <%=DateTime.Now.ToString() %></p>
        </div>

        <div id="summaryReport">


        </div>

    </div>

    </form>
</body>
</html>
