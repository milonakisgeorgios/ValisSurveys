using log4net;
using System;
using System.Globalization;
using System.Web;
using Valis.Core;
using ValisServer.Runtime;

namespace ValisServer
{
    public class SurveyRuntime : IHttpModule
    {
        /// <summary>
        /// This is the log4Net logger!
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(typeof(SurveyRuntime));

        public void Dispose()
        {

        }

        string GetIPAddress(HttpContext context)
        {
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            context.PostResolveRequestCache += OnPostResolveRequestCache;
        }


        private void OnBeginRequest(object sender, EventArgs e)
        {
            RuntimeRequestType requestType = RuntimeRequestType.Unknown;

            /*
             * Τα request που ζητάνε το 'Survey Runtime', αρχίζουν με συγκεκριμένο τρόπο.
             *      s   -> Preview
             *      em  -> Collector - Email
             *      w   -> Collector - WebLink
             */
            #region Filter Out Unknown Requests
            if (((HttpApplication)sender).Context.Request.Url.AbsolutePath.StartsWith("/s/", true, CultureInfo.InvariantCulture))
            {
                requestType = RuntimeRequestType.Preview;
            }
            if(requestType == RuntimeRequestType.Unknown)
            {
                if (((HttpApplication)sender).Context.Request.Url.AbsolutePath.StartsWith("/em/", true, CultureInfo.InvariantCulture))
                {
                    requestType = RuntimeRequestType.Collector_Email;
                }
            }
            if (requestType == RuntimeRequestType.Unknown)
            {
                if (((HttpApplication)sender).Context.Request.Url.AbsolutePath.StartsWith("/w/", true, CultureInfo.InvariantCulture))
                {
                    requestType = RuntimeRequestType.Collector_WebLink;
                }
            }
            if (requestType == RuntimeRequestType.Unknown)
            {
                if (((HttpApplication)sender).Context.Request.Url.AbsolutePath.StartsWith("/wm/", true, CultureInfo.InvariantCulture))
                {
                    requestType = RuntimeRequestType.Manual_webLink;
                }
            }
            if (requestType == RuntimeRequestType.Unknown)
            {
                if (((HttpApplication)sender).Context.Request.Url.AbsolutePath.StartsWith("/emm/", true, CultureInfo.InvariantCulture))
                {
                    requestType = RuntimeRequestType.Manual_Email;
                }
            }
            if (requestType == RuntimeRequestType.Unknown)
            {
                Logger.DebugFormat("RequestType Unknown: {0}", ((HttpApplication)sender).Context.Request.Url.AbsolutePath);
                return;
            }
            #endregion


            try
            {
                ValisHttpApplication application = (ValisHttpApplication)sender;
                HttpContext context = application.Context;

                var host = context.Request.Url.Host;
                var port = context.Request.Url.Port;
                var absolutePath = context.Request.Url.AbsolutePath;
                var rawUrl = context.Request.RawUrl;

                System.Diagnostics.Debug.WriteLine(string.Format("SurveyRuntime::BeginRequest -> host={0}, port={1}, absolutePath={2}", host, port, absolutePath));


                if (requestType == RuntimeRequestType.Preview)
                {
                    context.Items["RequestType"] = requestType;
                    PreviewRequest(application, context, absolutePath);
                }
                else if (requestType == RuntimeRequestType.Collector_WebLink)
                {
                    context.Items["RequestType"] = requestType;
                    WeblinkCollectorRequest(application, context, absolutePath);
                }
                else if (requestType == RuntimeRequestType.Collector_Email)
                {
                    context.Items["RequestType"] = requestType;
                    EmailCollectorRequest(application, context, absolutePath);
                }
                else if (requestType == RuntimeRequestType.Manual_webLink)
                {
                    context.Items["RequestType"] = requestType;
                    ManualWeblinkCollectorRequest(application, context, absolutePath);
                }
                else if (requestType == RuntimeRequestType.Manual_Email)
                {
                    context.Items["RequestType"] = requestType;
                    ManualEmailCollectorRequest(application, context, absolutePath);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(((HttpApplication)sender).Context.Request.RawUrl, ex);
                System.Diagnostics.Debug.WriteLine(string.Format("SurveyRuntime::BeginRequest -> {0}", ex.Message));
                throw;
            }
        }

        void PreviewRequest(ValisHttpApplication application, HttpContext context, string absolutePath)
        {
            /*Παίρνουμε τα URL segments:*/
            string[] segments = absolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            /*s, publicid, language, sessionid*/
            if (segments.Length < 3)
            {
                //throw new VLException("PreviewRequest -> segments.Length < 3");
                Logger.Error(context.Request.RawUrl, new VLException("PreviewRequest -> segments.Length < 3"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 1001;
                _handler.ErrorMessage = "PreviewRequest -> segments.Length < 3";
                application.Context.RemapHandler(_handler);
                return;
            }

            #region Βρίσκουμε την γλώσσα <segments[2]> (context.Items["language"])
            VLLanguage language = BuiltinLanguages.UnknownLanguage;
            foreach (var item in BuiltinLanguages.Languages)
            {
                if (String.Equals(item.TwoLetterISOCode, segments[2], StringComparison.OrdinalIgnoreCase))
                {
                    language = item;
                    break;
                }
            }
            if (language == BuiltinLanguages.UnknownLanguage)
            {
                //throw new VLException("PreviewRequest -> language == BuiltinLanguages.UnknownLanguage");
                Logger.Error(context.Request.RawUrl, new VLException("PreviewRequest -> language == BuiltinLanguages.UnknownLanguage"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 1002;
                _handler.ErrorMessage = "PreviewRequest -> language == BuiltinLanguages.UnknownLanguage";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["language"] = language;
            #endregion

            #region Βρίσκουμε το survey <segments[1]> (context.Items["survey"])
            var survey = application.SurveyManager.GetSurveyByPublicId(segments[1], language.LanguageId);
            if (survey == null)
            {
                //throw new VLException("PreviewRequest -> survey == null");
                Logger.Error(context.Request.RawUrl, new VLException("PreviewRequest -> survey == null"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 1003;
                _handler.ErrorMessage = "PreviewRequest -> survey == null";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["survey"] = survey;
            #endregion

            #region Βρίσκουμε το Session <segments[3]> (context.Items["runtimeSession"])
            VLRuntimeSession session = null;
            if (segments.Length > 3)
            {
                try
                {
                    Guid _sessionId = new Guid(segments[3]);
                    session = application.SurveyManager.AcquireSession(_sessionId);
                }
                catch(Exception ex)
                {
                    Logger.Error(context.Request.RawUrl, ex);
                }
            }
            if (session == null)
            {
                session = application.SurveyManager.CreateSession(survey.SurveyId, RuntimeRequestType.Preview, ResponseType.Default, collector: null);
            }
            else
            {
                session.IsRessurected = true;
            }
            context.Items["runtimeSession"] = session;
            #endregion


            //REMAP HANDLER!!!!
            application.Context.RemapHandler(new PreviewHandler());
        }

        void EmailCollectorRequest(ValisHttpApplication application, HttpContext context, string absolutePath)
        {
            /*Παίρνουμε τα URL segments:*/
            string[] segments = absolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            /*em, PublicId, RecipientKey, CollectorId, language, sessionid*/
            if (segments.Length < 5)
            {
                //throw new VLException("EmailCollectorRequest -> segments.Length < 5");
                Logger.Error(context.Request.RawUrl, new VLException("EmailCollectorRequest -> segments.Length < 5"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 3001;
                _handler.ErrorMessage = "EmailCollectorRequest -> segments.Length < 5";
                application.Context.RemapHandler(_handler);
                return;
            }


            #region Βρίσκουμε την γλώσσα <segments[4]> (context.Items["language"])
            VLLanguage language = BuiltinLanguages.UnknownLanguage;
            foreach (var item in BuiltinLanguages.Languages)
            {
                if (String.Equals(item.TwoLetterISOCode, segments[4], StringComparison.OrdinalIgnoreCase))
                {
                    language = item;
                    break;
                }
            }
            if (language == BuiltinLanguages.UnknownLanguage)
            {
                //throw new VLException("EmailCollectorRequest -> language == BuiltinLanguages.UnknownLanguage");
                Logger.Error(context.Request.RawUrl, new VLException("EmailCollectorRequest -> language == BuiltinLanguages.UnknownLanguage"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 3002;
                _handler.ErrorMessage = "EmailCollectorRequest -> language == BuiltinLanguages.UnknownLanguage";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["language"] = language;
            #endregion

            #region Βρίσκουμε το survey <segments[1]> (context.Items["survey"])
            var survey = application.SurveyManager.GetSurveyByPublicId(segments[1], language.LanguageId);
            if (survey == null)
            {
                //throw new VLException("EmailCollectorRequest -> survey == null");
                Logger.Error(context.Request.RawUrl, new VLException("EmailCollectorRequest -> survey == null"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 3003;
                _handler.ErrorMessage = "EmailCollectorRequest -> survey == null";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["survey"] = survey;
            #endregion

            #region Βρίσκουμε το collector <segments[3]> (context.Items["collector"])
            var collector = application.SurveyManager.GetCollectorById(Int32.Parse(segments[3]), language.LanguageId);
            if (collector == null)
            {
                //throw new VLException("EmailCollectorRequest -> collector == null");
                Logger.Error(context.Request.RawUrl, new VLException("EmailCollectorRequest -> collector == null"));
                var _handler = new CannotExecuteSurvey();
                _handler.ErrorCode = 3005;
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["collector"] = collector;
            #endregion


            /*Εάν ο collector, είναι κλειστός τερματίζουμε σε αυτό το σημείο*/
            if (collector.Status != CollectorStatus.Open)
            {
                application.Context.RemapHandler(new ClosedCollectorHandler());
                return;
            }


            #region Βρίσκουμε το recipient <segments[2]> (context.Items["recipient"])
            var recipient = application.SurveyManager.GetRecipientByKey(collector.CollectorId, segments[2]);
            if (recipient == null)
            {
                //throw new VLException("EmailCollectorRequest -> recipient == null");
                Logger.Error(context.Request.RawUrl, new VLException("EmailCollectorRequest -> recipient == null"));
                var _handler = new CannotExecuteSurvey();
                _handler.ErrorCode = 3006;
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["recipient"] = recipient;
            #endregion

            /*Εάν ο recipient έχει τρέξει ήδη το survey, δεν τον αφήνουμε να συνεχίσει:*/
            if (recipient != null && (recipient.Status == RecipientStatus.Completed || recipient.Status == RecipientStatus.Disqualified))
            {
                application.Context.RemapHandler(new CompletedRecipientHandler());
                return;
            }

            #region Βρίσκουμε το Session <segments[5]> (context.Items["runtimeSession"])
            VLRuntimeSession session = null;
            if (segments.Length > 5)
            {
                try
                {
                    Guid _sessionId = new Guid(segments[5]);
                    session = application.SurveyManager.AcquireSession(_sessionId);
                }
                catch (Exception ex)
                {
                    Logger.Error(context.Request.RawUrl, ex);
                }
            }
            if (session == null)
            {
                session = application.SurveyManager.AcquireSessionByRecipientKey(recipient.RecipientKey, collector.CollectorId);
                if (session == null)
                {
                    session = application.SurveyManager.CreateSession(survey.SurveyId, RuntimeRequestType.Collector_Email, ResponseType.Default, context.Request.UserAgent, collector.CollectorId, recipient.RecipientKey, GetIPAddress(context));
                }
                else
                {
                    session.IsRessurected = true;
                }
            }
            else
            {
                session.IsRessurected = true;
            }
            context.Items["runtimeSession"] = session;
            #endregion

            #region ενεργοποιούμε το recipient, (εάν δεν είναι ήδη ενεργοποιημένο):
            if (recipient.ActivationDate == null)
            {
                recipient.ActivationDate = Utility.UtcNow();
                recipient.Status = RecipientStatus.OpenSurvey;
                context.Items["recipient"] = application.SurveyManager.UpdateRecipient(recipient);
            }
            #endregion

            //REMAP HANDLER!!!!
            application.Context.RemapHandler(new DefaultHandler());
        }
        
        /// <summary>
        /// Δημιουργεί ένα νέο HttpCookie που λήγει σε 28 ημέρες!
        /// <para>Αυτό το cookie φέρει το κλειδί του Collector για το οποίο εκδίδεται, και το κλειδί το Recipient για
        /// τον οποίο παράχθηκε</para>
        /// <para></para>
        /// </summary>
        /// <param name="collector"></param>
        /// <param name="recipientWebKey"></param>
        /// <returns></returns>
        HttpCookie CreateWebKeyCookie(VLCollector collector, string recipientWebKey)
        {
            HttpCookie surveyCookie = new HttpCookie("Collector$" + collector.WebLink);
            surveyCookie.Values.Add("RecipientWebKey", recipientWebKey);
            surveyCookie.Expires = DateTime.UtcNow.AddDays(28);
            return surveyCookie;
        }
        string GetRecipientWebKeyFromCookies(VLCollector collector, HttpRequest request)
        {
            HttpCookie surveyCookie = request.Cookies["Collector$" + collector.WebLink];
            if (surveyCookie != null)
            {
                return surveyCookie.Values["RecipientWebKey"].ToString();
            }
            return null;
        }
        void WeblinkCollectorRequest(ValisHttpApplication application, HttpContext context, string absolutePath)
        {
            /*Παίρνουμε τα URL segments:*/
            string[] segments = absolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            /*w, WebLink, language, sessionid*/
            if (segments.Length < 3)
            {
                //throw new VLException("WeblinkCollectorRequest -> segments.Length < 3");
                Logger.Error(context.Request.RawUrl, new VLException("WeblinkCollectorRequest -> segments.Length < 3"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 2001;
                _handler.ErrorMessage = "WeblinkCollectorRequest -> segments.Length < 3";
                application.Context.RemapHandler(_handler);
                return;
            }

            #region Βρίσκουμε την γλώσσα <segments[2]> (context.Items["language"])
            VLLanguage language = BuiltinLanguages.UnknownLanguage;
            foreach (var item in BuiltinLanguages.Languages)
            {
                if (String.Equals(item.TwoLetterISOCode, segments[2], StringComparison.OrdinalIgnoreCase))
                {
                    language = item;
                    break;
                }
            }
            if (language == BuiltinLanguages.UnknownLanguage)
            {
                //throw new VLException("WeblinkCollectorRequest -> language == BuiltinLanguages.UnknownLanguage");
                Logger.Error(context.Request.RawUrl, new VLException("WeblinkCollectorRequest -> language == BuiltinLanguages.UnknownLanguage"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 2002;
                _handler.ErrorMessage = "WeblinkCollectorRequest -> language == BuiltinLanguages.UnknownLanguage";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["language"] = language;
            #endregion

            #region Βρίσκουμε το collector <segments[1]> (context.Items["collector"])
            var collector = application.SurveyManager.GetCollectorByWebLink(segments[1], language.LanguageId);
            if (collector == null)
            {
                //throw new VLException("WeblinkCollectorRequest -> collector == null");
                Logger.Error(context.Request.RawUrl, new VLException("WeblinkCollectorRequest -> collector == null"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 2003;
                _handler.ErrorMessage = "WeblinkCollectorRequest -> collector == null";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["collector"] = collector;
            #endregion


            #region Βρίσκουμε το survey:
            var survey = application.SurveyManager.GetSurveyById(collector.Survey, collector.TextsLanguage);
            if (survey == null)
            {
                //throw new VLException("WeblinkCollectorRequest -> survey == null");
                Logger.Error(context.Request.RawUrl, new VLException("WeblinkCollectorRequest -> survey == null"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 2004;
                _handler.ErrorMessage = "WeblinkCollectorRequest -> survey == null";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["survey"] = survey;
            #endregion


            /*Εάν ο collector, είναι κλειστός τερματίζουμε σε αυτό το σημείο*/
            if(collector.Status != CollectorStatus.Open)
            {
                application.Context.RemapHandler(new ClosedCollectorHandler());
                return;
            }


            #region Βρίσκουμε το Session <segments[3]> (context.Items["runtimeSession"])
            VLRuntimeSession session = null;
            if (segments.Length > 3)
            {
                #region
                try
                {
                    Guid _sessionId = new Guid(segments[3]);
                    session = application.SurveyManager.AcquireSession(_sessionId);
                    if (session != null)
                    {
                        /*Την συνεδρία την βρήκαμε απο το URL. Αλλά πρέπει να υπάρχει και στα cookies:*/
                        var recipientWebKey = GetRecipientWebKeyFromCookies(collector, context.Request);
                        if (string.IsNullOrEmpty(recipientWebKey))
                        {
                            //Δεν υπάρχει στα cookies, ->Δημιουργούμε το cookie:
                            context.Response.Cookies.Add(CreateWebKeyCookie(collector, session.RecipientKey));
                        }
                        else
                        {
                            //Υπάρχει στα cookies, ->Πρόκειται για το ίδιο κλειδί
                            if (!string.Equals(recipientWebKey, session.RecipientKey, StringComparison.OrdinalIgnoreCase))
                            {
                                /*Οπα, τι γίνεται εδώ?*/
                                System.Diagnostics.Debugger.Break();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(context.Request.RawUrl, ex);
                }
                #endregion
            }
            if (session == null)
            {
                var recipientWebKey = GetRecipientWebKeyFromCookies(collector, context.Request);
                if (!string.IsNullOrEmpty(recipientWebKey))
                {
                    session = application.SurveyManager.AcquireSessionByRecipientWebKey(recipientWebKey, collector.CollectorId);
                }

                if (session == null)
                {
                    /*Δημιουργούμε ένα εικονικό (ενεργοποιημένο) recipient:*/
                    var _vrecipient = application.SurveyManager.CreateRecipientVirtual(collector.CollectorId);
                    /*Δημιουργούμε το cookie:*/
                    context.Response.Cookies.Add(CreateWebKeyCookie(collector, _vrecipient.RecipientKey));
                    /*δημιουργούμε το session:*/
                    session = application.SurveyManager.CreateSession(survey.SurveyId, RuntimeRequestType.Collector_WebLink, ResponseType.Default, context.Request.UserAgent, collector.CollectorId, recipientKey: _vrecipient.RecipientKey, recipientIP: GetIPAddress(context));
                    if(collector.UseCredits && collector.CreditType.HasValue && collector.CreditType.Value == CreditType.ClickType)
                    {
                        session = application.SurveyManager.ChargePaymentForClick(session.SessionId, collector.CollectorId, collector.Survey);
                        if(session.IsCharged == false)
                        {
                            /*Δεν υπάρχουν διαθέσιμα credits!*/
                            //throw new VLException("WeblinkCollectorRequest -> noCredits!");
                            Logger.Error(context.Request.RawUrl, new VLException("WeblinkCollectorRequest -> noCredits!"));
                            var _handler = new CannotExecuteSurvey();
                            _handler.ErrorCode = 2006;
                            application.Context.RemapHandler(_handler);
                            return;
                        }
                    }
                }
                else
                {
                    session.IsRessurected = true;
                }
            }
            else
            {
                session.IsRessurected = true;
            }
            context.Items["runtimeSession"] = session;
            #endregion


            #region βρίσκουμε τον εικονικό Recipient:
            var recipient = application.SurveyManager.GetRecipientByKey(collector.CollectorId, session.RecipientKey);
            if (recipient == null)
            {
                //throw new VLException("WeblinkCollectorRequest -> virtual recipient == null");
                Logger.Error(context.Request.RawUrl, new VLException("WeblinkCollectorRequest -> virtual recipient == null"));
                var _handler = new CannotExecuteSurvey();
                _handler.ErrorCode = 2007;
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["recipient"] = recipient;
            #endregion


            /*Εάν ο recipient έχει τρέξει ήδη το survey, δεν τον αφήνουμε να συνεχίσει:*/
            if (recipient != null && (recipient.Status == RecipientStatus.Completed || recipient.Status == RecipientStatus.Disqualified))
            {
                application.Context.RemapHandler(new CompletedRecipientHandler());
                return;
            }

            //REMAP HANDLER!!!!
            application.Context.RemapHandler(new DefaultHandler());
        }

        /// <summary>
        /// Λειτουργεί όπως το WeblinkCollectorRequest, απλώς εδώ δεν δημιουργούμε VirtualRecipient και ούτε τοποθετούμε στα Cookies 
        /// του browser κάτι.
        /// </summary>
        /// <param name="application"></param>
        /// <param name="context"></param>
        /// <param name="absolutePath"></param>
        void ManualWeblinkCollectorRequest(ValisHttpApplication application, HttpContext context, string absolutePath)
        {
            /*Παίρνουμε τα URL segments:*/
            string[] segments = absolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            /*wm, WebLink, language, sessionid*/
            if (segments.Length < 3)
            {
                //throw new VLException("ManualWeblinkCollectorRequest -> segments.Length < 3");
                Logger.Error(context.Request.RawUrl, new VLException("ManualWeblinkCollectorRequest -> segments.Length < 3"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 4001;
                _handler.ErrorMessage = "ManualWeblinkCollectorRequest -> segments.Length < 3";
                application.Context.RemapHandler(_handler);
                return;
            }

            #region Βρίσκουμε την γλώσσα <segments[2]> (context.Items["language"])
            VLLanguage language = BuiltinLanguages.UnknownLanguage;
            foreach (var item in BuiltinLanguages.Languages)
            {
                if (String.Equals(item.TwoLetterISOCode, segments[2], StringComparison.OrdinalIgnoreCase))
                {
                    language = item;
                    break;
                }
            }
            if (language == BuiltinLanguages.UnknownLanguage)
            {
                //throw new VLException("ManualWeblinkCollectorRequest -> language == BuiltinLanguages.UnknownLanguage");
                Logger.Error(context.Request.RawUrl, new VLException("ManualWeblinkCollectorRequest -> language == BuiltinLanguages.UnknownLanguage"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 4002;
                _handler.ErrorMessage = "ManualWeblinkCollectorRequest -> language == BuiltinLanguages.UnknownLanguage";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["language"] = language;
            #endregion

            #region Βρίσκουμε το collector <segments[1]> (context.Items["collector"])
            var collector = application.SurveyManager.GetCollectorByWebLink(segments[1], language.LanguageId);
            if (collector == null)
            {
                //throw new VLException("ManualWeblinkCollectorRequest -> collector == null");
                Logger.Error(context.Request.RawUrl, new VLException("ManualWeblinkCollectorRequest -> collector == null"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 4003;
                _handler.ErrorMessage = "ManualWeblinkCollectorRequest -> collector == null";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["collector"] = collector;
            #endregion


            #region Βρίσκουμε το survey:
            var survey = application.SurveyManager.GetSurveyById(collector.Survey, collector.TextsLanguage);
            if (survey == null)
            {
                //throw new VLException("ManualWeblinkCollectorRequest -> survey == null");
                Logger.Error(context.Request.RawUrl, new VLException("ManualWeblinkCollectorRequest -> survey == null"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 4004;
                _handler.ErrorMessage = "ManualWeblinkCollectorRequest -> survey == null";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["survey"] = survey;
            #endregion


            /*Εάν ο collector, είναι κλειστός τερματίζουμε σε αυτό το σημείο*/
            if (collector.Status != CollectorStatus.Open)
            {
                application.Context.RemapHandler(new ClosedCollectorHandler());
                return;
            }


            #region Βρίσκουμε το Session <segments[3]> (context.Items["runtimeSession"])
            VLRuntimeSession session = null;
            if (segments.Length > 3)
            {
                #region
                try
                {
                    Guid _sessionId = new Guid(segments[3]);
                    session = application.SurveyManager.AcquireSession(_sessionId);
                }
                catch (Exception ex)
                {
                    Logger.Error(context.Request.RawUrl, ex);
                }
                #endregion
            }
            if (session == null)
            {
                /*δημιουργούμε το session:*/
                session = application.SurveyManager.CreateSession(survey.SurveyId, RuntimeRequestType.Manual_webLink, ResponseType.Manual, context.Request.UserAgent, collector.CollectorId);
            }
            else
            {
                session.IsRessurected = true;
            }
            context.Items["runtimeSession"] = session;
            #endregion


            #region βρίσκουμε τον εικονικό Recipient:
            context.Items["recipient"] = null;
            #endregion

            
            //REMAP HANDLER!!!!
            application.Context.RemapHandler(new DefaultHandler());
        }

        /// <summary>
        /// Λειτουργεί όπως το EmailCollectorRequest, απλώς αλλάζει τις παραμέτρους δημιουργίας του RuntimeSession
        /// </summary>
        /// <param name="application"></param>
        /// <param name="context"></param>
        /// <param name="absolutePath"></param>
        void ManualEmailCollectorRequest(ValisHttpApplication application, HttpContext context, string absolutePath)
        {
            /*Παίρνουμε τα URL segments:*/
            string[] segments = absolutePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            /*em, PublicId, RecipientKey, CollectorId, language, sessionid*/
            if (segments.Length < 5)
            {
                //throw new VLException("ManualEmailCollectorRequest -> segments.Length < 5");
                Logger.Error(context.Request.RawUrl, new VLException("ManualEmailCollectorRequest -> segments.Length < 5"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 5001;
                _handler.ErrorMessage = "ManualEmailCollectorRequest -> segments.Length < 5";
                application.Context.RemapHandler(_handler);
                return;
            }


            #region Βρίσκουμε την γλώσσα <segments[4]> (context.Items["language"])
            VLLanguage language = BuiltinLanguages.UnknownLanguage;
            foreach (var item in BuiltinLanguages.Languages)
            {
                if (String.Equals(item.TwoLetterISOCode, segments[4], StringComparison.OrdinalIgnoreCase))
                {
                    language = item;
                    break;
                }
            }
            if (language == BuiltinLanguages.UnknownLanguage)
            {
                //throw new VLException("ManualEmailCollectorRequest -> language == BuiltinLanguages.UnknownLanguage");
                Logger.Error(context.Request.RawUrl, new VLException("ManualEmailCollectorRequest -> language == BuiltinLanguages.UnknownLanguage"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 5002;
                _handler.ErrorMessage = "ManualEmailCollectorRequest -> language == BuiltinLanguages.UnknownLanguage";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["language"] = language;
            #endregion

            #region Βρίσκουμε το survey <segments[1]> (context.Items["survey"])
            var survey = application.SurveyManager.GetSurveyByPublicId(segments[1], language.LanguageId);
            if (survey == null)
            {
                //throw new VLException("ManualEmailCollectorRequest -> survey == null");
                Logger.Error(context.Request.RawUrl, new VLException("ManualEmailCollectorRequest -> survey == null"));
                var _handler = new ExceptionOccuredHandler();
                _handler.ErrorCode = 5003;
                _handler.ErrorMessage = "ManualEmailCollectorRequest -> survey == null";
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["survey"] = survey;
            #endregion

            #region Βρίσκουμε το collector <segments[3]> (context.Items["collector"])
            var collector = application.SurveyManager.GetCollectorById(Int32.Parse(segments[3]), language.LanguageId);
            if (collector == null)
            {
                //throw new VLException("ManualEmailCollectorRequest -> collector == null");
                Logger.Error(context.Request.RawUrl, new VLException("ManualEmailCollectorRequest -> collector == null"));
                var _handler = new CannotExecuteSurvey();
                _handler.ErrorCode = 5005;
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["collector"] = collector;
            #endregion


            /*Εάν ο collector, είναι κλειστός τερματίζουμε σε αυτό το σημείο*/
            if (collector.Status != CollectorStatus.Open)
            {
                application.Context.RemapHandler(new ClosedCollectorHandler());
                return;
            }

            #region Βρίσκουμε το recipient <segments[2]> (context.Items["recipient"])
            var recipient = application.SurveyManager.GetRecipientByKey(collector.CollectorId, segments[2]);
            if (recipient == null)
            {
                //throw new VLException("ManualEmailCollectorRequest -> recipient == null");
                Logger.Error(context.Request.RawUrl, new VLException("ManualEmailCollectorRequest -> recipient == null"));
                var _handler = new CannotExecuteSurvey();
                _handler.ErrorCode = 5006;
                application.Context.RemapHandler(_handler);
                return;
            }
            context.Items["recipient"] = recipient;
            #endregion

            /*Εάν ο recipient έχει τρέξει ήδη το survey, δεν τον αφήνουμε να συνεχίσει:*/
            if (recipient != null && (recipient.Status == RecipientStatus.Completed || recipient.Status == RecipientStatus.Disqualified))
            {
                application.Context.RemapHandler(new CompletedRecipientHandler());
                return;
            }

            #region Βρίσκουμε το Session <segments[5]> (context.Items["runtimeSession"])
            VLRuntimeSession session = null;
            if (segments.Length > 5)
            {
                try
                {
                    Guid _sessionId = new Guid(segments[5]);
                    session = application.SurveyManager.AcquireSession(_sessionId);
                }
                catch (Exception ex)
                {
                    Logger.Error(context.Request.RawUrl, ex);
                }
            }
            if (session == null)
            {
                session = application.SurveyManager.AcquireSessionByRecipientKey(recipient.RecipientKey, collector.CollectorId);
                if (session == null)
                {
                    session = application.SurveyManager.CreateSession(survey.SurveyId, RuntimeRequestType.Manual_Email, ResponseType.Manual, context.Request.UserAgent, collector.CollectorId, recipient.RecipientKey, GetIPAddress(context));
                }
                else
                {
                    session.IsRessurected = true;
                }
            }
            else
            {
                session.IsRessurected = true;
            }
            context.Items["runtimeSession"] = session;
            #endregion

            #region ενεργοποιούμε το recipient, (εάν δεν είναι ήδη ενεργοποιημένο):
            if (recipient.ActivationDate == null)
            {
                recipient.ActivationDate = Utility.UtcNow();
                context.Items["recipient"] = application.SurveyManager.UpdateRecipient(recipient);
            }
            #endregion


            //REMAP HANDLER!!!!
            application.Context.RemapHandler(new DefaultHandler());
        }



        private void OnPostResolveRequestCache(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpRequest request = application.Request;
            HttpContext context = application.Context;

            if (request.Url.AbsolutePath.StartsWith("/services/api/filebrowser/FetchFileEx", StringComparison.OrdinalIgnoreCase))
            {
                application.Context.RemapHandler(new ValisServer.Support.filebrowser.FetchFileEx());
                return;
            }
        }
    }
}