<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manager.Master" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="ValisManager.manager.home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
    <script src="/scripts/highcharts/js/highcharts.js"></script>
    <style type="text/css">
        div.form-header
        {
            background-color: #e56920;
        }
        div.dashboard-wrapper
        {
            padding: 12px 0px 0px 0px;
            margin-left: auto;
            margin-right: auto;
            position: relative;
        }
        div.greeting
        {
            text-align: center;
        }

        div.systemStats {
            margin: 0px 0px 24px 12px;
            width: 460px;
            color: #04003a;
            padding: 2px;
            border-radius: 4px;
        }
        div.systemStats h2 {
            display: inline-block;
            width: 320px;
            font-size: 20px;
        }
        div.systemStats span.value {
            display: inline-block;
            font-size: 22px;
        }
        div.stats:nth-child(even)
        {
            padding: 4px;
            background-color: #dfe1f1;
            color: #04003a;
        }
        div.stats:nth-child(odd)
        {
            padding: 4px;
            background-color: #d5d6e2;
            color: #2f0000;
        }

        
        div.graph{
            margin: 36px 12px 24px 12px;
        }
    </style>
    <script>
        $(document).ready(function () {


            $.ajax({
                url: theManagerPath + 'services/api/Statistics/GetSystemDashboard?ScId=' + theAccessToken, dataType: 'json',
                success: function (data)
                {
                    $('#TotalClients').html(data.TotalClients);
                    $('#TotalClientLogins').html(data.TotalClientLogins);
                    $('#TotalSystemLogins').html(data.TotalSystemLogins);
                    $('#Surveys').html(data.TotalSurveys + '/' + data.TotalActiveSurveys);
                    $('#Collectors').html(data.TotalCollectors + '/' + data.TotalActiveCollectors);
                    $('#TotalMessages').html(data.TotalMessages);
                    $('#Recipients').html(data.TotalRecipients + '/' + data.TotalSentRecipients);
                    $('#TotalMessages').html(data.TotalMessages);
                    $('#TotalRecipients').html(data.TotalRecipients);
                    $('#TotalClicks').html(data.TotalClicks);

                    drawResponsesGraph(data);
                    drawClicksGraph(data);
                    drawMessagesGraph(data);
                    drawLoginsGraph(data);
                    drawLogsGraph(data);
                }
            });

        });


        function Get_xAxis_categories(tuples) {
            var categories = new Array();

            for (var i = 0; i < tuples.length; i++) {
                categories.push(tuples[i].Item1);
            }
            return categories;
        }
        function Get_data(tuples) {
            var data = new Array();

            for (var i = 0; i < tuples.length; i++) {
                data.push(tuples[i].Item2);
            }
            return data;
        }
        function Get_data2(tuples, futureColor) {
            var data = new Array();

            for (var i = 0; i < tuples.length; i++) {
                if (i > 21) {
                    var item = new Object();
                    item.color = futureColor;
                    item.y = tuples[i].Item2;
                    data.push(item);
                }
                else {

                    data.push(tuples[i].Item2);
                }
            }
            return data;
        }

        function drawResponsesGraph(data) {
            var _categories = Get_xAxis_categories(data.LastResponses);
            var _data = Get_data(data.LastResponses);

            $('#responsesGraph').highcharts({
                chart: { type: 'column', height: 440 },
                title: { text: 'responses activity' }, plotOptions: { series: { animation: false } },
                xAxis: { categories: _categories, labels: { rotation: -45, style: { fontSize: '9px', fontFamily: 'Verdana, sans-serif' } } },
                yAxis: {
                    allowDecimals: false,
                    title: { text: null },
                    tickInterval: data.LastResponsesTickInterval, min: 0, max: data.LastResponsesMax
                }, credits: { enabled: false },
                series: [{ name: 'Responses', data: _data, showInLegend: false, color: '#59c1ca' }]
            });
        }

        function drawClicksGraph(data) {
            var _categories = Get_xAxis_categories(data.LastClicks);
            var _data1 = Get_data(data.LastClicks);
            var _data2 = Get_data(data.LastClicksWithResponse);

            $('#clicksGraph').highcharts({
                chart: { height: 440 },
                title: { text: 'weblinks activity' }, plotOptions: { series: { animation: false } },
                xAxis: { categories: _categories, labels: { rotation: -45, style: { fontSize: '9px', fontFamily: 'Verdana, sans-serif' } } },
                yAxis: {
                    allowDecimals: false,
                    title: { text: null },
                    tickInterval: data.LastClicksTickInterval, min: 0, max: data.LastClicksMax
                }, credits: { enabled: false },
                series: [{ name: 'Clicks', type: 'column', data: _data1, showInLegend: false, color: '#e04427' }, { name: 'Responses', type: 'scatter', data: _data2, showInLegend: false, color: '#59c1ca' }]
            });
        }

        function drawMessagesGraph(data) {
            var _categories = Get_xAxis_categories(data.LastMessageRecipients);
            var _data = Get_data2(data.LastMessageRecipients, '#899c0d');

            $('#messagesGraph').highcharts({
                chart: { type: 'column', height: 440 },
                title: { text: 'email activity' }, plotOptions: { series: { animation: false } },
                xAxis: { categories: _categories, labels: { rotation: -45, style: { fontSize: '9px', fontFamily: 'Verdana, sans-serif' } } },
                yAxis: {
                    allowDecimals: false,
                    title: { text: null },
                    tickInterval: data.LastMessageRecipientsTickInterval, min: 0, max: data.LastMessageRecipientsMax
                }, credits: { enabled: false },
                series: [{ name: 'messages', data: _data, showInLegend: false, color: '#bcd514' }]
            });
        }

        function drawLoginsGraph(data)
        {
            var _categories = Get_xAxis_categories(data.LastLogins);
            var _data = Get_data(data.LastLogins);

            $('#loginsGraph').highcharts({
                chart: { type: 'column', height: 440 },
                title: { text: 'logins activity' }, plotOptions: { series: { animation: false } },
                xAxis: { categories: _categories, labels: { rotation: -45, style: { fontSize: '9px', fontFamily: 'Verdana, sans-serif' } } },
                yAxis: {
                    allowDecimals: false,
                    title: { text: null },
                    tickInterval: data.LastLoginsTickInterval, min: 0, max: data.LastLoginsMax
                }, credits: { enabled: false },
                series: [{ name: 'Logins', data: _data, showInLegend: false, color: '#d831ec' }]
            });
        }
        function drawLogsGraph(data) {
            var _categories = Get_xAxis_categories(data.LastLogRecords);
            var _data = Get_data(data.LastLogRecords);

            $('#logsGraph').highcharts({
                chart: { type: 'column', height: 440 }, plotOptions: { series: { animation: false } },
                title: { text: 'Errors & Warns activity' },
                xAxis: { categories: _categories, labels: { rotation: -45, style: { fontSize: '9px', fontFamily: 'Verdana, sans-serif' } } },
                yAxis: {
                    allowDecimals: false,
                    title: { text: null },
                    tickInterval: data.LastLogRecordsTickInterval, min: 0, max: data.LastLogRecordsMax
                }, credits: { enabled: false },
                series: [{ name: 'Errors-Warns', data: _data, showInLegend: false, color: '#be0e0e' }]
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="form-header">
        <span class="form-header-title">Recent System Activity</span>
    </div>

    <div class="dashboard-wrapper">
        <table style="width:1400px;">
            <tr>
                <td colspan="2">
                    <table>

                        <tr>
                            <td style="width:500px">

                                <div class="systemStats">
                                    <div class="panels stats">
                                        <h2>Clients</h2>
                                        <span class="value" id="TotalClients"></span>
                                    </div>
                                    <div class="panels stats">
                                        <h2>Client Logins</h2>
                                        <span class="value" id="TotalClientLogins"></span>
                                    </div>
                                    <div class="panels stats">
                                        <h2>System Logins</h2>
                                        <span class="value" id="TotalSystemLogins"></span>
                                    </div>
                                    <div class="panels stats">
                                        <h2>Surveys (total/active)</h2>
                                        <span class="value" id="Surveys"></span>
                                    </div>
                                    <div class="panels stats">
                                        <h2>Collectors (total/active)</h2>
                                        <span class="value" id="Collectors"></span>
                                    </div>
                                    <div class="panels stats">
                                        <h2>Messages (active)</h2>
                                        <span class="value" id="TotalMessages"></span>
                                    </div>
                                    <div class="panels stats">
                                        <h2>Emails (total/sent)</h2>
                                        <span class="value" id="Recipients"></span>
                                    </div>
                                    <div class="panels stats">
                                        <h2>Responses</h2>
                                        <span class="value" id="TotalRecipients"></span>
                                    </div>
                                    <div class="panels stats">
                                        <h2>Clicks</h2>
                                        <span class="value" id="TotalClicks"></span>
                                    </div>
                                </div>

                            </td>
                            <td style="width:900px">
                                <div class="graph">
                                    <div id="messagesGraph">

                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width:700px">
                    
                    <div class="graph">
                        <div id="clicksGraph">

                        </div>
                    </div>
                </td>
                <td style="width:700px">
                    
                    <div class="graph">
                        <div id="responsesGraph">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">                    
                    <div class="graph">
                        <div id="loginsGraph" style="width: 800px; margin: 0px auto 0px auto">
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">                    
                    <div class="graph">
                        <div id="logsGraph" style="width: 800px; margin: 0px auto 0px auto">
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        
        

    </div>
</asp:Content>
