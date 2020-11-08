<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="addCollector.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.addCollector" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .rightButton
        {
            float: right;
        }

        .helpPanel 
        {
            font-size: 12px;
            color: #333333;
            width: 700px;
            margin: 0px 0px 48px 0px;
        }
        .InfoContent 
        {
            margin-top: 28px;
            font-size:1em;
            padding: 0 20px;
        }
        .InfoBarTitle
        {
            font-size: 1.2em;
            font-weight: bold;
            color: #87A32E;
            margin: 8px 0px 12px 0px;
        }
        .InfoTable
        {
            border: 0px;
            border-collapse: separate; border-spacing: 4px;
        }
        .InfoTable td
        {
            padding: 4px;
        }
        .InfoTable label
        {
            display: block;
        }
    </style>
    <script type="text/javascript">

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="pageTitle">
        <h1><a class="back-link" href='<%=_UrlSuffix("/clay/mysurveys/mysurveys.aspx") %>'><%=this.Server.HtmlEncode(this.SelectedSurvey.Title) %></a></h1>
    </div>
    <div class="horizontal_separator2"></div>
    <div class="pageSubTitle">
        <h1>Add Collector</h1>
    </div>
    
    <div class="pageTools">
        <asp:Button ID="btnCreate1" runat="server" CssClass="greenbutton rightButton" Text="Continue" OnClick="btnCreate_Click" />
        <a title="Cancel" class="greybutton rightButton" id="cancelCollector" href="<%=CancelLink %>">Cancel</a>
    </div>

    <div class="InfoContent">
        <div class="helpPanel">
            <b>How do you want to collect responses for your survey?</b> Here you must define how you will communicate your survey and collect responses. Hence the name "collector". You can have one or more collectors, each one with its won behavior and setings.
        </div>
        <div class="InfoBarTitle">How Would You Like to Collect Responses?</div>
        <table class="InfoTable">
            <tr><td>
                <asp:RadioButton ID="rdbtnWebLink" runat="server" GroupName="CollectorType" /></td><td>
                    <asp:Label ID="Label2" runat="server" Text="Web Link&lt;br/&gt;Create a Web Link to send via email or post to your web site." AssociatedControlID="rdbtnWebLink"></asp:Label>
                    </td></tr>
            <tr><td>
                <asp:RadioButton ID="rdbtnEmailList" runat="server" GroupName="CollectorType" Checked="True"/></td><td>
                    <asp:Label ID="Label3" runat="server" Text="Email&lt;br/&gt;Create custom email invitations and track who responds in your list." AssociatedControlID="rdbtnEmailList"></asp:Label>
                    </td></tr>
            <!--
            <tr><td>
                <asp:RadioButton ID="rdbtnPopLink" runat="server" GroupName="CollectorType" /></td><td>
                    <asp:Label ID="Label4" runat="server" Text="Website&lt;br/&gt;Embed your survey on your website or display your survey in a popup window." AssociatedControlID="rdbtnPopLink"></asp:Label>
                    </td></tr>
            -->
        </table>

        <div class="InfoBarTitle">Enter a Name for this Collector:</div>
        <div>
            <asp:Label ID="Label1" runat="server" Text="Name:" AssociatedControlID="collectorName"></asp:Label><asp:TextBox ID="collectorName" runat="server" Width="260px" required="required" MaxLength="100"></asp:TextBox><%=GetRequiredIcon() %>
        </div>
        <%if (this.UseCredits && this.ShowCreditTypeSelector)
          { %>
        <div class="InfoBarTitle">Choose How Would You Like to be Charged:</div>
        <div>
            <asp:Label ID="lblCreditType" runat="server" Text="Pay"></asp:Label>&nbsp;
            <asp:DropDownList ID="frmCreditType" runat="server">
                <asp:ListItem Value="1">by each EMAIL you send</asp:ListItem>
                <asp:ListItem Value="2">by each RESPONSE you receive</asp:ListItem>
                <asp:ListItem Value="3">by each CLICK on a web link</asp:ListItem>
            </asp:DropDownList><%=GetRequiredIcon() %>
        </div>
        <%} %>
    </div>

    <div class="pageTools">
        <asp:Button ID="btnCreate2" runat="server" CssClass="greenbutton rightButton" Text="Continue" OnClick="btnCreate_Click" />
        <a title="Cancel" class="greybutton rightButton" id="cancelCollector2" href="<%=CancelLink %>">Cancel</a>
    </div>
</asp:Content>
