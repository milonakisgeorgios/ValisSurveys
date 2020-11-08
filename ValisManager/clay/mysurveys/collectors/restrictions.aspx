<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="restrictions.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.restrictions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style type="text/css">
   

    </style>

    <script>
        function OnClick_chkCutoffDate()
        {
            if($('#chkCutoffDate').is(':checked'))
            {
                $('#DateMaxOff').hide();
                $('#DateMaxOn').show();
            }
            else
            {
                $('#DateMaxOn').hide();
                $('#DateMaxOff').show();
            }
        }
        function OnClick_chkMaxResponse()
        {
            if ($('#chkMaxResponse').is(':checked'))
            {
                $('#ResponseMaxOff').hide();
                $('#ResponseMaxOn').show();
            }
            else
            {
                $('#ResponseMaxOn').hide();
                $('#ResponseMaxOff').show();
            }
        }
        function OnClick_chkPassEnabled()
        {
            if ($('#chkPassEnabled').is(':checked'))
            {
                $('#PasswordOff').hide();
                $('#PasswordOn').show();
            }
            else
            {
                $('#PasswordOn').hide();
                $('#PasswordOff').show();
            }
        }
        function OnClick_chkIPEnabled()
        {
            if ($('#chkIPEnabled').is(':checked'))
            {
                $('#IPOff').hide();
                $('#IPOn').show();
            }
            else
            {
                $('#IPOn').hide();
                $('#IPOff').show();
            }
        }

        $(document).ready(function ()
        {
            $('#chkCutoffDate').click(OnClick_chkCutoffDate);
            $('#chkMaxResponse').click(OnClick_chkMaxResponse);
            $('#chkPassEnabled').click(OnClick_chkPassEnabled);
            $('#chkIPEnabled').click(OnClick_chkIPEnabled);

            OnClick_chkCutoffDate();
            OnClick_chkMaxResponse();
            OnClick_chkPassEnabled();
            OnClick_chkIPEnabled();


            $('#txtCutdate').datepicker({
                buttonImage: "/content/images/datepicker-20.png",
                showOn: "both",
                dateFormat: "yy-mm-dd"
            });

            $('#txtMaxResponse').onlyUnSignedNumbers().maxLength(8);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Collector Restrictions</span>
    </div>
    <asp:Button ID="saveRestrictions" runat="server" Text="Save Restrictions" CssClass="rightButton greenbutton" ClientIDMode="Static" OnClick="saveRestrictions_Click" />

    <div id="panCutDate" class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <asp:CheckBox ID="chkCutoffDate" runat="server" ClientIDMode="Static" /><asp:Label ID="Label1" runat="server" Text="Set a Cutoff Date & Time" AssociatedControlID="chkCutoffDate"></asp:Label>
            </div>
            <div id="DateMaxOff" style="display: block" class="form-paragraph-line indent1">
                To stop collecting responses after a particular date &amp; time, click the checkbox above.
            </div>
            <div id="DateMaxOn" style="display: none" class="form-paragraph-line indent1">
                <table border="0" class="">
                    <tr><td colspan="3" style="width: 100%;">Set a cutoff date and time that the survey will stop accepting responses:</td></tr>
                    <tr>
                        <td><asp:TextBox ID="txtCutdate" runat="server" ClientIDMode="Static" MaxLength="10"></asp:TextBox></td>
                        <td></td>
                        <td>
                            <table>
                                <tr>
                                    <td><asp:DropDownList ID="ddlHour" runat="server"></asp:DropDownList></td>
                                    <td>:</td>
                                    <td><asp:DropDownList ID="ddlMinute" runat="server"></asp:DropDownList></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td><span class="hint" style="padding-right: 20px;">(date format: yyyy-mm-dd)</span></td>
                        <td></td>
                        <td><span class="hint"><%=TimeZoneNotice %></span></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    
    <div id="panMaxResponse" class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <asp:CheckBox ID="chkMaxResponse" runat="server" ClientIDMode="Static" /><asp:Label ID="Label2" runat="server" Text="Set a Max Response Count" AssociatedControlID="chkMaxResponse"></asp:Label>
            </div>
            <div id="ResponseMaxOff" style="display: block" class="form-paragraph-line indent1">
                To automatically close the survey after reaching a maximum response count, click the checkbox above. 
            </div>
            <div id="ResponseMaxOn" style="display: none" class="form-paragraph-line indent1">
                <table border="0" class="">
                    <tr><td class="maxResponseTd">Set the maximum number of responses that this collector will accept:</td></tr>
                    <tr><td class="maxResponseTd">
                        <asp:TextBox ID="txtMaxResponse" runat="server" MaxLength="8" ClientIDMode="Static" TextMode="SingleLine"></asp:TextBox>
                        &nbsp;
                        <asp:Label ID="lblMaxResponse" runat="server" CssClass="hint" Text="Enter a number greater than current response count of 0."></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <!--
    <div id="panPass" class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <asp:CheckBox ID="chkPassEnabled" runat="server" ClientIDMode="Static" /><asp:Label ID="Label3" runat="server" Text="Enable Password Protection" AssociatedControlID="chkPassEnabled"></asp:Label>
            </div>
            <div id="PasswordOff" style="display: block" class="form-paragraph-line indent1">
                To set a password and restrict access to your survey, click the checkbox above.
            </div>
            <div id="PasswordOn" style="display: none" class="form-paragraph-line indent1">
                <table class="">
                    <tr><td colspan="2" class="hintHeader">Enter a password to restrict access to your survey.</td></tr>
                    <tr>
                        <td class=""><asp:Label ID="Label7" runat="server" Text="Password:" AssociatedControlID="txtPassword"></asp:Label></td>
                        <td class="">
                            <asp:TextBox ID="txtPassword" runat="server" MaxLength="50"></asp:TextBox>
                            <span class="hint">(max 50 characters)</span>
                        </td>
                    </tr>
                    <tr><td class="hintHeader" colspan="2">You can customize the text that will be displayed on the "Password Required" page.</td></tr>
                    <tr>
                        <td class=""><asp:Label ID="Label6" runat="server" Text="Password Label:" AssociatedControlID="txtPassLabel"></asp:Label></td>
                        <td class="">
                            <asp:TextBox ID="txtPassLabel" runat="server" MaxLength="50"></asp:TextBox>
                            <span class="hint">(max 50 characters)</span>
                        </td>
                    </tr>
                    <tr>
                        <td class=""><asp:Label ID="Label5" runat="server" Text="Submit Button Label:" AssociatedControlID="txtSubmitLabel"></asp:Label></td>
                        <td class="">
                            <asp:TextBox ID="txtSubmitLabel" runat="server" MaxLength="50"></asp:TextBox>
                            <span class="hint">(max 50 characters)</span>
                        </td>
                    </tr>
                    <tr>
                        <td class=""><asp:Label ID="Label8" runat="server" Text="Password Required Message:" AssociatedControlID="txtPassRequiredMessage"></asp:Label></td>
                        <td class="">
                            <asp:TextBox ID="txtPassRequiredMessage" runat="server" MaxLength="2000" TextMode="MultiLine" Rows="5" Columns="40"></asp:TextBox>
                            <span class="hint">(max 2000 characters)</span>
                        </td>
                    </tr>
                    <tr>
                        <td class=""><asp:Label ID="Label9" runat="server" Text="Password Failed Message:" AssociatedControlID="txtPassFailedMessage"></asp:Label></td>
                        <td class="">
                            <asp:TextBox ID="txtPassFailedMessage" runat="server" MaxLength="2000" TextMode="MultiLine" Rows="5" Columns="40"></asp:TextBox>
                            <span class="hint">(max 2000 characters)</span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    -->

    <!--
    <div id="panIPList" class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">
                <asp:CheckBox ID="chkIPEnabled" runat="server" ClientIDMode="Static" /><asp:Label ID="Label4" runat="server" Text="Enable IP Blocking" AssociatedControlID="chkIPEnabled"></asp:Label>
            </div>
            <div id="IPOff" style="display: block" class="form-paragraph-line indent1">
                To block or allow respondents from a particular IP range, click the checkbox above.
            </div>
            <div id="IPOn" style="display: none" class="form-paragraph-line indent1">
                Not Implemented yet!
            </div>
        </div>
    </div>
    -->

</asp:Content>
