using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite1
{
    [TestClass]
    public class SystemTests02 : AdminBaseClass
    {

        [TestMethod, Description("Builtin SystemUsers")]
        public void SystemUsersTest01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                /*
                 * Στο σύστημά μας, υπάρχουν 5 system users, τα οποία όμως δεν διαβάζονται απο το API 
                 * του συστήματος. Πρόκειται για 5 κρυφά accounts.
                 */
                Assert.IsTrue(systemManager.GetSystemUsersCount() == 0);
                Assert.IsTrue(systemManager.GetSystemUsers().Count == 0);

                //Διαβάζουμε το SystemAdmin:
                var systemAdmin = systemManager.GetSystemUserById(BuiltinSystemUsers.SystemAdmin.UserId);
                Assert.IsNotNull(systemAdmin);
                Assert.IsTrue(systemAdmin.DefaultLanguage == BuiltinLanguages.Invariant.LanguageId);
                Assert.AreEqual<string>("SystemAdmin", systemAdmin.FirstName);
                Assert.AreEqual<string>("Account", systemAdmin.LastName);
                Assert.AreEqual<string>("sysadmin@dummy.com", systemAdmin.Email);
                Assert.IsTrue(systemAdmin.IsActive);
                Assert.IsTrue(systemAdmin.IsBuiltIn);
                Assert.IsTrue(systemAdmin.AttributeFlags == 0);
                Assert.IsTrue(systemAdmin.Role == BuiltinRoles.SystemAdmin.RoleId);
                Assert.IsNull(systemAdmin.Notes);

                var svdSystemUser = systemManager.GetSystemUserByEmail(systemAdmin.Email);
                Assert.AreEqual<VLSystemUser>(systemAdmin, svdSystemUser);
                svdSystemUser = systemManager.GetSystemUserFromAccessToken(sysadmin.AccessTokenId);
                Assert.AreEqual<VLSystemUser>(systemAdmin, svdSystemUser);
                svdSystemUser = systemManager.GetSystemUserByLogOnToken("sysadmin");
                Assert.AreEqual<VLSystemUser>(systemAdmin, svdSystemUser);


                //Διαβάζουμε το Developer:
                var developer = systemManager.GetSystemUserById(BuiltinSystemUsers.Developer.UserId);
                Assert.IsNotNull(developer);
                Assert.IsTrue(developer.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<string>("Developer", developer.FirstName);
                Assert.AreEqual<string>("Account", developer.LastName);
                Assert.AreEqual<string>("developer@dummy.com", developer.Email);
                Assert.IsTrue(developer.IsActive);
                Assert.IsTrue(developer.IsBuiltIn);
                Assert.IsTrue(developer.AttributeFlags == 0);
                Assert.IsTrue(developer.Role == BuiltinRoles.Developer.RoleId);
                Assert.IsNull(developer.Notes);

                svdSystemUser = systemManager.GetSystemUserByEmail(developer.Email);
                Assert.AreEqual<VLSystemUser>(developer, svdSystemUser);
                svdSystemUser = systemManager.GetSystemUserFromAccessToken(dev.AccessTokenId);
                Assert.AreEqual<VLSystemUser>(developer, svdSystemUser);
                svdSystemUser = systemManager.GetSystemUserByLogOnToken("developer");
                Assert.AreEqual<VLSystemUser>(developer, svdSystemUser);

                //δεν μπορούμε να διαγράψουμε builtin SystemUser:
                _EXECUTEAndCATCHType(delegate { systemManager.DeleteSystemUser(systemAdmin); }, typeof(VLException));
                _EXECUTEAndCATCHType(delegate { systemManager.DeleteSystemUser(developer); }, typeof(VLException));
            }
            finally
            {

            }
        }


        [TestMethod, Description("CRUD operations for SystemUsers")]
        public void SystemUsersTest02()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                /*
                 * Στο σύστημά μας, υπάρχουν 5 system users, τα οποία όμως δεν διαβάζονται απο το API 
                 * του συστήματος. Πρόκειται για 5 κρυφά accounts.
                 */
                Assert.IsTrue(systemManager.GetSystemUsers().Count == 0);

                //Δημιουργώ ένα SystemUser:
                var user1 = systemManager.CreateSystemUser("George", "Milonakis", BuiltinRoles.Administrator.RoleId, "punkis@optonline.net");
                Assert.IsNotNull(user1);
                Assert.IsTrue(user1.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<string>("George", user1.FirstName);
                Assert.AreEqual<string>("Milonakis", user1.LastName);
                Assert.AreEqual<string>("punkis@optonline.net", user1.Email);
                Assert.IsFalse(user1.IsActive);
                Assert.IsFalse(user1.IsBuiltIn);
                Assert.IsTrue(user1.AttributeFlags == 0);
                Assert.IsTrue(user1.Role == BuiltinRoles.Administrator.RoleId);
                Assert.IsNull(user1.Notes);

                var svdSystemUser = systemManager.GetSystemUserByEmail(user1.Email);
                Assert.AreEqual<VLSystemUser>(user1, svdSystemUser); 
                svdSystemUser = systemManager.GetSystemUserById(user1.UserId);
                Assert.AreEqual<VLSystemUser>(user1, svdSystemUser);


                //Τώρα βλέπουμε 1 (ένα) SystemUser:
                Assert.IsTrue(systemManager.GetSystemUsers().Count == 1);


                //το σύστημα ελέγχει για παλαβές email addresses:
                user1.Email = string.Empty;
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateSystemUser(user1); }, typeof(VLException));
                user1.Email = "aqedfqeqer";
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateSystemUser(user1); }, typeof(VLException));
                user1.Email = "qweeeeeee@eeee";
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateSystemUser(user1); }, typeof(VLException));

                //Δεν μπορώ να χρησιμοποιήσω email που πάρχει ήδη:
                _EXECUTEAndCATCHType(delegate { systemManager.CreateSystemUser("George", "Milonakis", BuiltinRoles.Administrator.RoleId, "punkis@optonline.net"); }, typeof(VLException));


                //Δημιουργώ ένα SystemUser:
                var user2= systemManager.CreateSystemUser("qwerty", "asdfghhh", BuiltinRoles.Developer.RoleId, "milonakis@thetest12.gr");
                Assert.IsNotNull(user2);
                Assert.IsTrue(user2.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<string>("qwerty", user2.FirstName);
                Assert.AreEqual<string>("asdfghhh", user2.LastName);
                Assert.AreEqual<string>("milonakis@thetest12.gr", user2.Email);
                Assert.IsFalse(user2.IsActive);
                Assert.IsFalse(user2.IsBuiltIn);
                Assert.IsTrue(user2.AttributeFlags == 0);
                Assert.IsTrue(user2.Role == BuiltinRoles.Developer.RoleId);
                Assert.IsNull(user2.Notes);

                svdSystemUser = systemManager.GetSystemUserByEmail(user2.Email);
                Assert.AreEqual<VLSystemUser>(user2, svdSystemUser);
                svdSystemUser = systemManager.GetSystemUserById(user2.UserId);
                Assert.AreEqual<VLSystemUser>(user2, svdSystemUser);

                //Υπάρχουν 2 SystemUsers:
                Assert.IsTrue(systemManager.GetSystemUsers().Count == 2);


                //UPDATE:
                user2.FirstName = "Sandro";
                user2.LastName = "Henkel";
                user2.Email = "sandrokel@gmail.com";
                user2.Role = BuiltinRoles.Administrator.RoleId;
                user2.DefaultLanguage = BuiltinLanguages.German.LanguageId;
                user2 = systemManager.UpdateSystemUser(user2);
                Assert.IsTrue(user2.DefaultLanguage == BuiltinLanguages.German.LanguageId);
                Assert.AreEqual<string>("Sandro", user2.FirstName);
                Assert.AreEqual<string>("Henkel", user2.LastName);
                Assert.AreEqual<string>("sandrokel@gmail.com", user2.Email);
                Assert.IsFalse(user2.IsActive);
                Assert.IsFalse(user2.IsBuiltIn);
                Assert.IsTrue(user2.AttributeFlags == 0);
                Assert.IsTrue(user2.Role == BuiltinRoles.Administrator.RoleId);
                Assert.IsNull(user2.Notes);

                svdSystemUser = systemManager.GetSystemUserByEmail(user2.Email);
                Assert.AreEqual<VLSystemUser>(user2, svdSystemUser);


                //Υπάρχουν 2 SystemUsers:
                Assert.IsTrue(systemManager.GetSystemUsers().Count == 2);


                systemManager.DeleteSystemUser(user2);
                Assert.IsNull(systemManager.GetSystemUserById(user2.UserId));
            }
            finally
            {
                var systemUsers = systemManager.GetSystemUsers();
                foreach(var item in systemUsers)
                {
                    if (item.IsBuiltIn)
                        continue;

                    systemManager.DeleteSystemUser(item);
                }
            }
        }


        [TestMethod, Description("CRUD operations for SystemAccounts (SystemUser + Credentials)")]
        public void SystemUsersTest03()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //διαβάζουμε τα credentials για τον SystemAdmin:
                var credentials = systemManager.GetCredentialForPrincipal(BuiltinSystemUsers.SystemAdmin);
                Assert.IsNotNull(credentials);
                Assert.IsTrue(credentials.Principal == BuiltinSystemUsers.SystemAdmin.UserId);
                Assert.IsTrue(credentials.PrincipalType == PrincipalType.SystemUser);
                Assert.AreEqual<string>("sysadmin", credentials.LogOnToken);
                Assert.AreEqual<string>("g1DnbizYE/M5jhfjsQRi2w==", credentials.PswdSalt);
                Assert.AreEqual<string>("656M/ClNcGRUjxtEUQlpw46LXww=", credentials.PswdToken);
                Assert.IsTrue(credentials.PswdFormat == VLPasswordFormat.Hashed);
                Assert.IsNull(credentials.PswdQuestion);
                Assert.IsNull(credentials.PswdAnswer);
                Assert.IsTrue(credentials.IsApproved);
                Assert.IsFalse(credentials.IsLockedOut);
                Assert.IsNotNull(credentials.LastLoginDate);
                Assert.IsNotNull(credentials.LastPasswordChangedDate);
                Assert.IsNull(credentials.LastLockoutDate);
                Assert.IsTrue(credentials.FailedPasswordAttemptCount == 0);
                Assert.IsNull(credentials.FailedPasswordAttemptWindowStart);
                Assert.IsTrue(credentials.FailedPasswordAnswerAttemptCount == 0);
                Assert.IsNull(credentials.FailedPasswordAnswerAttemptWindowStart);
                Assert.IsNull(credentials.Comment);


                //Φτιάχνουμε ένα νέο System Account:
                var account1 = systemManager.CreateSystemAccount("George", "Milonakis", BuiltinRoles.Administrator.RoleId, "punkis@optonline.net", "gmil", "tolk!3n");
                Assert.IsNotNull(account1);
                Assert.IsTrue(account1.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<string>("George", account1.FirstName);
                Assert.AreEqual<string>("Milonakis", account1.LastName);
                Assert.AreEqual<string>("punkis@optonline.net", account1.Email);
                Assert.IsTrue(account1.IsActive);
                Assert.IsFalse(account1.IsBuiltIn);
                Assert.IsTrue(account1.AttributeFlags == 0);
                Assert.IsTrue(account1.Role == BuiltinRoles.Administrator.RoleId);
                Assert.IsNull(account1.Notes);

                var credentials1 = systemManager.GetCredentialForPrincipal(account1);
                Assert.IsNotNull(credentials1);
                Assert.IsTrue(credentials1.Principal == account1.UserId);
                Assert.IsTrue(credentials1.PrincipalType == PrincipalType.SystemUser);
                Assert.AreEqual<string>("gmil", credentials1.LogOnToken);
                Assert.IsNotNull(credentials1.PswdSalt);
                Assert.IsNotNull(credentials1.PswdToken);
                Assert.IsTrue(credentials1.PswdFormat == VLPasswordFormat.Hashed);
                Assert.IsNull(credentials1.PswdQuestion);
                Assert.IsNull(credentials1.PswdAnswer);
                Assert.IsTrue(credentials1.IsApproved);
                Assert.IsFalse(credentials1.IsLockedOut);
                Assert.IsNull(credentials1.LastLoginDate);
                Assert.IsNull(credentials1.LastPasswordChangedDate);
                Assert.IsNull(credentials1.LastLockoutDate);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 0);
                Assert.IsNull(credentials1.FailedPasswordAttemptWindowStart);
                Assert.IsTrue(credentials1.FailedPasswordAnswerAttemptCount == 0);
                Assert.IsNull(credentials1.FailedPasswordAnswerAttemptWindowStart);
                Assert.IsNull(credentials1.Comment);

                var svdCredentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.AreEqual<VLCredential>(credentials1, svdCredentials1);

                /*Ελέγχουμε το password!*/
                string encodedPasswd = Utility.EncodePassword("tolk!3n", credentials1.PswdFormat, credentials1.PswdSalt);
                Assert.AreEqual<string>(credentials1.PswdToken, encodedPasswd);




            }
            finally
            {
                var systemUsers = systemManager.GetSystemUsers();
                foreach (var item in systemUsers)
                {
                    if (item.IsBuiltIn)
                        continue;

                    systemManager.DeleteSystemUser(item);
                }
            }
        }


        [TestMethod, Description("SystemAccounts: Passwords #1!")]
        public void SystemUsersTest04()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //Φτιάχνουμε ένα νέο System Account:
                var account1 = systemManager.CreateSystemAccount("George", "Milonakis", BuiltinRoles.Administrator.RoleId, "punkis@optonline.net", "gmil", "tolk!3n");
                Assert.IsNotNull(account1);
                var credentials1 = systemManager.GetCredentialForPrincipal(account1);
                Assert.IsNotNull(credentials1);
                Assert.IsNull(credentials1.LastPasswordChangedDate);
                Assert.IsNull(credentials1.LastLoginDate);
                Assert.IsNull(credentials1.LastLockoutDate);

                //Φτιάχνουμε ένα δεύτερο System Account:
                var account2 = systemManager.CreateSystemAccount("Παναγιώτης", "Εχετλαίος", BuiltinRoles.Developer.RoleId, "ehetlaios@thetest12.gr", "xeti", "exetleos");
                Assert.IsNotNull(account2);
                var credentials2 = systemManager.GetCredentialForPrincipal(account2);
                Assert.IsNotNull(credentials2);
                Assert.IsNull(credentials2.LastPasswordChangedDate);
                Assert.IsNull(credentials2.LastLoginDate);
                Assert.IsNull(credentials2.LastLockoutDate);


                /*Ελέγχουμε το password!*/
                string encodedPasswd1 = Utility.EncodePassword("tolk!3n", credentials1.PswdFormat, credentials1.PswdSalt);
                Assert.AreEqual<string>(credentials1.PswdToken, encodedPasswd1);
                var accessToken = valisSystem.LogOnUser(credentials1.LogOnToken, "tolk!3n");
                Assert.IsNotNull(accessToken);
                Assert.IsTrue(accessToken.Principal == credentials1.Principal);

                /*Ελέγχουμε το password!*/
                string encodedPasswd2 = Utility.EncodePassword("exetleos", credentials2.PswdFormat, credentials2.PswdSalt);
                Assert.AreEqual<string>(credentials2.PswdToken, encodedPasswd2);
                accessToken = valisSystem.LogOnUser(credentials2.LogOnToken, "exetleos");
                Assert.IsNotNull(accessToken);
                Assert.IsTrue(accessToken.Principal == credentials2.Principal);


                /*Αλλάζουμε το password:*/
                Assert.IsFalse(systemManager.ChangePassword(credentials1.LogOnToken, "zdvcasdfa", "password!@#"));

                Assert.IsTrue(systemManager.ChangePassword(credentials1.LogOnToken, "tolk!3n", "password!@#"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsNotNull(credentials1);
                Assert.IsNotNull(credentials1.LastPasswordChangedDate);
                Assert.IsNotNull(credentials1.LastLoginDate);
                Assert.IsNull(credentials1.LastLockoutDate);
                /*Ελέγχουμε το password!*/
                encodedPasswd1 = Utility.EncodePassword("password!@#", credentials1.PswdFormat, credentials1.PswdSalt);
                Assert.AreEqual<string>(credentials1.PswdToken, encodedPasswd1);
                accessToken = valisSystem.LogOnUser(credentials1.LogOnToken, "password!@#");
                Assert.IsNotNull(accessToken);
                Assert.IsTrue(accessToken.Principal == credentials1.Principal);


                /*Αλλάζουμε το password:*/
                Assert.IsTrue(systemManager.ChangePassword(credentials2.LogOnToken, "exetleos", "qweqw@#$@QWERD!#$asdfq3$@#%@$R@!#$#!!#$Q#WER@#RQEadf"));
                credentials2 = systemManager.GetCredentialById(credentials2.CredentialId);
                Assert.IsNotNull(credentials2);
                Assert.IsNotNull(credentials2.LastPasswordChangedDate);
                Assert.IsNotNull(credentials2.LastLoginDate);
                Assert.IsNull(credentials2.LastLockoutDate);
                /*Ελέγχουμε το password!*/
                encodedPasswd2 = Utility.EncodePassword("qweqw@#$@QWERD!#$asdfq3$@#%@$R@!#$#!!#$Q#WER@#RQEadf", credentials2.PswdFormat, credentials2.PswdSalt);
                Assert.AreEqual<string>(credentials2.PswdToken, encodedPasswd2);
                accessToken = valisSystem.LogOnUser(credentials2.LogOnToken, "qweqw@#$@QWERD!#$asdfq3$@#%@$R@!#$#!!#$Q#WER@#RQEadf");
                Assert.IsNotNull(accessToken);
                Assert.IsTrue(accessToken.Principal == credentials2.Principal);


                /*Αλλάζουμε το password:*/
                Assert.IsTrue(systemManager.SetNewPassword(credentials1.PrincipalType, credentials1.Principal, "goodnight!@#!!@#!@asefqwe123rqe"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsNotNull(credentials1);
                Assert.IsNotNull(credentials1.LastPasswordChangedDate);
                Assert.IsNotNull(credentials1.LastLoginDate);
                Assert.IsNull(credentials1.LastLockoutDate);
                /*Ελέγχουμε το password!*/
                encodedPasswd1 = Utility.EncodePassword("goodnight!@#!!@#!@asefqwe123rqe", credentials1.PswdFormat, credentials1.PswdSalt);
                Assert.AreEqual<string>(credentials1.PswdToken, encodedPasswd1);
                accessToken = valisSystem.LogOnUser(credentials1.LogOnToken, "goodnight!@#!!@#!@asefqwe123rqe");
                Assert.IsNotNull(accessToken);
                Assert.IsTrue(accessToken.Principal == credentials1.Principal);

            }
            finally
            {
                var systemUsers = systemManager.GetSystemUsers();
                foreach (var item in systemUsers)
                {
                    if (item.IsBuiltIn)
                        continue;

                    systemManager.DeleteSystemUser(item);
                }
            }
        }


        [TestMethod, Description("SystemAccounts: Passwords #2!")]
        public void SystemUsersTest05()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //Φτιάχνουμε ένα νέο System Account:
                var account1 = systemManager.CreateSystemAccount("George", "Milonakis", BuiltinRoles.Administrator.RoleId, "punkis@optonline.net", "gmil", "tolk!3n");
                Assert.IsNotNull(account1);
                Assert.IsNull(account1.LastActivityDate);
                var credentials1 = systemManager.GetCredentialForPrincipal(account1);
                Assert.IsNotNull(credentials1);
                Assert.IsNull(credentials1.LastPasswordChangedDate);
                Assert.IsNull(credentials1.LastLoginDate);
                Assert.IsNull(credentials1.LastLockoutDate);

                //Κάνουμε login:
                var accessToken = valisSystem.LogOnUser("gmil", "tolk!3n");
                Assert.IsNotNull(accessToken);
                var svdAccessToken = valisSystem.ValidateAccessToken(accessToken.AccessTokenId);
                Assert.AreEqual<VLAccessToken>(accessToken, svdAccessToken);

                valisSystem.LogOffUser(accessToken);
                Assert.IsNull(valisSystem.ValidateAccessToken(accessToken.AccessTokenId));


                /*Τώρα θα κάνουμε αποτυχημένες προσπάθειες για login μέχρι να κλειδώσει το account:*/
                Assert.IsTrue(Utility.MaxInvalidPasswordAttempts == 5);

                //1η προσπαθεια
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 1);
                Assert.IsFalse(credentials1.IsLockedOut);
                Assert.IsNull(credentials1.LastLockoutDate);

                //2η προσπαθεια
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 2);
                Assert.IsFalse(credentials1.IsLockedOut);
                //3η προσπαθεια
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 3);
                Assert.IsFalse(credentials1.IsLockedOut);
                //4η προσπαθεια
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 4);
                Assert.IsFalse(credentials1.IsLockedOut);
                //5η προσπαθεια και κλείδωσε
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 5);
                Assert.IsTrue(credentials1.IsLockedOut);
                Assert.IsNotNull(credentials1.LastLockoutDate);

                /*Τώρα που κλείδωσε το account δεν μπορεί να χρησιμοποιηθέι:*/
                Assert.IsNull(valisSystem.LogOnUser("gmil", "tolk!3n"));

                //Τώρα θα ξεκλειδώσουμε το account
                Assert.IsTrue(systemManager.UnlockPrincipal("gmil"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsFalse(credentials1.IsLockedOut);
                Assert.IsNull(credentials1.LastLockoutDate);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 0);
                Assert.IsNull(credentials1.FailedPasswordAttemptWindowStart);
                Assert.IsTrue(credentials1.FailedPasswordAnswerAttemptCount == 0);
                Assert.IsNull(credentials1.FailedPasswordAnswerAttemptWindowStart);





                //Κάνουμε login:
                accessToken = valisSystem.LogOnUser("gmil", "tolk!3n");
                Assert.IsNotNull(accessToken);
                svdAccessToken = valisSystem.ValidateAccessToken(accessToken.AccessTokenId);
                Assert.AreEqual<VLAccessToken>(accessToken, svdAccessToken);

                valisSystem.LogOffUser(accessToken);
                Assert.IsNull(valisSystem.ValidateAccessToken(accessToken.AccessTokenId));



                /*Τώρα θα κάνουμε αποτυχημένες προσπάθειες για login μέχρι να κλειδώσει το account:*/
                //1η προσπαθεια
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 1);
                Assert.IsFalse(credentials1.IsLockedOut);
                Assert.IsNull(credentials1.LastLockoutDate);

                //2η προσπαθεια
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 2);
                Assert.IsFalse(credentials1.IsLockedOut);
                //3η προσπαθεια
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 3);
                Assert.IsFalse(credentials1.IsLockedOut);
                //4η προσπαθεια
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 4);
                Assert.IsFalse(credentials1.IsLockedOut);
                //5η προσπαθεια και κλείδωσε
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 5);
                Assert.IsTrue(credentials1.IsLockedOut);
                Assert.IsNotNull(credentials1.LastLockoutDate);

                /*Τώρα που κλείδωσε το account δεν μπορεί να χρησιμοποιηθέι:*/
                Assert.IsNull(valisSystem.LogOnUser("gmil", "tolk!3n"));

                //Τώρα θα ξεκλειδώσουμε το account
                Assert.IsTrue(systemManager.UnlockPrincipal(credentials1.PrincipalType, credentials1.Principal));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsFalse(credentials1.IsLockedOut);
                Assert.IsNull(credentials1.LastLockoutDate);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 0);
                Assert.IsNull(credentials1.FailedPasswordAttemptWindowStart);
                Assert.IsTrue(credentials1.FailedPasswordAnswerAttemptCount == 0);
                Assert.IsNull(credentials1.FailedPasswordAnswerAttemptWindowStart);




                //Κάνουμε login:
                accessToken = valisSystem.LogOnUser("gmil", "tolk!3n");
                Assert.IsNotNull(accessToken);
                svdAccessToken = valisSystem.ValidateAccessToken(accessToken.AccessTokenId);
                Assert.AreEqual<VLAccessToken>(accessToken, svdAccessToken);

                valisSystem.LogOffUser(accessToken);
                Assert.IsNull(valisSystem.ValidateAccessToken(accessToken.AccessTokenId));


            }
            finally
            {
                var systemUsers = systemManager.GetSystemUsers();
                foreach (var item in systemUsers)
                {
                    if (item.IsBuiltIn)
                        continue;

                    systemManager.DeleteSystemUser(item);
                }
            }
        }


    }
}
