<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="editMessage_content.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.editMessage_content" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style type="text/css">

        .messageContent
        {
            font-size: 12px;
            font-family: 'Courier New', sans-serif;
        }

        .helpContent {
            border: 1px solid #3f3f3f;
            border-radius: 6px;
            width: 200px;
            padding: 6px;
            font-size: 14px;
            color: #fff;
            background-color: #3f3f3f;
        }

            .helpContent .requiredTag {
                margin-left: 18px;
                padding: 4px;
                background-color: #1a1a1a;
            }
            .helpContent .optionalTag {
                margin-left: 18px;
                padding: 4px;
            }

            div.ErrorMessageLocal{
                padding-left: 0px;
                padding-bottom: 5px;
                margin-top: 5px;
                margin-bottom: 0px;
                font-size: 14px;
                line-height: 16px;
                font-weight: bold;
                color: #aa0000;
            }

            div.InfoMessageLocal{
                padding-left: 0px;
                padding-bottom: 5px;
                margin-top: 5px;
                margin-bottom: 0px;
                font-size: 14px;
                line-height: 16px;
                font-weight: bold;
                color: #00898a;
            }
    </style>
    <script>
        <%if(this.SelectedMessage.IsSenderOK == false){ %>

        $(document).ready(function ()
        {
            $("#verifyDialog").dialog({ dialogClass: 'questionDialog fixed-dialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, width: 500, height: 280, buttons: { 'OK': { 'class': 'greenbutton', text: 'Yes, Verify My Address', id: 'VerifyformButton', click: OnButtonVerify }, 'Cancel': { text: 'No, Cancel', id: 'CancelFormButton', click: function () { $(this).dialog("close"); } } } });
            $("#verifyDialog").dialog("option", "title", '<span class="commonDialogTitle">Verify reply-to address?<span>');

            $("#afterVerifyDialog").dialog({ dialogClass: 'infoDialog fixed-dialog', autoOpen: false, modal: true, closeOnEscape: true, resizable: false, width: 500, height: 280, buttons: { 'OK': { 'class': 'lightgreenbutton', text: 'OK', id: 'OKformButton', click: OnButtonDone } } });
            $("#afterVerifyDialog").dialog("option", "title", '<span class="commonDialogTitle">Done!<span>');

        });

        function SendVerificationLink()
        {
            $('#verifyDialog').dialog("open");
        }


        function OnButtonVerify()
        {
            $.ajax({
                url: theManagerPath + 'services/api/Collectors/VerifySenderAddress?ScId=' + theAccessToken + '&messageId=<%=this.MessageId%>', dataType: 'html',
                success: function (data)
                {
                    $('#verifyDialog').dialog("close");
                    $('#afterVerifyDialog').dialog("open");
                }
            });
        }
        
        function OnButtonDone()
        {
            $('#afterVerifyDialog').dialog("close");
            window.location.reload(true);
        }

        <%} %>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Create Message</span>
    </div>
    <asp:Button ID="saveAndContinue" runat="server" Text="Save and Continue >>" CssClass="rightButton greenbutton" ClientIDMode="Static" OnClick="saveAndContinue_Click" />


    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                From
            </div>
            <%if(this.SelectedMessage.IsSenderOK == false){ %>
            <div class="ErrorMessageLocal">
                This reply address hasn't been verified yet.
                <a href="javascript:SendVerificationLink();">
                    Send verification link now
                </a>
            </div>
            <%} else {%>
            <div class="InfoMessageLocal">
                This reply address has been verified!
            </div>
            <%} %>
            <div class="form-paragraph-line">
                <asp:TextBox ID="txtReplyEmail" runat="server" MaxLength="128" Width="300px"  required="required"></asp:TextBox><%=GetRequiredIcon() %>
            </div>

        </div>
    </div>
    
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                Subject
            </div>
            <div class="form-paragraph-line">
                <asp:TextBox ID="txtSubject" runat="server" MaxLength="256" Width="460px"  required="required"></asp:TextBox><%=GetRequiredIcon() %>
            </div>
        </div>
    </div>
    
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                Message
            </div>
            <div class="form-paragraph-line">
                <table>
                    <tr>
                        <td style="vertical-align: top;">
                            <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine"  CssClass="messageContent" required="required" Height="240px" Width="460px"></asp:TextBox><%=GetRequiredIcon() %>
                        </td>
                        <td style="vertical-align: top;">
                            <div class="helpContent">
                                <p>Available Tags:</p>
                                <p class="requiredTag">[SurveyLink] (required)</p>
                                <p class="requiredTag">[RemoveLink] (required)</p>
                                <br />
                                <p class="optionalTag">[FirstName] (optional)</p>
                                <p class="optionalTag">[LastName] (optional)</p>
                                <p class="optionalTag">[Title] (optional)</p>
                                <p class="optionalTag">[CustomValue] (optional)</p>
                            </div>
                        </td>
                    </tr>
                </table>
                
            </div>
        </div>
    </div>

    <div id="verifyDialog" style="display:none;font-size: 13pt;z-index: 10002;">
        <div id="verifyDialog_Message">
            <div style='padding: 12px;'><p>In order to schedule a message, its reply-to address must be verified. To verify your reply-to address, click the button and we'll send a verification link to the address you'd like to use. You only need to do this once for this address.</p></div>
        </div>
    </div>
    <div id="afterVerifyDialog" style="display:none;font-size: 13pt;z-index: 10002;">
        <div id="afterVerifyDialog_Message">
            <div style='padding: 12px;'><p>We sent you a link to the email address you'd like to use. Go to your inbox and click the link so you can proceed with this reply-to address. If you don't see the email, check your spam folder.</p></div>
        </div>
    </div>
</asp:Content>
