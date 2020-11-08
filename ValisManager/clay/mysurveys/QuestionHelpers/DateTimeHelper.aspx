<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="DateTimeHelper.aspx.cs" Inherits="ValisManager.clay.mysurveys.QuestionHelpers.DateTimeHelper" %>
<link href="QuestionHelpers/questionhelpers.css" rel="stylesheet" />
<script src="QuestionHelpers/questionhelpers.js"></script>
    
    <div class="ValidationWrapper">
        <label for="ValidationBehavior">Date Format:</label>
        <select id="ValidationBehavior" name="ValidationBehavior">
            <option value="4">MM/DD/YYYY</option>
            <option value="5">DD/MM/YYYY</option>
        </select>
    </div>


<script type="text/javascript">
    function initializeFieldAttributes(data)
    {
        if (data != null)
        {
            $('#ValidationBehavior').val(data.ValidationBehavior);
        }
    }
    function validateFieldAttributes()
    {
        return true;
    }
    function getFieldAttributesData()
    {
        var _data = '&ValidationBehavior=' + escape($('#ValidationBehavior').val());

        return _data;
    }
</script>