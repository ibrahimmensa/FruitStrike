//
//  TapdaqUnityIOS.m
//  TapdaqUnity
//
//  Created by Rheo Violenes on 05/05/15.
//  Copyright (c) 2015 Nerd. All rights reserved.
//
#import "TapdaqUnityIOS.h"
#import "TapdaqDelegates.h"
#import "TapdaqStandardAd.h"
#import "JsonHelper.h"

static NSString *const kTapdaqLogPrefix = @"[TapdaqUnity]";

// Helper method to create C string copy
char* TDMakeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

void TD_ConfigureTapdaq(const char* appIdChar,
                      const char* clientKeyChar,
                      const char* testDevicesChar,
                      bool isDebugMode,
                      bool autoReloadAds,
                      const char* pluginVersion,
                      int isUserSubjectToGDPR,
                      int isConsentGiven,
                      int isAgeRestrictedUser,
                      const char* userIdChar,
                      bool forwardUserId) {
    
    bool isValid = true;
    
    if (TD_isEmpty(appIdChar)) {
        NSLog(@"%@ iOS App ID is empty", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (TD_isEmpty(clientKeyChar)) {
        NSLog(@"%@ iOS Client Key is empty", kTapdaqLogPrefix);
        isValid = false;
    }
    
    if (!isValid) {
        NSLog(@"%@ Tapdaq did not initialise", kTapdaqLogPrefix);
        return;
    }
    
    NSString *appId = [[NSString stringWithUTF8String:appIdChar] copy];
    NSString *clientKey = [[NSString stringWithUTF8String:clientKeyChar] copy];
    NSString *testDevices = [[NSString stringWithUTF8String:testDevicesChar] copy];
    NSString *version = [[NSString stringWithUTF8String:pluginVersion] copy];
    
    NSString *userId = nil;
    
    if (!TD_isEmpty(userIdChar)) {
        userId = [[NSString stringWithUTF8String:userIdChar] copy];
    }
    
    
    TDProperties *properties = [[Tapdaq sharedSession] properties];
    [properties setPluginVersion:version];
    properties.isDebugEnabled = isDebugMode;
    TDLogger.logLevel = isDebugMode ? TDLogLevelDebug : TDLogLevelInfo;
    [properties setAutoReloadAds:autoReloadAds];
    properties.userId = userId;
    properties.forwardUserId = forwardUserId;

    [properties.privacySettings setUserSubjectToGdpr:(TDPrivacyStatus)isUserSubjectToGDPR];
    [properties.privacySettings setGdprConsentGiven:(TDPrivacyStatus)isConsentGiven];
    [properties.privacySettings setAgeRestrictedUser:(TDPrivacyStatus)isAgeRestrictedUser];
    
    NSData *data = [testDevices dataUsingEncoding:NSUTF8StringEncoding];
    
    NSError *error = nil;
    NSDictionary *testDevicesDictionary = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
    
    if(error == nil) {
        NSArray* amArray = testDevicesDictionary[@"adMobDevices"];
        NSArray* fbArray = testDevicesDictionary[@"facebookDevices"];
        
        if(amArray != nil) {
            TDTestDevices *amTestDevices = [[TDTestDevices alloc] initWithNetwork:TDMAdMob testDevices:amArray];
            [properties registerTestDevices: amTestDevices];
        }
        
        if(fbArray != nil) {
            TDTestDevices *fbTestDevices = [[TDTestDevices alloc] initWithNetwork:TDMFacebookAudienceNetwork testDevices:fbArray];
            [properties registerTestDevices: fbTestDevices];
        }
    }
    
    [[Tapdaq sharedSession] setApplicationId:appId
                                   clientKey:clientKey
                                  properties:properties];
}

void TD_SetDelegate() {
    Tapdaq.sharedSession.delegate = TapdaqDelegates.sharedInstance;
}

bool TD_IsInitialised() {
    return Tapdaq.sharedSession.isInitialised;
}

void TD_SetPluginTools(const char* unityVersion) {
    if(!TD_isEmpty(unityVersion)) {
        NSString *version = [[NSString stringWithUTF8String:unityVersion] copy];
        [Tapdaq setPluginToolsVersion:version userDefaults:NSUserDefaults.standardUserDefaults];
    }
}

void TD_SetUserSubjectToGDPR(int userSubjectToGDPR) {
    Tapdaq.sharedSession.properties.privacySettings.userSubjectToGdpr = (TDPrivacyStatus)userSubjectToGDPR;
}

int TD_UserSubjectToGDPR() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.userSubjectToGdpr;
}

void TD_SetGdprConsent(int gdprConsent) {
    Tapdaq.sharedSession.properties.privacySettings.gdprConsentGiven = (TDPrivacyStatus)gdprConsent;
}

int TD_GdprConsent() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.gdprConsentGiven;
}

void TD_SetAgeRestrictedUser(int ageRestrictedUser) {
    Tapdaq.sharedSession.properties.privacySettings.ageRestrictedUser = (TDPrivacyStatus)ageRestrictedUser;
}

int TD_AgeRestrictedUser() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.ageRestrictedUser;
}

void TD_SetUserSubjectToUSPrivacy(int userSubjectToUSPrivacy) {
    Tapdaq.sharedSession.properties.privacySettings.userSubjectToUSPrivacy = (TDPrivacyStatus)userSubjectToUSPrivacy;
}

int TD_UserSubjectToUSPrivacy() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.userSubjectToUSPrivacy;
}

void TD_SetUSPrivacy(int usPrivacy) {
    Tapdaq.sharedSession.properties.privacySettings.usPrivacyDoNotSell = (TDPrivacyStatus)usPrivacy;
}

int TD_USPrivacy() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.usPrivacyDoNotSell;
}

void TD_SetAdMobContentRating(const char * rating) {
    NSString *ratingString = [NSString stringWithUTF8String:rating].copy;
    Tapdaq.sharedSession.properties.adMobContentRating = ratingString;
}

const char * TD_GetAdMobContentRating() {
    NSString * ratingStr = Tapdaq.sharedSession.properties.adMobContentRating;
    return TDMakeStringCopy([ratingStr UTF8String]);
}

void TD_SetAdvertiserTracking(int status) {
    Tapdaq.sharedSession.properties.privacySettings.advertiserTracking = (TDPrivacyStatus)status;
}

int TD_AdvertiserTracking() {
    return (int)Tapdaq.sharedSession.properties.privacySettings.advertiserTracking;
}

void TD_SetUserId(const char * userId) {
    NSString *userIdStrting = nil;
    if (!TD_isEmpty(userId)) {
        userIdStrting = [[NSString stringWithUTF8String:userId] copy];
    }
    Tapdaq.sharedSession.properties.userId = userIdStrting;
}

const char * TD_GetUserId() {
    NSString * userIdStr = Tapdaq.sharedSession.properties.userId;
    return TDMakeStringCopy([userIdStr UTF8String]);
}

void TD_SetForwardUserId(bool forwardUserId) {
    Tapdaq.sharedSession.properties.forwardUserId = forwardUserId;
}

bool TD_ShouldForwardUserId() {
    return (bool)Tapdaq.sharedSession.properties.forwardUserId;
}

void TD_SetMuted(bool muted) {
    Tapdaq.sharedSession.properties.muted = muted;
}

bool TD_IsMuted() {
    return (bool)Tapdaq.sharedSession.properties.isMuted;
}

void TD_SetUserDataString(const char* key, const char* value) {
    NSString *keyStr = nil;
    NSString *valueStr = nil;
    if (!TD_isEmpty(key) && !TD_isEmpty(value)) {
        keyStr = [NSString stringWithUTF8String:key];
        valueStr = [NSString stringWithUTF8String:value];
    }
    [Tapdaq.sharedSession.properties setUserDataString:valueStr forKey:keyStr];
}

void TD_SetUserDataInteger(const char* key, int value) {
    NSString *keyStr = nil;
    if (!TD_isEmpty(key)) {
        keyStr = [NSString stringWithUTF8String:key];
    }
    [Tapdaq.sharedSession.properties setUserDataInteger:value forKey:keyStr];
}

void TD_SetUserDataBoolean(const char* key, bool value) {
    NSString *keyStr = nil;
    if (!TD_isEmpty(key)) {
        keyStr = [NSString stringWithUTF8String:key];
    }
    [Tapdaq.sharedSession.properties setUserDataBool:value forKey:keyStr];
}

const char* TD_GetUserDataString(const char* key) {
    NSString *keyStr = nil;
    if (!TD_isEmpty(key)) {
        keyStr = [[NSString stringWithUTF8String:key] copy];
    }
    
    NSString * dataStr = [Tapdaq.sharedSession.properties userDataStringForKey:keyStr];;
    return TDMakeStringCopy([dataStr UTF8String]);
}

int TD_GetUserDataInteger(const char* key) {
    NSString *keyStr = nil;
    if (!TD_isEmpty(key)) {
        keyStr = [[NSString stringWithUTF8String:key] copy];
    }
    
    return (int)[Tapdaq.sharedSession.properties userDataIntegerForKey:keyStr];
}

bool TD_GetUserDataBoolean(const char* key) {
    NSString *keyStr = nil;
    if (!TD_isEmpty(key)) {
        keyStr = [[NSString stringWithUTF8String:key] copy];
    }
    
    return (bool)[Tapdaq.sharedSession.properties userDataBoolForKey:keyStr];
}

const char* TD_GetAllUserData() {
    NSString * dataStr = [JsonHelper toJsonString:Tapdaq.sharedSession.properties.userData];
    return TDMakeStringCopy([dataStr UTF8String]);
}

void TD_RemoveUserData(const char* key) {
    NSString *keyStr = nil;
    if (!TD_isEmpty(key)) {
        keyStr = [[NSString stringWithUTF8String:key] copy];
    }
    [Tapdaq.sharedSession.properties removeUserDataForKey:keyStr];
}

const char* TD_GetNetworkStatuses() {
    NSArray* networkStatuses = [[Tapdaq sharedSession] networkStatusesDictionary];
    if (networkStatuses != nil && [networkStatuses count] > 0) {
        NSString * dataStr = [JsonHelper arrayToJsonString:networkStatuses];
        return TDMakeStringCopy([dataStr UTF8String]);
    }
    return "";
}

#pragma mark - Banner (Bridge)

void TD_LoadBannerForSize(const char* tagChar, const char* sizeChar) {
    
    if (TD_isEmpty(sizeChar)) {
        NSLog(@"%@ No banner size specified, cannot load banner", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    NSString *sizeStr = [[NSString stringWithUTF8String:sizeChar] copy];
    [[TapdaqBannerAd sharedInstance] loadForPlacementTag:tagStr withSize:sizeStr];
}

void TD_LoadBannerWithSize(const char* tagChar, int width, int height) {
    if (TD_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, cannot load banner", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    CGSize size = CGSizeMake(width, height);
    
    [[TapdaqBannerAd sharedInstance] loadForPlacementTag:tagStr withCustomSize:size];
}

bool TD_IsBannerReady(const char* tagChar) {
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    return (bool) [[TapdaqBannerAd sharedInstance] isReadyForPlacementTag:tagStr];
    
}

void TD_ShowBanner(const char* tagChar, const char* position) {
    
    if (TD_isEmpty(position)) {
        NSLog(@"%@ No banner position given, failed to show banner", kTapdaqLogPrefix);
        return;
    }
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    [[TapdaqBannerAd sharedInstance] showForPlacementTag:tagStr withPosition:position];
    
}

void TD_ShowBannerWithPosition(const char* tagChar, int x, int y) {
    if (TD_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show banner", kTapdaqLogPrefix);
        return;
    }
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    [[TapdaqBannerAd sharedInstance] showForPlacementTag:tagStr atPositionX:x atPositionY:y];
}

void TD_HideBanner(const char* tagChar) {
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    [[TapdaqBannerAd sharedInstance] hideForPlacementTag:tagStr];
}

void TD_DestroyBanner(const char* tagChar) {
    NSString *tagStr = [[NSString stringWithUTF8String:tagChar] copy];
    [[TapdaqBannerAd sharedInstance] destroyForPlacementTag:tagStr];
}

#pragma mark - Interstitial (Bridge)

void TD_LoadInterstitialWithTag(const char *tagChar) {
    
    if (TD_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load interstitial", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqInterstitialAd sharedInstance] loadForPlacementTag:tag];
    
}

bool TD_IsInterstitialReadyWithTag(const char *tagChar) {
    
    if (TD_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if interstitial is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqInterstitialAd sharedInstance] isReadyForPlacementTag:tag];
    
}

const char *TD_GetInterstitialFrequencyCapError(const char* tag) {
    NSString *placementTag = [NSString stringWithUTF8String:tag];
    TDError* error = [[Tapdaq sharedSession] interstitialCappedForPlacementTag:placementTag];
    
    NSString* result = [JsonHelper toJsonString:@{
        @"code": @(error.code),
        @"message": (error.localizedDescription == nil ? @"" : error.localizedDescription)
    }];
    
    return TDMakeStringCopy([result UTF8String]);
}

void TD_ShowInterstitialWithTag(const char* tagChar) {
    
    if (TD_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show interstitial", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqInterstitialAd sharedInstance] showForPlacementTag:tag];
    
}

#pragma mark - Video (Bridge)

void TD_LoadVideoWithTag(const char *tagChar) {
    
    if (TD_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqVideoAd sharedInstance] loadForPlacementTag:tag];
    
}

bool TD_IsVideoReadyWithTag(const char *tagChar) {
    
    if (TD_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if video is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqVideoAd sharedInstance] isReadyForPlacementTag:tag];
    
}

const char *TD_GetVideoFrequencyCapError(const char* tag) {
    NSString *placementTag = [NSString stringWithUTF8String:tag];
    TDError* error = [[Tapdaq sharedSession] videoCappedForPlacementTag:placementTag];
    
    NSString* result = [JsonHelper toJsonString:@{
        @"code": @(error.code),
        @"message": (error.localizedDescription == nil ? @"" : error.localizedDescription)
    }];
    
    return TDMakeStringCopy([result UTF8String]);
}

void TD_ShowVideoWithTag(const char* tagChar) {
    
    if (TD_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqVideoAd sharedInstance] showForPlacementTag:tag];
    
}

#pragma mark - Rewarded Video (Bridge)

void TD_LoadRewardedVideoWithTag(const char *tagChar) {
    
    if (TD_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to load rewarded video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    [[TapdaqRewardedVideoAd sharedInstance] loadForPlacementTag:tag];
    
}

bool TD_IsRewardedVideoReadyWithTag(const char *tagChar) {
    
    if (TD_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to check if rewarded video is ready", kTapdaqLogPrefix);
        return false;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    
    return (bool) [[TapdaqRewardedVideoAd sharedInstance] isReadyForPlacementTag:tag];
    
}

const char *TD_GetRewardedVideoFrequencyCapError(const char* tag) {
    NSString *placementTag = [NSString stringWithUTF8String:tag];
    TDError* error = [[Tapdaq sharedSession] rewardedVideoCappedForPlacementTag:placementTag];
    
    NSString* result = [JsonHelper toJsonString:@{
        @"code": @(error.code),
        @"message": (error.localizedDescription == nil ? @"" : error.localizedDescription)
    }];
    
    return TDMakeStringCopy([result UTF8String]);
}

void TD_ShowRewardedVideoWithTag(const char* tagChar, const char* hashedUserIdChar) {
    
    if (TD_isEmpty(tagChar)) {
        NSLog(@"%@ No tag given, failed to show rewarded video", kTapdaqLogPrefix);
        return;
    }
    
    NSString *tag = [NSString stringWithUTF8String:tagChar];
    NSString *hashedUserId = (hashedUserIdChar != nil ? [NSString stringWithUTF8String:hashedUserIdChar] : nil);
    
    [[TapdaqRewardedVideoAd sharedInstance] showForPlacementTag:tag withHashedUserId:hashedUserId];
    
}

#pragma mart - Stats

void TD_SendIAP(const char* transationId, const char* productId, const char* name, double price, const char* currency, const char* locale) {
    NSString *transactionIdString;
    NSString *productIdString;
    NSString *currencyString;
    NSString *nameString;
    NSString *localeString;
    
    if (transationId != NULL) {
        transactionIdString = [NSString stringWithUTF8String:transationId];
    }
    
    if (productId != NULL) {
        productIdString = [NSString stringWithUTF8String:productId];
    }
    
    if (currency != NULL) {
        currencyString = [NSString stringWithUTF8String:currency];
    }
    
    if (name != NULL) {
        nameString = [NSString stringWithUTF8String:name];
    }
    
    if (locale != NULL) {
        localeString = [NSString stringWithUTF8String:locale];
    }
    
    [[Tapdaq sharedSession] trackPurchaseForProductName:nameString
                                                  price:price
                                            priceLocale:localeString
                                               currency:currencyString
                                          transactionId:transactionIdString
                                              productId:productIdString];
}

const char *TD_GetRewardId(const char* tag) {
    NSString *placementTag = [NSString stringWithUTF8String:tag];
    NSString *rewardId = [[Tapdaq sharedSession] rewardIdForPlacementTag:placementTag];
    return [rewardId cStringUsingEncoding:NSUTF8StringEncoding];
}

void TD_LaunchMediationDebugger() {
    [[Tapdaq sharedSession] presentDebugViewController];
}

bool TD_isEmpty(const char* str) {
    return str == NULL;
}
