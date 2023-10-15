namespace EasyPlayfab
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using PlayFab;
    using PlayFab.ClientModels;
    using EasyPlayfab.Login;
    using Oculus.Platform;
    using Oculus.Platform.Models;
    using System.Linq;

    public static class EasyUsages
    {
        public static class EasyPlayfab
        {
            public static class Friends
            {
                /// <summary>
                /// Retrieves the current friend list for the local user.
                /// </summary>
                public static List<FriendInfo> friends { get { return GetFriends(); } }

                /// <summary>
                /// Adds the PlayFab user, based upon the targetPlayfabId, to the friend list of the local user.
                /// </summary>
                public static void AddFriend(string targetPlayfabId)
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn && !string.IsNullOrEmpty(targetPlayfabId))
                    {
                        PlayFabClientAPI.AddFriend(new AddFriendRequest { FriendPlayFabId = targetPlayfabId }, r => { }, ErrorHandle);
                    }
                }

                /// <summary>
                /// Removes a specified user from the friend list of the local user
                /// </summary>
                public static void RemoveFriend(string targetPlayfabId)
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn && !string.IsNullOrEmpty(targetPlayfabId))
                    {
                        PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest { FriendPlayFabId = targetPlayfabId }, r => { }, ErrorHandle);
                    }
                }

                /// <summary>
                /// Updates the tag list for a specified user in the friend list of the local user
                /// </summary>
                public static void SetFriendTag(string targetPlayfabId, params string[] tags)
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn && !string.IsNullOrEmpty(targetPlayfabId) && tags != null)
                    {
                        List<string> newTags = new List<string>();
                        foreach (string s in tags)
                        {
                            newTags.Add(s);
                        }
                        PlayFabClientAPI.SetFriendTags(new SetFriendTagsRequest { FriendPlayFabId = targetPlayfabId, Tags = newTags }, r => { }, ErrorHandle);
                    }
                }

                private static List<FriendInfo> GetFriends()
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn)
                    {
                        List<FriendInfo> friends = new List<FriendInfo>();
                        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest { }, r => { friends = r.Friends; }, ErrorHandle);
                        return friends;
                    }
                    return friends;
                }
            }

            public static class PlayerItemManagement
            {
                /// <summary>
                /// Retrieves the users owned items
                /// </summary>
                public static List<ItemInstance> ownedItems { get { return GetUserInventory(); } }

                /* 
                 * ref:
                 *  handlers.GivePlayerCurrency = function GivePlayerCurrency(args)
                 *  {
                 *      server.AddUserVirtualCurrency({PlayFabID: args.PlayerToGiveID, VirtualCurrency: args.CCode, Amount: args.AmountToGive});
                 *      return "Gave Currency";
                 *  }
                 */
                /// <summary>
                /// Adds the users virtual currency
                /// </summary>
                public static void AddUserVirtualCurrency(int amount, string currencyCode, string playfabId = "")
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn && amount != 0 && !string.IsNullOrEmpty(currencyCode) && currencyCode.Length < 3)
                    {
                        string targetId = "";
                        if (playfabId == string.Empty)
                            targetId = LoginHandle.playfab_playerId;
                        else
                            targetId = playfabId;
                        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest { FunctionName = "GivePlayerCurrency", FunctionParameter = new { PlayerToGiveID = targetId, CCode = currencyCode, AmountToGive = amount } }, r => { }, ErrorHandle);
                    }
                }

                /* 
                 * ref:
                 *  handlers.RemovePlayerCurrency = function RemovePlayerCurrency(args)
                 *  {
                 *      var SubtractUserVirtualCurrencyRequest = {
                 *          "PlayFabId" : args.PlayerToRemoveID,
                 *          "VirtualCurrency": args.CCode,
                 *          "Amount": args.AmountToRemove
                 *      };
                 *      server.SubtractUserVirtualCurrency(SubtractUserVirtualCurrencyRequest);
                 *      return "Removed Currency";
                 *  }
                 */
                /// <summary>
                /// Subtracts the users virtual currency
                /// </summary>
                public static void SubtractUserVirtualCurrency(int amount, string currencyCode, string playfabId = "")
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn && amount != 0 && !string.IsNullOrEmpty(currencyCode) && currencyCode.Length < 3)
                    {
                        string targetId = "";
                        if (playfabId == string.Empty)
                            targetId = LoginHandle.playfab_playerId;
                        else
                            targetId = playfabId;
                        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest { FunctionName = "RemovePlayerCurrency", FunctionParameter = new { PlayerToRemoveID = targetId, CCode = currencyCode, AmountToRemove = amount } }, r => { }, ErrorHandle);
                    }
                }

                /* 
                 * ref:
                 *  handlers.GrantItemToPlayer = function GrantItemToPlayer(args)
                 *  {
                 *      var GrantItemsToUserRequest = {
                 *          "PlayFabId" : args.GrantPlayerID,
                 *          "CatalogVersion": args.Ver,
                 *          "ItemIds": args.GrantItemID,
                 *          "Annotation": "purchase"
                 *      };
                 *      server.GrantItemsToUserRequest(GrantItemsToUserRequest);
                 *      return "Granted";
                 *  }
                 */
                /// <summary>
                /// Grants a PlayFab item
                /// </summary>
                public static void GrantItemToPlayer(string itemId, string catalogVer, string playfabId = "")
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn && !string.IsNullOrEmpty(itemId) && !string.IsNullOrEmpty(catalogVer))
                    {
                        string targetId = "";
                        if (playfabId == string.Empty)
                            targetId = LoginHandle.playfab_playerId;
                        else
                            targetId = playfabId;

                        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest { FunctionName = "GrantItemToPlayer", FunctionParameter = new { GrantPlayerID = targetId, Ver = catalogVer, GrantItemID = itemId } }, r => { }, ErrorHandle);
                    }
                }
                private static List<ItemInstance> GetUserInventory()
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn)
                    {
                        List<ItemInstance> newItems = new List<ItemInstance>();
                        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest { }, r => { newItems = r.Inventory; }, ErrorHandle);
                        return newItems;
                    }
                    return new List<ItemInstance>();
                }
            }

            public static class PlayerDataManagement
            {
                /// <summary>
                /// Returns the users PlayFab username, and Sets the Users username.
                /// </summary>
                public static string playfab_username { get { return GetUsername(); } set { SetUsername(value); } }

                /// <summary>
                /// Returns the users *owned* PlayFab items
                /// </summary>
                public static List<ItemInstance> player_ownedItems { get { return GetOwnedItems(); } }

                /// <summary>
                /// Retrieves the users data
                /// </summary>
                public static Dictionary<string, UserDataRecord> UserData { get { return GetUserData(); } }

                private static Dictionary<string, UserDataRecord> GetUserData()
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn)
                    {
                        Dictionary<string, UserDataRecord> record = new Dictionary<string, UserDataRecord>();
                        PlayFabClientAPI.GetUserData(new GetUserDataRequest { }, r => { record = r.Data; }, ErrorHandle);
                        return record;
                    }
                    return null;
                }
                /// <summary>
                /// Updates the users data
                /// </summary>
                public static void UpdateUserData(Dictionary<string, string> data, List<string> keysToRemove)
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn && data != null && keysToRemove != null)
                    {
                        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest { Data = data, KeysToRemove = keysToRemove, Permission = UserDataPermission.Public }, r => { }, ErrorHandle);
                    }
                }

                private static void SetUsername(string value)
                {
                    if (!LoginHandle.Spamming() && !string.IsNullOrEmpty(value) && LoginHandle.isLoggedIn)
                    {
                        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = value }, r => { Debug.Log("Set username to: " + value); }, e => { Debug.LogError("An error occured when setting username: " + e.ErrorMessage); ; });
                    }
                }
                private static string GetUsername()
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn)
                    {
                        string newUser = string.Empty;
                        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = LoginHandle.playfab_playerId }, r => { if (!string.IsNullOrEmpty(r.AccountInfo.Username)) { newUser = r.AccountInfo.Username; } else { /*fallback username*/ newUser = LoginHandle.oculus_username; } }, e => { Debug.LogError("An error occured when getting username: " + e.ErrorMessage); });
                        return newUser;
                    }
                    //prevent returing a null string
                    return LoginHandle.oculus_username;
                }

                private static List<ItemInstance> GetOwnedItems()
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn)
                    {
                        List<ItemInstance> itemInstances = new List<ItemInstance>();
                        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest { }, r => { itemInstances = r.Inventory; }, e => { Debug.LogError("An error occured when getting items: " + e.ErrorMessage); });
                        return itemInstances;
                    }
                    return null;
                }
            }

            public static class TitleDataManagement
            {
                /// <summary>
                /// Retrieves the titles catalog items
                /// </summary>
                public static List<CatalogItem> catalogItems { get { return GetCatalogItems(); } }

                /// <summary>
                /// Retrieves the title data
                /// </summary>
                public static Dictionary<string, string> TitleData { get { return GetTitleData(); } }

                /// <summary>
                /// Retrieves the title news items
                /// </summary>
                public static List<TitleNewsItem> titleNews { get { return GetTitleNews(); } }

                private static List<TitleNewsItem> GetTitleNews()
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn)
                    {
                        List<TitleNewsItem> data = new List<TitleNewsItem>();
                        PlayFabClientAPI.GetTitleNews(new GetTitleNewsRequest { }, r => { data = r.News; }, ErrorHandle);
                        return data;
                    }
                    return null;
                }

                private static Dictionary<string, string> GetTitleData()
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn)
                    {
                        Dictionary<string, string> data = new Dictionary<string, string>();
                        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest { }, r => { data = r.Data; }, ErrorHandle);
                        return data;
                    }
                    return null;
                }

                private static List<CatalogItem> GetCatalogItems()
                {
                    if (!LoginHandle.Spamming() && LoginHandle.isLoggedIn)
                    {
                        List<CatalogItem> items = new List<CatalogItem>();
                        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest { }, r => { items = r.Catalog; }, ErrorHandle);
                        return items;
                    }
                    return null;
                }
            }
        }

        public static class EasyOculus
        {
            public static class IAP
            {
                /// <summary>
                /// This triggers when a puchase is completed, in here you can add user currency.
                /// </summary>
                public static System.Action OnPurchase;

                /*
                 * ref:
                 *  handlers.CompleteIAPPurchase = function(args, context)
                 *  {
                 *      var method = "post";
                 *      var contentBody = "";
                 *      var contentType = "application/json";
                 *      var headers = {};
                 *      var url = "https://graph.oculus.com/5409297455797322/consume_entitlement?nonce=" + args.UserProof + "&user_id=" + args.MetaId + "&sku=" + args.SKU + "&access_token=OC|appId|appSecret";
                 *      var responseString = http.request(url,method,contentBody,contentType,headers);
                 *      if(JSON.parse(responseString).success != undefined){
                 *          if(JSON.parse(responseString).success){
                 *              return true;
                 *          }
                 *      }
                 *      return false;
                 *  }
                 */

                /// <summary>
                /// Purchase a oculus SKU securly
                /// </summary>
                public static void PurchaseSKU(string sku)
                {
                    Oculus.Platform.IAP.LaunchCheckoutFlow(sku).OnComplete(m => 
                    {
                        if (!m.IsError)
                        {
                            if (m.GetPurchase().Sku == sku)
                            {
                                Users.GetLoggedInUser().OnComplete(m =>
                                {
                                    if (!m.IsError && m.Type == Message.MessageType.User_GetLoggedInUser)
                                    {
                                        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
                                        {
                                            FunctionName = "CompleteIAPPurchase",
                                            FunctionParameter = new { SKU = sku, UserProof = m.GetUserProof().Value, LoginHandle.oculus_userId },
                                        }, r =>
                                        {
                                            if (r.FunctionResult != null && (bool)r.FunctionResult)
                                                OnPurchase?.Invoke();

                                        }, e => { });
                                    }
                                });
                            }
                        }
                    });
                }
            }
        }

        private static void ErrorHandle(PlayFabError error)
        {
            Debug.LogError("Error with recent request: " + error.ErrorMessage);
        }
    }
}