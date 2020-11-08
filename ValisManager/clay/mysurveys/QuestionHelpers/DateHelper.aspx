<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="DateHelper.aspx.cs" Inherits="ValisManager.clay.mysurveys.QuestionHelpers.DateHelper" %>
<link href="QuestionHelpers/questionhelpers.css" rel="stylesheet" />
<script src="QuestionHelpers/questionhelpers.js"></script>
    
    <div class="ValidationWrapper">
        <div><input type="checkbox" id="UseDateTimeControls" name="UseDateTimeControls" /><label for="UseDateTimeControls">Use Date Control</label></div>
        <div>
            <label for="ValidationBehavior">Date Format:</label>
            <select id="ValidationBehavior" name="ValidationBehavior">
                <option value="4">MM/DD/YYYY</option>
                <option value="5">DD/MM/YYYY</option>
            </select>
        </div>
    </div>


<script type="text/javascript">
    function initializeFieldAttributes(data) {
        if (data != null) {
            $('#UseDateTimeControls').prop('checked', data.UseDateTimeControls);
            $('#ValidationBehavior').val(data.ValidationBehavior);
        }
    }
    function validateFieldAttributes() {
        return true;
    }
    function getFieldAttributesData() {
        var _data = '&ValidationBehavior=' + escape($('#ValidationBehavior').val());
        _data = _data + '&UseDateTimeControls=' + ($('#UseDateTimeControls').is(':checked') == true ? 'on' : 'off');

        return _data;
    }
</script>