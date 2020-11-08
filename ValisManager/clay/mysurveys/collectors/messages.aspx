<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="messages.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.messages" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <script src="/External/jquery.jqGrid-4.6.0/js/i18n/grid.locale-en.js"></script>
    <script src="/External/jquery.jqGrid-4.6.0/js/jquery.jqGrid.min.js"></script>
    <style type="text/css">
        a.actionLinks {
            text-decoration: none;
            margin-left: 6px;
            margin-right: 8px;
            color: #0077b5;
        }
        span.currentlySending
        {
            cursor: wait;
        }
        .panel
        {
            margin: 24px 0px 64px 12px;
        }

        .panelTitle 
        {
            color: #87A32E;
            font-size: 16px;
            font-weight: bold;
            margin: 0 0 6px;
            padding: 0;
        }

        .noRecords {
            font-size: 16px;
            font-weight: bold;
            margin: 12px;
            text-align: left;
            color: #999999;
        }

        
        
        .ui-jqgrid tr.ui-row-ltr td 
            { 
                border-bottom-style: solid; 
                border-bottom-color: #eee;
                border-right-style: none;}
        .ui-jqgrid { border-right-width: 0px; border-left-width: 0px; }
        .ui-jqgrid .ui-jqgrid-view
        {
            font-size: .87em;
        }
        .ui-jqgrid tr.jqgrow td 
        {
            font-size: 1em;
            height: 16px;

        }

        div.SuccessMessage {padding:10px;margin:15px 0 15px 0;background:#87a61a; font-size:12px;font-weight:bold;color:#fff;}
    </style>
    <script>

        function actionsFormatter(cellvalue, options, rowObject)
        {
            var actions = '';

            actions += '<a class="actionLinks" href="javascript:OnDeleteMessage(' + rowObject.Collector + ',' + rowObject.MessageId + ');">Delete</a>';

            return actions;
        }
        function subjectFormatter(cellvalue, options, rowObject)
        {
            if (rowObject.Status == /*MessageStatus.Preparing*/ 2 || rowObject.Status == /*MessageStatus.Prepared*/ 4 || rowObject.Status == /*MessageStatus.Executing*/ 5)
                return '<span class="currentlySending">' + cellvalue + '</span>';
            else
            {
                if(rowObject.IsSenderOK == false || rowObject.IsContentOK == false)
                {
                    return '<a class=\"actionLinks\" style=\"font-size: 14px;\" href="editMessage_content.aspx?surveyid=<%=this.Surveyid%>&collectorId=' + rowObject.Collector + '&messageId=' + rowObject.MessageId + '&textslanguage=<%=this.TextsLanguage %>" >' + cellvalue + '</a>';
                }
                else
                {
                    return '<a class=\"actionLinks\" style=\"font-size: 14px;\" href="message_preview.aspx?surveyid=<%=this.Surveyid%>&collectorId=' + rowObject.Collector + '&messageId=' + rowObject.MessageId + '&textslanguage=<%=this.TextsLanguage %>" >' + cellvalue + '</a>';
                }
            }
                
        }

        function OnDeleteMessage(collectorId, messageId) {
            $.ajax({
                url: theManagerPath + 'services/api/Messages/GetById?ScId=' + theAccessToken + '&messageId=' + messageId, dataType: 'json',
                success: function (selectedMessage) {

                    showDelete('Are you sure you want to delete the message ' + selectedMessage.Subject, function () {

                        $.ajax({
                            url: theManagerPath + 'services/api/Messages/Delete?ScId=' + theAccessToken + '&messageId=' + messageId, dataType: 'html',
                            success: function (data) {
                                //$('#theDraftGrid').trigger('reloadGrid');
                                window.location = window.location.href;
                            }
                        });
                    }, 'Delete message');

                }
            });
        }
        function loadDraftGrid() {
            $grid = $('#theDraftGrid');

            $grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Messages/GetDraftMessages?ScId=' + theAccessToken + '&collectorId=<%=this.CollectorId%>',
                datatype: "json", jsonReader: { repeatitems: false, id: "MessageId" },
                colNames: ["Subject", " Recipients?", "Content?", "Sender Verified?", "Scheduled?", ""],
                colModel: [
                        { name: 'Subject', width: '384', align: 'left', sortable: true, formatter: subjectFormatter },
                        { name: 'IsDeliveryMethodOK', width: 68, align: 'center', sortable: false, formatter: checkGreenFormatter },
                        { name: 'IsContentOK', width: 68, align: 'center', sortable: false, formatter: checkGreenFormatter },
                        { name: 'IsSenderOK', width: 90, align: 'center', sortable: false, formatter: checkGreenFormatter },
                        { name: 'IsScheduleOK', width: 68, align: 'center', sortable: false, formatter: checkGreenFormatter },
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFormatter }
                ],
                sortname: 'MessageId', sortorder: 'asc', page: 1,
                rowNum: 6, rowList: [], pager: "#theDraftPager", viewrecords: false,
                loadui: "block ", hoverrows: false, gridview: false, height: 'auto', pginput: false,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theDraftGrid', _xml, ts, er); }
            });
        }



        function actionsFormatter2(cellvalue, options, rowObject)
        {
            var _html = '';

            if (rowObject.Status == /*MessageStatus.Pending*/ 1)
                _html += '<a class="actionLinks" href="javascript:OnCancelMessage(' + rowObject.Collector + ',' + rowObject.MessageId + ');">Cancel</a>';

            if (rowObject.Status == /*MessageStatus.PreparingError*/ 3 || rowObject.Status == /*MessageStatus.ExecutingError*/ 6 || rowObject.Status == /*MessageStatus.Cancel*/ 9)
                _html += '<a class="actionLinks" href="javascript:OnDeleteMessage(' + rowObject.Collector + ',' + rowObject.MessageId + ');">Delete</a>';

            if (rowObject.Status == /*MessageStatus.ExecutedWithErrors*/ 8 && rowObject.SentCounter == 0)
                _html += '<a class="actionLinks" href="javascript:OnDeleteMessage(' + rowObject.Collector + ',' + rowObject.MessageId + ');">Delete</a>';

            return _html;
        }
        function scheduleFormatter(cellvalue, options, rowObject)
        {
            if (rowObject.Status == /*MessageStatus.Pending*/ 1)
                return 'Scheduled for ' + cellvalue;
            if (rowObject.Status == /*MessageStatus.Preparing*/ 2 || rowObject.Status == /*MessageStatus.Prepared*/ 4 || rowObject.Status == /*MessageStatus.Executing*/ 5)
                return 'Currently Mailing';
            return 'Mailed on ' + cellvalue;
        }
        
        function msgStatusFormatter(cellvalue, options, rowObject) {
            if (cellvalue == 0)
                return 'Draft';
            else if (cellvalue == 1)
                return 'Pending';
            else if (cellvalue == 2)
                return 'Preparing';
            else if (cellvalue == 3)
                return 'PreparingError';
            else if (cellvalue == 4)
                return 'Prepared';
            else if (cellvalue == 5)
                return 'Executing';
            else if (cellvalue == 6)
                return 'ExecutingError';
            else if (cellvalue == 7)
                return 'Executed';
            else if (cellvalue == 8)
                return 'ExecutedWithErrors';
        }
        function OnCancelMessage(collectorId, messageId) {
            $.ajax({
                url: theManagerPath + 'services/api/Messages/GetById?ScId=' + theAccessToken + '&messageId=' + messageId, dataType: 'json',
                success: function (selectedMessage) {

                    showQuestion('Are you sure you want to unschedule this message, and move it into your drafts?', function () {

                        $.ajax({
                            url: theManagerPath + 'services/api/Messages/UnSchedule?ScId=' + theAccessToken + '&messageId=' + messageId, dataType: 'html',
                            success: function (data) {
                                window.location = window.location.href;
                            }
                        });
                    }, 'UnSchedule message');


                }
            });
        }

        function loadReadyGrid() {
            $grid = $('#theGrid');

            $grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Messages/GetReadyMessages?ScId=' + theAccessToken + '&collectorId=<%=this.CollectorId%>',
                datatype: "json", jsonReader: { repeatitems: false, id: "MessageId" },
                colNames: ["Subject", "Send Date", "Sent","Status",""],
                colModel: [
                        { name: 'Subject', width: '350', align: 'left', sortable: true, formatter: subjectFormatter },
                        { name: 'ScheduledAt', width: '190', align: 'left', sortable: true, formatter: scheduleFormatter },
                        { name: 'SentCounter', width: '40', align: 'center', sortable: false },
                        { name: 'Status', width: '110', align: 'center', sortable: false, formatter: msgStatusFormatter },
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFormatter2 }
                ],
                sortname: 'MessageId', sortorder: 'asc', page: 1,
                rowNum: 6, rowList: [], pager: "#thePager", viewrecords: false,
                loadui: "block ", hoverrows: false, gridview: false, height: 'auto', pginput: false,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theGrid', _xml, ts, er); }
            });
        }

        $(document).ready(function ()
        {
            <%if (this.TotalDraftMessages > 0)
              {%>
                loadDraftGrid();
            <%}%>
             <%if (this.TotalNonDraftMessages > 0)
               {%>
                loadReadyGrid();
            <%}%>
        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Message Manager</span>
    </div>
    <a title="create new message" class="greenbutton rightButton" id="createMessage" href="editMessage_recipients.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&textslanguage=<%=this.TextsLanguage %>">+ Create Message</a>
    
    <%if(ShowVerifySenderSuccessMessage){ %>
        <div class="SuccessMessage">Your reply address has been verified!</div>
    <%} %>
    <div class="panel">
        <div class="panelTitle">Draft Messages</div>
        
            <%if (this.TotalDraftMessages > 0)
              {%>
        <div class="gridWrapper">
                <table id="theDraftGrid"></table>
                <div id="theDraftPager"></div>
        </div>
        <%} else { %>
        <div class="noRecords">There are not any Messages</div>
        <%} %>
    </div>
    
    <div class="panel">
        <div class="panelTitle">Sent/Scheduled Messages</div>
        <%if (this.TotalNonDraftMessages > 0)
               {%>
        <div class="gridWrapper">
                <table id="theGrid"></table>
                <div id="thePager"></div>
        </div>
        <%} else { %>
        <div class="noRecords">There are not any Messages</div>
        <%} %>
    </div>

</asp:Content>
