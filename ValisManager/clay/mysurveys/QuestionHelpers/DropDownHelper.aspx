<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="DropDownHelper.aspx.cs" Inherits="ValisManager.clay.mysurveys.QuestionHelpers.DropDownHelper" %>
<link href="QuestionHelpers/questionhelpers.css" rel="stylesheet" />
<script src="QuestionHelpers/questionhelpers.js"></script>

    <label class="" id="QuestionChoicesLabel">Answer Choices: <span class="detail"> Enter each choice on a separate line.</span></label>
    <textarea name="QuestionChoices" class="QuestionChoices" id="QuestionChoices" rows="6" cols="48"></textarea>

    <div class="formRow"><input type="checkbox" id="Randomize" name="Randomize" /><label for="Randomize">Randomize or flip choices.</label></div>
    
<script type="text/javascript">
    function initializeFieldAttributes(data)
    {
        console.log('DropDownHelper::initializeFieldAttributes');

        if (data != null)
        {
            $('#QuestionChoices').val(data.Options);

            //Randomize
            $('#Randomize').prop('checked', data.RandomizeOptionsSequence);
        }
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

        return true;
    }
    function getFieldAttributesData()
    {
        var _data = '&QuestionChoices=' + escape($('#QuestionChoices').val());
        _data = _data + '&Randomize=' + ($('#Randomize').is(':checked') == true ? 'on' : 'off');

        return _data;
    }
</script>