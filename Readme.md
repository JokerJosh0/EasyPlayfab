# Introduction
When you import the package/files you need to navigate to EasyPlayfab > Settings and make sure you set your 'app id' and 'app secret' from your oculus dashboard then press 'Validate Cloudscript' (assuming youve logged into your title), once complete press 'Rule Tutorial' and follow the steps provided.

the package has built-in api spam prevention. Modding detection (untested). oculus user validation. secure api features (currency modification, IAP, Item granting). and has most of the playfab api features, just made easier. and give you access to all of the oculus features (requires the data use checkup features to be accepted) 

## LoginHandle Variables
### `LoginHandle.playfab_playerId`
Returns the users PlayFab Id (string)

### `LoginHandle.oculus_username`
Returns the users Oculus username (string)

### `LoginHandle.oculus_displayname`
Returns the users Oculus displayname (string)

### `LoginHandle.oculus_userId`
Returns the users Oculus userId (string)

### `LoginHandle.oculus_profileLink`
Returns the users Oculus profile image link (string)

### `LoginHandle.isLoggedIn`
Returns if the user is logged into playfab (bool)

### `LoginHandle.Spamming()`
Use this when making a raw playfab api call (raw meaning something thats not in the EasyUsages because it does this automatically.)

### `LoginHandle.HasMods()`
Use this to check if the user has mods (untested). This is already called before logging in.

## EasyPlayfab.Friends

### `public static List<FriendInfo> friends`
This property retrieves the current friend list for the local user.

### `public static void AddFriend(string targetPlayfabId)`

Adds the PlayFab user, based on the `targetPlayfabId`, to the friend list of the local user.

Example:

```csharp
EasyUsages.EasyPlayfab.Friends.AddFriend("TargetPlayerID");
```

### `public static void RemoveFriend(string targetPlayfabId)`

Removes a specified user from the friend list of the local user.

Example:

```csharp
EasyUsages.EasyPlayfab.Friends.RemoveFriend("TargetPlayerID");
```

### `public static void SetFriendTag(string targetPlayfabId, params string[] tags)`

Updates the tag list for a specified user in the friend list of the local user.

Example:

```csharp
EasyUsages.EasyPlayfab.Friends.SetFriendTag("TargetPlayerID", "tag1", "tag2");
```

## EasyPlayfab.PlayerItemManagement

### `public static List<ItemInstance> ownedItems`

This property retrieves the user's owned items.

### `public static void AddUserVirtualCurrency(int amount, string currencyCode, string playfabId = "")`

Adds the user's virtual currency.

If `PlayerID` is null then it will effect the local player.

Example:

```csharp
EasyUsages.EasyPlayfab.PlayerItemManagement.AddUserVirtualCurrency(100, "BN", "PlayerID");
```

### `public static void SubtractUserVirtualCurrency(int amount, string currencyCode, string playfabId = "")`

Subtracts the user's virtual currency.

If `PlayerID` is null then it will effect the local player.

Example:

```csharp
EasyUsages.EasyPlayfab.PlayerItemManagement.SubtractUserVirtualCurrency(50, "BN", "PlayerID");
```

### `public static void GrantItemToPlayer(string itemId, string catalogVer, string playfabId = "")`

Grants a PlayFab item to the player.

If `PlayerID` is null then it will effect the local player.

Example:

```csharp
EasyUsages.EasyPlayfab.PlayerItemManagement.GrantItemToPlayer("ItemID123", "CatalogVersion", "PlayerID");
```

## EasyPlayfab.PlayerDataManagement

### `public static string playfab_username`

This property gets or sets the user's PlayFab username.

### `public static List<ItemInstance> player_ownedItems`

This property retrieves the user's owned PlayFab items.

### `public static Dictionary<string, UserDataRecord> UserData`

This property retrieves the user's data.

### `public static void UpdateUserData(Dictionary<string, string> data, List<string> keysToRemove)`

Updates the user's data.

Example:

```csharp
Dictionary<string, string> data = new Dictionary<string, string>
{
    { "Key1", "Value1" },
    { "Key2", "Value2" }
};
List<string> keysToRemove = new List<string> { "KeyToRemove" };
EasyUsages.EasyPlayfab.PlayerDataManagement.UpdateUserData(data, keysToRemove);
```

## EasyPlayfab.TitleDataManagement

### `public static List<CatalogItem> catalogItems`

This property retrieves the title's catalog items.

### `public static Dictionary<string, string> TitleData`

This property retrieves the title's data.

### `public static List<TitleNewsItem> titleNews`

This property retrieves the title's news items.

## EasyOculus.IAP

### `public static System.Action OnPurchase`

This action is triggered when a purchase is completed. Use this to grant currency etc

```csharp
void Start(){
    //subscribe to event
    EasyUsages.OculusUsages.IAP.OnPurchase += OnPurchase;
}

void OnPurchase(){
    //triggered when purchase was completed
}
```

### `public static void PurchaseSKU(string sku)`

Purchase an Oculus SKU securely.

Example:

```csharp
EasyUsages.EasyOculus.IAP.PurchaseSKU("YourSKU");
```

## EasyAchievements

### `public static List<string> UnlockedAchievements`
Retrieves the ID for each Achievement unlocked by the user.

### `public static void Achieve(string Identifier)`
Unlocks an Achievement depending on the Identifer

Example:

```csharp
EasyAchievements.Achieve("Identifier");
```

### `public static void AddCount(string Identifier, int count)`
Adds a count to the achievement

Example:

```csharp
EasyAchievements.AddCount("Identifier", 1);
```

### `public static void AddFields(string Identifier, string fields)`
Adds a field to the achievement

Example:

```csharp
EasyAchievements.AddFields("Identifier", "field");
```

# Info
More will come in the future, join the [Discord](https://discord.gg/bvvCc9cjVP) to get updates or get future packages
## Credits
[JokerJosh](https://discord.com/users/791550177780563998) - Creating the package


[Maximal](https://discord.com/users/1108509518779920405) - Testing the package and writing documentation
