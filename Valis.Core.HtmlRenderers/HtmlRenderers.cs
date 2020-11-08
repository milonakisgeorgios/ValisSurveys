using System.Web.UI;
using Valis.Core.Html.Clay;

namespace Valis.Core.Html
{
    /// <summary>
    /// 
    /// </summary>
    public static class HtmlRenderers
    {

        public static QuestionRenderer GetQuestionRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession, QuestionType type)
        {
            switch(type)
            {
                case QuestionType.SingleLine:
                    return new SingleLineRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.MultipleLine:
                    return new MultipleLineRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.Integer:
                    return new IntegerRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.Decimal:
                    return new DecimalRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.Date:
                    return new DateRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.Range:
                    return new RangeRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.OneFromMany:
                    return new OneFromManyRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.ManyFromMany:
                    return new ManyFromManyRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.DropDown:
                    return new DropDownRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.DescriptiveText:
                    return new DescriptiveTextRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.MatrixOnePerRow:
                    return new MatrixOnePerRowRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.MatrixManyPerRow:
                    return new MatrixManyPerRowRenderer(surveyManager, writer, runtimeSession);
                case QuestionType.Composite:
                    return new CompositeRenderer(surveyManager, writer, runtimeSession);

            }

            return new DefaultRenderer(surveyManager, writer, null);
        }

    }
}
