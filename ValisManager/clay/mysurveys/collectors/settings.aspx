<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="settings.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style type="text/css">
        
        .form-paragraph-line input
        {
            float: left;
            margin: 2px 4px 0 0;
            padding: 0;
        }
        .form-paragraph-line label
        {
            display: block;
            font-size: 0.92em;
            padding: 0 0 0 18px;
        }
    </style>
    <script>
        $(document).ready(function () {
            $('#DisplayInstantResults_0').bind('click', function () {
                $('#DisplayNumberOfResponses_Paragraph').hide();
            });
            $('#DisplayInstantResults_1').bind('click', function () {
                $('#DisplayNumberOfResponses_Paragraph').show();
            });

            if ($('#DisplayInstantResults_0').is(':checked') == true)
                $('#DisplayNumberOfResponses_Paragraph').hide();




            $('#CompletionMode_0').bind('click', function () {
                $('#OnCompletionURL_Paragraph').hide();
            });
            $('#CompletionMode_1').bind('click', function () {
                $('#OnCompletionURL_Paragraph').show();
            });

            if ($('#CompletionMode_0').is(':checked') == true)
                $('#OnCompletionURL_Paragraph').hide();

        });



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Collector Settings</span>
    </div>
    <asp:Button ID="saveSettings" runat="server" Text="Save Settings" CssClass="rightButton greenbutton" ClientIDMode="Static" OnClick="saveSettings_Click" />

    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">Allow Multiple Responses? </div>
            <%if(this.SelectedCollector.CollectorType == Valis.Core.CollectorType.Email){ %>
            <div class="form-paragraph-line">
                <label><b>No</b>, allow only one response per email address that is sent. </label>
            </div>
            <%} else { %>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="AllowMultipleResponses_0" runat="server" GroupName="AllowMultipleResponses" /><asp:Label ID="Label1" runat="server" Text="&lt;b&gt;No&lt;/b&gt;, allow only one response per computer." AssociatedControlID="AllowMultipleResponses_0"></asp:Label></div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="AllowMultipleResponses_1" runat="server" GroupName="AllowMultipleResponses" /><asp:Label ID="Label2" runat="server" Text="&lt;b&gt;Yes&lt;/b&gt;, allow multiple responses per computer -- Recommended for kiosks or computer labs." AssociatedControlID="AllowMultipleResponses_1"></asp:Label></div>
            <%} %>
        </div>
    </div>
    
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">Allow Responses to be Edited?</div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="EditResponseMode_0" runat="server" GroupName="EditResponseMode" /><asp:Label ID="Label3" runat="server" Text="&lt;b&gt;No&lt;/b&gt;, once a page in the survey is submitted, respondents cannot go back and change existing responses." AssociatedControlID="EditResponseMode_0"></asp:Label></div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="EditResponseMode_1" runat="server" GroupName="EditResponseMode" /><asp:Label ID="Label4" runat="server" Text="&lt;b&gt;Yes&lt;/b&gt;, respondents can go back to previous pages in the survey and update existing responses until the survey is finished or until they have exited the survey. After the survey is finished, the respondent will not  be able to re-enter the survey." AssociatedControlID="EditResponseMode_1"></asp:Label></div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="EditResponseMode_2" runat="server" GroupName="EditResponseMode" /><asp:Label ID="Label5" runat="server" Text="&lt;b&gt;Yes&lt;/b&gt;, respondents can re-enter the survey at any time to update their responses." AssociatedControlID="EditResponseMode_2"></asp:Label></div>
        </div>
    </div>
    <!--
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">Display Survey Results?</div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="DisplayInstantResults_0" runat="server" GroupName="DisplayInstantResults" ClientIDMode="Static" /><asp:Label ID="Label8" runat="server" Text="&lt;b&gt;No&lt;/b&gt;, do not display results." AssociatedControlID="DisplayInstantResults_0"></asp:Label></div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="DisplayInstantResults_1" runat="server" GroupName="DisplayInstantResults" ClientIDMode="Static" /><asp:Label ID="Label9" runat="server" Text="&lt;b&gt;Yes&lt;/b&gt;, display results after a respondent completes the survey." AssociatedControlID="DisplayInstantResults_1"></asp:Label></div>
            <div class="form-paragraph-line indent1" id="DisplayNumberOfResponses_Paragraph">
                <asp:CheckBox ID="DisplayNumberOfResponses" runat="server" /><asp:Label ID="LabelOfDisplayNumberOfResponses" runat="server" Text="Display the number of responses for each question and answer option." AssociatedControlID="DisplayNumberOfResponses"></asp:Label>
            </div>

        </div>
    </div>
    -->

    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">Survey Completion</div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="CompletionMode_0" runat="server" GroupName="CompletionMode" ClientIDMode="Static" /><asp:Label ID="Label10" runat="server" Text="&lt;b&gt;Close the window&lt;/b&gt;" AssociatedControlID="CompletionMode_0"></asp:Label></div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="CompletionMode_1" runat="server" GroupName="CompletionMode" ClientIDMode="Static" /><asp:Label ID="Label11" runat="server" Text="&lt;b&gt;Go to the following URL:&lt;/b&gt;" AssociatedControlID="CompletionMode_1"></asp:Label>
                <div class="form-paragraph-line indent1" id="OnCompletionURL_Paragraph">
                <asp:TextBox ID="OnCompletionURL" runat="server" Width="660px" Rows="2" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox><%=GetTranslatableIcon() %></div>
            </div>
        </div>
    </div>
    <!--
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">Display a Disqualification Page?</div>
        </div>
    </div>
    -->
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title">Use SSL encryption?</div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="UseSSL_0" runat="server" GroupName="UseSSL" /><asp:Label ID="Label12" runat="server" Text="&lt;b&gt;Disable SSL&lt;/b&gt; for this collector.  Doing so may help allow respondents behind a firewall to access your survey." AssociatedControlID="UseSSL_0"></asp:Label></div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="UseSSL_1" runat="server" GroupName="UseSSL" /><asp:Label ID="Label13" runat="server" Text="&lt;b&gt;Enable SSL&lt;/b&gt; for this collector.  Secure the survey and the responses between ??????????? and the respondent." AssociatedControlID="UseSSL_1"></asp:Label></div>
        </div>
    </div>

    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-title"><%=SaveIPAddressOrEmail_Title %></div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="SaveIPAddressOrEmail_0" runat="server" GroupName="SaveIPAddressOrEmail" ClientIDMode="Static" /><label for="SaveIPAddressOrEmail_0"><%=SaveIPAddressOrEmail_Option0 %></label></div>
            <div class="form-paragraph-line">
                <asp:RadioButton ID="SaveIPAddressOrEmail_1" runat="server" GroupName="SaveIPAddressOrEmail" ClientIDMode="Static" /><label for="SaveIPAddressOrEmail_1"><%=SaveIPAddressOrEmail_Option1 %></label></div>
        </div>
    </div>
</asp:Content>
