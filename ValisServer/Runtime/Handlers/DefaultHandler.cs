using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using Valis.Core;
using Valis.Core.Html;

namespace ValisServer.Runtime
{
    public class DefaultHandler : SurveyHttpHandler
    {
        /// <summary>
        ///Η σελίδα που είχε γίνει Render στον χρήστη μας, την τελευταία φορά
        /// </summary>
        VLSurveyPage PreviousPage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        PageType CurrentPageType { get; set; }
        /// <summary>
        /// Η σελίδα του Survey που θα γίνει RENDER ΣΕ ΑΥΤΟ ΤΟ REQUEST!.
        /// </summary>
        VLSurveyPage CurrentPage { get; set; }
        /// <summary>
        /// Η επόμενη σελίδα μετά την CurrentPage (εάν υπάρχει)!
        /// </summary>
        VLSurveyPage NextPage { get; set; }
        /// <summary>
        /// Η κατεύθυνση (Previous/Next) που επέλεξε ο χρήστης μας!
        /// </summary>
        NavigationDirections Navigation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        Boolean RefreshPage { get; set; }

        Collection<VLSurveyQuestion> m_Questions = null;
        /// <summary>
        /// Επιστρέφει τις ερωτήσεις που υπάρχουν στην CurrentPage
        /// </summary>
        Collection<VLSurveyQuestion> Questions
        {
            get
            {
                if (m_Questions == null)
                {
                    if (this.CurrentPage != null)
                    {
                        m_Questions = SurveyManager.GetQuestionsForPage(this.CurrentPage);
                    }
                }
                return m_Questions;
            }
            set
            {
                m_Questions = value;
            }
        }

        Int32 PageQuestionsCounter = 0;
        Int32 WholeSurveyQuestionsCounter
        {
            get
            {
                if (this.RuntimeSession != null)
                {
                    object _value = this.RuntimeSession["_SurveyQuestionsCounter"];
                    if (_value != null)
                        return (Int32)_value;

                }
                return 0;
            }
            set
            {
                if (this.RuntimeSession != null)
                {
                    this.RuntimeSession["_SurveyQuestionsCounter"] = value;
                }
            }
        }
        Int32 WholeSurveyPageCounter
        {
            get
            {
                if (this.RuntimeSession != null)
                {
                    object _value = this.RuntimeSession["_WholeSurveyPageCounter"];
                    if (_value != null)
                        return (Int32)_value;

                }
                return 0;
            }
            set
            {
                if (this.RuntimeSession != null)
                {
                    this.RuntimeSession["_WholeSurveyPageCounter"] = value;
                }
            }
        }


        #region ProcessRequestImplementation
        protected override void ProcessRequestImplementation()
        {
            bool _Can_User_Continue = true;
            bool _Collect_and_Validate_PostedValues = true;
            bool _Question_With_Skip_Logic = false;
            SkipToBehavior _SkipTo = SkipToBehavior.None;
            Int16? _SkipToPage = null;
            Int16? _SkipToQuestion = null;
            String _SkipToWebUrl = null;

            if (this.Collector.Status != CollectorStatus.Open)
            {
                /*Σε αυτό το σημείο δεν πρέπει να φτάσουμε ποτέ!*/
                throw new VLException("Collector's Status != CollectorStatus.Open!");
            }
            else if (this.Recipient != null && (this.Recipient.Status == RecipientStatus.Completed || this.Recipient.Status == RecipientStatus.Disqualified))
            {
                /*Σε αυτό το σημείο δεν πρέπει να φτάσουμε ποτέ!*/
                throw new VLException("Recipient's Status != (None or OpenSurvey or PartiallyCompleted)!");
            }
            
			
            /*
             * Ελέγχουμε μήπως ο χρήστης έχει ζητήσει RefreshPage (π.χ. αλλαγή γλώσσας).
             * Οταν ο χρήστης έχει ζητήσει refreshPage, τότε στο Post δεν υπάρχει κάποιο απο τα NextButton, PreviousButton.
             * Αυτό που θέλουμε είναι να μην χαθούν αυτά που έχει συμπληρώσει, και να κάνουμε refresh την σελίδα
             */
            var _refreshPage = Request.Params["RefreshPage"];
            if(!string.IsNullOrWhiteSpace(_refreshPage))
            {
                if(_refreshPage=="1")
                {
                    this.RefreshPage = true;
                }
            }
            

            if(RuntimeSession.IsRessurected)
            {
                #region Βρίσκουμε το PreviousPage (εάν υπάρχει)
                if (!string.IsNullOrWhiteSpace(Request.Params["frmCurrentPage"]))
                {
                    var __previousPage = SurveyManager.GetSurveyPageById(Survey.SurveyId, Int16.Parse(Request.Params["frmCurrentPage"]), Survey.TextsLanguage);
                    if (__previousPage != null)
                    {
                        this.PreviousPage = __previousPage;
                    }
                    else
                    {
                        throw new VLException("DefaultHandler::ProcessRequestImplementation -> PreviousPage == null");
                    }
                }
                #endregion

                #region Βρίσκουμε το επιθυμητό Navigation (εάν υπάρχει)
                this.Navigation = NavigationDirections.None;
                if (Request.Params.AllKeys.Contains("NextButton"))
                {
                    this.Navigation = NavigationDirections.Next;
                }
                else if (Request.Params.AllKeys.Contains("PreviousButton"))
                {
                    this.Navigation = NavigationDirections.Previous;
                }
                #endregion
            }
            else
            {
                #region Ξεκινάμε για πρώτη φορά
                this.PreviousPage = null;
                this.Navigation = NavigationDirections.None;
                #endregion
            }


            if (RuntimeSession.IsRessurected && this.Navigation == NavigationDirections.None && this.PreviousPage == null)
            {
                #region Yποθέτουμε ότι ο χρήστης εγκατέλειψε το survey χωρίς να το ολοκληρώσει και τώρα έκανε ξανά click το link!
                /*
                 * Yποθέτουμε ότι ο χρήστης εγκατέλειψε το survey χωρίς να το ολοκληρώσει και τώρα έκανε ξανά click το link!
                 * Εχουμε αποθηκεύσει στο Ressurected Runtime Session, κάποια στοιχεία για να συνεχίσουμε απο εκεί που ήταν ο χρήστης
                 * πρίν διακόψει
                 */

                var _frmPreviousPage = this.RuntimeSession["frmPreviousPage"];
                if (_frmPreviousPage != null)
                {
                    Int16 previousPageId = -1;
                    if (Int16.TryParse((string)_frmPreviousPage, out previousPageId))
                    {
                        var __previousPage = SurveyManager.GetSurveyPageById(Survey.SurveyId, previousPageId, Survey.TextsLanguage);
                        if (__previousPage != null)
                        {
                            this.PreviousPage = __previousPage;
                            _Collect_and_Validate_PostedValues = false;
                        }
                    }
                }
                
                var _frmNavigation = this.RuntimeSession["frmNavigation"];
                if (_frmNavigation != null)
                {
                    byte _navigation = 0;
                    if (Byte.TryParse((string)_frmNavigation, out _navigation))
                    {
                        this.Navigation = (NavigationDirections)_navigation;
                    }
                }
                #endregion
            }


            #region CollectAndvalidateAnswers
            if (this.PreviousPage != null && _Collect_and_Validate_PostedValues && (Navigation == NavigationDirections.Next || this.RefreshPage == true))
            {
                /*
                 * Θέλουμε να μαζέψουμε τις απαντήσεις για την PreviousPage.
                 * Επίσης πρέπει να ελέγξουμε ότι δεν λείπει κάποια required τιμή.
                 * Επίσης πρέπει να ελέγξουμε και την εγκυρότητα του input
                 */
                byte _temp = 0;
                byte? _previousSelectedOption = null;
                byte? _afterSelectedOption = null;
                VLSurveyQuestion _question_with_logic = null;


                var renderedQuestions = SurveyManager.GetQuestionsForPage(this.PreviousPage);
                foreach (var q in renderedQuestions)
                {
                    /*
                     * Μήπως έχουμε ερώτηση με SKIP LOGIC?
                     * Τέτοιες ερωτήσεις είναι τύπου QuestionType.DropDown ή QuestionType.OneFromMany,
                     * που σημαίνει ότι έχουμε μία απάντηση (επιλογή απο τον χρήστη μας)
                     */
                    if (q.HasSkipLogic)
                    {
                        _Question_With_Skip_Logic = true;
                        _question_with_logic = q;
                        string _sessionValue = this.RuntimeSession[q.HtmlQuestionId] as string;
                        if (_sessionValue != null && byte.TryParse(_sessionValue, out _temp))
                        {
                            _previousSelectedOption = _temp;
                        }
                    }
                }

                if (CollectAndvalidateAnswers(this.PreviousPage, renderedQuestions) == false || this.RefreshPage == true)
                {
                    //Πρέπει να ξαναδείξουμε την προηγούμενη σελίδα, διότι έχουμε απαντήσεις που λείπουν ή μη έγκυρες απαντήσεις
                    _Can_User_Continue = false;

                    //Θα χρησιμοποιήσομε τις ερωτήσεις που διαβάσαμε ήδη, διότι κουβαλάνε τα αποτελέσματα του validation
                    this.CurrentPage = this.PreviousPage;
                    this.NextPage = SurveyManager.GetNextSurveyPage(this.PreviousPage, honorSkipLogic: true);
                    this.Questions = renderedQuestions;
                    if (this.CurrentPage.DisplayOrder == 1)
                        this.CurrentPageType = PageType.FirstPage;
                    else
                        this.CurrentPageType = PageType.NormalPage;
                }
                else
                {
                    /*Σε αυτό το σημείο πρέπει να ενημερώσουμε το recipient, (εάν υπάρχει), ότι o χρήστης έχει μερικώς απαντήσει:*/
                    if (this.Recipient != null)
                    {
                        bool update = false;
                        if (this.Recipient.HasPartiallyResponded == false)
                        {
                            this.Recipient.HasPartiallyResponded = true;
                            update = true;
                        }
                        if (this.Recipient.Status != RecipientStatus.PartiallyCompleted)
                        {
                            this.Recipient.Status = RecipientStatus.PartiallyCompleted;
                            update = true;
                        }
                        if (update)
                        {
                            this.Recipient = SurveyManager.UpdateRecipient(this.Recipient);
                        }
                    }
                }
                if (_Can_User_Continue == true)
                {
                    /*
                     * Μήπως έχουμε ερώτηση με SKIP LOGIC?
                     * Τέτοιες ερωτήσεις είναι τύπου QuestionType.DropDown ή QuestionType.OneFromMany,
                     * που σημαίνει ότι έχουμε μία απάντηση (επιλογή απο τον χρήστη μας)
                     */
                    if (_Question_With_Skip_Logic)
                    {
                        string _sessionValue = this.RuntimeSession[_question_with_logic.HtmlQuestionId] as string;

                        if (_sessionValue != null && byte.TryParse(_sessionValue, out _temp))
                        {
                            _afterSelectedOption = _temp;
                            var option = SurveyManager.GetQuestionOptionById(_question_with_logic.Survey, _question_with_logic.QuestionId, _temp, _question_with_logic.TextsLanguage);
                            if (option != null)
                            {
                                _SkipTo = option.SkipTo;
                                _SkipToPage = option.SkipToPage;
                                _SkipToQuestion = option.SkipToQuestion;
                                _SkipToWebUrl = option.SkipToWebUrl;
                            }
                        }

                        if (_previousSelectedOption.HasValue && _afterSelectedOption.HasValue && _previousSelectedOption.Value != _afterSelectedOption.Value)
                        {
                            //Θέλουμε να διαγράψουμε απο το RuntimeSession, όλες τις απαντήσεις απο εδώ και πέρα!*/
                            int findAt = -1;
                            for (int i = 0; i < this.RuntimeSession.Container.Count; i++)
                            {
                                var item = this.RuntimeSession.Container.ElementAt(i);
                                if(item.Key == _question_with_logic.HtmlQuestionId)
                                {
                                    findAt = i;
                                    break;
                                }
                            }
                            for(int i = this.RuntimeSession.Container.Count-1; i> findAt; i--)
                            {
                                var item = this.RuntimeSession.Container.ElementAt(i);
                                if (item.Key.StartsWith("QstnID_", StringComparison.OrdinalIgnoreCase))
                                    this.RuntimeSession.Container.Remove(item.Key);
                            }

                        }

                    }
                }
            }
            #endregion

            if (_Can_User_Continue == true)
            {
                #region Βρίσκουμε το CurrentPage, NextPage που θα δείξουμε
                if (this.Navigation == NavigationDirections.Next)
                {
                    #region
                    if (this.Survey.ShowGoodbyePage)
                        this.CurrentPageType = PageType.Goodbye;
                    else
                        this.CurrentPageType = PageType.EndSurvey;


                    if(_SkipTo == SkipToBehavior.None)
                    {
                        if (this.PreviousPage == null)
                        {
                            this.CurrentPage = SurveyManager.GetFirstSurveyPage(this.Survey);
                            if (this.CurrentPage != null)
                            {
                                this.CurrentPageType = PageType.FirstPage;
                            }
                        }
                        else
                        {
                            this.CurrentPage = SurveyManager.GetNextSurveyPage(this.PreviousPage, honorSkipLogic: true);
                            if(this.CurrentPage != null)
                            {
                                this.CurrentPageType = PageType.NormalPage;

                                if (this.CurrentPage.PageId == BuiltinVirtualPages.DisqualificationPage.PageId)
                                {
                                    this.CurrentPageType = PageType.Disqualification;
                                    this.CurrentPage = null;
                                }
                                else if (this.CurrentPage.PageId == BuiltinVirtualPages.EndSurveyPage.PageId)
                                {
                                    this.CurrentPageType = PageType.EndSurvey;
                                    this.CurrentPage = null;
                                }
                                else if (this.CurrentPage.PageId == BuiltinVirtualPages.GoodbyPage.PageId)
                                {
                                    this.CurrentPageType = PageType.Goodbye;
                                    this.CurrentPage = null;
                                }
                            }
                        }
                    }
                    else if(_SkipTo == SkipToBehavior.AnotherPage)
                    {
                        this.CurrentPage = SurveyManager.GetSurveyPageById(this.Survey.SurveyId, _SkipToPage.Value, this.Language.LanguageId);
                        if (this.CurrentPage != null)
                        {
                            if (this.CurrentPage.DisplayOrder == 1)
                                this.CurrentPageType = PageType.FirstPage;
                            else
                                this.CurrentPageType = PageType.NormalPage;
                        }
                    }
                    else if(_SkipTo == SkipToBehavior.GoodbyePage)
                    {
                        this.CurrentPageType = PageType.Goodbye;
                    }
                    else if(_SkipTo == SkipToBehavior.DisqualificationPage)
                    {
                        this.CurrentPageType = PageType.Disqualification;
                    }
                    else if(_SkipTo == SkipToBehavior.EndSurvey)
                    {
                        this.CurrentPageType = PageType.EndSurvey;
                    }


                    if (this.CurrentPage != null)
                    {
                        this.NextPage = SurveyManager.GetNextSurveyPage(this.CurrentPage, honorSkipLogic: true);
                    }

                    if (this.PreviousPage != null)
                    {
                        this.RuntimeSession.PushPage(this.PreviousPage.PageId);
                    }
                    #endregion
                }
                else if (this.Navigation == NavigationDirections.Previous)
                {
                    #region
                    if (!this.RuntimeSession.IsPagesStackEmpty)
                    {
                        this.CurrentPage = SurveyManager.GetSurveyPageById(Survey.SurveyId, this.RuntimeSession.PopPage(), Survey.TextsLanguage);
                    }

                    this.NextPage = this.PreviousPage;
                    if (this.CurrentPage != null)
                    {
                        if (this.CurrentPage.DisplayOrder == 1)
                            this.CurrentPageType = PageType.FirstPage;
                        else
                            this.CurrentPageType = PageType.NormalPage;
                    }
                    else
                    {
                        this.PreviousPage = null;
                        if (Survey.ShowWelcomePage)
                        {
                            this.CurrentPageType = PageType.Welcome;
                            this.CurrentPage = null;
                            this.NextPage = SurveyManager.GetFirstSurveyPage(this.Survey);
                        }
                        else
                        {
                            this.CurrentPageType = PageType.FirstPage;
                            this.CurrentPage = SurveyManager.GetFirstSurveyPage(this.Survey);
                            this.NextPage = SurveyManager.GetNextSurveyPage(this.CurrentPage, honorSkipLogic: true);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region
                    this.PreviousPage = null;
                    //ανοιξε το survey πρώτη φορά
                    if(Survey.ShowWelcomePage)
                    {
                        this.CurrentPageType = PageType.Welcome;
                        this.CurrentPage = null;
                        this.NextPage = SurveyManager.GetFirstSurveyPage(this.Survey);
                    }
                    else
                    {
                        this.CurrentPageType = PageType.FirstPage;
                        this.CurrentPage = SurveyManager.GetFirstSurveyPage(this.Survey);
                        this.NextPage = SurveyManager.GetNextSurveyPage(this.CurrentPage, honorSkipLogic: true);
                    }
                    #endregion
                }
                #endregion
            }


            {
                System.Diagnostics.Debug.WriteLine("---------------------------------");
                //var message = string.Format("Language = {0}", Language.Name);
                //System.Diagnostics.Debug.WriteLine(message);
                //message = string.Format("Survey = {0}", Survey.ShowTitle != null ? Survey.ShowTitle : Survey.Title);
                //System.Diagnostics.Debug.WriteLine(message);
                var message = string.Format("PreviousPage = {0}", this.PreviousPage != null ? this.PreviousPage.PageId.ToString() : "-");
                System.Diagnostics.Debug.WriteLine(message);
                message = string.Format("Navigation = {0}", Navigation.ToString());
                System.Diagnostics.Debug.WriteLine(message);
                message = string.Format("CurrentPageType = {0}", this.CurrentPageType);
                System.Diagnostics.Debug.WriteLine(message);
                message = string.Format("CurrentPage = {0}", CurrentPage != null ? CurrentPage.PageId.ToString() : "-");
                System.Diagnostics.Debug.WriteLine(message);
                message = string.Format("NextPage = {0}", NextPage != null ? NextPage.PageId.ToString() : "-");
                System.Diagnostics.Debug.WriteLine(message);
                message = string.Format("PageStack = {0}", this.RuntimeSession.GetStackDump());
                System.Diagnostics.Debug.WriteLine(message);
            }


            ShowSurvey();
        }

        void ShowSurvey()
        {
            var responseHtml = new StringBuilder(this.SurveyTheme.RtHtml);



            var _htmlBuffer = new StringBuilder();
            var htmlWriter = new HtmlTextWriter(new StringWriter(_htmlBuffer));
            {
                RenderHtmlHead(htmlWriter);
                responseHtml.Replace("#@HTMLHEAD", _htmlBuffer.ToString());
            }
            //HEADER
            {
                if (Survey.ShowSurveyTitle && !string.IsNullOrWhiteSpace(this.Survey.HeaderHtml))
                {
                    responseHtml.Replace("#@SURVEYHEADER", this.Survey.HeaderHtml);
                }
                else
                {
                    responseHtml.Replace("#@SURVEYHEADER", string.Empty);
                }

                if (Survey.ShowLanguageSelector && Survey.SupportedLanguages.Count > 1)
                {
                    _htmlBuffer.Clear();
                    RenderLanguageSelector(htmlWriter);
                    responseHtml.Replace("#@SURVEYLANGUAGESELECTOR", _htmlBuffer.ToString());
                }
                else
                {
                    responseHtml.Replace("#@SURVEYLANGUAGESELECTOR", string.Empty);
                }
            }
            //#@SURVEY_TOP_PROGRESSBAR
            //{
            //    if (this.Survey.UseProgressBar && this.Survey.ProgressBarPosition == ProgressBarPosition.AtTop && (this.CurrentPageType == PageType.NormalPage || this.CurrentPageType == PageType.FirstPage) )
            //    {
            //        _htmlBuffer.Clear();
            //        RenderProgressBar(htmlWriter, this.CurrentPage);
            //        responseHtml.Replace("#@SURVEY_TOP_PROGRESSBAR", _htmlBuffer.ToString());
            //    }
            //    else
            //    {
            //        responseHtml.Replace("#@SURVEY_TOP_PROGRESSBAR", string.Empty);
            //    }
            //}
            //FORM ATTRIBUTES
            {
                if (RequestType == RuntimeRequestType.Collector_Email)
                {
                    var action = string.Format("{0}{1}", Utility.GetSurveyRuntimeURL(Survey, Collector, this.Recipient, false), RuntimeSession.SessionId.ToString());
                    responseHtml.Replace("#@FORM_METHOD", "post");
                    responseHtml.Replace("#@FORM_ACTION", action);
                }
                else if (RequestType == RuntimeRequestType.Collector_WebLink)
                {
                    var action = string.Format("{0}{1}", Utility.GetSurveyRuntimeURL(this.Survey, this.Collector, false), RuntimeSession.SessionId.ToString());
                    responseHtml.Replace("#@FORM_METHOD", "post");
                    responseHtml.Replace("#@FORM_ACTION", action);
                }
                else if (RequestType == RuntimeRequestType.Manual_Email)
                {
                    var action = string.Format("{0}{1}", Utility.GetSurveyRuntimeURL(Survey, Collector, this.Recipient, true), RuntimeSession.SessionId.ToString());
                    responseHtml.Replace("#@FORM_METHOD", "post");
                    responseHtml.Replace("#@FORM_ACTION", action);
                }
                else if (RequestType == RuntimeRequestType.Manual_webLink)
                {
                    var action = string.Format("{0}{1}", Utility.GetSurveyRuntimeURL(this.Survey, this.Collector, true), RuntimeSession.SessionId.ToString());
                    responseHtml.Replace("#@FORM_METHOD", "post");
                    responseHtml.Replace("#@FORM_ACTION", action);
                }
            }
            //SURVEYBODY
            {
                _htmlBuffer.Clear();
                RenderSurveyBody(htmlWriter);
                responseHtml.Replace("#@SURVEYBODY", _htmlBuffer.ToString());
            }
            //#@SURVEY_BOTTOM_PROGRESSBAR
            {
                if (this.Survey.UseProgressBar && this.Survey.ProgressBarPosition == ProgressBarPosition.AtBottom && (this.CurrentPageType == PageType.NormalPage || this.CurrentPageType == PageType.FirstPage))
                {
                    _htmlBuffer.Clear();
                    RenderProgressBar(htmlWriter, this.CurrentPage);
                    responseHtml.Replace("#@SURVEY_BOTTOM_PROGRESSBAR", _htmlBuffer.ToString());
                }
                else
                {
                    responseHtml.Replace("#@SURVEY_BOTTOM_PROGRESSBAR", string.Empty);
                }
            }
            //NAVIGATION
            {
                _htmlBuffer.Clear();
                RenderSurveyNavigation(htmlWriter);

                if (this.PreviousPage != null)
                {
                    this.RuntimeSession["frmNavigation"] = ((byte)this.Navigation).ToString();
                    this.RuntimeSession["frmPreviousPage"] = this.PreviousPage.PageId.ToString();
                }
                else
                {
                    this.RuntimeSession["frmPreviousPage"] = null;
                    this.RuntimeSession["frmNavigation"] = null;
                }

                #region <input name="frmCurrentPage" type="hidden" value="" />
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, "frmCurrentPage");
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                if (CurrentPage != null)
                {
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Value, CurrentPage.PageId.ToString());
                }
                else
                {
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Value, string.Empty);
                }
                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Input);
                htmlWriter.RenderEndTag();
                htmlWriter.WriteLine();
                #endregion

                #region <input name="frmSurveyId" type="hidden" value="" />
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, "frmSurveyId");
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Value, Survey.SurveyId.ToString());
                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Input);
                htmlWriter.RenderEndTag();
                htmlWriter.WriteLine();
                #endregion

                var currentUnixTime = Utility.DatetimeToUnixTime(Utility.UtcNow());
                if (string.IsNullOrWhiteSpace(Request.Params["frmFirstAccess"]))
                {
                    #region <input name="frmFirstAccess" type="hidden" value="" />
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, "frmFirstAccess");
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Value, currentUnixTime.ToString(CultureInfo.InvariantCulture));
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Input);
                    htmlWriter.RenderEndTag();
                    htmlWriter.WriteLine();
                    #endregion
                }
                else
                {
                    #region <input name="frmFirstAccess" type="hidden" value="" />
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, "frmFirstAccess");
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Value, Request.Params["frmFirstAccess"]);
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Input);
                    htmlWriter.RenderEndTag();
                    htmlWriter.WriteLine();
                    #endregion
                }

                #region <input name="frmLastAccess" type="hidden" value="" />
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, "frmLastAccess");
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Value, currentUnixTime.ToString(CultureInfo.InvariantCulture));
                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Input);
                htmlWriter.RenderEndTag();
                htmlWriter.WriteLine();
                #endregion

                #region <input type="hidden" name="RefreshPage" id="RefreshPage" value="" />
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, "RefreshPage");
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Value, string.Empty);
                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Input);
                htmlWriter.RenderEndTag();
                htmlWriter.WriteLine();
                #endregion

                responseHtml.Replace("#@SURVEYNAVIGATION", _htmlBuffer.ToString());
            }
            //FOOTER
            {
                if (!string.IsNullOrWhiteSpace(this.Survey.FooterHtml))
                {
                    responseHtml.Replace("#@SURVEYFOOTER", this.Survey.FooterHtml);
                }
                else
                {
                    responseHtml.Replace("#@SURVEYFOOTER", string.Empty);
                }
            }



            SurveyManager.ReleaseSession(this.RuntimeSession);
            if (CurrentPage == null)
            {
                if (Navigation == NavigationDirections.Next)
                {
                    if (CurrentPageType == PageType.Disqualification)
                    {
                        //(Τελειώσαμε) έχουμε εμφανίσει την Disqualification Page:
                        SurveyManager.CreateResponse(this.RuntimeSession, this.Survey, this.Collector, this.Recipient, isDisqualified: true);
                    }
                    else
                    {
                        //(Τελειώσαμε) Εχουμε εμφανίσει την Goodbye / EndSurvey Page:
                        SurveyManager.CreateResponse(this.RuntimeSession, this.Survey, this.Collector, this.Recipient, isDisqualified: false);
                    }
                }
            }


			//Αφήνουμε να γραφεί το responseHTML, και τελειώσαμε απο εδώ!
            Response.ContentType = "text/html";
            Response.Write(responseHtml.ToString());
        }


        bool CollectAndvalidateAnswers(VLSurveyPage page, Collection<VLSurveyQuestion> questions)
        {
            //διαγράφουμε απο το RuntimeSession τις παλιές απαντήσεις για αυτές τις ερωτήσεις
            foreach (var q in questions)
            {
                //Το πάμε ανάποδα, γιατί όταν κάνουμε remove, το collection αλλάζει (καλύπτει το κενό που δημιουργείται!)
                int index = RuntimeSession.Keys.Count - 1;
                for (; index >= 0; index--)
                {
                    var key = RuntimeSession.Keys.ElementAt(index);

                    if (key.Contains(q.HtmlQuestionId))
                    {
                        this.RuntimeSession.Remove(key);
                    }
                }
            }


            //Μαζεύουμε τυχόν νέες απαντήσεις για αυτές τις ερωτήσεις
            foreach (var q in questions)
            {
                /*we coolect now now the data...*/
                foreach (var key in Request.Params.AllKeys)
                {
                    if (key.Contains(q.HtmlQuestionId))
                    {
                        var _value = Request.Params[key];
                        if (string.IsNullOrWhiteSpace(_value) == false)
                        {
                            this.RuntimeSession[key] = _value;
                        }
                    }
                }
            }


            //Now we Validate the input values
            foreach (var q in questions)
            {
                #region validation
                Int16 _totalAnswers = 0;

                /*we count now the input data for that question:*/
                foreach (var key in this.RuntimeSession.Keys)
                {
                    if (key.Contains(q.HtmlQuestionId))
                    {
                        _totalAnswers++;
                    }
                }

                /*Ελέγχουμε εάν η ερώτηση ήταν Required κσι εάν λάβαμε απάντηση απο τον χρήστη:*/
                Int32 minimumRequiredAnswers = 1;                
                if (q.IsRequired)
                {
                    #region
                    switch (q.QuestionType)
                    {
                        case QuestionType.SingleLine:
                        case QuestionType.MultipleLine:
                        case QuestionType.Integer:
                        case QuestionType.Decimal:
                            {
                                minimumRequiredAnswers = 1;
                            }
                            break;
                        case QuestionType.Date:
                            if (q.UseDateTimeControls)
                            {
                                minimumRequiredAnswers = 1;
                            }
                            else
                            {
                                minimumRequiredAnswers = 3;
                            }
                            break;
                        case QuestionType.Time:
                            break;
                        case QuestionType.DateTime:
                            break;
                        case QuestionType.OneFromMany:
                            {
                                /*Σε αυτή την περίπτωση έχουμε και το OptionalInputBox, το οποίο πρέπει να το λάβουμε υπόψην μας:*/
                                var _value = this.RuntimeSession[q.HtmlQuestionId] as string;
                                if (_value == q.HtmlQuestionId + "OptionalInputBox_")
                                {
                                    if (q.ValidationBehavior == ValidationMode.Date1 || q.ValidationBehavior == ValidationMode.Date2)
                                    {
                                        minimumRequiredAnswers = 4;
                                    }
                                    else
                                    {
                                        minimumRequiredAnswers = 2;
                                    }
                                }
                            }
                            break;
                        case QuestionType.ManyFromMany:
                            {
                                var _value = this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_"] as string;
                                if (_value == "1")
                                {
                                    if (q.ValidationBehavior == ValidationMode.Date1 || q.ValidationBehavior == ValidationMode.Date2)
                                    {
                                        minimumRequiredAnswers = 4;
                                    }
                                    else
                                    {
                                        minimumRequiredAnswers = 2;
                                    }
                                }
                            }
                            break;
                        case QuestionType.DropDown:
                            {
                                minimumRequiredAnswers = 1;
                            }
                            break;
                        case QuestionType.Slider:
                            break;
                        case QuestionType.MatrixOnePerRow:
                            {
                                var options = SurveyManager.GetQuestionOptions(q);
                                minimumRequiredAnswers = options.Count;
                            }
                            break;
                        case QuestionType.MatrixManyPerRow:
                            break;
                        case QuestionType.MatrixManyPerRowCustom:
                            break;
                        case QuestionType.Composite:
                            break;
                    }
                    #endregion

                    if (_totalAnswers < minimumRequiredAnswers)
                    {
                        q.HasMissingValues = true;
                        page.HasMissingValues = true;
                    }
                }


                /*Ελέγχουμε εάν η απάντηση(εις) είναι valid:*/
                bool _validate = false;
                object _valueToValidate = default(object);

                if (q.ValidationBehavior != ValidationMode.DoNotValidate && q.HasMissingValues == false && _totalAnswers > 0)
                {
                    try
                    {
                        if (q.QuestionType == QuestionType.Date || (q.QuestionType == QuestionType.SingleLine && q.ValidationBehavior == ValidationMode.Date1 || q.QuestionType == QuestionType.SingleLine && q.ValidationBehavior == ValidationMode.Date2))
                        {
                            #region Grab date value
                            if (q.UseDateTimeControls)
                            {
                                _validate = true;
                                _valueToValidate = this.RuntimeSession[q.HtmlQuestionId];
                            }
                            else
                            {
                                if (q.ValidationBehavior == ValidationMode.Date1)
                                {
                                    _validate = true;
                                    _valueToValidate = string.Format("{0:00}/{1:00}/{2:0000}", Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "_MONTH"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "_DAY"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "_YEAR"].ToString()));
                                }
                                else if (q.ValidationBehavior == ValidationMode.Date2)
                                {
                                    _validate = true;
                                    _valueToValidate = string.Format("{0:00}/{1:00}/{2:0000}", Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "_DAY"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "_MONTH"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "_YEAR"].ToString()));
                                }
                            }
                            #endregion
                        }
                        else if (q.QuestionType == QuestionType.SingleLine || q.QuestionType == QuestionType.Integer || q.QuestionType == QuestionType.Decimal || q.QuestionType == QuestionType.Range)
                        {
                            #region Grab simple value
                            _validate = true;
                            _valueToValidate = this.RuntimeSession[q.HtmlQuestionId];
                            #endregion
                        }
                        else if (q.OptionalInputBox == true)
                        {
                            #region Grab OptionalInputBox value
                            if (q.QuestionType == QuestionType.OneFromMany)
                            {
                                var _value = this.RuntimeSession[q.HtmlQuestionId] as string;
                                if (_value == q.HtmlQuestionId + "OptionalInputBox_")
                                {
                                    /*User has selected the Other Option!*/
                                    _validate = true;
                                    if (q.ValidationBehavior == ValidationMode.Date1)
                                    {
                                        _valueToValidate = string.Format("{0:00}/{1:00}/{2:0000}", Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_MONTH"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_DAY"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_YEAR"].ToString()));
                                    }
                                    else if (q.ValidationBehavior == ValidationMode.Date2)
                                    {
                                        _valueToValidate = string.Format("{0:00}/{1:00}/{2:0000}", Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_DAY"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_MONTH"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_YEAR"].ToString()));
                                    }
                                    else
                                    {
                                        _valueToValidate = this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput"];
                                    }
                                }
                            }
                            else if (q.QuestionType == QuestionType.ManyFromMany)
                            {
                                var _value = this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_"] as string;
                                if (_value == "1")
                                {
                                    /*User has selected the Other Option!*/
                                    _validate = true;
                                    if (q.ValidationBehavior == ValidationMode.Date1)
                                    {
                                        _valueToValidate = string.Format("{0:00}/{1:00}/{2:0000}", Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_MONTH"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_DAY"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_YEAR"].ToString()));
                                    }
                                    else if (q.ValidationBehavior == ValidationMode.Date2)
                                    {
                                        _valueToValidate = string.Format("{0:00}/{1:00}/{2:0000}", Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_DAY"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_MONTH"].ToString()), Int32.Parse(this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_YEAR"].ToString()));
                                    }
                                    else
                                    {
                                        _valueToValidate = this.RuntimeSession[q.HtmlQuestionId + "OptionalInputBox_userinput"];
                                    }
                                }

                            }
                       
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        _validate = true;
                        var message = string.Format("CollectAndvalidateAnswers Exception while servicing '{0}'", Context.Request.RawUrl);
                        Logger.Warn(message, ex);
                    }
                }

                if (_validate)
                {
                    #region
                    if (_valueToValidate != null)
                    {
                        #region
                        switch (q.ValidationBehavior)
                        {
                            case ValidationMode.TextOfSpecificLength:
                                {
                                    var inclusiveStart = Int32.Parse(q.ValidationField1);
                                    var inclusiveEnd = Int32.Parse(q.ValidationField2);
                                    var inputLength = _valueToValidate.ToString().Length;
                                    if (inputLength > inclusiveEnd || inputLength < inclusiveStart)
                                    {
                                        q.HasValidationErrors = true;
                                        page.HasValidationErrors = true;
                                    }
                                }
                                break;
                            case ValidationMode.WholeNumber:
                                {
                                    try
                                    {
                                        var inclusiveStart = Convert.ToInt32(q.ValidationField1);
                                        var inclusiveEnd = Convert.ToInt32(q.ValidationField2);
                                        var inputInteger = Int32.Parse(_valueToValidate.ToString(), CultureInfo.InvariantCulture);
                                        if (inputInteger > inclusiveEnd || inputInteger < inclusiveStart)
                                        {
                                            q.HasValidationErrors = true;
                                            page.HasValidationErrors = true;
                                        }
                                    }
                                    catch
                                    {
                                        q.HasValidationErrors = true;
                                        page.HasValidationErrors = true;
                                    }
                                }
                                break;
                            case ValidationMode.DecimalNumber:
                                try
                                {
                                    var inclusiveStart = Convert.ToDouble(q.ValidationField1, CultureInfo.InvariantCulture);
                                    var inclusiveEnd = Convert.ToDouble(q.ValidationField2, CultureInfo.InvariantCulture);
                                    var inputDouble = Double.Parse(_valueToValidate.ToString(), CultureInfo.InvariantCulture);
                                    if (inputDouble > inclusiveEnd || inputDouble < inclusiveStart)
                                    {
                                        q.HasValidationErrors = true;
                                        page.HasValidationErrors = true;
                                    }
                                }
                                catch
                                {
                                    q.HasValidationErrors = true;
                                    page.HasValidationErrors = true;
                                }
                                break;
                            case ValidationMode.Date1:
                                try
                                {
                                    var date1 = DateTime.ParseExact(_valueToValidate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    q.HasValidationErrors = true;
                                    page.HasValidationErrors = true;
                                }
                                break;
                            case ValidationMode.Date2:
                                try
                                {
                                    var date2 = DateTime.ParseExact(_valueToValidate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    q.HasValidationErrors = true;
                                    page.HasValidationErrors = true;
                                }
                                break;
                            case ValidationMode.Email:
                                if (!Utility.EmailIsValid(_valueToValidate.ToString()))
                                {
                                    q.HasValidationErrors = true;
                                    page.HasValidationErrors = true;
                                }
                                break;
                            case ValidationMode.RegularExpression:
                                break;
                        }
                        #endregion
                    }
                    else
                    {
                        q.HasValidationErrors = true;
                        page.HasValidationErrors = true;
                    }
                    #endregion
                }
                #endregion
            }


            if (page.HasMissingValues || page.HasValidationErrors)
                return false;

            return true;
        }
        #endregion

       
        void RenderSurveyBody(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "surveyBody");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            if (CurrentPageType == PageType.NormalPage || CurrentPageType == PageType.FirstPage || CurrentPageType == PageType.LastPage)
            {
                RenderSurveyPage(writer, CurrentPage);
            }
            else if (CurrentPageType == PageType.Welcome)
            {
                RenderWelcomePage(writer);
            }
            else if (CurrentPageType == PageType.Goodbye)
            {
                RenderGoodbyePage(writer);
            }
            else if (CurrentPageType == PageType.Disqualification)
            {
                RenderDisqualificationPage(writer);
            }
            else if (CurrentPageType == PageType.EndSurvey)
            {
                RenderEndSurveyPage(writer);
            }

            writer.RenderEndTag();
        }
        void RenderSurveyNavigation(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "surveyNavigation");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            if(this.CurrentPageType == PageType.Welcome)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Name, "NextButton");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "NextButton");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode(this.Survey.StartButton));
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.WriteLine();
            }
            else if(this.CurrentPageType == PageType.Goodbye)
            {
                //writer.AddAttribute(HtmlTextWriterAttribute.Name, "NextButton");
                //writer.AddAttribute(HtmlTextWriterAttribute.Id, "NextButton");
                //writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode("Close"));
                //writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "OnclientClose()");
                //writer.RenderBeginTag(HtmlTextWriterTag.Input);
                //writer.RenderEndTag();
                //writer.WriteLine();
            }
            else if(this.CurrentPageType == PageType.Disqualification)
            {
                //writer.AddAttribute(HtmlTextWriterAttribute.Name, "NextButton");
                //writer.AddAttribute(HtmlTextWriterAttribute.Id, "NextButton");
                //writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode("Close"));
                //writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "OnclientClose()");
                //writer.RenderBeginTag(HtmlTextWriterTag.Input);
                //writer.RenderEndTag();
                //writer.WriteLine();
            }
            else if (this.CurrentPageType == PageType.EndSurvey)
            {
                //writer.AddAttribute(HtmlTextWriterAttribute.Name, "NextButton");
                //writer.AddAttribute(HtmlTextWriterAttribute.Id, "NextButton");
                //writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode("Close"));
                //writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "OnclientClose()");
                //writer.RenderBeginTag(HtmlTextWriterTag.Input);
                //writer.RenderEndTag();
                //writer.WriteLine();
            }
            else if (this.CurrentPageType == PageType.FirstPage)
            {
                if(this.Survey.ShowWelcomePage)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, "PreviousButton");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "PreviousButton");
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode(this.Survey.PreviousButton));
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
                    writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    writer.RenderEndTag();
                    writer.WriteLine();
                }

                writer.AddAttribute(HtmlTextWriterAttribute.Name, "NextButton");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "NextButton");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode(this.Survey.NextButton));
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.WriteLine();
            }
            else if (this.CurrentPageType == PageType.NormalPage)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Name, "PreviousButton");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "PreviousButton");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode(this.Survey.PreviousButton));
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.WriteLine();

                writer.AddAttribute(HtmlTextWriterAttribute.Name, "NextButton");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "NextButton");
                if (this.NextPage != null)
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode(this.Survey.NextButton));
                else
                    writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode(this.Survey.DoneButton));
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();
                writer.WriteLine();
            }



            writer.RenderEndTag();



        }

        void RenderWelcomePage(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "surveyWelcome");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.Write(this.Survey.WelcomeHtml);

            writer.RenderEndTag();
        }
        void RenderGoodbyePage(HtmlTextWriter writer)
        {
            if (this.RuntimeSession.ResponseType == ResponseType.Manual)
            {
                /*Θέλουμε να κλείσουμε το παράθυρο:*/
                writer.Write("<script type=\"text/javascript\">setTimeout(\"window.close();\", 100);</script>");
            }
            else
            {
                if (string.IsNullOrEmpty(this.Survey.GoodbyeHtml))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "surveyGoodbye");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.Write("<div style=\"margin: 80px auto 24px auto;font-size: 2em; font-weight: bold;\">Thanks for completing this survey.</div>");

                    writer.RenderEndTag();
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "surveyGoodbye");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    writer.Write(this.Survey.GoodbyeHtml);

                    writer.RenderEndTag();
                }
            }
        }
        void RenderDisqualificationPage(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(this.Survey.DisqualificationHtml))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "surveyDisqualification");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.Write("<div style=\"margin: 80px auto 24px auto;font-size: 2em; font-weight: bold;\">We’re sorry. You do not meet the qualifications for this survey. We sincerely thank you and appreciate your time, dedication, and continued participation in our online surveys.</div>");

                writer.RenderEndTag();
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "surveyDisqualification");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.Write(this.Survey.DisqualificationHtml);

                writer.RenderEndTag();
            }
        }
        void RenderEndSurveyPage(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "surveyEnd");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.Write("<div style=\"margin: 80px auto 24px auto;font-size: 2em; font-weight: bold;\">End of survey!</div>");
            writer.Write("<div style=\"margin: 24px auto 24px auto;font-size: 1.2em;\">Thank you for taking time out to participate in our survey.  We truly value the information you have provided.</div>");

            writer.RenderEndTag();
        }

        void RenderSurveyPage(HtmlTextWriter writer, VLSurveyPage page)
        {
            if (Survey.ShowPageTitles)
            {
                RenderPageTitle(writer, page);
            }
            if (this.Survey.UseProgressBar && this.Survey.ProgressBarPosition == ProgressBarPosition.AtTop)
            {
                RenderProgressBar(writer, page);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "questions");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                foreach (var question in this.Questions)
                {
                    if (question.MasterQuestion.HasValue)
                        continue;

                    RenderQuestion(writer, page, question);
                }
            }
            writer.RenderEndTag();
        }
        void RenderPageTitle(HtmlTextWriter writer, VLSurveyPage page)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "pageHeader");
            writer.RenderBeginTag(HtmlTextWriterTag.H2);
            if (Survey.UsePageNumbering)
            {
                //WholeSurveyPageCounter++;
                writer.Write(string.Format("<abbr title=\"Page {0}\" class=\"pageNumber\">{0}</abbr>. ", page.DisplayOrder));
            }
            HttpUtility.HtmlEncode(page.ShowTitle, writer);

            writer.RenderEndTag();
        }

        void RenderProgressBar(HtmlTextWriter writer, VLSurveyPage page)
        {
            Int32 totalPages = this.Survey.TotalPages;
            Int32 shownPages = page.DisplayOrder;
            Int32 percentage = (shownPages * 100) / totalPages;

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "ProgBar");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
                writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    {
                        //
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "ProgTxt");
                        writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "noborder");
                            writer.AddAttribute(HtmlTextWriterAttribute.Title, string.Format("Page {0} of {1}",shownPages, totalPages) );
                            writer.RenderBeginTag("abbr");
                            writer.Write(string.Format("{0}/{1}", shownPages, totalPages));
                            writer.RenderEndTag();
                        }
                        writer.RenderEndTag();
                        //
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "ProgTxt");
                        writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        {
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "BarArea");
                            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, percentage.ToString()+"%");
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);
                            writer.RenderEndTag();
                        }
                        writer.RenderEndTag();
                        //
                        //writer.AddAttribute(HtmlTextWriterAttribute.Class, "ProgTxt");
                        //writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
                        //writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        //{
                        //    writer.AddAttribute(HtmlTextWriterAttribute.Class, "noborder");
                        //    writer.AddAttribute(HtmlTextWriterAttribute.Title, percentage.ToString() + " percent complete");
                        //    writer.RenderBeginTag("abbr");
                        //    writer.Write(percentage.ToString() + "%");
                        //    writer.RenderEndTag();
                        //}
                        //writer.RenderEndTag();
                    }
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }

        void RenderQuestion(HtmlTextWriter writer, VLSurveyPage page, VLSurveyQuestion question, bool insideComposite = false)
        {

            if (!insideComposite)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionBox");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                if (question.QuestionType == QuestionType.DescriptiveText)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionHeaderTransparent");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionHeader");
                }
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                if (question.HasMissingValues || question.HasValidationErrors)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "validationError");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    if (question.HasMissingValues)
                    {
                        writer.Write(HttpUtility.HtmlEncode(question.RequiredMessage));
                    }
                    else if (question.HasValidationErrors)
                    {
                        writer.Write(HttpUtility.HtmlEncode(question.ValidationMessage));
                    }
                    writer.RenderEndTag();
                }
                if (question.IsRequired)
                {
                    if (Survey.RequiredHighlightType == RequiredHighlightType.UseAsterisk)
                    {
                        if (question.HasMissingValues || question.HasValidationErrors)
                            writer.Write("<div class=\"requiredMarker red\"><abbr title=\"Required\">*</abbr></div>");
                        else
                            writer.Write("<div class=\"requiredMarker\"><abbr title=\"Required\">*</abbr></div>");
                    }
                }
                if (Survey.UseQuestionNumbering)
                {
                    if (Survey.QuestionNumberingType == QuestionNumberingType.NumberingEntireSurvey)
                    {
                        PageQuestionsCounter++;
                        writer.Write(string.Format("<abbr title=\"Question {0}\" class=\"questionNumber\">{0}</abbr>. ", PageQuestionsCounter));
                    }
                    else
                    {
                        //WholeSurveyQuestionsCounter++;
                        writer.Write(string.Format("<abbr title=\"Question {0}\" class=\"questionNumber\">{0}</abbr>. ", question.DisplayOrder));
                    }
                }

                HttpUtility.HtmlEncode(question.QuestionText, writer);
                if(!string.IsNullOrWhiteSpace(question.Description))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "questionDescription");
                    writer.RenderBeginTag(HtmlTextWriterTag.P);
                    HttpUtility.HtmlEncode(question.Description, writer);
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();//Div.questionHeader

                writer.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("questionControl {0}", question.QuestionType.ToString().ToLowerInvariant()));
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
            }



            var renderer = HtmlRenderers.GetQuestionRenderer(SurveyManager, writer, RuntimeSession, question.QuestionType);
            renderer.RenderQuestion(this.Survey, page, question);



            if (!insideComposite)
            {
                writer.RenderEndTag();//Div.questionControl

                writer.RenderEndTag();//Div.questionBox
            }
            writer.WriteLine();
        }
    }
}