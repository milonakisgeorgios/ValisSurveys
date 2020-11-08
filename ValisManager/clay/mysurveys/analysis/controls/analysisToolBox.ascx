<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="analysisToolBox.ascx.cs" Inherits="ValisManager.clay.mysurveys.analysis.controls.analysisToolBox" %>
<style type="text/css">
    div#toolboxwrapper
    {
        width: 100%;
        height: 500px;
        padding: 0px;
        font-size: 12px;
    }

    .ui-accordion-header
    {
        font-size: 14px;
    }
    .ui-accordion .ui-accordion-content
    {
        padding: 0px;
    }

    div.toolstrip
    {
        background-color: #c7c7c7;
        padding: 5px;
    }


    div#FilterTypePanel
    {
        margin: 0px 0px 8px 0px;
        font-size: 12px;
    }
    
    div.filterpanel
    {
        background-color: #eaeae8;
        color: #333;
        font-size: 12px;
        text-shadow: 0 1px 0 #fff;
        padding: 0px 0px 6px 0px;
    }
    div.filterpanel div.title
    {
        border-bottom: 1px solid #ccc;
        padding: 11px;
        position: relative;
    }
    div.filterpanel div.title span
    {
        color: #666;
        font-size: 12px;
        font-weight: bold;
        text-shadow: 0 1px 0 #fff;
        text-transform: uppercase;
    }

    div.filterpanel div.body
    {

        font-size: 12px;
    }


    select#fltrQuestion
    {
        margin: 10px 0px 8px 11px;
    }


    div.filterRow 
    {
        text-align: left;
        border-bottom: 1px solid #ccc;
        padding: 2px 2px 2px 10px;
    }

    div#qnaDynamic
    {
        padding: 8px 0px 8px 8px;
    }
    div#qnaDynamic div.dynatitle
    {
        border-bottom: 1px solid #ccc;
        padding: 10px;
        position: relative;
    }
    div#qnaDynamic div.dynabody
    {
    }
    div#qnaDynamic div.dynabody div.filterOptions
    {
        padding: 8px;
    }
    div#qnaDynamic div.dynabody div.filterColumns
    {

    }


    div.filter-summary-wrapper
    {
        display:table;
        table-layout:fixed;
        border: 1px solid #c6c6ba;
        border-radius: 6px;
        margin: 5px;
        width: 95%;
        background-color: #ffffd1;
    }
    div.filter-summary
    {
        display:table-row;
    }
    div.filter-summary div.filter-status {
        width: 26px;
        background-color: #37b545;
        display:table-cell;
        cursor: pointer;
    }
    div.filter-summary div.filter-name {
        display:table-cell;
        font-size: 10px;
        padding-left: 6px;
        width: 160px;
        vertical-align: middle;
    }
    div.filter-summary div.filter-actions {
        display:table-cell;
        cursor: pointer;
        width: 30px;
    }
    div.filter-summary div.filter-clear
    {
        clear: both;
    }


    select#comparisonOperator 
    {
        padding: 4px;
    }
    input.userinput-integer, input.userinput-decimal, input.userinput-date
    {
        display: inline-block;
        width: 88px;
        padding: 4px;
    }

    input.userinput-integer:first-of-type, input.userinput-decimal:first-of-type, input.userinput-date:first-of-type
    {
        margin-left: 18px;
    }
    span.userinput-separator
    {
        display: inline-block;
        margin: 0px 3px 0px 3px;
    }

    input#fltrTimePeriodStart, input#fltrTimePeriodEnd
    {
        width: 90px;
    }
    span.toend
    {
        display: inline-block;
        margin: 0px 12px 0px 12px;
    }

    a.download-link
    {
        display: block;
        font-size: .72em;
        margin: 12px 0px 16px 4px;
    }
</style>
<script>
    var theViewId = '<%=this.ViewId %>';
    var theSurveyId = '<%=this.Surveyid %>';
    var theTextsLanguage = '<%=this.TextsLanguage %>';


    function GetQuestion(questionId)
    {
        for (var i = 0; i < questions.length; i++) {
            if (questions[i].Id == questionId) {
                return questions[i];
            }
        }
        return null;
    }
    function GetQuestionOption(questionId, optionId)
    {
        for (var i = 0; i < questions.length; i++) {
            if (questions[i].Id == questionId)
            {
                for (var j = 0; j < questions[i].options.length; j++)
                {
                    if (questions[i].options[j].Id == optionId)
                        return questions[i].options[j];
                }
                break;
            }
        }
        return null;
    }
    function GetQuestionOptionEx(question, optionId)
    {
        for (var j = 0; j < question.options.length; j++)
        {
            if (question.options[j].Id == optionId)
                return question.options[j];
        }
        return null;
    }
    function GetQuestionColumn(questionId, columnId)
    {
        for (var i = 0; i < questions.length; i++) {
            if (questions[i].Id == questionId)
            {
                for (var j = 0; j < questions[i].columns.length; j++)
                {
                    if (questions[i].columns[j].Id == columnId)
                        return questions[i].columns[j];
                }
            }
        }
        return null;
    }


    $(function () {
        $('#TheFiltersForm').dialog({ dialogClass: 'inputDialog fixed-dialog', autoOpen: false, modal: true, resizable: false, draggable: false, width: 520, buttons: { 'Apply': { class: 'lightgreenbutton', text: '<%=Resources.Global.CommonDialogs_Btn_Apply %>', id: 'ApplyFilterButton', click: OnApplyOrUpdateFilterBtn }, 'Cancel': { text: '<%=Resources.Global.CommonDialogs_Btn_Cancel %>', id: 'CancelAddFilterButton', click: function () { $(this).dialog("close"); } } } });
        $('#filterType').bind('change', OnFilterTypeChange);
        $('#filterType').on('keyup', function (event) {
            if (event.which == $.ui.keyCode.UP || event.which == $.ui.keyCode.DOWN) {
                $('#filterType').trigger("change");
            }
        });

        $('#fltrQuestion').bind('change', OnQuestionChange);
        $('#fltrQuestion').on('keyup', function (event) {
            if (event.which == $.ui.keyCode.UP || event.which == $.ui.keyCode.DOWN) {
                $('#fltrQuestion').trigger("change");
            }
        });

        $("#toolbox").accordion({
            collapsible: true,
            icons: { header: "ui-icon-circle-arrow-e", activeHeader: "ui-icon-circle-arrow-s" },
            heightStyle: "fill"
        });

        $('#fltrTotalResponseTime').onlyUnSignedIntegers();

        $('#fltrTimePeriodStart').datepicker({
            buttonImage: "/content/images/datepicker-20.png",
            showOn: "both",
            buttonImageOnly: true,
            dateFormat: "mm/dd/yy"
        });

        $('#fltrTimePeriodEnd').datepicker({
            buttonImage: "/content/images/datepicker-20.png",
            showOn: "both",
            buttonImageOnly: true,
            dateFormat: "mm/dd/yy"
        });
    });


    function ResetFiltersForm()
    {
        $('#filterType').val('0');
        $('#fltrQuestion').val('0');
        $('#qnaDynamic').html('');
        $('.filterpanel').hide();
        $('#ApplyFilterButton').prop("disabled", true).css('cursor', 'default');
    }
    function OnFilterAdd()
    {
        ResetFiltersForm();

        /*we open the TheFiltersForm:*/
        $('#AddFilterButton').button('option', 'label', '<%=Resources.Global.CommonDialogs_Btn_Create %>');
        $("#TheFiltersForm").dialog("option", "title", 'Add Filter:').dialog({ position: { my: "top", at: "top", of: window } }).attr("createNew", true).dialog("open");
    }
    function OnFilterEdit()
    {
        ResetFiltersForm();

        /*we open the TheQuestionForm:*/
        $('#AddFilterButton').button('option', 'label', '<%=Resources.Global.CommonDialogs_Btn_Update %>');
        $("#TheFiltersForm").dialog("option", "title", 'Edit Filter:').dialog({ position: { my: "top", at: "top", of: window } }).attr("createNew", false).dialog("open");
    }

    function OnFilterTypeChange(evt)
    {
        var selOption = $('#filterType option:selected').get(0);
        if (selOption != null && selOption != undefined)
        {
            var filterType = $('#filterType').val();
            if (filterType == null || filterType == undefined || filterType == '')
                return;

            $('#fltrQuestion').val('0');
            $('#qnaDynamic').html('');
            $('.filterpanel').hide();

            if (filterType == 1) { $('#QuestionAndAnswerPanel').show(); }
            else if (filterType == 2) { $('#CollectorPanel').show(); }
            else if (filterType == 3) { $('#ResponseTimePanel').show(); }
            else if (filterType == 4) { $('#TimePeriodPanel').show(); }

            if (filterType == 0)
                $('#ApplyFilterButton').prop("disabled", true).css('cursor', 'default');
            else
                $('#ApplyFilterButton').prop("disabled", false).css('cursor', 'pointer');
        }
    }

    function OnQuestionChange(evt)
    {
        $('#qnaDynamic').html('');

        var selOption = $('#fltrQuestion option:selected').get(0);
        if (selOption != null && selOption != undefined)
        {
            var questionId = $('#fltrQuestion').val();
            if (questionId == null || questionId == undefined || questionId == '' || questionId == 0 || questionId == '0')
                return;
            var questionType = $(selOption).attr('questiontype');

            //βρίσκουμε την ερώτησή μας απο τον πίνακα των ερωτήσεων:
            var selectedQuestion = GetQuestion(questionId);
            if (selectedQuestion != null)
            {
                window['Draw_' + selectedQuestion.type + '_Detail'](selectedQuestion);
            }
        }
    }

    function Draw_SingleLine_Detail(selectedQuestion)
    {
        console.log('Draw_SingleLine_Detail');
    }
    function Draw_MultipleLine_Detail(selectedQuestion)
    {
        console.log('Draw_MultipleLine_Detail');

    }
    function Draw_Integer_Detail(selectedQuestion)
    {
        console.log('Draw_Integer_Detail');
        var _html = '';
        _html += '<div class="dynatitle"><span>Q' + selectedQuestion.Id + ': ' + selectedQuestion.text + '</span></div>';
        _html += '<div class="dynabody">'
        _html += '<div class="filterRow">'
        _html += '<select id="comparisonOperator" name="comparisonOperator" onchange="OnComparisonOperatorChange(this)"><option value="1">Equals</option><option value="2">Greater</option><option value="3">Less</option><option value="4">GreaterOrEqual</option><option value="5">LessOrEqual</option><option value="6">NotEqual</option><option value="7">Between</option></select>'
        _html += '<input id="userinput1" name="userinput1" type="text" class="userinput-integer" maxlength="10"/>';
        _html += '<span class="userinput-separator">and</span>';
        _html += '<input id="userinput2" name="userinput2" type="text" class="userinput-integer" maxlength="10"/>';
        _html += '</div>';
        _html += '</div>';
        $('#qnaDynamic').html(_html);

        $('#userinput1').onlySignedIntegers();
        $('#userinput2').onlySignedIntegers();
        OnComparisonOperatorChange($('#comparisonOperator').get(0));
    }
    function Draw_Decimal_Detail(selectedQuestion)
    {
        console.log('Draw_Decimal_Detail');
        var _html = '';
        _html += '<div class="dynatitle"><span>Q' + selectedQuestion.Id + ': ' + selectedQuestion.text + '</span></div>';
        _html += '<div class="dynabody">'
        _html += '<div class="filterRow">'
        _html += '<select id="comparisonOperator" name="comparisonOperator" onchange="OnComparisonOperatorChange(this)"><option value="1">Equals</option><option value="2">Greater</option><option value="3">Less</option><option value="4">GreaterOrEqual</option><option value="5">LessOrEqual</option><option value="6">NotEqual</option><option value="7">Between</option></select>'
        _html += '<input id="userinput1" name="userinput1" type="text" class="userinput-decimal" maxlength="10"/>';
        _html += '<span class="userinput-separator">and</span>';
        _html += '<input id="userinput2" name="userinput2" type="text" class="userinput-decimal" maxlength="10"/>';
        _html += '</div>';
        _html += '</div>';
        $('#qnaDynamic').html(_html);

        $('#userinput1').onlySignedNumbers();
        $('#userinput2').onlySignedNumbers();
        OnComparisonOperatorChange($('#comparisonOperator').get(0));
    }
    function Draw_Date_Detail(selectedQuestion)
    {
        console.log('Draw_Date_Detail');

        console.log('Draw_Integer_Detail');
        var _html = '';
        _html += '<div class="dynatitle"><span>Q' + selectedQuestion.Id + ': ' + selectedQuestion.text + '</span></div>';
        _html += '<div class="dynabody">'
        _html += '<div class="filterRow">'
        _html += '<select id="comparisonOperator" name="comparisonOperator" onchange="OnComparisonOperatorChange(this)"><option value="1">Equals</option><option value="2">Greater</option><option value="3">Less</option><option value="4">GreaterOrEqual</option><option value="5">LessOrEqual</option><option value="6">NotEqual</option><option value="7">Between</option></select>'
        _html += '<input id="userinput1" name="userinput1" type="text" class="userinput-date" maxlength="10"/>';
        _html += '<span class="userinput-separator">and</span>';
        _html += '<input id="userinput2" name="userinput2" type="text" class="userinput-date" maxlength="10"/>';
        _html += '</div>';
        _html += '</div>';
        $('#qnaDynamic').html(_html);

        $('#userinput1').datepicker({
            buttonImage: "/content/images/datepicker-20.png",
            showOn: "both",
            buttonImageOnly: true,
            dateFormat: "mm/dd/yy"
        });
        $('#userinput2').datepicker({
            buttonImage: "/content/images/datepicker-20.png",
            showOn: "both",
            buttonImageOnly: true,
            dateFormat: "mm/dd/yy"
        });
        OnComparisonOperatorChange($('#comparisonOperator').get(0));
    }

    function OnComparisonOperatorChange(sel)
    {
        var value = sel.value;
        if (value == '7' || value == 7) {
            $('span.userinput-separator').show();
            $('#userinput2').show();
        }
        else {
            $('span.userinput-separator').hide();
            $('#userinput2').hide();
        }
    }
    function Draw_Time_Detail(selectedQuestion)
    {
        console.log('Draw_Time_Detail');

    }
    function Draw_DateTime_Detail(selectedQuestion)
    {
        console.log('Draw_DateTime_Detail');

    }
    function Draw_OneFromMany_Detail(selectedQuestion)
    {
        console.log('Draw_OneFromMany_Detail');

        var _html = '';
        _html += '<div class="dynatitle"><span>Q' + selectedQuestion.Id + ': ' + selectedQuestion.text + '</span></div>';
        _html += '<div class="dynabody">'
        for (var i = 0; i < selectedQuestion.options.length; i++)
        {
            var optionid = "fltrOption_" + selectedQuestion.Id + "_" + selectedQuestion.options[i].Id;
            _html += '<div class="filterRow"><input type="checkbox" id="' + optionid + '" name="' + optionid + '" question="' + selectedQuestion.Id + '" optionId="' + selectedQuestion.options[i].Id +'" /><label for="' + optionid + '">' + selectedQuestion.options[i].text + '</label></div>'
        }
        _html += '</div>';
        $('#qnaDynamic').html(_html);
    }
    function Draw_ManyFromMany_Detail(selectedQuestion)
    {
        console.log('Draw_ManyFromMany_Detail');
        Draw_OneFromMany_Detail(selectedQuestion);
    }
    function Draw_DropDown_Detail(selectedQuestion)
    {
        console.log('Draw_DropDown_Detail');
        Draw_OneFromMany_Detail(selectedQuestion);
    }
    function Draw_Slider_Detail(selectedQuestion)
    {
        console.log('Draw_Slider_Detail');

    }
    function Draw_MatrixOnePerRow_Detail(selectedQuestion)
    {
        console.log('Draw_MatrixOnePerRow_Detail');

        var _html = '';
        _html += '<div class="dynatitle" id="dynatitle_' + selectedQuestion.Id + '"><span>Q' + selectedQuestion.Id + ': ' + selectedQuestion.text + '</span></div>';
        _html += '<div class="dynabody" id="dynabody_' + selectedQuestion.Id + '">'

        _html += Draw_MatrixOnePerRow_Body(selectedQuestion);
        
        _html += '</div>';

        $('#qnaDynamic').html(_html);
    }
    function Draw_MatrixOnePerRow_Body(selectedQuestion)
    {
        var numberOfSubpanels = $('.dynabody_section').length + 1;

        _html = '<div class="dynabody_section" id="dynabody_section_' + selectedQuestion.Id + '_' + numberOfSubpanels + '">';
        {
            //we render the available Options:
            var selectid = "fltrOptions_" + selectedQuestion.Id + "_" + numberOfSubpanels;
            _html += '<div class="filterOptions"><select id="' + selectid + '" name="' + selectid + '" onchange="OnfilterOptionsChange(this, ' + selectedQuestion.Id + ', ' + numberOfSubpanels + ')"><option value="">Choose a row...</option>';
            for (var i = 0; i < selectedQuestion.options.length; i++) {
                var optionid = "fltrOption_" + selectedQuestion.Id + "_" + selectedQuestion.options[i].Id + "_" + numberOfSubpanels;
                _html += '<option value="' + optionid + '">' + selectedQuestion.options[i].text + '</option>';
            }
            _html += '</select></div>'
        }

        {
            //we render the available Columns:
            var groupId = "fltrColumns_" + selectedQuestion.Id + "_" + numberOfSubpanels;
            _html += '<div id="' + groupId + '" class="filterColumns" style="display: none">';
            for (var i = 0; i < selectedQuestion.columns.length; i++) {
                var columndId = "fltrColumn_" + selectedQuestion.Id + "_" + selectedQuestion.columns[i].Id + "_" + numberOfSubpanels;
                _html += '<div class="filterRow"><input type="checkbox" id="' + columndId + '" name="' + columndId + '"/><label for="' + columndId + '">' + selectedQuestion.columns[i].text + '</label></div>'
            }
            var anchorid = 'anchor_' + selectedQuestion.Id + '_' + numberOfSubpanels;
            _html += '<div class="filterRow" id="' + anchorid + '"><a href="javascript:OnAddAnotherRow(' + selectedQuestion.Id + ', ' + numberOfSubpanels + ')">+ Add another row</a></div>'
            _html += '</div>'
        }
        
        _html += '</div>';
        return _html;
    }
    function OnfilterOptionsChange(sel, questionId, numberOfSubpanels)
    {
        var value = sel.value;
        console.log('OnfilterOptionsChange, value = ' + value + ', questionId = ' + questionId, +', numberOfSubpanels = ' + numberOfSubpanels);

        var groupId = "#fltrColumns_" + questionId + "_" + numberOfSubpanels;
        console.log(groupId);
        if(value == '0' || value == 0)
        {
            $(groupId).hide();
        }
        else
        {
            $(groupId).show();
        }
    }
    function OnAddAnotherRow(questionId, numberOfSubpanels)
    {
        console.log('OnAddAnotherRow');

        var anchorid = '#anchor_' + questionId + '_' + numberOfSubpanels;
        $(anchorid).hide();

        var selectedQuestion = GetQuestion(questionId);
        if (selectedQuestion == null) {
            console.log('OnAddAnotherRow, selectedQuestion is null!');
            return;
        }

        var _html = '';
        _html += Draw_MatrixOnePerRow_Body(selectedQuestion);

        $('#dynabody_' + selectedQuestion.Id).append(_html);
    }

    function Draw_MatrixManyPerRow_Detail(selectedQuestion)
    {
        console.log('Draw_MatrixManyPerRow_Detail');

    }
    function Draw_MatrixManyPerRowCustom_Detail(selectedQuestion)
    {
        console.log('Draw_MatrixManyPerRowCustom_Detail');

    }
    function Draw_Composite_Detail(selectedQuestion)
    {
        console.log('Draw_Composite_Detail');

    }


    function OnApplyOrUpdateFilterBtn()
    {
        var filterType = $('#filterType').val();
        if (filterType == null || filterType == undefined || filterType == '')
            return;
        if (filterType == 0)
        {
            alert('Choose a filter type....');
            return;
        }

        switch(filterType)
        {
            case '1'://QuestionAndAnswer
                OnApplyOrUpdateQuestionAndAnswerFilter();
                break;
            case '2'://Collector
                OnApplyOrUpdateCollectorFilter();
                break;
            case '3'://ResponseTime
                OnApplyOrUpdateResponseTimeFilter();
                break;
            case '4'://TimePeriod
                OnApplyOrUpdateTimePeriodFilter();
                break;
        }
    }
    function OnApplyOrUpdateQuestionAndAnswerFilter()
    {
        var questionId = $('#fltrQuestion').val();
        if (questionId == null || questionId == undefined || questionId == '' || questionId == 0 || questionId == '0') {
            alert('Choose a question...');
            return;
        }
        /*Ποια ερώτηση έχει επιλέξει ο χρήστης?*/
        var selectedQuestion = GetQuestion(questionId);
        if (selectedQuestion == null) {
            console.log('OnApplyOrUpdateQuestionAndAnswerFilter, selectedQuestion is NULL!!');
            alert('OnApplyOrUpdateQuestionAndAnswerFilter, selectedQuestion is NULL!!');
            return;
        }
        var questionType = selectedQuestion.type;



        if (questionType == 'OneFromMany' || questionType == 'ManyFromMany' || questionType == 'DropDown')
        {
            var options = '';
            $('input:checked', 'div.dynabody').each(function ()
            {
                if (options != '')
                    options += ',';
                options += $(this).attr('optionId');
            });
            if (options == '')
            {
                alert('Select one or more Options....');
                return;
            }
            

            var _data = 'viewId=' + theViewId;
            _data = _data + '&questionId=' + questionId;
            _data = _data + '&options=' + options;

            var _url = theManagerPath + 'services/api/ViewFilters/AddQnaFilterWithOptions?ScId=' + theAccessToken;

            $.ajax({
                url: _url, dataType: 'json', data: _data, async: false,
                success: function (data, textStatus, jqXHR) {
                    $('#TheFiltersForm').dialog('close');
                    window.location.href = theManagerPath + "clay/mysurveys/analysis/summary.aspx?surveyid=" + theSurveyId + '&viewId=' + theViewId + '&textslanguage=' + theTextsLanguage;
                }
            });

        }
        else if (questionType == 'MatrixOnePerRow' || questionType == 'MatrixManyPerRow')
        {
            var rows = '';
            $('div.filterOptions').find(':selected').each(function () {
                if (rows != '')
                    rows += ',';
                rows += $(this).attr('value');
            });
            if (rows == '') {
                alert('Select one or more Rows....');
                return;
            }

            var columns = '';
            $('input:checked', 'div.dynabody_section').each(function () {
                if (columns != '')
                    columns += ',';
                columns += $(this).attr('id');
            });
            if (columns == '') {
                alert('Select one or more Columns....');
                return;
            }


            var _data = 'viewId=' + theViewId;
            _data = _data + '&questionId=' + questionId;
            _data = _data + '&rows=' + rows;
            _data = _data + '&columns=' + columns;

            var _url = theManagerPath + 'services/api/ViewFilters/AddQnaFilterWithOptionsAndColumns?ScId=' + theAccessToken;

            $.ajax({
                url: _url, dataType: 'json', data: _data, async: false,
                success: function (data, textStatus, jqXHR) {
                    $('#TheFiltersForm').dialog('close');
                    window.location.href = theManagerPath + "clay/mysurveys/analysis/summary.aspx?surveyid=" + theSurveyId + '&viewId=' + theViewId + '&textslanguage=' + theTextsLanguage;
                }
            });
        }
        else if (questionType == 'Integer' || questionType == 'Decimal' || questionType == 'Date') 
        {
            var _comparisonOperator = $('#comparisonOperator').val();
            var _userInput1 = $('#userinput1').val();
            var _userInput2 = $('#userinput2').val();

            if (_userInput1 == '' || _userInput1 == null || _userInput1 == undefined)
            {
                alert('You must give a value!');
                return;
            }
            if(_comparisonOperator == 7)
            {
                if (_userInput2 == '' || _userInput2 == null || _userInput2 == undefined) 
                {
                    alert('You must give a second value!');
                    return;
                }
            }

            var _data = 'viewId=' + theViewId;
            _data = _data + '&questionId=' + questionId;
            _data = _data + '&operator=' + _comparisonOperator;
            _data = _data + '&userinput1=' + escape(_userInput1);
            _data = _data + '&userinput2=' + escape(_userInput2);

            var _url = theManagerPath + 'services/api/ViewFilters/AddQnaFilterWithUserInputs?ScId=' + theAccessToken;

            $.ajax({
                url: _url, dataType: 'json', data: _data, async: false,
                success: function (data, textStatus, jqXHR) {
                    $('#TheFiltersForm').dialog('close');
                    window.location.href = theManagerPath + "clay/mysurveys/analysis/summary.aspx?surveyid=" + theSurveyId + '&viewId=' + theViewId + '&textslanguage=' + theTextsLanguage;
                }
            });
            
        }
        


        

    }
    function OnApplyOrUpdateCollectorFilter()
    {
        var collectors = '';
        $('input:checked', 'div#CollectorPanel').each(function () {
            if (collectors != '')
                collectors += ',';
            collectors += $(this).attr('id');
        });
        if (collectors == '') {
            alert('Select one or more Collectors....');
            return;
        }

        var _data = 'viewId=' + theViewId;
        _data = _data + '&collectors=' + collectors;

        var _url = theManagerPath + 'services/api/ViewFilters/AddCollectorsFilter?ScId=' + theAccessToken;

        $.ajax({
            url: _url, dataType: 'json', data: _data, async: false,
            success: function (data, textStatus, jqXHR) {
                $('#TheFiltersForm').dialog('close');
                window.location.href = theManagerPath + "clay/mysurveys/analysis/summary.aspx?surveyid=" + theSurveyId + '&viewId=' + theViewId + '&textslanguage=' + theTextsLanguage;
            }
        });

    }
    function OnApplyOrUpdateResponseTimeFilter()
    {
        var totalResponseTimeOperator = $('#fltrTotalResponseTimeOperator').val();
        var totalResponseTime = $('#fltrTotalResponseTime').val();
        var totalResponseTimeUnit = $('#fltrResponseTimeUnit').val();

        if (totalResponseTime == null || totalResponseTime == undefined || totalResponseTime == '' || totalResponseTime == 0 || totalResponseTime == '0') {
            alert('Please give a valid Response Time!');
            return;
        }

        var _data = 'viewId=' + theViewId;
        _data = _data + '&totalResponseTimeOperator=' + totalResponseTimeOperator;
        _data = _data + '&totalResponseTime=' + totalResponseTime;
        _data = _data + '&totalResponseTimeUnit=' + totalResponseTimeUnit;

        var _url = theManagerPath + 'services/api/ViewFilters/AddResponseTimeFilter?ScId=' + theAccessToken;

        $.ajax({
            url: _url, dataType: 'json', data: _data, async: false,
            success: function (data, textStatus, jqXHR) {
                $('#TheFiltersForm').dialog('close');
                window.location.href = theManagerPath + "clay/mysurveys/analysis/summary.aspx?surveyid=" + theSurveyId + '&viewId=' + theViewId + '&textslanguage=' + theTextsLanguage;
            }
        });
    }
    function OnApplyOrUpdateTimePeriodFilter()
    {
        var timePeriodStart = $('#fltrTimePeriodStart').val();
        var timePeriodEnd = $('#fltrTimePeriodEnd').val();

        if (timePeriodStart == null || timePeriodStart == undefined || timePeriodStart == '') {
            alert('Please give a valid Period Start Date!');
            return;
        }
        if (timePeriodEnd == null || timePeriodEnd == undefined || timePeriodEnd == '') {
            alert('Please give a valid Period End Date!');
            return;
        }

        var _data = 'viewId=' + theViewId;
        _data = _data + '&timePeriodStart=' + escape(timePeriodStart);
        _data = _data + '&timePeriodEnd=' + escape(timePeriodEnd);

        var _url = theManagerPath + 'services/api/ViewFilters/AddTimePeriodFilter?ScId=' + theAccessToken;

        $.ajax({
            url: _url, dataType: 'json', data: _data, async: false,
            success: function (data, textStatus, jqXHR) {
                $('#TheFiltersForm').dialog('close');
                window.location.href = theManagerPath + "clay/mysurveys/analysis/summary.aspx?surveyid=" + theSurveyId + '&viewId=' + theViewId + '&textslanguage=' + theTextsLanguage;
            }
        });
    }


    function OnFilterStatusClick(elem, filterId) {
        alert('OnFilterStatusClick, filterId=' + filterId);
    }
    function OnFilterNameClick(elem, filterId) {
        alert('OnFilterNameClick, filterId=' + filterId);
    }
    function OnFilterDeleteClick(elem, filterId)
    {
        if (filterId == 'collectors')
        {
            showQuestion('Do you want to delete the Collectors Filter?',
                function () {
                    $.ajax({
                        url: theManagerPath + 'services/api/ViewFilters/DeleteCollectorsFilter?ScId=' + theAccessToken + '&viewId=' + theViewId,
                        dataType: 'html',
                        success: function (data) {
                            window.location.href = theManagerPath + "clay/mysurveys/analysis/summary.aspx?surveyid=" + theSurveyId + '&viewId=' + theViewId + '&textslanguage=' + theTextsLanguage;
                        }
                    });
                }
            );
        }
        else if (filterId == 'responsetime')
        {
            showQuestion('Do you want to delete the ResponseTime Filter?',
                function () {
                    $.ajax({
                        url: theManagerPath + 'services/api/ViewFilters/DeleteResponseTimeFilter?ScId=' + theAccessToken + '&viewId=' + theViewId,
                        dataType: 'html',
                        success: function (data) {
                            window.location.href = theManagerPath + "clay/mysurveys/analysis/summary.aspx?surveyid=" + theSurveyId + '&viewId=' + theViewId + '&textslanguage=' + theTextsLanguage;
                        }
                    });
                }
            );
        }
        else if (filterId == 'timeperiod')
        {
            showQuestion('Do you want to delete the TimePeriod Filter?',
                function () {
                    $.ajax({
                        url: theManagerPath + 'services/api/ViewFilters/DeleteTimePeriodFilter?ScId=' + theAccessToken + '&viewId=' + theViewId,
                        dataType: 'html',
                        success: function (data) {
                            window.location.href = theManagerPath + "clay/mysurveys/analysis/summary.aspx?surveyid=" + theSurveyId + '&viewId=' + theViewId + '&textslanguage=' + theTextsLanguage;
                        }
                    });
                }
            );
        }
        else
        {
            $.ajax({
                url: theManagerPath + 'services/api/ViewFilters/GetQnaFilterById?ScId=' + theAccessToken + '&filterId=' + filterId, dataType: 'json',
                success: function (data)
                {
                    showQuestion(
                        'Do you want to delete the filter ' + data.Name + ' ?',
                        function () {
                            $.ajax({
                                url: theManagerPath + 'services/api/ViewFilters/DeleteQnaFilter?ScId=' + theAccessToken + '&filterId=' + filterId,
                                dataType: 'html',
                                success: function (data)
                                {
                                    window.location.href = theManagerPath + "clay/mysurveys/analysis/summary.aspx?surveyid=" + theSurveyId + '&viewId=' + theViewId + '&textslanguage=' + theTextsLanguage;
                                }
                            });
                        }
                    );
                }
            });
        }
    }

    function OnExportAllSummaryDataPDF()
    {
        var _url = theManagerPath + 'services/api/Analysis/ExportSummaryPDF?ScId=' + theAccessToken;
        _url += '&surveyid=' + theSurveyId;
        _url += '&viewId=' + theViewId;
        _url += '&textsLanguage=' + theTextsLanguage;
        _url += + '&t=' + (new Date()).getTime();

        window.location.href = _url;
    }
    function OnExportAllResponsesDataXLSX()
    {
        var _url = theManagerPath + 'services/api/Analysis/ExportAllDataXLSX?ScId=' + theAccessToken;
        _url += '&surveyid=' + theSurveyId;
        _url += '&viewId=' + theViewId;
        _url += '&textsLanguage=' + theTextsLanguage;
        _url += + '&t=' + (new Date()).getTime();

        window.location.href = _url;
    }
</script>

<div id="toolboxwrapper">
    <div id="toolbox">
        <h3 class="ui-accordion-header ui-helper-reset">Current View</h3>
        <div class="ui-accordion-content ui-helper-reset">
            <div class="toolstrip">
                <a class="greenbutton" href="javascript:OnFilterAdd()">+Add Filter</a>
            </div>
            <%= GetFilters() %>


        </div>
        <h3 class="ui-accordion-header ui-helper-reset">Export</h3>
        <div class="ui-accordion-content ui-helper-reset">
            <a class="download-link" href="javascript:OnExportAllSummaryDataPDF()">All Summary Data (Pdf)</a>
            <a class="download-link" href="javascript:OnExportAllResponsesDataXLSX()">All Responses Data (xlsx)</a>
        </div>
    </div>
</div>


<div id="TheFiltersForm" title="+Add filter" class="" style="display: none">
    <div class="TheFiltersFormWrapper">

        <div id="FilterTypePanel">
            <div class="FilterTypePanelInner">
                <label style="font-weight:bold;font-size: 14px;">Add Filter</label>&nbsp;
                <select id="filterType" name="filterType">
                    <option value="0">Choose...</option><option value="1">By question and Answer</option><option value="2">By Collector</option><option value="3">By Response Time</option><option value="4">By Time Period</option>
                </select>
            </div>
        </div>

        <div id="QuestionAndAnswerPanel" class="filterpanel" style="display: none">
            <div class="title">
                <span>question and answer</span>
            </div>
            <div class="body">
                <select id="fltrQuestion" name="fltrQuestion" style="width: 400px;">
                    <%=this.GetSurveyQuestionsOptions() %>
                </select>
            </div>
            <div id="qnaDynamic">

            </div>
        </div>

        <div id="CollectorPanel" class="filterpanel" style="display: none">
            <div class="title">
                <span>collector</span>
            </div>
            <div class="body">
                <%=this.GetCollectorsCheckBoxes() %>
            </div>
        </div>

        <div id="ResponseTimePanel" class="filterpanel" style="display: none">
            <div class="title">
                <span>Total Response Time</span>
            </div>
            <div class="body">
                <div class="filterRow">
                    <select id="fltrTotalResponseTimeOperator" name="fltrTotalResponseTimeOperator">
                        <option value="0">Less than</option>
                        <option value="1">Greater than</option>
                    </select>
                    <input type="text" id="fltrTotalResponseTime" name="fltrTotalResponseTime" maxlength="4"/>
                    <select id="fltrResponseTimeUnit" name="fltrResponseTimeUnit">
                        <option value="0">Second(s)</option>
                        <option value="1">Minute(s)</option>
                        <option value="2">Hour(s)</option>
                        <option value="3">Day(s)</option>
                    </select>
                </div>

            </div>
        </div>

        <div id="TimePeriodPanel" class="filterpanel" style="display: none">
            <div class="title">
                <span>time period</span>
            </div>
            <div class="body">
                <div class="filterRow">
                    <table>
                        <tr><td>Start Date:</td><td>&nbsp;</td><td>End Date:</td></tr>
                        <tr><td><input type="text" id="fltrTimePeriodStart" name="fltrTimePeriodStart"/></td><td><span class="toend">to</span></td><td><input type="text" id="fltrTimePeriodEnd" name="fltrTimePeriodEnd"/></td></tr>
                        <tr><td><span style="font-size: .8em;">(MM/DD/YYYY)</span></td><td><span class="toend">&nbsp;</span></td><td><span style="font-size: .8em;">(MM/DD/YYYY)</span></td></tr>
                    </table>
                </div>
            </div>
        </div>

    </div>
</div>

