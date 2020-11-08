<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="OneFromManyHelper.aspx.cs" Inherits="ValisManager.clay.mysurveys.QuestionHelpers.OneFromManyHelper" %>
<link href="QuestionHelpers/questionhelpers.css" rel="stylesheet" />
<script src="QuestionHelpers/questionhelpers.js"></script>

    <label class="" id="QuestionChoicesLabel">Answer Choices: <span class="detail"> Enter each choice on a separate line.</span></label>
    <textarea name="QuestionChoices" class="QuestionChoices" id="QuestionChoices" rows="6" cols="48"></textarea>

    <div class="formRow"><input type="checkbox" id="Randomize" name="Randomize" /><label for="Randomize">Randomize or flip choices.</label></div>
    
    <div class="formRow"><input type="checkbox" id="AddOtherField" name="AddOtherField" /><label for="AddOtherField">Add "Other" or a comment field <span class="detail">(Optional)</span></label></div>
    
    <div class="OtherFieldWrapper" style="display:none">
        <div class="formRow"><label>Field label:</label><input type="text" id="OtherFieldLabel" name="OtherFieldLabel"/></div>
        <div class="formRow"><label>Field Type:</label><select id="OtherFieldType" name="OtherFieldType"><%=GetOtherFieldTypes() %></select></div>
        
        
        <div class="ValidationWrapper">
        <label for="ValidationBehavior">Validation:</label>
        <select id="ValidationBehavior" name="ValidationBehavior"><%=this.GetValidationOptions() %></select>
        <div class="ValidationWrapperHidden">
            <div class="RangeClass">
                <span><label for="ValidationField1">between</label><input id="ValidationField1" name="ValidationField1" type="text" maxlength="9"/></span>
                <span><label for="ValidationField2">and</label><input id="ValidationField2" name="ValidationField2" type="text" maxlength="9"/></span>
            </div>
            <div class="MessageClass">
                <label for="ValidationMessage">When the comment is an invalid format, display this error message:</label><br />
                <textarea id="ValidationMessage" name="ValidationMessage"></textarea>
            </div>
        </div>
    </div>


    </div>
<script type="text/javascript">
    function initializeFieldAttributes(data)
    {
        console.log('OneFromManyHelper::initializeFieldAttributes');

        $('#AddOtherField').bind('change', function () {
            if($(this).is(':checked'))
            {
                $('div.OtherFieldWrapper').show();
            }
            else
            {
                $('div.OtherFieldWrapper').hide();
            }
        });

        if (data != null)
        {
            $('#QuestionChoices').val(data.Options);

            //Randomize
            $('#Randomize').prop('checked', data.RandomizeOptionsSequence);
            if (data.OptionalInputBox)
            {
                $('#AddOtherField').prop('checked', true);
                $('#OtherFieldType').val(data.OtherFieldType);
                $('#OtherFieldLabel').val(data.OtherFieldLabel);
                $('div.OtherFieldWrapper').show();
            }
            else
            {
                $('#AddOtherField').prop('checked', false);
                $('#OtherFieldType').val('0');
                $('#OtherFieldLabel').val(null);
                $('div.OtherFieldWrapper').hide();
            }

        }
        OnInitializeValidationBehavior(data);
    }
    function validateFieldAttributes() {

        var value = $('#QuestionChoices').val();
        if (value == '' || value == null || value == undefined) {
            //alert('You must provide "Answer Choices" for this question!');
            //$("#QuestionChoices").fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100);
            $("#QuestionChoices").animateHighlight("#fd2525", 500);
            $('#QuestionChoices').focus();
            return false;
        }

        if (OnValidateValidationBehavior() == false)
            return false;

        return true;
    }
    function getFieldAttributesData()
    {
        var _data = '&QuestionChoices=' + escape($('#QuestionChoices').val());
        _data = _data + '&Randomize=' + ($('#Randomize').is(':checked') == true ? 'on' : 'off');
        _data = _data + '&AddOtherField=' + ($('#AddOtherField').is(':checked') == true ? 'on' : 'off');
        _data = _data + '&OtherFieldLabel=' + escape($('#OtherFieldLabel').val());
        _data = _data + '&OtherFieldType=' + escape($('#OtherFieldType').val());

        _data = _data + GetValidationBehaviorData();

        return _data;
    }
</script>