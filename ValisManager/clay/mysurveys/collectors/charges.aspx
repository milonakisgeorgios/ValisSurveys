<%@ Page Title="" Language="C#" MasterPageFile="~/clay/mysurveys/collectors/CollectorDetails.master" AutoEventWireup="false" CodeBehind="charges.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.charges" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
    <script src="/External/jquery.jqGrid-4.6.0/js/i18n/grid.locale-en.js"></script>
    <script src="/External/jquery.jqGrid-4.6.0/js/jquery.jqGrid.min.js"></script>
    <style type="text/css">
        
        .form-readonly-value {
            font-size: 16px;
            padding-left: 6px;
        }
        a.editLink {
            font-size: 12px;
            text-decoration: none;
        }
        
        .panel
        {
            margin: 48px 0px 64px 0px;
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
        var numberOfCharges = <%=TotalCollectorPayments%>;
        
        var paymentFormatter = function (cellvalue, options, rowObject) {
            return rowObject.PaymentDate+' ('+ rowObject.PaymentQuantity+')';
        }
        var actionsFormatter = function (cellvalue, options, rowObject) {
            var _html = '';
            //_html += "<a href=\"javascript:EditCollectorPayment(<%=this.Surveyid %>, <%=this.CollectorId %>, <%=this.TextsLanguage %>, " + rowObject.CollectorPaymentId + ")\" ><img border=\"0\" src=\"" + theManagerPath + "content/images/edititem.gif\" alt=\"edit\"/></a>";
            //_html += '&nbsp;';
            _html += "<a href=\"javascript:DeleteCollectorPayment(<%=this.Surveyid %>, <%=this.CollectorId %>, <%=this.TextsLanguage %>, " + rowObject.CollectorPaymentId + ")\" ><img border=\"0\" src=\"" + theManagerPath + "content/images/DeleteRed16.png\" alt=\"edit\"/></a>";
            return _html;
        };

        function loadCharges() 
        {
            
            $('#theGrid').jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Payments/GetChargesForCollector?ScId=' + theAccessToken + '&collectorId=<%=this.CollectorId%>',
                datatype: "json", jsonReader: { repeatitems: false, id: "CollectorPaymentId" },
                colNames: ["#","Payment","Reserved","Used", "FirstCharge", "LastCharge","IsActive","IsUsed",""],
                colModel: [
                        { name: 'UseOrder', width: 40, align: 'center', sortable: false },
                        { name: 'Payment', width: 200, align: 'center', sortable: false, formatter:paymentFormatter },
                        { name: 'QuantityReserved', width: 70, align: 'center', sortable: false },
                        { name: 'QuantityUsed', width: 70, align: 'center', sortable: false },
                        { name: 'FirstChargeDt', width: 96, align: 'center', sortable: false },
                        { name: 'LastChargeDt', width: 96, align: 'center', sortable: false },
                        { name: 'IsActive', width: 60, align: 'center', sortable: false, formatter: checkGreenFormatter },
                        { name: 'IsUsed', width: 60, align: 'center', sortable: false, formatter: checkGreenFormatter },
                        { name: 'Actions', width: 60, align: 'center', sortable: false, formatter: actionsFormatter }
                ],
                sortname: 'UseOrder', sortorder: 'asc', page: 1, 
                rowNum: 18, rowList: [18, 36, 54],pager: "#thePager", viewrecords: true,
                loadui: "block ", hoverrows:false , gridview: false, height:'auto',
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theGrid', _xml, ts, er); }
            });
        }

        $(document).ready(function () {

            if(numberOfCharges > 0)
            {
                loadCharges();
            }


        });


        function AddCollectorPayment(surveyid, collectorId, textslanguage) {
            window.location.replace('chargesAdd.aspx?surveyid=' + surveyid + '&collectorId=' + collectorId + '&textslanguage=' + textslanguage);
        }
        function EditCollectorPayment(surveyid, collectorId, textslanguage, collectorPaymentId)
        {
            window.location.replace('chargesEdit.aspx?surveyid=' + surveyid + '&collectorId=' + collectorId + '&textslanguage=' + textslanguage +'&collectorPaymentId='+ collectorPaymentId);
        }
        function DeleteCollectorPayment(surveyid, collectorId, textslanguage, collectorPaymentId)
        {
            $.ajax({
                url: theManagerPath + 'services/api/Payments/GetPaymentInfoForCollectorPayment?ScId=' + theAccessToken + '&collectorPaymentId=' + collectorPaymentId,
                dataType: 'json',
                success: function (data) {
                    
                    if(data.QuantityReserved == 0)
                    {
                        message = 'Are you sure you want to remove payment ' + data.Description + '?';

                        showDelete(message, function () {
                            $.ajax({
                                url: theManagerPath + 'services/api/Payments/RemoveFromCollector?ScId=' + theAccessToken + '&collectorPaymentId=' + collectorPaymentId, dataType: 'json',
                                success: function (data) {
                                    ReloadGrid('#theGrid'); 
                                }
                            });
                        }, 'Remove payment confirmation.');
                    }
                    else
                    {
                        message = 'You cannot remove the payment ' + data.Description + '. There are scheduled messages depending on it!';
                        showWarning(message,"Remove payment warning.");
                    }

                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="secondPageTitle">
        <span>Associated Payments</span>
    </div>
    <a title="add new payment" class="greenbutton rightButton" id="createCharge" href="javascript:AddCollectorPayment(<%=this.Surveyid %>, <%=this.CollectorId %>, <%=this.TextsLanguage %>)">+ Add Payment</a>
    
    <div class="form-paragraph-wrapper">
        <div class="form-paragraph">
            <div class="form-paragraph-line">
                <label class="form-paragraph-title">Payment Method:</label><label class="form-readonly-value"><%= this.PaymentMethod %></label>
            </div>
        </div>
    </div>


    <div class="panel">
        <div class="panelTitle">Associated Payments</div>
        
            <%if (this.TotalCollectorPayments > 0)
              {%>
        <div class="gridWrapper">
                <table id="theGrid"></table>
                <div id="thePager"></div>
        </div>
        <%} else { %>
        <div class="noRecords">There are not any Associated Payments</div>
        <%} %>
    </div>



</asp:Content>
