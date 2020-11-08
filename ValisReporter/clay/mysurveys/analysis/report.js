/// <reference path="P:\Projects\Valis Project\ValisSolution\ValisManager\scripts/jquery-2.1.1.js" />
/// <reference path="P:\Projects\Valis Project\ValisSolution\ValisManager\scripts/highcharts/js/highcharts.js" />

function GetQuestion(questionId) {
    for (var i = 0; i < questions.length; i++) {
        if (questions[i].Id == questionId) {
            return questions[i];
        }
    }
    return null;
}
function GetQuestionOption(questionId, optionId) {
    for (var i = 0; i < questions.length; i++) {
        if (questions[i].Id == questionId) {
            for (var j = 0; j < questions[i].options.length; j++) {
                if (questions[i].options[j].Id == optionId)
                    return questions[i].options[j];
            }
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
function GetQuestionColumn(questionId, columnId) {
    for (var i = 0; i < questions.length; i++) {
        if (questions[i].Id == questionId) {
            for (var j = 0; j < questions[i].columns.length; j++) {
                if (questions[i].columns[j].Id == columnId)
                    return questions[i].columns[j];
            }
        }
    }
    return null;
}


function Get_ResponseTotal(summaryQuestion, Id)
{
    for(var i =0; i<summaryQuestion.ResponseTotals.length; i++)
    {
        if(summaryQuestion.ResponseTotals[i].Id == Id)
            return summaryQuestion.ResponseTotals[i];
    }

    return null;
}
function Get_ResponseTotal_Column(responseTotal, Id)
{
    for(var i =0; i<responseTotal.Cols.length; i++)
    {
        if (responseTotal.Cols[i].Id == Id)
            return responseTotal.Cols[i];
    }

    return null;
}

function Get_xAxis_categories(question)
{
    // ['John', 'Bruto', 'Makis', 'Anna', 'George', 'Tasos']
    var categories = new Array();

    for(var i=0; i < question.options.length; i++)
    {
        categories.push(question.options[i].text);
    }
    return categories;
}
function Get_Question_Data(question)
{
    var data = new Array();

    var summaryQuestion = question.summaryQuestion;

    for (var i = 0; i < question.options.length; i++)
    {
        var option = question.options[i];
        var responseTotal = Get_ResponseTotal(summaryQuestion, option.Id);
        if (responseTotal == null)
        {
            data.push(0);
        }
        else
        {
            if (summaryQuestion.AxisScale == 1/*Absolute*/)
                data.push(responseTotal.Ttl);
            else
                data.push(responseTotal.Pcnt);
        }
    }

    return data;
}
function Get_Question_Pie_Data(question)
{
    var data = new Array();

    var summaryQuestion = question.summaryQuestion;

    for (var i = 0; i < summaryQuestion.ResponseTotals.length; i++)
    {
        if (summaryQuestion.AxisScale == 1/*Absolute*/)
        {
            var total = summaryQuestion.ResponseTotals[i].Ttl;
            var option = GetQuestionOptionEx(question, summaryQuestion.ResponseTotals[i].Id)
            if (option == null)
                data.push(new Array('??', total));
            else
                data.push(new Array(option.text, total));
        }
        else
        {
            var total = summaryQuestion.ResponseTotals[i].Pcnt;
            var option = GetQuestionOptionEx(question, summaryQuestion.ResponseTotals[i].Id)
            if(option == null)
                data.push(new Array('??', total));
            else
                data.push(new Array(option.text, total));
        }
    }

    return data;
}
function Get_question_3D_Series_R(question)
{
    var series = new Array();

    var summaryQuestion = question.summaryQuestion;

    for (var c = question.columns.length-1; c >=0; c--)
    {
        var column = question.columns[c];
        var record = { name: column.text };
        record.data = new Array();

        for (var p = 0; p < question.options.length; p++)
        {
            var option = question.options[p];

            var responseTotal = Get_ResponseTotal(summaryQuestion, option.Id);
            if (responseTotal == null) {
                record.data.push(0);
                continue;
            }

            var responseTotalColumn = Get_ResponseTotal_Column(responseTotal, column.Id);
            if (responseTotalColumn == null) {
                record.data.push(0);
                continue;
            }

            if (summaryQuestion.AxisScale == 1/*Absolute*/)
                record.data.push(responseTotalColumn.Ttl);
            else
                record.data.push(responseTotalColumn.Pcnt);

        }

        series.push(record);
    }

    return series;
}
function Get_question_3D_Series(question)
{
    var series = new Array();

    var summaryQuestion = question.summaryQuestion;

    for (var c = 0; c < question.columns.length; c++) {
        var column = question.columns[c];
        var record = { name: column.text };
        record.data = new Array();

        for (var p = 0; p < question.options.length; p++) {
            var option = question.options[p];

            var responseTotal = Get_ResponseTotal(summaryQuestion, option.Id);
            if (responseTotal == null) {
                record.data.push(0);
                continue;
            }

            var responseTotalColumn = Get_ResponseTotal_Column(responseTotal, column.Id);
            if (responseTotalColumn == null) {
                record.data.push(0);
                continue;
            }

            if (summaryQuestion.AxisScale == 1/*Absolute*/)
                record.data.push(responseTotalColumn.Ttl);
            else
                record.data.push(responseTotalColumn.Pcnt);

        }

        series.push(record);
    }

    return series;
}



$(document).ready(function ()
{
    for (var i = 0; i < summary.Pages.length; i++)
    {
        var page = summary.Pages[i];
        for (var j = 0; j < page.Questions.length; j++)
        {
            var summaryQuestion = page.Questions[j];
            var question = GetQuestion(summaryQuestion.Id);
            question.summaryQuestion = summaryQuestion;

            /*εμφανίζεται η ερώτηση στο Report?*/
            if (summaryQuestion.ShowResponses == false)
                continue;

            var html = '<div class="PageHeader"><span class="brandName">XCompany Surveys</span></div>';
            html += '<div class="QuestionReport clearfix" >';

            html += '<div class="QuestionReport-Header clearfix" id="QuestionReport-Header-' + summaryQuestion.Id + '">';
            html += '<span class="QuestionReport-Header-QN">Q' + summaryQuestion.Id + '</span>';
            html += '</div>';

            html += '<div class="QuestionReport-Body">';
            html += '<h1 class="QuestionReport-QTitle">' + question.text + '</h1>';
            html += '<h3 class="QuestionReport-QSubTitle"><span>Answered: ' + summaryQuestion.TotalAnswered + '</span><span>Skipped: ' + summaryQuestion.TotalSkipped + '</span></h3>'

            if (question.type == 'SingleLine' || question.type == 'Integer' || question.type == 'Decimal' || question.type == 'Date')
            {
                html += RenderResponsesTable(summaryQuestion, question);
            }
            else if (question.type == 'MultipleLine')
            {

            }
            else if (question.type == 'Time' || question.type == 'DateTime')
            {

            }
            else if (question.type == 'OneFromMany' || question.type == 'ManyFromMany' || question.type == 'DropDown' || question.type == 'Range')
            {
                html += Render1DChart(summaryQuestion, question);
                html += Render1DDataTable(summaryQuestion, question);
            }
            else if (question.type == 'Slider')
            {

            }
            else if (question.type == 'MatrixOnePerRow' || question.type == 'MatrixManyPerRow')
            {
                html += Render2DChart(summaryQuestion, question);
                html += Render2DDataTable(summaryQuestion, question);
            }
            html += '</div>';
            html += '</div>';

            $('#summaryReport').append(html);

        }
    }


    $('.QuestionReport-Q1DChart').each(function (index)
    {
        var questionId = $(this).attr('questionid');
        var question = GetQuestion(questionId);

        Draw_Q1DChart(question);
    });


    $('.QuestionReport-Q2DChart').each(function (index)
    {
        var questionId = $(this).attr('questionid');
        var question = GetQuestion(questionId);

        Draw_Q2DChart(question);
    });

});



function Draw_QChart(question)
{
    if (question.type == 'OneFromMany' || question.type == 'ManyFromMany' || question.type == 'DropDown' || question.type == 'Range')
    {
        Draw_Q1DChart(question);
    }
    else if (question.type == 'MatrixOnePerRow' || question.type == 'MatrixManyPerRow')
    {
        Draw_Q2DChart(question);
    }
}
function Draw_Q1DChart(question)
{
    var summaryQuestion = question.summaryQuestion;
    var _yAxis = { title: { text: null }, allowDecimals: false };


    if (summaryQuestion.ChartType == 0/*HorizontalBar*/)
    {
        var _categories = Get_xAxis_categories(question);
        var _data = Get_Question_Data(question);

        if(summaryQuestion.AxisScale == 0)
            _yAxis = {
                allowDecimals: false,
                title: { text: null }, tickInterval: 20, max: 100, min: 0, labels: {
                    formatter: function () { return this.value + '%'; }
                }
            };

        var chart1 = new Highcharts.Chart({
            chart: { renderTo: 'QChart_' + question.Id, type: 'bar' }, title: { text: null }, plotOptions: { series: { animation: false, states: { hover: {enabled: false}} } },
            xAxis: { categories: _categories },
            yAxis: _yAxis,
            credits: { enabled: true },
            tooltip: {enabled: false},
            series: [ { data: _data, name: 'responses', showInLegend: false }]
        });

    }
    else if (summaryQuestion.ChartType == null || summaryQuestion.ChartType == 1/*VerticalBar*/)
    {
        var _categories = Get_xAxis_categories(question);
        var _data = Get_Question_Data(question);

        if (summaryQuestion.AxisScale == 0)
            _yAxis = {
                allowDecimals: false,
                title: { text: null }, tickInterval: 20, max: 100, min: 0, labels: {
                    formatter: function () { return this.value + ' %'; }
                }
            };

        var chart1 = new Highcharts.Chart({
            chart: { renderTo: 'QChart_' + question.Id, type: 'column' }, title: { text: null }, plotOptions: { series: { animation: false, states: { hover: { enabled: false } } } },
            xAxis: { categories: _categories },
            yAxis: _yAxis,
            credits: { enabled: true },
            tooltip: { enabled: false },
            series: [ { data: _data, name: 'responses', showInLegend: false }]
        });
    }
    else if (summaryQuestion.ChartType == 2/*Pie*/)
    {
        var _data = Get_Question_Pie_Data(question);
        var chart1 = new Highcharts.Chart({
            chart: { renderTo: 'QChart_' + question.Id, type: 'pie' }, title: { text: null }, plotOptions: { series: { animation: false, states: { hover: { enabled: false } } } },
            credits: { enabled: true },
            tooltip: { enabled: false },
            series: [{ data: _data }]
        });
    }


    //if (summaryQuestion.ShowDataTable == true)
    //{
        $('#QuestionData' + summaryQuestion.Id).show();
    //}
    //else
    //{
    //    $('#QuestionData' + summaryQuestion.Id).hide();
    //}
}
function Draw_Q2DChart(question)
{
    var summaryQuestion = question.summaryQuestion;
    var _yAxis = { title: { text: null } };

    if (summaryQuestion.ChartType == 0/*HorizontalBar*/)
    {
        var _categories = Get_xAxis_categories(question);
        var _series = Get_question_3D_Series_R(question);

        if (summaryQuestion.AxisScale == 0)
            _yAxis = {
                title: { text: null }, tickInterval: 20, max: 100, min: 0, labels: {
                    formatter: function () { return this.value + '%'; }
                }
            };

        var chart5 = new Highcharts.Chart({
            chart: { renderTo: 'QChart_' + question.Id, type: 'bar', height: 800 }, title: { text: null }, plotOptions: { series: { animation: false, states: { hover: { enabled: false } } } },
            xAxis: { categories: _categories },
            yAxis: _yAxis,
            credits: { enabled: true },
            tooltip: { enabled: false },
            series: _series
        });
    }
    else if (summaryQuestion.ChartType == null || summaryQuestion.ChartType == 1/*VerticalBar*/)
    {
        var _categories = Get_xAxis_categories(question);
        var _series = Get_question_3D_Series(question);

        if (summaryQuestion.AxisScale == 0)
            _yAxis = {
                title: { text: null }, tickInterval: 20, max: 100, min: 0, labels: {
                    formatter: function () { return this.value + ' %'; }
                }
            };

        var chart5 = new Highcharts.Chart({
            chart: { renderTo: 'QChart_' + question.Id, type: 'column' }, title: { text: null }, plotOptions: { series: { animation: false, states: { hover: { enabled: false } } } },
            xAxis: { categories: _categories },
            yAxis: _yAxis,
            credits: { enabled: true },
            tooltip: { enabled: false },
            series: _series
        });
    }

    //if (summaryQuestion.ShowDataTable == true)
    //{
        $('#QuestionData' + summaryQuestion.Id).show();
    //}
    //else 
    //{
        $('#QuestionData' + summaryQuestion.Id).hide();
    //}
}


function RenderPageHeader()
{
    return '<div class="PageHeader">XCompany Surveys</div>';
}
function RenderPageFooter()
{
}
function Render1DChart(summaryQuestion, question)
{
    return '<div class="QuestionReport-Q1DChart" questionid="' + summaryQuestion.Id + '" id="QChart_' + summaryQuestion.Id + '"></div>';
}
function Render1DDataTable(summaryQuestion, question)
{
    var _html = '';
    
    _html += '<div style="display:none;" class="QuestionReport-QDataTable" id="QuestionData' + summaryQuestion.Id + '">';
    _html += '<table>';
    _html += '<thead><tr><th>Answer Choices</th><th colspan="2">Responses</th></tr></thead>';
    _html += '<tbody>';
    
    for (var i = 0; i < question.options.length; i++)
    {
        var option = question.options[i];
        var responseTotal = Get_ResponseTotal(summaryQuestion, option.Id);

        if(responseTotal == null)
        {
            _html += '<tr><td>' + option.text + '</td><td>0.00%</td><td>0</td></tr>';
        }
        else
        {
            _html += '<tr><td>' + option.text + '</td><td>' + responseTotal.Pcnt + '%</td><td>' + responseTotal.Ttl + '</td></tr>';
        }

    }

    _html += '</tbody>';
    _html += '</table>';
    _html += '</div>';
    return _html;
}

function Render2DChart(summaryQuestion, question)
{
    return '<div class="QuestionReport-Q2DChart" questionid="' + summaryQuestion.Id + '" id="QChart_' + summaryQuestion.Id + '"></div>';
}
function Render2DDataTable(summaryQuestion, question)
{
    var _html = '';

    _html += '<div class="QuestionReport-QDataTable" id="QuestionData' + summaryQuestion.Id + '">';
    _html += '<table>';
    _html += '<thead><tr><th></th>';
    for (var i = 0; i < question.columns.length; i++)
    {
        _html += '<th>' + question.columns[i].text+ '</th>';
    }
    _html += '</tr></thead>';

    _html += '<tbody>';

    for (var p = 0; p < question.options.length; p++)
    {
        _html += '<tr>';
        _html += '<td>' + question.options[p].text + '</td>';

        var responseTotal = Get_ResponseTotal(summaryQuestion, question.options[p].Id);

        for (var c = 0; c < question.columns.length; c++)
        {
            _html += '<td>';

            if (responseTotal != null)
            {
                var summarycolumn = Get_ResponseTotal_Column(responseTotal, question.columns[c].Id);
                if (summarycolumn != null)
                {
                    _html += '<div class="data-percent">' + summarycolumn.Pcnt +'%</div>';
                    _html += '<div class="data-total">' + summarycolumn.Ttl + '</div>';
                }
                else
                {
                    _html += '<div class="data-percent">0.00%</div>';
                    _html += '<div class="data-total">0</div>';
                }
            }

            _html += '</td>';
        }

        _html += '</tr>';
    }
    _html += '</tbody>';

    _html += '</table>';
    _html += '</div>';
    return _html;
}

function RenderResponsesTable(summaryQuestion, question)
{

    var _html = '<div class="QuestionReport-Responses">';
    {
        _html += '<div class="QuestionReport-Responses-Container">';

        for (var i = 0; i < summaryQuestion.ResponseTotals.length; i++)
        {
            _html += '<div class="Response-Item">';

            if (summaryQuestion.ResponseTotals[i].Ttl > 1)
                _html += '<p><span class="Total">(Total: ' + summaryQuestion.ResponseTotals[i].Ttl + ')</span>' + summaryQuestion.ResponseTotals[i].Input + '</p>';
            else
                _html += '<p>' + summaryQuestion.ResponseTotals[i].Input + '</p>';

            _html += '</div>';
        }


        _html += '</div>';
    }
    _html += '</div>';

    return _html;
}