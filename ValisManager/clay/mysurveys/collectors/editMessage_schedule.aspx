<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="editMessage_schedule.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.editMessage_schedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style type="text/css">
        .form-paragraph-title {
            color: #333;
            font-size: 14px;
        }

        #panSchedule {
            margin-left: 20px;
            border: 1px solid #c7c7c7;
            border-radius: 6px;
            padding: 8px;
        }
        #panImmediately {
            margin-left: 20px;
            border: 1px solid #c7c7c7;
            border-radius: 6px;
            padding: 8px;
        }

        div.ButtonsArea {
            margin-top: 32px;
        }

        img.ui-datepicker-trigger
        {
            float: right;
        }
    </style>
    <script>
        function OnClick_rdButton()
        {
            if($('#rdbtnSchedule').is(':checked'))
            {
                $('#panSchedule').show();
                $('#panImmediately').hide();
            }
            else if ($('#rdbtnImmediately').is(':checked'))
            {
                $('#panSchedule').hide();
                $('#panImmediately').show();
            }
        }


        $(document).ready(function () {
            $('#rdbtnSchedule').click(OnClick_rdButton);
            $('#rdbtnImmediately').click(OnClick_rdButton);

            $('#txtDate').datepicker({
                buttonImage: "/content/images/datepicker-20.png",
                showOn: "both",
                buttonImageOnly: true,
                dateFormat: "yy-mm-dd"
            });

            OnClick_rdButton();
        });

        function OnSaveBtnSchedule()
        {
            return true;
        }
        function OnSaveBtnImmediately()
        {
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Schedule Message Delivery</span>
    </div>

    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <asp:RadioButton ID="rdbtnSchedule" runat="server" GroupName="Schedule" ClientIDMode="Static" /><asp:Label ID="Label1" runat="server" Text="Send the message at a future date" AssociatedControlID="rdbtnSchedule"></asp:Label>
            </div>
            <div id="panSchedule" style="display: none" >

                <table>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server" required="required" ClientIDMode="Static" MaxLength="10"></asp:TextBox>
                        </td>
                        <td style="width: 48px;">
                            &nbsp;
                        </td>
                        <td style="width: 30px;">
                            <asp:DropDownList ID="ddlHours" runat="server" Width="50"></asp:DropDownList></td>
                        <td style="width: 10px; text-align: center;">:</td>
                        <td style="width: 30px;">
                            <asp:DropDownList ID="ddlMinutes" runat="server" Width="50"></asp:DropDownList></td>
                        <td>
                            </td>
                    </tr>
                    <tr>
                        <td><span class="hint">(date format: yyyy-mm-dd)</span></td>
                        <td></td>
                        <td colspan="4">
                            <span class="hint">
                            <%=TimeZoneNotice %>
                            </span>
                        </td>

                    </tr>
                </table>

                <div class="ButtonsArea">
                    <asp:Button ID="btnSaveSchedule" runat="server" Text="OK" CssClass="greenbutton" OnClientClick="return OnSaveBtnSchedule();" OnClick="btnSaveSchedule_Click" />
                    <a class='greybutton' id='btnCancel1' href='message_preview.aspx?surveyId=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&messageId=<%=this.MessageId %>&textslanguage=<%=this.TextsLanguage %>' >Cancel</a>
                </div>
            </div>
        </div>
    </div>

    
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <asp:RadioButton ID="rdbtnImmediately" runat="server" GroupName="Schedule" ClientIDMode="Static" /><asp:Label ID="Label2" runat="server" Text="Send the message immediately" AssociatedControlID="rdbtnImmediately"></asp:Label>
            </div>
            <div id="panImmediately" style="display: none" >
                <div>
                    <h3>Few minutes delay possible</h3>
                    <p>Your email will be delivered at the next available time. There could be up to a five minute delay.</p>
                </div>
                
                <div class="ButtonsArea">
                    <asp:Button ID="btnSendImmediately" runat="server" Text="OK" CssClass="greenbutton" OnClientClick="return OnSaveBtnImmediately();" CausesValidation="False" OnClick="btnSendImmediately_Click" formnovalidate="formnovalidate"/>
                    <a class='greybutton' id='btnCancel2' href='message_preview.aspx?surveyId=<%=this.Surveyid %>&collectorId=<%=this.CollectorId %>&messageId=<%=this.MessageId %>&textslanguage=<%=this.TextsLanguage %>' >Cancel</a>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
