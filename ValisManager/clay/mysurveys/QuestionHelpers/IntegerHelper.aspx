<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="IntegerHelper.aspx.cs" Inherits="ValisManager.clay.mysurveys.QuestionHelpers.IntegerHelper" %>
<link href="QuestionHelpers/questionhelpers.css" rel="stylesheet" />
<script src="QuestionHelpers/questionhelpers.js"></script>

<div class="ValidationWrapper">
    <div class="formRow"><input type="checkbox" id="ValidateNumber" name="ValidateNumber" /><label for="ValidateNumber">Validate number</label></div>
    <div class="ValidationWrapperHidden">
        <div class="RangeClass">
            <span><label for="ValidationField1">between</label><input id="ValidationField1" name="ValidationField1" type="text" maxlength="9"/></span>
            <span><label for="ValidationField2">and</label><input id="ValidationField2" name="ValidationField2" type="text" maxlength="9"/></span>
        </div>
        <div class="MessageClass">
            <label for="ValidationMessage">When the number is invalid, display this error message:</label><br />
            <textarea id="ValidationMessage" name="ValidationMessage"></textarea>
        </div>
    </div>
</div>
<script type="text/javascript">
    function initializeFieldAttributes(data)
    {
        $('#ValidationField1').onlySignedIntegers();
        $('#ValidationField2').onlySignedIntegers();

        $('#ValidateNumber').bind('change', function () {
            if($(this).is(':checked'))
            {
                $('div.RangeClass').show(); $('div.MessageClass').show();
            }
            else
            {
                $('div.RangeClass').hide(); $('div.MessageClass').hide();
            }
        });

        if (data != null) {
            if (data.ValidationBehavior == '0' || data.ValidationBehavior == 0) {
                /*DoNotValidate*/
                $('#ValidateNumber').prop('checked', false);
                $('div.RangeClass').hide(); $('div.MessageClass').hide();

                $('#ValidationField1').val('');
                $('#ValidationField2').val('');
                $('#ValidationMessage').val('The number you entered is in an invalid format.');
            }
            else if (data.ValidationBehavior == '2' || data.ValidationBehavior == 2) {
                /*WholeNumber*/
                $('#ValidateNumber').prop('checked', true);
                $('div.RangeClass').show(); $('div.MessageClass').show();

                $('#ValidationField1').val(data.ValidationField1);
                $('#ValidationField2').val(data.ValidationField2);
                $('#ValidationMessage').val(data.ValidationMessage);
            }
        }
        else
        {
            /*DoNotValidate*/
            $('#ValidateNumber').prop('checked', false);
            $('div.RangeClass').hide(); $('div.MessageClass').hide();

            $('#ValidationField1').val('');
            $('#ValidationField2').val('');
            $('#ValidationMessage').val('The number you entered is in an invalid format.');
        }
    }
    function validateFieldAttributes()
    {
        if ($('#ValidateNumber').is(':checked'))
        {
            var message = $('#ValidationMessage').val();
            var between = $('#ValidationField1').val();
            var and = $('#ValidationField2').val();

            if (message == '' || message == null || message == undefined) {
                $("#ValidationMessage").animateHighlight("#fd2525", 400);
                $('#ValidationMessage').focus();
                return false;
            }
            if (between == '' || between == null || between == undefined || isNaN(between)) {
                $("#ValidationField1").animateHighlight("#fd2525", 400);
                $('#ValidationField1').focus();
                return false;
            }
            if (and == '' || and == null || and == undefined || isNaN(and)) {
                $("#ValidationField2").animateHighlight("#fd2525", 400);
                $('#ValidationField2').focus();
                return false;
            }

            var _between = parseInt(between);
            var _and = parseInt(and);

            if (_and < _between) {
                $("#ValidationField1").animateHighlight("#fd2525", 400);
                $("#ValidationField2").animateHighlight("#fd2525", 400);
                return false;
            }

            return true;
        }
        return true;
    }
    function getFieldAttributesData()
    {
        var _data = '&ValidationField1=' + escape($('#ValidationField1').val());
        _data = _data + '&ValidationField2=' + escape($('#ValidationField2').val());
        _data = _data + '&ValidationMessage=' + escape($('#ValidationMessage').val());
        if ($('#ValidateNumber').is(':checked'))
        {
            /*WholeNumber*/
            _data = _data + '&ValidationBehavior=2';
        }
        else
        {
            /*DoNotValidate*/
            _data = _data + '&ValidationBehavior=0';
        }

        return _data;
    }
</script>