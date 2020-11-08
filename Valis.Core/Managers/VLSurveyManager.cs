using CsvHelper;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
using Valis.Core.Export;
using Valis.Core.ViewModel;

namespace Valis.Core
{
    public sealed class VLSurveyManager : VLManagerBase
    {
        static ILog Logger = LogManager.GetLogger(typeof(VLSurveyManager));

        /// <summary>
        /// 
        /// </summary>
        private const int RecipientKey_Length = 32;
        /// <summary>
        /// 
        /// </summary>
        private const int RecipientKey_WebLink_Length = 36;
        /// <summary>
        /// 
        /// </summary>
        private const int WebLink_Length = 24;


        #region support stuff
        private VLSurveyManager(IAccessToken accessToken) : base(accessToken) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static VLSurveyManager GetAnInstance(IAccessToken accessToken)
        {
            var instance = new VLSurveyManager(accessToken);
            return instance;
        }
        #endregion



        #region VLRuntimeSession
        /// <summary>
        /// Επιστρέφει όλα τα Runtime sessions του συστήματος
        /// </summary>
        /// <returns></returns>
        internal Collection<VLRuntimeSession> GetSessions()
        {
            return SurveysDal.GetRuntimeSessions(AccessTokenId);
        }
        /// <summary>
        /// Επιστρέφει μία αποθηκευμένη RuntimeSession στο σύστημα
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public VLRuntimeSession AcquireSession(Guid sessionId)
        {
            VLRuntimeSession _session = SurveysDal.GetRuntimeSessionById(AccessTokenId, sessionId);
            
            return _session;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipientKey"></param>
        /// <param name="collector"></param>
        /// <returns></returns>
        public VLRuntimeSession AcquireSessionByRecipientKey(string recipientKey, Int32 collector)
        {
            return SurveysDal.GetRuntimeSessionByRecipientKeyAndCollector(this.AccessTokenId, recipientKey, collector);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipientKey"></param>
        /// <param name="collector"></param>
        /// <returns></returns>
        public VLRuntimeSession AcquireSessionByRecipientWebKey(string recipientKey, Int32 collector)
        {
            return SurveysDal.GetRuntimeSessionByRecipientWebKeyAndCollector(this.AccessTokenId, recipientKey, collector);
        }
        /// <summary>
        /// Δημιουργεί ένα νέο RuntimeSession για το συγκεκριμένο survey
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="requestType"></param>
        /// <param name="responseType"></param>
        /// <param name="userAgent"></param>
        /// <param name="collector"></param>
        /// <param name="recipientKey"></param>
        /// <param name="recipientIP"></param>
        /// <returns></returns>
        public VLRuntimeSession CreateSession(Int32 survey, RuntimeRequestType requestType, ResponseType responseType = ResponseType.Default, string userAgent = null, Int32? collector = null, string recipientKey = null, string recipientIP = null)
        {
            VLRuntimeSession _session = new VLRuntimeSession();
            _session.Survey = survey;
            _session.RequestType = requestType;
            _session.ResponseType = responseType;
            _session.Collector = collector;
            _session.RecipientKey = recipientKey;
            _session.RecipientIP = recipientIP;

            return SurveysDal.CreateRuntimeSession(AccessTokenId, _session, userAgent);
        }
        /// <summary>
        /// Αποθηκεύει το συγκεκριμένο Runtime session στο σύστημα μας.
        /// </summary>
        /// <param name="session"></param>
        internal void ReleaseSession(VLRuntimeSession session)
        {
            if (session == null) throw new ArgumentNullException("session");

            session.LastDt = Utility.UtcNow();

            SurveysDal.UpdateRuntimeSession(AccessTokenId, session);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        public void DeleteSession(VLRuntimeSession session)
        {
            if (session == null) throw new ArgumentNullException("session");

            DeleteSession(session.SessionId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        public void DeleteSession(Guid sessionId)
        {
            SurveysDal.DeleteRuntimeSession(AccessTokenId, sessionId);
        }
        /// <summary>
        /// 
        /// </summary>
        internal void DeleteAllSessions()
        {
            SurveysDal.DeleteAllRuntimeSessions(AccessTokenId);
        }
        /// <summary>
        /// Χρεώνει
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="collectorId"></param>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        internal VLRuntimeSession ChargePaymentForClick(Guid sessionId, Int32 collectorId, Int32 surveyId)
        {
            return SurveysDal.ChargePaymentForClickImpl(this.AccessTokenId, sessionId, collectorId, surveyId, Utility.UtcNow());
        }
        #endregion

        #region VLSurvey
        /// <summary>
        /// Επιστρέφει όλα τα surveys, που είναι ορατά για τον χρήστη που κάνει την κλήση.
        /// <para>Εάν ο καλούντας είναι ένας SystemUser, τότε βλέπει όλα τα surveys του συστήματος.</para>
        /// <para>Εάν ο καλούντας είναι ένας clientUser, τότε βλέπει μόνο τα surveys του πελάτη στον οποίο ανήκει.</para>
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public Collection<VLSurvey> GetSurveys(string whereClause = null, string orderByClause = "order by SurveyId", short textsLanguage = -1)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                //PASS THROUGH
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumerateSurveys);
            }
            #endregion

            return SurveysDal.GetSurveys(AccessTokenId, whereClause, orderByClause, textsLanguage);
        }
        /// <summary>
        /// Επιστρέφει όλα τα surveys ανα σελίδα, που είναι ορατά για τον χρήστη που κάνει την κλήση.
        /// <para>Εάν ο καλούντας είναι ένας SystemUser, τότε βλέπει όλα τα surveys του συστήματος.</para>
        /// <para>Εάν ο καλούντας είναι ένας clientUser, τότε βλέπει μόνο τα surveys του πελάτη στον οποίο ανήκει.</para>
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public Collection<VLSurvey> GetSurveys(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = "order by SurveyId", short textsLanguage = -1)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                //PASS THROUGH
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumerateSurveys);
            }
            #endregion

            return SurveysDal.GetSurveys(AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause, textsLanguage);
        }
        /// <summary>
        /// Επιστρέφει τον συνολικό αριθμό των surveys, που είναι ορατά για τον χρήστη που κάνει την κλήση.
        /// <para>Εάν ο καλούντας είναι ένας SystemUser, τότε βλέπει όλα τα surveys του συστήματος.</para>
        /// <para>Εάν ο καλούντας είναι ένας clientUser, τότε βλέπει μόνο τα surveys του πελάτη στον οποίο ανήκει.</para>
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public int GetSurveysCount(string whereClause = null, short textsLanguage = -1)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                //PASS THROUGH
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumerateSurveys);
            }
            #endregion

            return SurveysDal.GetSurveysCount(AccessTokenId, whereClause, textsLanguage);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        internal Collection<VLSurvey> GetSurveysForClient(Int32 clientId, string whereClause = null, string orderByClause = "order by SurveyId", short textsLanguage = -1)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion

            return SurveysDal.GetSurveysForClient(AccessTokenId, clientId, whereClause, orderByClause, textsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        internal Collection<VLSurvey> GetSurveysForClient(Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = "order by SurveyId", short textsLanguage = -1)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion

            return SurveysDal.GetSurveysForClient(AccessTokenId, clientId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause, textsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        internal int GetSurveysCountForClient(Int32 clientId, string whereClause = null, short textsLanguage = -1)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion

            return SurveysDal.GetSurveysCountForClient(AccessTokenId, clientId, whereClause, textsLanguage);
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLSurvey GetSurveyById(Int32 surveyId, Int16 textsLanguage = -1)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetSurveyById(AccessTokenId, surveyId, textsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicId"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLSurvey GetSurveyByPublicId(string publicId, Int16 textsLanguage = -1)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetSurveyByPublicId(AccessTokenId, publicId, textsLanguage);
        }
        /// <summary>
        /// Επιστρέφει όλα τα μεταφρασμένα variants του συγκεκριμένου survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public Collection<VLSurvey> GetSurveyVariantsById(Int32 surveyId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetSurveyVariantsById(AccessTokenId, surveyId);
        }
        /// <summary>
        /// Δημιουργεί στο σύστημα ένα νέο Survey κάτω απο τον συγκεκριμένο πελάτη (clientId).
        /// <para>Tο σύστημα δημιουργεί ταυτόχρονα και το πρώτο view για αυτό το ερωτηματολόγιο (default view)</para>
        /// <para>Επίσης το σύστημα δημιουργεί και την πρώτη σελίδα για αυτό το ερωτηματολόγιο</para>
        /// <para>Εάν δεν οριστεί η γλώσσα δημιουργίας, τότε το σύστημα θα δημιουργήσει το survey στην default γλώσσα του χρήστη που εκτελεί την μέθοδο.</para>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="title"></param>
        /// <param name="showTitle"></param>
        /// <param name="welcomeHtml"></param>
        /// <param name="folder"></param>
        /// <param name="textsLanguage">Η γλώσσα δημιουργίας του survey.</param>
        /// <returns></returns>
        public VLSurvey CreateSurvey(VLClient client, string title, string showTitle = null, string welcomeHtml = null, short? folder = null, short textsLanguage = -2)
        {
            if (client == null) throw new ArgumentNullException("client");

            return CreateSurvey(client.ClientId, title, showTitle, welcomeHtml, folder, textsLanguage);
        }
        /// <summary>
        /// Δημιουργεί στο σύστημα ένα νέο Survey κάτω απο τον συγκεκριμένο πελάτη (clientId).
        /// <para>Tο σύστημα δημιουργεί ταυτόχρονα και το πρώτο view για αυτό το ερωτηματολόγιο (default view)</para>
        /// <para>Επίσης το σύστημα δημιουργεί και την πρώτη σελίδα για αυτό το ερωτηματολόγιο</para>
        /// <para>Εάν δεν οριστεί η γλώσσα δημιουργίας, τότε το σύστημα θα δημιουργήσει το survey στην default γλώσσα του χρήστη που εκτελεί την μέθοδο.</para>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="title"></param>
        /// <param name="showTitle"></param>
        /// <param name="welcomeText"></param>
        /// <param name="folder"></param>
        /// <param name="textsLanguage">Η γλώσσα στην οποία θα δημιουργηθει το survey. Εάν δεν οριστεί θα χρησιμοποιηθεί η DefaultLanguage της συνεδρίας.</param>
        /// <returns></returns>
        public VLSurvey CreateSurvey(Int32 clientId, string title, string showTitle = null, string welcomeHtml = null, short? folder = null, short textsLanguage = -2)
        {
            VLSurvey survey = new VLSurvey();
            survey.Client = clientId;
            survey.Title = title;
            survey.ShowTitle = showTitle;
            survey.WelcomeHtml = welcomeHtml;
            survey.Folder = folder;

            return CreateSurvey(survey, textsLanguage);
        }
        /// <summary>
        /// Δημιουργεί στο σύστημα ένα νέο Survey.
        /// <para>Tο σύστημα δημιουργεί ταυτόχρονα και το πρώτο view για αυτό το ερωτηματολόγιο (default view)</para>
        /// <para>Επίσης το σύστημα δημιουργεί και την πρώτη σελίδα για αυτό το ερωτηματολόγιο</para>
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="textsLanguage">Η γλώσσα στην οποία θα δημιουργηθει το survey. Εάν δεν οριστεί θα χρησιμοποιηθεί η DefaultLanguage της συνεδρίας.</param>
        /// <returns></returns>
        public VLSurvey CreateSurvey(VLSurvey survey, short textsLanguage = -2)
        {
            if (survey == null) throw new ArgumentNullException("survey");
            survey.ValidateInstance();

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientCreateSurveys);
            }
            #endregion


            /*
             * Ελέγχουμε εάν υπάρχει ο συγκεκριμένος client:
             */
            var client = SystemDal.GetClientById(AccessTokenId, survey.Client);
            if (client == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Client", survey.Client));
            }

            /*
             * Ελέγχουμε εάν υπάρχει ήδη στην βάση αυτό το publicId:
             */
            var existingItem = SurveysDal.GetSurveyByPublicId(AccessTokenId, survey.PublicId, BuiltinLanguages.PrimaryLanguage.LanguageId);
            if (existingItem != null)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "PublicId", survey.PublicId));
            }

            /*
             *  Εάν δεν έχει οριστεί textsLanguage, θα χρησιμοποιήσουμε την DefaultLanguage του τρέχοντος χρήστη
             */
            if (textsLanguage == BuiltinLanguages.DefaultLanguage.LanguageId)
            {
                textsLanguage = this.DefaultLanguage;
                survey.PrimaryLanguage = this.DefaultLanguage;
            }
            else
            {
                //Υπάρχει αυτή η γλώσσα?
                BuiltinLanguages.GetLanguageById(textsLanguage, true);
                survey.PrimaryLanguage = textsLanguage;
            }

            /*
             * Δημιουργούμε το SupportedLanguagesIds
             */
            survey.SupportedLanguagesIds = string.Format("{0},", textsLanguage);



            /*
             * Δημιουργούμε το survey:
             */
            var newsurvey = SurveysDal.CreateSurvey(AccessTokenId, survey, textsLanguage);

            /*
             * Τώρα δημιουργούμε μία πρώτη σελίδα για αυτό το survey:
             */
            var firstPage = new VLSurveyPage();
            firstPage.Survey = newsurvey.SurveyId;
            firstPage.ShowTitle = null;
            firstPage.Description = null;
            firstPage = SurveysDal.CreateSurveyPage(this.AccessTokenId, firstPage, textsLanguage, InsertPosition.Last, null);

            return SurveysDal.GetSurveyById(AccessTokenId, newsurvey.SurveyId, newsurvey.TextsLanguage);
        }

        /// <summary>
        /// Δημιουργεί ένα καινούργιο survey στο σύστημα, αντιγράφοντας την δομή ενός προυπάρχοντος survey
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="newTitle"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public VLSurvey CopySurvey(VLSurvey survey, string newTitle, Int16 languageId = /*BuiltinLanguages.AllLanguages*/ -4)
        {
            if (survey == null) throw new ArgumentNullException("survey");
            return CopySurvey(survey.SurveyId, newTitle, languageId);
        }

        /// <summary>
        /// Δημιουργεί ένα καινούργιο survey στο σύστημα, αντιγράφοντας την δομή ενός προυπάρχοντος survey.
        /// <para>Το νέο ερωτηματολόγιο μπορεί να δημιουργηθεί αντιγράφωντας μόνο την μία γλώσσα ή και όλες του προυπάρχοντος survey</para>
        /// </summary>
        /// <param name="sourceSurveyId"></param>
        /// <param name="newTitle"></param>
        /// <param name="cpLanguageId"></param>
        /// <returns></returns>
        public VLSurvey CopySurvey(Int32 sourceSurveyId, string newTitle, Int16 cpLanguageId = /*BuiltinLanguages.AllLanguages*/ -4)
        {

            /*Διαβάζουμε το source survey, στην PrimaryLanguage:*/
            var sourceSurvey = SurveysDal.GetSurveyById(AccessTokenId, sourceSurveyId, BuiltinLanguages.PrimaryLanguage.LanguageId);
            if (sourceSurvey == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Survey", sourceSurveyId));
            }

            /*Ελέγχουμε το cpLanguageId, να είναι γνωστή γλώσσα και να υποστηρίζεται απο το survey*/
            if(cpLanguageId != BuiltinLanguages.AllLanguages.LanguageId)
            {
                var cplanguage = BuiltinLanguages.GetLanguageById(cpLanguageId, false);
                if (cplanguage == null)
                {
                    throw new ApplicationException(string.Format("Unknown language (id = {0})", cpLanguageId));
                }

                if(!sourceSurvey.SupportsLanguage(cpLanguageId))
                {
                    throw new VLException(string.Format("Survey does not support language {0}", cplanguage.EnglishName));
                }
            }

            /*Ξαναφορτώνουμε το sourceSurvey στην σωστή γλώσσα αντιγραφής:*/
            if(cpLanguageId != BuiltinLanguages.AllLanguages.LanguageId)
            {
                if (sourceSurvey.TextsLanguage != cpLanguageId)
                {
                    sourceSurvey = SurveysDal.GetSurveyById(AccessTokenId, sourceSurveyId, cpLanguageId);
                }
            }
            /*Φορτώνουμε τα υπόλοιπα στοιχεία:*/
            var sourcePages = SurveysDal.GetSurveyPages(AccessTokenId, sourceSurvey.SurveyId, string.Empty, sourceSurvey.TextsLanguage);
            var sourceQuestions = SurveysDal.GetQuestionsForSurvey(AccessTokenId, sourceSurvey.SurveyId, string.Empty, sourceSurvey.TextsLanguage);


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientCreateSurveys);
            }
            #endregion

            Collection<VLSurveyPage> _newPages = new Collection<VLSurveyPage>();
            Collection<VLSurveyQuestion> _newQuestions = new Collection<VLSurveyQuestion>();
            Func<Int16, VLSurveyPage> GetNewPage = delegate(Int16 prvPageId)
            {
                foreach (var item in _newPages)
                {
                    if (item.CustomId == prvPageId.ToString(CultureInfo.InvariantCulture))
                        return item;
                }
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Page", prvPageId));
            };


            /*
             * Δημιουργούμε ένα νέο survey με βάση το source.
             * Προσοχή: η PrimaryLanguage του νέου survey είναι η TextsLanguage του sourceSurvey
             */
            VLSurvey _newSurvey = new VLSurvey(sourceSurvey);
            _newSurvey.Title = string.IsNullOrWhiteSpace(newTitle) ? sourceSurvey.Title : newTitle;
            _newSurvey.IsFromCopy = true;
            _newSurvey.SourceSurvey = sourceSurveyId;
            _newSurvey = SurveysDal.CreateSurvey(AccessTokenId, _newSurvey, sourceSurvey.TextsLanguage);

            /*Αντιγράφουμε τα pages:*/
            foreach (VLSurveyPage item in sourcePages)
            {
                var page = new VLSurveyPage(item);
                page.Survey = _newSurvey.SurveyId;

                page = CreateSurveyPageImpl(page, sourceSurvey.TextsLanguage, InsertPosition.Last);
                page.CustomId = item.PageId.ToString(CultureInfo.InvariantCulture);
                _newPages.Add(page);
            }

            /*αντιγράφουμε τις ερωτήσεις*/
            foreach (VLSurveyQuestion q in sourceQuestions)
            {
                var question = new VLSurveyQuestion(q);
                question.Survey = _newSurvey.SurveyId;
                question.Page = GetNewPage(q.Page).PageId;

                question = CreateQuestionImpl(question, sourceSurvey.TextsLanguage, InsertPosition.Last);
                question.CustomId = q.QuestionId.ToString(CultureInfo.InvariantCulture);
                _newQuestions.Add(question);

                var options = SurveysDal.GetQuestionOptions(this.AccessTokenId, q.Survey, q.QuestionId, q.TextsLanguage);
                foreach (var op in options)
                {
                    VLQuestionOption option = new VLQuestionOption(op);
                    option.Survey = question.Survey;
                    option.Question = question.QuestionId;
                    option = SurveysDal.CreateQuestionOption(AccessTokenId, option, q.TextsLanguage);
                }

                var columns = SurveysDal.GetQuestionColumns(this.AccessTokenId, q.Survey, q.QuestionId, q.TextsLanguage);
                foreach (var col in columns)
                {
                    VLQuestionColumn column = new VLQuestionColumn(col);
                    column.Survey = question.Survey;
                    column.Question = question.QuestionId;
                    column = SurveysDal.CreateQuestionColumn(AccessTokenId, column, q.TextsLanguage);
                }
            }

            if (cpLanguageId == BuiltinLanguages.AllLanguages.LanguageId)
            {
                #region τώρα δημιουργούμε τις υπόλοιπες γλώσσες
                foreach (var lang in sourceSurvey.SupportedLanguages)
                {
                    if (lang.LanguageId == sourceSurvey.TextsLanguage)
                    {
                        continue;
                    }

                    /*Προσθέτουμε την νέα γλώσσα:*/
                    var target = AddSurveyLanguage(_newSurvey.SurveyId, _newSurvey.PrimaryLanguage, lang.LanguageId);

                    /*Κάνουμε update τα μεταφραζόμενα λεκτικά του survey*/
                    var source = SurveysDal.GetSurveyById(this.AccessTokenId, sourceSurvey.SurveyId, lang.LanguageId);
                    target.ShowTitle = source.ShowTitle;
                    target.HeaderHtml = source.HeaderHtml;
                    target.WelcomeHtml = source.WelcomeHtml;
                    target.GoodbyeHtml = source.GoodbyeHtml;
                    target.FooterHtml = source.FooterHtml;
                    target.StartButton = source.StartButton;
                    target.PreviousButton = source.PreviousButton;
                    target.NextButton = source.NextButton;
                    target.DoneButton = source.DoneButton;
                    target = SurveysDal.UpdateSurvey(this.AccessTokenId, target);

                    /*Κάνουμε update τα μεταφραζόμενα λεκτικά των σελίδων*/
                    foreach (var pg in _newPages)
                    {
                        var targetPage = SurveysDal.GetSurveyPageById(this.AccessTokenId, pg.Survey, pg.PageId, lang.LanguageId);
                        var sourcePage = SurveysDal.GetSurveyPageById(this.AccessTokenId, sourceSurvey.SurveyId, Int16.Parse(pg.CustomId), lang.LanguageId);
                        targetPage.ShowTitle = sourcePage.ShowTitle;
                        targetPage.Description = sourcePage.Description;
                        targetPage = SurveysDal.UpdateSurveyPage(this.AccessTokenId, targetPage);
                    }

                    /*Κάνουμε update τα μεταφραζόμενα λεκτικά των ερωτήσεων*/
                    foreach (var qt in _newQuestions)
                    {
                        var targetQuestion = SurveysDal.GetQuestionById(this.AccessTokenId, qt.Survey, qt.QuestionId, lang.LanguageId);
                        var sourceQuestion = SurveysDal.GetQuestionById(this.AccessTokenId, sourceSurvey.SurveyId, Int16.Parse(qt.CustomId), lang.LanguageId);
                        targetQuestion.QuestionText = sourceQuestion.QuestionText;
                        targetQuestion.Description = sourceQuestion.Description;
                        targetQuestion.HelpText = sourceQuestion.HelpText;
                        targetQuestion.FrontLabelText = sourceQuestion.FrontLabelText;
                        targetQuestion.AfterLabelText = sourceQuestion.AfterLabelText;
                        targetQuestion.InsideText = sourceQuestion.InsideText;
                        targetQuestion.RequiredMessage = sourceQuestion.RequiredMessage;
                        targetQuestion.ValidationMessage = sourceQuestion.ValidationMessage;
                        targetQuestion.OtherFieldLabel = sourceQuestion.OtherFieldLabel;

                        targetQuestion = SurveysDal.UpdateSurveyQuestion(this.AccessTokenId, targetQuestion);

                        var targetOptions = SurveysDal.GetQuestionOptions(this.AccessTokenId, qt.Survey, qt.QuestionId, lang.LanguageId);
                        var sourceOptions = SurveysDal.GetQuestionOptions(this.AccessTokenId, sourceQuestion.Survey, sourceQuestion.QuestionId, lang.LanguageId);
                        for (int i = 0; i < targetOptions.Count; i++)
                        {
                            targetOptions[i].OptionText = sourceOptions[i].OptionText;
                            SurveysDal.UpdateQuestionOption(this.AccessTokenId, targetOptions[i]);
                        }

                        var targetColumns = SurveysDal.GetQuestionColumns(this.AccessTokenId, qt.Survey, qt.QuestionId, lang.LanguageId);
                        var sourceColumns = SurveysDal.GetQuestionColumns(this.AccessTokenId, sourceQuestion.Survey, sourceQuestion.QuestionId, lang.LanguageId);
                        for (int i = 0; i < targetColumns.Count; i++)
                        {
                            targetColumns[i].ColumnText = sourceColumns[i].ColumnText;
                            SurveysDal.UpdateQuestionColumn(this.AccessTokenId, targetColumns[i]);
                        }
                    }
                }
                #endregion
            }

            return SurveysDal.GetSurveyById(this.AccessTokenId, _newSurvey.SurveyId, _newSurvey.TextsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public VLSurvey UpdateSurvey(VLSurvey survey)
        {
            if (survey == null) throw new ArgumentNullException("survey");
            survey.ValidateInstance();

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEditSurveys);
            }
            #endregion

            /*Το publicID κάθε survey πρέπει να είναι μοναδικό*/
            var existingItem = SurveysDal.GetSurveyByPublicId(AccessTokenId, survey.PublicId, survey.TextsLanguage);
            if (existingItem != null && existingItem.SurveyId != survey.SurveyId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "PublicId", survey.PublicId));
            }

            if (existingItem == null)
            {
                existingItem = SurveysDal.GetSurveyById(AccessTokenId, survey.SurveyId, survey.TextsLanguage);
            }
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Survey", survey.SurveyId));


            //Δεν επιτρέπεται να μεταβάλουμε στοιχεία ενός Builtin Survey:
            if (survey.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_update_builtin, "Survey"));
            }




            existingItem.Folder = survey.Folder;
            existingItem.PublicId = survey.PublicId;
            existingItem.Title = survey.Title;
            existingItem.Theme = survey.Theme;
            existingItem.Logo = survey.Logo;
            existingItem.AttributeFlags = survey.AttributeFlags;
            //PageSequence
            //QuestionSequence
            //TicketSequence
            existingItem.PrimaryLanguage = survey.PrimaryLanguage;

            existingItem.QuestionNumberingType = survey.QuestionNumberingType;
            existingItem.ProgressBarPosition = survey.ProgressBarPosition;
            existingItem.RequiredHighlightType = survey.RequiredHighlightType;
            //m_designVersion
            //m_recordedResponses
            existingItem.CustomId = survey.CustomId;
            //m_sourceSurvey
            //m_templateSurvey
            existingItem.OnCompletionMode = survey.OnCompletionMode;
            existingItem.OnDisqualificationMode = survey.OnDisqualificationMode;

            existingItem.AllResponsesXlsxExportName = survey.AllResponsesXlsxExportName;
            existingItem.AllResponsesXlsxExportPath = survey.AllResponsesXlsxExportPath;
            existingItem.AllResponsesXlsxExportCreationDt = survey.AllResponsesXlsxExportCreationDt;

            existingItem.ShowTitle = survey.ShowTitle;
            existingItem.HeaderHtml = survey.HeaderHtml;
            existingItem.WelcomeHtml = survey.WelcomeHtml;
            existingItem.GoodbyeHtml = survey.GoodbyeHtml;
            existingItem.FooterHtml = survey.FooterHtml;
            existingItem.DisqualificationHtml = survey.DisqualificationHtml;
            existingItem.DisqualificationUrl = survey.DisqualificationUrl;
            existingItem.OnCompletionUrl = survey.OnCompletionUrl;
            existingItem.StartButton = survey.StartButton;
            existingItem.PreviousButton = survey.PreviousButton;
            existingItem.NextButton = survey.NextButton;
            existingItem.DoneButton = survey.DoneButton;

            return SurveysDal.UpdateSurvey(AccessTokenId, existingItem);
        }

        /// <summary>
        /// Προσθέτει μία επιπλέον γλώσσα στο Survey.
        /// <para>Eάν η PrimaryLanguage ενός Survey είναι η Invariant, τότε αυτό δεν υποστηρίζει επιπλέον γλώσσες.</para>
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="sourceLanguage">H γλώσσα απο την οποία θα αντιγραφούν τα μεταφραζόμενα πεδία</param>
        /// <param name="targetLanguage">Η γλώσσα στην οποία δημιουργούμε τη νέα μετάφραση (variant) του survey</param>
        /// <returns></returns>
        public VLSurvey AddSurveyLanguage(Int32 surveyId, Int16 sourceLanguage, Int16 targetLanguage)
        {
            /*Διαβάζουμε το survey απο το σύστημα:*/
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, surveyId, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Survey", surveyId));

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEditSurveys);
            }
            #endregion

            //Δεν επιτρέπεται να μεταβάλουμε στοιχεία ενός Builtin Survey:
            if(survey.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_update_builtin, "Survey"));
            }

            /*Μπορεί να μεταφραστεί?*/
            if (survey.PrimaryLanguage == BuiltinLanguages.Invariant)
            {
                throw new VLException(string.Format("Survey '{0}', does not support languages!", survey.Title));
            }


            return SurveysDal.AddSurveyLanguage(AccessTokenId, surveyId, sourceLanguage, targetLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="language">Αφαιρούμε την μετάφραση σε αυτή την γλώσσα</param>
        public void RemoveSurveyLanguage(Int32 surveyId, Int16 language)
        {
            /*Διαβάζουμε το survey απο το σύστημα:*/
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, surveyId, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Survey", surveyId));

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEditSurveys);
            }
            #endregion
            
            //Δεν επιτρέπεται να μεταβάλουμε στοιχεία ενός Builtin Survey:
            if(survey.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_update_builtin, "Survey"));
            }

            /*Μπορεί να μεταφραστεί?*/
            if (survey.PrimaryLanguage == BuiltinLanguages.Invariant)
            {
                throw new VLException(string.Format("Survey '{0}', does not support languages!", survey.Title));
            }


            SurveysDal.RemoveSurveyLanguage(AccessTokenId, surveyId, language);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        public void DeleteSurvey(VLSurvey survey)
        {
            if (survey == null) throw new ArgumentNullException("survey");

            DeleteSurvey(survey.SurveyId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        public void DeleteSurvey(Int32 surveyId)
        {
            var svdSurvey = SurveysDal.GetSurveyById(AccessTokenId, surveyId, BuiltinLanguages.PrimaryLanguage);
            if (svdSurvey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", surveyId));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientDeleteSurveys);
            }
            #endregion


            /*Δεν μπορούμε να διαγράψουμε ένα builtin survey*/
            if (svdSurvey.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_delete_builtin, "survey"));
            }
            /*Δεν μπορούμε να διαγράψουμε survey το οποίο έχει ανοιχτούς collectors:*/
            var collectors = SurveysDal.GetCollectorsCount(AccessTokenId, surveyId, "where Status=1", BuiltinLanguages.PrimaryLanguage);
            if (collectors > 0)
            {
                throw new VLException("This survey has open collectors. You cannot delete the survey!");
            }
            /*Μήπως έχουμε απαντήσεις?*/


            try
            {
                /*
                 * διαγράφουμε τυχον αρχεία που διαθέτει το survey
                 */
                var filesToBeDeleted = FilesDal.DeleteAllFilesInSurvey(AccessTokenId, svdSurvey.Client, svdSurvey.SurveyId);
                foreach (var file in filesToBeDeleted)
                {
                    FilesDal.DeletePhysicalFile(file, FileInventoryPath);
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                /*
                 * διαγράφουμε το directory για αυτό το survey: 
                 */
                string fileDirPath = Path.Combine(FileInventoryPath, svdSurvey.Client.ToString(CultureInfo.InvariantCulture), svdSurvey.SurveyId.ToString(CultureInfo.InvariantCulture));
                if (Directory.Exists(fileDirPath))
                {
                    Directory.Delete(fileDirPath, true);
                }
            }



            SurveysDal.DeleteSurvey(AccessTokenId, surveyId);
        }

        /// <summary>
        /// Χρησιμοποιείτε μόνο για το UnitTesting
        /// </summary>
        /// <param name="surveyId"></param>
        internal void UnitTesting_DestroySurvey(Int32 surveyId)
        {
            var svdSurvey = SurveysDal.GetSurveyById(AccessTokenId, surveyId, BuiltinLanguages.PrimaryLanguage);
            if (svdSurvey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", surveyId));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientDeleteSurveys);
            }
            #endregion


            /*Δεν μπορούμε να διαγράψουμε ένα builtin survey*/
            if (svdSurvey.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_delete_builtin, "survey"));
            }



            try
            {
                /*
                 * διαγράφουμε τυχον αρχεία που διαθέτει το survey
                 */
                var filesToBeDeleted = FilesDal.DeleteAllFilesInSurvey(AccessTokenId, svdSurvey.Client, svdSurvey.SurveyId);
                foreach (var file in filesToBeDeleted)
                {
                    FilesDal.DeletePhysicalFile(file, FileInventoryPath);
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                /*
                 * διαγράφουμε το directory για αυτό το survey: 
                 */
                string fileDirPath = Path.Combine(FileInventoryPath, svdSurvey.Client.ToString(CultureInfo.InvariantCulture), svdSurvey.SurveyId.ToString(CultureInfo.InvariantCulture));
                if (Directory.Exists(fileDirPath))
                {
                    Directory.Delete(fileDirPath, true);
                }
            }



            SurveysDal.DestroySurveyImpl(AccessTokenId, surveyId, Utility.UtcNow());
        }

        #endregion

        #region VLFiles
        /// <summary>
        /// Επιστρέφει όλα τα αρχεία που ανήκουν στο συγκεκριμένο survey
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public Collection<VLFile> GetFiles(VLSurvey survey)
        {
            if (survey == null) throw new ArgumentNullException("survey");

            var whereclause = string.Format("where Survey={0}", survey.SurveyId);
            return FilesDal.GetFiles(AccessTokenId, survey.Client, whereclause, string.Empty);
        }
        /// <summary>
        /// Επιστρέφει όλα τα αρχεία που ανήκουν στο συγκεκριμένο survey
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public Collection<VLFile> GetFiles(Int32 clientId, Int32 surveyId)
        {
            var whereclause = string.Format("where Survey={0}", surveyId);
            return FilesDal.GetFiles(AccessTokenId, clientId, whereclause, string.Empty);
        }
        /// <summary>
        /// Επιστρέφει το αρχείο με το συγκεκριμένο Id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public VLFile GetFileById(Guid fileId)
        {
            return FilesDal.GetFileById(AccessTokenId, fileId);
        }

        public System.Byte[] GetFileContent(VLFile file)
        {
            if (file == null) throw new ArgumentNullException("file");

            //Αναλόγως που βρίσκεται το binary περιεχόμενο του αρχείου μας το διαβάζουμε μέσα στον πίνακα _binaryContent
            System.Byte[] _binaryContent = null;
            if (file.IsPhysicalFile)
            {
                #region read file content from filesystem
                string filePath = GetFilePath(file);

                FileInfo finfo = new FileInfo(filePath);
                if (!finfo.Exists) throw new VLException(string.Format("Physical file {0} with path: '{1}' and fileId={2} does not exists!", file.OriginalFileName, filePath, file.FileId.ToString()));

                using (FileStream fs = finfo.OpenRead())
                {
                    //το _binaryContent το θέτουμε στο μέγεθος του φυσικού αρχείου που θα διαβάσουμε
                    _binaryContent = new byte[finfo.Length];
                    //Διαβάζουμε μονομιάς το binary περιεχόμενο
                    int totalNumberOfBytes = fs.Read(_binaryContent, 0, _binaryContent.Length);

                    if (file.IsEncrypted == false)
                    {
                        //θα πρέπει το κατεγεγραμμένο μέγεθος του αρχείου στην βάση (file.FileBytes) να είναι ίδιο
                        //με αυτό που διαβάσαμε απο το φυσικό αρχείο
                        if (totalNumberOfBytes != file.Size)
                        {
                            throw new VLException(string.Format("Physical size {0} of file {1} with path: {2} and fileId={3} does not agree with the recorded size of {4} bytes!", finfo.Length, file.OriginalFileName, filePath, file.FileId.ToString(), file.Size));
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region read file content from database
                _binaryContent = FilesDal.GetFileStream(AccessTokenId, file.FileId);
                #endregion
            }


            if (file.IsEncrypted)
            {
                #region decrypt binary contents
                #endregion
            }

            return _binaryContent;
        }

        public System.Byte[] GetThumbnail(VLFile managedFile, int thumbWidth = 104, int thumbHeight = 80)
        {
            if (managedFile.IsImage == false)
                return null;

            System.Byte[] FileContent = GetFileContent(managedFile);
            MemoryStream stream = new MemoryStream(FileContent);

            Image img = Image.FromStream(stream);
            ImageFormat imgFormat = img.RawFormat;


            float MaxRatio = thumbWidth / (float)thumbHeight;
            float ImgRatio = img.Width / (float)img.Height;

            #region thumbnail creation
            if (ImgRatio > 1)
            {
                //Το πλάτος της εικόνας είναι μεγαλύτερο απο το ύψος της
                if (img.Width > thumbWidth)
                {
                    img = img.GetThumbnailImage(thumbWidth, (int)Math.Round(thumbWidth / ImgRatio, 0), null, System.IntPtr.Zero);
                }
                else
                    if (img.Height > thumbHeight)
                    {
                        img = img.GetThumbnailImage((int)Math.Round(thumbWidth * ImgRatio, 0), thumbHeight, null, System.IntPtr.Zero);
                    }
                    else
                    {
                        img = img.GetThumbnailImage(thumbWidth, thumbHeight, null, System.IntPtr.Zero);
                    }
            }
            else
            {
                //Το ύψος της εικόνας είναι μεγαλύτερο απο το πλάτος της
                if (img.Height > thumbHeight)
                {
                    img = img.GetThumbnailImage((int)Math.Round(thumbWidth * ImgRatio, 0), thumbHeight, null, System.IntPtr.Zero);
                }
                else
                    if (img.Width > thumbWidth)
                    {
                        img = img.GetThumbnailImage(thumbWidth, (int)Math.Round(thumbWidth / ImgRatio, 0), null, System.IntPtr.Zero);
                    }
                    else
                    {
                        img = img.GetThumbnailImage(thumbWidth, thumbHeight, null, System.IntPtr.Zero);
                    }
            }
            #endregion

            using (MemoryStream _tstream = new MemoryStream())
            {
                img.Save(_tstream, imgFormat);
                img.Dispose();
                img = null;
                return _tstream.GetBuffer();
            }
        }

        /// <summary>
        /// Καταχωρεί ενα αρχείο για το συγκκριμένο survey
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public VLFile AssignFile(VLSurvey survey, Stream stream, string fileName)
        {
            if (survey == null) throw new ArgumentNullException("survey");
            if (stream == null) throw new ArgumentNullException("stream");

            using (BinaryReader reader = new BinaryReader(stream))
            {
                return AssignFile(survey.SurveyId, reader.ReadBytes((Int32)stream.Length), fileName);
            }
        }
        /// <summary>
        /// Καταχωρεί ενα αρχείο για το συγκκριμένο survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="buffer"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public VLFile AssignFile(Int32 surveyId, byte[] buffer, string fileName)
        {
            var survey = SurveysDal.GetSurveyById(AccessTokenId, surveyId, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", surveyId));


            var file = AssignFileImpl(survey, buffer, fileName);
            return file;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="buffer"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private VLFile AssignFileImpl(VLSurvey survey, byte[] buffer, string fileName)
        {
            #region input validation
            if (survey == null) throw new ArgumentNullException("survey");
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (buffer.Length == 0) throw new ArgumentException("buffer cannot be empty!");
            Utility.CheckParameter(ref fileName, true, true, true, 512, "fileName");
            #endregion

            try
            {

                VLFile savedFile = FilesDal.SaveFile(this.AccessTokenId, survey, buffer, fileName, this.FileInventoryPath);


                return savedFile;
            }
            catch (Exception ex)
            {
                throw new VLException("AssignFileImpl failed!", ex);
            }
        }

        /// <summary>
        /// Διαγράφει το αρχείο απο το σύστημα.
        /// <para>εάν το αρχείο χρησιμοποιείται, τότε δεν διαγράφεται και το σύστημα ρίχνει ένα exception</para>
        /// </summary>
        /// <param name="fileId"></param>
        public void Removefile(Guid fileId)
        {
            VLFile file = FilesDal.GetFileById(AccessTokenId, fileId);
            if (file == null)
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "File", fileId));

            var survey = SurveysDal.GetSurveyById(AccessTokenId, file.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null)
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", file.Survey));

            /*Επιτρέπεται να σβήσουμε το αρχείο?*/

            try
            {
                FilesDal.DeletePhysicalFile(file, this.FileInventoryPath);
            }
            finally
            {
                FilesDal.DeleteFile(AccessTokenId, file.FileId);
            }
        }
        /// <summary>
        /// Διαγράφει όλα τα αρχεία του συγκεκριμένου survey
        /// </summary>
        /// <param name="survey"></param>
        public void RemoveAllFiles(VLSurvey survey)
        {
            if (survey == null) throw new ArgumentNullException("survey");
            RemoveAllFiles(survey.Client, survey.SurveyId);
        }
        /// <summary>
        /// Διαγράφει όλα τα αρχεία του συγκεκριμένου survey
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="surveyId"></param>
        public void RemoveAllFiles(Int32 clientId, Int32 surveyId)
        {
            var filesToBeDeleted = FilesDal.DeleteAllFilesInSurvey(AccessTokenId, clientId, surveyId);
            foreach (var file in filesToBeDeleted)
            {
                FilesDal.DeletePhysicalFile(file, this.FileInventoryPath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public System.Byte[] GetFileStream(Guid fileId)
        {
            VLFile file = FilesDal.GetFileById(AccessTokenId, fileId);
            return GetFileStream(file);
        }
        public System.Byte[] GetFileStream(VLFile file)
        {
            if (file == null) throw new ArgumentNullException("file");

            //Αναλόγως που βρίσκεται το binary περιεχόμενο του αρχείου μας, το διαβάζουμε μέσα στον πίνακα _binaryContent:
            System.Byte[] _binaryContent = null;
            if (file.IsPhysicalFile)
            {
                #region read file content from filesystem
                string filePath = this.GetFilePath(file);

                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    using (FileStream fs = fileInfo.OpenRead())
                    {
                        //το _binaryContent το θέτουμε στο μέγεθος του φυσικού αρχείου που θα διαβάσουμε
                        _binaryContent = new byte[fileInfo.Length];
                        //Διαβάζουμε μονομιάς το binary περιεχόμενο
                        int totalNumberOfBytes = fs.Read(_binaryContent, 0, _binaryContent.Length);
                    }
                }
                else
                {
                    throw new VLException(string.Format("Physical file {0} with path: '{1}' and fileId={2} does not exists!", file.OriginalFileName, filePath, file.FileId));
                }
                #endregion
            }
            else
            {
                #region read file content from database
                _binaryContent = FilesDal.GetFileStream(AccessTokenId, file.FileId);
                #endregion
            }

            //θα πρέπει το κατεγεγραμμένο μέγεθος του αρχείου στην βάση (file.FileBytes) να είναι ίδιο
            //με αυτό που διαβάσαμε απο το φυσικό αρχείο
            if (_binaryContent.Length != file.Size)
            {
                throw new VLException(string.Format("The readed physical size of {0} file is {1} bytes which is different from the recorded value of {2} bytes!", file.OriginalFileName, _binaryContent.Length, file.Size));
            }


            return _binaryContent;
        }
        #endregion

        #region VLSurveyTheme
        public Collection<VLSurveyTheme> GetSurveyThemes()
        {
            return SurveysDal.GetSurveyThemes(AccessTokenId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="themeId"></param>
        /// <returns></returns>
        public VLSurveyTheme GetSurveyThemeById(Int32 themeId)
        {
            return SurveysDal.GetSurveyThemeById(AccessTokenId, themeId);
        }

        public VLSurveyTheme CreateSurveyTheme(string name, string rthtml, string rtcss, Int32? clientId = null)
        {
            var theme = new VLSurveyTheme();
            theme.ClientId = clientId;
            theme.Name = name;
            theme.RtHtml = rthtml;
            theme.RtCSS = rtcss;

            return CreateSurveyTheme(theme);
        }

        public VLSurveyTheme CreateSurveyTheme(VLSurveyTheme theme)
        {
            if (theme == null) throw new ArgumentNullException("theme");
            theme.ValidateInstance();


            #region SecurityLayer
            #endregion

            var existingTheme = SurveysDal.GetSurveyThemeByName(AccessTokenId, theme.Name, theme.ClientId);
            if (existingTheme != null)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", theme.Name));
            }


            return SurveysDal.CreateSurveyTheme(AccessTokenId, theme);
        }

        public VLSurveyTheme UpdateSurveyTheme(VLSurveyTheme theme)
        {
            if (theme == null) throw new ArgumentNullException("theme");
            theme.ValidateInstance();

            var existingTheme = SurveysDal.GetSurveyThemeByName(AccessTokenId, theme.Name, theme.ClientId);
            if(existingTheme != null && existingTheme.ThemeId != theme.ThemeId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", theme.Name));
            }
            if(existingTheme == null)
            {
                existingTheme = SurveysDal.GetSurveyThemeById(AccessTokenId, theme.ThemeId);
            }
            if(existingTheme == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "SurveyTheme", theme.ThemeId));
            }

            #region SecurityLayer
            #endregion


            if(existingTheme.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_update_builtin, "surveyTheme"));
            }



            existingTheme.Name = theme.Name;
            existingTheme.RtHtml = theme.RtHtml;
            existingTheme.DtHtml = theme.DtHtml;
            existingTheme.DtCSS = theme.DtCSS;
            existingTheme.AttributeFlags = theme.AttributeFlags;

            return SurveysDal.UpdateSurveyTheme(AccessTokenId, existingTheme);
        }
        public void DeleteSurveyTheme(VLSurveyTheme theme)
        {
            if (theme == null) throw new ArgumentNullException("theme");
            DeleteSurveyTheme(theme.ThemeId);
        }
        public void DeleteSurveyTheme(Int32 themeId)
        {
            var svdTheme = SurveysDal.GetSurveyThemeById(AccessTokenId, themeId);
            if (svdTheme == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "SurveyTheme", themeId));

            #region SecurityLayer
            #endregion

            /*Μπορούμε να διαγράψουμε το συγκεκριμένο theme?*/
            if(svdTheme.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_delete_builtin, "SurveyTheme"));
            }

            SurveysDal.DeleteSurveyTheme(AccessTokenId, themeId);
        }
        #endregion

        #region VLSurveyPage
        /// <summary>
        /// Επιστρέφει τις σελίδες του survey με τα μεταφραζόμενα πεδία στην ίδια γλώσσα με αυτά του survey
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public Collection<VLSurveyPage> GetSurveyPages(VLSurvey survey)
        {
            if (survey == null) throw new ArgumentNullException("survey");

            return GetSurveyPages(survey.SurveyId, textsLanguage:survey.TextsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="whereClause"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public Collection<VLSurveyPage> GetSurveyPages(Int32 surveyId, string whereClause = null, short textsLanguage = -1)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetSurveyPages(AccessTokenId, surveyId, whereClause, textsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="pageId"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLSurveyPage GetSurveyPageById(Int32 survey, Int16 pageId, Int16 textsLanguage = -1)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetSurveyPageById(AccessTokenId, survey, pageId, textsLanguage);
        }

        public VLSurveyPage GetFirstSurveyPage(VLSurvey survey)
        {
            if (survey == null) throw new ArgumentNullException("survey");


            var pages = SurveysDal.GetSurveyPages(AccessTokenId, survey.SurveyId, "where DisplayOrder=1", survey.TextsLanguage);
            if (pages.Count == 1)
                return pages[0];
            if (pages.Count == 0)
                return null;

            throw new VLException("GetFirstSurveyPage!!");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="honorSkipLogic"></param>
        /// <returns></returns>
        public VLSurveyPage GetNextSurveyPage(VLSurveyPage currentPage, bool honorSkipLogic = false)
        {
            if (currentPage == null) throw new ArgumentNullException("currentPage");

            if (honorSkipLogic == false || (honorSkipLogic == true && currentPage.SkipTo == SkipToBehavior.None))
            {
                short displayOrder = currentPage.DisplayOrder;
                displayOrder++;

                var pages = SurveysDal.GetSurveyPages(AccessTokenId, currentPage.Survey, string.Format("where DisplayOrder={0}", displayOrder), currentPage.TextsLanguage);
                if (pages.Count == 1)
                    return pages[0];
                if (pages.Count == 0)
                    return null;
            }
            else
            {
                if (currentPage.SkipTo == SkipToBehavior.AnotherPage)
                {
                    if (currentPage.SkipToPage.HasValue == false)
                    {
                        throw new VLException(string.Format("Page with id {0}:{1}, has invalid SkipTo behavior!", currentPage.Survey, currentPage.PageId));
                    }
                    else
                    {
                        return SurveysDal.GetSurveyPageById(AccessTokenId, currentPage.Survey, currentPage.SkipToPage.Value, currentPage.TextsLanguage);
                    }
                }
                else if(currentPage.SkipTo == SkipToBehavior.DisqualificationPage)
                {
                    return BuiltinVirtualPages.DisqualificationPage;
                }
                else if(currentPage.SkipTo == SkipToBehavior.EndSurvey)
                {
                    return BuiltinVirtualPages.EndSurveyPage;
                }
                else if(currentPage.SkipTo == SkipToBehavior.GoodbyePage)
                {
                    return BuiltinVirtualPages.GoodbyPage;
                }
            }

            throw new VLException(string.Format("Page with id {0}:{1}, has invalid SkipTo behavior!", currentPage.Survey, currentPage.PageId));
        }
        public VLSurveyPage GetPreviousSurveyPage(VLSurveyPage currentPage)
        {
            if (currentPage == null) throw new ArgumentNullException("currentPage");

            short displayOrder = currentPage.DisplayOrder;
            if (displayOrder == 1)
                return null;
            displayOrder--;


            var pages = SurveysDal.GetSurveyPages(AccessTokenId, currentPage.Survey, string.Format("where DisplayOrder={0}", displayOrder), currentPage.TextsLanguage);
            if (pages.Count == 1)
                return pages[0];

            if (pages.Count == 0)
                return null;

            throw new VLException("GetPreviousSurveyPage!!");
        }

        /// <summary>
        /// Επιστρέφει ένα collection απο σελίδες στις οποίες μπορεί να πάει ο χρήστης (skip) μετά την τρέχουσα σελίδα 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="pageId"></param>
        /// <param name="addVirtualPages">Eάν θα προστεθούν και τα GoodbyePage, DisqualificationPage και η EndSurvey</param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLSurveyPage> GetCandidateSkipPagesForPage(Int32 survey, Int16 pageId, bool addVirtualPages = true, Int16 textsLanguage = -1)
        {
            var candidatePages = new Collection<VLSurveyPage>();

            //Θέλουμε όλες τις σελίδες απο το σύστημα για αυτό το survey
            var pages = SurveysDal.GetSurveyPages(AccessTokenId, survey, null, textsLanguage);
            if (pages.Count <= 2)
                return candidatePages;

            //Βρίσκουμε την δική μας σελίδα για την οποία ψάχνουμε skip pages:
            VLSurveyPage basePage = null;
            foreach(var pg in pages)
            {
                if(pg.PageId == pageId)
                {
                    basePage = pg;
                    break;
                }
            }
            if (basePage == null)
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id,"Page", pageId));

            //Θέλουμε μόνο αυτές που έχουν DisplayOrder μεγαλύτερο απο το δικό μας:
            foreach(var pg in pages)
            {
                if (pg.DisplayOrder > basePage.DisplayOrder)
                    candidatePages.Add(pg);
            }

            if(addVirtualPages)
            {
                /*
                 * Οι σελίδες EndSurvey, Thankyou και Disqualification έχουν καρφωτά PageId, διαφορετικά απο τις
                 * κανονικές για να μπορούμε να τις καταλαβαίνουμε και να τις ξεχωρίζουμε απο τις κανονικές σελίδες
                 * του survey.
                 */
                BuiltinVirtualPages.AddVirtualPagesToCollection(survey, candidatePages);
            }

            return candidatePages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="pageId"></param>
        /// <param name="skipToPage"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLSurveyPage SetPageSkipLogic(Int32 survey, Int16 pageId, Int16 skipToPage, Int16 textsLanguage = -1)
        {
            if(pageId == skipToPage)
            {
                throw new VLException("A survey page cannot jump to itself!");
            }


            #region SecurityLayer
            //PASS THROUGH
            #endregion


            var existingPage = SurveysDal.GetSurveyPageById(this.AccessTokenId, survey, pageId, textsLanguage);
            if (existingPage == null)
                throw new VLException(string.Format("There is no page with id = {0} for survey with id = {1}", pageId, survey));

            if(skipToPage > 0)
            {
                var skipPage = SurveysDal.GetSurveyPageById(this.AccessTokenId, survey, skipToPage, textsLanguage);
                if (skipPage == null)
                    throw new VLException(string.Format("There is no page with id = {0} for survey with id = {1}", skipToPage, survey));

                if(existingPage.DisplayOrder >= skipPage.DisplayOrder)
                {
                    throw new VLException("A page cannot jump to a previous page!");
                }

                existingPage.HasSkipLogic = true;
                existingPage.SkipTo = SkipToBehavior.AnotherPage;
                existingPage.SkipToPage = skipToPage;
                existingPage.SkipToWebUrl = null;
            }
            else
            {
                if(skipToPage == -1)
                {
                    existingPage.HasSkipLogic = true;
                    existingPage.SkipTo = SkipToBehavior.EndSurvey;
                    existingPage.SkipToPage = null;
                    existingPage.SkipToWebUrl = null;
                }
                else if(skipToPage == -2)
                {
                    existingPage.HasSkipLogic = true;
                    existingPage.SkipTo = SkipToBehavior.GoodbyePage;
                    existingPage.SkipToPage = null;
                    existingPage.SkipToWebUrl = null;
                }
                else if(skipToPage == -3)
                {
                    existingPage.HasSkipLogic = true;
                    existingPage.SkipTo = SkipToBehavior.DisqualificationPage;
                    existingPage.SkipToPage = null;
                    existingPage.SkipToWebUrl = null;
                }
                else
                {
                    throw new VLException(string.Format("Unknown skip behavior, {0}", skipToPage));
                }
            }

            return SurveysDal.UpdateSurveyPage(AccessTokenId, existingPage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="pageId"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLSurveyPage UnSetPageSkipLogic(Int32 survey, Int16 pageId, Int16 textsLanguage = -1)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion


            var existingPage = SurveysDal.GetSurveyPageById(this.AccessTokenId, survey, pageId, textsLanguage);
            if (existingPage == null)
                throw new VLException(string.Format("There is no page with id = {0} for survey with id = {1}", pageId, survey));

            if(existingPage.HasSkipLogic || existingPage.SkipTo != SkipToBehavior.None)
            {
                existingPage.HasSkipLogic = false;
                existingPage.SkipTo = SkipToBehavior.None;
                existingPage.SkipToPage = null;
                existingPage.SkipToWebUrl = null;

                existingPage = SurveysDal.UpdateSurveyPage(AccessTokenId, existingPage);
            }

            return existingPage;
        }

        /// <summary>
        /// Δημιουργεί μία νέα SurveyPage στο τέλος των ήδη υπάρχοντων σελίδων για αυτό το survey:
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="showTitle"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public VLSurveyPage CreateSurveyPage(VLSurvey survey, string showTitle, string description = null)
        {
            if (survey == null) throw new ArgumentNullException("survey");

            return CreateSurveyPage(survey.SurveyId, showTitle, description, survey.TextsLanguage);
        }
        /// <summary>
        /// Δημιουργεί μία νέα SurveyPage στο τέλος των ήδη υπάρχοντων σελίδων για αυτό το survey:
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="showTitle"></param>
        /// <param name="description"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLSurveyPage CreateSurveyPage(Int32 survey, string showTitle, string description = null, short textsLanguage = -1)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            var page = new VLSurveyPage();
            page.Survey = survey;
            page.ShowTitle = showTitle;
            page.Description = description;

            return CreateSurveyPageImpl(page, textsLanguage, InsertPosition.Last);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referingPage"></param>
        /// <param name="showTitle"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public VLSurveyPage CreateSurveyPageAfter(VLSurveyPage referingPage, string showTitle, string description = null)
        {
            if (referingPage == null) throw new ArgumentNullException("referingPage");

            return CreateSurveyPageAfter(referingPage.Survey, referingPage.PageId, showTitle, description, referingPage.TextsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="referingPageId"></param>
        /// <param name="showTitle"></param>
        /// <param name="description"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLSurveyPage CreateSurveyPageAfter(Int32 surveyId, Int16 referingPageId, string showTitle, string description = null, short textsLanguage = -1)
        {
            #region SecurityLayer

            #endregion

            var page = new VLSurveyPage();
            page.Survey = surveyId;
            page.ShowTitle = showTitle;
            page.Description = description;

            return CreateSurveyPageImpl(page, textsLanguage, InsertPosition.After, referingPageId);
        }

        public VLSurveyPage CreateSurveyPageBefore(VLSurveyPage referingPage, string showTitle, string description = null)
        {
            if (referingPage == null) throw new ArgumentNullException("referingPage");

            return CreateSurveyPageBefore(referingPage.Survey, referingPage.PageId, showTitle, description, referingPage.TextsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="referingPageId"></param>
        /// <param name="showTitle"></param>
        /// <param name="description"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLSurveyPage CreateSurveyPageBefore(Int32 surveyId, Int16 referingPageId, string showTitle, string description = null, short textsLanguage = -1)
        {
            #region SecurityLayer

            #endregion

            var page = new VLSurveyPage();
            page.Survey = surveyId;
            page.ShowTitle = showTitle;
            page.Description = description;

            return CreateSurveyPageImpl(page, textsLanguage, InsertPosition.Before, referingPageId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyPage"></param>
        /// <param name="textsLanguage"></param>
        /// <param name="position"></param>
        /// <param name="referingPageId"></param>
        /// <returns></returns>
        VLSurveyPage CreateSurveyPageImpl(VLSurveyPage surveyPage, short textsLanguage, InsertPosition position = InsertPosition.Last, Int16? referingPageId = null)
        {
            if (surveyPage == null) throw new ArgumentNullException("surveyPage");


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientEditSurveys);
            }
            #endregion


            try
            {
                return SurveysDal.CreateSurveyPage(AccessTokenId, surveyPage, textsLanguage, position, referingPageId);              
            }
            catch(Exception ex)
            {
                var message = string.Format("CreateSurveyPageImpl(), AccessTokenId={0}, surveyId={1}", this.AccessTokenId, surveyPage.Survey);
                Logger.Error(message, ex);
                throw;
            }
        }



        public VLSurveyPage UpdateSurveyPage(VLSurveyPage surveyPage)
        {
            if (surveyPage == null) throw new ArgumentNullException("surveyPage");
            surveyPage.ValidateInstance();

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.UpdateSurveyPage(AccessTokenId, surveyPage);
        }


        public VLSurveyPageDeleteOptions GetDeleteSurveyPageOptions(Int32 surveyId, Int16 pageId)
        {
            var _options = new VLSurveyPageDeleteOptions();


            #region SecurityLayer
            _options.CanBeDeleted = true;
            _options.HasUserDeletePermission = true;
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                if (ValidatePermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService) == false)
                {
                    _options.CanBeDeleted = false;
                    _options.HasUserDeletePermission = false;
                }
            }
            else
            {
                if (ValidatePermissions(VLPermissions.ClientEditSurveys) == false)
                {
                    _options.CanBeDeleted = false;
                    _options.HasUserDeletePermission = false;
                }
            }
            #endregion


            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, surveyId, BuiltinLanguages.PrimaryLanguage);
            _options.SurveyId = survey.SurveyId;
            if (survey.IsBuiltIn)
            {
                _options.CanBeDeleted = false;
                _options.IsBuiltin = true;
            }
            _options.SurveyHasResponses = survey.HasResponses;


            var totalQuestions = SurveysDal.GetQuestionsForSurveyCount(this.AccessTokenId, surveyId, pageId);
            if (totalQuestions > 0)
                _options.HasQuestions = true;


            var page = SurveysDal.GetSurveyPageById(this.AccessTokenId, surveyId, pageId, BuiltinLanguages.PrimaryLanguage);
            _options.PageId = page.PageId;
            _options.ShowTitle = page.ShowTitle;

            var nextPage = GetNextSurveyPage(page);
            if (nextPage != null)
            {
                _options.HasNextPage = true;
                _options.NextPageId = nextPage.PageId;
            }

            var previousPage = GetPreviousSurveyPage(page);
            if (previousPage != null)
            {
                _options.HasPreviousPage = true;
                _options.PreviousPageId = previousPage.PageId;
            }


            return _options;
        }
        /// <summary>
        /// Διαγράφει την συγκεκριμένη σελίδα απο ένα Survey
        /// <para>Οι ερωτήσεις που περιέχει η σελίδα μπορεί να διαγραφούν μαζί με αυτή ή να μεταφερθούν σε κάποια γειτονική σελίδα</para>
        /// </summary>
        /// <param name="surveyPage"></param>
        /// <param name="questionsDeleteBehavior"></param>
        public void DeleteSurveyPage(VLSurveyPage surveyPage, DeleteQuestionsBehavior questionsDeleteBehavior = DeleteQuestionsBehavior.DeleteAll)
        {
            if (surveyPage == null) throw new ArgumentNullException("surveyPage");

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientEditSurveys);
            }
            #endregion

            DeleteSurveyPage(surveyPage.Survey, surveyPage.PageId, questionsDeleteBehavior);
        }
        /// <summary>
        /// Διαγράφει την συγκεκριμένη σελίδα απο ένα Survey
        /// <para>Οι ερωτήσεις που περιέχει η σελίδα μπορεί να διαγραφούν μαζί με αυτή ή να μεταφερθούν σε κάποια γειτονική σελίδα</para>
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="pageId"></param>
        /// <param name="questionsDeleteBehavior"></param>
        public void DeleteSurveyPage(Int32 surveyId, Int16 pageId, DeleteQuestionsBehavior questionsDeleteBehavior = DeleteQuestionsBehavior.DeleteAll)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientEditSurveys);
            }
            #endregion

            try
            {
                DeleteSurveyPageImpl(surveyId, pageId, questionsDeleteBehavior);
            }
            catch (VLException ex)
            {
                var message = string.Format("An exception occured while calling DeleteSurveyPage(), AccessTokenId={0}, surveyId={1}, pageId={2}, questionsDeleteBehavior={3}", this.AccessTokenId, surveyId, pageId, questionsDeleteBehavior);
                Logger.Error(string.Format("RefId={0}, {1}", ex.ReferenceId, message), ex);
                throw new VLException(ex.Message, ex.ReferenceId);
            }
            catch (Exception ex)
            {
                var message = string.Format("An exception occured while calling DeleteSurveyPage(), AccessTokenId={0}, surveyId={1}, pageId={2}, questionsDeleteBehavior={3}", this.AccessTokenId, surveyId, pageId, questionsDeleteBehavior);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }
        }

        void DeleteSurveyPageImpl(Int32 surveyId, Int16 pageId, DeleteQuestionsBehavior questionsDeleteBehavior = DeleteQuestionsBehavior.DeleteAll)
        {
            /*Διαβάζουμε όλες τις σελίδες απο το σύστημα για αυτό το survey, διότι θα τις χρειαστούμε παρακάτω:*/
            var pages = SurveysDal.GetSurveyPages(this.AccessTokenId, surveyId, null, BuiltinLanguages.PrimaryLanguage);

            /*Τώρα θα εντοπίσουμε την σελίδα μας:*/
            VLSurveyPage existingPage = null;
            foreach (var pg in pages)
            {
                if (pg.PageId == pageId)
                {
                    existingPage = pg;
                    break;
                }
            }
            if (existingPage == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "SurveyPage", string.Format("({0}, {1})", surveyId, pageId)));
            

            /*Δεν επιτρέπουμε την διαγραφή της σελίδας, εάν έχει SKIP LOGIC:*/
            if (existingPage.SkipTo != SkipToBehavior.None)
            {
                throw new VLException(string.Format("Page '{0}', cannot be deleted because it contains PAGE LOGIC.", existingPage.ShowTitle, existingPage.PageId));
            }
            /*Δεν επιτρέπουμε την διαγραφή της σελίδας, εάν έχει τεθεί σαν SkipToPage σε κάποια άλλη σελίδα του ερωτηματολογίου:*/
            bool isEngaged = false;
            foreach (var pg in pages)
            {
                if (pg.PageId == existingPage.PageId)
                    continue;

                if (pg.SkipTo == SkipToBehavior.AnotherPage && pg.SkipToPage == existingPage.PageId)
                {
                    isEngaged = true;
                    break;
                }
            }
            if (isEngaged == true)
            {
                /*Συμμετέχει σε skip logic*/
                throw new VLException(string.Format("Page '{0}', cannot be deleted because is target of PAGE LOGIC.", existingPage.ShowTitle, existingPage.PageId));
            }
            /*Δεν επιτρέπουμε την διαγραφή της σελίδας, εάν έχει τεθεί σαν SkipToPage σε κάποιo option του ερωτηματολογίου:*/
            var options = SurveysDal.GetQuestionOptions(this.AccessTokenId, surveyId, BuiltinLanguages.PrimaryLanguage);
            foreach(var op in options)
            {
                if(op.SkipTo == SkipToBehavior.AnotherPage && op.SkipToPage == existingPage.PageId)
                {
                    isEngaged = true;
                    break;
                }
            }
            if (isEngaged == true)
            {
                /*Συμμετέχει σε skip logic*/
                throw new VLException(string.Format("Page '{0}', cannot be deleted because is target of QUESTION LOGIC.", existingPage.ShowTitle, existingPage.PageId));
            }


            /*εδώ μεταφέρουμε τις ερωτήσεις:*/
            if (questionsDeleteBehavior != DeleteQuestionsBehavior.DeleteAll)
            {
                var questions = SurveysDal.GetQuestionsForSurvey(AccessTokenId, surveyId, pageId, "order by DisplayOrder", BuiltinLanguages.PrimaryLanguage);
                if (questions.Count > 0)
                {
                    if (questionsDeleteBehavior == DeleteQuestionsBehavior.MoveAbove)
                    {
                        var targetPage = GetPreviousSurveyPage(existingPage);
                        if (targetPage == null)
                        {
                            throw new VLException("Page's questions cannot be transfered! there is no Previous page!");
                        }

                        foreach (var q in questions)
                        {
                            q.Page = targetPage.PageId;
                            SurveysDal.UpdateSurveyQuestion(this.AccessTokenId, q);
                        }
                    }
                    else if (questionsDeleteBehavior == DeleteQuestionsBehavior.MoveBellow)
                    {
                        var targetPage = GetNextSurveyPage(existingPage);
                        if (targetPage == null)
                        {
                            throw new VLException("Page's questions cannot be transfered! there is no Next page!");
                        }

                        foreach (var q in questions)
                        {
                            q.Page = targetPage.PageId;
                            SurveysDal.UpdateSurveyQuestion(this.AccessTokenId, q);
                        }
                    }
                }
            }


            SurveysDal.DeleteSurveyPage(AccessTokenId, surveyId, pageId);
        }
        
        #endregion

        #region VLSurveyQuestion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public Collection<VLSurveyQuestion> GetQuestionsForSurvey(VLSurvey survey)
        {
            if (survey == null) throw new ArgumentNullException("survey");
            return GetQuestionsForSurvey(survey.SurveyId, survey.TextsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public Collection<VLSurveyQuestion> GetQuestionsForSurvey(Int32 survey, short textsLanguage = -1)
        {
            return SurveysDal.GetQuestionsForSurvey(AccessTokenId, survey, "order by Page,DisplayOrder", textsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public Collection<VLSurveyQuestion> GetQuestionsForPage(VLSurveyPage page)
        {
            if (page == null) throw new ArgumentNullException("page");
            return GetQuestionsForPage(page.Survey, page.PageId, page.TextsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="page"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public Collection<VLSurveyQuestion> GetQuestionsForPage(Int32 survey, Int16 page, short textsLanguage = -1)
        {
            return SurveysDal.GetQuestionsForSurvey(AccessTokenId, survey, page, "order by DisplayOrder", textsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="masterQuestion"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLSurveyQuestion> GetChildQuestions(Int32 survey, Int16 masterQuestion, short textsLanguage = -1)
        {
            return SurveysDal.GetChildQuestions(AccessTokenId, survey, masterQuestion, textsLanguage);
        }

        /// <summary>
        /// Επιστρέφει ένα collection απο σελίδες στις οποίες μπορεί να πάει ο χρήστης (skip) μετά την τρέχουσα σελίδα
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="questionId"></param>
        /// <param name="addVirtualPages"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLSurveyPage> GetCandidateSkipPagesForQuestion(Int32 survey, Int16 questionId, bool addVirtualPages = true, short textsLanguage = -1)
        {
            var candidatePages = new Collection<VLSurveyPage>();

            //Θέλουμε να διαβάσουμε την ερωτησή μας απο το σύστημα:
            var question = SurveysDal.GetQuestionById(this.AccessTokenId, survey, questionId, textsLanguage);
            if(question == null)
                throw new VLException(string.Format("There is no Question with id = {0} for survey with id = {1}", questionId, survey));

            //Θέλουμε όλες τις σελίδες απο το σύστημα για αυτό το survey
            var pages = SurveysDal.GetSurveyPages(AccessTokenId, survey, null, textsLanguage);
            if (pages.Count <= 2)
                return candidatePages;

            //Βρίσκουμε την δική μας σελίδα πάνω στην οποία βρίσκεται η ερωτησή μας:
            VLSurveyPage basePage = null;
            foreach (var pg in pages)
            {
                if (pg.PageId == question.Page)
                {
                    basePage = pg;
                    break;
                }
            }
            if (basePage == null)
                throw new VLException(string.Format("There is no page with id = {0} for survey with id = {1}", question.Page, survey));

            //Θέλουμε μόνο αυτές που έχουν DisplayOrder μεγαλύτερο απο το δικό μας:
            foreach (var pg in pages)
            {
                if (pg.DisplayOrder > basePage.DisplayOrder)
                    candidatePages.Add(pg);
            }


            if (addVirtualPages)
            {
                /*
                 * Οι σελίδες EndSurvey, Thankyou και Disqualification έχουν καρφωτά PageId, διαφορετικά απο τις
                 * κανονικές για να μπορούμε να τις καταλαβαίνουμε και να τις ξεχωρίζουμε απο τις κανονικές σελίδες
                 * του survey.
                 */
                BuiltinVirtualPages.AddVirtualPagesToCollection(survey, candidatePages);
            }


            return candidatePages;
        }
        public VLSurveyQuestion SetQuestionSkipLogic(Int32 survey, Int16 questionId, Collection<VLQuestionOptionHelper> hoptions, Int16 textsLanguage = -1)
        {
            if (hoptions == null) throw new ArgumentNullException("hoptions");

            //Διαβάζουμε την ερώτησή μας απο το σύστημα:
            var question = SurveysDal.GetQuestionById(this.AccessTokenId, survey, questionId, textsLanguage);
            if (question == null) throw new VLException(string.Format("There is no Question with id = {0} for survey with id = {1}", questionId, survey));

            //Διαβάζουμε την σελίδα στην οποία βρίσκεται η ερώτησή μας:
            var questionPage = SurveysDal.GetSurveyPageById(this.AccessTokenId, survey, question.Page, textsLanguage);
            if (questionPage == null) throw new VLException(string.Format("There is no Page with id = {0} for survey with id = {1}", question.Page, survey));

            //Διαβάζουμε τα options, της ερώτησης, απο το σύστημά μας:
            var options = SurveysDal.GetQuestionOptions(this.AccessTokenId, survey, questionId, textsLanguage);
            if (options.Count != hoptions.Count) throw new VLException("Wrong number of Question-Options!");


            /*
             * Στοιχίζουμε τα Options με τα Helper-Options, 
             * και ελέγχουμε μήπως υπάρχει κάποιο που δεν στοιχήθηκε:
             */
            foreach(var hop in hoptions)
            {
                foreach(var op in options)
                {
                    if(hop.OptionId == op.OptionId)
                    {
                        op.ResetSkipping();
                        hop.OptionPtr = op;
                        break;
                    }
                }
                if (hop.OptionPtr == null)
                {
                    throw new VLException(string.Format("Option with id={0} for question with id = {1}, does not exist!", hop.OptionId, questionId));
                }
            }


            question.HasSkipLogic = false;
            //Τώρα λουπάρουμε για τελευταία φορά και κάνουμε update
            foreach(var hop in hoptions)
            {
                if (hop.skipToPage.HasValue)
                {
                    #region impementation
                    question.HasSkipLogic = true;

                    if(hop.skipToPage > 0)
                    {
                        var skipPage = SurveysDal.GetSurveyPageById(this.AccessTokenId, survey, hop.skipToPage.Value, textsLanguage);
                        if (skipPage == null) throw new VLException(string.Format("There is no page with id = {0} for survey with id = {1}", hop.skipToPage.Value, survey));

                        if (questionPage.DisplayOrder >= skipPage.DisplayOrder)
                        {
                            throw new VLException("A Question cannot jump to a previous page!");
                        }

                        hop.OptionPtr.SkipTo = SkipToBehavior.AnotherPage;
                        hop.OptionPtr.SkipToPage = hop.skipToPage.Value;

                        if (hop.SkipToQuestion.HasValue)
                        {
                            var skipQuestion = SurveysDal.GetQuestionById(this.AccessTokenId, survey, hop.SkipToQuestion.Value, textsLanguage);
                            if (skipQuestion == null) throw new VLException(string.Format("There is no Question with id = {0} for survey with id = {1}", hop.SkipToQuestion.Value, survey));

                            if(question.QuestionId == skipQuestion.QuestionId)
                            {
                                throw new VLException("A survey question cannot jump to itself!");
                            }
                            if (skipQuestion.Page != skipPage.PageId)
                            {
                                throw new VLException("The skipTo Question must belong to skipTo Page!");
                            }

                            hop.OptionPtr.SkipToQuestion = skipQuestion.QuestionId;
                        }
                    }
                    else
                    {
                        if (hop.skipToPage == -1)
                        {
                            hop.OptionPtr.SkipTo = SkipToBehavior.EndSurvey;
                        }
                        else if (hop.skipToPage == -2)
                        {
                            hop.OptionPtr.SkipTo = SkipToBehavior.GoodbyePage;
                        }
                        else if (hop.skipToPage == -3)
                        {
                            hop.OptionPtr.SkipTo = SkipToBehavior.DisqualificationPage;
                        }
                        else
                        {
                            throw new VLException(string.Format("Unknown skip behavior, {0}", hop.skipToPage));
                        }
                    }
                    #endregion
                }
            }


            //Αποθηκεύουμε:
            foreach (var op in options)
            {
                SurveysDal.UpdateQuestionOption(this.AccessTokenId, op);
            }

            return SurveysDal.UpdateSurveyQuestion(this.AccessTokenId, question);
        }
        public VLSurveyQuestion UnSetQuestionSkipLogic(Int32 survey, Int16 questionId, Int16 textsLanguage = -1)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLSurveyQuestion GetQuestionById(Int32 survey, Int16 question, short textsLanguage = -1)
        {
            return SurveysDal.GetQuestionById(AccessTokenId, survey, question, textsLanguage);
        }

        /// <summary>
        /// Προσθέτει μία νέα ερώτηση στο τέλος των ερωτήσεων της τρέχουσας σελίδας
        /// </summary>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <param name="questionText"></param>
        /// <returns></returns>
        public VLSurveyQuestion CreateQuestion(VLSurveyPage page, QuestionType type, string questionText)
        {
            if (page == null) throw new ArgumentNullException("page");
            return CreateQuestion(page.Survey, page.PageId, type, questionText, page.TextsLanguage);
        }
        /// <summary>
        /// Προσθέτει μία νέα ερώτηση στο τέλος των ερωτήσεων της τρέχουσας σελίδας
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <param name="questionText"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLSurveyQuestion CreateQuestion(Int32 survey, Int16 page, QuestionType type, string questionText, short textsLanguage = -1)
        {
            VLSurveyQuestion question = new VLSurveyQuestion();
            question.Survey = survey;
            question.Page = page;
            question.MasterQuestion = null;
            question.QuestionType = type;
            question.QuestionText = questionText;


            try
            {
                return CreateQuestionImpl(question, textsLanguage, InsertPosition.Last);
            }
            catch (VLException ex)
            {
                var message = string.Format("An exception occured while calling CreateQuestion(), AccessTokenId={0}, Survey={1}, Page={2}, QuestionType={3}, textsLanguage={4}", this.AccessTokenId, survey, page, type, textsLanguage);
                Logger.Error(string.Format("RefId={0}, {1}", ex.ReferenceId, message), ex);
                throw new VLException("An exception occured at CreateQuestion()", ex.ReferenceId);
            }
            catch (Exception ex)
            {
                var message = string.Format("An exception occured while calling CreateQuestion(), AccessTokenId={0}, Survey={1}, Page={2}, QuestionType={3}, textsLanguage={4}", this.AccessTokenId, survey, page, type, textsLanguage);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }
        }

        public VLSurveyQuestion CreateQuestionAfter(VLSurveyQuestion referingQuestion, QuestionType type, string questionText)
        {
            if (referingQuestion == null) throw new ArgumentNullException("referingQuestion");
            return CreateQuestionAfter(referingQuestion.Survey, referingQuestion.Page, referingQuestion.QuestionId, type, questionText, referingQuestion.TextsLanguage);
        }
        public VLSurveyQuestion CreateQuestionAfter(Int32 survey, Int16 page, Int16 referingQuestionId, QuestionType type, string questionText, short textsLanguage = -1)
        {
            VLSurveyQuestion question = new VLSurveyQuestion();
            question.Survey = survey;
            question.Page = page;
            question.MasterQuestion = null;
            question.QuestionType = type;
            question.QuestionText = questionText;


            try
            {
                return CreateQuestionImpl(question, textsLanguage, InsertPosition.After, referingQuestionId);
            }
            catch (VLException ex)
            {
                var message = string.Format("An exception occured while calling CreateQuestionAfter(), AccessTokenId={0}, Survey={1}, Page={2}, QuestionType={3}, textsLanguage={4}, referingQuestionId={5}", this.AccessTokenId, survey, page, type, textsLanguage, referingQuestionId);
                Logger.Error(string.Format("RefId={0}, {1}", ex.ReferenceId, message), ex);
                throw new VLException("An exception occured at CreateQuestionAfter()", ex.ReferenceId);
            }
            catch (Exception ex)
            {
                var message = string.Format("An exception occured while calling CreateQuestionAfter(), AccessTokenId={0}, Survey={1}, Page={2}, QuestionType={3}, textsLanguage={4}, referingQuestionId={5}", this.AccessTokenId, survey, page, type, textsLanguage, referingQuestionId);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }
        }

        public VLSurveyQuestion CreateQuestionBefore(VLSurveyQuestion referingQuestion, QuestionType type, string questionText)
        {
            if (referingQuestion == null) throw new ArgumentNullException("referingQuestion");
            return CreateQuestionBefore(referingQuestion.Survey, referingQuestion.Page, referingQuestion.QuestionId, type, questionText, referingQuestion.TextsLanguage);
        }
        public VLSurveyQuestion CreateQuestionBefore(Int32 survey, Int16 page, Int16 referingQuestionId, QuestionType type, string questionText, short textsLanguage = -1)
        {
            VLSurveyQuestion question = new VLSurveyQuestion();
            question.Survey = survey;
            question.Page = page;
            question.MasterQuestion = null;
            question.QuestionType = type;
            question.QuestionText = questionText;

            try
            {
                return CreateQuestionImpl(question, textsLanguage, InsertPosition.Before, referingQuestionId);
            }
            catch (VLException ex)
            {
                var message = string.Format("An exception occured while calling CreateQuestionBefore(), AccessTokenId={0}, Survey={1}, Page={2}, QuestionType={3}, textsLanguage={4}, referingQuestionId={5}", this.AccessTokenId, survey, page, type, textsLanguage, referingQuestionId);
                Logger.Error(string.Format("RefId={0}, {1}", ex.ReferenceId, message), ex);
                throw new VLException("An exception occured at CreateQuestionBefore()", ex.ReferenceId);
            }
            catch(Exception ex)
            {
                var message = string.Format("An exception occured while calling CreateQuestionBefore(), AccessTokenId={0}, Survey={1}, Page={2}, QuestionType={3}, textsLanguage={4}, referingQuestionId={5}", this.AccessTokenId, survey, page, type, textsLanguage, referingQuestionId);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        /// <param name="textsLanguage"></param>
        /// <param name="position"></param>
        /// <param name="referingQuestionId"></param>
        /// <returns></returns>
        VLSurveyQuestion CreateQuestionImpl(VLSurveyQuestion question, short textsLanguage, InsertPosition position = InsertPosition.Last, Int16? referingQuestionId = null)
        {
            #region input parameters validation
            if (question == null) throw new ArgumentNullException("question");
            question.ValidateInstance();
            #endregion


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientEditSurveys);
            }
            #endregion

            /*Ελέγχουμε την εγκυρότητα της ερώτησης που πάμε να δημιουργήσουμε:*/
            _Validatequestion(question);

            try
            {
                return SurveysDal.CreateSurveyQuestion(AccessTokenId, question, textsLanguage, position, referingQuestionId);
            }
            catch(Exception ex)
            {
                var message = string.Format("CreateQuestionImpl(), AccessTokenId={0}, Survey={1}, Page={2}, QuestionType={3}, textsLanguage={4}, position = {5}, referingQuestionId={6}", this.AccessTokenId, question.Survey, question.Page, question.QuestionType, textsLanguage, position, referingQuestionId);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }
        }



        /// <summary>
        /// Προσθέτει την συγκεκριμένη 'library-question' στο τέλος των ερωτήσεων της τρέχουσας σελίδας
        /// </summary>
        /// <param name="page"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public VLSurveyQuestion AddLibraryQuestion(VLSurveyPage page, VLLibraryQuestion libraryQuestion)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (libraryQuestion == null) throw new ArgumentNullException("libraryQuestion");

            return AddLibraryQuestion(page.Survey, page.PageId, libraryQuestion.QuestionId);
        }
        /// <summary>
        /// Προσθέτει την συγκεκριμένη 'library-question' στο τέλος των ερωτήσεων της τρέχουσας σελίδας
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="page"></param>
        /// <param name="libraryQuestionId"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLSurveyQuestion AddLibraryQuestion(Int32 surveyId, Int16 pageId, Int32 libraryQuestionId, short textsLanguage = -1)
        {
            try
            {
                return AddLibraryQuestionImpl(surveyId, pageId, libraryQuestionId, textsLanguage, InsertPosition.Last);
            }
            catch (VLException ex)
            {
                var message = string.Format("An exception occured while calling AddLibraryQuestion(), AccessTokenId={0}, Survey={1}, Page={2}, libraryQuestionId={3}, textsLanguage={4}", this.AccessTokenId, surveyId, pageId, libraryQuestionId, textsLanguage);
                Logger.Error(string.Format("RefId={0}, {1}", ex.ReferenceId, message), ex);
                throw new VLException("An exception occured at CreateQuestion()", ex.ReferenceId);
            }
            catch (Exception ex)
            {
                var message = string.Format("An exception occured while calling AddLibraryQuestion(), AccessTokenId={0}, Survey={1}, Page={2}, libraryQuestionId={3}, textsLanguage={4}", this.AccessTokenId, surveyId, pageId, libraryQuestionId, textsLanguage);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }
        }

        /// <summary>
        /// Προσθέτει την συγκεκριμένη 'library-question' μετά την referingQuestion
        /// </summary>
        /// <param name="referingQuestion"></param>
        /// <param name="libraryQuestion"></param>
        /// <returns></returns>
        public VLSurveyQuestion AddLibraryQuestionAfter(VLSurveyQuestion referingQuestion, VLLibraryQuestion libraryQuestion)
        {
            if (referingQuestion == null) throw new ArgumentNullException("referingQuestion");
            if (libraryQuestion == null) throw new ArgumentNullException("libraryQuestion");

            return AddLibraryQuestionAfter(referingQuestion.Survey, referingQuestion.Page, referingQuestion.QuestionId, libraryQuestion.QuestionId, referingQuestion.TextsLanguage);
        }
        /// <summary>
        /// Προσθέτει την συγκεκριμένη 'library-question' μετά την referingQuestion
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="pageId"></param>
        /// <param name="referingQuestionId"></param>
        /// <param name="libraryQuestionId"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLSurveyQuestion AddLibraryQuestionAfter(Int32 surveyId, Int16 pageId, Int16 referingQuestionId, Int32 libraryQuestionId, short textsLanguage = -1)
        {
            try
            {
                return AddLibraryQuestionImpl(surveyId, pageId, libraryQuestionId, textsLanguage, InsertPosition.After, referingQuestionId);
            }
            catch (VLException ex)
            {
                var message = string.Format("An exception occured while calling AddLibraryQuestionAfter(), AccessTokenId={0}, Survey={1}, Page={2}, libraryQuestionId={3}, textsLanguage={4}, referingQuestionId={5}", this.AccessTokenId, surveyId, pageId, libraryQuestionId, textsLanguage, referingQuestionId);
                Logger.Error(string.Format("RefId={0}, {1}", ex.ReferenceId, message), ex);
                throw new VLException("An exception occured at CreateQuestion()", ex.ReferenceId);
            }
            catch (Exception ex)
            {
                var message = string.Format("An exception occured while calling AddLibraryQuestionAfter(), AccessTokenId={0}, Survey={1}, Page={2}, libraryQuestionId={3}, textsLanguage={4}, referingQuestionId={5}", this.AccessTokenId, surveyId, pageId, libraryQuestionId, textsLanguage, referingQuestionId);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }
        }
        /// <summary>
        /// Προσθέτει την συγκεκριμένη 'library-question' πρίν την referingQuestion
        /// </summary>
        /// <param name="referingQuestion"></param>
        /// <param name="libraryQuestion"></param>
        /// <returns></returns>
        public VLSurveyQuestion AddLibraryQuestionBefore(VLSurveyQuestion referingQuestion, VLLibraryQuestion libraryQuestion)
        {
            if (referingQuestion == null) throw new ArgumentNullException("referingQuestion");
            if (libraryQuestion == null) throw new ArgumentNullException("libraryQuestion");

            return AddLibraryQuestionBefore(referingQuestion.Survey, referingQuestion.Page, referingQuestion.QuestionId, libraryQuestion.QuestionId, referingQuestion.TextsLanguage);
        }
        /// <summary>
        ///  Προσθέτει την συγκεκριμένη 'library-question' πρίν την referingQuestion
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="pageId"></param>
        /// <param name="referingQuestionId"></param>
        /// <param name="libraryQuestionId"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLSurveyQuestion AddLibraryQuestionBefore(Int32 surveyId, Int16 pageId, Int16 referingQuestionId, Int32 libraryQuestionId, short textsLanguage = -1)
        {
            try
            {
                return AddLibraryQuestionImpl(surveyId, pageId, libraryQuestionId, textsLanguage, InsertPosition.Before, referingQuestionId);
            }
            catch (VLException ex)
            {
                var message = string.Format("An exception occured while calling AddLibraryQuestionBefore(), AccessTokenId={0}, Survey={1}, Page={2}, libraryQuestionId={3}, textsLanguage={4}, referingQuestionId={5}", this.AccessTokenId, surveyId, pageId, libraryQuestionId, textsLanguage, referingQuestionId);
                Logger.Error(string.Format("RefId={0}, {1}", ex.ReferenceId, message), ex);
                throw new VLException("An exception occured at CreateQuestion()", ex.ReferenceId);
            }
            catch (Exception ex)
            {
                var message = string.Format("An exception occured while calling AddLibraryQuestionBefore(), AccessTokenId={0}, Survey={1}, Page={2}, libraryQuestionId={3}, textsLanguage={4}, referingQuestionId={5}", this.AccessTokenId, surveyId, pageId, libraryQuestionId, textsLanguage, referingQuestionId);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }
        }

        VLSurveyQuestion AddLibraryQuestionImpl(Int32 surveyId, Int16 pageId, Int32 libraryQuestionId, short textsLanguage, InsertPosition position = InsertPosition.Last, Int16? referingQuestionId = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientEditSurveys);
            }
            #endregion

            try
            {
                return SurveysDal.AddLibraryQuestion(AccessTokenId, surveyId, pageId, libraryQuestionId, textsLanguage, position, referingQuestionId);
            }
            catch (Exception ex)
            {
                var message = string.Format("AddLibraryQuestionImpl(), AccessTokenId={0}, Survey={1}, Page={2}, libraryQuestionId={3}, textsLanguage={4}, position = {5}, referingQuestionId={6}", this.AccessTokenId, surveyId, pageId, libraryQuestionId, textsLanguage, position, referingQuestionId);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="masterQuestion"></param>
        /// <param name="type"></param>
        /// <param name="questionText"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLSurveyQuestion CreateChildQuestion(VLSurveyQuestion masterQuestion, QuestionType type, string questionText, short textsLanguage = -1)
        {
            throw new NotImplementedException();
        }

        public VLSurveyQuestion UpdateQuestion(VLSurveyQuestion question)
        {
            #region input parameters validation
            if (question == null) throw new ArgumentNullException("question");
            question.ValidateInstance();
            #endregion



            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientEditSurveys);
            }
            #endregion


            //Υπάρχει η ερώτησή στο σύστημα:
            var existingItem = SurveysDal.GetQuestionById(AccessTokenId, question.Survey, question.QuestionId, question.TextsLanguage);
            if(existingItem == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Question", question.QuestionId));
            }
            //Δεν μπορούμε να αλλάξουμε τον τύπο της ερώτησης στο update:
            if(existingItem.QuestionType != question.QuestionType)
            {
                throw new VLException(string.Format("You cannot change the questionType of an existing question from {0} to {1}!", existingItem.QuestionType.ToString(), question.QuestionType.ToString()));
            }


            /*Ελέγχουμε την εγκυρότητα της αλλαγμένης ερώτησης, που πάμε να αποθηκεύσουμε:*/
            _Validatequestion(question);


            //existingItem.Page = question.Page;
            //existingItem.MasterQuestion = question.MasterQuestion;
            //existingItem.DisplayOrder = question.DisplayOrder;
            //existingItem.QuestionType = question.QuestionType;
            //existingItem.CustomType = question.CustomType;
            existingItem.IsRequired = question.IsRequired;
            existingItem.RequiredBehavior = question.RequiredBehavior;
            existingItem.RequiredMinLimit = question.RequiredMinLimit;
            existingItem.RequiredMaxLimit = question.RequiredMaxLimit;

            //AttributeFlags
            existingItem.OptionalInputBox = question.OptionalInputBox;
            existingItem.RandomizeOptionsSequence = question.RandomizeOptionsSequence;
            existingItem.DoNotRandomizeLastOption = question.DoNotRandomizeLastOption;
            existingItem.RandomizeColumnSequence = question.RandomizeColumnSequence;
            existingItem.OneResponsePerColumn = question.OneResponsePerColumn;
            existingItem.AddResetLink = question.AddResetLink;
            existingItem.UseDateTimeControls = question.UseDateTimeControls;

            existingItem.ValidationBehavior = question.ValidationBehavior;
            existingItem.ValidationField1 = question.ValidationField1;
            existingItem.ValidationField2 = question.ValidationField2;
            existingItem.ValidationField3 = question.ValidationField3;
            existingItem.RegularExpression = question.RegularExpression;
            existingItem.RandomBehavior = question.RandomBehavior;
            existingItem.OtherFieldType = question.OtherFieldType;
            existingItem.OtherFieldRows = question.OtherFieldRows;
            existingItem.OtherFieldChars = question.OtherFieldChars;
            //OptionsSequence
            //ColumnsSequence
            existingItem.RangeStart = question.RangeStart;
            existingItem.RangeEnd = question.RangeEnd;
            existingItem.CustomId = question.CustomId;
            //TextsLanguage
            existingItem.QuestionText = question.QuestionText;
            existingItem.Description = question.Description;
            existingItem.HelpText = question.HelpText;
            existingItem.FrontLabelText = question.FrontLabelText;
            existingItem.AfterLabelText = question.AfterLabelText;
            existingItem.InsideText = question.InsideText;
            existingItem.RequiredMessage = question.RequiredMessage;
            existingItem.ValidationMessage = question.ValidationMessage;
            existingItem.OtherFieldLabel = question.OtherFieldLabel;



            return SurveysDal.UpdateSurveyQuestion(AccessTokenId, existingItem);
        }


        private void _Validatequestion(VLSurveyQuestion question)
        {

            if (question.IsRequired == false)
            {
                question.RequiredBehavior = null;
                question.RequiredMinLimit = null;
                question.RequiredMaxLimit = null;
                question.RequiredMessage = null;
            }
            else
            {
                if(string.IsNullOrWhiteSpace(question.RequiredMessage))
                {
                    throw new VLException("RequiredMessage cannot be null or an empty string!");
                }
            }
            if (question.OptionalInputBox == false)
            {
                question.OtherFieldType = null;
                question.OtherFieldRows = null;
                question.OtherFieldChars = null;
                question.OtherFieldLabel = null;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(question.OtherFieldLabel))
                {
                    throw new VLException("You must give a label for the OtherField!");
                }
                if (question.OtherFieldType != OtherFieldType.SingleLine && question.OtherFieldType != OtherFieldType.MultipleLines)
                {
                    throw new VLException("You must give a type for the OtherField!");
                }

            }
            if (question.ValidationBehavior == ValidationMode.DoNotValidate)
            {
                question.ValidationField1 = null;
                question.ValidationField2 = null;
                question.ValidationField3 = null;
                question.RegularExpression = null;
                question.ValidationMessage = null;
            }
            else
            {

                if (question.ValidationBehavior == ValidationMode.TextOfSpecificLength || question.ValidationBehavior == ValidationMode.WholeNumber)
                {
                    Int32 _between = 0, _and = 0;

                    //Είναι απαραίτητο να υπάρχει ValidationMessage:
                    if (string.IsNullOrWhiteSpace(question.ValidationMessage))
                    {
                        throw new VLException("ValidationMessage cannot be null or an empty string!");
                    }

                    //Πρέπει το ValidationField1 & ValidationField2 να είναι ακέραιοι αριθμοί και ValidationField1 <= ValidationField2:                    
                    if (string.IsNullOrWhiteSpace(question.ValidationField1))
                    {
                        throw new VLException("Validation setup error. First range value is wrong!");
                    }
                    if (string.IsNullOrWhiteSpace(question.ValidationField2))
                    {
                        throw new VLException("Validation setup error. Second range value is wrong!");
                    }
                    try
                    {
                        _between = Convert.ToInt32(question.ValidationField1, CultureInfo.InvariantCulture);
                        _and = Convert.ToInt32(question.ValidationField2, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        throw new VLException("Validation setup error. Range values have wrong types!");
                    }

                    if(_between > _and)
                    {
                        throw new VLException("Validation setup error. Range values are wrong!");
                    }
                }
                else if (question.ValidationBehavior == ValidationMode.DecimalNumber)
                {
                    Double _between = 0, _and = 0;

                    //Είναι απαραίτητο να υπάρχει ValidationMessage:
                    if (string.IsNullOrWhiteSpace(question.ValidationMessage))
                    {
                        throw new VLException("ValidationMessage cannot be null or an empty string!");
                    }

                    //Πρέπει το ValidationField1 & ValidationField2 να είναι ακέραιοι αριθμοί και ValidationField1 <= ValidationField2:
                    if (string.IsNullOrWhiteSpace(question.ValidationField1))
                    {
                        throw new VLException("Validation setup error. First range value is wrong!");
                    }
                    if (string.IsNullOrWhiteSpace(question.ValidationField2))
                    {
                        throw new VLException("Validation setup error. Second range value is wrong!");
                    }
                    try
                    {
                        _between = Convert.ToDouble(question.ValidationField1, CultureInfo.InvariantCulture);
                        _and = Convert.ToDouble(question.ValidationField2, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        throw new VLException("Validation setup error. Range values have wrong types!");
                    }

                    if (_between > _and)
                    {
                        throw new VLException("Validation setup error. Range values are wrong!");
                    }
                }
                else if (question.ValidationBehavior == ValidationMode.Date1)
                {
                    //Είναι απαραίτητο να υπάρχει ValidationMessage:
                    if (string.IsNullOrWhiteSpace(question.ValidationMessage))
                    {
                        question.ValidationMessage = "The specified date is invalid";
                    }

                }
                else if (question.ValidationBehavior == ValidationMode.Date2)
                {
                    //Είναι απαραίτητο να υπάρχει ValidationMessage:
                    if (string.IsNullOrWhiteSpace(question.ValidationMessage))
                    {
                        question.ValidationMessage = "The specified date is invalid";
                    }

                }
                else if (question.ValidationBehavior == ValidationMode.Email)
                {
                    //Είναι απαραίτητο να υπάρχει ValidationMessage:
                    if (string.IsNullOrWhiteSpace(question.ValidationMessage))
                    {
                        question.ValidationMessage = "Email address is invalid";
                    }

                }
                else if (question.ValidationBehavior == ValidationMode.RegularExpression)
                {
                    //ΔΕΝ Είναι απαραίτητο να υπάρχει ValidationMessage:

                }

            }
        }


        public VLSurveyQuestionDeleteOptions GetDeleteQuestionOptions(Int32 surveyId, Int16 questionId, short textsLanguage = -1)
        {
            var _options = new VLSurveyQuestionDeleteOptions();


            #region SecurityLayer
            _options.CanBeDeleted = true;
            _options.HasUserDeletePermission = true;
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                if (ValidatePermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService) == false)
                {
                    _options.CanBeDeleted = false;
                    _options.HasUserDeletePermission = false;
                }
            }
            else
            {
                if (ValidatePermissions(VLPermissions.ClientEditSurveys) == false)
                {
                    _options.CanBeDeleted = false;
                    _options.HasUserDeletePermission = false;
                }
            }
            #endregion

            var question = SurveysDal.GetQuestionById(this.AccessTokenId, surveyId, questionId, textsLanguage);
            _options.SurveyId = question.Survey;
            _options.PageId = question.Page;
            _options.QuestionId = question.QuestionId;
            _options.MasterQuestion = question.MasterQuestion;
            _options.QuestionText = question.QuestionText;
            _options.QuestionType = question.QuestionType;

            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, surveyId, textsLanguage);
            _options.SurveyHasResponses = survey.HasResponses;
            _options.IsBuiltin = survey.IsBuiltIn;


            return _options;
        }
        public void DeleteQuestion(VLSurveyQuestion question)
        {
            if (question == null) throw new ArgumentNullException("question");

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientEditSurveys);
            }
            #endregion

            DeleteQuestion(question.Survey, question.QuestionId);
        }
        public void DeleteQuestion(Int32 survey, Int16 questionId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientEditSurveys);
            }
            #endregion

            try
            {
                DeleteQuestionImpl(survey, questionId);
            }
            catch (VLException ex)
            {
                var message = string.Format("An exception occured while calling DeleteQuestion(), AccessTokenId={0}, surveyId={1}, questionId={2}", this.AccessTokenId, survey, questionId);
                Logger.Error(string.Format("RefId={0}, {1}", ex.ReferenceId, message), ex);
                throw new VLException(ex.Message, ex.ReferenceId);
            }
            catch (Exception ex)
            {
                var message = string.Format("An exception occured while calling DeleteQuestion(), AccessTokenId={0}, surveyId={1}, questionId={2}", this.AccessTokenId, survey, questionId);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }
        }
        public void DeleteQuestionImpl(Int32 survey, Int16 questionId)
        {
            var existingQuestion = SurveysDal.GetQuestionById(this.AccessTokenId, survey, questionId, BuiltinLanguages.PrimaryLanguage);
            if (existingQuestion == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "SurveyQuestion", string.Format("({0}, {1})", survey, questionId)));

            /*Δεν επιτρέπουμε την διαγραφή της ερώτησης, εάν έχει SKIP LOGIC:*/
            if(existingQuestion.HasSkipLogic)
            {
                throw new VLException(string.Format("Question '{0}', cannot be deleted because it contains QUESTION LOGIC.", Utility.TruncateString(existingQuestion.QuestionText), existingQuestion.QuestionId));
            }
            /*Δεν επιτρέπουμε την διαγραφή της ερώτησης, εάν έχει τεθεί σαν SkipToQuestion σε κάποιο Option του ερωτηματολογίου:*/
            var options = SurveysDal.GetQuestionOptions(this.AccessTokenId, survey, BuiltinLanguages.PrimaryLanguage);
            foreach (var op in options)
            {
                if (op.SkipTo == SkipToBehavior.AnotherPage && op.SkipToQuestion == existingQuestion.QuestionId)
                {
                    /*Συμμετέχει σε skip logic*/
                    throw new VLException(string.Format("Question '{0}', cannot be deleted because is target of QUESTION LOGIC.", Utility.TruncateString(existingQuestion.QuestionText), existingQuestion.QuestionId));
                }
            }


            /*Προχωρούμε στην διαγραφή:*/
            SurveysDal.DeleteQuestion(this.AccessTokenId, survey, questionId);
        }
        #endregion

        #region VLQuestionOption
        /// <summary>
        /// Επιστρέφει τα options της συγκεκριμένης ερώτησης, πάντα ταξονομημένα ως προς το DisplayOrder τους.
        /// <para>Η ταξινόμηση αυτή είναι πολύ σημαντική! (Χρησιμοποιείται για την διαδικασία της μετάφρασης)</para>
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public Collection<VLQuestionOption> GetQuestionOptions(VLSurveyQuestion question)
        {
            if(question == null) throw new ArgumentNullException("question");
            return GetQuestionOptions(question.Survey, question.QuestionId, question.TextsLanguage);
        }
        /// <summary>
        /// Επιστρέφει τα options της συγκεκριμένης ερώτησης, πάντα ταξονομημένα ως προς το DisplayOrder τους.
        /// <para>Η ταξινόμηση αυτή είναι πολύ σημαντική! (Χρησιμοποιείται για την διαδικασία της μετάφρασης)</para>
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public Collection<VLQuestionOption> GetQuestionOptions(Int32 survey, Int16 question, short textsLanguage = -1)
        {
            return SurveysDal.GetQuestionOptions(AccessTokenId, survey, question, textsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="option"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLQuestionOption GetQuestionOptionById(Int32 survey, Int16 question, Byte option, short textsLanguage = -1)
        {
            return SurveysDal.GetQuestionOptionById(AccessTokenId, survey, question, option, textsLanguage);
        }

        /// <summary>
        /// Διαγράφει το ΣΥΓΚΕΚΡΙΜΕΝΟ option απο το σύστημα.
        /// <para>Η διαγραφή αφορά όλες τις (τυχόν) μεταφράσεις του option.</para>
        /// </summary>
        /// <param name="option"></param>
        public void DeleteQuestionOption(VLQuestionOption option)
        {
            if (option == null) throw new ArgumentNullException("option");
            DeleteQuestionOption(option.Survey, option.Question, option.OptionId);
        }
        /// <summary>
        /// Διαγράφει το ΣΥΓΚΕΚΡΙΜΕΝΟ option απο το σύστημα.
        /// <para>Η διαγραφή αφορά όλες τις (τυχόν) μεταφράσεις του option.</para>
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="option"></param>
        public void DeleteQuestionOption(Int32 survey, Int16 question, Byte option)
        {
            SurveysDal.DeleteQuestionOption(AccessTokenId, survey, question, option);
        }



        /// <summary>
        /// Διαγράφει ΟΛΑ ΤΑ OPTIONS της συγκεκριμένης ερώτησης.
        /// <para>Η διαγραφή αφορά όλες τις (τυχόν) μεταφράσεις των options.</para>
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        public void DeleteAllQuestionOptions(Int32 survey, Int16 question)
        {
            //security

            //can i delete them?

            SurveysDal.DeleteAllQuestionOptions(AccessTokenId, survey, question);
        }




        public VLQuestionOption CreateQuestionOption(VLSurveyQuestion question, string optionText, QuestionOptionType type = QuestionOptionType.Default)
        {
            if (question == null) throw new ArgumentNullException("question");
            return CreateQuestionOption(question.Survey, question.QuestionId, optionText, type, question.TextsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="optionText"></param>
        /// <param name="type"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLQuestionOption CreateQuestionOption(Int32 survey, Int16 question, string optionText, QuestionOptionType type = QuestionOptionType.Default, short textsLanguage = -1)
        {
            VLQuestionOption option = new VLQuestionOption();
            option.Survey = survey;
            option.Question = question;
            option.OptionText = optionText;
            option.OptionType = type;
            return CreateQuestionOption(option, textsLanguage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        internal VLQuestionOption CreateQuestionOption(VLQuestionOption option, short textsLanguage = -1)
        {
            if (option == null) throw new ArgumentNullException("option");
            option.ValidateInstance();

            //διαβάζουμε την ερώτηση απο το σύστημα:
            var question = SurveysDal.GetQuestionById(this.AccessTokenId, option.Survey, option.Question, textsLanguage);
            if(question == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "SurveyQuestion", option.Question));
            }

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            //Options μπορούμε να δημιουργήσουμε σε συγκεκριμένους τύπους ερωτήσεων:
            if (SupportOptions(question.QuestionType) == false)
            {
                throw new VLException(string.Format("A question of type {0}, does not support options!", question.QuestionType));
            }



            return SurveysDal.CreateQuestionOption(AccessTokenId, option, textsLanguage);
        }

        public VLQuestionOption UpdateQuestionOption(VLQuestionOption option)
        {
            if (option == null) throw new ArgumentNullException("option");
            option.ValidateInstance();

            //διαβάζουμε το option απο την βάση μας:
            var existingItem = SurveysDal.GetQuestionOptionById(AccessTokenId, option.Survey, option.Question, option.OptionId, option.TextsLanguage);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Option", option.OptionId));

            existingItem.OptionText = option.OptionText;
            existingItem.CustomId = option.CustomId;

            return SurveysDal.UpdateQuestionOption(AccessTokenId, existingItem);
        }
        #endregion

        #region VLQuestionColumn
        /// <summary>
        /// Επιστρέφει τα columns της συγκεκριμένης ερώτησης, πάντα ταξονομημένα ως προς το DisplayOrder τους.
        /// <para>Η ταξινόμηση αυτή είναι πολύ σημαντική! (Χρησιμοποιείται για την διαδικασία της μετάφρασης)</para>
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public Collection<VLQuestionColumn> GetQuestionColumns(VLSurveyQuestion question)
        {
            if (question == null) throw new ArgumentNullException("question");
            return GetQuestionColumns(question.Survey, question.QuestionId, question.TextsLanguage);
        }
        /// <summary>
        /// Επιστρέφει τα columns της συγκεκριμένης ερώτησης, πάντα ταξονομημένα ως προς το DisplayOrder τους.
        /// <para>Η ταξινόμηση αυτή είναι πολύ σημαντική! (Χρησιμοποιείται για την διαδικασία της μετάφρασης)</para>
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public Collection<VLQuestionColumn> GetQuestionColumns(Int32 survey, Int16 question, short textsLanguage = -1)
        {
            return SurveysDal.GetQuestionColumns(AccessTokenId, survey, question, textsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="column"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLQuestionColumn GetQuestionColumnById(Int32 survey, Int16 question, Byte column, short textsLanguage = -1)
        {
            return SurveysDal.GetQuestionColumnById(AccessTokenId, survey, question, column, textsLanguage);
        }

        public void DeleteQuestionColumn(VLQuestionColumn column)
        {
            if (column == null) throw new ArgumentNullException("column");
            
            DeleteQuestionColumn(column.Survey, column.Question, column.ColumnId);
        }
        public void DeleteQuestionColumn(Int32 survey, Int16 question, Byte columnId)
        {
            SurveysDal.DeleteQuestionColumn(AccessTokenId, survey, question, columnId);
        }

        public void DeleteAllQuestionColumns(VLSurveyQuestion question)
        {
            if (question == null) throw new ArgumentNullException("question");
            DeleteAllQuestionColumns(question.Survey, question.QuestionId);
        }
        public void DeleteAllQuestionColumns(Int32 survey, Int16 question)
        {
            SurveysDal.DeleteAllQuestionColumns(AccessTokenId, survey, question);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        /// <param name="columnText"></param>
        /// <returns></returns>
        public VLQuestionColumn CreateQuestionColumn(VLSurveyQuestion question, string columnText)
        {
            if (question == null) throw new ArgumentNullException("question");
            return CreateQuestionColumn(question.Survey, question.QuestionId, columnText, question.TextsLanguage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="columnText"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public VLQuestionColumn CreateQuestionColumn(Int32 survey, Int16 question, string columnText, short textsLanguage = -1)
        {
            VLQuestionColumn column = new VLQuestionColumn();
            column.Survey = survey;
            column.Question = question;
            column.ColumnText = columnText;
            return CreateQuestionColumn(column, textsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        internal VLQuestionColumn CreateQuestionColumn(VLQuestionColumn column, short textsLanguage = -1)
        {
            if (column == null) throw new ArgumentNullException("column");
            column.ValidateInstance();

            //διαβάζουμε την ερώτηση απο το σύστημα:
            var question = SurveysDal.GetQuestionById(this.AccessTokenId, column.Survey, column.Question, textsLanguage);
            if (question == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "SurveyQuestion", column.Question));
            }

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            //Columns μπορούμε να δημιουργήσουμε σε συγκεκριμένους τύπους ερωτήσεων:
            if (SupportColumns(question.QuestionType) == false)
            {
                throw new VLException(string.Format("A question of type {0}, does not support columns!", question.QuestionType));
            }



            return SurveysDal.CreateQuestionColumn(AccessTokenId, column, textsLanguage);
        }

        public VLQuestionColumn UpdateQuestionColumn(VLQuestionColumn column)
        {
            if (column == null) throw new ArgumentNullException("column");
            column.ValidateInstance();

            //διαβάζουμε το option απο την βάση μας:
            var existingItem = SurveysDal.GetQuestionColumnById(AccessTokenId, column.Survey, column.Question, column.ColumnId, column.TextsLanguage);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Column", column.ColumnId));

            existingItem.ColumnText = column.ColumnText;
            existingItem.CustomId = column.CustomId;

            return SurveysDal.UpdateQuestionColumn(AccessTokenId, existingItem);
        }
        #endregion

        #region VLCollector
        /// <summary>
        /// Επιστρέφει όλους τους collectors για το συγκεκριμένο survey
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLCollector> GetCollectors(VLSurvey survey, string whereClause = null, string orderByClause = null, short textsLanguage = -1)
        {
            if (survey == null) throw new ArgumentNullException("survey");
            return GetCollectors(survey.SurveyId, whereClause, orderByClause, textsLanguage);
        }
        /// <summary>
        /// Επιστρέφει όλους τους collectors για το συγκεκριμένο survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public Collection<VLCollector> GetCollectors(Int32 surveyId, string whereClause = null, string orderByClause = null, short textsLanguage = -1)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetCollectors(this.AccessTokenId, surveyId, whereClause, orderByClause, textsLanguage);
        }
        /// <summary>
        /// Επιστρέφει, ανα σελίδα, όλους τους collectors για το συγκεκριμένο survey
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLCollector> GetCollectors(VLSurvey survey, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null, short textsLanguage = -1)
        {
            if (survey == null) throw new ArgumentNullException("survey");
            return GetCollectors(survey.SurveyId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause, textsLanguage);
        }
        /// <summary>
        /// Επιστρέφει, ανα σελίδα, όλους τους collectors για το συγκεκριμένο survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage">Η γλώσσα ανάκτησης των μεταφραζόμενων πεδίων. Εάν δεν οριστεί θα χρησιμοποιηθεί η PrimaryLanguage του survey.</param>
        /// <returns></returns>
        public Collection<VLCollector> GetCollectors(Int32 surveyId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null, short textsLanguage = -1)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetCollectors(this.AccessTokenId, surveyId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause, textsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="whereClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Int32 GetCollectorsCount(Int32 surveyId, string whereClause = null, short textsLanguage = -1)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetCollectorsCount(this.AccessTokenId, surveyId, whereClause, textsLanguage);
        }



        /// <summary>
        /// Επιστρέφει όλους τους collectors που ανήκουν στον συγκεκριμένο χρήστη (client) που πραγματοποιεί την κλήση, και είναι του πρσδιορισμένου τύπου.
        /// </summary>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public Collection<VLCollectorPeek> GetCollectorPeeks(CollectorType ctype)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion


            return SurveysDal.GetCollectorPeeks(this.AccessTokenId, this.ClientId.Value, ctype);
        }

        public VLCollector GetCollectorById(Int32 collectorId, short textsLanguage = -1)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, textsLanguage);
        }

        public VLCollector GetCollectorByWebLink(string webLink, short textsLanguage = -1)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetCollectorByWebLink(this.AccessTokenId, webLink, textsLanguage);
        }

        /// <summary>
        /// ια να διαγράψουμε ένα collector, δεν πρέπει να υπάρχουν responses για αυτόν στον πίνακα 'Responses'.
        /// </summary>
        /// <param name="collector"></param>
        public void DeleteCollector(VLCollector collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            DeleteCollector(collector.CollectorId);
        }

        /// <summary>
        /// Για να διαγράψουμε ένα collector, δεν πρέπει να υπάρχουν responses για αυτόν στον πίνακα 'Responses'.
        /// </summary>
        /// <param name="collectorId"></param>
        public void DeleteCollector(Int32 collectorId)
        {
            var existingItem = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", collectorId));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion

            /*εάν ο collector είναι ανοιχτός, δεν επιτρέπουμε την διαγραφή του:*/
            if (existingItem.Status == CollectorStatus.Open)
            {
                throw new VLException("Cannot delete an open Collector!");
            }
            /*εάν ο collector έχει responses, δεν επιτρέπουμε την διαγραφή του:*/
            if (SurveysDal.GetResponsesCount(this.AccessTokenId, existingItem.Survey, string.Format("where Collector={0}", collectorId)) > 0)
            {
                throw new VLException("Cannot delete a Collector with Responses. You must Clear it first!");
            }
            /*εάν ο collector έχει συνδεδεμένα payments που έχουν χρησιμοποιηθεί τότε, δεν επιτρέπουμε την διαγραφή του:*/
            var collectorPayments = SystemDal.GetCollectorPaymentsForCollector(this.AccessTokenId, collectorId);
            foreach (var item in collectorPayments)
            {
                if(item.IsUsed)
                {
                    throw new VLException("Cannot delete a Collector with charges!");
                }
            }


            SurveysDal.DeleteCollector(this.AccessTokenId, existingItem.CollectorId);
        }
        /// <summary>
        /// Δημιουργεί ένα νέο collector για κάποιο προυπάρχον survey.
        /// <para>Κατα την δημιουργία ενός collector, αποθηκεύονται μαζί με αυτό, το ClientProfile και το UseCredits που φέρει εκείνη την στιγμή ο Client,
        /// στον οποίο ανήκει ο Collector.</para>
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="collectorType"></param>
        /// <param name="collectorName"></param>
        /// <param name="creditType"></param>
        /// <returns></returns>
        public VLCollector CreateCollector(VLSurvey survey, CollectorType collectorType, string collectorName, CreditType creditType = CreditType.None)
        {
            if (survey == null) throw new ArgumentNullException("survey");
            return CreateCollector(survey.SurveyId, collectorType, collectorName, creditType, survey.TextsLanguage);
        }
        /// <summary>
        /// Δημιουργεί ένα νέο collector για κάποιο προυπάρχον survey.
        /// <para>Κατα την δημιουργία ενός collector, αποθηκεύονται μαζί με αυτό, το ClientProfile και το UseCredits που φέρει εκείνη την στιγμή ο Client,
        /// στον οποίο ανήκει ο Collector.</para>
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="collectorType"></param>
        /// <param name="collectorName"></param>
        /// <param name="creditType"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLCollector CreateCollector(Int32 surveyId, CollectorType collectorType, string collectorName, CreditType creditType = CreditType.None, short textsLanguage = -1)
        {
            switch (collectorType)
            {
                case CollectorType.WebLink:
                    return Prepare_WebLink_Collector(surveyId, collectorName, creditType, textsLanguage);
                case CollectorType.Email:
                    return Prepare_Email_Collector(surveyId, collectorName, creditType, textsLanguage);
                default:
                    throw new NotSupportedException();
            }
        }

        private VLCollector Prepare_WebLink_Collector(Int32 surveyId, string collectorName, CreditType creditType, short textsLanguage = -1)
        {
            VLCollector newCollector = new VLCollector();
            newCollector.Survey = surveyId;
            newCollector.CollectorType = CollectorType.WebLink;
            newCollector.Name = collectorName;
            newCollector.Status = CollectorStatus.Open;

            //Allow Multiple Responses? 
            newCollector.AllowMultipleResponsesPerComputer = false;
            //Allow Responses to be Edited?
            newCollector.EditResponseMode = EditResponseModes.AllowedBetween;

            //Display Survey Results?
            newCollector.DisplayInstantResults = true;
            newCollector.DisplayNumberOfResponses = false;
            //Survey Completion
            newCollector.OnCompletionMode = EndSurveyMode.CloseWindow;
            newCollector.OnCompletionURL = null;
            newCollector.DisqualificationPageURL = null;
            //Use SSL encryption?
            newCollector.UseSSL = false; //TODO: true
            //Save IP Address in Results?
            newCollector.SaveIPAddressOrEmail = true;

            //Set a Cutoff Date & Time
            newCollector.EnableStopCollectorDT = false;
            // Set a Max Response Count
            newCollector.EnableMaxResponseCount = false;
            //Enable Password Protection
            newCollector.EnablePasswordProtection = false;
            //Enable IP Blocking
            newCollector.EnableIPBlocking = false;


            /*
                 * Πρέπει να φτιάξουμε ένα νέο μοναδικό WebLink  
                 */
            int length = WebLink_Length, tries = 0;
            newCollector.WebLink = Utility.GenerateWebLink(length);
            while (SurveysDal.GetCollectorByWebLink(this.AccessTokenId, newCollector.Survey, newCollector.WebLink, BuiltinLanguages.PrimaryLanguage) != null)
            {
                tries++;
                if (tries % 12 == 0)
                    length++;

                newCollector.WebLink = Utility.GenerateWebLink(length);
            }



            return CreateCollectorImpl(newCollector, creditType, textsLanguage);
        }
        private VLCollector Prepare_Email_Collector(Int32 surveyId, string collectorName, CreditType creditType, short textsLanguage = -1)
        {
            VLCollector newCollector = new VLCollector();
            newCollector.Survey = surveyId;
            newCollector.CollectorType = CollectorType.Email;
            newCollector.Name = collectorName;
            newCollector.Status = CollectorStatus.New;

            //Allow Multiple Responses? 
            newCollector.AllowMultipleResponsesPerComputer = false;
            //Allow Responses to be Edited?
            newCollector.EditResponseMode = EditResponseModes.AllowedBetween;

            //Display Survey Results?
            newCollector.DisplayInstantResults = false;
            newCollector.DisplayNumberOfResponses = false;
            //Survey Completion
            newCollector.OnCompletionMode = EndSurveyMode.CloseWindow;
            newCollector.OnCompletionURL = null;

            newCollector.DisqualificationPageURL = null;
            //Use SSL encryption?
            newCollector.UseSSL = false; //TODO: true
            //Save IP Address in Results?
            newCollector.SaveIPAddressOrEmail = true;

            //Set a Cutoff Date & Time
            newCollector.EnableStopCollectorDT = false;
            // Set a Max Response Count
            newCollector.EnableMaxResponseCount = false;
            //Enable Password Protection
            newCollector.EnablePasswordProtection = false;
            //Enable IP Blocking
            newCollector.EnableIPBlocking = false;

            return CreateCollectorImpl(newCollector, creditType, textsLanguage);
        }
        private VLCollector Prepare_WebSite_Collector(Int32 surveyId, string collectorName, CreditType creditType, short textsLanguage = -1)
        {
            VLCollector newCollector = new VLCollector();
            newCollector.Survey = surveyId;
            newCollector.CollectorType = CollectorType.Website;
            newCollector.Name = collectorName;
            newCollector.Status = CollectorStatus.New;

            //Allow Multiple Responses? 
            newCollector.AllowMultipleResponsesPerComputer = false;
            //Allow Responses to be Edited?
            newCollector.EditResponseMode = EditResponseModes.AllowedBetween;

            //Display Survey Results?
            newCollector.DisplayInstantResults = false;
            newCollector.DisplayNumberOfResponses = false;
            //Survey Completion
            newCollector.OnCompletionMode = EndSurveyMode.CloseWindow;
            newCollector.OnCompletionURL = null;

            newCollector.DisqualificationPageURL = null;
            //Use SSL encryption?
            newCollector.UseSSL = false; //TODO: true
            //Save IP Address in Results?
            newCollector.SaveIPAddressOrEmail = true;

            //Set a Cutoff Date & Time
            newCollector.EnableStopCollectorDT = false;
            // Set a Max Response Count
            newCollector.EnableMaxResponseCount = false;
            //Enable Password Protection
            newCollector.EnablePasswordProtection = false;
            //Enable IP Blocking
            newCollector.EnableIPBlocking = false;

            return CreateCollectorImpl(newCollector, creditType, textsLanguage);
        }

        private VLCollector CreateCollectorImpl(VLCollector collector, CreditType creditType, short textsLanguage = -1)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            collector.ValidateInstance();


            //Διαβάζουμε το survey απο το σύστημα
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, textsLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));
            Boolean _useCredits = false;

            #region SecurityLayer & find if Survey's client UseCredits:
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);

                var profile = SystemDal.GetClientProfileForClient(this.AccessTokenId, survey.Client);
                if (profile == null) throw new VLException(string.Format("Invalid profile for client with id {0}", survey.Client));
                _useCredits = profile.UseCredits;
                collector.Profile = profile.ProfileId;
            }
            else
            {
                if (this.ClientId != survey.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
                _useCredits = this.UseCredits.Value;
                collector.Profile = this.Profile;
            }
            #endregion


            //Πρέπει το όνομα ενός Collector να είναι μοναδικό για το ίδιο survey:
            if(SurveysDal.GetCollectorByName(this.AccessTokenId, collector.Survey, collector.Name, BuiltinLanguages.PrimaryLanguage) != null)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", collector.Name));
            }

            /*Τώρα θα ασχοληθούμε με την Χρέωση του Collector:*/
            if(_useCredits == true)
            {
                /*
                 * Απο την στιγμή που χρεώνεται ο Collector, εμείς πρέπει να δημιουργήσουμε τον Collector σε Status New, έτσι ώστε να μπορέσει 
                 * ο χρήστης να προσθέσει στον Collector την πληρωμή απο όπου θα γίνει η χρέωση!
                 */
                collector.Status = CollectorStatus.New;
             
                if (collector.CollectorType == CollectorType.Email)
                {
                    if (creditType != CreditType.ResponseType && creditType != CreditType.EmailType)
                        throw new VLException(string.Format("CreditType '{0}', is invalid for an Email Collector!",creditType));
             
                    collector.UseCredits = true;
                    collector.CreditType = creditType;
                }
                else if (collector.CollectorType == CollectorType.WebLink)
                {
                    if (creditType != CreditType.ResponseType && creditType != CreditType.ClickType)
                        throw new VLException(string.Format("CreditType '{0}', is invalid for a WebLink Collector!", creditType));

                    collector.UseCredits = true;
                    collector.CreditType = creditType;
                }
                else if (collector.CollectorType == CollectorType.Website)
                {
                    if (creditType != CreditType.ResponseType && creditType != CreditType.ClickType)
                        throw new VLException(string.Format("CreditType '{0}', is invalid for a Website Collector!", creditType));

                    collector.UseCredits = true;
                    collector.CreditType = creditType;
                }
                else
                {
                    throw new VLException(string.Format("Unknown Collector type!", collector.CollectorType));
                }
            }


            return SurveysDal.CreateCollector(this.AccessTokenId, collector, textsLanguage);
        }


        public VLCollector UpdateCollector(VLCollector collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            collector.ValidateInstance();

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientManageCollectors);
            }
            #endregion


            /*Το όνομα ενός Collector πρέπει να είναι μοναδικό για το ίδιο survey:*/
            var existingItem = SurveysDal.GetCollectorByName(this.AccessTokenId, collector.Survey, collector.Name, collector.TextsLanguage);
            if(existingItem != null && existingItem.CollectorId != collector.CollectorId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", collector.Name));
            }
            if (existingItem == null)
            {
                existingItem = SurveysDal.GetCollectorById(this.AccessTokenId, collector.CollectorId, collector.TextsLanguage);
            }
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", collector.CollectorId));


            /*Δεν μπορεί να αλλάξει ο τύπος ενός collector άπαξ και δημιουργηθεί!*/
            if(existingItem.CollectorType != collector.CollectorType)
            {
                throw new VLException("A collector's type cannot be changed!");
            }


            //To update θα γίνει μέσω του existingItem, και ειδικά για κάθε τύπο:
            existingItem.Name = collector.Name;
            if(existingItem.CollectorType == CollectorType.WebLink)
            {
                #region WebLink
                existingItem.AllowMultipleResponsesPerComputer = collector.AllowMultipleResponsesPerComputer;
                existingItem.EditResponseMode = collector.EditResponseMode;



                if(collector.DisplayInstantResults)
                {
                    existingItem.DisplayInstantResults = true;
                    existingItem.DisplayNumberOfResponses = collector.DisplayNumberOfResponses;
                }
                else
                {
                    existingItem.DisplayInstantResults = false;
                    existingItem.DisplayNumberOfResponses = false;
                }

                if (collector.OnCompletionMode == EndSurveyMode.CloseWindow)
                {
                    existingItem.OnCompletionMode = EndSurveyMode.CloseWindow;
                    existingItem.OnCompletionURL = null;
                }
                else if(collector.OnCompletionMode == EndSurveyMode.GoToUrl)
                {
                    existingItem.OnCompletionMode = EndSurveyMode.GoToUrl;
                    if (string.IsNullOrEmpty(collector.OnCompletionURL))
                    {
                        throw new ArgumentNullException("OnCompletionURL");
                    }
                    existingItem.OnCompletionURL = collector.OnCompletionURL;
                }
                #endregion

            }
            else if (existingItem.CollectorType == CollectorType.Email)
            {
                #region Email
                existingItem.AllowMultipleResponsesPerComputer = false;
                existingItem.EditResponseMode = collector.EditResponseMode;



                if (collector.DisplayInstantResults)
                {
                    existingItem.DisplayInstantResults = true;
                    existingItem.DisplayNumberOfResponses = collector.DisplayNumberOfResponses;
                }
                else
                {
                    existingItem.DisplayInstantResults = false;
                    existingItem.DisplayNumberOfResponses = false;
                }

                if (collector.OnCompletionMode == EndSurveyMode.CloseWindow)
                {
                    existingItem.OnCompletionMode = EndSurveyMode.CloseWindow;
                    existingItem.OnCompletionURL = null;
                }
                else if (collector.OnCompletionMode == EndSurveyMode.GoToUrl)
                {
                    existingItem.OnCompletionMode = EndSurveyMode.GoToUrl;
                    if (string.IsNullOrEmpty(collector.OnCompletionURL))
                    {
                        throw new ArgumentNullException("OnCompletionURL");
                    }
                    existingItem.OnCompletionURL = collector.OnCompletionURL;
                }
                #endregion
                existingItem.HasSentEmails = collector.HasSentEmails;
            }
            else if (existingItem.CollectorType == CollectorType.Website)
            {

            }

            existingItem.UseSSL = collector.UseSSL;
            existingItem.SaveIPAddressOrEmail = collector.SaveIPAddressOrEmail;


            //Restrictions
            if (collector.EnableStopCollectorDT && collector.StopCollectorDT.HasValue)
            {
                //Πρέπει το StopCollectorDT να βρίσκεται ττο μέλλον
                var stopCollectorDtUtc = this.ConvertTimeToUtc(collector.StopCollectorDT.Value);
                if (stopCollectorDtUtc < Utility.UtcNow().AddMinutes(5))
                {
                    throw new VLException("Please select a cut off day and time that is in the future.");
                }

                existingItem.EnableStopCollectorDT = true;
                existingItem.StopCollectorDT = stopCollectorDtUtc;
            }
            else
            {
                existingItem.EnableStopCollectorDT = false;
                existingItem.StopCollectorDT = null;
            }
            if (collector.EnableMaxResponseCount && collector.MaxResponses.HasValue)
            {
                if (collector.MaxResponses.Value <= 0)
                {
                    throw new VLException("Max Responses Count must a positive number!");
                }

                existingItem.EnableMaxResponseCount = true;
                existingItem.MaxResponses = collector.MaxResponses;
            }
            else
            {
                existingItem.EnableMaxResponseCount = false;
                existingItem.MaxResponses = null;
            }



            return SurveysDal.UpdateCollector(this.AccessTokenId, existingItem);
        }

        /// <summary>
        /// Κλείνει έναν ανοιχτό Collector.
        /// <para></para>
        /// </summary>
        /// <param name="collector"></param>
        /// <returns></returns>
        public VLCollector CloseCollector(VLCollector collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return CloseCollector(collector.CollectorId);
        }
        /// <summary>
        /// Κλείνει έναν ανοιχτό Collector.
        /// <para></para>
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public VLCollector CloseCollector(Int32 collectorId)
        {
            /*διαβάζουμε τον collector απο το σύστημά μας:*/
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", collectorId));
            
            //Διαβάζουμε το survey απο το σύστημα
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                if (this.ClientId != survey.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion


            if(collector.Status != CollectorStatus.Open)
            {
                throw new VLException(string.Format("Collector {0}, is not Open", collector.Name));
            }

            /*Τώρα θέλουμε το σύνολο των μηνυμάτων που είναι scheduled:*/
            var totalScheduledMessages = GetScheduledMessagesCount(collector.CollectorId);
            if (totalScheduledMessages > 0)
            {
                throw new VLException(string.Format("This collector, '{0}', has pending or scheduled messages. Cannot be closed!", collector.Name));
            }

            collector.Status = CollectorStatus.Close;
            collector.ClosedByUser = true;
            return SurveysDal.UpdateCollector(this.AccessTokenId, collector);
        }

        /// <summary>
        /// Ανοίγει ένα Collector, για συλλογή απαντήσεων στο ερωτηματολόγιο του.
        /// </summary>
        /// <param name="collector"></param>
        /// <returns></returns>
        public VLCollector OpenCollector(VLCollector collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return OpenCollector(collector.CollectorId);
        }
        /// <summary>
        /// Ανοίγει ένα Collector, για συλλογή απαντήσεων στο ερωτηματολόγιο του.
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public VLCollector OpenCollector(Int32 collectorId)
        {
            /*διαβάζουμε τον collector απο το σύστημά μας:*/
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", collectorId));

            //Διαβάζουμε το survey απο το σύστημα
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                if (this.ClientId != survey.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion

            if (collector.Status == CollectorStatus.Open)
            {
                throw new VLException(string.Format("Collector {0}, is already Open", collector.Name));
            }


            var clientProfile = SystemDal.GetClientProfileForClient(this.AccessTokenId, survey.Client);
            if (clientProfile == null) throw new VLException(string.Format("Invalid profile for client with id {0}", survey.Client));
            Boolean _useCredits = clientProfile.UseCredits;
            if(collector.CollectorType == CollectorType.WebLink && collector.CreditType.HasValue)
            {
                /*Για να ανοίξει ένας WebLink Collector, πρέπει να υπάρχουν συνδεδεμένα payments του σωστού τύπου:*/
                var collectorPayments = SystemDal.GetCollectorPaymentsForCollector(this.AccessTokenId, collector.CollectorId);
                if (collectorPayments.Count <= 0)
                {
                    throw new VLPaymentException("You cannot open this Collector! There are no payments associated with the collector!");
                }

                var totalUnits = CountAvailableCreditsForCollectorPayments(collectorPayments, collector.CreditType.Value, "You cannot open this Collector!");

                /*στην περίπτωση των Clicks, δεν απαιτούμε κάποιο συγκεκριμένο ποσό, απλώς να έχουμε κάποια θετική τιμή*/
                if (totalUnits < 1)
                {
                    throw new VLInvalidPaymentException("You cannot open this Collector!. Payment has no available credits!");
                }
            }

            collector.Status = CollectorStatus.Open;
            collector.ClosedByUser = false;
            return SurveysDal.UpdateCollector(this.AccessTokenId, collector);
        }
        #endregion

        #region VLMessage
        public Collection<VLMessage> GetMessages(VLCollector collector, string whereClause = null, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetMessages(collector.CollectorId, whereClause, orderByClause);
        }
        public Collection<VLMessage> GetMessages(Int32 collectorId,  string whereClause = null, string orderByClause = null)
        {

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                var client = SystemDal.GetClientForCollector(this.AccessTokenId, collectorId);
                if (client == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", collectorId));
                }

                if (this.ClientId != client.ClientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumerateCollectors);
            }
            #endregion


            return SurveysDal.GetMessages(this.AccessTokenId, collectorId, whereClause, orderByClause);
        }
        public Collection<VLMessage> GetMessages(VLCollector collector, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetMessages(collector.CollectorId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        public Collection<VLMessage> GetMessages(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                var client = SystemDal.GetClientForCollector(this.AccessTokenId, collectorId);
                if (client == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", collectorId));
                }

                if (this.ClientId != client.ClientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumerateCollectors);
            }
            #endregion


            return SurveysDal.GetMessages(this.AccessTokenId, collectorId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        public Int32 GetMessagesCount(Int32 collectorId, string whereClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetMessagesCount(this.AccessTokenId, collectorId, whereClause);
        }

        public Collection<VLMessage> GetDraftMessages(VLCollector collector, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetMessages(collector.CollectorId, pageIndex, pageSize, ref totalRows, "where [Status] = /*MessageStatus.Draft*/0", orderByClause);
        }
        public Collection<VLMessage> GetDraftMessages(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            return GetMessages(collectorId, pageIndex, pageSize, ref totalRows, "where [Status] = /*MessageStatus.Draft*/0", orderByClause);
        }
        public Int32 GetDraftMessagesCount(Int32 collectorId)
        {

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                var client = SystemDal.GetClientForCollector(this.AccessTokenId, collectorId);
                if (client == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", collectorId));
                }

                if (this.ClientId != client.ClientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumerateCollectors);
            }
            #endregion

            return SurveysDal.GetMessagesCount(this.AccessTokenId, collectorId, "where [Status] = /*MessageStatus.Draft*/0");
        }

        public Collection<VLMessage> GetNonDraftMessages(VLCollector collector, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetMessages(collector.CollectorId, pageIndex, pageSize, ref totalRows, "where [Status] != /*MessageStatus.Draft*/0", orderByClause);
        }
        public Collection<VLMessage> GetNonDraftMessages(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            return GetMessages(collectorId, pageIndex, pageSize, ref totalRows, "where [Status] != /*MessageStatus.Draft*/0", orderByClause);
        }
        public Collection<VLMessage> GetNonDraftMessages(Int32 collectorId, string orderByClause = null)
        {
            return GetMessages(collectorId, "where [Status] != /*MessageStatus.Draft*/0", orderByClause);
        }
        public Int32 GetNonDraftMessagesCount(Int32 collectorId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                var client = SystemDal.GetClientForCollector(this.AccessTokenId, collectorId);
                if (client == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", collectorId));
                }

                if (this.ClientId != client.ClientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumerateCollectors);
            }
            #endregion

            return SurveysDal.GetMessagesCount(this.AccessTokenId, collectorId, "where [Status] != /*MessageStatus.Draft*/0");
        }


        /// <summary>
        /// Επιστρέφει το πλήθος των μηνυμάτων που περιμένουν να σταλθούν ή στέλνονται την στιγμή της ερώτησης
        /// <para>Επιστρέφει το πλήθος των μηνυμάτων που βρίσκονται σε status: Pending, Preparing, Prepared και Executing</para>
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public Int32 GetScheduledMessagesCount(Int32 collectorId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetMessagesCount(this.AccessTokenId, collectorId, "where [Status] in (/*Pending*/1, /*Preparing*/2,/*Prepared*/4,/*Executing*/5)");
        }


        public VLMessage GetMessageBySenderVerificationCode(string code)
        {
            var message = SurveysDal.GetMessageBySenderVerificationCode(this.AccessTokenId, code);
            if (message == null)
            {
                return null;
            }

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                var client = SystemDal.GetClientForCollector(this.AccessTokenId, message.Collector);
                if (client == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", message.Collector));
                }

                if (this.ClientId != client.ClientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumerateCollectors, VLPermissions.ClientEnumerateSurveys);
            }
            #endregion

            return message;
        }
        public VLMessage GetMessageById(Int32 messageId)
        {
            var message = SurveysDal.GetMessageById(this.AccessTokenId, messageId);
            if (message == null)
            {
                return null;
            }

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                var client = SystemDal.GetClientForCollector(this.AccessTokenId, message.Collector);
                if (client == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", message.Collector));
                }

                if (this.ClientId != client.ClientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumerateCollectors, VLPermissions.ClientEnumerateSurveys);
            }
            #endregion

            return message;
        }




        /// <summary>
        /// Δημιουργεί ένα Draft message για τον συγκεκριμένο Collector, με default values που αντιγράφονται απο το αντίστοιχο email-template.
        /// <para>Σε αυτή την φάση το σύστημα ελέγχει και την Sender Address. Εάν είναι γνωστή και verifies θέτει IsSenderOK να φέρει την τιμή true!</para>
        /// </summary>
        /// <param name="collector"></param>
        /// <param name="sender"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public VLMessage CreateMessage(VLCollector collector, string sender = null, string subject = null, string body = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return CreateMessage(collector.CollectorId, sender, subject, body);
        }
        /// <summary>
        /// Δημιουργεί ένα Draft message για τον συγκεκριμένο Collector, με default values που αντιγράφονται απο το αντίστοιχο email-template.
        /// <para>Σε αυτή την φάση το σύστημα ελέγχει και την Sender Address. Εάν είναι γνωστή και verifies θέτει IsSenderOK να φέρει την τιμή true!</para>
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="sender"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public VLMessage CreateMessage(Int32 collectorId, string sender = null, string subject = null, string body = null)
        {
            //Θέλουμε να διαβάσουμε απο το σύστημα το emailTemplate 'SurveyInvitation':
            var template = SystemDal.GetEmailTemplateByName(this.AccessTokenId, "SurveyInvitation");
            if (template == null)
            {
                throw new VLException("EmailTemplate 'SurveyInvitation' does not exist!");
            }

            VLMessage message = new VLMessage();
            message.Collector = collectorId;
            message.Status = MessageStatus.Draft;

            if(!string.IsNullOrWhiteSpace(sender))
                message.Sender = sender;
            else
                message.Sender = this.Email;

            if (!string.IsNullOrWhiteSpace(subject))
                message.Subject = subject;
            else
                message.Subject = template.Subject;

            if (!string.IsNullOrWhiteSpace(body))
                message.Body = body;
            else
                message.Body = template.Body;

            message.DeliveryMethod = DeliveryMethod.All;

            return CreateMessageImpl(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        VLMessage CreateMessageImpl(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            message.ValidateInstance();

            //διαβάζουμε τον collector απο την βάση μας:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, message.Collector, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", message.Collector));

            //διαβάζουμε και το survey απο την βάση μας:
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != survey.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion

            //Ελέγχουμε εάν το μήνυμα είναι σωστά φτιαγμένο:
            ValidateMessage(message);

            /*
             * Σε αυτό το σημείο θα ασχοληθούμε με το Sender Address 
             */
            message.IsSenderOK = false;
            var knownEmail = SystemDal.GetKnownEmailByAddress(this.AccessTokenId, survey.Client, message.Sender);
            if (knownEmail != null && knownEmail.IsVerified)
            {
                message.IsSenderOK = true;
            }
            if(message.IsSenderOK == false)
            {
                //To συγκεκριμένο account που έχει κάνει login, πρέπει να επαληθεύσει ότι το sender address είναι valid.
                message.SenderVerificationCode = Utility.GenerateSenderVerificationCode(12);                
            }


            return SurveysDal.CreateMessage(this.AccessTokenId, message);
        }

        /// <summary>
        /// Ο χρήστης μπορεί και μεταβάλλει/update ένα message, όσο αυτό είναι σε Draf status.
        /// <para>Η αλλαγή του Sender email address, αναγκάζει το σύστημα να ξανακάνει verify το νέο Sender email address!</para>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public VLMessage UpdateMessage(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            message.ValidateInstance();

            var existingMessage = SurveysDal.GetMessageById(this.AccessTokenId, message.MessageId);
            if (existingMessage == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "message", message.MessageId));


            //διαβάζουμε τον collector απο την βάση μας:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, message.Collector, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", message.Collector));

            //διαβάζουμε και το survey απο την βάση μας:
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));



            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != survey.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion

            
            //εάν το message δεν είναι σε Draft status τότε δεν επιτρέουμε κάποιου είδους χειρισμό απο τον χρήστη μας:
            if (existingMessage.Status != MessageStatus.Draft)
            {
                var _msg = string.Format("You cannot update a Message in {0} status!", existingMessage.Status);
                throw new VLException(_msg);
            }


            ValidateMessage(message);

            /*
             * Σε αυτό το σημείο θα ασχοληθούμε με το Sender Address, εάν αυτή άλλαξε:
             */
            if(string.Equals(existingMessage.Sender, message.Sender, StringComparison.OrdinalIgnoreCase) == false)
            {
                message.IsSenderOK = false;
                var knownEmail = SystemDal.GetKnownEmailByAddress(this.AccessTokenId, survey.Client, message.Sender);
                if (knownEmail != null && knownEmail.IsVerified)
                {
                    message.IsSenderOK = true;
                }
                if (message.IsSenderOK == false)
                {
                    //To συγκεκριμένο account που έχει κάνει login, πρέπει να επαληθεύσει ότι το sender address είναι valid.
                    message.SenderVerificationCode = Utility.GenerateSenderVerificationCode(12);
                }
            }


            existingMessage.Sender = message.Sender;
            existingMessage.IsSenderOK = message.IsSenderOK;
            existingMessage.SenderVerificationCode = message.SenderVerificationCode;

            existingMessage.Subject = message.Subject;
            existingMessage.Body = message.Body;
            //Status
            //AttributeFlags
            existingMessage.IsDeliveryMethodOK = message.IsDeliveryMethodOK;
            existingMessage.IsContentOK = message.IsContentOK;
            //existingMessage.IsScheduleOK = message.IsScheduleOK;
            //existingMessage.SkipPaymentValidations = message.SkipPaymentValidations;
            //existingMessage.ScheduledAt = message.ScheduledAt;
            //existingMessage.SentCounter = message.SentCounter;
            existingMessage.DeliveryMethod = message.DeliveryMethod;
            existingMessage.CustomSearchField = message.CustomSearchField;
            existingMessage.CustomOperator = message.CustomOperator;
            existingMessage.CustomKeyword = message.CustomKeyword;
            //PendingAt
            //PreparingAt
            //PreparedAt
            //ExecutingAt
            //TerminatedAt

            return SurveysDal.UpdateMessage(this.AccessTokenId, existingMessage);
        }
        /// <summary>
        /// Σε αυτό που διαφέρει απο την απλή UpdateMessage, είναι ότι κάνει update to flag SkipPaymentValidations του message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal VLMessage UpdateMessageIntl(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            message.ValidateInstance();

            var existingMessage = SurveysDal.GetMessageById(this.AccessTokenId, message.MessageId);
            if (existingMessage == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "message", message.MessageId));


            //διαβάζουμε τον collector απο την βάση μας:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, message.Collector, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", message.Collector));

            //διαβάζουμε και το survey απο την βάση μας:
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));



            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != survey.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion


            //εάν το message δεν είναι σε Draft status τότε δεν επιτρέουμε κάποιου είδους χειρισμό απο τον χρήστη μας:
            if (existingMessage.Status != MessageStatus.Draft)
            {
                var _msg = string.Format("You cannot update a Message in {0} status!", existingMessage.Status);
                throw new VLException(_msg);
            }


            ValidateMessage(message);


            existingMessage.Sender = message.Sender;
            existingMessage.Subject = message.Subject;
            existingMessage.Body = message.Body;
            //Status
            //AttributeFlags
            existingMessage.IsDeliveryMethodOK = message.IsDeliveryMethodOK;
            existingMessage.IsSenderOK = message.IsSenderOK;
            existingMessage.IsContentOK = message.IsContentOK;
            //existingMessage.IsScheduleOK = message.IsScheduleOK;
            existingMessage.SkipPaymentValidations = message.SkipPaymentValidations;
            //existingMessage.ScheduledAt = message.ScheduledAt;
            //existingMessage.SentCounter = message.SentCounter;
            existingMessage.DeliveryMethod = message.DeliveryMethod;
            existingMessage.CustomSearchField = message.CustomSearchField;
            existingMessage.CustomOperator = message.CustomOperator;
            existingMessage.CustomKeyword = message.CustomKeyword;
            //PendingAt
            //PreparingAt
            //PreparedAt
            //ExecutingAt
            //TerminatedAt

            return SurveysDal.UpdateMessage(this.AccessTokenId, existingMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void ValidateMessage(VLMessage message)
        {
            /*
             * Θα ελέγξουμε το sender. 
             * 
             * Πρέπει να είναι μία valid email address
             */
            if (string.IsNullOrWhiteSpace(message.Sender) == false)
            {
                if (Utility.EmailIsValid(message.Sender) == false)
                {
                    throw new VLException("Sender's email address is invalid!");
                }
                MailAddress mailAddress = new MailAddress(message.Sender);
                var _LocalPart = mailAddress.User;
                var _DomainPart = mailAddress.Host;

                if (!Utility.IsValidDomainName(_DomainPart))
                {
                    throw new VLException("Sender's email domain is invalid!");
                }
            }

            /*
             * Θα ελέγξουμε το body.
             * 
             * Πρέπει να έχει οπωσδήποτε ακριβώς μία φορά το [@SurveyLink] και μία φορά ακριβώς το [@RemoveLink].
             * Μπορεί να φέρει και τα εξής δυναμικά πεδία: [@Title], [@FirstName], [@LastName], [@CustomValue] 
             * 
             */
            if (string.IsNullOrWhiteSpace(message.Body) == false)
            {
                if (message.Body.Contains("[SurveyLink]") == false)
                {
                    throw new VLException("Malformed Body. Tag [SurveyLink] does not exist!");
                }

                if (message.Body.Contains("[RemoveLink]") == false)
                {
                    throw new VLException("Malformed Body. Tag [RemoveLink] does not exist!");
                }
            }

            //Ελέγχουμε την DeliveryMethod:
            if (message.DeliveryMethod == DeliveryMethod.Custom)
            {
                //CustomSearchField
                if (message.CustomSearchField.HasValue == false)
                {
                    throw new VLException("CustomSearchField is not defined!");
                }
                //CustomOperator
                if (message.CustomOperator.HasValue == false)
                {
                    throw new VLException("CustomOperator is not defined!");
                }
                //CustomKeyword
                if (string.IsNullOrWhiteSpace(message.CustomKeyword))
                {
                    throw new VLException("CustomKeyword is not defined!");
                }
            }


            //Ελέγχουμε το ScheduleAt
            if (message.IsScheduleOK)
            {
                if(message.ScheduledAt.HasValue == false)
                {
                    throw new VLException("Schedule is not defined!");
                }
            }
        }



        /// <summary>
        /// Χρονοπρογραμματίζει το μήνυμα για αποστολή, και ταυτόχρονα το προωθεί σε status Pending
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="scheduleDt">Η ημέρα και ώρα κατα την οποία θα αρχίσει η αποστολή του μηνύματος ατους παραλήπτες του. Αυτό το Datetime πρέπει να βρίσκεται στην τοπική ώρα του χρήστη. (Tο σύστημα με βάση το δηλωθέν time zone του χρήστη, θα την μετατρέψει σε UTC).</param>
        /// <returns></returns>
        public VLMessage ScheduleMessage(Int32 messageId, DateTime scheduleDt)
        {
            //Βρίσκουμε το message απο το σύστημά μας:
            var message = SurveysDal.GetMessageById(this.AccessTokenId, messageId);
            if (message == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "message", messageId));

            return ScheduleMessage(message, scheduleDt);
        }
        /// <summary>
        /// Χρονοπρογραμματίζει το μήνυμα για αποστολή, και ταυτόχρονα το προωθεί σε status Pending
        /// <para>O χρονοπρογραμματισμός είναι το τελευταίο βήμα της δημιουργίας ενός μηνύματος για αποστολή. Πρέπει να έχει προηγηθεί η επιλογή
        /// των recipients, η δημιουργία του κειμένου του μηνύματος και η επαλήθευση του Sender του μηνύματος.</para>
        /// <para>Επίσης εάν την στιγμή του χρονοπρογραμματισμού δεν υπάρχουν παραλήπτες τότε, το σύστημα δεν επιτρέπει τον χρονοπρογραμματισμό.</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="scheduleDt">Η ημέρα και ώρα κατα την οποία θα αρχίσει η αποστολή του μηνύματος ατους παραλήπτες του. 
        /// Αυτό το Datetime πρέπει να βρίσκεται στην τοπική ώρα του χρήστη. (Tο σύστημα με βάση το δηλωθέν time zone του χρήστη, θα την μετατρέψει σε UTC).</param>
        /// <param name="promoteMessageToPendingStatus"></param>
        /// <param name="validatePayment"></param>
        /// <param name="scheduleDtOffset"></param>
        /// <returns></returns>
        internal VLMessage ScheduleMessage(VLMessage message, DateTime scheduleDt, bool promoteMessageToPendingStatus = true, bool validatePayment = true, int scheduleDtOffset = 5)
        {
            if (message == null) throw new ArgumentNullException("message");

            //Πρέπει το scheduleDt να βρίσκεται ττο μέλλον
            var scheduleDtUtc = this.ConvertTimeToUtc(scheduleDt);
            if (scheduleDtOffset != -1)
            {
                if (scheduleDtUtc < Utility.UtcNow().AddMinutes(scheduleDtOffset))
                {
                    throw new VLException("Please select a day and time that is in the future.");
                }
            }

            return ScheduleMessageImpl(message, scheduleDtUtc, promoteMessageToPendingStatus, validatePayment);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public VLMessage ScheduleMessageImmediately(Int32 messageId)
        {
            //Βρίσκουμε το message απο το σύστημά μας:
            var message = SurveysDal.GetMessageById(this.AccessTokenId, messageId);
            if (message == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "message", messageId));

            return ScheduleMessageImmediately(message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="promoteMessageToPendingStatus"></param>
        /// <param name="validatePayment"></param>
        /// <returns></returns>
        internal VLMessage ScheduleMessageImmediately(VLMessage message, bool promoteMessageToPendingStatus = true, bool validatePayment = true)
        {
            if (message == null) throw new ArgumentNullException("message");

            //Ο Χρονοπρογραμματισμός γίνεται αμέσως:
            var scheduleDtUtc = Utility.UtcNow();

            return ScheduleMessageImpl(message, scheduleDtUtc, promoteMessageToPendingStatus, validatePayment);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="scheduleDtUtc"></param>
        /// <param name="promoteMessageToPendingStatus"></param>
        /// <param name="validatePayment"></param>
        /// <returns></returns>
        VLMessage ScheduleMessageImpl(VLMessage message, DateTime scheduleDtUtc, bool promoteMessageToPendingStatus = true, bool validatePayment = true)
        {
            //διαβάζουμε τον collector απο την βάση μας:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, message.Collector, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", message.Collector));

            //διαβάζουμε και το survey απο την βάση μας:
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));

            //διαβάζουμε τον πελάτη μας:
            var client = SystemDal.GetClientById(this.AccessTokenId, survey.Client);
            if (client == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "client", survey.Client));



            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != survey.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion


            //εάν ο collector είναι κλειστός, τότε δεν μπορούμε να προσθέσουμε ή να αφαιρέσουμε κάτι!
            //if (collector.Status == CollectorStatus.Close)
            //{
            //    throw new VLException(string.Format("Collector {0} is closed. You cannot add any messages to it!", collector.Name));
            //}

            //εάν το message δεν είναι σε Draft status τότε δεν επιτρέπουμε κάποιου είδους χειρισμό απο τον χρήστη μας:
            if(message.IsScheduleOK == true)
            {
                throw new VLException("The message is already Scheduled!");
            }
            if (message.Status != MessageStatus.Draft)
            {
                var _msg = string.Format("You cannot Schedule this Message. It's status is {0}!", message.Status);
                throw new VLException(_msg);
            }

            //Για να χρονοπρογραμματίσουμε ένα message, πρέπει να έχουμε recipients, content, payments και να έχει επαληθευτεί το email του sender:
            if (message.IsSenderOK == false)
            {
                //throw new VLException("You cannot schedule this message! Sender email has not been verified!");
            }
            if (message.IsDeliveryMethodOK == false)
            {
                throw new VLException("You cannot schedule this message! Recipients are not set/valid!");
            }
            if (message.IsContentOK == false)
            {
                throw new VLException("You cannot schedule this message! Content is not set/valid!");
            }

            //Εχουμε ένα καλώς σχηματισμένο message?
            ValidateMessage(message);


            string error_message = null;
            if (IsPaymentValidforScheduling(collector, message, out error_message, validatePayment) == false)
            {
                throw new VLPaymentException(error_message);
            }

            try
            {
                //Καθαρίζουμε τυχόν προηγούμενα MessageRecipients και δημιουργoύμε νέα:
                PrepareMessageRecipients(message);

                /*Χρονοπρογραμματισμός*/
                message.ScheduledAt = scheduleDtUtc;
                message.IsScheduleOK = true;
                //Αλλάζουμε το status του μηνύματος σε Pending:
                message.Status = MessageStatus.Pending;
                message.PendingAt = Utility.UtcNow();
                //Κάνουμε update το message
                message = SurveysDal.UpdateMessage(this.AccessTokenId, message);

                if (collector.Status != CollectorStatus.Open)
                {
                    collector.Status = CollectorStatus.Open;
                    collector = SurveysDal.UpdateCollector(this.AccessTokenId, collector);
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            //Κάνουμε update το message και επιστρέφουμε
            return message;
        }
        /// <summary>
        /// Χρησιμοποιείται κατα την διαδικασία του χρονοπρογραμματισμού ενός message για να βεβαιωθεί το σύστημα οτι υπάρχουν αρκετά
        /// συνδεδεμένα credits με τον Collector του μηνύματος, έτσι ώστε να μπορεί το μήνυμα να σταλεί με επιτυχία
        /// </summary>
        /// <param name="collector"></param>
        /// <param name="message"></param>
        /// <param name="error_message"></param>
        /// <param name="validatePayment"></param>
        /// <returns></returns>
        internal bool IsPaymentValidforScheduling(VLCollector collector, VLMessage message, out string error_message, bool validatePayment = true)
        {
            error_message = string.Empty;

            //Εχουμε recipients:
            var recipients = CompileRecipientsCountForMessage(message);
            if (recipients <= 0)
            {
                error_message = "You cannot schedule this message! There are no recipients!";
                return false;
            }
            
            //Η λειτουργία του collector, χρεώνεται?
            if (collector.UseCredits == false || collector.CreditType.HasValue == false)
            {
                return true;
            }

            //Χρεώνεται ο Πελάτης:
            var clientProfile = SystemDal.GetClientProfileById(this.AccessTokenId, collector.Profile.Value);
            if (clientProfile == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Profile", collector.Profile.Value));
            if (clientProfile.UseCredits == true)
            {
                //Εχουμε συνδεδεμένες πληρωμές με τον Collector:
                var collectorPayments = SystemDal.GetCollectorPaymentsForCollector(this.AccessTokenId, collector.CollectorId);
                if (collectorPayments.Count <= 0)
                {
                    error_message = "You cannot schedule this message! There are no payments associated with the collector!";
                    return false;
                }

                if (validatePayment && message.SkipPaymentValidations == false)
                {
                    #region validate if there is enough credit for all emails to go!
                    var totalUnits = 0;
                    foreach (var cp in collectorPayments)
                    {
                        var payment = SystemDal.GetPaymentById(this.AccessTokenId, cp.Payment);
                        if (payment == null)
                        {
                            error_message = SR.GetString(SR.There_is_no_item_with_id, "Payment", cp.Payment);
                            return false;
                        }

                        if (payment.CreditType != collector.CreditType)
                        {
                            error_message = string.Format("You cannot schedule this message! Payment type is '{0}', but this Collector needs payments of type '{1}'.", payment.CreditType, collector.CreditType);
                            return false;
                        }

                        var availableUnits = payment.Quantity - payment.QuantityUsed;
                        if (cp.QuantityLimit.HasValue == false)
                        {
                            totalUnits += availableUnits;
                        }
                        else
                        {
                            if (cp.QuantityLimit >= availableUnits)
                            {
                                totalUnits += availableUnits;
                            }
                            else
                            {
                                totalUnits += cp.QuantityLimit.Value;
                            }
                        }
                    }
                    if (collector.CreditType == CreditType.EmailType)
                    {
                        /*Στην περίπτωση των emails, πρέπει να καλύπτεται το πλήθος αποστολής τους:*/
                        if (totalUnits < recipients)
                        {
                            error_message = string.Format("You cannot schedule this message!. There are enough payments to send {0} emails. But you want to send email to {1} resipients!", totalUnits, recipients);
                            return false;
                        }
                    }
                    else if (collector.CreditType == CreditType.ResponseType)
                    {
                        /*στην περίπτωση των Responses, δεν απαιτούμε κάποιο συγκεκριμένο ποσό, απλώς να έχουμε κάποια θετική τιμή*/
                        if (totalUnits < 1)
                        {
                            error_message = "You cannot schedule this message!. There must be positive balance in your payments.";
                            return false;
                        }
                    }
                    #endregion
                }
            }


            return true;
        }


        /// <summary>
        /// Ακυρώνουμε την προγραμματισμένη αποστολή ενός Pending μηνύματος.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public VLMessage UnScheduleMessage(Int32 messageId)
        {
            //Βρίσκουμε το message απο το σύστημά μας:
            var message = SurveysDal.GetMessageById(this.AccessTokenId, messageId);
            if (message == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "message", messageId));

            return UnScheduleMessage(message);
        }
        /// <summary>
        /// Ακυρώνουμε την προγραμματισμένη αποστολή ενός Pending μηνύματος.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public VLMessage UnScheduleMessage(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            /*εάν το μήνυμα δεν είναι χρονοπρογραμματισμένο, δεν συνεχίζουμε:*/
            if (message.IsScheduleOK == false)
            {
                throw new VLException("The message is not Scheduled!");
            }

            //διαβάζουμε τον collector απο την βάση μας:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, message.Collector, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", message.Collector));

            //διαβάζουμε και το survey απο την βάση μας:
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != survey.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion



            /* Μπορούμε να διαγράψουμε, μόνο όταν ένα μήνυμα δεν έχει δημιουργήσει χρεώσεις, και όταν βρίσκεται
             * σε συγκεκριμενο τερματικό status:
             */
            if(message.SentCounter > 0 )
            {
                throw new VLException("You cannot unschedule a Message which has been used to send emails!");
            }
            if (
                message.Status != MessageStatus.Pending && 
                message.Status != MessageStatus.PreparingError && 
                message.Status != MessageStatus.ExecutingError && 
                message.Status != MessageStatus.ExecutedWithErrors &&
                message.Status != MessageStatus.Cancel
                )
            {
                var _msg = string.Format("You cannot unschedule a Message in {0} status!", message.Status);
                throw new VLException(_msg);
            }


            try
            {
                /*τώρα τρέχουμε να γυρίσουμε τo status του μηνύματος απο Pending σε Draft:*/
                message = SurveysDal.UnScheduleMessage(this.AccessTokenId, message);
                if(message.Status != MessageStatus.Draft)
                {
                    //Δεν προλάβαμε να το ακυρώσουμε, το σύστημα ξεκίνησε την αποστολή!
                    var _msg = string.Format("You cannot Unschedule this Message. It's status is {0}!", message.Status);
                    throw new VLException(_msg);
                }

                /*Τώρα θέλουμε να διαγράψουμε τα messageRecipients, και να επιστρέψουν στην προτέρα κατάσταση οι paymentCollectors:*/
                SurveysDal.UnPrepareMessageRecipients(this.AccessTokenId, message.Collector, message.MessageId);


                return SurveysDal.UpdateMessage(this.AccessTokenId, message);
            }
            catch (Exception ex)
            {
                /*εσκασε, πρφανώς λόγω concurency control!*/
                var _msg = string.Format("Failed to cancel message. Error = {0}", ex.Message);
                throw new VLException(_msg);
            }
        }




        /// <summary>
        /// Ελέγχει ένα υπάρχουν credits στους συνδεδεμένουν payments για την επιτυχή αποστολή των emails, για αυτό το message!
        /// <para>Αυτός ο έλεγχος έχει νόημα, μόνο για messages που ανήκουν σε collectors που χρεώνονται με την αποστολή emails</para>
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public bool ValidatePaymentForMessage(Int16 messageId, bool throwException)
        {
            //Βρίσκουμε το message απο το σύστημά μας:
            var message = SurveysDal.GetMessageById(this.AccessTokenId, messageId);
            if (message == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "message", messageId));

            return ValidatePaymentForMessage(message, throwException);
        }
        /// <summary>
        /// Ελέγχει ένα υπάρχουν credits στους συνδεδεμένουν payments για την επιτυχή αποστολή των emails, για αυτό το message!
        /// <para>Αυτός ο έλεγχος έχει νόημα, μόνο για messages που ανήκουν σε collectors που χρεώνονται με την αποστολή emails</para>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public bool ValidatePaymentForMessage(VLMessage message, bool throwException)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (message.Status != MessageStatus.Pending && message.Status != MessageStatus.Preparing && message.Status != MessageStatus.Prepared)
            {
                var _msg = string.Format("You cannot validate the payment for message. It's status is {0}!", message.Status);
                throw new VLException(_msg);
            }
            if(message.SkipPaymentValidations == true)
            {
                return true;
            }

            //διαβάζουμε τον collector απο την βάση μας:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, message.Collector, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", message.Collector));

            //Η λειτουργία του collector, χρεώνεται?
            if (collector.UseCredits == false || collector.CreditType.HasValue == false)
            {
                return true;
            }
            //Εάν ο collector, δεν χρεώνεται με emails, τότε επιστρέφουμε:
            if(collector.CreditType.Value != CreditType.EmailType)
            {
                return true;
            }

            //διαβάζουμε τον πελάτη μας:
            var client = SystemDal.GetClientForCollector(this.AccessTokenId, message.Collector);
            if (client == null) throw new VLException(string.Format("There is no client for message with id = {0}", message.MessageId));

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != client.ClientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion


            //Χρεώνεται ο Πελάτης:
            var clientProfile = SystemDal.GetClientProfileById(this.AccessTokenId, client.Profile);
            if (clientProfile == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Profile", client.Profile));
            if (clientProfile.UseCredits == true)
            {
                var messageRecipients = SurveysDal.GetMessageRecipients(this.AccessTokenId, message.MessageId, null);

                //βρίσκουμε τις πληρωμές που έχουν συνδεθεί με κάθε messageRecipient:
                Dictionary<Int32, Int32> cpayments = new Dictionary<int, int>();
                foreach(var mr in messageRecipients)
                {
                    if (mr.CollectorPayment.HasValue == false)
                        continue;

                    if (!cpayments.ContainsKey(mr.CollectorPayment.Value))
                        cpayments[mr.CollectorPayment.Value] = 0;
                    cpayments[mr.CollectorPayment.Value]++;
                }

                foreach(KeyValuePair<Int32, Int32> pair in cpayments)
                {
                    var collectorPayment = SystemDal.GetCollectorPaymentById(this.AccessTokenId, pair.Key);
                    if (collectorPayment == null)
                    {
                        if (throwException)
                            throw new VLPaymentException(SR.GetString(SR.There_is_no_item_with_id,"CollectorPayment", pair.Key));
                        return false;
                    }

                    var payment = SystemDal.GetPaymentById(this.AccessTokenId, collectorPayment.Payment);
                    if (payment == null)
                    {
                        if (throwException)
                            throw new VLPaymentException(SR.GetString(SR.There_is_no_item_with_id, "Payment", collectorPayment.Payment));
                        return false;
                    }


                    if ((payment.Quantity - payment.QuantityUsed) < pair.Value)
                    {
                        if (throwException)
                            throw new VLPaymentException(string.Format("Payment wih id {0} is not valid. Quantity needed is {1}, but is available only {2}!", payment.PaymentId, pair.Value, (payment.Quantity - payment.QuantityUsed)));
                        return false;
                    }
                }
            }

            return true;
        }

        internal VLMessage PromoteMessageToPendingStatus(Int32 messageId)
        {
            var message = SurveysDal.GetMessageById(this.AccessTokenId, messageId);
            if (message == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "message", messageId));

            return PromoteMessageToPendingStatus(message);
        }
        internal VLMessage PromoteMessageToPendingStatus(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");


            if (message.Status != MessageStatus.Draft)
            {
                throw new VLException("Message has invalid status!");
            }

            //εχουμε ένα καλώς σχηματισμένο message?
            ValidateMessage(message);

            //εχουμε ScheduleDt:
            if(message.ScheduledAt == null || message.IsScheduleOK == false)
            {
                throw new VLException("There is no ScheduledAt!");
            }

            //εχουμε recipients:
            var recipients = CompileRecipientsCountForMessage(message);
            if (recipients <= 0)
            {
                throw new VLException("There are no recipients!");
            }
            //Καθαρίζουμε τυχόν προηγούμενα MessageRecipients και δημιουργoύμε νέα:
            PrepareMessageRecipients(message);

            //Κάνουμε update το message και επιστρέφουμε
            message.Status = MessageStatus.Pending;
            message.PendingAt = Utility.UtcNow();
            return SurveysDal.UpdateMessage(this.AccessTokenId, message);
        }


        public void DeleteMessage(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            DeleteMessage(message.Collector);
        }
        public void DeleteMessage(Int32 messageId)
        {
            var existingMessage = SurveysDal.GetMessageById(this.AccessTokenId, messageId);
            if (existingMessage == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "message", messageId));

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                var client = SystemDal.GetClientForCollector(this.AccessTokenId, existingMessage.Collector);
                if (client == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", existingMessage.Collector));
                }

                if (this.ClientId != client.ClientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion


            /*
             * Επιτρέπεται νε διαγράψουμε messages που δεν έχουν δημιουργήσει χρεώσεις στο σύστημα.
             * Εάν το μήνυμα είναι χρονοπρογραμματισμένο, τότε πρίν την διαγραφή προβαίνουμε σε κατάργηση
             * του χρονοπρογραμματισμού, διότι καθαρίζει τα messageRecipietns, και επιστρέφει τα Reserved credits
             * στα CollectorPayments:
             */
            if(existingMessage.IsScheduleOK)
            {
                if(existingMessage.SentCounter == 0)
                {
                    if(
                        existingMessage.Status == MessageStatus.Pending || 
                        existingMessage.Status == MessageStatus.PreparingError ||
                        existingMessage.Status == MessageStatus.ExecutingError ||
                        existingMessage.Status == MessageStatus.ExecutedWithErrors || 
                        existingMessage.Status == MessageStatus.Cancel
                        )
                    {
                        existingMessage = UnScheduleMessage(existingMessage);
                    }
                }
            }
            if(existingMessage.IsScheduleOK)
            {
                var _msg = string.Format("You cannot delete a message with status {0}!", existingMessage.Status);
                throw new VLException(_msg);
            }





            SurveysDal.DeleteMessage(this.AccessTokenId, existingMessage.MessageId, existingMessage.LastUpdateDT);
        }



        /// <summary>
        /// Επιστρέφει το επόμενο διαθέσιμο (προς εκτέλεση) χρονοπρογραμματισμένο Message.
        /// <para>Αλλάζει το status του μηνύματος απο Pending -> Preparing</para>
        /// <para>Αρχικοποιεί το status όλων των αντίστοιχων MessageRecipients σε Pending.</para>
        /// </summary>
        /// <param name="minuteOffset"></param>
        /// <returns></returns>
        internal VLMessage GetNextPendingMessage(Int32 minuteOffset = 2)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            /*Στην βάση, όλα τα αποθηκευμένα DateTimes είναι σε UTC:*/
            var scheduleDtUtc = DateTime.UtcNow;

            return SurveysDal.GetNextPendingMessage(this.AccessTokenId, scheduleDtUtc, minuteOffset);
        }
        /// <summary>
        /// Αλλάζει το status του συγκεκιρμένου μηνύματος απο Preparing -> Prepared.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal VLMessage PromoteMessageToPreparedStatus(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            var existingMessage = SurveysDal.GetMessageById(this.AccessTokenId, message.MessageId);
            if (existingMessage == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Message", message.MessageId));

            if (existingMessage.Status != MessageStatus.Preparing)
                throw new VLException("Cannot promote message to Prepared status. Invalid current status!");

            //Κάνουμε update το message και επιστρέφουμε
            existingMessage.Status = MessageStatus.Prepared;
            existingMessage.PreparedAt = Utility.UtcNow();
            return SurveysDal.UpdateMessage(this.AccessTokenId, existingMessage);
        }
        /// <summary>
        /// Επιστρέφει το επόμενο Prepared μήνυμα που είναι διαθέσιμο για αποστολή.
        /// <para>Αλλάζει το status του μηνύματος απο Prepared -> Executing</para>
        /// </summary>
        /// <returns></returns>
        internal VLMessage GetNextPreparedMessage()
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            /*Στην βάση, όλα τα αποθηκευμένα DateTimes είναι σε UTC:*/
            var scheduleDtUtc = DateTime.UtcNow;

            return SurveysDal.GetNextPreparedMessage(this.AccessTokenId, scheduleDtUtc);
        }
        internal VLMessage PromoteMessageToExecutingErrorStatus(VLMessage message, Exception exception)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (exception == null) throw new ArgumentNullException("exception");


            return PromoteMessageToExecutingErrorStatus(message, Utility.UnWindExceptionContent(exception));
        }
        internal VLMessage PromoteMessageToExecutingErrorStatus(VLMessage message, string errormessage)
        {
            if (message == null) throw new ArgumentNullException("message");

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            var existingMessage = SurveysDal.GetMessageById(this.AccessTokenId, message.MessageId);
            if (existingMessage == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Message", message.MessageId));

            if (existingMessage.Status != MessageStatus.Executing)
                throw new VLException("Cannot promote message to ExecutingError status. Invalid current status!");

            //Κάνουμε update το message και επιστρέφουμε
            existingMessage.Status = MessageStatus.ExecutingError;
            existingMessage.Error = errormessage;
            existingMessage.TerminatedAt = Utility.UtcNow();
            existingMessage.SentCounter = message.SentCounter;
            existingMessage.FailedCounter = message.FailedCounter;
            existingMessage.SkipCounter = message.SkipCounter;
            return SurveysDal.UpdateMessage(this.AccessTokenId, existingMessage);
        }

        internal VLMessage PromoteMessageToPreparingErrorStatus(VLMessage message, Exception exception)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (exception == null) throw new ArgumentNullException("exception");

            return PromoteMessageToPreparingErrorStatus(message, Utility.UnWindExceptionContent(exception));
        }
        internal VLMessage PromoteMessageToPreparingErrorStatus(VLMessage message, string errormessage)
        {
            if (message == null) throw new ArgumentNullException("message");

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            var existingMessage = SurveysDal.GetMessageById(this.AccessTokenId, message.MessageId);
            if (existingMessage == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Message", message.MessageId));

            if (existingMessage.Status != MessageStatus.Preparing && existingMessage.Status != MessageStatus.Prepared)
                throw new VLException("Cannot promote message to PreparingError status. Invalid current status!");

            //Κάνουμε update το message και επιστρέφουμε
            existingMessage.Status = MessageStatus.PreparingError;
            existingMessage.Error = errormessage;
            existingMessage.TerminatedAt = Utility.UtcNow();
            return SurveysDal.UpdateMessage(this.AccessTokenId, existingMessage);
        }
        internal VLMessage PromoteMessageToExecutedStatus(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            var existingMessage = SurveysDal.GetMessageById(this.AccessTokenId, message.MessageId);
            if (existingMessage == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Message", message.MessageId));

            if (existingMessage.Status != MessageStatus.Executing)
                throw new VLException("Cannot promote message to Executed status. Invalid current status!");

            //Κάνουμε update το message και επιστρέφουμε
            existingMessage.Status = MessageStatus.Executed;
            existingMessage.TerminatedAt = Utility.UtcNow();
            existingMessage.SentCounter = message.SentCounter;
            existingMessage.FailedCounter = message.FailedCounter;
            existingMessage.SkipCounter = message.SkipCounter;
            return SurveysDal.UpdateMessage(this.AccessTokenId, existingMessage);
        }
        internal VLMessage PromoteMessageToExecutedWithErrorsStatus(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            var existingMessage = SurveysDal.GetMessageById(this.AccessTokenId, message.MessageId);
            if (existingMessage == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Message", message.MessageId));

            if (existingMessage.Status != MessageStatus.Executing)
                throw new VLException("Cannot promote message to Executed status. Invalid current status!");

            //Κάνουμε update το message και επιστρέφουμε
            existingMessage.Status = MessageStatus.ExecutedWithErrors;
            existingMessage.TerminatedAt = Utility.UtcNow();
            existingMessage.SentCounter = message.SentCounter;
            existingMessage.FailedCounter = message.FailedCounter;
            existingMessage.SkipCounter = message.SkipCounter;
            return SurveysDal.UpdateMessage(this.AccessTokenId, existingMessage);
        }


        /// <summary>
        /// Στέλνει στην email address του Message.sender ένα email με έναν κωδικό, τον οποίο ο χρήστης πρέπει να 
        /// χρησιμοποιησει για να κάνει validation την Sender Address
        /// </summary>
        /// <param name="message"></param>
        public VLSystemEmail SendVerifySenderEmail(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (message.IsSenderOK == true)
            {
                throw new VLException("Message's Sender is OK! You cannot verify again sender!");
            }


            var client = SystemDal.GetClientForCollector(this.AccessTokenId, message.Collector);
            if(client == null)
            {
                throw new VLException("Invalid message");
            }

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != client.ClientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion

            //Θέλουμε να διαβάσουμε απο το σύστημα το emailTemplate 'VerifySender':
            var template = SystemDal.GetEmailTemplateByName(this.AccessTokenId, "VerifySender");
            if (template == null)
            {
                throw new VLException("EmailTemplate 'VerifySender' does not exist!");
            }

            //Θέλουμε να διαβάσουμε μερικά στοιχεία απο το SystemParameters:
            var noreplyEmail = SystemDal.GetSystemParameterByKey(this.AccessTokenId, "@NoreplyEmail");
            if (noreplyEmail == null) throw new VLException("SystemParameters: @NoreplyEmail undefined");
            var fromDisplayName = SystemDal.GetSystemParameterByKey(this.AccessTokenId, "@FromDisplayName");
            if (fromDisplayName == null) throw new VLException("SystemParameters: @FromDisplayName undefined");
            var brandName = SystemDal.GetSystemParameterByKey(this.AccessTokenId, "@BrandName");
            if (brandName == null) throw new VLException("SystemParameters: @BrandName undefined");
            var supportEmail = SystemDal.GetSystemParameterByKey(this.AccessTokenId, "@SupportEmail");
            if (supportEmail == null) throw new VLException("SystemParameters: @SupportEmail undefined");
            var surveyTeam = SystemDal.GetSystemParameterByKey(this.AccessTokenId, "@SurveyTeam");
            if (surveyTeam == null) throw new VLException("SystemParameters: @SurveyTeam undefined");
            var emailSignature = SystemDal.GetSystemParameterByKey(this.AccessTokenId, "@EmailSignature");
            if (emailSignature == null) throw new VLException("SystemParameters: @EmailSignature undefined");

            var subject = template.Subject.Replace("@BrandName", brandName.ParameterValue);
            var body = template.Body.Replace("@Sender", message.Sender);
            body = body.Replace("@useremail", this.Email);
            body = body.Replace("@NoreplyEmail", noreplyEmail.ParameterValue);
            body = body.Replace("@FromDisplayName", fromDisplayName.ParameterValue);
            body = body.Replace("@BrandName", brandName.ParameterValue);
            body = body.Replace("@SupportEmail", supportEmail.ParameterValue);
            body = body.Replace("@SurveyTeam", surveyTeam.ParameterValue);
            body = body.Replace("@EmailSignature", emailSignature.ParameterValue);

            body = body.Replace("@VerifyReplyToLink", Utility.GetVerifySenderURL(message));
            

            //Δημιουργούμε το email:
            var email = SystemDal.CreateSystemEmail(
                    this.AccessTokenId, 
                    "VerifySenderEmail", 
                    noreplyEmail.ParameterValue, 
                    fromDisplayName.ParameterValue, 
                    message.Sender, 
                    subject, 
                    body
            );


            return email;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="senderVerificationCode"></param>
        /// <returns></returns>
        public VLMessage VerifySenderEmail(string senderVerificationCode)
        {
            //Βρίσκουμε το message που αφορά αυτό το verificationCode:
            var message = SurveysDal.GetMessageBySenderVerificationCode(this.AccessTokenId, senderVerificationCode);
            if(message == null)
            {
                throw new VLException("Invalid verification code.");
            }
            //Πρέπει αυτό το message να ανήκει στον ίδιο πελάτη με τον χρήστη που έχει κάνει login:
            var client = SystemDal.GetClientForCollector(this.AccessTokenId, message.Collector);
            if (client == null)
            {
                throw new VLException("Invalid verification code.");
            }


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != client.ClientId)
                {
                    throw new VLAccessDeniedException("This email verification request is for a different account");
                }
                //Δεν ελέγχουμε για ιδιαίτερα δικαιώματα:
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors, VLPermissions.ClientEnumerateCollectors, VLPermissions.ClientEnumerateSurveys);
            }
            #endregion


            if (message.IsSenderOK == false)
            {
                var knownemail = SystemDal.GetKnownEmailByAddress(this.AccessTokenId, client.ClientId, message.Sender);
                if (knownemail == null)
                {
                    knownemail = new VLKnownEmail();
                    knownemail.Client = client.ClientId;
                    knownemail.EmailAddress = message.Sender;
                    knownemail.RegisterDt = Utility.UtcNow();
                    knownemail.IsDomainOK = true;
                    knownemail.IsVerified = true;
                    knownemail.VerifiedDt = Utility.UtcNow();
                    MailAddress mailAddress = new MailAddress(message.Sender);
                    knownemail.LocalPart = mailAddress.User;
                    knownemail.DomainPart = mailAddress.Host;

                    knownemail = SystemDal.CreateKnownEmail(this.AccessTokenId, knownemail);
                }
                else
                {
                    knownemail.IsDomainOK = true;
                    knownemail.IsVerified = true;
                    knownemail.VerifiedDt = Utility.UtcNow();

                    knownemail = SystemDal.UpdateKnownEmail(this.AccessTokenId, knownemail);
                }

                message.IsSenderOK = true;
                message = SurveysDal.UpdateMessage(this.AccessTokenId, message);
            }

            return message;
        }

        #endregion

        #region VLRecipient
        public Collection<VLRecipient> GetRecipients(VLCollector collector, string whereClause = null, string orderByClause = null)
        {
            if (collector == null) throw new VLException("collector");
            return GetRecipients(collector.CollectorId, whereClause, orderByClause);
        }
        public Collection<VLRecipient> GetRecipients(Int32 collectorId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, whereClause, orderByClause);
        }
        public Collection<VLRecipient> GetRecipients(VLCollector collector, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetRecipients(collector.CollectorId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        public Collection<VLRecipient> GetRecipients(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        /// <summary>
        /// Επιστρέφει το σύνολο των Recipients για τον επιλεγμένο collector
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public int GetRecipientsCount(Int32 collectorId, string whereClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientsCount(this.AccessTokenId, collectorId, whereClause);
        }


        public Collection<VLRecipient> GetOptedOutRecipients(VLCollector collector, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetOptedOutRecipients(collector.CollectorId, orderByClause);
        }
        public Collection<VLRecipient> GetOptedOutRecipients(Int32 collectorId, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, "where [AttributeFlags] & /*IsOptedOut*/2 = 2", orderByClause);
        }
        public Collection<VLRecipient> GetOptedOutRecipients(VLCollector collector, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetOptedOutRecipients(collector.CollectorId, pageIndex, pageSize, ref totalRows, orderByClause);
        }
        public Collection<VLRecipient> GetOptedOutRecipients(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, pageIndex, pageSize, ref totalRows, "where [AttributeFlags] & /*IsOptedOut*/2 = 2", orderByClause);
        }
        /// <summary>
        /// επιστρέφει το σύνολο των Recipients που δεν επιθυμούν να παραλάβουν ξανά emails
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public int GetOptedOutRecipientsCount(Int32 collectorId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientsCount(this.AccessTokenId, collectorId, "where [AttributeFlags] & /*IsOptedOut*/2 = 2");
        }

        public Collection<VLRecipient> GetBouncedRecipients(VLCollector collector, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetBouncedRecipients(collector.CollectorId, orderByClause);
        }
        public Collection<VLRecipient> GetBouncedRecipients(Int32 collectorId, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, "where [AttributeFlags] & /*IsBouncedEmail*/4 = 4", orderByClause);
        }
        public Collection<VLRecipient> GetBouncedRecipients(VLCollector collector, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetBouncedRecipients(collector.CollectorId, pageIndex, pageSize, ref totalRows, orderByClause);
        }
        public Collection<VLRecipient> GetBouncedRecipients(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, pageIndex, pageSize, ref totalRows, "where [AttributeFlags] & /*IsBouncedEmail*/4 = 4", orderByClause);
        }
        /// <summary>
        /// Επιστρέφει το πλήθος των Recipients των οποίων το email έχει κάποιο πρόβλημα (δεν υπάρχει)
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public int GetBouncedRecipientsCount(Int32 collectorId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientsCount(this.AccessTokenId, collectorId, "where [AttributeFlags] & /*IsBouncedEmail*/4 = 4");
        }

        public Collection<VLRecipient> GetEmailedRecipients(VLCollector collector, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetEmailedRecipients(collector.CollectorId, orderByClause);
        }
        public Collection<VLRecipient> GetEmailedRecipients(Int32 collectorId, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, "where [AttributeFlags] & /*IsSentEmail*/1 = 1", orderByClause);
        }
        public Collection<VLRecipient> GetEmailedRecipients(VLCollector collector, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetEmailedRecipients(collector.CollectorId, pageIndex, pageSize, ref totalRows, orderByClause);
        }
        public Collection<VLRecipient> GetEmailedRecipients(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, pageIndex, pageSize, ref totalRows, "where [AttributeFlags] & /*IsSentEmail*/1 = 1", orderByClause);
        }
        /// <summary>
        /// Επιστρέφει το πλήθος των recipients στους οποίους έχει σταλει τουλάχιστον μία φορά email.
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public int GetEmailedRecipientsCount(Int32 collectorId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientsCount(this.AccessTokenId, collectorId, "where [AttributeFlags] & /*IsSentEmail*/1 = 1");
        }


        public Collection<VLRecipient> GetNotEmailedRecipients(VLCollector collector, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetNotEmailedRecipients(collector.CollectorId, orderByClause);
        }
        public Collection<VLRecipient> GetNotEmailedRecipients(Int32 collectorId, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, "where [AttributeFlags] & /*IsSentEmail*/1 = 0", orderByClause);
        }
        public Collection<VLRecipient> GetNotEmailedRecipients(VLCollector collector, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetNotEmailedRecipients(collector.CollectorId, pageIndex, pageSize, ref totalRows, orderByClause);
        }
        public Collection<VLRecipient> GetNotEmailedRecipients(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, pageIndex, pageSize, ref totalRows, "where [AttributeFlags] & /*IsSentEmail*/1 = 0", orderByClause);
        }
        /// <summary>
        /// Επιστρέφει το πλήθος των recipients στους οποίους δεν έχει σταλει ούτε μία φορά email.
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public int GetNotEmailedRecipientsCount(Int32 collectorId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientsCount(this.AccessTokenId, collectorId, "where [AttributeFlags] & /*IsSentEmail*/1 = 0");
        }

        /// <summary>
        /// Επιστρέφει τους recipients που έχουν απαντήσει σε μία τουλάχιστον ερώτηση απο το Survey.
        /// <para>Περιλαμβάνει αυτούς που τελείωσαν το ερωτηματολόγιο (HasResponded), αλλά, και αυτούς που δεν το έχουν τελειώσει ακόμα (HasPartiallyResponded).</para>
        /// </summary>
        /// <param name="collector"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLRecipient> GetRespondedRecipients(VLCollector collector, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetRespondedRecipients(collector.CollectorId, orderByClause);
        }
        /// <summary>
        /// Επιστρέφει τους recipients που έχουν απαντήσει σε μία τουλάχιστον ερώτηση απο το Survey.
        /// <para>Περιλαμβάνει αυτούς που τελείωσαν το ερωτηματολόγιο (HasResponded), αλλά, και αυτούς που δεν το έχουν τελειώσει ακόμα (HasPartiallyResponded).</para>
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLRecipient> GetRespondedRecipients(Int32 collectorId, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, "where ([AttributeFlags] & /*HasResponded*/8 = 8) or ([AttributeFlags] & /*HasPartiallyResponded*/16 = 16)", orderByClause);
        }
        /// <summary>
        /// Επιστρέφει τους recipients που έχουν απαντήσει σε μία τουλάχιστον ερώτηση απο το Survey.
        /// <para>Περιλαμβάνει αυτούς που τελείωσαν το ερωτηματολόγιο (HasResponded), αλλά, και αυτούς που δεν το έχουν τελειώσει ακόμα (HasPartiallyResponded).</para>
        /// </summary>
        /// <param name="collector"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLRecipient> GetRespondedRecipients(VLCollector collector, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetRespondedRecipients(collector.CollectorId, pageIndex, pageSize, ref totalRows, orderByClause);
        }
        /// <summary>
        /// Επιστρέφει τους recipients που έχουν απαντήσει σε μία τουλάχιστον ερώτηση απο το Survey.
        /// <para>Περιλαμβάνει αυτούς που τελείωσαν το ερωτηματολόγιο (HasResponded), αλλά, και αυτούς που δεν το έχουν τελειώσει ακόμα (HasPartiallyResponded).</para>
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLRecipient> GetRespondedRecipients(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, pageIndex, pageSize, ref totalRows, "where ([AttributeFlags] & /*HasResponded*/8 = 8) or ([AttributeFlags] & /*HasPartiallyResponded*/16 = 16)", orderByClause);
        }
        /// <summary>
        /// Επιστρέφει το πλήθος των recipients οι οποίοι έχουν απαντήσει σε μία τουλάχιστον ερώτηση απο το Survey.
        /// <para>Περιλαμβάνει αυτούς που τελείωσαν το ερωτηματολόγιο (HasResponded), αλλά, και αυτούς που δεν το έχουν τελειώσει ακόμα (HasPartiallyResponded).</para>
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public int GetRespondedRecipientsCount(Int32 collectorId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientsCount(this.AccessTokenId, collectorId, "where ([AttributeFlags] & /*HasResponded*/8 = 8) or ([AttributeFlags] & /*HasPartiallyResponded*/16 = 16)");
        }

        public int GetPartiallyRespondeRecipientsCount(Int32 collectorId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientsCount(this.AccessTokenId, collectorId, "where ([AttributeFlags] & /*HasPartiallyResponded*/16 = 16) and ([AttributeFlags] & /*HasResponded*/8 = 0)");
        }
        public int GetFullRespondeRecipientsCount(Int32 collectorId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientsCount(this.AccessTokenId, collectorId, "where [AttributeFlags] & /*HasResponded*/8 = 8");
        }




        /// <summary>
        /// Επιστρέφει τους recipients που τους έχει σταλεί email αλλά, δεν έχουν ανταποκριθεί και δεν έχουν ξεκινήσει καθόλου το Survey.
        /// </summary>
        /// <param name="collector"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLRecipient> GetNotRespondedRecipients(VLCollector collector, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetNotRespondedRecipients(collector.CollectorId, orderByClause);
        }
        /// <summary>
        /// Επιστρέφει τους recipients που τους έχει σταλεί email αλλά, δεν έχουν ανταποκριθεί και δεν έχουν ξεκινήσει καθόλου το Survey.
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLRecipient> GetNotRespondedRecipients(Int32 collectorId, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, "where [AttributeFlags] & /*IsSentEmail*/1 = 1 and [AttributeFlags] & /*HasPartiallyResponded*/16 = 0", orderByClause);
        }
        /// <summary>
        /// Επιστρέφει τους recipients που τους έχει σταλεί email αλλά, δεν έχουν ανταποκριθεί και δεν έχουν ξεκινήσει καθόλου το Survey.
        /// </summary>
        /// <param name="collector"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLRecipient> GetNotRespondedRecipients(VLCollector collector, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return GetNotRespondedRecipients(collector.CollectorId, pageIndex, pageSize, ref totalRows, orderByClause);
        }
        /// <summary>
        /// Επιστρέφει τους recipients που τους έχει σταλεί email αλλά, δεν έχουν ανταποκριθεί και δεν έχουν ξεκινήσει καθόλου το Survey.
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLRecipient> GetNotRespondedRecipients(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipients(this.AccessTokenId, collectorId, pageIndex, pageSize, ref totalRows, "where [AttributeFlags] & /*IsSentEmail*/1 = 1 and [AttributeFlags] & /*HasPartiallyResponded*/16 = 0", orderByClause);
        }
        /// <summary>
        /// Επιστρέφει το πλήθος των recipients που τους έχει σταλεί email αλλά, δεν έχουν ανταποκριθεί και δεν έχουν ξεκινήσει καθόλου το Survey.
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public int GetNotRespondedRecipientsCount(Int32 collectorId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientsCount(this.AccessTokenId, collectorId, "where [AttributeFlags] & /*IsSentEmail*/1 = 1 and [AttributeFlags] & /*HasPartiallyResponded*/16 = 0");
        }


        public Collection<VLRecipient> CompileRecipientsForMessage(Int32 messageId)
        {
            var message = SurveysDal.GetMessageById(this.AccessTokenId, messageId);
            if (message == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Message", messageId));

            return CompileRecipientsForMessage(message);
        }
        internal Collection<VLRecipient> CompileRecipientsForMessage(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            if (message.DeliveryMethod == DeliveryMethod.All)
            {
                return GetRecipients(message.Collector);
            }

            if (message.DeliveryMethod == DeliveryMethod.AllResponded)
            {
                //IsSentEmail== true && HasResponded == true
                return SurveysDal.GetRecipients(this.AccessTokenId, message.Collector, "where [AttributeFlags] & /*IsSentEmail*/1 = 1 and [AttributeFlags] & /*HasResponded*/8 = 8");
            }

            if (message.DeliveryMethod == DeliveryMethod.NotResponded)
            {
                //IsSentEmail== true && HasResponded == false
                return SurveysDal.GetRecipients(this.AccessTokenId, message.Collector, "where [AttributeFlags] & /*IsSentEmail*/1 = 1 and [AttributeFlags] & /*HasResponded*/8 = 0");
            }
            if (message.DeliveryMethod == DeliveryMethod.NewAndUnsent)
            {
                //IsSentEmail== false
                return SurveysDal.GetRecipients(this.AccessTokenId, message.Collector, "where [AttributeFlags] & /*IsSentEmail*/1 = 0");
            }
            if (message.DeliveryMethod == DeliveryMethod.Custom)
            {
                throw new NotSupportedException();
            }

            throw new VLException("Unknown DeliveryMethod");
        }
        public Int32 CompileRecipientsCountForMessage(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            if (message.DeliveryMethod == DeliveryMethod.All)
            {
                return GetRecipientsCount(message.Collector);
            }

            if (message.DeliveryMethod == DeliveryMethod.AllResponded)
            {
                //IsSentEmail== true && HasResponded == true
                return SurveysDal.GetRecipientsCount(this.AccessTokenId, message.Collector, "where [AttributeFlags] & /*IsSentEmail*/1 = 1 and [AttributeFlags] & /*HasResponded*/8 = 8");
            }

            if (message.DeliveryMethod == DeliveryMethod.NotResponded)
            {
                //IsSentEmail== true && HasResponded == false
                return SurveysDal.GetRecipientsCount(this.AccessTokenId, message.Collector, "where [AttributeFlags] & /*IsSentEmail*/1 = 1 and [AttributeFlags] & /*HasResponded*/8 = 0");
            }
            if (message.DeliveryMethod == DeliveryMethod.NewAndUnsent)
            {
                //IsSentEmail== false
                return SurveysDal.GetRecipientsCount(this.AccessTokenId, message.Collector, "where [AttributeFlags] & /*IsSentEmail*/1 = 0");
            }
            if (message.DeliveryMethod == DeliveryMethod.Custom)
            {
                throw new NotSupportedException();
            }

            throw new VLException("Unknown DeliveryMethod");
        }


        public VLRecipient GetRecipientById(Int64 recipientId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientById(this.AccessTokenId, recipientId);
        }
        public VLRecipient GetRecipientByEmail(Int32 collectorId, string email)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientByEmail(this.AccessTokenId, collectorId, email);
        }

        public VLRecipient GetRecipientByKey(Int32 collectorId, string recipientKey)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientByKey(this.AccessTokenId, collectorId, recipientKey);
        }
        internal VLRecipient GetRecipientByKey(string recipientKey)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetRecipientByKey(this.AccessTokenId, recipientKey);
        }


        /// <summary>
        /// Δημιουργεί ένα recipient για έναν collector τύπου Email.
        /// </summary>
        /// <param name="collector"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="title"></param>
        /// <param name="customValue"></param>
        /// <returns></returns>
        public VLRecipient CreateRecipient(VLCollector collector, string email, string firstName = null, string lastName = null, string title = null, string customValue = null)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return CreateRecipient(collector.CollectorId, email, firstName, lastName, title, customValue);
        }
        /// <summary>
        /// Δημιουργεί ένα recipient για έναν collector τύπου Email.
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="title"></param>
        /// <param name="customValue"></param>
        /// <returns></returns>
        public VLRecipient CreateRecipient(Int32 collectorId, string email, string firstName = null, string lastName = null, string title = null, string customValue = null)
        {
            var item = new VLRecipient();
            item.Collector = collectorId;
            item.Email = email;
            item.FirstName = firstName;
            item.LastName = lastName;
            item.Title = title;
            item.CustomValue = customValue;
            item.Status = RecipientStatus.None;

            return CreateRecipientImplementation(item);
        }

        /// <summary>
        /// Δημιουργεί ένα εικονικό recipient για collector τύπου WebLink.
        /// <para>Ενας τέτοιος Recipient δεν διαθέτει valid email address, και χρησιμοποιείται αποκλειστικά απο το σύστημα για εσωτερική χρήση.</para>
        /// <para>Ο Recipient δημιουργείται ενεργοποιημένος (το ActivationDate εχει τιμή)</para>
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        internal VLRecipient CreateRecipientVirtual(Int32 collectorId)
        {
            var item = new VLRecipient();
            item.Collector = collectorId;
            item.IsWebLinkRecipient = true;
            item.Status = RecipientStatus.OpenSurvey;
            item.ActivationDate = Utility.UtcNow();

            return CreateRecipientImplementation(item);
        }

        private VLRecipient CreateRecipientImplementation(VLRecipient recipient)
        {
            if (recipient == null) throw new ArgumentNullException("recipient");
            recipient.ValidateInstance();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            #endregion

            //τραβάμε το collector απο το σύστημά μας:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, recipient.Collector, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "CollectorRecipient", recipient.Collector));


            //Αναλόγως του τύπου του Recipient συνεχίζουμε διαφορετικα:
            if (recipient.IsWebLinkRecipient)
            {
                if (collector.CollectorType != CollectorType.WebLink)
                {
                    throw new VLException("In order to add recipients to the Collector, it must be of type WebLink!");
                }

                //Δημιουργούμε ένα μοναδικό RecipientKey:
                recipient.RecipientKey = MakeRecipientKey(RecipientKey_WebLink_Length);
                //Δημιουργούμε ένα ψεύτικο email:
                recipient.Email = recipient.RecipientKey + "@virtual.com";
                //Αποθηκεύουμε και επιστρέφουμε:
                return SurveysDal.CreateRecipient(this.AccessTokenId, recipient);
            }
            else
            {
                if(collector.CollectorType != CollectorType.Email)
                {
                    throw new VLException("In order to add recipients to the Collector, it must be of type Email!");
                }

                //Ελέγχουμε εάν το email έχει κανονική μορφή:
                if (!Utility.EmailIsValid(recipient.Email))
                {
                    throw new VLException(string.Format("Invalid email address '{0}'!", recipient.Email));
                }
                //Ελέγχουμε εάν το email είναι μοναδικό μεταξύ των recipients για αυτόν τον colllector:
                var existingItem = SurveysDal.GetRecipientByEmail(this.AccessTokenId, recipient.Collector, recipient.Email);
                if (existingItem != null)
                {
                    throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Email", recipient.Email));
                }

                //Δημιουργούμε ένα μοναδικό RecipientKey:
                recipient.RecipientKey = MakeRecipientKey(RecipientKey_Length);
                //Αποθηκεύουμε και επιστρέφουμε:
                return SurveysDal.CreateRecipient(this.AccessTokenId, recipient);
            }
        }

        /// <summary>
        /// Δημιουργεί ένα νεό RecipientKey, που είναι μοναδικό σε ολόκληρο το σύστημα
        /// <para>εάν δεν μπορεί να βρεί μοναδικό RecipientKey, αυτόματα αυξάνει το ζητούμενο πλήθος των χαρακτήρων κατα ένα και επαναλαμβάνει</para>
        /// </summary>
        /// <param name="length"></param>
        /// <param name="rnd"></param>
        /// <returns></returns>
        private string MakeRecipientKey(int length, System.Random rnd = null)
        {
            /*
             * Πρέπει να φτιάξουμε ένα νέο μοναδικό RecipientKey  
             */
            int tries = 0;
            var _recipientKey = Utility.GenerateRecipientKey(length, rnd);
            while (SurveysDal.GetRecipientByKey(this.AccessTokenId, _recipientKey) != null)
            {
                tries++;
                if (tries % 12 == 0)
                    length++;

                _recipientKey = Utility.GenerateRecipientKey(length, rnd);
            }

            return _recipientKey;
        }



        public ContactImportResult ImportRecipientsFromString(Int32 collectorId, string buffer, ContactImportOptions options)
        {
            if (string.IsNullOrWhiteSpace(buffer)) throw new ArgumentNullException("buffer");
            if (options == null) throw new ArgumentNullException("options");

            using (Stream s = Utility.GenerateStreamFromString(buffer))
            {
                return ImportRecipientsFromCSV(collectorId, s, options);
            }
        }
        public ContactImportResult ImportRecipientsFromCSV(Int32 collectorId, Stream stream, ContactImportOptions options)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (options == null) throw new ArgumentNullException("options");

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            #endregion

            //τραβάμε το collector απο το σύστημά μας:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "CollectorRecipient", collectorId));

            //recipients μπορεί να έχει μόνο ένας collector τύπου 
            if (collector.CollectorType != CollectorType.Email)
            {
                throw new VLException("In order to add recipients to a Collector, it maust be of type Email!");
            }


            ContactImportResult result = new ContactImportResult(); 
            try
            {
                using (TextReader tr = new StreamReader(stream))
                {
                    if (options.HasHeaderRecord)
                    {
                        tr.ReadLine();
                    }
                    #region
                    var random = new System.Random();
                    var csv = new CsvReader(tr, CultureInfo.InvariantCulture);
                    csv.Configuration.Delimiter = options.DelimiterCharacter;
                    csv.Configuration.HasHeaderRecord = false;
                    csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
                    csv.Configuration.BadDataFound = null;


                    var recipients = new VLRecipient[8] { new VLRecipient(), new VLRecipient(), new VLRecipient(), new VLRecipient(), new VLRecipient(), new VLRecipient(), new VLRecipient(), new VLRecipient() };
                    int _cindex = -1;
                    string _value = null;

                    while (csv.Read())
                    {
                        try
                        {
                            string _email = csv.GetField<string>(options.EmailOrdinal - 1);

                            //Ελέγχουμε εάν το email έχει κανονική μορφή:
                            if (!Utility.EmailIsValid(_email))
                            {
                                result.InvalidEmails++;
                                if (options.ContinueOnError == false)
                                    throw new VLException(string.Format("Invalid email address '{0}'!", _email));
                                else
                                    continue;
                            }

                            _cindex++;
                            recipients[_cindex].InitializeInstance(collectorId, _email, MakeRecipientKey(RecipientKey_Length, random));
                            recipients[_cindex].HasImportMark = true;

                            if (options.FirstNameOrdinal > 0)
                            {
                                if (csv.TryGetField<string>(options.FirstNameOrdinal - 1, out _value)) recipients[_cindex].FirstName = _value;
                            }
                            if (options.LastNameOrdinal > 0)
                            {
                                if (csv.TryGetField<string>(options.LastNameOrdinal - 1, out _value)) recipients[_cindex].LastName = _value;
                            }
                            if (options.TitleOrdinal > 0)
                            {
                                if (csv.TryGetField<string>(options.TitleOrdinal - 1, out _value)) recipients[_cindex].Title = _value;
                            }

                            if (_cindex >= 7)
                            {
                                Int32 successImports = 0, sameEmails = 0;
                                //Αποθηκεύουμε
                                SurveysDal.ImportRecipients(this.Principal, recipients, _cindex+1, ref successImports, ref sameEmails);
                                result.SuccesfullImports += successImports;
                                result.SameEmails += sameEmails;
                                _cindex = -1;
                            }
                        }
                        catch(Exception ex)
                        {
                            result.FailedImports++;
                            if (options.ContinueOnError == false)
                                throw;
                        }
                    }
                    if (_cindex > -1)
                    {
                        Int32 successImports = 0, sameEmails = 0;
                        //Αποθηκεύουμε
                        SurveysDal.ImportRecipients(this.Principal, recipients, _cindex+1, ref successImports, ref sameEmails);
                        result.SuccesfullImports += successImports;
                        result.SameEmails += sameEmails;
                        _cindex = -1;
                    }
                    #endregion
                }
            }
            finally
            {
                Int32 optedOutRecipients = 0, bouncedRecipients = 0, totalRecipients = 0;

                SurveysDal.ImportRecipientsFinalize(this.Principal, collectorId, ref optedOutRecipients, ref bouncedRecipients, ref totalRecipients);
                result.OptedOutEmails = optedOutRecipients;
                result.BouncedEmails = bouncedRecipients;
            }

            return result;
        }

        public ContactImportResult ImportRecipientsFromList(VLCollector collector, VLClientList list)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            if (list == null) throw new ArgumentNullException("list");

            return ImportRecipientsFromList(collector.CollectorId, list.ListId);
        }
        public ContactImportResult ImportRecipientsFromList(Int32 collectorId, Int32 listId)
        {
            //διαβάζουμε την λίστα απο το σύστημα:
            VLClientList list = SystemDal.GetClientListById(this.AccessTokenId, listId);
            if (list == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientList", listId));

            //διαβάζουμε τον Collector απο την βάση:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", collectorId));            
            //recipients μπορεί να έχει μόνο ένας collector τύπου 
            if(collector.CollectorType != CollectorType.Email)
            {
                throw new VLException("In order to add recipients to a Collector, it maust be of type Email!");
            }

            //διαβάζουμε το survey:
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, collector.TextsLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Survey", collector.Survey));


            if (survey.Client != list.Client)
            {
                throw new VLException("Invalid clientList!");
            }

            ContactImportResult result = new ContactImportResult();
            try
            {
                #region
                var random = new System.Random();
                int totalRows = 0, pageIndex = 1;
                var contacts = SystemDal.GetContacts(this.AccessTokenId, list.ListId, pageIndex++, 100, ref totalRows);
                while(contacts.Count > 0)
                {
                    foreach(var item in contacts)
                    {
                        try
                        {
                            //Ελέγχουμε εάν το email είναι μοναδικό μεταξύ των recipients για αυτόν τον colllector:
                            var existingItem = SurveysDal.GetRecipientByEmail(this.AccessTokenId, collector.CollectorId, item.Email);
                            if (existingItem != null)
                            {
                                result.SameEmails++;
                                continue;
                            }

                            //Ελέγχουμε μήπως αυτό το Contact είναι OptedOut:
                            if(item.IsOptedOut)
                            {
                                result.OptedOutEmails++;
                            }
                            //Ελέγχουμε μήπως αυτό το Contact είναι Bounced:
                            if(item.IsBouncedEmail)
                            {
                                result.BouncedEmails++;
                            }

                            var recipient = new VLRecipient();
                            recipient.Collector = collector.CollectorId;
                            recipient.Email = item.Email;
                            recipient.FirstName = item.FirstName;
                            recipient.LastName = item.LastName;
                            recipient.Title = item.Title;
                            recipient.Status = RecipientStatus.None;
                            recipient.RecipientKey = MakeRecipientKey(RecipientKey_Length);
                            recipient.IsBouncedEmail = item.IsBouncedEmail;
                            recipient.IsOptedOut = item.IsOptedOut;
                            recipient.HasImportMark = true;

                            recipient = SurveysDal.CreateRecipient(this.AccessTokenId, recipient);
                            result.SuccesfullImports++;
                        }
                        catch (Exception ex)
                        {
                            result.FailedImports++;
                        }
                    }

                    contacts = SystemDal.GetContacts(this.AccessTokenId, list.ListId, pageIndex++, 100, ref totalRows);
                }
                #endregion
            }
            finally
            {
                Int32 optedOutRecipients = 0, bouncedRecipients = 0, totalRecipients = 0;

                SurveysDal.ImportRecipientsFinalize(this.Principal, collectorId, ref optedOutRecipients, ref bouncedRecipients, ref totalRecipients);
                result.OptedOutEmails = optedOutRecipients;
                result.BouncedEmails = bouncedRecipients;
            }

            return result;
        }
        public ContactImportResult ImportRecipientsFromCollector(Int32 targetCollectorId, Int32 sourceCollectorId)
        {
            //διαβάζουμε τον Collector απο την βάση:
            var targetCollector = SurveysDal.GetCollectorById(this.AccessTokenId, targetCollectorId, BuiltinLanguages.PrimaryLanguage);
            if (targetCollector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "targetCollector", targetCollectorId));
            //recipients μπορεί να έχει μόνο ένας collector τύπου 
            if (targetCollector.CollectorType != CollectorType.Email)
            {
                throw new VLException("In order to add recipients to a Collector, it maust be of type Email!");
            }

            //διαβάζουμε τον Source Collector απο την βάση:
            var sourceCollector = SurveysDal.GetCollectorById(this.AccessTokenId, sourceCollectorId, BuiltinLanguages.PrimaryLanguage);
            if (sourceCollector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "sourceCollector", targetCollectorId));

            if(targetCollector.Survey != sourceCollector.Survey)
            {
                throw new VLException("Invalid Source Collector!");
            }

            ContactImportResult result = new ContactImportResult();
            try
            {
                #region
                var random = new System.Random();
                int totalRows = 0, pageIndex = 1;
                var recipients = SurveysDal.GetRecipients(this.AccessTokenId, sourceCollector.CollectorId, pageIndex++, 100, ref totalRows);
                while (recipients.Count > 0)
                {
                    foreach (var item in recipients)
                    {
                        try
                        {
                            //Ελέγχουμε εάν το email είναι μοναδικό μεταξύ των recipients για αυτόν τον colllector:
                            var existingItem = SurveysDal.GetRecipientByEmail(this.AccessTokenId, targetCollector.CollectorId, item.Email);
                            if (existingItem != null)
                            {
                                result.SameEmails++;
                                continue;
                            }

                            //Ελέγχουμε μήπως αυτό το Recipient είναι OptedOut:
                            if (item.IsOptedOut)
                            {
                                result.OptedOutEmails++;
                            }
                            //Ελέγχουμε μήπως αυτό το Recipient είναι Bounced:
                            if (item.IsBouncedEmail)
                            {
                                result.BouncedEmails++;
                            }

                            var recipient = new VLRecipient();
                            recipient.Collector = targetCollector.CollectorId;
                            recipient.Email = item.Email;
                            recipient.FirstName = item.FirstName;
                            recipient.LastName = item.LastName;
                            recipient.Title = item.Title;
                            recipient.CustomValue = item.CustomValue;
                            recipient.Status = RecipientStatus.None;
                            recipient.RecipientKey = MakeRecipientKey(RecipientKey_Length);
                            recipient.IsOptedOut = item.IsOptedOut;
                            recipient.IsBouncedEmail = item.IsBouncedEmail;
                            recipient.HasImportMark = true;

                            recipient = SurveysDal.CreateRecipient(this.AccessTokenId, recipient);
                            result.SuccesfullImports++;
                        }
                        catch (Exception)
                        {
                            result.FailedImports++;
                        }
                    }
                    recipients = SurveysDal.GetRecipients(this.AccessTokenId, sourceCollector.CollectorId, pageIndex++, 100, ref totalRows);
                }
                #endregion
            }
            finally
            {
                Int32 optedOutRecipients = 0, bouncedRecipients = 0, totalRecipients = 0;

                SurveysDal.ImportRecipientsFinalize(this.Principal, targetCollectorId, ref optedOutRecipients, ref bouncedRecipients, ref totalRecipients);
                result.OptedOutEmails = optedOutRecipients;
                result.BouncedEmails = bouncedRecipients;
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipient"></param>
        /// <returns></returns>
        public VLRecipient UpdateRecipient(VLRecipient recipient)
        {
            if (recipient == null) throw new ArgumentNullException("recipient");
            recipient.ValidateInstance();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            #endregion

            //Το recipient.RecipientKey πρέπει να είναι μοναδικό μέσα σε ολόκληρο το σύστημα:
            var existingItem = SurveysDal.GetRecipientByKey(this.AccessTokenId, recipient.RecipientKey);
            if(existingItem != null && existingItem.RecipientId != recipient.RecipientId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "RecipientKey", recipient.RecipientKey));
            }
            //To email πρέπει να είναι μοναδικό μέσα σε ολόκληρο τον collector:
            existingItem = SurveysDal.GetRecipientByEmail(this.AccessTokenId, recipient.Collector, recipient.Email);
            if (existingItem != null && existingItem.RecipientId != recipient.RecipientId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Email", recipient.Email));
            }
            if (existingItem == null) existingItem = SurveysDal.GetRecipientById(this.AccessTokenId, recipient.RecipientId);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Recipient", recipient.RecipientId.ToString()));


            //Κάνουμε update με βάση το existingItem:
            existingItem.RecipientKey = recipient.RecipientKey;
            existingItem.Email = recipient.Email;
            existingItem.FirstName = recipient.FirstName;
            existingItem.LastName = recipient.LastName;
            existingItem.Title = recipient.Title;
            existingItem.CustomValue = recipient.CustomValue;
            existingItem.Status = recipient.Status;
            existingItem.AttributeFlags = recipient.AttributeFlags;

            existingItem.PersonalPassword = recipient.PersonalPassword;
            existingItem.ActivationDate = recipient.ActivationDate;
            existingItem.ValidFromDate = recipient.ValidFromDate;
            existingItem.ValidToDate = recipient.ValidToDate;
            existingItem.ExpireAfter = recipient.ExpireAfter;
            existingItem.ExpirationDate = recipient.ExpirationDate;


            return SurveysDal.UpdateRecipient(this.AccessTokenId, existingItem);
        }

        /// <summary>
        /// Για εσωτερική χρήση, για λόγους performance, δεν κάνει έλεγχο!
        /// </summary>
        /// <param name="recipient"></param>
        /// <returns></returns>
        internal VLRecipient UpdateRecipientIntl(VLRecipient recipient)
        {
            if (recipient == null) throw new ArgumentNullException("recipient");
            recipient.ValidateInstance();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            #endregion

            return SurveysDal.UpdateRecipient(this.AccessTokenId, recipient);
        }
        /// <summary>
        /// Εάν ο recipient δεν έχει χρησιμοποιηθεί για την αποστολή email, δεν έχει δώσει response, τότε η RemoveRecipient είναι σαν
        /// την DeleteRecipient. Γίνεται καθολική διαγραφή του Recipient απο τον Collector.
        /// Εάν όμως ο recipient έχει χρησιμοποιηθεί, τότε η RemoveRecipient, καθιστά τον Recipient ως hidden σε οποιοδήποτε μελλοντικό
        /// request για recipients.
        /// </summary>
        /// <param name="recipientId"></param>
        public void RemoveRecipient(Int64 recipientId)
        {
            DeleteRecipient(recipientId);
        }

        public void DeleteRecipient(VLRecipient recipient)
        {
            if (recipient == null) throw new ArgumentNullException("recipient");
            DeleteRecipient(recipient.RecipientId);
        }
        public void DeleteRecipient(Int64 recipientId)
        {
            var existingItem = SurveysDal.GetRecipientById(this.AccessTokenId, recipientId);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Recipient", recipientId.ToString()));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion

            //Μπορούμε να το διαγράψουμε?




            SurveysDal.DeleteRecipient(this.AccessTokenId, existingItem.RecipientId);
        }


        public int BounceRecipient(VLCollector collector, string email, bool propagate = true)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return BounceRecipient(collector.CollectorId, email, propagate);
        }
        public int BounceRecipient(Int32 collectorId, string email, bool propagate=true)
        {
            /*Βρίσκουμε τον collector στο σύστημα (για να βεβαιωθούμε ότι μας πέρασε έγκυρο collectorId και όχι κάτι άλλο!)*/
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", collectorId));

            /*Βρίσκουμε το survey:*/
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));

            /**/
            var systemManager = VLSystemManager.GetAnInstance(this);

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion

            /*Θέλουμε όλα τα Recipients που φέρουν αυτό το email, αδιαφορώντας για τον Collector στον οποίο ανήκουν:*/
            var recipients = SurveysDal.GetRecipientsForSurveyByEmail(this.AccessTokenId, survey.SurveyId, email);
            int numberOfRecipients = 0;

            foreach (var recipient in recipients)
            {
                try
                {
                    if(recipient.IsBouncedEmail == false)
                    {
                        numberOfRecipients++;

                        recipient.IsBouncedEmail = true;
                        SurveysDal.UpdateRecipient(this.AccessTokenId, recipient);
                    }
                }
                catch
                {
                    throw;
                }
            }
            if (propagate)
            {
                try
                {
                    //Πρέπει να γίνει το ίδιο και στις ClientLists:
                    systemManager.BounceContacts(survey.Client, email);
                }
                catch
                {
                    throw;
                }
            }



            return numberOfRecipients;
        }
        internal int BounceRecipient(string recipientKey, bool propagate = true)
        {
            var recipient = SurveysDal.GetRecipientByKey(this.AccessTokenId, recipientKey);
            if (recipient == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_key, "Recipient", recipientKey));

            return BounceRecipient(recipient.Collector, recipient.Email, propagate);
        }


        public int OptOutRecipient(VLCollector collector, string email, bool propagate = true)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return OptOutRecipient(collector.CollectorId, email, propagate);
        }
        public int OptOutRecipient(Int32 collectorId, string email, bool propagate = true)
        {
            /*Βρίσκουμε τον collector στο σύστημα (για να βεβαιωθούμε ότι μας πέρασε έγκυρο collectorId και όχι κάτι άλλο!)*/
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", collectorId));

            /*Βρίσκουμε το survey:*/
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));

            /**/
            var systemManager = VLSystemManager.GetAnInstance(this);

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion

            /*Θέλουμε όλα τα Recipients που φέρουν αυτό το email, αδιαφορώντας για τον Collector στον οποίο ανήκουν:*/
            var recipients = SurveysDal.GetRecipientsForSurveyByEmail(this.AccessTokenId, survey.SurveyId, email);
            int numberOfRecipients = 0;

            foreach (var recipient in recipients)
            {
                try
                {
                    if (recipient.IsOptedOut == false)
                    {
                        numberOfRecipients++;

                        recipient.IsOptedOut = true;
                        SurveysDal.UpdateRecipient(this.AccessTokenId, recipient);
                    }
                }
                catch
                {
                    throw;
                }
            }
            if (propagate)
            {
                try
                {
                    //Πρέπει να γίνει το ίδιο και στις ClientLists:
                    systemManager.OptOutContacts(survey.Client, email);
                }
                catch
                {
                    throw;
                }
            }



            return numberOfRecipients;
        }
        internal int OptOutRecipient(string recipientKey, bool propagate = true)
        {
            var recipient = SurveysDal.GetRecipientByKey(this.AccessTokenId, recipientKey);
            if (recipient == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_key, "Recipient", recipientKey));

            return OptOutRecipient(recipient.Collector, recipient.Email, propagate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipientKey"></param>
        /// <param name="collectorId"></param>
        /// <param name="surveyPublicId"></param>
        /// <returns></returns>
        internal VLRecipient OptOutRecipient(string recipientKey, int collectorId, string surveyPublicId)
        {
            //Βρίσκουμε τον recipient που αφορά αυτό το recipientKey:
            var recipient = SurveysDal.GetRecipientByKey(this.AccessTokenId, recipientKey);
            if (recipient == null)
            {
                throw new VLException("Invalid RecipientKey.");
            }

            //Ελέγχουμε το CollectorId:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null)
            {
                throw new VLException("Invalid CollectorId.");
            }
            if (recipient.Collector != collector.CollectorId)
            {
                throw new VLException("Invalid Collector.");
            }

            //Διαβάζουμε το survey:
            var survey = SurveysDal.GetSurveyByPublicId(this.AccessTokenId, surveyPublicId, BuiltinLanguages.PrimaryLanguage);
            if (survey == null)
            {
                throw new VLException("Invalid PublicId.");
            }
            if (survey.SurveyId != collector.CollectorId)
            {
                throw new VLException("Invalid Survey.");
            }

            
            /*
             * 1. τοποθετούμε το recipient.Email στα KnownEmails:
             */
            var knownemail = SystemDal.GetKnownEmailByAddress(this.AccessTokenId, survey.Client, recipient.Email);
            if (knownemail == null)
            {
                knownemail = new VLKnownEmail();
                knownemail.Client = survey.Client;
                knownemail.EmailAddress = recipient.Email;
                knownemail.RegisterDt = Utility.UtcNow();
                knownemail.IsDomainOK = true;
                knownemail.IsVerified = true;
                knownemail.IsOptedOut = true;
                knownemail.VerifiedDt = Utility.UtcNow();
                knownemail.OptedOutDt = Utility.UtcNow();
                MailAddress mailAddress = new MailAddress(recipient.Email);
                knownemail.LocalPart = mailAddress.User;
                knownemail.DomainPart = mailAddress.Host;

                knownemail = SystemDal.CreateKnownEmail(this.AccessTokenId, knownemail);
            }
            else
            {
                if (knownemail.IsVerified == false)
                {
                    knownemail.IsDomainOK = true;
                    knownemail.IsVerified = true;
                    knownemail.VerifiedDt = Utility.UtcNow();
                }
                if (knownemail.IsOptedOut == false)
                {
                    knownemail.IsOptedOut = true;
                    knownemail.OptedOutDt = Utility.UtcNow();
                }

                knownemail = SystemDal.UpdateKnownEmail(this.AccessTokenId, knownemail);
            }

            /*
             * 2. Κάνουμε OptOut όλα τα Recipients που φέρουν αυτό το Email Address, αδιαφορώντας για τον Collector στον οποίο ανήκουν:
             */
            var recipients = SurveysDal.GetRecipientsForSurveyByEmail(this.AccessTokenId, survey.SurveyId, recipient.Email);
            int numberOfRecipients = 0;
            
            foreach (var item in recipients)
            {
                if (item.IsOptedOut == false)
                {
                    numberOfRecipients++;

                    item.IsOptedOut = true;
                    SurveysDal.UpdateRecipient(this.AccessTokenId, item);
                }
            }

            //Μετά απο το Update, διαβάζουμε το Recipient απο την βάση:
            recipient = SurveysDal.GetRecipientById(this.AccessTokenId, recipient.RecipientId);


            /*
             * 2. Κάνουμε OptedOut όλα τα Contacs του πελάτη που έχουν την ίδια Email Address:
             */
            var contacts = SystemDal.GetContactsForClientByEmail(this.AccessTokenId, survey.Client, recipient.Email);
            int numberOfContacts = 0;

            foreach (var contact in contacts)
            {
                if (contact.IsOptedOut == false)
                {
                    numberOfContacts++;

                    contact.IsOptedOut = true;
                    SystemDal.UpdateContact(this.AccessTokenId, contact);
                }
            }


            return recipient;
        }

        /// <summary>
        /// Remove all recipients that have not been sent a message yet.
        /// </summary>
        /// <param name="collector"></param>
        /// <returns></returns>
        public int RemoveAllUnsentRecipients(VLCollector collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return RemoveAllUnsentRecipients(collector.CollectorId);
        }
        /// <summary>
        /// Remove all recipients that have not been sent a message yet.
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public int RemoveAllUnsentRecipients(Int32 collectorId)
        {
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", collectorId));

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion

            return SurveysDal.RemoveAllUnsentRecipientsFromCollector(this.AccessTokenId, collector.CollectorId);
        }
        /// <summary>
        /// Remove all recipients that have declined to receive further mailings. (Responded recipients will remain in list.)
        /// </summary>
        /// <param name="collector"></param>
        /// <returns></returns>
        public int RemoveAllOptedOutRecipients(VLCollector collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return RemoveAllOptedOutRecipients(collector.CollectorId);
        }
        /// <summary>
        /// Remove all recipients that have declined to receive further mailings. (Responded recipients will remain in list.)
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public int RemoveAllOptedOutRecipients(Int32 collectorId)
        {
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", collectorId));

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion


            return SurveysDal.RemoveAllOptedOutRecipientsFromCollector(this.AccessTokenId, collector.CollectorId);
        }
        /// <summary>
        /// Remove all recipients with bounced email address. (Responded recipients will remain in list.)
        /// </summary>
        /// <param name="collector"></param>
        /// <returns></returns>
        public int RemoveAllBouncedRecipients(VLCollector collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");
            return RemoveAllBouncedRecipients(collector.CollectorId);
        }
        /// <summary>
        /// Remove all recipients with bounced email address. (Responded recipients will remain in list.)
        /// </summary>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public int RemoveAllBouncedRecipients(Int32 collectorId)
        {
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", collectorId));

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion

            return SurveysDal.RemoveAllBouncedRecipientsFromCollector(this.AccessTokenId, collector.CollectorId);
        }
        /// <summary>
        /// Remove all contacts that match the domain name you enter the textbox below. (Responded recipients will remain in list.)
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="domainName"></param>
        /// <returns></returns>
        public int RemoveByDomainRecipients(Int32 collectorId, string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName)) throw new ArgumentNullException("domainName");

            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", collectorId));

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors);
            }
            #endregion

            return SurveysDal.RemoveByDomainRecipientsFromCollector(this.AccessTokenId, collector.CollectorId, domainName);
        }

        #endregion

        #region Support for MessageRecipient & Sending
        /// <summary>
        /// Με βάση το DeliveryMethod δημιουργεί τα MessageRecipients για το συγκεκριμένο message
        /// <para>Ο πίνακας MessageRecipients, γεμίζει μέ όλους τους recipients, στους οποίους πρέπει να στείλουμε το μήνυμα</para>
        /// <para>Παράλληλα (εάν ο Πελάτης πληρώνει, καi Collector χρεώνεται με emails που αποστέλονται), τότε δημιουργήται και η σύνδεση με
        /// τον CollectorPayment απο τον οποίο θα γίνει η χρέωση</para>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private Int32 PrepareMessageRecipients(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            /*Prepare γίνεται μόνο όταν το μήνυμα είναι σε status DRAFT:*/
            if (message.Status != MessageStatus.Draft)
            {
                throw new VLException("Message has invalid status!");
            }


            return SurveysDal.PrepareMessageRecipients(this.AccessTokenId, message.Collector, message.MessageId);
        }



        /// <summary>
        /// Επιστρέφει όλα τα MessageRecipients που ανήκουν στο συγκεκριμένο message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public Collection<VLMessageRecipient> GetMessageRecipients(Int32 messageId, string whereClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetMessageRecipients(this.AccessTokenId, messageId, whereClause);
        }
        /// <summary>
        /// Επιστρέφει, ανα σελίδα, όλα τα MessageRecipients που ανήκουν στο συγκεκριμένο message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public Collection<VLMessageRecipient> GetMessageRecipients(Int32 messageId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetMessageRecipients(this.AccessTokenId, messageId, pageIndex, pageSize, ref totalRows, whereClause);
        }
        /// <summary>
        /// Επιστρέφει το πλήθος των MessageRecipients που ανήκουν στο συγκεκριμένο message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public Int32 GetMessageRecipientsCount(Int32 messageId, string whereClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetMessageRecipientsCount(this.AccessTokenId, messageId, whereClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="recipientId"></param>
        /// <returns></returns>
        internal VLMessageRecipient GetMessageRecipientById(Int32 messageId, Int64 recipientId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion


            return SurveysDal.GetMessageRecipientById(this.AccessTokenId, messageId, recipientId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageRecipient"></param>
        /// <returns></returns>
        internal VLMessageRecipient UpdateMessageRecipient(VLMessageRecipient messageRecipient)
        {
            if (messageRecipient == null) throw new ArgumentNullException("messageRecipient");

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.UpdateMessageRecipient(this.AccessTokenId, messageRecipient);
        }



        /// <summary>
        /// Επιστρέφει όλα τα Recipients που έχουν συνδεθεί στο συγκεκριμένο message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public Collection<VLRecipient> GetRecipientsForMessage(Int32 messageId)
        {
            //Βρίσκουμε το message απο το σύστημά μας:
            var message = SurveysDal.GetMessageById(this.AccessTokenId, messageId);
            if (message == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "message", messageId));
            
            return GetRecipientsForMessage(message);
        }
        /// <summary>
        /// Επιστρέφει όλα τα Recipients που έχουν συνδεθεί στο συγκεκριμένο message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal Collection<VLRecipient> GetRecipientsForMessage(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            #region SecurityLayer
            //PASS THROUGH
            #endregion


            var whereClause = string.Format("where [RecipientId] in (select [Recipient] from [dbo].[MessageRecipients] where [Message]={0})", message.MessageId);
            return SurveysDal.GetRecipients(this.AccessTokenId, message.Collector, whereClause, "order by RecipientId");
        }
        /// <summary>
        /// Επιστρέφει, ανα σελίδα, όλα τα Recipients που έχουν συνδεθεί στο συγκεκριμένο message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public Collection<VLRecipient> GetRecipientsForMessage(Int32 messageId, int pageIndex, int pageSize, ref int totalRows)
        {
            //Βρίσκουμε το message απο το σύστημά μας:
            var message = SurveysDal.GetMessageById(this.AccessTokenId, messageId);
            if (message == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "message", messageId));

            return GetRecipientsForMessage(message, pageIndex, pageSize, ref totalRows);
        }
        /// <summary>
        /// Επιστρέφει, ανα σελίδα, όλα τα Recipients που έχουν συνδεθεί στο συγκεκριμένο message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        internal Collection<VLRecipient> GetRecipientsForMessage(VLMessage message, int pageIndex, int pageSize, ref int totalRows)
        {
            if (message == null) throw new ArgumentNullException("message");

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            var whereClause = string.Format("where [RecipientId] in (select [Recipient] from [dbo].[MessageRecipients] where [Message]={0})", message.MessageId);
            return SurveysDal.GetRecipients(this.AccessTokenId, message.Collector, pageIndex, pageSize, ref totalRows, whereClause, "order by RecipientId");
        }
        /// <summary>
        /// Επιστρέφει το πλήθος των Recipients που έχουν συνδεθεί στο συγκεκριμένο message
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public Int32 GetRecipientsCountForMessage(Int32 messageId)
        {
            //Βρίσκουμε το message απο το σύστημά μας:
            var message = SurveysDal.GetMessageById(this.AccessTokenId, messageId);
            if (message == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "message", messageId));

            return GetRecipientsCountForMessage(message);
        }
        /// <summary>
        /// Επιστρέφει το πλήθος των Recipients που έχουν συνδεθεί στο συγκεκριμένο message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal Int32 GetRecipientsCountForMessage(VLMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            var whereClause = string.Format("where [RecipientId] in (select [Recipient] from [dbo].[MessageRecipients] where [Message]={0})", message.MessageId);
            return SurveysDal.GetRecipientsCount(this.AccessTokenId, message.Collector, whereClause);
        }





        #endregion

        #region VLResponse
        public Collection<VLResponse> GetResponses(Int32 survey, string whereClause = null, string orderByClause = null)
        {
            return SurveysDal.GetResponses(AccessTokenId, survey, whereClause, orderByClause);
        }
        public Collection<VLResponse> GetResponses(Int32 survey, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            return SurveysDal.GetResponses(AccessTokenId, survey, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        public int GetResponsesCount(Int32 survey, string whereClause = null)
        {
            return SurveysDal.GetResponsesCount(this.AccessTokenId, survey, whereClause);
        }
        public int GetResponsesCountForCollector(Int32 survey, Int32 collector)
        {
            return SurveysDal.GetResponsesCount(this.AccessTokenId, survey, string.Format("where Collector={0}", collector));
        }

        public VLResponse GetResponseById(Int64 responseId)
        {
            return SurveysDal.GetResponseById(AccessTokenId, responseId);
        }

        /// <summary>
        /// Δημιουργεί στο σύστημα ένα νέο Response, για τον συγκεκριμένο recipient.
        /// <para>Ταυτόχρονα ενημερώνει τον μετρητή των Responses επάνω στον Collector, τον μετρητή των Responses επάνω Surveys (RecordedResponses),
        /// και το attribute HasResponses επάνω στο Survey.</para>
        /// </summary>
        /// <param name="survey"></param>
        /// <param name="collector"></param>
        /// <param name="recipient"></param>
        /// <param name="openDate"></param>
        /// <param name="closeDate"></param>
        /// <returns></returns>
        internal VLResponse CreateResponse(Int32 survey, Int32? collector, Int64? recipient, DateTime openDate, DateTime? closeDate = null)
        {
            VLResponse response = new VLResponse();
            response.Survey = survey;
            response.Collector = collector;
            response.Recipient = recipient;
            response.OpenDate = openDate;
            response.CloseDate = closeDate;

            return CreateResponse(response);
        }
        /// <summary>
        /// Δημιουργεί στο σύστημα ένα νέο Response, για τον συγκεκριμένο recipient.
        /// <para>Tαυτόχρονα ενημερώνει τον μετρητή των responses επάνω στον Collector, και το attribute Hasresponses επάνω στο Survey.</para>
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal VLResponse CreateResponse(VLResponse response)
        {
            if (response == null) throw new ArgumentNullException("response");
            response.ValidateInstance();

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientManageAnswers);
            }
            #endregion

            return SurveysDal.CreateResponse(AccessTokenId, response);
        }
        public VLResponse UpdateResponse(VLResponse response)
        {
            if (response == null) throw new ArgumentNullException("response");
            response.ValidateInstance();

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientManageAnswers);
            }
            #endregion


            return SurveysDal.UpdateResponse(AccessTokenId, response);
        }
        public void DeleteResponse(VLResponse response)
        {
            if (response == null) throw new ArgumentNullException("response");
            DeleteResponse(response.ResponseId);
        }
        public void DeleteResponse(Int64 responseId)
        {
            var existingItem = SurveysDal.GetResponseById(this.AccessTokenId, responseId);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Response", responseId));



            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientManageAnswers);
            }
            #endregion


            SurveysDal.DeleteResponse(AccessTokenId, responseId);
        }


        /// <summary>
        /// Δημιουργεί ένα Response για το survey που μόλις τελείωσε ο συγκεκριμένος Recipient.
        /// <para>ΣΤΑ RESPONSES ΟΙ ΗΜΕΡΟΜΗΝΙΕΣ ΕΙΝΑΙ ΠΑΝΤΑ MM/DD/YYYY</para>
        /// <para></para>
        /// </summary>
        /// <param name="session"></param>
        /// <param name="survey"></param>
        /// <param name="collector"></param>
        /// <param name="recipient"></param>
        /// <param name="isDisqualified"></param>
        /// <returns></returns>
        internal VLResponse CreateResponse(VLRuntimeSession session, VLSurvey survey, VLCollector collector, VLRecipient recipient, bool isDisqualified)
        {
            /* 
             * Ελέγχουμε μήπως έχουμε ήδη δημιουργήσει Response για αυτό το recipient:
             */
            if (recipient!=null && (recipient.Status == RecipientStatus.Completed || recipient.Status == RecipientStatus.Disqualified))
            {
                var existingResponse = SurveysDal.GetResponseByRecipient(this.AccessTokenId, recipient.RecipientId);
                return existingResponse;
            }


            /*
             * Δημιουργούμε ένα νέο Response
             */
            VLResponse response = new VLResponse();
            response.Survey = survey.SurveyId;
            response.Collector = collector.CollectorId;
            if(recipient != null)
            {
                response.Recipient = recipient.RecipientId;
            }
            response.OpenDate = session.StartDt;
            response.CloseDate = Utility.UtcNow();
            /*
             * τώρα πρέπει να χρεώσουμε το Response (εάν έτσι πρέπει) 
             */
            if (collector.UseCredits && collector.CreditType.HasValue)
            {
                response.CreditType = collector.CreditType.Value;
                if(collector.CreditType.Value == CreditType.ResponseType)
                {
                    //Θα το χρεώσουμε εμείς, αργότερα μετά την δημιουργία του Response
                    response.MustBeCharged = true;
                }
                else if(collector.CreditType.Value == CreditType.ClickType)
                {
                    //Εχει ήδη χρεωθεί ο Πελάτης για αυτό το Response, σαν click. Οι πληροφορίες πληρωμής, βρίσκονται στο session
                    if(session.IsCharged)
                    {
                        response.IsCharged = true;
                        response.CollectorPayment = session.CollectorPayment;
                    }
                }
                else if(collector.CreditType.Value == CreditType.EmailType)
                {
                    /*
                     * Εχει ήδη χρεωθεί ο Πελάτης για αυτό το Response, σαν αποστολή email. Οι πληροφορίες πληρωμής, βρίσκονται στο αντίστοιχο MessageRecipient
                     * Αλλά μπορεί να έχουμε στείλει πολλαπλά μηνύματα για ένα Recipient, για αυτό θεωρούμε πάντα ότι το σώστί είναι το πρώτο
                     */
                    var messageRecipients = SurveysDal.GetMessageRecipientsForRecipient(this.AccessTokenId, recipient.RecipientId);
                    if(messageRecipients.Count >=1)
                    {
                        response.IsCharged = messageRecipients[0].IsCharged;
                        response.CollectorPayment = messageRecipients[0].CollectorPayment;
                    }
                }
            }

            var collectorResponses = collector.Responses;
            var surveyHasResponses = survey.HasResponses;
            try
            {
                //Τραβάμε τις ερωτήσεις απο το σύστημα:
                var questions = SurveysDal.GetQuestionsForSurvey(this.AccessTokenId, survey.SurveyId, null, survey.TextsLanguage);

                //Δημιουργούμε το Respose:
                response = SurveysDal.CreateResponse(this.AccessTokenId, response);
                //Χρεώνουμε κάποια πληρωμή για το Response:
                if (collector.CreditType.HasValue && collector.CreditType.Value == CreditType.ResponseType)
                {
                    response = SurveysDal.ChargePaymentForResponseImpl(this.AccessTokenId, response.ResponseId, collector.CollectorId, collector.Survey, Utility.UtcNow());
                }


                //τραβάμε τις απαντήσεις σε κάθε ερώτηση του survey και δημιουργούμε Responsedetails:
                foreach (var q in questions)
                {
                    VLResponseDetail detail = new VLResponseDetail();
                    detail.Response = response.ResponseId;
                    detail.Question = q.QuestionId;

                    #region
                    switch (q.QuestionType)
                    {
                        case QuestionType.SingleLine:
                            {
                                #region
                                if (q.ValidationBehavior == ValidationMode.Date1 || q.ValidationBehavior == ValidationMode.Date2)
                                {
                                    try
                                    {
                                        //ΣΤΑ RESPONSES ΟΙ ΗΜΕΡΟΜΗΝΙΕΣ ΕΙΝΑΙ ΠΑΝΤΑ MM/DD/YYYY:
                                        detail.UserInput = string.Format("{0:00}/{1:00}/{2:0000}", Int32.Parse(session[q.HtmlQuestionId + "_MONTH"].ToString()), Int32.Parse(session[q.HtmlQuestionId + "_DAY"].ToString()), Int32.Parse(session[q.HtmlQuestionId + "_YEAR"].ToString()));
                                    }
                                    catch
                                    {
                                        detail.UserInput = null;
                                    }
                                    detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                }
                                else
                                {
                                    detail.UserInput = session[q.HtmlQuestionId] as string;
                                    detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                }
                                #endregion
                            }
                            break;
                        case QuestionType.MultipleLine:
                        case QuestionType.Integer:
                        case QuestionType.Decimal:
                            {
                                #region
                                detail.UserInput = session[q.HtmlQuestionId] as string;
                                detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                #endregion
                            }
                            break;
                        case QuestionType.Date:
                            {
                                #region
                                if (q.UseDateTimeControls)
                                {
                                    if (q.ValidationBehavior == ValidationMode.Date1)
                                    {
                                        detail.UserInput = session[q.HtmlQuestionId] as string;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            //ΣΤΑ RESPONSES ΟΙ ΗΜΕΡΟΜΗΝΙΕΣ ΕΙΝΑΙ ΠΑΝΤΑ MM/DD/YYYY:
                                            detail.UserInput = session[q.HtmlQuestionId] as string;
                                            var date2 = DateTime.ParseExact(detail.UserInput, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                            detail.UserInput = string.Format("{0:00}/{1:00}/{2:0000}", date2.Month, date2.Day, date2.Year);
                                        }
                                        catch
                                        {
                                            detail.UserInput = null;
                                        }
                                    }
                                    detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                }
                                else
                                {
                                    if (q.ValidationBehavior == ValidationMode.Date1 || q.ValidationBehavior == ValidationMode.Date2)
                                    {
                                        try
                                        {
                                            //ΣΤΑ RESPONSES ΟΙ ΗΜΕΡΟΜΗΝΙΕΣ ΕΙΝΑΙ ΠΑΝΤΑ MM/DD/YYYY:
                                            detail.UserInput = string.Format("{0:00}/{1:00}/{2:0000}", Int32.Parse(session[q.HtmlQuestionId + "_MONTH"].ToString()), Int32.Parse(session[q.HtmlQuestionId + "_DAY"].ToString()), Int32.Parse(session[q.HtmlQuestionId + "_YEAR"].ToString()));
                                        }
                                        catch
                                        {
                                            detail.UserInput = null;
                                        }
                                        detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                    }
                                }
                                #endregion
                            }
                            break;
                        case QuestionType.Time:
                            {

                            }
                            break;
                        case QuestionType.DateTime:
                            {

                            }
                            break;
                        case QuestionType.DropDown:
                        case QuestionType.OneFromMany:
                            {
                                #region
                                byte _selectedOption = 0;
                                string _sessionValue = session[q.HtmlQuestionId] as string;
                                if (_sessionValue == q.HtmlQuestionId + "OptionalInputBox_")
                                {
                                    //εχει επιλέξει το OptionalInputBox
                                    detail.SelectedOption = 0;
                                    try
                                    {
                                        if (q.ValidationBehavior == ValidationMode.Date1 || q.ValidationBehavior == ValidationMode.Date2)
                                        {
                                            detail.UserInput = string.Format("{0:00}/{1:00}/{2:0000}", Int32.Parse(session[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_MONTH"].ToString()), Int32.Parse(session[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_DAY"].ToString()), Int32.Parse(session[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_YEAR"].ToString()));
                                        }
                                        else
                                        {
                                            detail.UserInput = session[q.HtmlQuestionId + "OptionalInputBox_userinput"] as string;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //TODO....
                                        detail.UserInput = null;
                                    }
                                    detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                }
                                else
                                {
                                    if (_sessionValue != null && byte.TryParse(_sessionValue, out _selectedOption))
                                    {
                                        detail.SelectedOption = _selectedOption;
                                        detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                    }
                                }
                                #endregion
                            }
                            break;
                        case QuestionType.ManyFromMany:
                            {
                                #region
                                //Μαζεύουμε τα options της απάντησης:
                                var options = SurveysDal.GetQuestionOptions(this.AccessTokenId, q.Survey, q.QuestionId, q.TextsLanguage);
                                foreach (var option in options)
                                {
                                    byte _selectedOption = 0;
                                    string _sessionValue = session[option.HtmlOptionId] as string;
                                    if (_sessionValue != null && byte.TryParse(_sessionValue, out _selectedOption))
                                    {
                                        detail.SelectedOption = option.OptionId;
                                        detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                    }
                                }

                                //Μαζεύουμε τα OptionalInputBox value (εάν υπάρχει):
                                var _value = session[q.HtmlQuestionId + "OptionalInputBox_"] as string;
                                if (_value == "1")
                                {
                                    //εχει επιλέξει το OptionalInputBox
                                    detail.SelectedOption = 0;
                                    try
                                    {
                                        if (q.ValidationBehavior == ValidationMode.Date1 || q.ValidationBehavior == ValidationMode.Date2)
                                        {
                                            detail.UserInput = string.Format("{0:00}/{1:00}/{2:0000}", Int32.Parse(session[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_MONTH"].ToString()), Int32.Parse(session[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_DAY"].ToString()), Int32.Parse(session[q.HtmlQuestionId + "OptionalInputBox_userinput" + "_YEAR"].ToString()));
                                        }
                                        else
                                        {
                                            detail.UserInput = session[q.HtmlQuestionId + "OptionalInputBox_userinput"] as string;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //TODO....
                                        detail.UserInput = null;
                                    }
                                    detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                }
                                #endregion
                            }
                            break;
                        case QuestionType.Slider:
                            {

                            }
                            break;
                        case QuestionType.Range:
                            {
                                #region
                                detail.UserInput = session[q.HtmlQuestionId] as string;
                                detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                #endregion
                            }
                            break;
                        case QuestionType.MatrixOnePerRow:
                            {
                                #region
                                var options = SurveysDal.GetQuestionOptions(this.AccessTokenId, q.Survey, q.QuestionId, q.TextsLanguage);
                                //var columns = SurveysDal.GetQuestionColumns(this.AccessTokenId, q.Survey, q.QuestionId, q.TextsLanguage);
                                foreach (var option in options)
                                {
                                    byte _selectedColumn = 0;
                                    string _sessionValue = session[option.HtmlOptionId] as string;
                                    if (_sessionValue != null && byte.TryParse(_sessionValue, out _selectedColumn))
                                    {
                                        detail.SelectedOption = option.OptionId;
                                        detail.SelectedColumn = _selectedColumn;
                                        detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                    }
                                }
                                #endregion
                            }
                            break;
                        case QuestionType.MatrixManyPerRow:
                            {
                                #region
                                var options = SurveysDal.GetQuestionOptions(this.AccessTokenId, q.Survey, q.QuestionId, q.TextsLanguage);
                                var columns = SurveysDal.GetQuestionColumns(this.AccessTokenId, q.Survey, q.QuestionId, q.TextsLanguage);
                                foreach (var option in options)
                                {
                                    foreach(var column in columns)
                                    {
                                        byte _value = 0;
                                        string _sessionValue = session[option.HtmlOptionId + column.HtmlPartialColumnId] as string;
                                        if (_sessionValue != null && byte.TryParse(_sessionValue, out _value))
                                        {
                                            detail.SelectedOption = option.OptionId;
                                            detail.SelectedColumn = column.ColumnId;
                                            detail = SurveysDal.CreateResponseDetail(AccessTokenId, detail);
                                        }
                                    }
                                }
                                #endregion
                            }
                            break;
                        case QuestionType.MatrixManyPerRowCustom:
                            {
                            }
                            break;
                        case QuestionType.Composite:
                            {
                            }
                            break;
                    }
                    #endregion
                }


                //σετάρουμε μερικά στοιχεία επάνω στο response:
                response.RecipientIP = session.RecipientIP;
                response.UserAgent = session.UserAgent;
                response.ResponseType = session.ResponseType;
                response.IsDisqualified = isDisqualified;
                response = SurveysDal.UpdateResponse(this.AccessTokenId, response);

                //τώρα λέμε στο σύστημα ότι ο recipient απάντησε:
                if (recipient != null)
                {
                    if (isDisqualified)
                    {
                        recipient.Status = RecipientStatus.Disqualified;
                    }
                    else
                    {
                        recipient.Status = RecipientStatus.Completed;
                    }
                    recipient.HasResponded = true;
                    if (session.ResponseType == ResponseType.Manual)
                    {
                        recipient.HasManuallyAdded = true;
                    }
                    recipient = SurveysDal.UpdateRecipient(this.AccessTokenId, recipient);
                }

                //τώρα λέμε στο session ότι το survey τερμάτισε
                session.IsFinished = true;
                SurveysDal.UpdateRuntimeSession(this.AccessTokenId, session);

                //άυξάνουμε κατα ένα τον μετρητή των απαντήσεων στον collector:
                //collector.Responses++;
                //collector = SurveysDal.UpdateCollector(this.AccessTokenId, collector);

                //if (survey.HasResponses == false)
                //{
                //    survey.HasResponses = true;
                //    survey = SurveysDal.UpdateSurvey(this.AccessTokenId, survey);
                //}

                //
                return response;
            }
            catch
            {
                if (response != null)
                {
                    if (recipient.Status == RecipientStatus.Completed)
                    {
                        recipient.Status = RecipientStatus.PartiallyCompleted;
                        recipient.HasResponded = false;
                        recipient = SurveysDal.UpdateRecipient(this.AccessTokenId, recipient);
                    }
                    SurveysDal.DeleteResponse(this.AccessTokenId, response.ResponseId);
                    response = null;
                }

                throw;
            }
        }

        internal VLCollector ClearResponsesForCollector(Int32 collectorId)
        {
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if (collector == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "collector", collectorId));

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientManageAnswers);
            }
            #endregion


            //TODO: check if we can clear all the responses for this Collector:

            //Διαγράφουμε όλη την αλυσίδα Responses/ResponseDetails για  αυτό το Collector:
            collector = SurveysDal.DeleteAllResponsesForCollector(this.AccessTokenId, collector.Survey, collector.CollectorId);


            return collector;
        }
        #endregion


        #region VLResponseDetail
        /// <summary>
        /// Επιστρέφει ολόκληρη το φύλλο απάντησεων με το συγκεκριμένο responseId
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public Collection<VLResponseDetail> GetResponseDetails(Int64 response)
        {
            return SurveysDal.GetResponseDetails(AccessTokenId, response);
        }
        /// <summary>
        /// Επιστρέφει τις αποκρίσεις μόνο για συγκεκριμένη ερώτηση απο το φύλλο απαντήσεων
        /// </summary>
        /// <param name="response"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public Collection<VLResponseDetail> GetResponseDetails(Int64 response, Int16 question)
        {
            return SurveysDal.GetResponseDetails(AccessTokenId, response, question);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="question"></param>
        /// <param name="option"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public VLResponseDetail GetResponseDetail(Int64 response, Int16 question, byte option, byte column)
        {
            return SurveysDal.GetResponseDetailById(AccessTokenId, response, question, option, column);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="question"></param>
        /// <param name="selectedOption"></param>
        /// <param name="selectedColumn"></param>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public VLResponseDetail CreateResponseDetail(Int64 response, Int16 question, Byte? selectedOption, Byte? selectedColumn = null, string userInput = null)
        {
            VLResponseDetail detail = new VLResponseDetail();
            detail.Response = response;
            detail.Question = question;
            detail.SelectedOption = selectedOption;
            detail.SelectedColumn = selectedColumn;
            detail.UserInput = userInput;

            return CreateResponseDetail(detail);
        }
        internal VLResponseDetail CreateResponseDetail(VLResponseDetail detail)
        {
            return SurveysDal.CreateResponseDetail(AccessTokenId, detail);
        }


        public void DeleteResponseDetail(Int64 response)
        {
            SurveysDal.DeleteResponseDetail(AccessTokenId, response);
        }
        public void DeleteResponseDetail(Int64 response, Int16 question)
        {
            SurveysDal.DeleteResponseDetail(AccessTokenId, response, question);
        }

        
        #endregion

        #region VLView
        /// <summary>
        /// Επιστρέφει τα views για το συγκεκριμένο survey.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public Collection<VLView> GetViews(VLSurvey survey)
        {
            if (survey == null) throw new ArgumentNullException("survey");
            return GetViews(survey.SurveyId);
        }
        /// <summary>
        /// Επιστρέφει τα views για το συγκεκριμένο survey.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public Collection<VLView> GetViews(Int32 surveyId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetViews(this.AccessTokenId, surveyId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public int GetViewsCount(Int32 surveyId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetViewsCount(this.AccessTokenId, surveyId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public VLView GetViewById(Guid viewId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetViewById(this.AccessTokenId, viewId);
        }
        /// <summary>
        /// Επιστρέφει το default view για το συγκεκριμένο ερωτηματολόγιο
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public VLView GetDefaultView(VLSurvey survey)
        {
            if (survey == null) throw new ArgumentNullException("survey");
            return GetDefaultView(survey.SurveyId);
        }
        /// <summary>
        /// Επιστρέφει το default view για το συγκεκριμένο ερωτηματολόγιο
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public VLView GetDefaultView(Int32 surveyId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetDefaultView(this.AccessTokenId, surveyId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        internal VLView UpdateView(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");
            view.ValidateInstance();


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEditSurveys);
            }
            #endregion


            return SurveysDal.UpdateView(this.AccessTokenId, view);
        }
        #endregion

        #region VLView - VLViewPage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public Collection<VLViewPage> GetViewPages(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");
            return GetViewPages(view.ViewId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public Collection<VLViewPage> GetViewPages(Guid viewId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetViewPages(this.AccessTokenId, viewId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public VLViewPage GetViewPagebyId(VLView view, VLSurveyPage page)
        {
            if (view == null) throw new ArgumentNullException("view");
            if (page == null) throw new ArgumentNullException("page");

            return GetViewPagebyId(view.ViewId, page.Survey, page.PageId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="survey"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public VLViewPage GetViewPagebyId(Guid viewId, Int32 survey, Int16 page)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetViewPageByIdImpl(this.AccessTokenId, viewId, survey, page);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewPage"></param>
        /// <returns></returns>
        public VLViewPage UpdateViewPage(VLViewPage viewPage)
        {
            if (viewPage == null) throw new ArgumentNullException("viewPage");

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.UpdateViewPage(this.AccessTokenId, viewPage);
        }
        #endregion

        #region VLView - VLViewQuestion
        /// <summary>
        /// Επιστρέφει όλες τις ερωτήσεις μίας συγκεκριμένης όψης
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public Collection<VLViewQuestion> GetViewQuestions(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");
            return GetViewQuestions(view.ViewId);
        }
        /// <summary>
        /// Επιστρέφει όλες τις ερωτήσεις μίας συγκεκριμένης όψης
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public Collection<VLViewQuestion> GetViewQuestions(Guid viewId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetViewQuestions(this.AccessTokenId, viewId);
        }
        /// <summary>
        /// Επιστρέφει όλες τις ερωτήσεις μίας συγκεκριμένης σελίδας μίας όψης
        /// </summary>
        /// <param name="viewPage"></param>
        /// <returns></returns>
        public Collection<VLViewQuestion> GetViewPageQuestions(VLViewPage viewPage)
        {
            if (viewPage == null) throw new ArgumentNullException("viewPage");
            return GetViewPageQuestions(viewPage.ViewId, viewPage.Survey, viewPage.Page);
        }
        /// <summary>
        /// Επιστρέφει όλες τις ερωτήσεις μίας συγκεκριμένης σελίδας μίας όψης
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="survey"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Collection<VLViewQuestion> GetViewPageQuestions(Guid viewId, Int32 survey, Int16 page)
        {
            return SurveysDal.GetViewPageQuestions(this.AccessTokenId, viewId, survey, page);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public VLViewQuestion GetViewQuestionById(VLView view, VLSurveyQuestion question)
        {
            if (view == null) throw new ArgumentNullException("view");
            if (question == null) throw new ArgumentNullException("question");

            return GetViewQuestionById(view.ViewId, question.Survey, question.QuestionId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public VLViewQuestion GetViewQuestionById(Guid viewId, Int32 survey, Int16 question)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetViewQuestionById(this.AccessTokenId, viewId, survey, question);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public VLViewQuestion UpdateViewQuestion(VLViewQuestion question)
        {
            if (question == null) throw new ArgumentNullException("question");

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.UpdateViewQuestion(this.AccessTokenId, question);
        }

        internal VLViewQuestion SetChartType(Guid viewId, Int32 survey, Int16 question, ChartType chartType)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion


            return SurveysDal.SetChartType(this.AccessTokenId, viewId, survey, question, chartType);
        }
        internal VLViewQuestion SwitchAxisScale(Guid viewId, Int32 survey, Int16 question)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion


            return SurveysDal.SwitchAxisScale(this.AccessTokenId, viewId, survey, question);
        }
        internal VLViewQuestion ToggleChartVisibility(Guid viewId, Int32 survey, Int16 question)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion


            return SurveysDal.ToggleChartVisibility(this.AccessTokenId, viewId, survey, question);
        }
        internal VLViewQuestion ToggleDataTableVisibility(Guid viewId, Int32 survey, Int16 question)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion


            return SurveysDal.ToggleDataTableVisibility(this.AccessTokenId, viewId, survey, question);
        }
        internal VLViewQuestion ToggleZeroResponseOptionsVisibility(Guid viewId, Int32 survey, Int16 question)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion


            return SurveysDal.ToggleZeroResponseOptionsVisibility(this.AccessTokenId, viewId, survey, question);
        }
        #endregion

        #region VLView - VLViewCollector
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public Collection<VLViewCollector> GetViewCollectors(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");
            return GetViewCollectors(view.ViewId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public Collection<VLViewCollector> GetViewCollectors(Guid viewId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetViewCollectors(this.AccessTokenId, viewId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="collectorId"></param>
        /// <returns></returns>
        public VLViewCollector GetViewCollectorById(Guid viewId, Int32 collectorId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.GetViewCollectorById(this.AccessTokenId, viewId, collectorId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collector"></param>
        /// <returns></returns>
        public VLViewCollector UpdateViewCollector(VLViewCollector collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SurveysDal.UpdateViewCollector(this.AccessTokenId, collector);
        }
        #endregion

        #region VLView - VLViewFilter
        /// <summary>
        /// Επιστρέφει όλα τα viewFilters της συγκεκριμένης όψης ταξινομημένα κατα σειρά εφαρμογής τους.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public Collection<VLViewFilter> GetViewFilters(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");
            return GetViewFilters(view.ViewId);
        }
        /// <summary>
        /// Επιστρέφει όλα τα viewFilters της συγκεκριμένης όψης ταξινομημένα κατα σειρά εφαρμογής τους.
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public Collection<VLViewFilter> GetViewFilters(Guid viewId)
        {
            return SurveysDal.GetViewFilters(this.AccessTokenId, viewId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterId"></param>
        /// <returns></returns>
        public VLViewFilter GetViewFilterById(Int32 filterId)
        {
            return SurveysDal.GetViewFilterById(this.AccessTokenId, filterId);
        }



        public VLViewFilter AddFilter(VLView view, Int16 questionId, params VLFilterDetail[] details)
        {
            VLViewFilter _operator = new VLViewFilter();
            _operator.ViewId = view.ViewId;
            _operator.Survey = view.Survey;
            _operator.IsRule = true;
            _operator.Question = questionId;
            _operator.IsActive = true;
            foreach(var dtl in details)
            {
                _operator.FilterDetails.Add(dtl);
            }

            return AddFilterImpl(_operator);
        }
        public VLViewFilter AddFilter(VLView view, Int16 questionId, IList<VLFilterDetail> details)
        {
            VLViewFilter _operator = new VLViewFilter();
            _operator.ViewId = view.ViewId;
            _operator.Survey = view.Survey;
            _operator.IsRule = true;
            _operator.Question = questionId;
            _operator.IsActive = true;
            foreach (var dtl in details)
            {
                _operator.FilterDetails.Add(dtl);
            }

            return AddFilterImpl(_operator);
        }

        VLViewFilter AddFilterImpl(VLViewFilter filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            /*Υπάρχει η όψη για την οποία προορίζεται το φίλτρο:*/
            var existingView = SurveysDal.GetViewById(this.AccessTokenId, filter.ViewId);
            if(existingView == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "view", filter.ViewId));
            }
            /*Εχουμε σωστό ορισμένο survey επάνω στο φίλτρο:*/
            if(filter.Survey != existingView.Survey)
            {
                throw new VLException("Invalid filter.Survey!");
            }


            if (filter.IsRule == false)
            {
                /*Εχουμε σωστό LogicalOperator?*/
                if(filter.LogicalOperator.HasValue == false)
                {
                    throw new VLException("filter.LogicalOperator cannot be null!");
                }
                /*Πρέπει το FilterDetails να είναι άδειο:*/
                if(filter.FilterDetails.Count > 0 )
                {
                    throw new VLException("filter.FilterDetails must be empty!");
                }

                return SurveysDal.CreateViewFilter(this.AccessTokenId, filter);
            }

            /*Πρέπει να υπάρχει τουλάχιστον ένα FilterDetail:*/
            if(filter.FilterDetails.Count <= 0)
            {
                throw new VLException("filter.FilterDetails cannot be empty!");
            }

            /*διαβάζουε την ερώτηση απο το σύστημα:*/
            if (filter.Question.HasValue == false)
            {
                throw new VLException("filter.Question cannot be null!");
            }
            var question = SurveysDal.GetQuestionById(this.AccessTokenId, filter.Survey, filter.Question.Value, BuiltinLanguages.PrimaryLanguage);
            if (question == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Question", filter.Question.Value));
            }
            filter.QuestionType = question.QuestionType;

            /*Υποστηρίζουμε μέχρι και 12 filterDetails:*/
            if (filter.FilterDetails.Count > 12)
            {
                throw new VLException("Too mmany details. A filter can have up to 12 filter-details!");
            }
            /*Ελέγχομμε ένα-ένα τα φιλτρα:*/
            foreach(var detail in filter.FilterDetails)
            {
                ValidateFilterDetails(filter, question, detail);                
            }
            /*hints για την δημιουργία της SQL των φίλτρων:*/
            if(question.QuestionType == QuestionType.OneFromMany || question.QuestionType == QuestionType.DropDown)
            {
                filter.ORingDetails = true;
            }


            return SurveysDal.CreateViewFilter(this.AccessTokenId, filter);
        }

        void ValidateFilterDetails(VLViewFilter filter, VLSurveyQuestion question, VLFilterDetail detail)
        {
            ComparisonOperator _op = detail.Operator;
            Byte? option = detail.SelectedOption;
            Byte? column = detail.SelectedColumn;
            string input1 = detail.UserInput1;
            string input2 = detail.UserInput2;


            switch (question.QuestionType)
            {
                case QuestionType.SingleLine: case QuestionType.MultipleLine:
                    {
                        if(_op != ComparisonOperator.Equals && _op != ComparisonOperator.Like && _op != ComparisonOperator.StartsWith && _op != ComparisonOperator.EndsWith)
                            throw new VLException(string.Format("Operator '{0}', is not valid for a question of type '{1}'!", _op, question.QuestionType));
                        if (string.IsNullOrEmpty(input1))
                            throw new VLException("UserInput1 cannot be null or an empty string!");
                    }
                    break;
                case QuestionType.Integer:
                    {
                        if (_op != ComparisonOperator.Equals && _op != ComparisonOperator.Greater && _op != ComparisonOperator.Less && _op != ComparisonOperator.GreaterOrEqual && _op != ComparisonOperator.LessOrEqual && _op != ComparisonOperator.NotEqual && _op != ComparisonOperator.Between)
                            throw new VLException(string.Format("Operator '{0}', is not valid for a question of type 'Integer'!", _op));
                        
                        Int32 value1 = 0; Int32 value2 = 0;
                        if (!Int32.TryParse(input1, NumberStyles.Integer, CultureInfo.InvariantCulture, out value1))
                        {
                            throw new VLException(string.Format("UserInput1 '{0}' is not an integer!", input1));
                        }
                        if (_op == ComparisonOperator.Between)
                        {                            
                            if (!Int32.TryParse(input2, NumberStyles.Integer, CultureInfo.InvariantCulture, out value2))
                            {
                                throw new VLException(string.Format("UserInput2 '{0}' is not an integer!", input2));
                            }
                        }
                    }
                    break;
                case QuestionType.Decimal:
                    {
                        if (_op != ComparisonOperator.Equals && _op != ComparisonOperator.Greater && _op != ComparisonOperator.Less && _op != ComparisonOperator.GreaterOrEqual && _op != ComparisonOperator.LessOrEqual && _op != ComparisonOperator.NotEqual && _op != ComparisonOperator.Between)
                            throw new VLException(string.Format("Operator '{0}', is not valid for a question of type 'Decimal'!", _op));

                        Decimal value1 = 0; Decimal value2 = 0;
                        if (!Decimal.TryParse(input1, NumberStyles.Number, CultureInfo.InvariantCulture, out value1))
                        {
                            throw new VLException(string.Format("UserInput1 '{0}' is not an Decimal!", input1));
                        }
                        if (_op == ComparisonOperator.Between)
                        {
                            if (!Decimal.TryParse(input2, NumberStyles.Number, CultureInfo.InvariantCulture, out value2))
                            {
                                throw new VLException(string.Format("UserInput2 '{0}' is not an Decimal!", input2));
                            }
                        }
                    }
                    break;
                case QuestionType.Date:
                    {
                        if (_op != ComparisonOperator.Equals && _op != ComparisonOperator.Greater && _op != ComparisonOperator.Less && _op != ComparisonOperator.GreaterOrEqual && _op != ComparisonOperator.LessOrEqual && _op != ComparisonOperator.NotEqual && _op != ComparisonOperator.Between)
                            throw new VLException(string.Format("Operator '{0}', is not valid for a question of type 'Date'!", _op));

                        DateTime value1 = DateTime.MinValue; DateTime value2 = DateTime.MinValue;
                        if (!DateTime.TryParseExact(input1, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out value1))
                        {
                            throw new VLException(string.Format("UserInput1 '{0}' is not an proper Date (expected format is MM/dd/yyyy)!", input1));
                        }
                        if (_op == ComparisonOperator.Between)
                        {
                            if (!DateTime.TryParseExact(input2, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out value2))
                            {
                                throw new VLException(string.Format("UserInput2 '{0}' is not an proper Date (expected format is MM/dd/yyyy)!", input2));
                            }
                        }
                    }
                    break;
                case QuestionType.Time:
                    break;
                case QuestionType.DateTime:
                    break;
                case QuestionType.OneFromMany: case QuestionType.DropDown:
                    {
                        if (_op != ComparisonOperator.IsChecked && _op != ComparisonOperator.IsNotChecked)
                            throw new VLException(string.Format("Operator '{0}', is not valid for a question of type '{1}'!", _op, question.QuestionType));
                        if (option.HasValue == false)
                        {
                            throw new VLException("SelectedOption is invalid!");
                        }
                    }
                    break;
                case QuestionType.ManyFromMany:
                    {
                        if (_op != ComparisonOperator.IsChecked && _op != ComparisonOperator.IsNotChecked)
                            throw new VLException(string.Format("Operator '{0}', is not valid for a question of type '{1}'!", _op, question.QuestionType));
                        if (option.HasValue == false)
                        {
                            throw new VLException("SelectedOption is invalid!");
                        }
                    }
                    break;
                case QuestionType.Slider:
                    break;
                case QuestionType.MatrixOnePerRow: case QuestionType.MatrixManyPerRow:
                    {
                        if (_op != ComparisonOperator.IsChecked && _op != ComparisonOperator.IsNotChecked)
                            throw new VLException(string.Format("Operator '{0}', is not valid for a question of type '{1}'!", _op, question.QuestionType));
                        if (option.HasValue == false && column.HasValue == false)
                        {
                            throw new VLException("At least one of SelectedOption or SelectedColumn must have a value!");
                        }
                    }
                    break;
            }
        }

        public VLViewFilter AddLogicalOperator(VLView view, LogicalOperator logicalOperator)
        {
            VLViewFilter _operator = new VLViewFilter();
            _operator.ViewId = view.ViewId;
            _operator.Survey = view.Survey;
            _operator.LogicalOperator = logicalOperator;
            _operator.IsRule = false;
            _operator.Question = null;
            _operator.QuestionType = null;
            _operator.IsActive = true;
                        
            return SurveysDal.CreateViewFilter(this.AccessTokenId, _operator);
        }

        /// <summary>
        /// Διαγράφει το συγκεκριμένο ViewFilter
        /// <para></para>
        /// </summary>
        /// <param name="filter"></param>
        public void DeleteViewFilter(VLViewFilter filter)
        {
            if(filter == null) throw new ArgumentNullException("filter");
            DeleteViewFilter(filter.ViewId, filter.FilterId);
        }
        /// <summary>
        /// Διαγράφει το συγκεκριμένο ViewFilter
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="filterId"></param>
        public void DeleteViewFilter(Guid viewId, Int32 filterId)
        {

            SurveysDal.DeleteViewFilter(this.AccessTokenId, viewId, filterId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public void DeleteViewFiltersForView(VLView view)
        {
            SurveysDal.DeleteViewFiltersForView(this.AccessTokenId, view.ViewId);
        }


        /// <summary>
        /// Απενεργοποιεί το συγκεκριμένο φίλτρο
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public VLViewFilter DisableViewFilter(VLViewFilter filter)
        {
            if (!filter.IsActive)
                throw new VLException("ViewFilter is already disabled!");

            filter.IsActive = false;

            return SurveysDal.UpdateViewFilterQuick(this.AccessTokenId, filter);
        }
        /// <summary>
        /// Ενεργοποιεί το συγκεκριμένο φίλτρο
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public VLViewFilter EnableViewFilter(VLViewFilter filter)
        {
            if (filter.IsActive)
                throw new VLException("ViewFilter is already enabled!");

            filter.IsActive = true;

            return SurveysDal.UpdateViewFilterQuick(this.AccessTokenId, filter);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="enableCollectors"></param>
        /// <returns></returns>
        public VLView AddCollectorFilter(VLView view, Collection<Int32> enableCollectors)
        {
            if (view == null) throw new ArgumentNullException("view");

            var collectors = SurveysDal.GetViewCollectors(this.AccessTokenId, view.ViewId);
            foreach (var item in collectors)
            {
                if (enableCollectors.Contains(item.Collector))
                {
                    item.IncludeResponses = true;
                }
                else
                {
                    item.IncludeResponses = false;
                }
            }

            foreach (var item in collectors)
            {
                SurveysDal.UpdateViewCollector(this.AccessTokenId, item);
            }

            if (view.EnableFilteringByCollector == false || view.FilteringByCollectorInUse == false)
            {
                view.EnableFilteringByCollector = true;
                view.FilteringByCollectorInUse = true;
                view.FiltersVersion++;
                view = SurveysDal.UpdateView(this.AccessTokenId, view);
            }

            return view;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public VLView DeleteCollectorFilter(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");

            var collectors = SurveysDal.GetViewCollectors(this.AccessTokenId, view.ViewId);
            foreach (var item in collectors)
            {
                if (item.IncludeResponses == false)
                {
                    item.IncludeResponses = true;
                    SurveysDal.UpdateViewCollector(this.AccessTokenId, item);
                }
            }


            if (view.EnableFilteringByCollector == true || view.FilteringByCollectorInUse == true)
            {
                view.EnableFilteringByCollector = false;
                view.FilteringByCollectorInUse = false;
                view.FiltersVersion++;
                view = SurveysDal.UpdateView(this.AccessTokenId, view);
            }

            return view;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public VLView EnableFilteringByCollector(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");
            
            if (view.EnableFilteringByCollector == false)
            {
                view.EnableFilteringByCollector = true;
                view.FiltersVersion++;
                view = SurveysDal.UpdateView(this.AccessTokenId, view);
            }

            return view;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public VLView DisableFilteringByCollector(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");

            if (view.EnableFilteringByCollector == true)
            {
                view.EnableFilteringByCollector = false;
                view.FiltersVersion++;
                view = SurveysDal.UpdateView(this.AccessTokenId, view);
            }

            return view;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="totalResponseTimeOperator"></param>
        /// <param name="totalResponseTime"></param>
        /// <param name="totalResponseTimeUnit"></param>
        /// <returns></returns>
        public VLView AddResponseTimeFilter(VLView view, ResponseTimeOperator totalResponseTimeOperator, Int32 totalResponseTime, ResponseTimeUnit totalResponseTimeUnit)
        {
            if (view == null) throw new ArgumentNullException("view");
            
            view.TotalResponseTime = totalResponseTime;
            view.TotalResponseTimeOperator = totalResponseTimeOperator;
            view.TotalResponseTimeUnit = totalResponseTimeUnit;
            view.EnableFilteringByResponseTime = true;
            view.FilteringByResponseTimeInUse = true;
            view.FiltersVersion++;

            return SurveysDal.UpdateView(this.AccessTokenId, view);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public VLView DeleteResponseTimeFilter(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");

            if (view.EnableFilteringByResponseTime == true || view.FilteringByResponseTimeInUse == true)
            {
                view.EnableFilteringByResponseTime = false;
                view.FilteringByResponseTimeInUse = false;
                view.TotalResponseTime = null;
                view.TotalResponseTimeOperator = null;
                view.TotalResponseTimeUnit = null;
                view.FiltersVersion++;
                view = SurveysDal.UpdateView(this.AccessTokenId, view);
            }

            return view;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public VLView EnableFilteringByTotalResponseTime(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");

            if (view.EnableFilteringByResponseTime == false)
            {
                view.EnableFilteringByResponseTime = true;
                view.FiltersVersion++;
                view = SurveysDal.UpdateView(this.AccessTokenId, view);
            }

            return view;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public VLView DisableFilteringByTotalResponseTime(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");

            if (view.EnableFilteringByResponseTime == true)
            {
                view.EnableFilteringByResponseTime = false;
                view.FiltersVersion++;
                view = SurveysDal.UpdateView(this.AccessTokenId, view);
            }

            return view;
        }


        public VLView AddTimePeriodFilter(VLView view, DateTime timePeriodStart, DateTime timePeriodEnd)
        {
            if (view == null) throw new ArgumentNullException("view");

            if(timePeriodStart > timePeriodEnd)
            {
                throw new VLException("Invalid TimePeriodStart and/or TimePeriodEnd!");
            }

            view.TimePeriodStart = timePeriodStart;
            view.TimePeriodEnd = timePeriodEnd;
            view.EnableFilteringByTimePeriod = true;
            view.FilteringByTimePeriodInUse = true;
            view.FiltersVersion++;

            return SurveysDal.UpdateView(this.AccessTokenId, view);
        }
        public VLView DeleteTimePeriodFilter(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");


            if (view.EnableFilteringByTimePeriod == true || view.FilteringByTimePeriodInUse == true)
            {
                view.EnableFilteringByTimePeriod = false;
                view.FilteringByTimePeriodInUse = false;
                view.TimePeriodStart = null;
                view.TimePeriodEnd = null;
                view.FiltersVersion++;
                view = SurveysDal.UpdateView(this.AccessTokenId, view);
            }
            return view;
        }
        public VLView EnableFilteringByTimePeriod(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");

            if (view.EnableFilteringByTimePeriod == false)
            {
                view.EnableFilteringByTimePeriod = true;
                view.FiltersVersion++;
                view = SurveysDal.UpdateView(this.AccessTokenId, view);
            }

            return view;
        }
        public VLView DisableFilteringByTimePeriod(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");

            if (view.EnableFilteringByTimePeriod == true)
            {
                view.EnableFilteringByTimePeriod = false;
                view.FiltersVersion++;
                view = SurveysDal.UpdateView(this.AccessTokenId, view);
            }

            return view;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="enableWidget"></param>
        /// <returns></returns>
        public VLView EnablePartialShow(VLView view, bool enableWidget = true)
        {
            if (view.EnablePartialShow == true)
                throw new VLException("EnablePartialShow is already enabled!");

            view.EnablePartialShow = true;
            if (enableWidget == true)
            {
                view.PartialShowInUse = true;
                view.FiltersVersion++;
            }

            return SurveysDal.UpdateView(this.AccessTokenId, view);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="disableWidget"></param>
        /// <returns></returns>
        public VLView DisablePartialShow(VLView view, bool disableWidget = false)
        {
            view.EnablePartialShow = false;
            if (disableWidget == true)
            {
                view.PartialShowInUse = false;
                view.FiltersVersion++;
            }

            return SurveysDal.UpdateView(this.AccessTokenId, view);
        }




        /// <summary>
        /// Reverts the view to its original state.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public VLView RevertView(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");
            return RevertView(view.ViewId);
        }
        public VLView RevertView(Guid viewId)
        {
            var existingView = SurveysDal.GetViewById(this.AccessTokenId, viewId);
            if (existingView == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "View", viewId));

            //Απενεργοποιούμε όλα τα υπόλοιπα φίλτρα επάνω στην όψη:
            var collectors = SurveysDal.GetViewCollectors(this.AccessTokenId, existingView.ViewId);
            foreach (var item in collectors)
            {
                if (item.IncludeResponses == false)
                {
                    item.IncludeResponses = true;
                    SurveysDal.UpdateViewCollector(this.AccessTokenId, item);
                }
            }
            existingView.EnableFilteringByCollector = false;
            existingView.FilteringByCollectorInUse = false;
            existingView.FiltersVersion++;

            existingView.EnableFilteringByResponseTime = false;
            existingView.FilteringByResponseTimeInUse = false;
            existingView.TotalResponseTime = null;
            existingView.TotalResponseTimeOperator = null;
            existingView.TotalResponseTimeUnit = null;
            existingView.FiltersVersion++;

            existingView.EnableFilteringByTimePeriod = false;
            existingView.FilteringByTimePeriodInUse = false;
            existingView.TimePeriodStart = null;
            existingView.TimePeriodEnd = null;
            existingView.FiltersVersion++;

            existingView.EnablePartialShow = false;
            existingView.PartialShowInUse = false;
            existingView.FiltersVersion++;

            existingView = SurveysDal.UpdateView(this.AccessTokenId, existingView);

            //Διαγράφουμε όλα τα viewFilters:
            SurveysDal.DeleteViewFiltersForView(this.AccessTokenId, existingView.ViewId);

            //διαβάζουμε απο την βάση την όψη:
            return SurveysDal.GetViewById(this.AccessTokenId, existingView.ViewId);
        }
        #endregion

        #region VLResponse*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        internal Collection<string> MakeViewFilterStatements(VLView view, Collection<VLViewFilter> filters)
        {
            Collection<string> qfilters = new Collection<string>();

            /*εάν τα φίλτρα είναι άδεια, επιστρέφουμε το άδειο collection:*/
            if(filters == null || filters.Count == 0)
            {
                return qfilters;
            }


            StringBuilder sb = new StringBuilder("where (");
            Int32 stackedFilters = 0;
            bool prvFilterWasLogicalOperator = false;

            for (int index = 0; index < filters.Count; index++)
            {
                var filter = filters[index];

                if (filter.IsActive == false)
                    continue;

                if (filter.IsRule)
                {
                    if (stackedFilters > 0)
                    {
                        if (prvFilterWasLogicalOperator == false)
                        {
                            sb.Append(")");
                            qfilters.Add(sb.ToString());
                            sb.Clear();
                            sb.Append("where (");
                        }
                        stackedFilters = 0;
                    }

                    sb.AppendFormat("({0})", filter.ViewFilterSql);
                    stackedFilters++;
                    prvFilterWasLogicalOperator = false;
                }
                else
                {
                    if (stackedFilters > 0 && prvFilterWasLogicalOperator == false)
                    {
                        if (filter.LogicalOperator.Value == LogicalOperator.Or)
                            sb.Append(" or ");
                        else if (filter.LogicalOperator.Value == LogicalOperator.And)
                            sb.Append(" and ");

                        prvFilterWasLogicalOperator = true;
                    }
                }
            }
            if (stackedFilters > 0)
            {
                sb.Append(")");
                qfilters.Add(sb.ToString());
            }
            return qfilters;
        }

        /// <summary>
        /// Επιστρέφει ολα τα Responses σύμφωνα με το συγκεκριμένο view
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public Collection<VLResponse> GetViewResponses(Guid viewId)
        {
            var view = SurveysDal.GetViewById(this.AccessTokenId, viewId);
            if (view == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "View", viewId));

            if (view.NumberOfQuestionFilters > 0)
            {
                Collection<VLViewFilter> filters = SurveysDal.GetViewFilters(this.AccessTokenId, view.ViewId);
                Collection<string> qfilters = MakeViewFilterStatements(view, filters);

                return SurveysDal.GetViewReponses(this.AccessTokenId, view.Survey, view.ViewId, qfilters);
            }
            else
            {
                return SurveysDal.GetViewReponses(this.AccessTokenId, view.Survey, view.ViewId);
            }
        }
        /// <summary>
        /// Επιστρέφει ολα τα Responses σύμφωνα με το συγκεκριμένο view
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public Collection<VLResponse> GetViewResponses(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");

            //Τραβάμε απο την βάση μας το σύνολο των φίλτρων για την συγκεκριμένη όψη:
            Collection<VLViewFilter> filters = SurveysDal.GetViewFilters(this.AccessTokenId, view.ViewId);
            if(filters.Count > 0)
            {
                Collection<string> qfilters = MakeViewFilterStatements(view, filters);

                return SurveysDal.GetViewReponses(this.AccessTokenId, view.Survey, view.ViewId, qfilters);
            }
            else
            {
                return SurveysDal.GetViewReponses(this.AccessTokenId, view.Survey, view.ViewId);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public Int32 GetViewResponsesCount(Guid viewId)
        {
            var view = SurveysDal.GetViewById(this.AccessTokenId, viewId);
            if (view == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "View", viewId));

            if (view.NumberOfQuestionFilters > 0)
            {
                Collection<VLViewFilter> filters = SurveysDal.GetViewFilters(this.AccessTokenId, view.ViewId);
                Collection<string> qfilters = MakeViewFilterStatements(view, filters);

                return SurveysDal.GetViewReponsesCount(this.AccessTokenId, view.Survey, view.ViewId, qfilters);
            }
            else
            {
                return SurveysDal.GetViewReponsesCount(this.AccessTokenId, view.Survey, view.ViewId);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public Int32 GetViewResponsesCount(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");

            //Τραβάμε απο την βάση μας το σύνολο των φίλτρων για την συγκεκριμένη όψη:
            Collection<VLViewFilter> filters = SurveysDal.GetViewFilters(this.AccessTokenId, view.ViewId);
            if (filters.Count > 0)
            {
                Collection<string> qfilters = MakeViewFilterStatements(view, filters);

                return SurveysDal.GetViewReponsesCount(this.AccessTokenId, view.Survey, view.ViewId, qfilters);
            }
            else
            {
                return SurveysDal.GetViewReponsesCount(this.AccessTokenId, view.Survey, view.ViewId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        public Collection<VLResponse> GetViewResponses(Guid viewId, int pageIndex, int pageSize, ref int totalRows)
        {
            var view = SurveysDal.GetViewById(this.AccessTokenId, viewId);
            if (view == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "View", viewId));

            if (view.NumberOfQuestionFilters > 0)
            {
                Collection<VLViewFilter> filters = SurveysDal.GetViewFilters(this.AccessTokenId, view.ViewId);
                Collection<string> qfilters = MakeViewFilterStatements(view, filters);

                return SurveysDal.GetViewReponses(this.AccessTokenId, view.Survey, view.ViewId, pageIndex, pageSize, ref totalRows, qfilters);
            }
            else
            {
                return SurveysDal.GetViewReponses(this.AccessTokenId, view.Survey, view.ViewId, pageIndex, pageSize, ref totalRows);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        public Collection<VLResponse> GetViewResponses(VLView view, int pageIndex, int pageSize, ref int totalRows)
        {
            if (view == null) throw new ArgumentNullException("view");

            //Τραβάμε απο την βάση μας το σύνολο των φίλτρων για την συγκεκριμένη όψη:
            Collection<VLViewFilter> filters = SurveysDal.GetViewFilters(this.AccessTokenId, view.ViewId);
            if (filters.Count > 0)
            {
                Collection<string> qfilters = MakeViewFilterStatements(view, filters);

                return SurveysDal.GetViewReponses(this.AccessTokenId, view.Survey, view.ViewId, pageIndex, pageSize, ref totalRows, qfilters);
            }
            else
            {
                return SurveysDal.GetViewReponses(this.AccessTokenId, view.Survey, view.ViewId, pageIndex, pageSize, ref totalRows);
            }
        }

        public VLSummaryEx GetViewSummary(VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");
            return GetViewSummaryEx(view.ViewId);
        }
        public VLSummaryEx GetViewSummaryEx(Guid viewId)
        {
            var view = SurveysDal.GetViewById(this.AccessTokenId, viewId);
            if (view == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "View", viewId));

            if (view.NumberOfQuestionFilters > 0)
            {
                Collection<VLViewFilter> filters = SurveysDal.GetViewFilters(this.AccessTokenId, view.ViewId);
                Collection<string> qfilters = MakeViewFilterStatements(view, filters);

                return SurveysDal.GetViewSummaryEx(this.AccessTokenId, view.Survey, view.ViewId, qfilters);
            }
            else
            {
                return SurveysDal.GetViewSummaryEx(this.AccessTokenId, view.Survey, view.ViewId);
            }
        }
        #endregion

        /// <summary>
        /// Μετράει το σύνολο των διαθέσιμων credits για τις δοθείσες collectorPayments
        /// </summary>
        /// <param name="collectorPayments"></param>
        /// <param name="requiredResource"></param>
        /// <returns></returns>
        Int32 CountAvailableCreditsForCollectorPayments(Collection<VLCollectorPayment> collectorPayments, CreditType requiredResource, string message)
        {
            Int32 totalUnits = 0;

            foreach (var cp in collectorPayments)
            {
                var payment = SystemDal.GetPaymentById(this.AccessTokenId, cp.Payment);
                if (payment == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Payment", cp.Payment));

                if (payment.CreditType != requiredResource)
                {
                    var _message = string.Format("{0} Payment type is '{1}', but this Collector needs payments of type '{2}'.", message, payment.CreditType, requiredResource);
                    throw new VLException(_message);
                }


                var availableUnits = payment.Quantity - payment.QuantityUsed;
                if (cp.QuantityLimit.HasValue == false)
                {
                    totalUnits += availableUnits;
                }
                else
                {
                    if (cp.QuantityLimit >= availableUnits)
                    {
                        totalUnits += availableUnits;
                    }
                    else
                    {
                        totalUnits += cp.QuantityLimit.Value;
                    }
                }
            }

            return totalUnits;
        }




        /// <summary>
        /// 'All Responses Data' exports organize survey results by respondent. The XLS export 
        /// includes a spreadsheet where each row contains the answers from a given respondent, 
        /// allowing you to do your own analysis in Excel. The question type determines how the 
        /// data is reported in the spreadsheet.
        /// </summary>
        /// <param name="surveyId"></param>
        public VLSurvey ExportAllResponsesAsXls(int surveyId)
        {
            Func<VLSurvey, string> _GetFilePath = delegate(VLSurvey survey)
            {
                string rootDirectory = ValisSystem.Core.FileInventory.Path;
                return Path.Combine(Path.Combine(rootDirectory, survey.AllResponsesXlsxExportPath), survey.AllResponsesXlsxExportName);
            };

            try
            {
                /*Διαβάζουμε το survey απο το σύστημα:*/
                var survey = SurveysDal.GetSurveyById(this.AccessTokenId, surveyId, BuiltinLanguages.PrimaryLanguage);
                if (survey == null)
                    return null;


                /*check for the existance of the file:*/
                if (survey.AllResponsesXlsxExportIsValid)
                {
                    FileInfo fileInfo = new FileInfo(_GetFilePath(survey));
                    if (fileInfo.Exists == false)
                    {
                        survey.AllResponsesXlsxExportIsValid = false;
                    }
                }

                if(!survey.AllResponsesXlsxExportIsValid)
                {
                    /*Πρέπει να δημιουργήσουμε το export ξανά!*/

                    if (string.IsNullOrWhiteSpace(survey.AllResponsesXlsxExportName) == false && string.IsNullOrWhiteSpace(survey.AllResponsesXlsxExportPath) == false)
                    {
                        #region erase previous report
                        try
                        {
                            FileInfo fileInfo = new FileInfo(_GetFilePath(survey));
                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }

                        }
                        catch (Exception ex)
                        {
                            var message = string.Format("An exception occured while calling ExportAllResponsesAsXls #FileInfo.Delete, AccessTokenId={0}, surveyId={1}", this.AccessTokenId, surveyId);
                            Logger.Warn(message, ex);
                        }
                        finally
                        {
                            survey.AllResponsesXlsxExportPath = null;
                            survey.AllResponsesXlsxExportName = null;
                        }
                        #endregion
                    }

                    /*Θέλουμε όλες τις ερωτήσεις (+options, + columns) για αυτό το survey:*/
                    var questions = SurveysDal.GetQuestionExsForSurvey(this.AccessTokenId, survey.SurveyId, null, survey.TextsLanguage);

                    /*Θέλουμε όλα τα Ορατά responses για αυτό το Survey:*/
                    var responses = SurveysDal.GetPaidResponseExs(this.AccessTokenId, surveyId);

                    survey.AllResponsesXlsxExportCreationDt = DateTime.Now;
                    survey.AllResponsesXlsxExportPath = Path.Combine(Path.Combine(survey.Client.ToString(CultureInfo.InvariantCulture), surveyId.ToString(CultureInfo.InvariantCulture)), "reports");
                    survey.AllResponsesXlsxExportName = "AllResponses_" + survey.PublicId + ".xlsx";

                    /*Πρέπει να φτιάξουμε το directory, εάν δεν υπάρχει*/
                    try
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(_GetFilePath(survey)));
                        if (!dirInfo.Exists)
                        {
                            dirInfo.Create();
                        }

                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("An exception occured while calling ExportAllResponsesAsXls #DirectoryInfo.Create, AccessTokenId={0}, surveyId={1}", this.AccessTokenId, surveyId);
                        Logger.Warn(message, ex);
                        throw;
                    }

                    AllResponsesXlsExport.CreateExcelDocument(this, survey, questions, responses, _GetFilePath(survey));

                    survey.AllResponsesXlsxExportIsValid = true;
                    survey = SurveysDal.UpdateSurvey(this.AccessTokenId, survey);
                }

                return survey;
            }
            catch (VLException ex)
            {
                var message = string.Format("An exception occured while calling ExportAllResponsesAsXls, AccessTokenId={0}, surveyId={1}", this.AccessTokenId, surveyId);
                Logger.Error(string.Format("RefId={0}, {1}", ex.ReferenceId, message), ex);
                throw new VLException(ex.Message, ex.ReferenceId);
            }
            catch (Exception ex)
            {
                var message = string.Format("An exception occured while calling ExportAllResponsesAsXls, AccessTokenId={0}, surveyId={1}", this.AccessTokenId, surveyId);
                var nex = new VLException(message, ex);
                Logger.Error(string.Format("RefId={0}, {1}", nex.ReferenceId, message), ex);
                throw nex;
            }
        }

        /// <summary>
        /// Επιστρέφει πίσω όλα τα στατιστικά στοιχεία για το dashboard του
        /// συγκεκριμένου πελάτη.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public VLClientDashboard GetClientDashboard(Int32 clientId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {

            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException("Invalid clientId!!");
                }
            }
            #endregion

            return SurveysDal.GetClientDashboard(this.AccessTokenId, clientId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public VLSystemDashboard GetSystemDashboard()
        {
            #region SecurityLayer
            if (this.PrincipalType != Core.PrincipalType.SystemUser)
            {
                throw new VLAccessDeniedException("Anauthorized call!");
            }
            #endregion

            return SurveysDal.GetSystemDashboard(this.AccessTokenId);
        }


    }
}
