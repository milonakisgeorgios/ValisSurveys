<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="mypayments.aspx.cs" Inherits="ValisManager.clay.payments.mypayments" %>
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
            $('#chargesForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, resizable: false, width: 1048, height: 500, buttons: { 'Close': { text: '<%=Resources.Global.CommonDialogs_Btn_Close %>', id: 'closeFormBtn', click: function () { $(this).dialog("close"); } } } });

            var actionsFrmtr = function (cellvalue, options, rowObject) {
                _html = '';
                _html += '<a class="actionLinks" href="javascript:OnViewCharges(\''+rowObject.PaymentId+'\');"><img width="20" height="20" src="'+theManagerPath + 'content/images/view.gif" alt="Charges" title="View the charges for this payment."/></a>';
                return _html;
            };
            var restCreditsFormatter = function (cellvalue, options, rowObject) {
                
                return '<b style="font-size: 1.12em;">' + cellvalue + '</b>';
            }
            var OnLoadCompletePaymentGrid = function()
            {
                var cRows = this.rows.length;
                for (var iRow = 0; iRow < cRows; iRow++) {
                    var row = this.rows[iRow];
                    if ($(row).hasClass("jqgrow"))
                    {
                        /*Η εκτη κολώνα είναι η RestCredits, που φυλάει το υπόλοιπο*/
                        var rest = $(row.cells[6]).text();
                        if(rest !='0')
                        {
                            $(row).css('background', '#bfcfb5');
                        }
                        else
                        {

                        }
                    }
                }
            }

            $('#thePaymentsGrid').jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Payments/GetPaymentsView1?ScId=' + theAccessToken + '&ClientId=<%= this.ClientId%>',
                datatype: "json", jsonReader: { repeatitems: false, id: "PaymentId" },
                colNames: ['', 'PaymentDate', 'Total Credits', 'Reserved','Used', 'Balance', 'IsActive', 'Responses'],
                colModel: [
                        { name: 'actions', width: 50, sortable: false, align: 'center', formatter: actionsFrmtr },
                        { name: 'PaymentDate', index: 'PaymentDate', width: 170, align: 'center', formatter: 'date', formatoptions: { srcformat: 'ISO8601Long', newformat: 'd/m/Y' } },
                        { name: 'TotalCredits', sortable: false, align: 'center', width: 180, },
                        { name: 'QuantityReserved', sortable: false, align: 'center', width: 150 },
                        { name: 'UsedCredits', sortable: false, align: 'center', width: 150 },
                        { name: 'RestCredits', sortable: false, align: 'center', width: 150, formatter: restCreditsFormatter },
                        { name: 'IsActive', sortable: false, align: 'center', hidden: true, width: 140, formatter: isActiveFormatter },
                        { name: 'Responses', sortable: false, align: 'center', width: 140 }
                ],
                sortname: '<%=SortName %>', sortorder: '<%=SortOrder %>', page:<%=PageNumber %>, 
                rowNum: <%=RowNum %>, rowList: [],pager: "#thePaymentsPager", viewrecords: false,
                loadui: "block ", hoverrows:false , gridview: false, pginput: false,height: 'auto',
                loadComplete: OnLoadCompletePaymentGrid,
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#thePaymentsGrid', _xml, ts, er); }
            });

        });

        function OnViewCharges(paymentId)
        {
            loadChargesGrid(paymentId);
            $("#chargesForm").dialog("option", "title", 'Charges Details').dialog("open");
        }
        
        function loadChargesGrid(paymentId)
        {
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

            $('#theChargesGrid').jqGrid('GridUnload');
            $grid = $('#theChargesGrid');
            $grid.jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + 'services/api/Payments/GetCharges?ScId=' + theAccessToken + '&paymentId='+paymentId,
                datatype: "json", jsonReader: { repeatitems: false, id: "CollectorPaymentId" },
                colNames: ['Collector', 'Charge by', 'Reserved','Used', 'Responses','FirstCharge','LastCharge'],
                colModel: [
                        { name: 'CollectorTitle', sortable: false, width: 400, align: 'center', formatter: collectorFrmtr },
                        { name: 'CreditType', sortable: false, width: 80, formatter: resourceTypeFrmtr, align: 'center' },
                        { name: 'QuantityReserved', sortable: false, width: 90, align: 'center',formatter: quantityFrmtr },
                        { name: 'QuantityUsed', sortable: false, width: 90, align: 'center',formatter: quantityFrmtr },
                        { name: 'Responses', sortable: false, width: 80, align: 'center' },
                        { name: 'FirstChargeDt', sortable: false, width: 120, align: 'center' },
                        { name: 'LastChargeDt', sortable: false, width: 120, align: 'center' }
                ],
                sortname: 'CollectorPaymentId', sortorder: 'asc', page: 1, 
                rowNum: 64, rowList: [],viewrecords: false,
                loadui: "block ", hoverrows:false , gridview: false, height:'auto',
                beforeSelectRow: function (id) { return false; },
                loadError: function (_xml, ts, er) { OnJqGridLoadError('#theChargesGrid', _xml, ts, er); }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="pageTitle">
        <h1>Credits</h1>
        <a title="Charges" class="lightgreenbutton rightButton" id="gotoCharges" href="mycharges.aspx">Charges</a>
    </div>
    <div class="pageTools">

        

    </div>
    
    <uc1:balanceStatistics ID="balanceStatistics1" runat="server" />


    <div style="margin-top: 56px;">
        <table id="thePaymentsGrid"></table>
        <div id="thePaymentsPager"></div>
    </div>

    <div id="chargesForm" style="display: none;">

        <div style="margin-top: 24px;">
            <table id="theChargesGrid"></table>
            <div id="theChargesPager"></div>
        </div>
    </div>
</asp:Content>
