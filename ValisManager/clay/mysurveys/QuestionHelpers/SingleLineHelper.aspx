<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="SingleLineHelper.aspx.cs" Inherits="ValisManager.clay.mysurveys.QuestionHelpers.SingleLineHelper" %>
<link href="QuestionHelpers/questionhelpers.css" rel="stylesheet" />
<script src="QuestionHelpers/questionhelpers.js"></script>

    <div class="ValidationWrapper">
        <label for="ValidationBehavior">Validation:</label>
        <select id="ValidationBehavior" name="ValidationBehavior"><%=this.GetSingleLineValidationOptions() %></select>
        <div class="ValidationWrapperHidden">
            <div class="RangeClass">
                <span><label for="ValidationField1">between</label><input id="ValidationField1" name="ValidationField1" type="text" maxlength="9"/></span>
                <span><label for="ValidationField2">and</label><input id="ValidationField2" name="ValidationField2" type="text" maxlength="9"/></span>
            </div>
            <div class="MessageClass">
                <label for="ValidationMessage">When the comment is an invalid format, display this error message:</label><br />
                <textarea id="ValidationMessage" name="ValidationMessage">The comment you entered is in an invalid format.</textarea>
            </div>
        </div>
    </div>


<script type="text/javascript">
    function initializeFieldAttributes(data)
    {
        OnInitializeValidationBehavior(data);
    }
    function validateFieldAttributes()
    {
        if (OnValidateValidationBehavior() == false)
            return false;

        return true;
    }
    function getFieldAttributesData()
    {
        var _data = GetValidationBehaviorData();

        return _data;
    }
</script>