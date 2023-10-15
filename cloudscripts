handlers.NewLogin = function(args, context) {
    if (context.playStreamEvent.CustomTags['userProof'] != undefined && context.playStreamEvent.CustomTags['userId'] != undefined) {
        var url = 'https://graph.oculus.com/user_nonce_validate?nonce=' + context.playStreamEvent.CustomTags['userProof'] + '&user_id=' + context.playStreamEvent.CustomTags['userId'] + '&access_token=OC|appId|appSecret';
        var method = 'post';
        var contentBody = '';
        var contentType = 'application/json';
        var headers = {};
        var responseString = http.request(url, method, contentBody, contentType, headers);

        if (JSON.parse(responseString).is_valid != undefined) 
        {
            if (!JSON.parse(responseString).is_valid) 
            {
                var banRequest = {
                    'Bans': [{
                        DurationInHours: 72,
                        PlayFabId: currentPlayerId,
                        Reason: 'Invalid Oculus account',
                        IPAddress: context.playStreamEvent.IPV4Address
                        }]
                    }
                    server.BanUsers(banRequest);
                    return { error: 'Login request invalid' };
            }else 
            {
                var banRequest = {
                    'Bans': 
                    [{
                        DurationInHours: 72,
                        PlayFabId: currentPlayerId,
                        Reason: 'Invalid Oculus account',
                        IPAddress: context.playStreamEvent.IPV4Address
                    }]
                }
                server.BanUsers(banRequest);
                return { error: 'Login request invalid' };
            }
        } 
        else 
        {
            var banRequest = {
                'Bans': 
                [{
                    DurationInHours: 72,
                    PlayFabId: currentPlayerId,
                    Reason: 'Invalid Oculus account',
                    IPAddress: context.playStreamEvent.IPV4Address
                }]
            }
            server.BanUsers(banRequest);
            return { error: 'Login request invalid' };
        }
    }
}
handlers.GrantItemToPlayer = function GrantItemToPlayer(args)
{
    var GrantItemsToUserRequest = {
	    'PlayFabId' : args.GrantPlayerID,
	    'CatalogVersion': args.Ver,
	    'ItemIds': args.GrantItemID,
	    'Annotation': 'purchase'
    };
    server.GrantItemsToUserRequest(GrantItemsToUserRequest);
    return 'Granted';
}
handlers.GivePlayerCurrency = function GivePlayerCurrency(args)
{
    server.AddUserVirtualCurrency({PlayFabID: args.PlayerToGiveID, VirtualCurrency: args.CCode, Amount: args.AmountToGive});
    return 'Gave Currency';
}
handlers.RemovePlayerCurrency = function RemovePlayerCurrency(args)
{
    var SubtractUserVirtualCurrencyRequest = {
	    'PlayFabId' : args.PlayerToRemoveID,
	    'VirtualCurrency': args.CCode,
	    'Amount': args.AmountToRemove
    };
    server.SubtractUserVirtualCurrency(SubtractUserVirtualCurrencyRequest);
    return 'Removed Currency';
}
handlers.CompleteIAPPurchase = function(args, context){
    var method = 'post';
    var contentBody = '';
    var contentType = 'application/json';
    var headers = { };
    var url = 'https://graph.oculus.com/5409297455797322/consume_entitlement?nonce=' + args.UserProof + '&user_id=' + args.MetaId + '&sku=' + args.SKU + '&access_token=OC|appId|appSecret';
    var responseString = http.request(url, method, contentBody, contentType, headers);
    if(JSON.parse(responseString).success != undefined){
        if(JSON.parse(responseString).success){
            return true;
        }
    }
    return false;
}
