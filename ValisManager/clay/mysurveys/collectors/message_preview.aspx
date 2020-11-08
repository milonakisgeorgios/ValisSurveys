<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="true" CodeBehind="message_preview.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.message_preview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style type="text/css">

        a.unscheduleLink {
            border: 1px solid #272727;
            background-color: #d81313;
            color: #fff;
            border-radius: 4px;
            padding: 3px;
            width: 140px;
            font-size: 14px;
            text-decoration: none;
            display: inline-block;
            text-align: center;
        }
        p.youCannotSchedule
        {
            color: #d81313;
            margin-left: 8px;
            width: 240px;
        }
        #detailsPreview {
            margin: 12px 0px 24px 0px;
            padding: 12px;
        }
        .panelTitle
        { 
            color: #87A32E;
            font-size: 1.2em;
            font-weight: bold;
            margin: 0 0 6px;
            padding: 0;
        }
        .panelLine
        {
            padding: 4px;
            font-size: 14px;
        }
        span.panelKey {
            display: inline-block; margin-right: 6px;font-size: 10px;
            width: 60px;
        }
        span.panelKey2 {
            display: inline-block; margin-right: 6px;font-size: 10px;
        }
        span.panelValue {
            font-size: 14px;
            font-weight: bold;
        }

        table.delivaryTable {
            border-collapse: collapse;
            vertical-align: top;
        }
        table.delivaryTable td
        {
            padding: 0px;
            margin: 0px;
            vertical-align: top;
        }
        table.delivaryTable div.panelLine{
            margin-left: 8px;
        }


        .messageContent
        {
            border: 1px solid #D6D6D6;
            border-radius: 8px;
            margin-left: -3px;
            padding: 15px 10px;
            font-family: 'Courier New', sans-serif;
        }
        .messageContent tr
        {
            line-height: 2em;
        }
        td.msgField
        {
            color: #333333;
            font-size: 12px;
            font-weight: bold;
            padding-right: 5px;
            text-align: right;
            white-space: nowrap;
            vertical-align: top;
        }
        td.msgValue 
        {
            vertical-align: top;
        }
        span#litTo, span#litFro,span#litSubject,span#litBody  {
            color: #333333;
            display: inline-block;
        }
        .highlight 
        {
            background-color: #f6e312;
        }

        .editLink {
            margin: 0px;
            font-size: 12px;
            font-weight: normal;
            color: #0077b5;
            cursor: pointer;
        }
        .rightButton
        {
            margin-top: 0px;
            margin-right: 24px;
        }
    </style>
    <script>

        function OnCancelMessage() {
            $.ajax({
                url: theManagerPath + 'services/api/Messages/GetById?ScId=' + theAccessToken + '&messageId=<%=this.MessageId %>', dataType: 'json',
                success: function (selectedMessage) {

                    showQuestion('Are you sure you want to unschedule this message, and move it into your drafts?', function () {

                        $.ajax({
                            url: theManagerPath + 'services/api/Messages/UnSchedule?ScId=' + theAccessToken + '&messageId=<%=this.MessageId %>', dataType: 'html',
                            success: function (data) {
                                window.location = 'messages.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&textslanguage=<%=this.TextsLanguage %>';
                            }
                        });
                    }, 'UnSchedule message');


                }
            });
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Preview Message</span>
    </div>

    <div id="detailsPreview">
        <table style="width: 100%">
            <tr>
                <td style="width: 50%; vertical-align: top;">
                    <div class="panelTitle">
                        Delivery Date
                        <%if (this.SelectedMessage.Status == Valis.Core.MessageStatus.Draft && this.SelectedMessage.IsContentOK && this.SelectedMessage.IsDeliveryMethodOK && this.IsPaymentValid)
                          { %>
                            <a title="edit message" class="editLink" id="scheduleButton" href="editMessage_schedule.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&MessageId=<%=this.MessageId %>&textslanguage=<%=this.TextsLanguage %>">edit</a>
                        <%} %>
                    </div>
                    <table class="delivaryTable">
                        <tr>
                            <td>
                                <div style="background-image: url(/content/images/datepicker-32.png); width:32px; height: 32px; margin-top: 8px;"></div>
                            </td>
                            <td>
                                <div class="panelLine"><span class="panelKey2"><%=GetIntentionHtml() %></span></div>
                                <div class="panelLine"><span class="panelValue"><%=GetScheduleHtml() %></span></div>
                                <%if(this.SelectedMessage.Status == Valis.Core.MessageStatus.Pending){ %>
                                    <a class="unscheduleLink" href="javascript:OnCancelMessage();">Cancel Delivery</a>
                                <%} else if(this.SelectedMessage.Status == Valis.Core.MessageStatus.Draft && (!this.SelectedMessage.IsContentOK)){ %>
                                    <p class="youCannotSchedule">You cannot schedule the message. Content is not ready!</p>
                                <%} else if(this.SelectedMessage.Status == Valis.Core.MessageStatus.Draft && (!this.SelectedMessage.IsDeliveryMethodOK)){ %>
                                    <p class="youCannotSchedule">You cannot schedule the message. Recipients are not ready!</p>
                                <%} else if(this.SelectedMessage.Status == Valis.Core.MessageStatus.Draft && (!this.IsPaymentValid)){%>
                                    <p class="youCannotSchedule"><%:this.PaymentValidationErrorMessage %></p>
                                <%} %>
                            </td>
                        </tr>
                    </table>

                </td>
                <td style="width: 10px;"></td>
                <td style="width: 50%; vertical-align: top;">
                    <div class="panelTitle">
                        Recipients
                        <%if(this.SelectedMessage.Status == Valis.Core.MessageStatus.Draft){ %>
                        <a title="edit message" class="editLink" id="recipientsButton" href="editMessage_recipients.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&MessageId=<%=this.MessageId %>&textslanguage=<%=this.TextsLanguage %>">edit</a>
                        <%} %>
                    </div>
                    <div class="panelLine indent1"><span class="panelKey">SEND TO</span><%=GetDeliveryMethodHtml() %></div>
                    <div class="panelLine indent1"><span class="panelKey">TOTAL</span><%=GetTotalRecipientsHtml() %></div>
                </td>
            </tr>
        </table>
    </div>
    
    <div id="contentPreview">
        <div class="panelTitle">
            Message Preview
            <%if(this.SelectedMessage.Status == Valis.Core.MessageStatus.Draft){ %>
            <a title="edit message" class="editLink" id="editButton" href="editMessage_content.aspx?surveyid=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&MessageId=<%=this.MessageId %>&textslanguage=<%=this.TextsLanguage %>">edit</a>
            <%} %>
        </div>
        <span class="hint">Below is a preview of your message.</span>
        <div class="messageContent">
            <%=GetMessageHtml() %>
        </div>
    </div>

</asp:Content>
