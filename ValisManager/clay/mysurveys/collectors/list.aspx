<%@ Page Title="" Language="C#" MasterPageFile="~/clay/Default.Master" AutoEventWireup="false" CodeBehind="list.aspx.cs" Inherits="ValisManager.clay.mysurveys.collectors.list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/External/jquery.jqGrid-4.6.0/js/i18n/grid.locale-en.js"></script>
    <script src="/External/jquery.jqGrid-4.6.0/js/jquery.jqGrid.min.js"></script>
    <style type="text/css">
        
        div.pageTitle {
            background-image: url(/content/images/survey.png);
            background-position: 0px 4px;
            background-repeat: no-repeat;
            padding-left: 36px;
        }
        div.pageSubTitle {
            background-image: url(/content/images/collectors28.png);
            background-position: 0px 0px;
            background-repeat: no-repeat;
            padding-left: 36px;
        }
        a.actionLinks {
            text-decoration: none;
            margin-left: 0px;
            margin-right: 8px;
            color: #0077b5;
            font-size: .92em;
        }
        
        
        .ui-jqgrid tr.ui-row-ltr td { border-bottom-style: none; border-right-style: none;}

        .ui-jqgrid { border-right-width: 0px; border-left-width: 0px; }

        .ui-jqgrid .ui-jqgrid-view
        {
            font-size: .87em;
        }
        .ui-jqgrid tr.jqgrow td {font-size: 1.2em}
        
        .rightButton
        {
            float: right;
        }
    </style>
    <script type="text/javascript">
        var selectedSurveyId = <%=this.Surveyid %>;

        function resourceFormatter(cellvalue, options, rowObject)
        {
            if(cellvalue == 0)
                return "";
            
            if(cellvalue == 1)
                return "<span style=\"font-size: 0.82em;\">Email</span>";
            if(cellvalue == 2)
                return "<span style=\"font-size: 0.82em;\">Response</span>";
            if(cellvalue == 3)
                return "<span style=\"font-size: 0.82em;\">Click</span>";
        }
        function statusFormatter(cellvalue, options, rowObject)
        {
            if(cellvalue == 0)
                return '<div style="background: url(/content/images/orange-bullet.png) no-repeat; background-position: 8px 7px; color: #d59b00;">New</div>';
            if(cellvalue == 1)
                return '<div style="background: url(/content/images/green-bullet.png) no-repeat; background-position: 8px 7px; color: #869144;">Open</div>';
            if(cellvalue == 2)
                return '<div style="background: url(/content/images/red-bullet.png) no-repeat; background-position: 8px 7px; color: #942222;">Close</div>';

            return '';
        }
        function actionsFormatter(cellvalue, options, rowObject)
        {
            var actions = '';
            if(rowObject.CollectorType == /*WebLink*/0 || rowObject.CollectorType == /*Email*/1 || rowObject.CollectorType == /*Website*/2)
            {
                //actions += '<a class="actionLinks" href="details-weblink.aspx?surveyId='+rowObject.Survey+'&collectorId='+rowObject.CollectorId+'">Edit</a>';
                if(rowObject.Responses > 0 )
                {
                    actions += '<a class="actionLinks" href="javascript:OnClearCollector('+rowObject.Survey+','+rowObject.CollectorId+',' + rowObject.TextsLanguage+ ');">Clear</a>';
                }
                else
                {
                    actions += '<span style="color: #999999; display: inline-block; margin-right: 8px; font-size: 12px;" >Clear</span>';
                }                
                actions += '<a class="actionLinks" href="javascript:OnDeleteCollector('+rowObject.Survey+','+rowObject.CollectorId+',' + rowObject.TextsLanguage+ ');">Delete</a>';
            }

            return actions;
        }
        
        function nameFormatter(cellvalue, options, rowObject)
        {
            var html = '';
            if(rowObject.CollectorType == /*WebLink*/0)
            {
                html += '<a class="actionLinks" title="Edit the ' + rowObject.Name+'" href="details-weblink.aspx?surveyId='+rowObject.Survey+'&collectorId='+rowObject.CollectorId+'&textslanguage='+rowObject.PrimaryLanguage+'">'+ rowObject.Name+'</a>';
            }
            else if(rowObject.CollectorType == /*Email*/1)
            {
                html += '<a class="actionLinks" title="Edit the '+ rowObject.Name+'" href="details-email.aspx?surveyId='+rowObject.Survey+'&collectorId='+rowObject.CollectorId+'&textslanguage='+rowObject.PrimaryLanguage+'">'+ rowObject.Name+'</a>';
            }
            else if(rowObject.CollectorType == /*Website*/2)
            {
                html += '<a class="actionLinks" title="Edit the '+ rowObject.Name+'" href="details-website.aspx?surveyId='+rowObject.Survey+'&collectorId='+rowObject.CollectorId+'&textslanguage='+rowObject.PrimaryLanguage+'">'+ rowObject.Name+'</a>';
            }
            return html;
        }
        function supportedLanguagesFormatter(cellvalue, options, rowObject )
        {
            var tokens = cellvalue.split(",");
            var html = '';
            for(var i=0; i< tokens.length; i++)
            {
                if(tokens[i] == '')
                    continue;
                if(rowObject.CollectorType == /*WebLink*/0)
                {
                    html += '<a title="Design the '+GetLanguageName(tokens[i])+' version of '+rowObject.Name+'" href="details-weblink.aspx?surveyId='+rowObject.Survey+'&collectorId='+rowObject.CollectorId+'&textslanguage='+tokens[i]+'"><img href="javascript:OnEdit()" alt="'+GetLanguageName(tokens[i])+'" src="'+theManagerPath + 'content/' + GetLanguageIcon(tokens[i])+'" /></a>&nbsp;&nbsp;';
                }
                else if(rowObject.CollectorType == /*Email*/1)
                {
                    html += '<a title="Design the '+GetLanguageName(tokens[i])+' version of '+rowObject.Name+'" href="details-email.aspx?surveyId='+rowObject.Survey+'&collectorId='+rowObject.CollectorId+'&textslanguage='+tokens[i]+'"><img href="javascript:OnEdit()" alt="'+GetLanguageName(tokens[i])+'" src="'+theManagerPath + 'content/' + GetLanguageIcon(tokens[i])+'" /></a>&nbsp;&nbsp;';
                }
                else if(rowObject.CollectorType == /*Website*/2)
                {
                    html += '<a title="Design the '+GetLanguageName(tokens[i])+' version of '+rowObject.Name+'" href="details-website.aspx?surveyId='+rowObject.Survey+'&collectorId='+rowObject.CollectorId+'&textslanguage='+tokens[i]+'"><img href="javascript:OnEdit()" alt="'+GetLanguageName(tokens[i])+'" src="'+theManagerPath + 'content/' + GetLanguageIcon(tokens[i])+'" /></a>&nbsp;&nbsp;';
                }
            }
            return html;
        }

        $(document).ready(function () {

            $('#clearForm').dialog({ dialogClass: 'inputDialog', autoOpen: false, modal: true, resizable: false, width: 450, height: 235, buttons: { 'ClearResponses': { text: 'Yes, Clear Responses', id: 'ClearResponses', click: OnClearCollectorClicked }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'clearCancelBtn', click: function () { $(this).dialog("close"); } } } });
            
            
            $('#theGrid').jqGrid({
                ajaxGridOptions: { showGlobalAjaxError: false, showGlobalAjaxFuzz: false },
                url: theManagerPath + "services/api/Collectors/GetAll?ScId=" + theAccessToken +'&surveyId=' + selectedSurveyId,
                datatype: "json", jsonReader: { repeatitems: false, id: "CollectorId" },
                <%if (ValisManager.Globals.UseCredits)
                  {%>
                colNames: ["Name","Paid by","Status","Responses","Modified","edit",""],
                <%}else{%>
                colNames: ["Name","Status","Responses","Date Modified","edit",""],
                <%}%>
                colModel: [
                        { name: 'Name', width: 380, sortable: true, formatter: nameFormatter },
                        <%if (ValisManager.Globals.UseCredits)
                          {%>
                        { name: 'CreditType', width: 70, align: 'center', sortable: false, formatter: resourceFormatter },
                        <%}%>
                        { name: 'Status', width: 90, align: 'center', sortable: true, formatter: statusFormatter },
                        { name: 'Responses', width: 90, align: 'center', sortable: true },
                        { name: 'LastUpdateDT', width: 200, align: 'center', sortable: true },
                        { name: 'SupportedLanguagesIds', width: 90, align: 'center', sortable: false, formatter: supportedLanguagesFormatter },
                        { name: 'Actions', width: 130, sortable: false, align: 'right', formatter: actionsFormatter }
                ],
                pager: "#thePager", page:<%=PageNumber %>, sortname: '<%=SortName %>', sortorder: '<%=SortOrder %>', rowNum: <%=RowNum %>, rowList: [],
                loadui: "block ", viewrecords: false,hoverrows:false , gridview: false, pginput: false,
                loadError: function (xhr, status, error) {
                    OnjqGridLoadError('#theGrid', xhr, status, error)
                },
                beforeSelectRow: function(rowid, e) {
                    return false;
                }
            });


        });

        function OnDeleteCollector(surveyId, collectorId, textsLanguage)
        {
            $.ajax({
                url: theManagerPath + 'services/api/Collectors/GetById?ScId=' + theAccessToken + '&collectorId=' + collectorId + '&textsLanguage=' + textsLanguage, dataType: 'json',
                success: function (selectedCollector) {

                    if(selectedCollector.Status == /*CollectorStatus.Open*/1)
                    {
                        showWarning('<div style="background-color: orange; color: #fff; padding: 6px; font-size: 1.2em;">You cannot delete an Open collector.</div>', 'Delete this collector and the responses');
                        return;
                    }
                    else if(selectedCollector.TotalResponses > 0)
                    {
                        showWarning('<div style="background-color: orange; color: #fff; padding: 6px; font-size: 1.2em;">You cannot delete a Collector with Responses.<br /><br />You must Clear the collector\' s Responses</div>', 'Delete this collector and the responses');
                        return;
                    }
                    else if(selectedCollector.HasUsedPayments == true)
                    {
                        showError('<div style="background-color: orange; color: #fff; padding: 6px; font-size: 1.2em;">You cannot delete a Collector who has been charged.<br /><b>One</b> or <b>more</b> Credits have been spent for this Collector.<br /><b>Check associated payments for more info</b>.</div>', 'Collector cannot be deleted');
                        return;
                    }
                    else
                    {
                        showDelete('Are you sure you want to delete this collector ' + selectedCollector.Name, function(){                    
                            $.ajax({
                                url: theManagerPath + 'services/api/Collectors/Delete?ScId=' + theAccessToken + '&collectorId=' + collectorId, dataType: 'html',
                                success: function (data) {
                                    $('#theGrid').trigger('reloadGrid');
                                }
                            });
                        }, 'Delete this collector and the responses');
                    }
                }
            });
        }


        function OnClearCollector(surveyId, collectorId, textsLanguage)
        {
            $.ajax({
                url: theManagerPath + 'services/api/Collectors/GetById?ScId=' + theAccessToken + '&collectorId=' + collectorId + '&textsLanguage=' + textsLanguage, dataType: 'json',
                success: function (selectedCollector) {

                    $('#clr_name').text(selectedCollector.Name);
                    $('#clr_creation').text(selectedCollector.CreateDT);
                    $('#clr_modified').text(selectedCollector.LastUpdateDT);
                    $('#clr_responses').text(selectedCollector.Responses);
                    $("#clearForm").dialog({ position: { my: "bottom", at: "center", of: window } }).attr("collectorId", collectorId).dialog("open");

                }
            });
        }
        function OnClearCollectorClicked()
        {
            var collectorId = $('#clearForm').attr("collectorId");

            $.ajax({
                url: theManagerPath + 'services/api/Collectors/ClearResponses?ScId=' + theAccessToken + '&collectorId=' + collectorId, dataType: 'json',
                success: function (selectedCollector) {
                    $('#theGrid').trigger('reloadGrid');
                    $('#clearForm').dialog('close');
                },
                error: function(data){
                    $('#clearForm').dialog('close');
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHolder" runat="server">
    <div class="pageTitle">
        <h1><a class="back-link" href='<%=_UrlSuffix("/clay/mysurveys/mysurveys.aspx") %>'><%=this.Server.HtmlEncode(this.SelectedSurvey.Title) %></a></h1>
    </div>
    <div class="horizontal_separator2"></div>
    <div class="pageSubTitle">
        <h1>Collectors</h1>
        <a title="Add collector" class="greenbutton rightButton" id="createCollector" href="addCollector.aspx?surveyid=<%=Surveyid %>">+ Add Collector</a>
    </div>
    <div class="pageTools">
    </div>
    <table id="theGrid"></table>
    <div id="thePager"></div>

    <div id="clearForm" title="Delete all responses from this collector" style="display: none">
        <div style="font-size: 1.2em; font-weight: bold; color: #b0bf5a; margin-bottom: 8px;">Are you sure you want to clear the responses?</div>
        <div><label style="font-weight: bold; display:inline-block; width: 160px;">Collector Name:</label><span id="clr_name"></span></div>
        <div><label style="font-weight: bold; display:inline-block; width: 160px;">Date Created:</label><span id="clr_creation"></span></div>
        <div><label style="font-weight: bold; display:inline-block; width: 160px;">Date Modified:</label><span id="clr_modified"></span></div>
        <div><label style="font-weight: bold; display:inline-block; width: 160px;">Number of Responses:</label><span id="clr_responses"></span></div>
    </div>
</asp:Content>
