namespace Valis.Core
{
    public interface IQuestionRenderer
    {
        void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question);
    }
}
