<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="mycharges.aspx.cs" Inherits="ValisManager.clay.payments.mycharges" %>
<%@ Register src="../controls/balanceStatistics.ascx" tagname="balanceStatistics" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/External/jquery.jqGrid-4.6.0/js/i18n/grid.locale-en.js"></script>
    <script src="/External/jquery.jqGrid-4.6.0/js/jquery.jqGrid.min.js"></script>
    <style type="text/css">
        div.pageTitle {
            background-image: url(/content/images/coins.png);
            background-position: 0px 5px;
            background-repeat: no-repeat;
            padding-left: 36px;
        }
        
        .rightButton
        {
            float: right;
        }


    </style>
    <script>

        $(document).ready(function () {
            $('#paymentsForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, resizable: false, width: 1048, height: 500, buttons: { 'Close': { text: '<%=Resources.Global.CommonDialogs_Btn_Close %>', id: 'closeFormBtn', click: function () { $(this).dialog("close"); } } } });
            
            var resourceTypeFrmtr = function (cellvalue, options, rowObject) {
                if (cellvalue == '0')   return 'None';
                if (cellvalue == '1')   return 'emails';
                if (cellvalue == '2')   return 'responses';
                if (cellvalue == '3')   return 'clicks';
                return '??';
            };
            var collectorFrmtr = function (cellvalue, options, rowObject) {
                if(rowObject.Status == '0')
                    return '<div>'+rowObject.CollectorTitle+' ('+ rowObject.SurveyTitle+')</div>';
                if(rowObject.Status == '1')
                    return '<div style="background-color: #8fc38d; color: 151d15;font-size: 1.12em;"><span style="font-size: 1.12em;">'+rowObject.CollectorTitle+'</span> (<span style="font-size: .82em;">'+ rowObject.SurveyTitle+'</span>)</div>';
                if(rowObject.Status == '2')
                    return '<div style="background-color: #c3ab8d">'+rowObject.CollectorTitle+' ('+ rowObject.SurveyTitle+')</div>';
            };
            var actionsFrmtr = function (cellvalue, options, rowObject) {
                return '<a class="actionLinks" href="javascript:OnViewPayments(\''+rowObject.CollectorId+'\');"><img width="20" height="20" src="'+theManagerPath + 'content/images/view.gif" alt="Charges" title="View the charges for this payment."/></a>';
            };
            
            $('#theChargesGrid').jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Collectors/GetChargedCollectors?ScId=' + theAccessToken + '&ClientId=<%= this.ClientId%>',
                datatype: "json", jsonReader: { repeatitems: false, id: "CollectorId" },
                colNames: ['','Collector', 'Charge by', 'Responses','FirstCharge','LastCharge'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFrmtr },
                        { name: 'CollectorTitle', index: 'CollectorTitle', width: 440, align: 'center', formatter: collectorFrmtr },
                        { name: 'CreditType', sortable: false, width: 90, formatter: resourceTypeFrmtr, align: 'center' },
                        { name: 'Responses', sortable: false, width: 80, align: 'center' },
                        { name: 'FirstChargeDt', sortable: false, width: 120, align: 'center' },
                        { name: 'LastChargeDt', sortable: false, width: 120, align: 'center' }
                ],
                sortname: '<%=SortName %>', sortorder: '<%=SortOrder %>', page:<%=PageNumber %>, 
                rowNum: <%=RowNum %>, rowList: [],pager: "#theChargesPager", viewrecords: false,
                loadui: "block ", hoverrows:false , gridview: false, pginput: false,height: 'auto',
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theChargesGrid', _xml, ts, er); }
            });

        });

        function OnViewPayments(collectorId)
        {
            loadPaymentsGrid(collectorId);
            $("#paymentsForm").dialog("option", "title", 'Payment Details').dialog("open");
        }
        
        function loadPaymentsGrid(collectorId)
        {
            
            var quantityFrmtr = function(cellvalue, options, rowObject) {
                if(cellvalue == '0')
                    return '-';
                else if(cellvalue == '1')
                {
                    if(rowObject.CreditType =='1')
                        return '1 email';
                    else if(rowObject.CreditType =='2')
                        return '1 response';
                    else if(rowObject.CreditType =='3')
                        return '1 click';
                }
                else
                {
                    if(rowObject.CreditType =='1')
                        return cellvalue + ' emails';
                    else if(rowObject.CreditType =='2')
                        return cellvalue + ' responses';
                    else if(rowObject.CreditType =='3')
                        return cellvalue + ' clicks';
                }

                return cellvalue;
            };

            $('#thePaymentsGrid').jqGrid('GridUnload');
            $('#thePaymentsGrid').jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Collectors/GetCharges?ScId=' + theAccessToken + '&collectorId='+collectorId,
                datatype: "json", jsonReader: { repeatitems: false, id: "CollectorPaymentId" },
                colNames: ['Payment','QuantityReserved','QuantityUsed','FirstCharge','LastCharge'],
                colModel: [
                        { name: 'PaymentTitle', sortable: false, width: 220 },
                        { name: 'QuantityReserved', sortable: false, width: 100, align: 'center',formatter: quantityFrmtr },
                        { name: 'QuantityUsed', sortable: false, width: 100, align: 'center',formatter: quantityFrmtr },
                        { name: 'FirstChargeDt', sortable: false, width: 120, align: 'center' },
                        { name: 'LastChargeDt', sortable: false, width: 120, align: 'center' }
                ],
                sortname: 'CollectorPaymentId', sortorder: 'asc', page:1, 
                rowNum: 64, rowList: [],pager: "#thePaymentsPager", viewrecords: false,
                loadui: "block ", hoverrows:false , gridview: false, pginput: false,height: 'auto',
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#thePaymentsGrid', _xml, ts, er); }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="pageTitle">
        <h1>Charges</h1>
        <a title="Payments" class="lightgreenbutton rightButton" id="gotoPayments" href="mypayments.aspx">Credits</a>
    </div>
    <div class="pageTools">

    </div>
    
    <uc1:balanceStatistics ID="balanceStatistics1" runat="server" />


    
    <div style="margin-top: 56px;">
        <table id="theChargesGrid"></table>
        <div id="theChargesPager"></div>
    </div>

    
    <div id="paymentsForm" style="display: none;">
        <div style="margin-top: 24px;">
            <table id="thePaymentsGrid"></table>
            <div id="thePaymentsPager"></div>
        </div>
    </div>
</asp:Content>
