<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="RangeHelper.aspx.cs" Inherits="ValisManager.clay.mysurveys.QuestionHelpers.RangeHelper" %>
<link href="QuestionHelpers/questionhelpers.css" rel="stylesheet" />
<script src="QuestionHelpers/questionhelpers.js"></script>

    <div class="formRow">
        <label for="FrontLabelText" id="FrontLabelTextLbl">FrontLabel:</label>
        <input type="text" id="FrontLabelText" name="FrontLabelText" />
    </div>
    <div class="formRow">
        <label for="RangeStart" id="RangeStartLbl">RangeStart:</label>
        <input type="text" id="RangeStart" name="RangeStart" />
    </div>
    <div class="formRow">
        <label for="AfterLabelText" id="AfterLabelTextLbl">AfterLabel:</label>
        <input type="text" id="AfterLabelText" name="AfterLabelText" />
    <div class="formRow">
        <label for="RangeEnd" id="RangeEndLbl">RangeEnd:</label>
        <input type="text" id="RangeEnd" name="RangeEnd" />
    </div>
    </div>


<script type="text/javascript">
    function initializeFieldAttributes(data) {

        $('#RangeStart').onlySignedIntegers();
        $('#RangeEnd').onlySignedIntegers();
        if (data != null) {

            $('#FrontLabelText').val(data.FrontLabelText);
            $('#RangeStart').val(data.RangeStart);
            $('#AfterLabelText').val(data.AfterLabelText);
            $('#RangeEnd').val(data.RangeEnd);
        }
    }
    function validateFieldAttributes() {
        var start = $('#RangeStart').val();
        var end = $('#RangeEnd').val();

        if (start == '' || start == null || start == undefined) {
            $("#RangeStart").animateHighlight("#fd2525", 400);
            $('#RangeStart').focus();
            return false;
        }
        if (end == '' || end == null || end == undefined) {
            $("#RangeEnd").animateHighlight("#fd2525", 400);
            $('#RangeEnd').focus();
            return false;
        }

        var _start = parseInt(start);
        var _end = parseInt(end);

        if (_end < _start) {
            $("#RangeStart").animateHighlight("#fd2525", 400);
            $("#RangeEnd").animateHighlight("#fd2525", 400);
            return false;
        }

        return true;
    }
    function getFieldAttributesData() {

        var _data = '&FrontLabelText=' + encodeURIComponent($('#FrontLabelText').val());
        _data = _data + '&RangeStart=' + encodeURIComponent($('#RangeStart').val());
        _data = _data + '&AfterLabelText=' + encodeURIComponent($('#AfterLabelText').val());
        _data = _data + '&RangeEnd=' + encodeURIComponent($('#RangeEnd').val());

        return _data;
    }
</script>