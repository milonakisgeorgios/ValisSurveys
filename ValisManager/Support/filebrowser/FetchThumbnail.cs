using System;
using System.Web;
using System.Web.SessionState;
using Valis.Core;

namespace ValisManager.Support.filebrowser
{
    public class FetchThumbnail : WebApiHandler, IRequiresSessionState
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);

            try
            {
                //Παίρνουμε το FileId απο το Request
                Guid fileId = TryParseGuid(context, "fileid");
                Int32 width = TryParseInt32(context, "width", false, 104);
                Int32 height = TryParseInt32(context, "height", false, 80);

                #region Cache Hook
                #endregion


                //τραβάμε τα στοιχεία του αρχείου απο το σύστημα
                var file = surveyManager.GetFileById(fileId);
                if (file == null)
                {
                    return;
                }

                System.Byte[] _thumbnail = surveyManager.GetThumbnail(file, width, height);
                if (_thumbnail != null)
                {
                    #region Cache Hook

                    #endregion

                    Response.ClearHeaders();
                    Response.Clear();
                    Response.Cache.SetExpires(DateTime.Now.AddDays(1));
                    Response.Cache.SetCacheability(HttpCacheability.Public);
                    Response.Cache.SetValidUntilExpires(true);
                    Response.ContentType = GetMimeType(file.OriginalFileName);

                    Response.BinaryWrite(_thumbnail);
                    //Response.End();/*It throws ThreadAbortException */
                    context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    //Response.Clear();
                    ////Response.CacheControl = "Public";
                    //Response.Cache.SetExpires(DateTime.Now.AddDays(1));
                    //Response.Cache.SetCacheability(HttpCacheability.Public);
                    //Response.Cache.SetValidUntilExpires(true);
                    //string image = Globals.GetImageForMimeType(managedFile.Mime, true);
                    //Response.WriteFile(context.Server.MapPath(string.Format("{0}/ecms/images/", Request.ApplicationPath).Replace("//", "/")) + image);
                    ////Response.End();/*It throws ThreadAbortException */
                    //context.ApplicationInstance.CompleteRequest();
                }

            }
            catch(Exception)
            {
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}