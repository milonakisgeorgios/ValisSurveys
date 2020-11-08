using System;
using System.Web;

namespace ValisManager.Support
{
    public class ValisWebApiModule : IHttpModule
    {
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.PostResolveRequestCache += OnPostResolveRequestCache;
        }


        private void OnPostResolveRequestCache(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpRequest request = application.Request;
            HttpContext context = application.Context;


            if (request.Url.AbsolutePath.StartsWith("/services/api/", StringComparison.OrdinalIgnoreCase))
            {
                System.Diagnostics.Debug.WriteLine(string.Format("(OnPostResolveRequestCache) AbsolutePath = {0}", request.Url.AbsolutePath));

                #region filebrowser
                if (request.Url.AbsolutePath.StartsWith("/services/api/filebrowser/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/filebrowser/FetchFile", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.filebrowser.FetchFile());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/filebrowser/FetchFileEx", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.filebrowser.FetchFileEx());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/filebrowser/FetchThumbnail", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.filebrowser.FetchThumbnail());
                        return;
                    }
                }
                #endregion


                #region ClientLists
                if (request.Url.AbsolutePath.StartsWith("/services/api/ClientLists/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/ClientLists/GetPage", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ClientLists.GetPage());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ClientLists/Create", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ClientLists.Create());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ClientLists/Delete", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ClientLists.Delete());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ClientLists/GetById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ClientLists.GetById());
                        return;
                    }
                }
                #endregion

                
                #region ClientLists
                if (request.Url.AbsolutePath.StartsWith("/services/api/ClientLists/Contacts/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/ClientLists/Contacts/Delete", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ClientLists.Contacts.Delete());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ClientLists/Contacts/GetById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ClientLists.Contacts.GetById());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ClientLists/Contacts/GetPage", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ClientLists.Contacts.GetPage());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ClientLists/Contacts/Update", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ClientLists.Contacts.Update());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ClientLists/Contacts/Create", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ClientLists.Contacts.Create());
                        return;
                    }
                }
                #endregion

                #region Client
                if (request.Url.AbsolutePath.StartsWith("/services/api/Clients/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/Clients/GetClients", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Clients.GetClients());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Clients/GetClientUsers", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Clients.GetClientUsers());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Clients/UnlockUser", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Clients.UnlockUser());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Clients/SetNewPswd", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Clients.SetNewPswd());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Clients/GetCharges", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Clients.GetCharges());
                        return;
                    }
                }
                #endregion

                #region Payments
                if (request.Url.AbsolutePath.StartsWith("/services/api/Payments/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/Payments/GetPayments", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Payments.GetPayments());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Payments/GetChargesForCollector", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Payments.GetChargesForCollector());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Payments/GetPaymentInfoForCollectorPayment", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Payments.GetPaymentInfoForCollectorPayment());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Payments/RemoveFromCollector", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Payments.RemoveFromCollector());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Payments/GetPaymentsView1", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Payments.GetPaymentsView1());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Payments/GetChargesForSubgrid", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Payments.GetChargesForSubgrid());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Payments/GetCharges", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Payments.GetCharges());
                        return;
                    }
                }
                #endregion

                #region Profiles
                if (request.Url.AbsolutePath.StartsWith("/services/api/Profiles/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/Profiles/GetProfiles", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Profiles.GetProfiles());
                        return;
                    }
                }
                #endregion

                #region Roles
                if (request.Url.AbsolutePath.StartsWith("/services/api/Roles/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/Roles/GetRoles", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Roles.GetRoles());
                        return;
                    }
                }
                #endregion

                #region SystemUser
                if (request.Url.AbsolutePath.StartsWith("/services/api/SystemUser/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/SystemUser/GetSystemUsers", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SystemUser.GetSystemUsers());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SystemUser/UnlockUser", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SystemUser.UnlockUser());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SystemUser/SetNewPswd", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SystemUser.SetNewPswd());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SystemUser/ChangePassword", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SystemUser.ChangePassword());
                        return;
                    }
                }
                #endregion

                #region VLSurvey
                if (request.Url.AbsolutePath.StartsWith("/services/api/Surveys/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/Surveys/ClientGetAll", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Surveys.ClientGetAll());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Surveys/Delete", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Surveys.Delete());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Surveys/GetById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Surveys.GetById());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Surveys/AddLanguage", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Surveys.AddLanguage());
                        return;
                    }
                }
                #endregion

                #region VLSurveyPage
                if (request.Url.AbsolutePath.StartsWith("/services/api/SurveyPages/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyPages/Create", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyPages.Create());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyPages/GetDeleteOptions", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyPages.GetDeleteOptions());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyPages/Delete", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyPages.Delete());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyPages/GetAll", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyPages.GetAll());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyPages/GetById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyPages.GetById());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyPages/Update", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyPages.Update());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyPages/GetCandidateSkipToPages", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyPages.GetCandidateSkipToPages());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyPages/SetSkipLogic", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyPages.SetSkipLogic());
                        return;
                    }
                }
                #endregion

                #region VLSurveyQuestion
                if (request.Url.AbsolutePath.StartsWith("/services/api/SurveyQuestions/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/Create", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.Create());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/GetDeleteOptions", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.GetDeleteOptions());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/Delete", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.Delete());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/GetById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.GetById());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/GetByIdForEdit", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.GetByIdForEdit());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/Update", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.Update());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/GetOptions", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.GetOptions());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/GetCandidateSkipToPages", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.GetCandidateSkipToPages());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/GetQuestionsForPage", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.GetQuestionsForPage());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/SetSkipLogic", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.SetSkipLogic());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/GetByIdForSkipLogic", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.GetByIdForSkipLogic());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/SurveyQuestions/AddLibraryQuestion", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.SurveyQuestions.AddLibraryQuestion());
                        return;
                    }

                }
                #endregion

                #region VLCollector
                if (request.Url.AbsolutePath.StartsWith("/services/api/Collectors/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/Collectors/GetAll", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Collectors.GetAll());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Collectors/GetById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Collectors.GetById());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Collectors/Delete", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Collectors.Delete());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Collectors/Close", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Collectors.Close());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Collectors/Open", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Collectors.Open());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Collectors/UpdateName", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Collectors.UpdateName());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Collectors/ClearResponses", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Collectors.ClearResponses());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Collectors/GetChargedCollectors", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Collectors.GetChargedCollectors());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Collectors/GetCharges", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Collectors.GetCharges());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Collectors/VerifySenderAddress", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Collectors.VerifySenderAddress());
                        return;
                    }

                }
                #endregion

                #region VLRecipient
                if (request.Url.AbsolutePath.StartsWith("/services/api/Recipients/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/Recipients/GetAll", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Recipients.GetAll());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Recipients/GetById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Recipients.GetById());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Recipients/Remove", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Recipients.Remove());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Recipients/GetSurveyRuntimeURL", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Recipients.GetSurveyRuntimeURL());
                        return;
                    }
                }
                #endregion

                #region VLMessages
                if (request.Url.AbsolutePath.StartsWith("/services/api/Messages/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/Messages/GetReadyMessages", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Messages.GetReadyMessages());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Messages/GetDraftMessages", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Messages.GetDraftMessages());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Messages/GetById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Messages.GetById());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Messages/Delete", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Messages.Delete());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Messages/UnSchedule", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Messages.UnSchedule());
                        return;
                    }
                }
                #endregion

                #region ViewFilters
                if (request.Url.AbsolutePath.StartsWith("/services/api/ViewFilters/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/AddQnaFilterWithOptions", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.AddQnaFilterWithOptions());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/AddQnaFilterWithOptionsAndColumns", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.AddQnaFilterWithOptionsAndColumns());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/AddQnaFilterWithUserInputs", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.AddQnaFilterWithUserInputs());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/DeleteQnaFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.DeleteQnaFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/DisableQnaFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.DisableQnaFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/EnableQnaFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.EnableQnaFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/GetQnaFilterById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.GetQnaFilterById());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/AddCollectorsFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.AddCollectorsFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/DeleteCollectorsFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.DeleteCollectorsFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/DisableCollectorsFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.DisableCollectorsFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/EnableCollectorsFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.EnableCollectorsFilter());
                        return;
                    }


                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/AddResponseTimeFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.AddResponseTimeFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/DeleteResponseTimeFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.DeleteResponseTimeFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/EnableResponseTimeFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.EnableResponseTimeFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/DisableResponseTimeFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.DisableResponseTimeFilter());
                        return;
                    }


                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/AddTimePeriodFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.AddTimePeriodFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/DeleteTimePeriodFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.DeleteTimePeriodFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/EnableTimePeriodFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.EnableTimePeriodFilter());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/ViewFilters/DisableTimePeriodFilter", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.ViewFilters.DisableTimePeriodFilter());
                        return;
                    }
                }
                #endregion

                #region Analysis
                if (request.Url.AbsolutePath.StartsWith("/services/api/Analysis/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/Analysis/SetChartType", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Analysis.SetChartType());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Analysis/SwitchAxisScale", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Analysis.SwitchAxisScale());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Analysis/ToggleChartVisibility", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Analysis.ToggleChartVisibility());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Analysis/ToggleDataTableVisibility", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Analysis.ToggleDataTableVisibility());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Analysis/ToggleZeroResponseOptionsVisibility", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Analysis.ToggleZeroResponseOptionsVisibility());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Analysis/ExportSummaryPDF", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Analysis.ExportSummaryPDF());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Analysis/ExportAllDataXLSX", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Analysis.ExportAllDataXLSX());
                        return;
                    }
                }
                #endregion

                #region Logins
                if (request.Url.AbsolutePath.StartsWith("/services/api/Logins/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/Logins/GetLogins", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Logins.GetLogins());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Logins/GetById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Logins.GetById());
                        return;
                    }
                }
                #endregion

                #region Statistics
                if (request.Url.AbsolutePath.StartsWith("/services/api/Statistics/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/Statistics/GetClientDashboard", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Statistics.GetClientDashboard());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/Statistics/GetSystemDashboard", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.Statistics.GetSystemDashboard());
                        return;
                    }
                }
                #endregion

                #region LibraryQuestions
                if (request.Url.AbsolutePath.StartsWith("/services/api/LibraryQuestions/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/GetQuestions", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.GetQuestions());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/CreateQuestion", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.CreateQuestion());
                        return;
                    }

                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/GetOptions", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.GetOptions());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/CreateOption", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.CreateOption());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/DeleteOption", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.DeleteOption());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/GetOptionById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.GetOptionById());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/UpdateOption", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.UpdateOption());
                        return;
                    }

                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/GetColumns", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.GetColumns());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/CreateColumn", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.CreateColumn());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/DeleteColumn", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.DeleteColumn());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/GetColumnById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.GetColumnById());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/LibraryQuestions/UpdateColumn", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.LibraryQuestions.UpdateColumn());
                        return;
                    }
                }
                #endregion

                #region
                if (request.Url.AbsolutePath.StartsWith("/services/api/KnownEmails/", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(request.Url.AbsolutePath, "/services/api/KnownEmails/AddBounced", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.KnownEmails.AddBounced());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/KnownEmails/AddOptedOut", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.KnownEmails.AddOptedOut());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/KnownEmails/AddVerified", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.KnownEmails.AddVerified());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/KnownEmails/Delete", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.KnownEmails.Delete());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/KnownEmails/GetById", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.KnownEmails.GetById());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/KnownEmails/GetByAddress", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.KnownEmails.GetByAddress());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/KnownEmails/GetPage", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.KnownEmails.GetPage());
                        return;
                    }
                    else if (string.Equals(request.Url.AbsolutePath, "/services/api/KnownEmails/Update", StringComparison.OrdinalIgnoreCase))
                    {
                        application.Context.RemapHandler(new ValisManager.Support.WebApi.KnownEmails.Update());
                        return;
                    }
                }
                #endregion
            }


        }


    }
}