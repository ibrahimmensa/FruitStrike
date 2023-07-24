//
//  TapdaqUnityIOS.h
//  TapdaqUnity
//
//  Created by Rheo Violenes on 05/05/15.
//  Copyright (c) 2015 Nerd. All rights reserved.
//
#import <Foundation/Foundation.h>
#import <Tapdaq/Tapdaq.h>

void TD_LaunchMediationDebugger();

void TD_ConfigureTapdaq(const char* appIdChar,
                      const char* clientKeyChar,
                      const char* testDevices,
                      bool isDebugMode,
                      bool autoReloadAds,
                      const char* pluginVersion,
                      int isUserSubjectToGDPR,
                      int isConsentGiven,
                      int isAgeRestrictedUser,
                      const char* userIdChar,
                      bool forwardUserId);

void TD_SetDelegate();

bool TD_IsInitialised();

void TD_SetPluginTools(const char* unityVersion);

void TD_SetUserSubjectToGDPR(int userSubjectToGDPR);
int TD_UserSubjectToGDPR();

void TD_SetGdprConsent(int gdprConsent);
int TD_GdprConsent();

void TD_SetAgeRestrictedUser(int ageRestrictedUser);
int TD_AgeRestrictedUser();

void TD_SetUserSubjectToUSPrivacy(int userSubjectToUSPrivacy);
int TD_UserSubjectToUSPrivacy();

void TD_SetUSPrivacy(int usPrivacy);
int TD_USPrivacy();

void TD_SetAdMobContentRating(const char* rating);
const char* TD_GetAdMobContentRating();

void TD_SetAdvertiserTracking(int status);
int TD_AdvertiserTracking();

void TD_SetUserId(const char* userId);
const char* TD_GetUserId();

void TD_SetForwardUserId(bool forwardUserId);
bool TD_ShouldForwardUserId();

void TD_SetMuted(bool muted);
bool TD_IsMuted();

void TD_SetUserDataString(const char* key, const char* value);
void TD_SetUserDataInteger(const char* key, int value);
void TD_SetUserDataBoolean(const char* key, bool value);

const char* TD_GetUserDataString(const char* key);
int TD_GetUserDataInteger(const char* key);
bool TD_GetUserDataBoolean(const char* key);

const char* TD_GetAllUserData();

void TD_RemoveUserData(const char* key);

const char * TD_GetNetworkStatuses();
// banner

/**
 * Loads a banner.
 * @param size A string. Must be one of following values: TDMBannerStandard, TDMBannerLarge, TDMBannerMedium, TDMBannerFull, TDMBannerLeaderboard, TDMBannerSmartPortrait, TDMBannerSmartLandscape
 */
void TD_LoadBannerForSize(const char* tagChar, const char* sizeChar);

void TD_LoadBannerWithSize(const char* tagChar, int width, int height);

bool TD_IsBannerReady(const char* tagChar);

void TD_ShowBanner(const char* tagChar, const char* position);

void TD_ShowBannerWithPosition(const char* tagChar, int x, int y);

void TD_HideBanner(const char* tagChar);

void TD_DestroyBanner(const char* tagChar);

// interstitial

void TD_LoadInterstitialWithTag(const char *tagChar);

bool TD_IsInterstitialReadyWithTag(const char *tagChar);

const char * TD_GetInterstitialFrequencyCapError(const char *tagChar);

void TD_ShowInterstitialWithTag(const char* tagChar);

// video

void TD_LoadVideoWithTag(const char *tagChar);

bool TD_IsVideoReadyWithTag(const char *tagChar);

const char * TD_GetVideoFrequencyCapError(const char *tagChar);

void TD_ShowVideoWithTag(const char* tagChar);

// reward video

void TD_LoadRewardedVideoWithTag(const char *tagChar);

bool TD_IsRewardedVideoReadyWithTag(const char *tagChar);

const char * TD_GetRewardedVideoFrequencyCapError(const char *tagChar);

void TD_ShowRewardedVideoWithTag(const char* tagChar, const char* hashedUserIdChar);

bool TD_isEmpty(const char *str);

// stats

void TD_SendIAP(const char* transationId, const char* productId, const char* name, double price, const char* currency, const char* locale);

// rewards

const char *TD_GetRewardId(const char* tag);

@interface TapdaqUnityIOS : NSObject

+ (instancetype)sharedInstance;

- (void)initWithApplicationId:(NSString *)appID
                    clientKey:(NSString *)clientKey
                  testDevices:(NSString *)testDevices
                  isDebugMode:(bool)isDebugMode
                autoReloadAds:(bool)autoReloadAds
                pluginVersion:(NSString *)pluginVersion
          isUserSubjectToGDPR:(int)isUserSubjectToGDPR
               isConsentGiven:(int)isConsentGiven
          isAgeRestrictedUser:(int)isAgeRestrictedUser
                       userId:(NSString*)userId
          shouldForwardUserId:(bool)forwardUserId;
    
- (void) setDelegate;

-(BOOL) IsInitialised;

- (void) setUserSubjectToGDPR:(int)userSubjectToGDPR;

- (int) userSubjectToGDPR;

- (void) setGdprConsent:(int)gdprConsent;

- (int) gdprConsent;

- (void) setAgeRestrictedUser:(int)ageRestrictedUser;

- (int) ageRestrictedUser;

- (void) setUserSubjectToUSPrivacy:(int)userSubjectToUSPrivacy;

- (int) userSubjectToUSPrivacy;

- (void) setUSPrivacy:(int)usPrivacy;

- (int) usPrivacy;

- (void) setAdMobContentRating:(NSString *)rating;

- (NSString *) getAdMobContentRating;

- (void) setUserId:(NSString *)userId;

- (NSString *) getUserId;

- (void) setForwardUserId:(BOOL)forwardUserId;

- (BOOL) shouldForwardUserId;

- (void) setMuted:(BOOL)muted;

- (BOOL) isMuted;

- (void) setUserDataString:(NSString*)value forKey:(NSString*)key;
    
- (void) setUserDataBoolean:(BOOL)value forKey:(NSString*)key;

- (void) setUserDataInteger:(NSInteger)value forKey:(NSString*)key;

- (NSString*) userDataStringForKey:(NSString*) key;

- (NSInteger) userDataIntegerForKey:(NSString*) key;

- (BOOL) userDataBooleanForKey:(NSString*) key;

- (NSString*) userData;

- (void) removeUserDataForKey:(NSString*) key;
@end
