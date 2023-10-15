namespace EasyPlayfab.Login
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using PlayFab;
    using PlayFab.ClientModels;
    using Oculus.Platform;
    using System.Linq;
    using System.Reflection;
    using System;
#if PHOTON_UNITY_NETWORKING
    using Photon.Realtime;
    using Photon.Pun;
#endif
    public static class LoginHandle
    {
        /// <summary>
        /// Returns the users PlayFab Id
        /// </summary>
        public static string playfab_playerId { get; internal set; } = string.Empty;
        /// <summary>
        /// Returns the users Oculus username
        /// </summary>
        public static string oculus_username { get; internal set; } = string.Empty;
        /// <summary>
        /// Returns the users Oculus displayname
        /// </summary>
        public static string oculus_displayname { get; internal set; } = string.Empty;
        /// <summary>
        /// Returns the users Oculus userId
        /// </summary>
        public static string oculus_userId { get; internal set; } = string.Empty;
        /// <summary>
        /// Returns the users Oculus profile image link
        /// </summary>
        public static string oculus_profileLink { get; internal set; } = string.Empty;
        /// <summary>
        /// Returns if the user is logged into playfab
        /// </summary>
        public static bool isLoggedIn { get { return PlayFabClientAPI.IsClientLoggedIn(); } }


        //make sure we dont spam the title
        private static float requestCount = 0f;
        private static float timeSinceLastRequest = 0f;

        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Login()
        {
            if (HasMods()) return;

            Core.AsyncInitialize().OnComplete(c => 
            {
                Entitlements.IsUserEntitledToApplication().OnComplete(callback =>
                {
                    if (callback.IsError) { Debug.LogError("Meta Platform entitlement error: " + callback.GetError()); return; }
                    Users.GetLoggedInUser().OnComplete(m =>
                    {
                        if (!m.IsError && m.Type == Message.MessageType.User_GetLoggedInUser)
                        {
                            Oculus.Platform.Models.User user = m.GetUser();
                            oculus_userId = user.ID.ToString();
                            oculus_profileLink = user.ImageURL;
                            oculus_username = user.OculusID;
                            oculus_displayname = user.DisplayName;
                            Users.GetUserProof().OnComplete(r =>
                            {
                                if (r.IsError) return;
                                Dictionary<string, string> customTags = new Dictionary<string, string>();
                                customTags.Add("userId", user.ID.ToString());
                                customTags.Add("userProof", m.GetUserProof().Value);
                                PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
                                {
                                    CreateAccount = true,
                                    CustomId = "META" + PlayFabSettings.DeviceUniqueIdentifier,
                                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams { GetUserAccountInfo = true, GetUserInventory = true },
                                    CustomTags = customTags
                                }, RetrieveData, error => { Debug.Log("Error with logging in: " + error.ErrorMessage); });
                            });
                        }

                    });
                });
            });
        }


        private static void RetrieveData(LoginResult result)
        {
            playfab_playerId = result.PlayFabId;
            Debug.Log("Logged in!");
#if PHOTON_UNITY_NETWORKING
            PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
            {
                PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime
            }, AuthenticateWithPhoton, e => {});
#endif
        }
#if PHOTON_UNITY_NETWORKING
        private static void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj)
        {
            Debug.Log("Photon token acquired: " + obj.PhotonCustomAuthenticationToken + "  Authentication complete.");
            var customAuth = new Photon.Realtime.AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
            customAuth.AddAuthParameter("username", playfab_playerId);
            customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);
            PhotonNetwork.AuthValues = customAuth;
        }
#endif

        public static bool HasMods()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            string[] bannedDlls = new string[] 
            { 
                "lemon", 
                "harmony", 
                "melonloader"
            };

            foreach (Assembly x in assemblies)
            {
                if (bannedDlls.Contains(x.FullName.ToLower()))
                {
                    UnityEngine.Application.Quit(1);
                    return true;
                }
            }
            return false;
        }

        public static bool Spamming()
        {
            requestCount += 1;
            if (requestCount > 2 && Time.time - timeSinceLastRequest < 2) return true;
            requestCount = 0;
            timeSinceLastRequest = Time.time;
            return false;
        }
    }

}