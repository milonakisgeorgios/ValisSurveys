<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="details-weblink.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.details_weblink" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <style type="text/css">
        #txtEmailLink {
            width: 660px;
            font-size: 14px;
            padding: 12px;
            border: 1px dashed #999999;
        }
        
        #txtHtmlCode {
            width: 660px;
            font-size: 14px;
            padding: 12px;
            border: 1px dashed #999999;
        }

        .emailLink, .htmlCode
        {
            background-color: #f1f1f1;
            padding: 12px 0px 24px 12px;
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

        function OpenChangeNameForm() {
            $("#changeNameForm").dialog({ position: { my: "bottom", at: "center", of: window } }).dialog("open");
        }
        function OnChangeNameButton() {
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

    <div class="form-paragraph-wrapper">
        <div class="form-paragraph emailLink">
            <div class="form-paragraph-title">WebLink:</div>
            <div class="form-paragraph-line hint">
                Copy, paste and email the web link below to your audience.
            </div>
            <div class="form-paragraph-line">
                <asp:TextBox ID="txtEmailLink" runat="server" ClientIDMode="Static" ReadOnly="True"></asp:TextBox>
            </div>
        </div>
    </div>
    
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph htmlCode">
            <div class="form-paragraph-title">HTML code:</div>
            <div class="form-paragraph-line hint">
                Copy and paste the HTML code below to add your Web Link to any webpage:
            </div>
            <div class="form-paragraph-line">
                <asp:TextBox ID="txtHtmlCode" runat="server" ClientIDMode="Static" ReadOnly="True" Rows="2" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
    </div>

    
    <div id="changeNameForm" class="valisInputForm" title="Change Collector Title" style="display: none">
        <div class="formWrapper">
            <div class="formRow" id="collectorNameWrapper">
                    <label for="collectorName">Collector Name:</label><input type="text" name="collectorName" id="collectorName" value="<%=this.SelectedCollector.Name %>" required/><%=GetRequiredIcon() %>
            </div>
        </div>
    </div>

</asp:Content>
