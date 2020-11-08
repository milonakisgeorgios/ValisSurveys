<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="home.aspx.cs" Inherits="ValisManager.clay.home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <script src="/scripts/highcharts/js/highcharts.js"></script>
    <style type="text/css">
        div.dashboard-wrapper
        {
            padding: 12px;

            margin-left: auto;
            margin-right: auto;
            position: relative;
            width: 974px;
            text-align: center;
        }
        div.greeting h1
        {
            color: #2e3645;
            font-size: 32px;
            font-weight: normal;
            line-height: 38px;
            margin: 0;
            padding: 0;
        }


        ul.panels {
            padding-left: 0;
            margin-bottom: 10px;
            display: inline-block;
            list-style: outside none none;
            margin-top: 38px;
            text-align: center;
        }
        ul.panels li h2 {
            border-bottom: 1px solid #f7f7f7;
            color: #8c9392;
            font-family: "Proxima N W15 Smbd",Helvetica,sans-serif;
            font-size: 11px;
            letter-spacing: 1px;
            line-height: 13px;
            margin: 5px 15px 0;
            padding: 7px 0;
            text-transform: uppercase;
        }
        ul.panels li {
            background: none repeat scroll 0 0 #ffffff;
            border: 1px solid rgba(0, 0, 0, 0.09);
            border-radius: 5px;
            box-shadow: 0 2px 0 0 rgba(0, 0, 0, 0.03);
            display: inline-block;
            height: 97px;
            list-style: outside none none;
            margin: 6px;
            min-width: 138px;
            padding: 0 10px;
            position: relative;
            vertical-align: top;
            z-index: 5;
            font-size: 36px;
        }         
        ul.panels li.surveys {
            color: #246201;
        }
        ul.panels li.collecors {
            color: #028690;
        }
        ul.panels li.responses {
            color: #59c1ca;
        }
        ul.panels li.messages {
            color: #bcd514;
        }
        ul.panels li.clicks {
            color: #e04427;
        }

        div.surveysStats
        {
            margin-bottom: 72px;
        }
        div.graph{
            margin: 72px 0px 72px 0px;
        }
    </style>
    <script>
        var clientId = <%=ValisManager.Globals.UserToken.ClientId.Value%>;


        function Get_xAxis_categories(tuples)
        {
            var categories = new Array();

            for(var i=0; i < tuples.length; i++)
            {
                categories.push(tuples[i].Item1);
            }
            return categories;
        }
        function Get_data(tuples)
        {
            var data = new Array();

            for(var i=0; i < tuples.length; i++)
            {
                data.push(tuples[i].Item2);
            }
            return data;
        }
        function Get_data2(tuples, futureColor)
        {
            var data = new Array();

            for(var i=0; i < tuples.length; i++)
            {
                if(i>21){
                    var item = new Object();
                    item.color = futureColor;
                    item.y=tuples[i].Item2;
                    data.push(item);
                }
                else {
            
                    data.push(tuples[i].Item2);
                }
            }
            return data;
        }


        function drawResponsesGraph(data)
        {
            var _categories = Get_xAxis_categories(data.LastResponses);
            var _data = Get_data(data.LastResponses);

            $('#responsesGraph').highcharts({
                chart: { type: 'column' },
                title: { text: 'responses activity' },
                xAxis: { categories: _categories, labels:{rotation: -45, style: {fontSize: '9px', fontFamily: 'Verdana, sans-serif' }} },
                yAxis: { 
                    allowDecimals: false,
                    title: { text: null },
                    tickInterval: data.LastResponsesTickInterval,min: 0, max: data.LastResponsesMax
                },credits: { enabled: true },
                series: [{name: 'Responses', data: _data, showInLegend: false, color: '#59c1ca' }]
            });
        }

        function drawClicksGraph(data)
        {
            var _categories = Get_xAxis_categories(data.LastClicks);
            var _data1 = Get_data(data.LastClicks);
            var _data2 = Get_data(data.LastClicksWithResponse);

            $('#clicksGraph').highcharts({
                title: { text: 'weblinks activity' },
                xAxis: { categories: _categories, labels:{rotation: -45, style: {fontSize: '9px', fontFamily: 'Verdana, sans-serif' }} },
                yAxis: { 
                    allowDecimals: false,
                    title: { text: null },
                    tickInterval: data.LastClicksTickInterval,min: 0, max: data.LastClicksMax
                },credits: { enabled: true },
                series: [{name: 'Clicks', type: 'column', data: _data1, showInLegend: false, color: '#e04427' },{name: 'Responses',type: 'scatter', data: _data2, showInLegend: false, color: '#59c1ca' }]
            });
        }

        function drawMessagesGraph(data)
        {
            var _categories = Get_xAxis_categories(data.LastMessageRecipients);
            var _data = Get_data2(data.LastMessageRecipients, '#899c0d');

            $('#messagesGraph').highcharts({
                chart: { type: 'column' },
                title: { text: 'email activity' },
                xAxis: { categories: _categories, labels:{rotation: -45, style: {fontSize: '9px', fontFamily: 'Verdana, sans-serif' }} },
                yAxis: { 
                    allowDecimals: false,
                    title: { text: null },
                    tickInterval: data.LastMessageRecipientsTickInterval,min: 0, max: data.LastMessageRecipientsMax
                },credits: { enabled: true },
                series: [{name: 'messages', data: _data, showInLegend: false, color: '#bcd514' }]
            });
        }


        $(document).ready(function ()
        {

            $.ajax({
                url: theManagerPath + 'services/api/Statistics/GetClientDashboard?ScId=' + theAccessToken + '&clientId=' + clientId, dataType: 'json',
                success: function (data)
                {
                    $('#surveysCount').html(data.TotalActiveSurveys);
                    $('#collectorsCount').html(data.TotalActiveCollectors);
                    $('#responsesCount').html(data.TotalResponses);
                    $('#messagesCount').html(data.TotalRecipients);
                    $('#clickCount').html(data.TotalClicks);

                    drawResponsesGraph(data);
                    drawClicksGraph(data);
                    drawMessagesGraph(data);
                }
            });


        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="dashboard-wrapper">

        <div class="greeting">
            <div>
                <h1>Hello! Here's your recent activity.</h1>
            </div>
        </div>
        <div class="surveysStats">
            <div>
                <ul class="panels stats">
                    <li class="surveys">
                        <h2>Active Surveys</h2>
                        <span id="surveysCount"></span>
                    </li>
                </ul>
                <ul class="panels stats">
                    <li class="collectors">
                        <h2>Active Collectors</h2>
                        <span id="collectorsCount"></span>
                    </li>
                </ul>
                <ul class="panels stats">
                    <li class="messages">
                        <h2>Messages</h2>
                        <span id="messagesCount"></span>
                    </li>
                </ul>
                <ul class="panels stats">
                    <li class="clicks">
                        <h2>Clicks</h2>
                        <span id="clickCount"></span>
                    </li>
                </ul>
                <ul class="panels stats">
                    <li class="responses">
                        <h2>Responses</h2>
                        <span id="responsesCount"></span>
                    </li>
                </ul>
            </div>
        </div>

        <div class="graph">
            <div id="responsesGraph">

            </div>
        </div>

        <div class="graph">
            <div id="clicksGraph">

            </div>
        </div>

        <div class="graph">
            <div id="messagesGraph">

            </div>
        </div>
    </div>

</asp:Content>
