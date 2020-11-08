<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="recipients.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.recipients" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <script src="/External/jquery.jqGrid-4.6.0/js/i18n/grid.locale-en.js"></script>
    <script src="/External/jquery.jqGrid-4.6.0/js/jquery.jqGrid.min.js"></script>
    <style type="text/css">

        .actionsPanel
        {
            text-align:left;
            background-color: #f1f1f1;
            border-radius: 10px;
            padding: 12px 12px 5px;
            display: inline-table;
            margin-top: 12px;
        }
        .actionHeader
        {
            color: #6B6B6B;<a href="../../../Support/WebApi/Collectors/GetAll.cs">../../../Support/WebApi/Collectors/GetAll.cs</a>
            font-family: Arial,Verdana;
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 12px;
        }
        .actionsPanel a
        {
            margin: 0 12px 12px 16px;
            display: inline-block;
        }



        .statisticsPanel
        {
            border-top: medium none;
            margin: 0;
            padding: 0;
            background-color: #FFFFFF;
            width: 300px;
        }
        .statisticsHeader
        {
            color: #87A32E;
            font-size: 16px;
            font-weight: bold;
        }
        .totalsLine
        {    
            background-color: #EAEAE8;
        }
        .statisticsLine .label
        {    
            color: #797979;
            font-family: Arial,Verdana;
            font-size: 12px;
            font-weight: normal;
            display: inline-block;
            width: 230px;
        }
        .statisticsLine .value
        {    
            color: #424242;
            font-family: arial;
            font-size: 16px;
            font-weight: bold;
            padding-left: 10px;
        }
        .statisticsLine .valueSmall
        {    
            color: #A9A9A9;
            font-family: arial;
            font-size: 12px;
            padding-left: 10px;
        }

        #panListDetails
        {
            margin-top: 25px;
        }


        .listTitle {
            color: #87A32E;
            font-size: 16px;
            font-weight: bold;
        }



        div#recipientForm {
            font-size: 14px;
        }
        
        div#recipientForm .line{
            margin: 6px;
        }
        
        div#recipientForm .key{
            color: #4d5e18;
            display: inline-block;
            width: 130px;
            text-align: right;
            margin-right: 8px;
        }
        div#recipientForm .value{
            color: #272727;
        }
        
        div#recipientForm .section{
            margin: 32px 0px 16px 16px;
            text-align: center;
        }
        div#recipientForm .greybutton{
            margin-top: 8px;
        }

        .ui-jqgrid tr.ui-row-ltr td 
            { 
                border-bottom-style: solid; 
                border-bottom-color: #eee;
                border-right-style: none;}
        <!--
        .ui-jqgrid { border-right-width: 0px; border-left-width: 0px; }
        -->
        .ui-jqgrid .ui-jqgrid-view
        {
            font-size: .87em;
        }
        .ui-jqgrid tr.jqgrow td 
        {
            font-size: 1em;
            height: 16px;

        }
    </style>
    <script>
        var numberOfRecipients = <%=TotalRecipients%>;

        function column1formatter(cellvalue, options, rowObject)
        {
            var html = '';
            if(rowObject.IsOptedOut)
            {
                html += '<img width="16" height="16" src="'+theManagerPath + 'content/images/chk_green.gif" alt="" title=""/>';
            }
            else
            {
                html += '<img width="16" height="16" src="'+theManagerPath + 'content/images/chk_off.gif" alt="" title="Active"/>';
            }
            return html;
        }
        function column2formatter(cellvalue, options, rowObject)
        {
            var html = '';
            if(rowObject.IsSentEmail)
            {
                html += '<img width="16" height="16" src="'+theManagerPath + 'content/images/email_sent.gif" alt="" title=""/>';
            }
            else
            {
                html += '<img width="16" height="16" src="'+theManagerPath + 'content/images/email_off.gif" alt="" title="Not Sent"/>';
            }
            return html;
        }
        function column3formatter(cellvalue, options, rowObject)
        {
            var html = '';
            if(rowObject.HasPartiallyResponded)
            {
                if(rowObject.HasResponded)
                {
                    html += '<img width="16" height="16" src="'+theManagerPath + 'content/images/resp_full.gif" alt="" title=""/>';
                }
                else
                {
                    html += '<img width="16" height="16" src="'+theManagerPath + 'content/images/resp_part.gif" alt="" title=""/>';
                }
            }
            else
            {
                html += '<img width="16" height="16" src="'+theManagerPath + 'content/images/resp_no.gif" alt="" title="No Response"/>';
            }


            return html;
        }
        function emailFormatter(cellvalue, options, rowObject)
        {
            var html = '<a class="actionLinks" href="javascript:OnRecipientEdit(\''+rowObject.RecipientId+'\');">' + cellvalue + '</a>';
            return html;
        }
        

        function loadRecipientsGrid()
        {
            $grid = $('#theGrid');
            
            $grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Recipients/GetAll?ScId=' + theAccessToken + '&collectorId=<%=this.CollectorId%>',
                datatype: "json", jsonReader: { repeatitems: false, id: "RecipientId" },
                colNames: ["Opt-out","Send Msg","Responded","Email", "FirstName", "LastName"],
                colModel: [
                        { name: 'IsOptedOut', width: 70, align: 'center', sortable: true, formatter: column1formatter },
                        { name: 'IsSentEmail', width: 70, align: 'center', sortable: true, formatter: column2formatter },
                        { name: 'HasResponded', width: 70, align: 'center', sortable: true, formatter: column3formatter },
                        { name: 'Email', width: 240, align: 'left', sortable: true, formatter: emailFormatter },
                        { name: 'FirstName', width: 140, align: 'left', sortable: true },
                        { name: 'LastName', width: 140, align: 'left', sortable: true }
                ],
                sortname: 'Email', sortorder: 'asc', page: 1, 
                rowNum: 18, rowList: [18, 36, 54],pager: "#thePager", viewrecords: true,
                loadui: "block ", hoverrows:false , gridview: false, height:'auto',
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theGrid', _xml, ts, er); }
            });
        }
        $(document).ready(function () 
        {
            $('#recipientForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, resizable: false, width: 500, height: 400, buttons: { 'Close': { text: '<%=Resources.Global.CommonDialogs_Btn_Close %>', id: 'closeFormBtn', click: function () { $(this).dialog("close"); } } } });

            if(numberOfRecipients>0)
            {
                $('#panListDetails').show();
                loadRecipientsGrid();
            }
            else
            {
                $('#panListDetails').hide();
            }

            $('#btnRemoveFromList').click(OnBtnRemoveFromList);
            $('#btnAddManuallyResponse').click(OnBtnAddManuallyResponse);
        });

        function OnRecipientEdit(recipientId)
        {
            $.ajax({
                url: theManagerPath + 'services/api/Recipients/GetById?ScId=' + theAccessToken + '&recipientId=' + recipientId, dataType: 'json',
                success: function (selectedRecipient) 
                {
                    $('#recipientId', '#recipientForm').val(selectedRecipient.RecipientId);
                    if(selectedRecipient.IsSentEmail)
                        $('#mailedStatus', '#recipientForm').html('Mailed');
                    else
                        $('#mailedStatus', '#recipientForm').html('Not Mailed');
                    if(selectedRecipient.HasResponded)
                        $('#responseStatus', '#recipientForm').html('Responded');
                    else
                        $('#responseStatus', '#recipientForm').html('Not Responded');
                    $('#email', '#recipientForm').html(selectedRecipient.Email);
                    $('#firstName', '#recipientForm').html(selectedRecipient.FirstName);
                    $('#lastName', '#recipientForm').html(selectedRecipient.LastName);

                    if(selectedRecipient.HasResponded || selectedRecipient.HasPartiallyResponded)
                    {
                        //Δεν δείχνουμε το section για την προσθήκη manual response!
                        $('#addManualSection').hide();
                    }
                    else
                    {
                        $('#addManualSection').show();
                    }

                    $("#recipientForm").dialog("option", "title", 'Recipient Details').dialog({ position: { my: "center", at: "center", of: window } }).dialog("open");
                }
            });
        }
        function OnBtnRemoveFromList()
        {
            var recipientId =  $('#recipientId', '#recipientForm').val();
            
            
            $.ajax({
                url: theManagerPath + 'services/api/Recipients/GetById?ScId=' + theAccessToken + '&recipientId=' + recipientId, dataType: 'json',
                success: function (selectedRecipient) {

                    showDelete('Are you sure you want to delete the recipient ' + selectedRecipient.Email , function(){
                    
                        $.ajax({
                            url: theManagerPath + 'services/api/Recipients/Remove?ScId=' + theAccessToken + '&recipientId=' + recipientId, dataType: 'html',
                            success: function (data) {
                                $('#theGrid').trigger('reloadGrid');
                                $('#recipientForm').dialog('close');
                            }
                        });
                    }, 'Delete Recipient.');
                }
            });
        }

        function OnBtnAddManuallyResponse()
        {
            var recipientId =  $('#recipientId', '#recipientForm').val();
            
            $.ajax({
                url: theManagerPath + 'services/api/Recipients/GetSurveyRuntimeURL?ScId=' + theAccessToken + '&recipientId=' + recipientId, dataType: 'json',
                success: function (data) {
                    window.open(data.url,'_blank');
                    $('#recipientForm').dialog('close');
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Edit recipients</span>
    </div>

    <table style="width: 100%" border="1">
        <tr>
            <td style="width: 60%;">

                <div class="statisticsPanel">
                    <div class="statisticsHeader">Recipient Summary</div>
                    <div class="statisticsLine" style="border-bottom: 1px solid #e1e1e1;"><span class="label">Sent</span><span class="value"><%=TotalEmailedRecipients %></span></div>

                    <div class="statisticsLine" style="border-bottom: 1px solid #e1e1e1;"><span class="label">Unsent</span><span class="value"><%=TotalNotEmailedRecipients %></span></div>

                    <%if (this.TotalEmailedRecipients > 0)
                      { %>
                    <div class="statisticsLine" style="border-bottom: 1px solid #e1e1e1;"><span class="label">Unresponded</span><span class="value"><%=TotalNotRespondedRecipients %></span></div>

                    <div class="statisticsLine"><span class="label">Responded</span><span class="value"><%=TotalRespondedRecipients %></span></div>
                    <div class="statisticsLine" style="border-bottom: 1px solid #e1e1e1;"><span class="label">Partial/Complete</span><span class="valueSmall"><%=TotalPartiallyRespondeRecipients %>/<%=TotalFullRespondeRecipients %></span></div>

                    <div class="statisticsLine"><span class="label">Opted Out</span><span class="value"><%=TotalOptedOutRecipients %></span></div>
                    <div class="statisticsLine" style="border-bottom: 1px solid #e1e1e1;"><span class="label">Bounced</span><span class="value"><%=TotalBouncedRecipients %></span></div>
                    <%} %>

                    <div class="statisticsLine totalsLine"><span class="label">TOTAL</span><span class="value"><%=TotalRecipients %></span></div>
                </div>

            </td>
            <td style="width: 25px;"></td>
            <td style="vertical-align: top; text-align: right">
                <div class="actionsPanel">
                    <div class="actionHeader">Actions:</div>
                    <div><a href="recipientsAdd.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&textslanguage=<%=this.TextsLanguage %>">Add recipients</a></div>
                    <div><a href="recipientsRemove.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&textslanguage=<%=this.TextsLanguage %>">Remove recipients</a></div>
                    <div><a href="recipientsDownload.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&textslanguage=<%=this.TextsLanguage %>">Download recipients</a></div>
                </div>
            </td>
        </tr>
    </table>

    <div id="panListDetails" style="display: none;">
        <div class="listTitle">
            Recipients List:
        </div>
        
        <div>
            <table id="theGrid"></table>
            <div id="thePager"></div>
        </div>
    </div>

    <div id="recipientForm" style="display: none">
        <div class="line"><label class="key">Mailed Status:</label><span class="value" id="mailedStatus"></span></div>
        <div class="line"><label class="key">Response Status:</label><span class="value" id="responseStatus"></span></div>
        <div class="line"><label class="key">Email:</label><span class="value" id="email"></span></div>
        <div class="line"><label class="key">First Name:</label><span class="value" id="firstName"></span></div>
        <div class="line"><label class="key">Last Name:</label><span class="value" id="lastName"></span></div>
        <div class="section">
            <p>
                I'd like to remove this person from the list:
            </p>
            <a class="greybutton" id="btnRemoveFromList">Remove From List</a>
        </div>
        <div class="section" id="addManualSection">
            <p>
                I'd like to manually add a response for this person:
            </p>
            <a class="greybutton" id="btnAddManuallyResponse">Add Response</a>
        </div>
        <input type="hidden" id="recipientId" />
    </div>
</asp:Content>
