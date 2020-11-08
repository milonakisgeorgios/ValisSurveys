using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Valis.Core.UnitTests.Suite1
{
    [TestClass]
    public class ClientTests02 : AdminBaseClass
    {
        
        [TestMethod, Description("CRUD operations for VLClientUser")]
        public void ClientTests02_01()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create one more client::
                var client2 = systemManager.CreateClient("FastrishCert S.A.", BuiltinCountries.Greece, "pcert", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client2);

                //We create a clientUser:
                var user1 = systemManager.CreateClientUser(client1.ClientId, "Татьяна", "Петрова", BuiltinRoles.PowerClient.RoleId, "tatyanova@gmail.com");
                Assert.IsNotNull(user1);
                Assert.IsTrue(user1.Client == client1.ClientId);
                Assert.IsTrue(user1.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<string>("Татьяна", user1.FirstName);
                Assert.AreEqual<string>("Петрова", user1.LastName);
                Assert.AreEqual<string>("tatyanova@gmail.com", user1.Email);
                Assert.IsFalse(user1.IsActive);
                Assert.IsFalse(user1.IsBuiltIn);
                Assert.IsTrue(user1.AttributeFlags == 0);
                Assert.IsTrue(user1.Role == BuiltinRoles.PowerClient.RoleId);
                Assert.IsNull(user1.Comment);

                var svdClientUser = systemManager.GetClientUserByEmail(user1.Email);
                Assert.AreEqual<VLClientUser>(user1, svdClientUser);
                svdClientUser = systemManager.GetClientUserById(user1.UserId);
                Assert.AreEqual<VLClientUser>(user1, svdClientUser);

                //
                Assert.IsTrue(systemManager.GetClientUsers(client1).Count == 1);
                Assert.IsTrue(systemManager.GetClientUsers(client2).Count == 0);


                //the system checks for nonsense email addresses:
                user1.Email = string.Empty;
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateClientUser(user1); }, typeof(VLException));
                user1.Email = "aqedfqeqer";
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateClientUser(user1); }, typeof(VLException));
                user1.Email = "qweeeeeee@eeee";
                _EXECUTEAndCATCHType(delegate { systemManager.UpdateClientUser(user1); }, typeof(VLException));

                //I can't use an existing email:
                _EXECUTEAndCATCHType(delegate { systemManager.CreateClientUser(client1.ClientId, "George", "Milonakis", BuiltinRoles.PowerClient.RoleId, "tatyanova@gmail.com"); }, typeof(VLException));


                //We create a second clientUser:
                var user2 = systemManager.CreateClientUser(client1.ClientId, "qwerty", "asdfghhh", BuiltinRoles.Client.RoleId, "milonakis@thetest12.gr");
                Assert.IsNotNull(user2);
                Assert.IsTrue(user2.Client == client1.ClientId);
                Assert.IsTrue(user2.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<string>("qwerty", user2.FirstName);
                Assert.AreEqual<string>("asdfghhh", user2.LastName);
                Assert.AreEqual<string>("milonakis@thetest12.gr", user2.Email);
                Assert.IsFalse(user2.IsActive);
                Assert.IsFalse(user2.IsBuiltIn);
                Assert.IsTrue(user2.AttributeFlags == 0);
                Assert.IsTrue(user2.Role == BuiltinRoles.Client.RoleId);
                Assert.IsNull(user2.Comment);

                svdClientUser = systemManager.GetClientUserByEmail(user2.Email);
                Assert.AreEqual<VLClientUser>(user2, svdClientUser);
                svdClientUser = systemManager.GetClientUserById(user2.UserId);
                Assert.AreEqual<VLClientUser>(user2, svdClientUser);

                //
                Assert.IsTrue(systemManager.GetClientUsers(client1).Count == 2);
                Assert.IsTrue(systemManager.GetClientUsers(client2).Count == 0);


                //UPDATE:
                user2.FirstName = "Sandro";
                user2.LastName = "Henkel";
                user2.Email = "sandrokel@gmail.com";
                user2.Role = BuiltinRoles.DemoClient.RoleId;
                user2.DefaultLanguage = BuiltinLanguages.German.LanguageId;
                user2 = systemManager.UpdateClientUser(user2);
                Assert.IsTrue(user2.Client == client1.ClientId);
                Assert.IsTrue(user2.DefaultLanguage == BuiltinLanguages.German.LanguageId);
                Assert.AreEqual<string>("Sandro", user2.FirstName);
                Assert.AreEqual<string>("Henkel", user2.LastName);
                Assert.AreEqual<string>("sandrokel@gmail.com", user2.Email);
                Assert.IsFalse(user2.IsActive);
                Assert.IsFalse(user2.IsBuiltIn);
                Assert.IsTrue(user2.AttributeFlags == 0);
                Assert.IsTrue(user2.Role == BuiltinRoles.DemoClient.RoleId);
                Assert.IsNull(user2.Comment);

                svdClientUser = systemManager.GetClientUserByEmail(user2.Email);
                Assert.AreEqual<VLClientUser>(user2, svdClientUser);


                //
                Assert.IsTrue(systemManager.GetClientUsers(client1).Count == 2);
                Assert.IsTrue(systemManager.GetClientUsers(client2).Count == 0);

                systemManager.DeleteClientUser(user2);
                Assert.IsNull(systemManager.GetClientUserById(user2.UserId));
                Assert.IsNull(systemManager.GetClientUserByEmail(user2.Email));


                //
                Assert.IsTrue(systemManager.GetClientUsers(client1).Count == 1);
                Assert.IsTrue(systemManager.GetClientUsers(client2).Count == 0);

            }
            finally
            {
                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
            }
        }


        [TestMethod, Description("CRUD operations for ClientAccounts (clientUser + Credentials)")]
        public void ClientTests02_03()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create one more client:
                var client2 = systemManager.CreateClient("FastrishCert S.A.", BuiltinCountries.Greece, "pcert", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client2);


                //Now we make a new Client Account:
                var account1 = systemManager.CreateClientAccount(client1.ClientId, "George", "Milonakis", BuiltinRoles.PowerClient.RoleId, "punkis@optonline.net", "gmil", "tolk!3n");
                Assert.IsNotNull(account1);
                Assert.IsTrue(account1.Client == client1.ClientId);
                Assert.IsTrue(account1.DefaultLanguage == BuiltinLanguages.Greek.LanguageId);
                Assert.AreEqual<string>("George", account1.FirstName);
                Assert.AreEqual<string>("Milonakis", account1.LastName);
                Assert.AreEqual<string>("punkis@optonline.net", account1.Email);
                Assert.IsTrue(account1.IsActive);
                Assert.IsFalse(account1.IsBuiltIn);
                Assert.IsTrue(account1.AttributeFlags == 0);
                Assert.IsTrue(account1.Role == BuiltinRoles.PowerClient.RoleId);
                Assert.IsNull(account1.Comment);

                var credentials1 = systemManager.GetCredentialForPrincipal(account1);
                Assert.IsNotNull(credentials1);
                Assert.IsTrue(credentials1.Principal == account1.UserId);
                Assert.IsTrue(credentials1.PrincipalType == PrincipalType.ClientUser);
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

                /*We check the password!*/
                string encodedPasswd = Utility.EncodePassword("tolk!3n", credentials1.PswdFormat, credentials1.PswdSalt);
                Assert.AreEqual<string>(credentials1.PswdToken, encodedPasswd);


            }
            finally
            {
                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
            }

        }


        [TestMethod, Description("ClientAccounts: Passwords #1!")]
        public void ClientTests02_04()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create one more client:
                var client2 = systemManager.CreateClient("FastrishCert S.A.", BuiltinCountries.Greece, "pcert", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client2);


                //Now we make a new Client Account:
                var account1 = systemManager.CreateClientAccount(client1.ClientId, "George", "Milonakis", BuiltinRoles.PowerClient.RoleId, "punkis@optonline.net", "gmil", "tolk!3n");
                Assert.IsNotNull(account1);
                var credentials1 = systemManager.GetCredentialForPrincipal(account1);
                Assert.IsNotNull(credentials1);
                Assert.IsNull(credentials1.LastPasswordChangedDate);
                Assert.IsNull(credentials1.LastLoginDate);
                Assert.IsNull(credentials1.LastLockoutDate);

                //Now we make a second new Client Account:
                var account2 = systemManager.CreateClientAccount(client1.ClientId, "Παναγιώτης", "Εχετλαίος", BuiltinRoles.PowerClient.RoleId, "ehetlaios@thetest12.gr", "xeti", "exetleos");
                Assert.IsNotNull(account2);
                var credentials2 = systemManager.GetCredentialForPrincipal(account2);
                Assert.IsNotNull(credentials2);
                Assert.IsNull(credentials2.LastPasswordChangedDate);
                Assert.IsNull(credentials2.LastLoginDate);
                Assert.IsNull(credentials2.LastLockoutDate);


                /*We check the password!*/
                string encodedPasswd1 = Utility.EncodePassword("tolk!3n", credentials1.PswdFormat, credentials1.PswdSalt);
                Assert.AreEqual<string>(credentials1.PswdToken, encodedPasswd1);
                var accessToken = valisSystem.LogOnUser(credentials1.LogOnToken, "tolk!3n");
                Assert.IsNotNull(accessToken);
                Assert.IsTrue(accessToken.Principal == credentials1.Principal);

                /*We check the password!*/
                string encodedPasswd2 = Utility.EncodePassword("exetleos", credentials2.PswdFormat, credentials2.PswdSalt);
                Assert.AreEqual<string>(credentials2.PswdToken, encodedPasswd2);
                accessToken = valisSystem.LogOnUser(credentials2.LogOnToken, "exetleos");
                Assert.IsNotNull(accessToken);
                Assert.IsTrue(accessToken.Principal == credentials2.Principal);


                /*We change the password!*/
                Assert.IsFalse(systemManager.ChangePassword(credentials1.LogOnToken, "zdvcasdfa", "password!@#"));

                Assert.IsTrue(systemManager.ChangePassword(credentials1.LogOnToken, "tolk!3n", "password!@#"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsNotNull(credentials1);
                Assert.IsNotNull(credentials1.LastPasswordChangedDate);
                Assert.IsNotNull(credentials1.LastLoginDate);
                Assert.IsNull(credentials1.LastLockoutDate);
                /*We check the password!*/
                encodedPasswd1 = Utility.EncodePassword("password!@#", credentials1.PswdFormat, credentials1.PswdSalt);
                Assert.AreEqual<string>(credentials1.PswdToken, encodedPasswd1);
                accessToken = valisSystem.LogOnUser(credentials1.LogOnToken, "password!@#");
                Assert.IsNotNull(accessToken);
                Assert.IsTrue(accessToken.Principal == credentials1.Principal);


                /*We change the password!*/
                Assert.IsTrue(systemManager.ChangePassword(credentials2.LogOnToken, "exetleos", "qweqw@#$@QWERD!#$asdfq3$@#%@$R@!#$#!!#$Q#WER@#RQEadf"));
                credentials2 = systemManager.GetCredentialById(credentials2.CredentialId);
                Assert.IsNotNull(credentials2);
                Assert.IsNotNull(credentials2.LastPasswordChangedDate);
                Assert.IsNotNull(credentials2.LastLoginDate);
                Assert.IsNull(credentials2.LastLockoutDate);
                /*We check the password!*/
                encodedPasswd2 = Utility.EncodePassword("qweqw@#$@QWERD!#$asdfq3$@#%@$R@!#$#!!#$Q#WER@#RQEadf", credentials2.PswdFormat, credentials2.PswdSalt);
                Assert.AreEqual<string>(credentials2.PswdToken, encodedPasswd2);
                accessToken = valisSystem.LogOnUser(credentials2.LogOnToken, "qweqw@#$@QWERD!#$asdfq3$@#%@$R@!#$#!!#$Q#WER@#RQEadf");
                Assert.IsNotNull(accessToken);
                Assert.IsTrue(accessToken.Principal == credentials2.Principal);


                /*We change the password!*/
                Assert.IsTrue(systemManager.SetNewPassword(credentials1.PrincipalType, credentials1.Principal, "goodnight!@#!!@#!@asefqwe123rqe"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsNotNull(credentials1);
                Assert.IsNotNull(credentials1.LastPasswordChangedDate);
                Assert.IsNotNull(credentials1.LastLoginDate);
                Assert.IsNull(credentials1.LastLockoutDate);
                /*We check the password!*/
                encodedPasswd1 = Utility.EncodePassword("goodnight!@#!!@#!@asefqwe123rqe", credentials1.PswdFormat, credentials1.PswdSalt);
                Assert.AreEqual<string>(credentials1.PswdToken, encodedPasswd1);
                accessToken = valisSystem.LogOnUser(credentials1.LogOnToken, "goodnight!@#!!@#!@asefqwe123rqe");
                Assert.IsNotNull(accessToken);
                Assert.IsTrue(accessToken.Principal == credentials1.Principal);

            }
            finally
            {
                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
            }

        }



        [TestMethod, Description("ClientAccounts: Passwords #2!")]
        public void ClientTests02_05()
        {
            var systemManager = VLSystemManager.GetAnInstance(admin);

            try
            {
                //We create a customer:
                var client1 = systemManager.CreateClient("MySoftavia S.A.", BuiltinCountries.Greece, "man", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client1);
                //We create one more client:
                var client2 = systemManager.CreateClient("FastrishCert S.A.", BuiltinCountries.Greece, "pcert", profile: BuiltinProfiles.UTESTFree.ProfileId);
                Assert.IsNotNull(client2);



                //Now we make a new Client Account:
                var account1 = systemManager.CreateClientAccount(client1.ClientId, "George", "Milonakis", BuiltinRoles.PowerClient.RoleId, "punkis@optonline.net", "gmil", "tolk!3n");
                Assert.IsNotNull(account1);
                Assert.IsNull(account1.LastActivityDate);
                var credentials1 = systemManager.GetCredentialForPrincipal(account1);
                Assert.IsNotNull(credentials1);
                Assert.IsNull(credentials1.LastPasswordChangedDate);
                Assert.IsNull(credentials1.LastLoginDate);
                Assert.IsNull(credentials1.LastLockoutDate);

                //Let our new client account to logon:
                var accessToken = valisSystem.LogOnUser("gmil", "tolk!3n");
                Assert.IsNotNull(accessToken);
                var svdAccessToken = valisSystem.ValidateAccessToken(accessToken.AccessTokenId);
                Assert.AreEqual<VLAccessToken>(accessToken, svdAccessToken);

                valisSystem.LogOffUser(accessToken);
                Assert.IsNull(valisSystem.ValidateAccessToken(accessToken.AccessTokenId));


                //Now we will attempt wrongly five (5) times to logon, in order the system to lcok the account:
                Assert.IsTrue(Utility.MaxInvalidPasswordAttempts == 5);

                //1st attempt
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 1);
                Assert.IsFalse(credentials1.IsLockedOut);
                Assert.IsNull(credentials1.LastLockoutDate);

                //2nd attempt
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 2);
                Assert.IsFalse(credentials1.IsLockedOut);
                //3rd attempt
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 3);
                Assert.IsFalse(credentials1.IsLockedOut);
                //4th attempt
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 4);
                Assert.IsFalse(credentials1.IsLockedOut);
                //5th attempt and the system locks the account
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 5);
                Assert.IsTrue(credentials1.IsLockedOut);
                Assert.IsNotNull(credentials1.LastLockoutDate);

                /*A locked account cannot be used*/
                Assert.IsNull(valisSystem.LogOnUser("gmil", "tolk!3n"));

                //Now we will unlock the acount
                Assert.IsTrue(systemManager.UnlockPrincipal("gmil"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsFalse(credentials1.IsLockedOut);
                Assert.IsNull(credentials1.LastLockoutDate);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 0);
                Assert.IsNull(credentials1.FailedPasswordAttemptWindowStart);
                Assert.IsTrue(credentials1.FailedPasswordAnswerAttemptCount == 0);
                Assert.IsNull(credentials1.FailedPasswordAnswerAttemptWindowStart);





                //We logon
                accessToken = valisSystem.LogOnUser("gmil", "tolk!3n");
                Assert.IsNotNull(accessToken);
                svdAccessToken = valisSystem.ValidateAccessToken(accessToken.AccessTokenId);
                Assert.AreEqual<VLAccessToken>(accessToken, svdAccessToken);

                valisSystem.LogOffUser(accessToken);
                Assert.IsNull(valisSystem.ValidateAccessToken(accessToken.AccessTokenId));



                //Now we will attempt wrongly five (5) times to logon, in order the system to lcok the account:
                //1st attempt
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 1);
                Assert.IsFalse(credentials1.IsLockedOut);
                Assert.IsNull(credentials1.LastLockoutDate);

                //2nd attempt
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 2);
                Assert.IsFalse(credentials1.IsLockedOut);
                //3rd attempt
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 3);
                Assert.IsFalse(credentials1.IsLockedOut);
                //4th attempt
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 4);
                Assert.IsFalse(credentials1.IsLockedOut);
                //5th attempt and the system locks the account
                Assert.IsNull(valisSystem.LogOnUser("gmil", "qwerty"));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 5);
                Assert.IsTrue(credentials1.IsLockedOut);
                Assert.IsNotNull(credentials1.LastLockoutDate);

                /*A locked account cannot be used*/
                Assert.IsNull(valisSystem.LogOnUser("gmil", "tolk!3n"));

                //Now we will unlock the acount
                Assert.IsTrue(systemManager.UnlockPrincipal(credentials1.PrincipalType, credentials1.Principal));
                credentials1 = systemManager.GetCredentialById(credentials1.CredentialId);
                Assert.IsFalse(credentials1.IsLockedOut);
                Assert.IsNull(credentials1.LastLockoutDate);
                Assert.IsTrue(credentials1.FailedPasswordAttemptCount == 0);
                Assert.IsNull(credentials1.FailedPasswordAttemptWindowStart);
                Assert.IsTrue(credentials1.FailedPasswordAnswerAttemptCount == 0);
                Assert.IsNull(credentials1.FailedPasswordAnswerAttemptWindowStart);




                //We logon
                accessToken = valisSystem.LogOnUser("gmil", "tolk!3n");
                Assert.IsNotNull(accessToken);
                svdAccessToken = valisSystem.ValidateAccessToken(accessToken.AccessTokenId);
                Assert.AreEqual<VLAccessToken>(accessToken, svdAccessToken);

                valisSystem.LogOffUser(accessToken);
                Assert.IsNull(valisSystem.ValidateAccessToken(accessToken.AccessTokenId));



            }
            finally
            {
                var clients = systemManager.GetClients();
                foreach (var client in clients)
                {
                    if (client.IsBuiltIn)
                        continue;

                    systemManager.DeleteClient(client);
                }
            }

        }


    }
}
