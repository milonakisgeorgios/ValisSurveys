<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="details-email.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.details_email" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style>
        .overviewPanel {
            margin-top: 5em;
            font-family: Arial,Verdana;
        }
        .overviewPanelTitle
        {

        }
        .recipientsPanel, .messagesPanel {
            width: 300px;
        }
        .recipientsHeader, .messagesHeader
        {
            color: #87A32E;
            font-size: 1.2em;
            font-weight: bold;
            border-bottom: 1px solid #D6D6D6;
            margin-bottom: 8px;
            cursor: pointer;
        }
        
        .statisticsLine .label
        {    
            color: #797979;
            font-size: 1em;
            font-weight: normal;
            display: inline-block;
            width: 230px;
        }
        .statisticsLine .value
        {    
            color: #424242;
            font-size: 1em;
            font-weight: normal;
            padding-left: 10px;
        }
        .footer .label{
            color: #87A32E;
            font-size: 1.2em;
            font-weight: bold;
            display: inline-block;
            width: 230px;
        }
        .footer .value
        {    
            color: #87A32E;
            font-size: 1.2em;
            font-weight: bold;
            padding-left: 10px;
        }


        .form-readonly-value {
            font-size: 16px;
            padding-left: 6px;
        }
        a.editLink {
            font-size: 12px;
            text-decoration: none;
        }
        #collectorNameWrapper label {
            width: 120px;
        }
        #collectorNameWrapper input[type=text] {
            width: 300px;
        }

        label.form-paragraph-title
        {
            display: inline-block;
            width: 140px;
            line-height: 1.6em;
        }
    </style>
    <script>
        $(document).ready(function () {
            $('#changeNameForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, resizable: false, width: 580, height: 235, buttons: { '<%=Resources.Global.CommonDialogs_Btn_Save %>': OnChangeNameButton, '<%=Resources.Global.CommonDialogs_Btn_Cancel %>': function () { $(this).dialog("close"); } } });

        });

        function OpenChangeNameForm()
        {
            $("#changeNameForm").dialog({ position: { my: "bottom", at: "center", of: window } }).dialog("open");
        }
        function OnChangeNameButton()
        {
            var value = $('#collectorName').val();
            if (value == '' || value == null || value == undefined) {
                $("#collectorName").animateHighlight("#fd2525", 500);
                $('#collectorName').focus();
                return false;
            }

            var _data = 'collectorName=' + escape($('#collectorName').val());
            _data = _data + '&collectorId=<%=this.SelectedCollector.CollectorId %>';
            _data = _data + '&textsLanguage=<%=this.SelectedCollector.TextsLanguage %>';

            $.ajax({
                url: theManagerPath + 'services/api/Collectors/UpdateName?ScId=' + theAccessToken, data: _data, dataType: 'json',
                success: function (data) {
                    window.location = window.location.href;
                }
            });

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Collector Overview</span>
    </div>
    
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-line">
                <label class="form-paragraph-title">Collector Name:</label><label class="form-readonly-value"><%=this.SelectedCollector.Name %></label> <a class="editLink" href="javascript:OpenChangeNameForm()">edit</a>
            </div>
            <%if (ValisManager.Globals.UseCredits)
              { %>
            <div class="form-paragraph-line">
                <label class="form-paragraph-title">Payment Method:</label><label class="form-readonly-value"><%: PaymentMethod %></label>
            </div>
            <%} %>
            <div class="form-paragraph-line">
                <label class="form-paragraph-title">Responses:</label><label class="form-readonly-value"><%=this.SelectedCollector.Responses %></label>
            </div>
        </div>
    </div>


    <div class="overviewPanel">
        <div class="overviewPanelTitle"></div>
        <table style="width: 100%" border="1">
            <tr>
                <td style="width: 50%;vertical-align: top;">

                    <div class="recipientsPanel">
                        <div class="recipientsHeader" onclick="window.location='recipients.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&textslanguage=<%=this.TextsLanguage %>'">Recipients</div>
                        
                        <div class="statisticsLine"><span class="label">Sent</span><span class="value"><%=TotalRecipientsEmailed %></span></div>
                        <div class="horizontal_separator"></div>
                        <div class="statisticsLine"><span class="label">Responded</span><span class="value"><%=TotalRecipientsResponded %></span></div>
                        <div class="horizontal_separator"></div>

                        <div class="footer"><span class="label">TOTAL</span><span class="value"><%=TotalRecipients %></span></div>
                    </div>

                </td>
                <td style="width: 10px;"></td>
                <td style="vertical-align: top;">

                    <div class="messagesPanel">
                        <div class="messagesHeader" onclick="window.location='messages.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&textslanguage=<%=this.TextsLanguage %>'">Messages</div>
                        
                        
                        <div class="statisticsLine"><span class="label">Drafts</span><span class="value"><%=TotalDraftMessages %></span></div>
                        <div class="horizontal_separator"></div>
                        <div class="statisticsLine"><span class="label">Scheduled</span><span class="value"><%=TotalScheduledMessages %></span></div>
                        <div class="horizontal_separator"></div>


                        <div class="footer"><span class="label">TOTAL</span><span class="value"><%=TotalMessages %></span></div>
                    </div>

                </td>
            </tr>
        </table>
    </div>

    <div id="changeNameForm" class="valisInputForm" title="Change Collector Title" style="display: none">
        <div class="formWrapper">
            <div class="formRow" id="collectorNameWrapper">
                    <label for="collectorName">Collector Name:</label><input type="text" name="collectorName" id="collectorName" value="<%=this.SelectedCollector.Name %>" required/><%=GetRequiredIcon() %>
            </div>
        </div>
    </div>
    

</asp:Content>
