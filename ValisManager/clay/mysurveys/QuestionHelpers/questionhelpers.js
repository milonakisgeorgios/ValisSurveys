
function OnChangeValidationBehavior()
{
    var _value = $('#ValidationBehavior').val();

    if (_value == '0' || _value == 0 /*DoNotValidate*/) { $('div.RangeClass').hide(); $('div.MessageClass').hide(); }
    else if (_value == '1' || _value == 1 /*TextOfSpecificLength*/) { $('div.RangeClass').show(); $('div.MessageClass').show(); }
    else if (_value == '2' || _value == 2 /*WholeNumber*/) { $('div.RangeClass').show(); $('div.MessageClass').show(); }
    else if (_value == '3' || _value == 3 /*DecimalNumber*/) { $('div.RangeClass').show(); $('div.MessageClass').show(); }
    else if (_value == '4' || _value == 4 /*date (DD/MM/YYYY) */) { $('div.RangeClass').hide(); $('div.MessageClass').show(); }
    else if (_value == '5' || _value == 5 /*date (DD/MM/YYYY) */) { $('div.RangeClass').hide(); $('div.MessageClass').show(); }
    else if (_value == '6' || _value == 6 /*email address*/) { $('div.RangeClass').hide(); $('div.MessageClass').show(); }
    else if (_value == '7' || _value == 7 /*RegularExpression*/) { $('div.RangeClass').hide(); $('div.MessageClass').hide(); }
    else { $('div.RangeClass').hide(); $('div.MessageClass').hide(); };
}

function OnInitializeValidationBehavior(data)
{
    $('#ValidationField1').onlySignedNumbers();
    $('#ValidationField2').onlySignedNumbers();
    $('#ValidationBehavior').change(OnChangeValidationBehavior);

    if (data != null) {
        if (data.ValidationBehavior == '0' || data.ValidationBehavior == 0) {
            OnChangeValidationBehavior();
            return;
        }
        $('#ValidationBehavior').val(data.ValidationBehavior);
        $('#ValidationField1').val(data.ValidationField1);
        $('#ValidationField2').val(data.ValidationField2);
        $('#ValidationMessage').val(data.ValidationMessage);
        OnChangeValidationBehavior();
    }
    else {
        $('#ValidationBehavior').val('0');
        $('#ValidationField1').val('');
        $('#ValidationField2').val('');
        $('#ValidationMessage').val('The comment you entered is in an invalid format.');

        OnChangeValidationBehavior();
    }
}

function OnValidateValidationBehavior()
{
    var _value = $('#ValidationBehavior').val();
    if (_value == '0')
        return true;
    
    if (_value == /*TextOfSpecificLength*/'1' || _value == /*WholeNumber*/'2' || _value == /*DecimalNumber*/'3')
    {
        value = $('#ValidationMessage').val();
        if (value == '' || value == null || value == undefined) {
            $("#ValidationMessage").animateHighlight("#fd2525", 400);
            //alert('You must provide a QuestionText for this question!');
            $('#ValidationMessage').focus();
            return false;
        }
    }
    if (_value == /*TextOfSpecificLength*/'1' || _value == /*WholeNumber*/'2' || _value == /*DecimalNumber*/'3') {
        var between = $('#ValidationField1').val();
        var and = $('#ValidationField2').val();

        if (between == '' || between == null || between == undefined || isNaN(between)) {
            $("#ValidationField1").animateHighlight("#fd2525", 400);
            //alert('You must provide a QuestionText for this question!');
            $('#ValidationField1').focus();
            return false;
        }

        if (and == '' || and == null || and == undefined || isNaN(and)) {
            $("#ValidationField2").animateHighlight("#fd2525", 400);
            //alert('You must provide a QuestionText for this question!');
            $('#ValidationField2').focus();
            return false;
        }
        if (_value == '1' || _value == '2') {
            var _between = parseInt(between);
            var _and = parseInt(and);

            if (_and < _between) {
                $("#ValidationField1").animateHighlight("#fd2525", 400);
                $("#ValidationField2").animateHighlight("#fd2525", 400);
                return false;
            }
        }
        else if (_value == '3') {
            var _between = parseFloat(between);
            var _and = parseFloat(and);

            if (_and < _between) {
                $("#ValidationField1").animateHighlight("#fd2525", 400);
                $("#ValidationField2").animateHighlight("#fd2525", 400);
                return false;
            }
        }

    }

    return true;
}

function GetValidationBehaviorData()
{
    var _data = '&ValidationBehavior=' + escape($('#ValidationBehavior').val());
    _data = _data + '&ValidationField1=' + escape($('#ValidationField1').val());
    _data = _data + '&ValidationField2=' + escape($('#ValidationField2').val());
    _data = _data + '&ValidationMessage=' + escape($('#ValidationMessage').val());

    return _data;
}