using ARSoft.Tools.Net;
using ARSoft.Tools.Net.Dns;
using CsvHelper;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Valis.Core.ViewModel;

namespace Valis.Core
{
    public sealed class VLSystemManager : VLManagerBase
    {
        static ILog Logger = LogManager.GetLogger(typeof(VLSystemManager));

        #region support stuff
        private VLSystemManager(IAccessToken accessToken) : base(accessToken) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static VLSystemManager GetAnInstance(IAccessToken accessToken)
        {
            var instance = new VLSystemManager(accessToken);
            instance.TheSystem = instance;
            return instance;
        }
        #endregion


        #region VLSystemParameter

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Collection<VLSystemParameter> GetSystemParameters()
        {
            #region SecurityLayer

            #endregion

            return SystemDal.GetSystemParameters(AccessTokenId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        public Collection<VLSystemParameter> GetSystemParameters(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer

            #endregion

            return SystemDal.GetSystemParameters(AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public VLSystemParameter GetSystemParameterByKey(string dataKey)
        {
            VLSystemParameter.ValidateParameterKey(ref dataKey);

            #region SecurityLayer

            #endregion

            return SystemDal.GetSystemParameterByKey(AccessTokenId, dataKey);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public VLSystemParameter GetSystemParameterById(Guid parameterId)
        {
            var value = SystemDal.GetSystemParameterById(AccessTokenId, parameterId);


            #region SecurityLayer

            #endregion


            return value;
        }


        /// <summary>
        /// Updates an existing SystemParameter or creates a new one 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public VLSystemParameter SetSystemParameter(string name, string value)
        {
            var parameter = new VLSystemParameter();
            parameter.ParameterKey = name;
            parameter.ParameterValue = value;
            parameter.ParameterType = VLParameterType.StringType;
            return CreateOrUpdateParameter(parameter);
        }
        /// <summary>
        /// Updates an existing SystemParameter or creates a new one 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public VLSystemParameter SetSystemParameter(string name, int value)
        {
            var parameter = new VLSystemParameter();
            parameter.ParameterKey = name;
            parameter.ParameterValue = value.ToString(CultureInfo.InvariantCulture);
            parameter.ParameterType = VLParameterType.Int32Type;
            return CreateOrUpdateParameter(parameter);
        }
        /// <summary>
        /// Updates an existing SystemParameter or creates a new one 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public VLSystemParameter SetSystemParameter(string name, double value)
        {
            var parameter = new VLSystemParameter();
            parameter.ParameterKey = name;
            parameter.ParameterValue = value.ToString(CultureInfo.InvariantCulture);
            parameter.ParameterType = VLParameterType.NumberType;
            return CreateOrUpdateParameter(parameter);
        }
        /// <summary>
        /// Updates an existing SystemParameter or creates a new one 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public VLSystemParameter SetSystemParameter(string name, Guid value)
        {
            var parameter = new VLSystemParameter();
            parameter.ParameterKey = name;
            parameter.ParameterValue = value.ToString();
            parameter.ParameterType = VLParameterType.GuidType;
            return CreateOrUpdateParameter(parameter);
        }
        /// <summary>
        /// Updates an existing SystemParameter or creates a new one 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public VLSystemParameter SetSystemParameter(string name, DateTime value)
        {
            var parameter = new VLSystemParameter();
            parameter.ParameterKey = name;
            parameter.ParameterValue = value.ToString(CultureInfo.InvariantCulture);
            parameter.ParameterType = VLParameterType.DateType;
            return CreateOrUpdateParameter(parameter);
        }
        /// <summary>
        /// Updates an existing SystemParameter or creates a new one 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public VLSystemParameter SetSystemParameter(string name, Boolean value)
        {
            var parameter = new VLSystemParameter();
            parameter.ParameterKey = name;
            parameter.ParameterValue = value.ToString(CultureInfo.InvariantCulture);
            parameter.ParameterType = VLParameterType.BooleanType;
            return CreateOrUpdateParameter(parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal VLSystemParameter CreateOrUpdateParameter(VLSystemParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");
            parameter.ValidateInstance();
            parameter.ParameterKey = parameter.ParameterKey.ToLowerInvariant();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer);
            #endregion


            VLSystemParameter existingParameter = SystemDal.GetSystemParameterByKey(AccessTokenId, parameter.ParameterKey);
            /* Η DataValueDal.GetDataValueByName επιστρέφει ορατά στο Repository dataValues. 
             * Αυτό δεν σημαίνει ότι όλα ανήκουν στο Repository!
             */
            if (existingParameter != null)
            {
                if (existingParameter.IsBuiltIn)
                {
                    throw new VLException(SR.GetString(SR.You_cannot_update_builtin, "SystemParameter"));
                }

                existingParameter.ParameterValue = parameter.ParameterValue;
                existingParameter.ParameterType = parameter.ParameterType;
                existingParameter.IsHidden = parameter.IsHidden;
                existingParameter.IsBuiltIn = parameter.IsBuiltIn;

                return SystemDal.UpdateSystemParameter(AccessTokenId, existingParameter);
            }

            return CreateParameter(parameter);
        }
        /// <summary>
        /// Δημιουργεί ένα νέο system parameter στο σύστημα.
        /// <para>Απαιτείται δικαίωμα ManageRepository επάνω στο Repository ή ManageSystem επάνω στο σύστημα</para>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal VLSystemParameter CreateParameter(VLSystemParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");
            parameter.ValidateInstance();
            parameter.ParameterKey = parameter.ParameterKey.ToLowerInvariant();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer);
            #endregion


            VLSystemParameter existingParameter = SystemDal.GetSystemParameterByKey(AccessTokenId, parameter.ParameterKey);
            if (existingParameter != null)
            {
                throw new VLException("Already exists a parameter with the same key!");
            }

            return SystemDal.CreateSystemParameter(AccessTokenId, parameter);
        }
        /// <summary>
        /// Μεταβάλει τα στοιχεία του συγκεκριμένου system parameter.
        /// <para>Builtin system parameter μεταβάλλεται μόνο απο χρήστη που φέρει το δικαίωμα ManageSystem επάνω στο σύστημα</para>
        /// <para>Απαιτείται δικαίωμα ManageRepository επάνω στο Repository ή ManageSystem επάνω στο σύστημα</para>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal VLSystemParameter UpdateParameter(VLSystemParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");
            parameter.ValidateInstance();
            parameter.ParameterKey = parameter.ParameterKey.ToLowerInvariant();


            VLSystemParameter existingParameter = SystemDal.GetSystemParameterById(AccessTokenId, parameter.ParameterId);
            if (existingParameter == null)
                throw new VLException(string.Format(CultureInfo.InvariantCulture, "There is no SystemParameter with id = '{0}'", parameter.ParameterId));


            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer);
            #endregion

            if (existingParameter.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_update_builtin, "SystemParameter"));
            }

            existingParameter.ParameterValue = parameter.ParameterValue;
            existingParameter.ParameterType = parameter.ParameterType;
            existingParameter.IsHidden = parameter.IsHidden;
            existingParameter.IsBuiltIn = parameter.IsBuiltIn;

            return SystemDal.UpdateSystemParameter(AccessTokenId, existingParameter);
        }
        /// <summary>
        /// Διαγράφει ένα datavalue στο επίπεδο του συστηματος.
        /// <para>Builtin datavalue δεν είναι δυνατόν να διαγραφεί</para>
        /// <para>Απαιτείται δικαίωμα ManageSystem επάνω στο σύστημα</para>
        /// </summary>
        /// <param name="parameterKey"></param>
        public void DeleteSystemParameter(string parameterKey)
        {
            VLSystemParameter.ValidateParameterKey(ref parameterKey);

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer);
            #endregion

            var parameter = SystemDal.GetSystemParameterByKey(AccessTokenId, parameterKey);
            if (parameter == null)
                throw new VLException(string.Format(CultureInfo.InvariantCulture, "There is no Parameter with key='{0}'!", parameterKey));

            if (parameter.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_delete_builtin, "SystemParameter"));
            }

            SystemDal.DeleteSystemParameter(AccessTokenId, parameter.ParameterId, parameter.LastUpdateDT);
        }
        /// <summary>
        /// Διαγράφει το συγκεκριμένο datavalue απο το σύστημα ή τον repository (όπου ανήκει).
        /// <para>Builtin datavalue δεν είναι δυνατόν να διαγραφεί</para>
        /// <para>Απαιτείται δικαίωμα ManageRepository επάνω στο Repository ή ManageSystem επάνω στο σύστημα</para>
        /// </summary>
        /// <param name="parameterId"></param>
        public void DeleteSystemParameter(Guid parameterId)
        {
            var parameter = SystemDal.GetSystemParameterById(AccessTokenId, parameterId);
            if (parameter == null)
                throw new VLException(string.Format(CultureInfo.InvariantCulture, "There is no SystemParameter with id='{0}'!", parameterId));

            if (parameter.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_delete_builtin, "SystemParameter"));
            }

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer);
            #endregion

            SystemDal.DeleteSystemParameter(AccessTokenId, parameter.ParameterId, parameter.LastUpdateDT);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="overrideBuiltin"></param>
        internal void DeleteSystemParameters(IEnumerable<VLSystemParameter> values, bool overrideBuiltin = false)
        {
            if (values == null) throw new ArgumentNullException("values");


            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer);
            #endregion

            //Check first if all values are system values!
            foreach (var item in values)
            {
                if (!overrideBuiltin)
                {
                    if (item.IsBuiltIn)
                    {
                        throw new VLException(string.Format(CultureInfo.InvariantCulture, "Parameter with key = '{0}', is a builtin object. You cannot delete it!", item.ParameterKey));
                    }
                }
            }

            foreach (var item in values)
            {
                SystemDal.DeleteSystemParameter(AccessTokenId, item.ParameterId, item.LastUpdateDT);
            }
        }
        #endregion


        #region VLRole
        public Collection<VLRole> GetRoles(string whereClause = null, string orderByClause = "order by Name")
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetRoles(AccessTokenId, whereClause, orderByClause);
        }
        public Collection<VLRole> GetRoles(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = "order by Name")
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetRoles(AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        public VLRole GetRoleById(Int16 roleId)
        {
            var item = SystemDal.GetRoleById(AccessTokenId, roleId);

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return item;
        }
        public VLRole GetRoleByName(string name)
        {
            VLRole.ValidateName(ref name);

            var item = SystemDal.GetRoleByName(AccessTokenId, name);

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return item;
        }
        public VLRole CreateRole(string name, string description = null, VLPermissions permissions = VLPermissions.None, Boolean isClientRole = false)
        {
            var role = new VLRole();
            role.Name = name;
            role.Description = description;
            role.Permissions = permissions;
            role.IsBuiltIn = false;
            role.IsClientRole = isClientRole;
            return CreateRole(role);
        }
        internal VLRole CreateRole(VLRole role)
        {
            #region input parameters validation
            if (role == null) throw new ArgumentNullException("role");
            role.ValidateInstance();
            #endregion

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer);
            #endregion

            /*Check Name uniqueness*/
            var existingRole = SystemDal.GetRoleByName(AccessTokenId, role.Name);
            if (existingRole != null)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", role.Name));
            }

            /*a user cannot create a builtin role*/
            if(role.IsBuiltIn)
            {
                throw new VLException("You cannot create a builtin role!");
            }
            /*A user cannot set the following Permissions: */
            if ((role.Permissions & VLPermissions.ManageSystem) == VLPermissions.ManageSystem)
            {
                throw new VLException(SR.GetString(SR.You_cannot_set_the_XXX_permission, VLPermissions.ManageSystem.ToString()));
            }

            return SystemDal.CreateRole(AccessTokenId, role);
        }
        public VLRole UpdateRole(VLRole role)
        {
            #region input parameters validation
            if (role == null) throw new ArgumentNullException("role");
            role.ValidateInstance();
            #endregion

            var existingRole = SystemDal.GetRoleByName(AccessTokenId, role.Name);
            if (existingRole != null && existingRole.RoleId != role.RoleId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", role.Name));
            }
            if (existingRole == null)
            {
                existingRole = SystemDal.GetRoleById(AccessTokenId, role.RoleId);
            }
            if (existingRole == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "role", role.RoleId));



            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer);
            #endregion

            /*a user cannot update a builtin role:*/
            if (existingRole.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_update_builtin, "Role"));
            }

            /*A user cannot set the following Permissions: */
            if ((role.Permissions & VLPermissions.ManageSystem) == VLPermissions.ManageSystem)
            {
                throw new VLException(SR.GetString(SR.You_cannot_set_the_XXX_permission, VLPermissions.ManageSystem.ToString()));
            }


            existingRole.Name = role.Name;
            existingRole.Description = role.Description;
            existingRole.Permissions = role.Permissions;
            //existingRole.IsBuiltIn = role.IsBuiltIn;
            existingRole.IsClientRole = role.IsClientRole;


            return SystemDal.UpdateRole(AccessTokenId, existingRole);
        }
        public void DeleteRole(VLRole role)
        {
            if (role == null) throw new ArgumentNullException("role");
            DeleteRole(role.RoleId);
        }
        public void DeleteRole(Int16 roleId)
        {
            var role = SystemDal.GetRoleById(AccessTokenId, roleId);
            if (role == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "role", roleId));

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer);
            #endregion

            /*A user cannot delete a builtin role:*/
            if (role.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_delete_the_builtin_entity, "role", role.Name));
            }

            SystemDal.DeleteRole(AccessTokenId, roleId, role.LastUpdateDT);
        }
        #endregion


        #region VLCountry
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLCountry> GetCountries(string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SystemDal.GetCountries(AccessTokenId, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public VLCountry GetCountryByIf(Int32 countryId)
        {
            return SystemDal.GetCountryById(this.AccessTokenId, countryId);
        }
        #endregion


        #region VLEmailTemplate
        #endregion


        #region VLSystemEmail
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLSystemEmail> GetSystemEmails(string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion


            return SystemDal.GetSystemEmails(this.AccessTokenId, whereClause, orderByClause);
        }


        /// <summary>
        /// Επιστρέφει pending SystemEmails, τα οποία περιμένουν να αποσταλλούν.
        /// <para>Οσα pending SystemEmails επιστραφούν, αλλάζουν στην βάση το Status τους σε Executing και το πεδίο SendDT γίνεται update με την τρέχουσα ημερομηνία και ώρα που έγινε η αλλαγή!</para>
        /// </summary>
        /// <param name="maxRows"></param>
        /// <returns></returns>
        internal Collection<VLSystemEmail> GetPendingSystemEmails(Int32 maxRows = 12)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion


            return SystemDal.GetPendingSystemEmails(this.AccessTokenId, maxRows);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        internal Collection<VLSystemEmail> GetSystemEmails(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion

            return SystemDal.GetSystemEmails(this.AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        internal VLSystemEmail GetSystemEmailById(Int32 emailId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion

            return SystemDal.GetSystemEmailById(this.AccessTokenId, emailId);
        }

        /// <summary>
        /// Δημιουργεί ένα νέο SystemΕmail το οποίο θα αποσταλεί στο επόμενο διαθέσιμο time slot!
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        internal VLSystemEmail CreateSystemEmail(string moduleName, string fromAddress, string fromDisplayName, string toAddress, string subject, string body)
        {
            VLSystemEmail.ValidateModuleName(ref moduleName);
            VLSystemEmail.ValidateFromAddress(ref fromAddress);
            VLSystemEmail.ValidateFromDisplayName(ref fromDisplayName);
            VLSystemEmail.ValidateToAddress(ref toAddress);
            VLSystemEmail.ValidateSubject(ref subject);


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion


            return SystemDal.CreateSystemEmail(this.AccessTokenId, moduleName, fromAddress, fromDisplayName, toAddress, subject, body);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal VLSystemEmail UpdateSystemEmail(VLSystemEmail email)
        {
            if (email == null) throw new ArgumentNullException("email");
            email.ValidateInstance();

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion


            return SystemDal.UpdateSystemEmail(this.AccessTokenId, email);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailId"></param>
        public void DeleteSystemEmail(Int32 emailId)
        {
            var email = SystemDal.GetSystemEmailById(this.AccessTokenId, emailId);
            if (email == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "SystemEmail", emailId));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion

            SystemDal.DeleteSystemEmail(this.AccessTokenId, email.EmailId);
        }


        #endregion


        #region VLSystemUser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLSystemUser> GetSystemUsers(string whereClause = null, string orderByClause = "order by LastName, FirstName")
        {
            Utility.CheckParameter(ref whereClause, false, false, false, 2048, "whereClause");
            Utility.CheckParameter(ref orderByClause, false, false, false, 1024, "orderByClause");

            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetSystemUsers(AccessTokenId, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLSystemUser> GetSystemUsers(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = "order by LastName, FirstName")
        {
            Utility.CheckParameter(ref whereClause, false, false, false, 2048, "whereClause");
            Utility.CheckParameter(ref orderByClause, false, false, false, 1024, "orderByClause");

            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion


            return SystemDal.GetSystemUsers(AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        internal Collection<VLSystemUserView> GetSystemUserViews(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = "order by LastName, FirstName")
        {
            Utility.CheckParameter(ref whereClause, false, false, false, 2048, "whereClause");
            Utility.CheckParameter(ref orderByClause, false, false, false, 1024, "orderByClause");

            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
			#endregion


            return ViewModelDal.GetSystemUserViews(AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public int GetSystemUsersCount(string whereClause = null)
        {
            Utility.CheckParameter(ref whereClause, false, false, false, 2048, "whereClause");

            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetSystemUsersCount(AccessTokenId, whereClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public VLSystemUser GetSystemUserById(Int32 userId)
        {
            var item = SystemDal.GetSystemUserById(AccessTokenId, userId);

            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return item;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logOnToken"></param>
        /// <returns></returns>
        public VLSystemUser GetSystemUserByLogOnToken(string logOnToken)
        {
            VLCredential.ValidateLogOnToken(ref logOnToken);

            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetSystemUserByLogOnToken(AccessTokenId, logOnToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public VLSystemUser GetSystemUserByEmail(string email)
        {
            Utility.CheckParameter(ref email, true, true, true, 256, "email");

            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetSystemUserByEmail(AccessTokenId, email);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessTokenId"></param>
        /// <returns></returns>
        public VLSystemUser GetSystemUserFromAccessToken(Int32 accessTokenId)
        {
            var item = SystemDal.GetSystemUserFromAccessToken(AccessTokenId, accessTokenId);

            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return item;
        }
        /// <summary>
        /// Δημιουργεί ένα SystemUser αλλά χωρίς credentials, και άρα μη ενεργό
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="roleId"></param>
        /// <param name="email"></param>
        /// <param name="timezoneId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        internal VLSystemUser CreateSystemUser(string firstName, string lastName, Int16 roleId, string email, string timezoneId = null, string comment = null)
        {
            var user = new VLSystemUser();
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.Role = roleId;
            if (string.IsNullOrWhiteSpace(timezoneId))
            {
                var timezone = SystemDal.GetSystemParameterByKey(this.AccessTokenId, "DefaultTimeZoneId");
                if (timezone != null)
                {
                    user.TimeZoneId = timezone.ParameterValue;
                }
                else
                {
                    throw new VLException("Cannot find System's DefaultTimeZoneId!");
                }
            }
            else
            {
                user.TimeZoneId = timezoneId;
            }
            user.Notes = comment;

            return CreateSystemUser(user);
        }
        /// <summary>
        /// Δημιουργεί ένα SystemUser αλλά χωρίς credentials, και άρα μη ενεργό
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal VLSystemUser CreateSystemUser(VLSystemUser user)
        {
            #region input parameters validation
            if (user == null) throw new ArgumentNullException("user");
            user.ValidateInstance();
            #endregion


            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion


            /*Ελεγχος του ρόλου του SystemUser:*/
            if (user.Role == 0)
            {
                throw new ArgumentException("Invalid Role", "user.Role");
            }
            var role = SystemDal.GetRoleById(AccessTokenId, user.Role);
            if (role == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Role", user.Role));
            }
            /*Ελεγχος του email του SystemUser:*/
            if(!string.IsNullOrWhiteSpace(user.Email))
            {
                if(!Utility.EmailIsValid(user.Email))
                {
                    throw new VLException(string.Format("Invalid email address '{0}'!", user.Email));
                }
            }
            if(Utility.RequiresUniqueEmail)
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    throw new VLException("SystemUsers are required to have an email address!");
                }
                if(SystemDal.GetSystemUserByEmail(AccessTokenId, user.Email) != null)
                {
                    throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Email", user.Email));
                }
            }
            //Ελέγχουμε το timeZoneId:
            if (!string.IsNullOrWhiteSpace(user.TimeZoneId))
            {
                try
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);

                }
                catch (TimeZoneNotFoundException)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "TimeZone", user.TimeZoneId));
                }
                catch (InvalidTimeZoneException)
                {
                    throw new VLException("The time zone identifier was found but the registry data is corrupted!");
                }
                catch (Exception ex)
                {
                    throw new VLException("TimeZone cannot be validated!", ex);
                }
            }
            else
            {
                user.TimeZoneId = null;
            }

            user.IsBuiltIn = false;
            user.IsActive = false;
            user.DefaultLanguage = BuiltinLanguages.Greek.LanguageId;


            return SystemDal.CreateSystemUser(AccessTokenId, user);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public VLSystemUser UpdateSystemUser(VLSystemUser user)
        {
            #region input parameters validation
            if (user == null) throw new ArgumentNullException("user");
            user.ValidateInstance();
            #endregion


            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            var existingUser = SystemDal.GetSystemUserById(AccessTokenId, user.UserId);
            if (existingUser == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "SystemUser", user.UserId));

            if (existingUser.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_update_the_builtin_entity, "SystemUser", string.Format(CultureInfo.InvariantCulture, "{0}, {1}", user.LastName, user.FirstName)));
            }


            /* Εδώ θα κάνουμε έλεγχο του ρόλου. 
             * Ενας sysadmin, μπορεί να έχει έναν μόνο ρόλο, τον SysAdmin ρόλο.
             * Ενας οποισδήποτε άλλος χρήστες μπορεέ να έχει οποιονδήποτε άλλον ρόλο θέλουμε:
             */
            if (user.Role == 0)
            {
                throw new ArgumentException("Invalid Role", "user.Role");
            }
            var role = SystemDal.GetRoleById(AccessTokenId, user.Role);
            if (role == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Role", user.Role));
            }
            /*Ελεγχος του email του SystemUser:*/
            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                if (!Utility.EmailIsValid(user.Email))
                {
                    throw new VLException(string.Format("Invalid email address '{0}'!", user.Email));
                }
            }
            if (Utility.RequiresUniqueEmail)
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    throw new VLException("SystemUsers are required to have an email address!");
                }
                var _user = SystemDal.GetSystemUserByEmail(AccessTokenId, user.Email);
                if(_user != null && _user.UserId != user.UserId)
                {
                    throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Email", user.Email));
                }
            }
            //Ελέγχουμε το timeZoneId:
            if (!string.IsNullOrWhiteSpace(user.TimeZoneId))
            {
                try
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);

                }
                catch (TimeZoneNotFoundException)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "TimeZone", user.TimeZoneId));
                }
                catch (InvalidTimeZoneException)
                {
                    throw new VLException("The time zone identifier was found but the registry data is corrupted!");
                }
                catch (Exception ex)
                {
                    throw new VLException("TimeZone cannot be validated!", ex);
                }
            }
            else
            {
                user.TimeZoneId = null;
            }


            //UserId
            existingUser.DefaultLanguage = user.DefaultLanguage;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.TimeZoneId = user.TimeZoneId;
            existingUser.IsActive = user.IsActive;
            //existingUser.IsBuiltIn = user.IsBuiltIn;
            existingUser.AttributeFlags = user.AttributeFlags;
            existingUser.Role = user.Role;
            existingUser.Notes = user.Notes;
            //existingUser.LastActivityDate = user.LastActivityDate;


            return SystemDal.UpdateSystemUser(AccessTokenId, existingUser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void DeleteSystemUser(VLSystemUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            DeleteSystemUser(user.UserId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteSystemUser(Int32 userId)
        {
            /*
             * We have to exclude the case that a user is trying to delete itself.
             * It is not allowed
             */
            if (this.Principal == userId)
            {
                throw new VLException("A user cannot delete itself!");
            }

            var user = GetSystemUserById(userId);
            if (user == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "user", userId));

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            if (user.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_delete_the_builtin_entity, "user", string.Format(CultureInfo.InvariantCulture, "{0}, {1}", user.LastName, user.FirstName)));
            }


            SystemDal.DeleteSystemUser(AccessTokenId, user.UserId, user.LastUpdateDT);
        }
        #endregion


        #region VLCredential 
        public VLCredential GetCredentialForPrincipal(VLClientUser principal)
        {
            if (principal == null) throw new ArgumentNullException("principal");
            return GetCredentialForPrincipal(PrincipalType.ClientUser, principal.UserId);
        }
        public VLCredential GetCredentialForPrincipal(VLSystemUser principal)
        {
            if (principal == null) throw new ArgumentNullException("principal");
            return GetCredentialForPrincipal(PrincipalType.SystemUser, principal.UserId);
        }
        public VLCredential GetCredentialForPrincipal(PrincipalType principalType, Int32 principalId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                /*Ενα clientAccount, μπορεί μόνο για τον εαύτό του να ανακτήσει το VLCredential:*/
                if (principalType != PrincipalType.ClientUser || principalId != this.Principal)
                {
                    throw new VLAccessDeniedException();
                }
            }
            #endregion

            return SystemDal.GetCredentialForPrincipal(AccessTokenId, principalType, principalId);
        }


        internal VLCredential GetCredentialById(Int32 credentialId)
        {
            var _credentials = SystemDal.GetCredentialById(AccessTokenId, credentialId);

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                /*Ενα clientAccount, μπορεί μόνο για τον εαύτό του να ανακτήσει το VLCredential:*/
                if (_credentials.PrincipalType != PrincipalType.ClientUser || _credentials.Principal != this.Principal)
                {
                    throw new VLAccessDeniedException();
                }
            }
            #endregion

            return _credentials;
        }
        internal void DeleteCredential(Int32 credentialId)
        {
            var existingItem = SystemDal.GetCredentialById(AccessTokenId, credentialId);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "VLCredential", credentialId));

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            SystemDal.DeleteCredential(AccessTokenId, credentialId, existingItem.LastUpdateDT);
        }
        internal VLCredential CreateCredential(Int32 principalId, PrincipalType principalType, string logOnToken, string pswdToken, string passwordQuestion, string passwordAnswer, bool isApproved = false)
        {
            if(principalType == Core.PrincipalType.SystemUser)
            {
                var principal = SystemDal.GetSystemUserById(AccessTokenId, principalId);
                if (principal == null) throw new VLException("The principal is invalid!");
            }
            else if (principalType == Core.PrincipalType.ClientUser)
            {
                var principal = SystemDal.GetClientUserById(AccessTokenId, principalId);
                if (principal == null) throw new VLException("The principal is invalid!");
            }
            else
            {
                throw new ArgumentException(string.Format("principalType '{0}', is invalid!", principalType));
            }

            //μήπως ο χρήστης έχει ήδη credentials στο σύστημα
            if (SystemDal.GetCredentialForPrincipal(AccessTokenId, principalType, principalId) != null)
            {
                throw new VLException("There are already defined credentials for the principal.");
            }


            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            /*Ελέγχουμε το logOnToken:*/
            VLCredential.ValidateLogOnToken(ref logOnToken);
            if(SystemDal.GetCredentialsForLogOnToken(AccessTokenId, logOnToken) != null)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "LogOnToken", logOnToken));
            }
            
            

            /*Ελέγχουμε το password με βάση system-wide τιμές του Security-Policy:*/
            CheckPassword(ref pswdToken);

            /*Retrieve Security-Policy required values, and check password:*/
            var RequiresQuestionAndAnswer = Utility.RequiresQuestionAndAnswer;
            var PasswordFormat = Utility.PasswordFormat;

            string salt = Utility.GenerateSalt();
            string pass = Utility.EncodePassword(pswdToken, PasswordFormat, salt);
            if (pass.Length > 256)
            {
                throw new VLException(SR.GetString(SR.Password_is_invalid));
            }

            #region ελέγχουμε το PasswordAnswer
            string encodedPasswordAnswer = null;
            if (!string.IsNullOrWhiteSpace(passwordAnswer))
            {
                encodedPasswordAnswer = Utility.EncodePassword(passwordAnswer.ToLowerInvariant(), PasswordFormat, salt);
            }
            if (!Utility.ValidateParameter(ref encodedPasswordAnswer, RequiresQuestionAndAnswer, true, false, 128))
            {
                throw new VLException(SR.GetString(SR.PasswordAnswer_is_invalid));
            }
            #endregion

            #region ελέγχουμε passwordQuestion
            if (!Utility.ValidateParameter(ref passwordQuestion, RequiresQuestionAndAnswer, true, false, 255))
            {
                throw new VLException(SR.GetString(SR.PasswordQuestion_is_invalid));
            }
            #endregion


            var credential = new VLCredential();
            credential.PrincipalType = principalType;
            credential.Principal = principalId;
            credential.LogOnToken = logOnToken;
            credential.PswdToken = pass;
            credential.PswdSalt = salt;
            credential.PswdFormat = PasswordFormat;
            credential.PswdAnswer = encodedPasswordAnswer;
            credential.PswdQuestion = passwordQuestion;
            credential.IsApproved = isApproved;
            credential.IsLockedOut = false;


            return SystemDal.CreateCredential(AccessTokenId, credential);
        }
        internal VLCredential UpdateCredential(VLCredential credential)
        {
            if (credential == null) throw new ArgumentNullException("credential");
            credential.ValidateInstance();

            /*διαβαζουμε τα credentials απο την βάση και ταυτόχρονα ελέγχουμε την μοναδικότητα του LogOnToken*/
            var svdCredential = SystemDal.GetCredentialsForLogOnToken(AccessTokenId, credential.LogOnToken);
            if(svdCredential != null && svdCredential.CredentialId != credential.CredentialId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "LogOnToken", credential.LogOnToken));
            }
            if(svdCredential == null)
            {
                svdCredential = SystemDal.GetCredentialById(AccessTokenId, credential.CredentialId);
            }
            if (svdCredential == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "VLCredential", credential.CredentialId));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                /*Ενα clientAccount, μπορεί να κάνει update, μόνο το δικό του VLCredential*/
                if (svdCredential.PrincipalType != PrincipalType.ClientUser || svdCredential.Principal != this.Principal)
                {
                    throw new VLAccessDeniedException();
                }
            }
            #endregion


            svdCredential.LogOnToken = credential.LogOnToken;
            svdCredential.IsApproved = credential.IsApproved;
            svdCredential.IsLockedOut = credential.IsLockedOut;
            svdCredential.LastLoginDate = credential.LastLoginDate;
            svdCredential.LastPasswordChangedDate = credential.LastPasswordChangedDate;
            svdCredential.LastLockoutDate = credential.LastLockoutDate;
            svdCredential.FailedPasswordAttemptCount = credential.FailedPasswordAttemptCount;
            svdCredential.FailedPasswordAttemptWindowStart = credential.FailedPasswordAttemptWindowStart;
            svdCredential.FailedPasswordAnswerAttemptCount = credential.FailedPasswordAnswerAttemptCount;
            svdCredential.FailedPasswordAnswerAttemptWindowStart = credential.FailedPasswordAnswerAttemptWindowStart;
            svdCredential.Comment = credential.Comment;

            return SystemDal.UpdateCredential(AccessTokenId, svdCredential);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logOnToken"></param>
        /// <param name="pswdToken"></param>
        /// <param name="newPasswordQuestion"></param>
        /// <param name="newPasswordAnswer"></param>
        /// <returns></returns>
        public bool ChangePasswordQuestionAndAnswer(string logOnToken, string pswdToken, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the password for the specified logOnToken from the system store.
        /// <para>If the Repository supports hashed passwords, the GetPassword method should throw an exception if the EnablePasswordRetrieval
        /// property is set to true and the password format is set to Hashed. Hashed passwords cannot be retrieved.</para>
        /// </summary>
        /// <param name="logOnToken">The principal to get the password for.</param>
        /// <param name="passwordAnswer">The password answer for the specified principal.</param>
        /// <returns></returns>
        public string GetPassword(string logOnToken, string passwordAnswer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Process a request to update the password for a principal (user or module).
        /// <para>Takes, as input, a logOnToken, a current password, and a new password, and updates the password in 
        /// the system store if the supplied logOnToken and current password are valid.</para>
        /// </summary>
        /// <param name="logOnToken">The logOnToken of the principal to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns></returns>
        public bool ChangePassword(string logOnToken, string oldPassword, string newPassword)
        {
            if (Utility.ValidateParameter(ref logOnToken, true, true, true, 72) == false)
                return false;
            if (Utility.ValidateParameter(ref oldPassword, true, true, false, 128) == false)
                return false;
            if (Utility.ValidateParameter(ref newPassword, true, true, false, 128) == false)
                return false;

            var userPassword = GetPasswordWithFormat(logOnToken, oldPassword, false, false);
            if (userPassword == null)
                return false;

            #region SecurityLayer
            if(userPassword.Principal == this.Principal && userPassword.PrincipalType == this.PrincipalType)
            {
                //Ενας χρήστης αλλάζει το δικό του password
            }
            else
            {
                //Ενας SystemUser του συστήματος αλλάζει το password σε κάποιο account:
                CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            #endregion

            /*αλλάζουμε το password του principal:*/
            return SetPasswordImpl(userPassword.PrincipalType, userPassword.Principal, newPassword);
        }

        /// <summary>
        /// Process a request to set a new password for a principal (SystemUser or ClientUser).
        /// </summary>
        /// <param name="principalType"></param>
        /// <param name="principalId">The Id of the principal to set a new password for.</param>
        /// <param name="newPassword">The new password for the specified principal.</param>
        /// <returns></returns>
        internal bool SetNewPassword(PrincipalType principalType, Int32 principalId, string newPassword)
        {
            if (Utility.ValidateParameter(ref newPassword, true, true, false, 128) == false)
                return false;


            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            /*αλλάζουμε το password του principal:*/
            return SetPasswordImpl(principalType, principalId, newPassword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principalId"></param>
        /// <param name="principalRepositoryId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        bool SetPasswordImpl(PrincipalType principalType, Int32 principalId, string newPassword)
        {
            /*Retrieve Security-Policy required values, and check password:*/
            CheckPassword(ref newPassword);


            var passwordFormat = Utility.PasswordFormat; 
            string salt = Utility.GenerateSalt();
            string pass = Utility.EncodePassword(newPassword, passwordFormat, salt);
            if (pass.Length > 256)
            {
                throw new VLException(SR.GetString(SR.Password_is_invalid));
            }

            //Διαβάζουμε τα credentials απο το σύστημα για αυτόν τον χρήστη
            var credentials = SystemDal.GetCredentialForPrincipal(AccessTokenId, principalType, principalId);
            if (credentials == null)
                return false;

            credentials.PswdFormat = passwordFormat;
            credentials.PswdSalt = salt;
            credentials.PswdToken = pass;
            credentials.LastPasswordChangedDate = Utility.RoundToSeconds(DateTime.UtcNow);
            credentials = SystemDal.UpdateCredential(AccessTokenId, credentials);

            return true;
        }

        /// <summary>
        /// Resets a principal's password to a new, auotmatically generated password.
        /// </summary>
        /// <param name="logOnToken">The principal to reset the password for.</param>
        /// <param name="passwordAnswer">The password answer for the specified principal.</param>
        /// <returns></returns>
        public string ResetPassword(string logOnToken, string passwordAnswer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears a principal's lock, so that it can attempt to log on to the system again.
        /// </summary>
        /// <param name="logOnToken">The principal whose lock status is going to be cleared.</param>
        /// <returns></returns>
        public bool UnlockPrincipal(string logOnToken)
        {
            VLCredential.ValidateLogOnToken(ref logOnToken);

            var credentials = SystemDal.GetCredentialsForLogOnToken(AccessTokenId, logOnToken);
            if(credentials == null)
            {
                throw new VLException(string.Format("There is no Credentials for logOnToken '{0}'!", logOnToken));
            }

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return UnlockPrincipalImpl(credentials.PrincipalType, credentials.Principal);
        }
        /// <summary>
        /// Clears a principal's lock, so that it can attempt to log on to the system again.
        /// </summary>
        /// <param name="principalId">The principal whose lock status is going to be cleared.</param>
        /// <returns></returns>
        public bool UnlockPrincipal(PrincipalType principalType, Int32 principalId)
        {
            if(principalType == Core.PrincipalType.SystemUser)
            {
                var systemUser = SystemDal.GetSystemUserById(AccessTokenId, principalId);
                if(systemUser == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "SystemUser", principalId));
                }
            }
            else if(principalType == Core.PrincipalType.ClientUser)
            {
                var clientUser = SystemDal.GetClientUserById(AccessTokenId, principalId);
                if (clientUser == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientUser", principalId));
                }
            }

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSecurity, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return UnlockPrincipalImpl(principalType, principalId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="principalId"></param>
        /// <returns></returns>
        bool UnlockPrincipalImpl(PrincipalType principalType, Int32 principalId)
        {
            var credentials = SystemDal.GetCredentialForPrincipal(AccessTokenId, principalType, principalId);
            if (credentials == null)
                return false;

            credentials.IsLockedOut = false;
            credentials.LastLockoutDate = null;
            credentials.FailedPasswordAttemptCount = 0;
            credentials.FailedPasswordAttemptWindowStart = null;
            credentials.FailedPasswordAnswerAttemptCount = 0;
            credentials.FailedPasswordAnswerAttemptWindowStart = null;
            credentials = SystemDal.UpdateCredential(AccessTokenId, credentials);

            return true;
        }



        /// <summary>
        /// Ελέγχει το υποψήφιο password εάν είναι σύμφωνο με την πολιτική ασφαλείας του συστήματος.
        /// </summary>
        /// <param name="pswdToken"></param>
        void CheckPassword(ref string pswdToken)
        {
            /*Retrieve required SecurityPolicy values:*/
            int MinRequiredPasswordLength = Utility.MinRequiredPasswordLength;
            string PasswordStrengthRegularExpression = Utility.PasswordStrengthRegularExpression;
            int MinRequiredNonAlphanumericCharacters = Utility.MinRequiredNonAlphanumericCharacters;


            if (!Utility.ValidateParameter(ref pswdToken, true, true, false, 128))
            {
                throw new VLException(SR.GetString(SR.Password_is_invalid));
            }
            if (pswdToken.Length < MinRequiredPasswordLength)
            {
                throw new VLException(SR.GetString(SR.Password_is_too_small, MinRequiredPasswordLength));
            }
            int count = 0;
            for (int i = 0; i < pswdToken.Length; i++)
            {
                if (!char.IsLetterOrDigit(pswdToken, i))
                {
                    count++;
                }
            }
            if (count < MinRequiredNonAlphanumericCharacters)
            {
                throw new VLException(SR.GetString(SR.Password_MinRequiredNonAlphanumericCharacters, MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture)));
            }
            if (PasswordStrengthRegularExpression != null)
            {
                if (PasswordStrengthRegularExpression.Length > 0)
                {
                    if (!Regex.IsMatch(pswdToken, PasswordStrengthRegularExpression))
                    {
                        throw new VLException(SR.GetString(SR.Password_is_too_simple));
                    }
                }
            }
        }
        /// <summary>
        /// Αντίγραφο της μεθόδου υπάρχει και στην class ValisSystem
        /// </summary>
        /// <param name="logOnToken"></param>
        /// <param name="password"></param>
        /// <param name="updateLastActivityDate">Εάν θα ανανεωθεί το πεδίο LastActivityDate στον πίνακα Principals</param>
        /// <param name="failIfNotApproved"></param>
        /// <returns></returns>
        UserPassword GetPasswordWithFormat(string logOnToken, string password, bool updateLastActivityDate = true, bool failIfNotApproved = true)
        {
            DateTime dtNow = Utility.RoundToSeconds(DateTime.UtcNow);
            var userPassword = SystemDal.GetPasswordWithFormat(logOnToken, updateLastActivityDate, dtNow);
            if (userPassword == null)
                return null;


            if (userPassword.IsLockedOut)
                return null;
            if (!userPassword.IsActive)
                return null;
            if (!userPassword.IsApproved && failIfNotApproved)
                return null;


            string encodedPasswd = Utility.EncodePassword(password, userPassword.PswdFormat, userPassword.PswdSalt);

            bool isPasswordCorrect = userPassword.PswdFromDB.Equals(encodedPasswd);


            if (isPasswordCorrect && userPassword.FailedPasswordAttemptCount == 0 && userPassword.FailedPasswordAnswerAttemptCount == 0)
                return userPassword;

            SystemDal.UpdateUserInfo(
                        logOnToken,
                        isPasswordCorrect,
                        updateLastActivityDate,
                        userPassword.MaxInvalidPasswordAttempts,
                        userPassword.PasswordAttemptWindow,
                        dtNow,
                        isPasswordCorrect ? dtNow : userPassword.LastLoginDate,
                        isPasswordCorrect ? dtNow : userPassword.LastActivityDate);

            if (isPasswordCorrect)
                return userPassword;

            return null;
        }


        #endregion


        /// <summary>
        /// Δημιουργεί ένα πλήρες account τύπου SystemUser.
        /// <para>Δημιουργεί ταυτόχρονα ένα systemUser μαζί με τα crdentials αυτού!</para>
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="roleId"></param>
        /// <param name="email"></param>
        /// <param name="logOnToken"></param>
        /// <param name="pswdToken"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public VLSystemUser CreateSystemAccount(string firstName, string lastName, Int16 roleId, string email, string logOnToken, string pswdToken, string passwordQuestion=null, string passwordAnswer=null, bool isApproved = true)
        {
            int step = 0;
            VLSystemUser principal = null;
            VLCredential credentials = null;

            try
            {
                principal = new VLSystemUser();
                principal.FirstName = firstName;
                principal.LastName = lastName;
                principal.Email = email;
                principal.Role = roleId;
                principal = CreateSystemUser(principal);

                step = 1; /*Our principal created*/

                credentials = CreateCredential(principal.UserId, Core.PrincipalType.SystemUser, logOnToken, pswdToken, passwordQuestion, passwordAnswer, isApproved);
                

                if(credentials.IsApproved)
                {
                    principal.IsActive = true;
                    principal = SystemDal.UpdateSystemUser(AccessTokenId, principal);
                }


                step = 2; /*Our credential created*/
            }
            finally
            {
                if (step == 2)
                {
                    //ολα έχουν πάει καλά
                }
                else if (step == 1)
                {
                    //Δεν μπόρεσαμε να δημιουργήσουμε credentials
                    credentials = null;
                    SystemDal.DeleteSystemUser(AccessTokenId, principal.UserId, principal.LastUpdateDT);
                    principal = null;
                }
                else if (step == 0)
                {
                    credentials = null;
                    principal = null;
                }
            }

            return principal;
        }

        /// <summary>
        /// Δημιουργεί ένα πλήρες account τύπου ClientUser.
        /// <para>Δημιουργεί ταυτόχρονα ένα ClientUser μαζί με τα credentials αυτού!</para>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="roleId"></param>
        /// <param name="email"></param>
        /// <param name="logOnToken"></param>
        /// <param name="pswdToken"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public VLClientUser CreateClientAccount(Int32 clientId, string firstName, string lastName, Int16 roleId, string email, string logOnToken, string pswdToken, string passwordQuestion = null, string passwordAnswer = null, bool isApproved = true)
        {
            int step = 0;
            VLClientUser principal = null;
            VLCredential credentials = null;

            try
            {
                principal = new VLClientUser();
                principal.Client = clientId;
                principal.FirstName = firstName;
                principal.LastName = lastName;
                principal.Role = roleId;
                principal.Email = email;
                principal = CreateClientUser(principal);

                step = 1; /*Our principal created*/

                credentials = CreateCredential(principal.UserId, Core.PrincipalType.ClientUser, logOnToken, pswdToken, passwordQuestion, passwordAnswer, isApproved);


                if (credentials.IsApproved)
                {
                    principal.IsActive = true;
                    principal = SystemDal.UpdateClientUser(AccessTokenId, principal);
                }


                step = 2; /*Our credential created*/
            }
            finally
            {
                if (step == 2)
                {
                    //ολα έχουν πάει καλά
                }
                else if (step == 1)
                {
                    //Δεν μπόρεσαμε να δημιουργήσουμε credentials
                    credentials = null;
                    SystemDal.DeleteClientUser(AccessTokenId, principal.UserId, principal.LastUpdateDT);
                    principal = null;
                }
                else if (step == 0)
                {
                    credentials = null;
                    principal = null;
                }
            }

            return principal;
        }


        #region VLClient
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLClient> GetClients(string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetClients(AccessTokenId, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLClient> GetClients(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetClients(AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLClientEx> GetClientExs(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetClientExs(AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public VLClient GetClientById(Int32 clientId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ο τρέχων χρήστης μπορεί να δεί τα στοιχεία του Πελάτη στον οποίο ανήκει
                if(this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException("Invalid clientId!!");
                }
            }
            #endregion

            return SystemDal.GetClientById(AccessTokenId, clientId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public VLClient GetClientByName(string name)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetClientByName(AccessTokenId, name);
        }


        public VLClient CreateClient(string name, VLCountry country, string code = null, string timezoneId = null, Int32 profile = /*BuiltinProfiles.Default.ProfileId*/7)
        {
            VLClient client = new VLClient();
            client.Country = country.CountryId;
            client.Name = name;
            client.Code = code;
            if (string.IsNullOrWhiteSpace(timezoneId))
            {
                var timezone = SystemDal.GetSystemParameterByKey(this.AccessTokenId, "DefaultTimeZoneId");
                if (timezone != null)
                {
                    client.TimeZoneId = timezone.ParameterValue;
                }
                else
                {
                    throw new VLException("Cannot find System's DefaultTimeZoneId!");
                }
            }
            else
            {
                client.TimeZoneId = timezoneId;
            }
            client.Profile = profile;

            return CreateClient(client);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        internal VLClient CreateClient(VLClient client)
        {
            if (client == null) throw new ArgumentNullException("client");
            client.ValidateInstance();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion


            //Ελέγχουμε το όνομα του πελάτη να είναι μοναδικό:
            var existingItem = SystemDal.GetClientByName(AccessTokenId, client.Name);
            if(existingItem!= null)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", client.Name));
            }
            //Ελέγχουμε την χώρα:
            if (client.Country == 0)
            {
                throw new ArgumentException(SR.GetString(SR.Parameter_can_not_be_empty, "Country"), "Country");
            }
            var country = SystemDal.GetCountryById(AccessTokenId, client.Country);
            if(country == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Country", client.Country));
            }
            //Ελέγχουμε το timeZoneId:
            try
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(client.TimeZoneId);

            }
            catch (TimeZoneNotFoundException)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "TimeZone", client.TimeZoneId));
            }
            catch (InvalidTimeZoneException)
            {
                throw new VLException("The time zone identifier was found but the registry data is corrupted!");
            }
            catch (Exception ex)
            {
                throw new VLException("TimeZone cannot be validated!", ex);
            }


           


            return SystemDal.CreateClient(AccessTokenId, client);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public VLClient UpdateClient(VLClient client)
        {
            if (client == null) throw new ArgumentNullException("client");
            client.ValidateInstance();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion


            //Ελέγχουμε το όνομα του πελάτη να είναι μοναδικό:
            var existingItem = SystemDal.GetClientByName(AccessTokenId, client.Name);
            if (existingItem != null && existingItem.ClientId != client.ClientId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", client.Name));
            }
            if(existingItem == null)
            {
                existingItem = SystemDal.GetClientById(AccessTokenId, client.ClientId);
            }
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Client", client.ClientId));


            //εάν έχουμε ένα builtin client, τότε δεν μπορούμε να προχωρήσουμε:
            if (existingItem.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_update_builtin, "Client"));
            }

            //Ελέγχουμε την χώρα:
            if (client.Country == 0)
            {
                throw new ArgumentException(SR.GetString(SR.Parameter_can_not_be_empty, "Country"), "Country");
            }
            var country = SystemDal.GetCountryById(AccessTokenId, client.Country);
            if (country == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Country", client.Country));
            }
            //Ελέγχουμε το timeZoneId:
            try
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(client.TimeZoneId);

            }
            catch (TimeZoneNotFoundException)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "TimeZone", client.TimeZoneId));
            }
            catch (InvalidTimeZoneException)
            {
                throw new VLException("The time zone identifier was found but the registry data is corrupted!");
            }
            catch (Exception ex)
            {
                throw new VLException("TimeZone cannot be validated!", ex);
            }

            /*
             *  ΕΛΕΓΧΟΥΜΕ ΤΥΧΟΝ ΑΛΛΑΓΗ ΤΟΥ PROFILE TOY ΠΕΛΑΤΗ ΜΑΣ:
             */
            if(existingItem.Profile != client.Profile)
            {
                /*διαβάζουμε απο το σύστημα το νέο profile*/
                var newProfile = SystemDal.GetClientProfileById(this.AccessTokenId, client.Profile);
                if (newProfile == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Profile", existingItem.Profile));


                /*  
                 * Αυτό που μας ενδιαφέρει στο Profile του πελάτη είναι κυρίως η παράμετρος UseCredits, η οποία δεν μπορεί να αλλάζει ελεύθερα.
                 * Επειδή οι χρεώσεις γίνονται επάνω στους Collectors, εάν ο Πελάτης δεν έχει ακόμα κανένα Collector, το Profile του μπορεί να αλλάζει
                 * ελεύθερα.
                 * 
                 */
                if (SurveysDal.GetCollectorsCountForClient(this.AccessTokenId, client.ClientId) > 0)
                {
                    /* Δεν μπορούμε να αλλάξουμε απο ένα Profile που χρεώνει σε ένα Profile που δεν χρεώνει.
                     * Οι Collectors αποθηκεύουν το Profile.UseCredits την στιγμή που δημιουργούνται, και εν τω μεταξύ έχουν
                     * δημιουργηθεί payments, CollectorPayments, κ.α. που δεν ξέρω πως να τα χειριστώ για μία τέτοια αλλαγή.
                     * 
                     * ΑΡΑ ΠΡΟΣ ΤΟ ΠΑΡΩΝ ΑΠΑΓΟΡΕΥΟΥΜΕ ΤΕΤΟΙΑ ΑΛΛΑΓΗ!
                     */
                    var previousProfile = SystemDal.GetClientProfileById(this.AccessTokenId, existingItem.Profile);
                    if (previousProfile == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Profile", existingItem.Profile));

                    if (previousProfile.UseCredits == true && newProfile.UseCredits == false)
                    {
                        throw new VLException("You cannot change from one PAID Profile to one FREE Profile!");
                    }
                }


            }


            //Κάνουμε update μέσω του existingItem:
            existingItem.Code = client.Code;
            existingItem.Name = client.Name;
            existingItem.Profession = client.Profession;
            existingItem.Country = client.Country;
            existingItem.TimeZoneId = client.TimeZoneId;
            existingItem.Prefecture = client.Prefecture;
            existingItem.Town = client.Town;
            existingItem.Address = client.Address;
            existingItem.Zip = client.Zip;
            existingItem.Telephone1 = client.Telephone1;
            existingItem.Telephone2 = client.Telephone2;
            existingItem.WebSite = client.WebSite;
            existingItem.AttributeFlags = client.AttributeFlags;
            existingItem.Profile = client.Profile;
            existingItem.Comment = client.Comment;


            return SystemDal.UpdateClient(AccessTokenId, existingItem);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public void DeleteClient(VLClient client)
        {
            if (client == null) throw new ArgumentNullException("client");
            DeleteClient(client.ClientId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        public void DeleteClient(Int32 clientId)
        {
            var client = SystemDal.GetClientById(AccessTokenId, clientId);
            if (client == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Client", client.ClientId));
            }

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion


            //Δεν μπορούμε να διαγράψουμε ένα builtin πελάτη:
            if (client.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_delete_builtin, "Client"));
            }
            //Μπορούμε να διαγράψουμε έναν πελάτη εάν έχει απο κάτω του surveys:
            if (SurveysDal.GetSurveysCount(AccessTokenId, string.Format("where Client={0}", client.ClientId), BuiltinLanguages.PrimaryLanguage) > 0)
            {
                throw new VLException("You can't delete a Client when there are one or more surveys depending on it!");
            }

            try
            {
                /*
                 * διαγράφουμε το directory για αυτό το πελατη: 
                 */
                string fileDirPath = Path.Combine(FileInventoryPath, client.ClientId.ToString(CultureInfo.InvariantCulture));
                if (Directory.Exists(fileDirPath))
                {
                    Directory.Delete(fileDirPath, true);
                }
            }
            finally
            {
                SystemDal.DeleteClient(AccessTokenId, client.ClientId, client.LastUpdateDT);
            }

        }
        #endregion

        #region VLClientProfile
        public Collection<VLClientProfile> GetClientProfiles(string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion


            return SystemDal.GetClientProfiles(this.AccessTokenId, whereClause, orderByClause);
        }
        public Collection<VLClientProfile> GetClientProfiles(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SystemDal.GetClientProfiles(this.AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }

        public VLClientProfile GetClientProfileById(Int32 profileId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SystemDal.GetClientProfileById(this.AccessTokenId, profileId);
        }
        public VLClientProfile UpdateClientProfile(VLClientProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile");
            profile.ValidateInstance();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            //ελέγχουμε το όνομα του profile να είναι μοναδικό
            var existingItem = SystemDal.GetClientProfileByName(AccessTokenId, profile.Name);
            if (existingItem != null && existingItem.ProfileId != profile.ProfileId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", profile.Name));
            }
            if (existingItem == null)
            {
                existingItem = SystemDal.GetClientProfileById(AccessTokenId, profile.ProfileId);
            }
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientProfile", profile.ProfileId));

            
            //εάν έχουμε ένα builtin profile, τότε δεν μπορούμε να προχωρήσουμε:
            if (existingItem.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_update_builtin, "ClientProfile"));
            }



            //Κάνουμε update μέσω του existingItem:
            existingItem.Name = profile.Name;
            existingItem.Comment = profile.Comment;
            existingItem.MaxNumberOfUsers = profile.MaxNumberOfUsers;
            existingItem.MaxNumberOfSurveys = profile.MaxNumberOfSurveys;
            existingItem.MaxNumberOfLists = profile.MaxNumberOfLists;
            existingItem.MaxNumberOfRecipientsPerList = profile.MaxNumberOfRecipientsPerList;
            existingItem.MaxNumberOfRecipientsPerMessage = profile.MaxNumberOfRecipientsPerMessage;
            existingItem.MaxNumberOfCollectorsPerSurvey = profile.MaxNumberOfCollectorsPerSurvey;
            existingItem.MaxNumberOfEmailsPerDay = profile.MaxNumberOfEmailsPerDay;
            existingItem.MaxNumberOfEmailsPerWeek = profile.MaxNumberOfEmailsPerWeek;
            existingItem.MaxNumberOfEmailsPerMonth = profile.MaxNumberOfEmailsPerMonth;
            existingItem.MaxNumberOfEmails = profile.MaxNumberOfEmails;
            //existingItem.IsBuiltIn = profile.IsBuiltIn;
            existingItem.UseCredits = profile.UseCredits;
            existingItem.CanTranslateSurveys = profile.CanTranslateSurveys;
            existingItem.CanUseSurveyTemplates = profile.CanUseSurveyTemplates;
            existingItem.CanUseQuestionTemplates = profile.CanUseQuestionTemplates;
            existingItem.CanCreateWebLinkCollectors = profile.CanCreateWebLinkCollectors;
            existingItem.CanCreateEmailCollectors = profile.CanCreateEmailCollectors;
            existingItem.CanCreateWebsiteCollectors = profile.CanCreateWebsiteCollectors;
            existingItem.CanUseSkipLogic = profile.CanUseSkipLogic;
            existingItem.CanExportData = profile.CanExportData;            
            existingItem.CanExportReport = profile.CanExportReport;
            existingItem.CanUseWebAPI = profile.CanUseWebAPI;

            
            return SystemDal.UpdateClientProfile(AccessTokenId, existingItem);
        }
        public VLClientProfile CreateClientProfile(VLClientProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile");
            profile.ValidateInstance();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            //ελέγχουμε το όνομα του profile να είναι μοναδικό
            var existingItem = SystemDal.GetClientProfileByName(AccessTokenId, profile.Name);
            if (existingItem != null)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", profile.Name));
            }

            return SystemDal.CreateClientProfile(this.AccessTokenId, profile);
        }

        public void DeleteClientProfile(Int32 profileId)
        {
            var existingItem = SystemDal.GetClientProfileById(this.AccessTokenId, profileId);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientProfile", profileId));

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            //Δεν μπορούμε να διαγράψουμε ένα builtin profile:
            if(existingItem.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_delete_builtin, "Profile"));
            }
            //Δεν μπορούμε να διαγράψουμε ένα Profile, όταν υπάρχουν Clients συνδεδεμένοι με αυτό
            if (SystemDal.GetClientsByProfileCount(this.AccessTokenId, profileId) > 0)
            {
                throw new VLException(string.Format("Profile '{0}', cannot be deleted because it is used by Clients!", existingItem.Name));
            }


            SystemDal.DeleteClientProfile(this.AccessTokenId, existingItem.ProfileId, existingItem.LastUpdateDT);
        }

        #endregion

        #region VLClientUser
        public Collection<VLClientUser> GetClientUsers(VLClient client, string whereClause = null, string orderByClause = null)
        {
            if (client == null) throw new ArgumentNullException("client");
            return GetClientUsers(client.ClientId, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLClientUser> GetClientUsers(Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException("Invalid clientId!!");
                }
                CheckPermissions(VLPermissions.ClientEnumerateUsers);
            }
            #endregion

            return SystemDal.GetClientUsers(AccessTokenId, clientId, whereClause, orderByClause);        
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLClientUser> GetClientUsers(VLClient client, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = "order by LastName, FirstName")
        {
            if (client == null) throw new ArgumentNullException("client");

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != client.ClientId)
                {
                    throw new VLAccessDeniedException("Invalid client!!");
                }
                CheckPermissions(VLPermissions.ClientEnumerateUsers);
            }
            #endregion

            return GetClientUsers(client.ClientId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLClientUser> GetClientUsers(Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = "order by LastName, FirstName")
        {
            Utility.CheckParameter(ref whereClause, false, false, false, 2048, "whereClause");
            Utility.CheckParameter(ref orderByClause, false, false, false, 1024, "orderByClause");

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException("Invalid clientId!!");
                }
                CheckPermissions(VLPermissions.ClientEnumerateUsers);
            }
            #endregion


            return SystemDal.GetClientUsers(AccessTokenId, clientId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        internal Collection<VLClientUserView> GetclientUserViews(Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = "order by LastName, FirstName")
        {
            Utility.CheckParameter(ref whereClause, false, false, false, 2048, "whereClause");
            Utility.CheckParameter(ref orderByClause, false, false, false, 1024, "orderByClause");

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException("Invalid clientId!!");
                }
                CheckPermissions(VLPermissions.ClientEnumerateUsers);
            }
            #endregion


            return ViewModelDal.GetClientUserViews(AccessTokenId, clientId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public VLClientUser GetClientUserById(Int32 userId)
        {            
            var clientUser = SystemDal.GetClientUserById(AccessTokenId, userId);

            if(clientUser == null)
            {
                return clientUser;
            }

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            
                return clientUser;
            }
            else
            {
                //Ο τρέχων χρήστης μπορεί να ζητά πληροφορίες που ανήκουν στον πελάτη του:
                if(this.ClientId != clientUser.Client)
                {
                    throw new VLAccessDeniedException();
                }

                if (this.Principal == clientUser.UserId)
                    return clientUser;

                CheckPermissions(VLPermissions.ClientEnumerateUsers);
                return clientUser;
            }
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logOnToken"></param>
        /// <returns></returns>
        public VLClientUser GetClientUserByLogOnToken(string logOnToken)
        {
            VLCredential.ValidateLogOnToken(ref logOnToken);

            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetClientUserByLogOnToken(AccessTokenId, logOnToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public VLClientUser GetClientUserByEmail(string email)
        {
            Utility.CheckParameter(ref email, true, true, true, 256, "email");

            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return SystemDal.GetClientUserByEmail(AccessTokenId, email);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessTokenId"></param>
        /// <returns></returns>
        public VLClientUser GetClientUserFromAccessToken(Int32 accessTokenId)
        {
            var item = SystemDal.GetClientUserFromAccessToken(AccessTokenId, accessTokenId);

            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            return item;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="roleId"></param>
        /// <param name="email"></param>
        /// <param name="timezoneId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        internal VLClientUser CreateClientUser(Int32 clientId, string firstName, string lastName, Int16 roleId, string email, string timezoneId = null, string comment = null)
        {
            var user = new VLClientUser();
            user.Client = clientId;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.Role = roleId;
            user.Comment = comment;
            if (string.IsNullOrWhiteSpace(timezoneId))
            {
                var timezone = SystemDal.GetSystemParameterByKey(this.AccessTokenId, "DefaultTimeZoneId");
                if (timezone != null)
                {
                    user.TimeZoneId = timezone.ParameterValue;
                }
                else
                {
                    throw new VLException("Cannot find System's DefaultTimeZoneId!");
                }
            }
            else
            {
                user.TimeZoneId = timezoneId;
            }
            return CreateClientUser(user);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal VLClientUser CreateClientUser(VLClientUser user)
        {
            #region input parameters validation
            if (user == null) throw new ArgumentNullException("user");
            user.ValidateInstance();
            #endregion


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSecurity | VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != user.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientManageUsers);
            }
            #endregion


            /*
             * Ελεγχος του ρόλου του ClientUser.
             * Δεν είναι δυνατόν να του δώσουμε ρόλους που υπάρχουν μόνο για τους SystemUsers:
             */
            var role = SystemDal.GetRoleById(AccessTokenId, user.Role);
            if (role == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Role", user.Role));
            }
            if (role.IsClientRole == false)
            {
                throw new VLException(string.Format("Role '{0}', cannot be assigned ta a client account!", role.Name));
            }
            /*Ελεγχος του email του ClientUser:*/
            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                if (!Utility.EmailIsValid(user.Email))
                {
                    throw new VLException(string.Format("Invalid email address '{0}'!", user.Email));
                }
            }
            if (Utility.RequiresUniqueEmail)
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    throw new VLException("ClientUsers are required to have an email address!");
                }
                if (SystemDal.GetClientUserByEmail(AccessTokenId, user.Email) != null)
                {
                    throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Email", user.Email));
                }
            }
            //Ελέγχουμε το timeZoneId:
            if (!string.IsNullOrWhiteSpace(user.TimeZoneId))
            {
                try
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);
                }
                catch (TimeZoneNotFoundException)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "TimeZone", user.TimeZoneId));
                }
                catch (InvalidTimeZoneException)
                {
                    throw new VLException("The time zone identifier was found but the registry data is corrupted!");
                }
                catch (Exception ex)
                {
                    throw new VLException("TimeZone cannot be validated!", ex);
                }
            }
            else
            {
                user.TimeZoneId = null;
            }

            user.IsBuiltIn = false;
            user.IsActive = false;
            user.DefaultLanguage = BuiltinLanguages.Greek.LanguageId;


            return SystemDal.CreateClientUser(AccessTokenId, user);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public VLClientUser UpdateClientUser(VLClientUser user)
        {
            #region input parameters validation
            if (user == null) throw new ArgumentNullException("user");
            user.ValidateInstance();
            #endregion


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSecurity | VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != user.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientManageUsers);
            }
            #endregion

            var existingUser = SystemDal.GetClientUserById(AccessTokenId, user.UserId);
            if (existingUser == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientUser", user.UserId));

            if (existingUser.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_update_the_builtin_entity, "ClientUser", string.Format(CultureInfo.InvariantCulture, "{0}, {1}", user.LastName, user.FirstName)));
            }


            /*
             * Ελεγχος του ρόλου του ClientUser.
             * Δεν είναι δυνατόν να του δώσουμε ρόλους που υπάρχουν μόνο για τους SystemUsers:
             */
            var role = SystemDal.GetRoleById(AccessTokenId, user.Role);
            if (role == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Role", user.Role));
            }
            if(role.IsClientRole == false)
            {
                throw new VLException(string.Format("Role '{0}', cannot be assigned ta a client account!", role.Name));
            }
            /*Ελεγχος του email του ClientUser:*/
            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                if (!Utility.EmailIsValid(user.Email))
                {
                    throw new VLException(string.Format("Invalid email address '{0}'!", user.Email));
                }
            }
            if (Utility.RequiresUniqueEmail)
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    throw new VLException("ClientUsers are required to have an email address!");
                }
                var _user = SystemDal.GetClientUserByEmail(AccessTokenId, user.Email);
                if (_user != null && _user.UserId != user.UserId)
                {
                    throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Email", user.Email));
                }
            }
            //Ελέγχουμε το timeZoneId:
            if (!string.IsNullOrWhiteSpace(user.TimeZoneId))
            {
                try
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);

                }
                catch (TimeZoneNotFoundException)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "TimeZone", user.TimeZoneId));
                }
                catch (InvalidTimeZoneException)
                {
                    throw new VLException("The time zone identifier was found but the registry data is corrupted!");
                }
                catch (Exception ex)
                {
                    throw new VLException("TimeZone cannot be validated!", ex);
                }
            }
            else
            {
                user.TimeZoneId = null;
            }


            //UserId
            existingUser.DefaultLanguage = user.DefaultLanguage;
            existingUser.Title = user.Title;
            existingUser.Department = user.Department;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Country = user.Country;
            existingUser.TimeZoneId = user.TimeZoneId;
            existingUser.Prefecture = user.Prefecture;
            existingUser.Town = user.Town;
            existingUser.Address = user.Address;
            existingUser.Zip = user.Zip;
            existingUser.Telephone1 = user.Telephone1;
            existingUser.Telephone2 = user.Telephone2;
            existingUser.Email = user.Email;
            existingUser.IsActive = user.IsActive;
            //existingUser.IsBuiltIn = user.IsBuiltIn;
            existingUser.AttributeFlags = user.AttributeFlags;
            existingUser.Role = user.Role;
            existingUser.Comment = user.Comment;
            //existingUser.LastActivityDate = user.LastActivityDate;


            return SystemDal.UpdateClientUser(AccessTokenId, existingUser);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void DeleteClientUser(VLClientUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            DeleteClientUser(user.UserId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteClientUser(Int32 userId)
        {
            /*
             * We have to exclude the case that a user is trying to delete itself.
             * It is not allowed
             */
            if (this.Principal == userId)
            {
                throw new VLException("A user cannot delete itself!");
            }

            var user = SystemDal.GetClientUserById(AccessTokenId, userId);
            if (user == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientUser", userId));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSecurity | VLPermissions.ManageClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != user.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientManageUsers);
            }
            #endregion

            if (user.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_delete_the_builtin_entity, "ClientUser", string.Format(CultureInfo.InvariantCulture, "{0}, {1}", user.LastName, user.FirstName)));
            }


            SystemDal.DeleteClientUser(AccessTokenId, user.UserId, user.LastUpdateDT);
        }
        #endregion


        #region VLKnownEmail
        /// <summary>
        /// Επιστρέφει Knownemails ανεξάρτητα του Client στον οποίο ανήκουν
        /// <para>Προορίζεται για χρήση μόνο απο SystemUsers</para>
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        internal Collection<VLKnownEmail> GetKnownEmails(string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion

            return SystemDal.GetKnownEmails(this.AccessTokenId, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLKnownEmail> GetKnownEmails(Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException("Invalid clientId!!");
                }
                //Δεν υπάρχουν ειδικά permissions για τον ίδιο τον client!

            }
            #endregion

            return SystemDal.GetKnownEmails(this.AccessTokenId, clientId, whereClause, orderByClause);
        }
        /// <summary>
        /// Επιστρέφει paged Knownemails ανεξάρτητα του Client στον οποίο ανήκουν
        /// <para>Προορίζεται για χρήση μόνο απο SystemUsers</para>
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        internal Collection<VLKnownEmailEx> GetKnownEmailExs(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion

            return SystemDal.GetKnownEmailExs(AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public int GetKnownEmailsCount(Int32 clientId, string whereClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException("Invalid clientId!!");
                }
                //Δεν υπάρχουν ειδικά permissions για τον ίδιο τον client!

            }
            #endregion

            return SystemDal.GetKnownEmailsCount(this.AccessTokenId, clientId, whereClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public VLKnownEmail GetKnownEmailById(Int32 emailId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);

                return SystemDal.GetKnownEmailById(this.AccessTokenId, emailId);
            }
            else
            {
                var email = SystemDal.GetKnownEmailById(this.AccessTokenId, emailId);
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != email.Client)
                {
                    throw new VLAccessDeniedException("Invalid clientId!!");
                }
                //Δεν υπάρχουν ειδικά permissions για τον ίδιο τον client!


                return email;
            }
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public VLKnownEmail GetKnownEmailByAddress(Int32 clientId, string address)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException("Invalid clientId!!");
                }
                //Δεν υπάρχουν ειδικά permissions για τον ίδιο τον client!

            }
            #endregion

            return SystemDal.GetKnownEmailByAddress(this.AccessTokenId, clientId, address);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="emailAddress"></param>
        /// <param name="isVerified"></param>
        /// <param name="verifiedDt"></param>
        /// <param name="isOptedOut"></param>
        /// <param name="optedOutDt"></param>
        /// <param name="validateDomain"></param>
        /// <returns></returns>
        public VLKnownEmail RegisterKnownEmail(Int32 clientId, string emailAddress, bool isVerified = false, DateTime? verifiedDt=null, bool isOptedOut = false, DateTime? optedOutDt = null, bool validateDomain = true)
        {
            VLKnownEmail email = new VLKnownEmail();
            email.Client = clientId;
            email.EmailAddress = emailAddress;
            email.RegisterDt = Utility.UtcNow();
            email.IsDomainOK = false;

            email.IsVerified = isVerified;
            email.VerifiedDt = verifiedDt;

            email.IsOptedOut = isOptedOut;
            email.OptedOutDt = optedOutDt;

            email.IsBounced = false;

            return RegisterKnownEmail(email, validateDomain);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="validateDomain"></param>
        /// <returns></returns>
        internal VLKnownEmail RegisterKnownEmail(VLKnownEmail email, bool validateDomain = true)
        {
            if (string.IsNullOrWhiteSpace(email.EmailAddress)) throw new ArgumentNullException("email");


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != email.Client)
                {
                    throw new VLAccessDeniedException();
                }

            }
            #endregion

            //Μετατρέπουμε την EmailAddress σε μικρά γράμματα:
            email.EmailAddress = email.EmailAddress.ToLowerInvariant();

            /*για κάθε πελάτη, μία EmailAddress πρέπει να εμφανίζεται μόνο μία φορά:*/
            if(SystemDal.GetKnownEmailByAddress(this.AccessTokenId, email.Client, email.EmailAddress) != null)
            {
                throw new VLException(string.Format("Email Address '{0}', already exists!", email.EmailAddress));
            }

            /*Ελέγχουμε το format του emailAddress:*/
            if (!Utility.EmailIsValid(email.EmailAddress))
            {
                throw new VLException("Invalid email format!");
            }
            MailAddress mailAddress = new MailAddress(email.EmailAddress);
            email.LocalPart = mailAddress.User;
            email.DomainPart = mailAddress.Host;
            
            /*Ελέγχουμε το domain (εάν μας έχει ζητηθεί):*/
            if (validateDomain)
            {
                if (!Utility.IsValidDomainName(email.DomainPart))
                {
                    throw new VLException("Invalid email domain!");
                }

                //https://docs.ar-soft.de/arsoft.tools.net/
                //http://arsofttoolsnet.codeplex.com/
                //http://www.codeproject.com/Articles/12072/C-NET-DNS-query-component
                DomainName name = null;
                if(DomainName.TryParse(email.DomainPart, out name))
                {
                    var response = DnsClient.Default.Resolve(name, RecordType.Mx);
                    if (response.AnswerRecords.Count == 0)
                    {
                        throw new VLException("Unknown email domain!");
                    }
                }

                email.IsDomainOK = true;
            }
            email.ValidateInstance();


            return SystemDal.CreateKnownEmail(this.AccessTokenId, email);
        }

        public VLKnownEmail UpdateKnownEmail(VLKnownEmail email)
        {
            if (email == null) throw new ArgumentNullException();
            email.ValidateInstance();


            var existingEmail = SystemDal.GetKnownEmailByAddress(this.AccessTokenId, email.Client, email.EmailAddress);
            if (existingEmail != null && existingEmail.EmailId != email.EmailId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "EmailAddress", email.EmailAddress));
            }
            if (existingEmail == null)
            {
                existingEmail = SystemDal.GetKnownEmailById(this.AccessTokenId, email.EmailId);
            }
            if (existingEmail == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Knownemail", email.EmailId));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != email.Client)
                {
                    throw new VLAccessDeniedException();
                }

            }
            #endregion

            /*Δεν αλλάζει με κανένα τρόπο η Email Address:*/
            if(existingEmail.EmailAddress != email.EmailAddress)
            {
                throw new VLException("You cannot change the EmailAddress for a KnownEmail! In order to change it, create a new Knownemail and delete the old one!");
            }

            //Οι αλλαγές γίνονται με βάση το record που διαβάσαμε απο την βάση μας:
            existingEmail.IsDomainOK = email.IsDomainOK;
            existingEmail.IsVerified = email.IsVerified;
            existingEmail.IsOptedOut = email.IsOptedOut;
            existingEmail.IsBounced = email.IsBounced;
            existingEmail.VerifiedDt = email.VerifiedDt;
            existingEmail.OptedOutDt = email.OptedOutDt;

            return SystemDal.UpdateKnownEmail(this.AccessTokenId, existingEmail);
        }

        public void DeleteKnownEmail(Int32 emailId)
        {
            var email = SystemDal.GetKnownEmailById(this.AccessTokenId, emailId);
            if (email == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "KnownEmail", emailId));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateSecurity | VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != email.Client)
                {
                    throw new VLAccessDeniedException();
                }

            }
            #endregion


            SystemDal.DeleteKnownEmail(this.AccessTokenId, email.EmailId);
        }

        #endregion


        #region VLPayment
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Collection<VLBalance> GetBalances(Int32 clientId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetBalances(this.AccessTokenId, clientId);
        }
        /// <summary>
        /// Επιστρέφει όλες τις πληρωμές του Πελάτη
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLPayment> GetPayments(Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients|VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetPayments(this.AccessTokenId, clientId, whereClause, orderByClause);
        }
        
        public Int32 GetPaymentsCount(Int32 clientId, string whereClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients|VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetPaymentsCount(this.AccessTokenId, clientId, whereClause);
        }
        public Collection<VLPayment> GetPayments(Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients|VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetPayments(this.AccessTokenId, clientId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        public Collection<VLPaymentView1> GetPaymentsView1(Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                //Ελέγχουμε έτσι ώστε ο τρέχων χρήστης να χρησιμοποιεί το δικό του clientId:
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetPaymentsView1PagedImpl(this.AccessTokenId, clientId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        
        public VLPayment GetPaymentById(Int32 paymentId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients|VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
                
                return SystemDal.GetPaymentById(this.AccessTokenId, paymentId);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);

                var payment = SystemDal.GetPaymentById(this.AccessTokenId, paymentId);
                if(this.ClientId != payment.Client)
                {
                    throw new VLAccessDeniedException();
                }
                return payment;
            }
            #endregion
        }

        public VLPayment AddPayment(Int32 clientId, CreditType resource, Int32 quantity)
        {
            return AddPayment(clientId, resource, quantity, DateTime.Now, PaymentType.Default);
        }
        public VLPayment AddPayment(Int32 clientId, CreditType resource, Int32 quantity, DateTime paymentDate, PaymentType paymentType)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.ManagePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            /*Διαβάζουμε τον πελάτη απο το σύστημα μας:*/
            var client = SystemDal.GetClientById(this.AccessTokenId, clientId);
            if (client == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Client", clientId));
            }
            /*Διαβάζουμε το profile του πελάτη μας */
            var profile = SystemDal.GetClientProfileById(this.AccessTokenId, client.Profile);
            if (profile == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientProfile", client.Profile));
            }
            if (profile.UseCredits == false)
            {
                throw new VLException(string.Format("Client '{0}', cannot use payments. It's profile doesn't allow payments!", client.Name));
            }

            /*Ελέγχουμε το quantity:*/
            if(quantity <=0)
            {
                throw new VLException("Quantity must be greater than zero (0)!");
            }


            VLPayment payment = new VLPayment();
            payment.Client = client.ClientId;
            payment.PaymentType = paymentType;
            payment.PaymentDate = paymentDate;
            payment.IsActive = true;
            payment.IsTimeLimited = false;
            payment.CreditType = resource;
            payment.Quantity = quantity;
            payment.QuantityUsed = 0;

            return SystemDal.CreatePayment(this.AccessTokenId, payment);
        }
                
        public VLPayment UpdatePayment(VLPayment payment)
        {
            if (payment == null) throw new ArgumentException("payment");
            payment.ValidateInstance();

            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.ManagePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            var existingPayment = SystemDal.GetPaymentById(this.AccessTokenId, payment.PaymentId);
            if (existingPayment == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Payment", payment.PaymentId));
            }


            /*τραβάμε όλα τα συνδεδεμένα collectorPayments, και μετράμε το QuantityReserved:*/
            var collectorPayments = SystemDal.GetCollectorPaymentsForPayment(this.AccessTokenId, payment.PaymentId);
            var QuantityReserved = 0;
            foreach (var item in collectorPayments)
            {
                QuantityReserved += item.QuantityReserved;
            }

            /*
             * Ελέγχουμε το CreditType. Εάν η πληρωμή έχει συνδεθεί με collectors ή 
             * το QuantityUsed έχει θετική τιμή τότε δεν μπορούμε να κάνουμε update 
             * το RespourceType:
             */
            if (payment.CreditType != existingPayment.CreditType)
            {
                if(collectorPayments.Count > 0 )
                {
                    throw new VLException("You cannot change the CreditType of the payment! There are collectors associated with it!");
                }
                if(payment.QuantityUsed > 0 )
                {
                    throw new VLException("You cannot change the CreditType of the payment! It has been used!");
                }
            }
            /*Ελέγχουμε το quantity:*/
            if (payment.Quantity <= 0)
            {
                throw new VLException("Quantity must be greater than zero (0)!");
            }
            if (payment.Quantity < payment.QuantityUsed)
            {
                throw new VLException("Quantity cannot be lower than the QuantityUsed!");
            }
            if (payment.Quantity < QuantityReserved)
            {
                throw new VLException("Quantity cannot be lower than the total QuantityUsed of all payment's CollectorPayments!");
            }

            if (payment.IsTimeLimited)
            {
                if(payment.ValidFromDt.HasValue == false)
                {
                    throw new VLException("ValidFromDt must be defined!");
                }
                if (payment.ValidToDt.HasValue == false)
                {
                    throw new VLException("ValidToDt must be defined!");
                }

                if(payment.ValidToDt < payment.ValidFromDt)
                {
                    throw new VLException("ValidToDt must be greater than ValidFromDt!");
                }
            }



            //
            existingPayment.Comment = payment.Comment;
            existingPayment.PaymentType = payment.PaymentType;
            existingPayment.PaymentDate = payment.PaymentDate;
            existingPayment.CustomCode1 = payment.CustomCode1;
            existingPayment.CustomCode2 = payment.CustomCode2;
            existingPayment.IsActive = payment.IsActive;
            existingPayment.IsTimeLimited = payment.IsTimeLimited;
            existingPayment.ValidFromDt = payment.ValidFromDt;
            existingPayment.ValidToDt = payment.ValidToDt;
            existingPayment.CreditType = payment.CreditType;
            existingPayment.Quantity = payment.Quantity;

            return SystemDal.UpdatePayment(this.AccessTokenId, existingPayment);
        }

        public void DeletePayment(VLPayment payment)
        {
            if (payment == null) throw new ArgumentNullException("payment");

            DeletePayment(payment.PaymentId);
        }
        public void DeletePayment(Int32 paymentId)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.ManagePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            #endregion

            var existingPayment = SystemDal.GetPaymentById(this.AccessTokenId, paymentId);
            if (existingPayment == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Payment", paymentId));


            //Δεν μπορούμε να διαγράψουμε ένα Payment, εάν αυτό έχει συνδεθεί με Collectors:
            if(SystemDal.GetCollectorPaymentsForPayment(this.AccessTokenId, existingPayment.PaymentId).Count > 0)
            {
                throw new VLException("Payment cannot be deleted because it is associated with one or more Collectors!");
            }
            //Δεν μπορούμε να διαγράψουμε ένα Payment, εάν αυτό έχει χρησιμοποιηθεί:
            if(existingPayment.QuantityUsed > 0 )
            {
                throw new VLException("Payment cannot be deleted because it has been used!");
            }




            SystemDal.DeletePayment(this.AccessTokenId, existingPayment.PaymentId, existingPayment.LastUpdateDT);
        }
        #endregion

        #region VLCollectorPayment
        /// <summary>
        /// Eπιστρέφει όλα τα CollectorPayments που έχουν δημιουργηθεί στο σύστημα για (όλους) τους Collectors του συγκεκριμένου Survey.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLCollectorPayment> GetSurveyPayments(Int32 surveyId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments, VLPermissions.ClientEnumerateCollectors);
            }
            #endregion

            return SystemDal.GetCollectorPaymentsForSurvey(this.AccessTokenId, surveyId, whereClause, orderByClause);
        }
        /// <summary>
        /// Επιστρέφει όλα τα CollectorPayments που έχουν δημιουργηθεί στο σύστημα για τον συγκεκριμένο Collector
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLCollectorPayment> GetCollectorPayments(Int32 collectorId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments, VLPermissions.ClientEnumerateCollectors);
            }
            #endregion

            return SystemDal.GetCollectorPaymentsForCollector(this.AccessTokenId, collectorId, whereClause, orderByClause);
        }

        /// <summary>
        /// Επιστρέφει όλα τα CollectorPayments που έχουν δημιουργηθεί στο σύστημα για τον συγκεκριμένο Collector, ανα σελίδα
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLCollectorPayment> GetCollectorPayments(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments, VLPermissions.ClientEnumerateCollectors);
            }
            #endregion

            return SystemDal.GetCollectorPaymentsForCollector(this.AccessTokenId, collectorId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }

        /// <summary>
        /// Επιστρέφει όλα τα CollectorPayments στα οποία συμμετέχει η συγκεκριμένη πληρωμή
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLCollectorPayment> GetCollectorPaymentsForPayment(Int32 paymentId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments, VLPermissions.ClientEnumerateCollectors);
            }
            #endregion

            return SystemDal.GetCollectorPaymentsForPayment(this.AccessTokenId, paymentId, whereClause, orderByClause);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectorPaymentId"></param>
        /// <returns></returns>
        public VLCollectorPayment GetCollectorPaymentById(Int32 collectorPaymentId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments, VLPermissions.ClientEnumerateCollectors);
            }
            #endregion

            return SystemDal.GetCollectorPaymentById(this.AccessTokenId, collectorPaymentId);

        }

        internal VLCollectorPayment GetCollectorPaymentByCollectorAndPayment(Int32 collectorId, Int32 paymentId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments, VLPermissions.ClientEnumerateCollectors);
            }
            #endregion

            return SystemDal.GetCollectorPaymentByCollectorAndPayment(this.AccessTokenId, collectorId, paymentId);
        }
        /// <summary>
        /// Συνδέει μία διαθέσιμη πληρωμή (payment) του σωστού CreditType, με το συγκεκριμένο Collector.
        /// <para>Κατα την λειτουργία του Collector το σύστημα θα διαχειριστεί (λογιστικά) αυτόματα τους
        /// διαθέσιμους πόρους για την λειτουργία του collector</para>
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="paymentId"></param>
        /// <param name="quantityLimit"></param>
        /// <returns></returns>
        public VLCollectorPayment AddPaymentToCollector(Int32 collectorId, Int32 paymentId, Int32? quantityLimit = null)
        {
            //Διαβάζουμε απο το σύστημα τον collector μας:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorId, BuiltinLanguages.PrimaryLanguage);
            if(collector == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", collectorId));
            }

            //Διαβάζουμε το survey απο το σύστημα
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));
            Boolean _useCredits = false;

            #region SecurityLayer & find if Survey's client UseCredits:
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);

                var profile = SystemDal.GetClientProfileForClient(this.AccessTokenId, survey.Client);
                if (profile == null) throw new VLException(string.Format("Invalid profile for client with id {0}", survey.Client));
                _useCredits = profile.UseCredits;
            }
            else
            {
                if (this.ClientId != survey.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors, VLPermissions.ClientManagePayments);
                _useCredits = this.UseCredits.Value;
            }
            #endregion

            //Μήπως υπάρχει ήδη αυτή η σύνδεση?
            if (SystemDal.GetCollectorPaymentByCollectorAndPaymentImpl(this.AccessTokenId, collectorId, paymentId) != null)
            {
                throw new VLException("The payment is already associated with Collector!");
            }

            //Ο πελάτης χρεώνεται?
            if(_useCredits == false)
            {
                throw new VLException("You cannot add payments. Client's profile does not allow it!");
            }
            //Η λειτουργία του collector, χρεώνεται?
            if (collector.UseCredits == false || collector.CreditType.HasValue == false)
            {
                throw new VLException("You cannot add payments. Collector does not support payments!");
            }

            //Διαβάζουμε την πληρωμή απο το σύστημα:
            var payment = SystemDal.GetPaymentById(this.AccessTokenId, paymentId);
            if (payment == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Payment", paymentId));

            //Ανήκει στον πελάτη του Survey?
            if (payment.Client != survey.Client)
            {
                throw new VLInvalidPaymentException();
            }

            //Η πληρωμή αφορά το creditType που χρεώνουμε στον Collector?
            if (payment.CreditType != collector.CreditType)
            {
                throw new VLInvalidPaymentException(string.Format("The payment's type ({0}) is not compatible with Collector's type ({1})!", payment.CreditType, collector.CreditType));
            }

            //TODO Μία πληρωμή δεν μπορεί να είναι ταυτόχρονα open σε δύο collectors:

            //Υπάρχουν διαθέσιμοι πόροι στην πληρωμή για να συνδεθεί με αυτό τον Collector:
            var _quantity = payment.Quantity - payment.QuantityUsed;
            if (_quantity <= 0)
            {
                throw new VLInvalidPaymentException("Payment has no available credits!");
            }
            if (payment.IsActive == false)
            {
                throw new VLInvalidPaymentException("Payment is inactive!");
            }
            if(quantityLimit.HasValue)
            {
                if(_quantity < quantityLimit.Value)
                {
                    throw new VLInvalidPaymentException("QuantityLimit must be equal or less than the available credits!");
                }
            }



            //Δημιουργούμε ένα νέο VLCollectorPayment
            var collectorPayment = new VLCollectorPayment();
            collectorPayment.Collector = collector.CollectorId;
            collectorPayment.Payment = payment.PaymentId;
            collectorPayment.QuantityLimit = quantityLimit;
            collectorPayment.QuantityUsed = 0;
            collectorPayment.FirstChargeDt = null;
            collectorPayment.LastChargeDt = null;
            collectorPayment.IsActive = true;
            collectorPayment.IsUsed = false;
            collectorPayment = SystemDal.CreateCollectorPayment(this.AccessTokenId, collectorPayment);


            return collectorPayment;
        }

        public VLCollectorPayment UpdateCollectorPayment(VLCollectorPayment collectorPayment)
        {
            if (collectorPayment == null) throw new ArgumentNullException("collectorPayment");

            //Διαβάζουμε απο το σύστημα τον collector μας:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorPayment.Collector, BuiltinLanguages.PrimaryLanguage);
            if (collector == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", collectorPayment.Collector));
            }

            //Διαβάζουμε το survey απο το σύστημα
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));
            Boolean _useCredits = false;

            #region SecurityLayer & find if Survey's client UseCredits:
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);

                var profile = SystemDal.GetClientProfileForClient(this.AccessTokenId, survey.Client);
                if (profile == null) throw new VLException(string.Format("Invalid profile for client with id {0}", survey.Client));
                _useCredits = profile.UseCredits;
            }
            else
            {
                if (this.ClientId != survey.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors, VLPermissions.ClientManagePayments);
                _useCredits = this.UseCredits.Value;
            }
            #endregion


            //διαβάζουμε απο το σύστημα το CollectorPayment:
            var existingCollectorPayment = SystemDal.GetCollectorPaymentById(this.AccessTokenId, collectorPayment.CollectorPaymentId);
            if(existingCollectorPayment == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "CollectorPayment", collectorPayment.CollectorPaymentId));
            }

            //το update θα γίνει με βαση την εγγραφή που διαβάσαμε απο το σύστημα:
            existingCollectorPayment.QuantityReserved = collectorPayment.QuantityReserved;
            existingCollectorPayment.QuantityLimit = collectorPayment.QuantityLimit;

            return SystemDal.UpdateCollectorPayment(this.AccessTokenId, existingCollectorPayment);
        }

        /// <summary>
        /// Για να αφαιρέσουμε μία συνδεδεμένη πληρωμή απο ένα collector, πρέπει αυτή να μην έχει χρησιμοποηθεί καθόλου.
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="paymentId"></param>
        public void RemovePaymentFromCollector(Int32 collectorId, Int32 paymentId)
        {
            //διαβάζουμε απο το σύστημά μας το CollectorPayment:
            var collectorPayment = SystemDal.GetCollectorPaymentByCollectorAndPayment(this.AccessTokenId, collectorId, paymentId);
            if(collectorPayment == null)
            {
                throw new VLException(string.Format("There is no CollectorPayment for collector={0} and payment={1}", collectorId, paymentId));
            }

            RemovePaymentFromCollectorImpl(collectorPayment);
        }


        public void RemovePaymentFromCollector(VLCollectorPayment collectorPayment)
        {
            if (collectorPayment == null) throw new ArgumentNullException("collectorPayment");

            RemovePaymentFromCollector(collectorPayment.CollectorPaymentId);
        }
        public void RemovePaymentFromCollector(Int32 collectorPaymentId)
        {
            //διαβάζουμε απο το σύστημά μας το CollectorPayment:
            var collectorPayment = SystemDal.GetCollectorPaymentById(this.AccessTokenId, collectorPaymentId);
            if (collectorPayment == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "CollectorPayment", collectorPaymentId));
            }

            RemovePaymentFromCollectorImpl(collectorPayment);
        }

        void RemovePaymentFromCollectorImpl(VLCollectorPayment collectorPayment)
        {
            //Διαβάζουμε απο το σύστημα τον collector μας:
            var collector = SurveysDal.GetCollectorById(this.AccessTokenId, collectorPayment.Collector, BuiltinLanguages.PrimaryLanguage);
            if (collector == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Collector", collectorPayment.Collector));
            }

            //Διαβάζουμε το survey απο το σύστημα
            var survey = SurveysDal.GetSurveyById(this.AccessTokenId, collector.Survey, BuiltinLanguages.PrimaryLanguage);
            if (survey == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "survey", collector.Survey));
            Boolean _useCredits = false;

            #region SecurityLayer & find if Survey's client UseCredits:
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);

                var profile = SystemDal.GetClientProfileForClient(this.AccessTokenId, survey.Client);
                if (profile == null) throw new VLException(string.Format("Invalid profile for client with id {0}", survey.Client));
                _useCredits = profile.UseCredits;
            }
            else
            {
                if (this.ClientId != survey.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors, VLPermissions.ClientManagePayments);
                _useCredits = this.UseCredits.Value;
            }
            #endregion

            //εάν έχει χρησιμοποιηθεί, δεν διαγράφεται:
            if (collectorPayment.IsUsed == true || collectorPayment.QuantityUsed > 0)
            {
                throw new VLException("Payment cannot be deassociated from the Collector, because it has been used!");
            }
            if(collectorPayment.QuantityReserved >  0)
            {
                throw new VLException("Payment cannot be deassociated from the Collector, because message(s) are scheduled and charged from it!!");
            }
            //εάν ο collector είναι ανοιχτός, τότε δεν διαγράφεται:
            if(collector.CollectorType == CollectorType.Email)
            {

            }
            else
            {
                
                if (collector.Status == CollectorStatus.Open)
                {
                    throw new VLException("Payment cannot be deassociated from the Collector, because the collector is Open! Close the Collector first, and try again!");
                }
            }

            //Προχωρούμε στην διαγραφή του collectorPayment:
            SystemDal.DeleteCollectorPayment(this.AccessTokenId, collectorPayment.CollectorPaymentId);
        }

        public VLCollectorPayment ActivateCollectorPayment(Int32 collectorPaymentId)
        {
            //διαβάζουμε απο το σύστημά μας το CollectorPayment:
            var collectorPayment = SystemDal.GetCollectorPaymentById(this.AccessTokenId, collectorPaymentId);
            if (collectorPayment == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "CollectorPayment", collectorPaymentId));
            }

            return ActivateCollectorPayment(collectorPayment);
        }
        public VLCollectorPayment ActivateCollectorPayment(VLCollectorPayment collectorPayment)
        {
            if (collectorPayment == null) throw new ArgumentNullException("collectorPayment");

            #region SecurityLayer & find if Survey's client UseCredits:
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                var payment = SystemDal.GetPaymentById(this.AccessTokenId, collectorPayment.Payment);
                if (payment == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Payment", collectorPayment.Payment));

                if (this.ClientId != payment.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors, VLPermissions.ClientManagePayments);
            }
            #endregion

            if (collectorPayment.IsActive == true)
                return collectorPayment;

            collectorPayment.IsActive = true;
            return SystemDal.UpdateCollectorPayment(this.AccessTokenId, collectorPayment);
        }
        
        public VLCollectorPayment DeactivateCollectorPayment(Int32 collectorPaymentId)
        {
            //διαβάζουμε απο το σύστημά μας το CollectorPayment:
            var collectorPayment = SystemDal.GetCollectorPaymentById(this.AccessTokenId, collectorPaymentId);
            if (collectorPayment == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "CollectorPayment", collectorPaymentId));
            }

            return DeactivateCollectorPayment(collectorPayment);
        }
        public VLCollectorPayment DeactivateCollectorPayment(VLCollectorPayment collectorPayment)
        {
            if (collectorPayment == null) throw new ArgumentNullException("collectorPayment");

            #region SecurityLayer & find if Survey's client UseCredits:
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                var payment = SystemDal.GetPaymentById(this.AccessTokenId, collectorPayment.Payment);
                if (payment == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Payment", collectorPayment.Payment));

                if (this.ClientId != payment.Client)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientManageCollectors, VLPermissions.ClientManagePayments);
            }
            #endregion

            if (collectorPayment.IsActive == false)
                return collectorPayment;

            collectorPayment.IsActive = false;
            return SystemDal.UpdateCollectorPayment(this.AccessTokenId, collectorPayment);
        }




        #endregion

        #region VLCharge
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLChargedCollector> GetChargedCollectors(Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetChargedCollectors(this.AccessTokenId, clientId, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLChargedCollector> GetChargedCollectorsPaged(Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetChargedCollectorsPaged(this.AccessTokenId, clientId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }



        /// <summary>
        /// Επιστρέφει όλες τις χρεώσεις που αφορούν την συγκεκριμένη Πληρωμή
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public Collection<VLCharge> GetCharges(Int32 paymentId)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetCharges(this.AccessTokenId, paymentId);
        }
        /// <summary>
        /// Επιστρέφει όλες τις χρεώσεις που υπάρχουν για τον συγκεκριμένο Πελάτη
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLCharge> GetChargesForClient(Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetChargesForClient(this.AccessTokenId, clientId, whereClause, orderByClause);
        }
        /// <summary>
        /// Επιστρέφει όλες τις χρεώσεις που υπάρχουν για τον συγκεκριμένο Πελάτη, ανα σελίδα
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLCharge> GetChargesForClient(Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                if (this.ClientId != clientId)
                {
                    throw new VLAccessDeniedException();
                }
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetChargesPagedForClient(this.AccessTokenId, clientId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLCharge> GetChargesForCollector(Int32 collectorId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetChargesForCollector(this.AccessTokenId, collectorId, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectorId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLCharge> GetChargesForCollector(Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.EnumerateClients | VLPermissions.EnumeratePayments, VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService);
            }
            else
            {
                CheckPermissions(VLPermissions.ClientFullControl, VLPermissions.ClientEnumeratePayments);
            }
            #endregion

            return SystemDal.GetChargesPagedForCollector(this.AccessTokenId, collectorId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }



        /// <summary>
        /// Πραγματοποιεί την χρέωση για την αποστολή ενός email.
        /// <para>Αυξάνει κατα μία μονάδα το QuantityUsed στον πίνακα Payments</para>
        /// <para>Αυξάνει κατα μία μονάδα το QuantityUsed στον πίνακα CollectorPayments</para>
        /// <para>Ενημερώνει τα πεδία FirstChargeDt, LastChargeDt στους πίνακες CollectorPayments και SurveyCollectors</para>
        /// <para>Επίσης εαν η πληρωμή δεν διαθέτει άλλα credits, τότε την απενεργοποιεί (Payment.IsActive == false)</para>
        /// </summary>
        /// <param name="collectorPaymentId"></param>
        /// <param name="collectorId"></param>
        /// <param name="messageId"></param>
        /// <param name="recipientId"></param>
        /// <returns></returns>
        internal bool ChargePaymentForEmail(Int32 collectorPaymentId, Int32 collectorId, Int32 messageId, Int64 recipientId)
        {
            return SystemDal.ChargePaymentForEmail(this.AccessTokenId, collectorPaymentId, collectorId, messageId, recipientId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectorPaymentId"></param>
        /// <param name="collectorId"></param>
        /// <param name="messageId"></param>
        /// <param name="recipientId"></param>
        /// <returns></returns>
        internal bool UnchargePaymentForEmail(Int32 collectorPaymentId, Int32 collectorId, Int32 messageId, Int64 recipientId)
        {
            return SystemDal.UnchargePaymentForEmail(this.AccessTokenId, collectorPaymentId, collectorId, messageId, recipientId);
        }
        #endregion

        #region VLClientLists
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLClientList> GetClientLists(VLClient client, string whereClause = null, string orderByClause = null)
        {
            if (client == null) throw new ArgumentNullException("client");
            return GetClientLists(client.ClientId, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLClientList> GetClientLists(Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientEnumerateLists);
            #endregion

            return SystemDal.GetClientLists(this.AccessTokenId, clientId, whereClause, orderByClause);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLClientList> GetClientLists(Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientEnumerateLists);
            #endregion

            return SystemDal.GetClientLists(this.AccessTokenId, clientId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public VLClientList GetClientListById(Int32 listId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SystemDal.GetClientListById(this.AccessTokenId, listId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public VLClientList CreateClientList(Int32 clientId, string name)
        {
            VLClientList item = new VLClientList();
            item.Client = clientId;
            item.Name = name;

            return CreateClientList(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        internal VLClientList CreateClientList(VLClientList list)
        {
            if (list == null) throw new ArgumentNullException("list");
            list.ValidateInstance();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion

            //Ελέγχουμε το όνομα της λίστας, να είναι μοναδικό μέσα στα όρια του πελάτη:
            var existingItem = SystemDal.GetClientListByName(AccessTokenId, list.Client, list.Name);
            if (existingItem != null)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", list.Name));
            }

            //Δημιουργούμε την λίστα
            return SystemDal.CreateClientList(this.AccessTokenId, list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public VLClientList UpdateClientList(VLClientList list)
        {
            if (list == null) throw new ArgumentNullException("list");
            list.ValidateInstance();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion


            //Ελέγχουμε το όνομα της λίστας, να είναι μοναδικό μέσα στα όρια του πελάτη:
            var existingItem = SystemDal.GetClientListByName(AccessTokenId, list.Client, list.Name);
            if (existingItem != null && existingItem.ListId != list.ListId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", list.Name));
            }
            if (existingItem == null)
            {
                existingItem = SystemDal.GetClientListById(this.AccessTokenId, list.ListId);
            }
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientList", list.ListId));

            //
            existingItem.Name = list.Name;
            return SystemDal.UpdateClientList(this.AccessTokenId, existingItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public void DeleteClientList(VLClientList list)
        {
            if (list == null) throw new ArgumentNullException("list");
            DeleteClientList(list.ListId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listId"></param>
        public void DeleteClientList(Int32 listId)
        {
            var existingItem = SystemDal.GetClientListById(this.AccessTokenId, listId);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientList", listId));

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion

            //Μπορούμε να διαγράψουμε την λίστα


            SystemDal.DeleteClientList(this.AccessTokenId, existingItem.ListId, existingItem.LastUpdateDT);
        }

        #endregion


        #region VLContacts
        public Collection<VLContact> GetContacts(VLClientList list, string whereClause = null, string orderByClause = null)
        {
            if (list == null) throw new ArgumentNullException("list");
            return GetContacts(list.ListId, whereClause, orderByClause);
        }
        public Collection<VLContact> GetContacts(Int32 listId, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientEnumerateLists);
            #endregion

            return SystemDal.GetContacts(this.AccessTokenId, listId, whereClause, orderByClause);
        }
        public Collection<VLContact> GetContacts(Int32 listId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientEnumerateLists);
            #endregion

            return SystemDal.GetContacts(this.AccessTokenId, listId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        public int GetContactsCount(Int32 listId, string whereClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientEnumerateLists);
            #endregion

            return SystemDal.GetContactsCount(this.AccessTokenId, listId, whereClause);
        }

        public Collection<VLContact> GetOptedOutContacts(VLClientList list, string orderByClause = null)
        {
            if (list == null) throw new ArgumentNullException("list");
            return GetOptedOutContacts(list.ListId, orderByClause);
        }
        public Collection<VLContact> GetOptedOutContacts(Int32 listId, string orderByClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientEnumerateLists);
            #endregion

            return SystemDal.GetContacts(this.AccessTokenId, listId, "where [AttributeFlags] & /*IsOptedOut*/2 = 2", orderByClause);
        }
        public Collection<VLContact> GetOptedOutContacts(Int32 listId, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientEnumerateLists);
            #endregion

            return SystemDal.GetContacts(this.AccessTokenId, listId, pageIndex, pageSize, ref totalRows, "where [AttributeFlags] & /*IsOptedOut*/2 = 2", orderByClause);
        }
        public int GetOptedOutContactsCount(Int32 listId, string whereClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientEnumerateLists);
            #endregion

            return SystemDal.GetContactsCount(this.AccessTokenId, listId, "where [AttributeFlags] & /*IsOptedOut*/2 = 2");
        }

        public Collection<VLContact> GetBouncedContacts(VLClientList list, string orderByClause = null)
        {
            if (list == null) throw new ArgumentNullException("list");
            return GetBouncedContacts(list.ListId, orderByClause);
        }
        public Collection<VLContact> GetBouncedContacts(Int32 listId, string orderByClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientEnumerateLists);
            #endregion

            return SystemDal.GetContacts(this.AccessTokenId, listId, "where [AttributeFlags] & /*IsBouncedEmail*/4 = 4", orderByClause);
        }
        public Collection<VLContact> GetBouncedContacts(Int32 listId, int pageIndex, int pageSize, ref int totalRows, string orderByClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientEnumerateLists);
            #endregion

            return SystemDal.GetContacts(this.AccessTokenId, listId, pageIndex, pageSize, ref totalRows, "where [AttributeFlags] & /*IsBouncedEmail*/4 = 4", orderByClause);
        }
        public int GetBouncedContactsCount(Int32 listId, string whereClause = null)
        {
            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientEnumerateLists);
            #endregion

            return SystemDal.GetContactsCount(this.AccessTokenId, listId, "where [AttributeFlags] & /*IsBouncedEmail*/4 = 4");
        }

        public VLContact GetContactById(Int32 contactId)
        {
            #region SecurityLayer
            //PASS THROUGH
            #endregion

            return SystemDal.GetContactById(this.AccessTokenId, contactId);
        }

        /// <summary>
        /// Δημιουργεί μία νέα επαφή μέσα στην συγκεκριμένη λίστα.
        /// <para>η μοναδική απαίτηση είναι το email να έχει έγκυρη μορφή, και μέσα στα πλαίσια της συγκεκριμένης λίστα να είναι μοναδικό (case insensitive).</para>
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastname"></param>
        /// <param name="organization"></param>
        /// <param name="title"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        public VLContact CreateContact(Int32 listId, string email, string firstName, string lastname, string organization = null, string title = null, string department = null)
        {
            var item = new VLContact();
            item.ListId = listId;
            item.FirstName = firstName;
            item.LastName = lastname;
            item.Email = email;
            item.Organization = organization;
            item.Title = title;
            item.Department = department;

            return CreateContact(item);
        }

        /// <summary>
        /// Δημιουργεί μία νέα επαφή μέσα σε συγκεκριμένη λίστα.
        /// <para>Το email μέσα σε κάθε λίστα πρέπει να είναι μοναδικό!</para>
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        internal VLContact CreateContact(VLContact contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");
            contact.ValidateInstance();


            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion


            //Τραβάμε την λίστα απο το σύστημα:
            var list = SystemDal.GetClientListById(this.AccessTokenId, contact.ListId);
            if (list == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientList", contact.ListId));

            //Διορθώνουμε το clientid στο contact:
            contact.ClientId = list.Client;

            //Ελέγχουμε εάν το email έχει κανονική μορφή:
            if(!Utility.EmailIsValid(contact.Email))
            {
                throw new VLException(string.Format("Invalid email address '{0}'!", contact.Email));
            }

            //Ελέγχουμε εάν το email είναι μοναδικό σε αυτή την λίστα:
            var existingItem = SystemDal.GetContactByEmail(this.AccessTokenId, contact.ListId, contact.Email);
            if(existingItem != null)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Email", contact.Email));
            }

            //Αποθηκεύουμε
            return SystemDal.CreateContact(this.AccessTokenId, contact);
        }
        public VLContact UpdateContact(VLContact contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");
            contact.ValidateInstance();

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion

            //Ελέγχουμε εάν το email έχει κανονική μορφή:
            if (!Utility.EmailIsValid(contact.Email))
            {
                throw new VLException(string.Format("Invalid email address '{0}'!", contact.Email));
            }

            //Ελέγχουμε εάν το email είναι μοναδικό σε αυτή την λίστα:
            var existingItem = SystemDal.GetContactByEmail(this.AccessTokenId, contact.ListId, contact.Email);
            if (existingItem != null && existingItem.ContactId != contact.ContactId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Email", contact.Email));
            }
            if (existingItem == null)
            {
                existingItem = SystemDal.GetContactById(this.AccessTokenId, contact.ContactId);
            }
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Contact", contact.ContactId));

            //
            existingItem.Organization = contact.Organization;
            existingItem.Title = contact.Title;
            existingItem.Department = contact.Department;
            existingItem.FirstName = contact.FirstName;
            existingItem.LastName = contact.LastName;
            existingItem.Email = contact.Email;
            existingItem.AttributeFlags = contact.AttributeFlags;
            existingItem.Comment = contact.Comment;

            return SystemDal.UpdateContact(this.AccessTokenId, existingItem);
        }

        public void DeleteContact(VLContact contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");
            DeleteContact(contact.ContactId);
        }
        public void DeleteContact(Int32 contactId)
        {
            var existingItem = SystemDal.GetContactById(this.AccessTokenId, contactId);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Contact", contactId));

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion

            //Μπορούμε να διαγράψουμε την επαφή

            SystemDal.DeleteContact(this.AccessTokenId, existingItem.ContactId, existingItem.LastUpdateDT);
        }


        /// <summary>
        /// Κάνει bounce σε όλες τις λίστες του Πελάτη το συγκεκριμένο email επαφής.
        /// <para>Μπορει να βρεθεί μόνο σε μία λίστα ή σε περισσότερες</para>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        internal int BounceContacts(VLClient client, string email)
        {
            if (client == null) throw new ArgumentNullException("client");
            return BounceContacts(client.ClientId, email);
        }
        /// <summary>
        /// Κάνει bounce σε όλες τις λίστες του Πελάτη το συγκεκριμένο email επαφής.
        /// <para>Μπορει να βρεθεί μόνο σε μία λίστα ή σε περισσότερες</para>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        internal int BounceContacts(Int32 clientId, string email)
        {
            /*Βρίσκουμε τον client στο σύστημα (για να βεβαιωθούμε ότι μας πέρασε έγκυρο clientId και όχι κάποιο listId!)*/
            var client = SystemDal.GetClientById(this.AccessTokenId, clientId);
            if (client == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "client", clientId));

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion


            /*Θέλουμε όλα τα contacs που φέρουν αυτό το email, αδιαφορώντας για την λίστα στην οποίαανήκουν:*/
            var contacts = SystemDal.GetContactsForClientByEmail(this.AccessTokenId, clientId, email);
            int numberOfContacts = 0;

            foreach(var contact in contacts)
            {
                if(contact.IsBouncedEmail == false)
                {
                    numberOfContacts++;

                    contact.IsBouncedEmail = true;
                    SystemDal.UpdateContact(this.AccessTokenId, contact);
                }
            }

            return numberOfContacts;
        }

        /// <summary>
        /// Κάνει OptedOut σε όλες τις λίστες του Πελάτη το συγκεκριμένο email επαφής.
        /// <para>Μπορει να βρεθεί μόνο σε μία λίστα ή σε περισσότερες</para>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        internal int OptOutContacts(VLClient client, string email)
        {
            if (client == null) throw new ArgumentNullException("client");
            return OptOutContacts(client.ClientId, email);
        }
        /// <summary>
        /// Κάνει OptedOut σε όλες τις λίστες του Πελάτη το συγκεκριμένο email επαφής.
        /// <para>Μπορει να βρεθεί μόνο σε μία λίστα ή σε περισσότερες</para>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        internal int OptOutContacts(Int32 clientId, string email)
        {
            /*Βρίσκουμε τον client στο σύστημα (για να βεβαιωθούμε ότι μας πέρασε έγκυρο clientId και όχι κάποιο listId!)*/
            var client = SystemDal.GetClientById(this.AccessTokenId, clientId);
            if (client == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "client", clientId));


            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion

            /*Θέλουμε όλα τα contacs που φέρουν αυτό το email, αδιαφορώντας για την λίστα στην οποίαανήκουν:*/
            var contacts = SystemDal.GetContactsForClientByEmail(this.AccessTokenId, clientId, email);
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

            return numberOfContacts;
        }


        /// <summary>
        /// Clear out all the contacts in your list.
        /// </summary>
        /// <param name="listId"></param>
        public int RemoveAllContactsFromList(Int32 listId)
        {
            /*Βρίσκουμε την λίστα στο σύστημα (για να βεβαιωθούμε ότι μας πέρασε έγκυρο listId!)*/
            var list = SystemDal.GetClientListById(this.AccessTokenId, listId);
            if (list == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientList", listId));

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion


            return SystemDal.RemoveAllContactsFromList(this.AccessTokenId, list.ListId);
        }
        /// <summary>
        /// Remove all opted-out contacts from this list
        /// </summary>
        /// <param name="listId"></param>
        public int RemoveAllOptedOutContactsFromList(Int32 listId)
        {
            /*Βρίσκουμε την λίστα στο σύστημα (για να βεβαιωθούμε ότι μας πέρασε έγκυρο listId!)*/
            var list = SystemDal.GetClientListById(this.AccessTokenId, listId);
            if (list == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientList", listId));

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion


            return SystemDal.RemoveAllOptedOutContactsFromList(this.AccessTokenId, list.ListId);
        }
        /// <summary>
        /// Remove all non-deliverable contacts from this list.
        /// </summary>
        /// <param name="listId"></param>
        public int RemoveAllBouncedContactsFromList(Int32 listId)
        {
            /*Βρίσκουμε την λίστα στο σύστημα (για να βεβαιωθούμε ότι μας πέρασε έγκυρο listId!)*/
            var list = SystemDal.GetClientListById(this.AccessTokenId, listId);
            if (list == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientList", listId));

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion


            return SystemDal.RemoveAllBouncedContactsFromList(this.AccessTokenId, list.ListId);
        }        
        /// <summary>
        /// Remove all contacts that match the domain name
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="domainName"></param>
        public int RemoveByDomainContactsFromList(Int32 listId, string domainName)
        {
            Utility.CheckParameter(ref domainName, true, true, true, 128, "domainName");

            /*Βρίσκουμε την λίστα στο σύστημα (για να βεβαιωθούμε ότι μας πέρασε έγκυρο listId!)*/
            var list = SystemDal.GetClientListById(this.AccessTokenId, listId);
            if (list == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientList", listId));

            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion


            return SystemDal.RemoveByDomainContactsFromList(this.AccessTokenId, list.ListId, domainName);
        }



        public ContactImportResult ImportContactsFromString(Int32 listId, string buffer, ContactImportOptions options)
        {
            if (string.IsNullOrWhiteSpace(buffer)) throw new ArgumentNullException("buffer");
            if (options == null) throw new ArgumentNullException("options");

            using (Stream s = Utility.GenerateStreamFromString(buffer))
            {
                return ImportContactsFromCSV(listId, s, options);
            }
        }

        public ContactImportResult ImportContactsFromCSV(Int32 listId, Stream stream, ContactImportOptions options)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            if (options == null) throw new ArgumentNullException("options");


            #region SecurityLayer
            CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.EnumerateClients, VLPermissions.ClientManageLists);
            #endregion
            

            //Τραβάμε την λίστα απο το σύστημα:
            var list = SystemDal.GetClientListById(this.AccessTokenId, listId);
            if (list == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "ClientList", listId));


            ContactImportResult result = new ContactImportResult();
            try
            {
                using(TextReader tr = new StreamReader(stream))
                {
                    if(options.HasHeaderRecord)
                    {
                        tr.ReadLine();
                    }
                    #region
                    var csv = new CsvReader(tr, CultureInfo.InvariantCulture);
                    csv.Configuration.Delimiter = options.DelimiterCharacter;
                    csv.Configuration.HasHeaderRecord = false;
                    csv.Configuration.TrimOptions = CsvHelper.Configuration.TrimOptions.Trim;
                    csv.Configuration.BadDataFound = null;


                    var contacts = new VLContact[8] { new VLContact(), new VLContact(), new VLContact(), new VLContact(), new VLContact(), new VLContact(), new VLContact(), new VLContact() };
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
                            contacts[_cindex].InitializeInstance(list.Client, list.ListId, _email);
                            contacts[_cindex].HasImportMark = true;

                            if (options.FirstNameOrdinal > 0)
                            {
                                if (csv.TryGetField<string>(options.FirstNameOrdinal - 1, out _value)) contacts[_cindex].FirstName = _value;
                            }
                            if (options.LastNameOrdinal > 0)
                            {
                                if (csv.TryGetField<string>(options.LastNameOrdinal - 1, out _value)) contacts[_cindex].LastName = _value;
                            }
                            if (options.TitleOrdinal > 0)
                            {
                                if (csv.TryGetField<string>(options.TitleOrdinal - 1, out _value)) contacts[_cindex].Title = _value;
                            }
                            if (options.OrganizationOrdinal > 0)
                            {
                                if (csv.TryGetField<string>(options.OrganizationOrdinal - 1, out _value)) contacts[_cindex].Organization = _value;
                            }
                            if (options.DepartmentOrdinal > 0)
                            {
                                if (csv.TryGetField<string>(options.DepartmentOrdinal - 1, out _value)) contacts[_cindex].Department = _value;
                            }
                            if (options.CommentOrdinal > 0)
                            {
                                if (csv.TryGetField<string>(options.CommentOrdinal - 1, out _value)) contacts[_cindex].Comment = _value;
                            }


                            if (_cindex >= 7)
                            {
                                Int32 successImports = 0, sameEmails = 0;
                                //Αποθηκεύουμε
                                SystemDal.ImportContact(this.Principal, contacts, _cindex + 1, ref successImports, ref sameEmails);
                                result.SuccesfullImports += successImports;
                                result.SameEmails += sameEmails;

                                _cindex = -1;
                            }
                        }
                        catch (Exception ex)
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
                        SystemDal.ImportContact(this.Principal, contacts, _cindex + 1, ref successImports, ref sameEmails);
                        result.SuccesfullImports += successImports;
                        result.SameEmails += sameEmails;
                    }
                    #endregion
                }
            }
            finally
            {
                Int32 optedOutContacts = 0, bouncedContacts = 0, totalContacts = 0;

                SystemDal.ImportContactsFinalize(this.Principal, list.Client, list.ListId, ref optedOutContacts, ref bouncedContacts, ref totalContacts);
                result.OptedOutEmails = optedOutContacts;
                result.BouncedEmails = bouncedContacts;

                //Κάνουμε update το TotalContacts της λίστας μας
                //SystemDal.UpdateContactCounter(this.Principal, list.ListId);
            }


            return result;
        }


        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLLogin> GetLogins(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            return SystemDal.GetLogins(this.AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public VLLogin GetLoginById(Int32 loginId)
        {
            return SystemDal.GetLoginById(this.AccessTokenId, loginId);
        }
        #endregion
    }
}
