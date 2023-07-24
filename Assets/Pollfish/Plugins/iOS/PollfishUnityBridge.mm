#import "PollfishUnityBridge.h"

extern void UnitySendMessage(const char * className,const char * methodName, const char * param);

@implementation PollfishUnityBridge

NSString *gameObj; // the main game object in Unity that will listen for the events

//iOS-to-Unity, methods get called when the notification triggers

- (void)pollfishOpened
{
    UnitySendMessage([gameObj UTF8String], "surveyOpened","");
}

- (void)pollfishClosed
{
    UnitySendMessage([gameObj UTF8String], "surveyClosed","");
}

- (void)pollfishUserNotEligible
{
    UnitySendMessage([gameObj UTF8String], "userNotEligible","");
}

- (void)pollfishUserRejectedSurvey
{
    UnitySendMessage([gameObj UTF8String],"userRejectedSurvey","");
}

- (void)pollfishSurveyNotAvailable
{
    UnitySendMessage([gameObj UTF8String],"surveyNotAvailable","");
}

- (void)pollfishSurveyReceivedWithSurveyInfo:(SurveyInfo *)surveyInfo
{
    int surveyPrice = [[surveyInfo cpa] intValue];
    int surveyIR = [[surveyInfo ir] intValue];
    int surveyLOI = [[surveyInfo loi] intValue];
    int rewardValue = [[surveyInfo rewardValue] intValue];
    int remainingCompletes = [[surveyInfo remainingCompletes] intValue];
    
    NSString *surveyClass = [surveyInfo surveyClass];
    NSString *rewardName = [surveyInfo rewardName];
    
    const char *result = (surveyInfo!=nil) ? [[NSString stringWithFormat:@"%d,%d,%d,%@,%@,%d,%d", surveyPrice, surveyIR, surveyLOI, surveyClass, rewardName, rewardValue, remainingCompletes] UTF8String]: "";
    
    UnitySendMessage([gameObj UTF8String], "surveyReceived", result);
}

- (void)pollfishSurveyCompletedWithSurveyInfo:(SurveyInfo *)surveyInfo
{
    int surveyPrice = [[surveyInfo cpa] intValue];
    int surveyIR = [[surveyInfo ir] intValue];
    int surveyLOI = [[surveyInfo loi] intValue];
    int rewardValue = [[surveyInfo rewardValue] intValue];
    int remainingCompletes = [[surveyInfo remainingCompletes] intValue];
    
    NSString *surveyClass = [surveyInfo surveyClass];
    NSString *rewardName = [surveyInfo rewardName];
    
    const char *result = [[NSString stringWithFormat:@"%d,%d,%d,%@,%@,%d,%d", surveyPrice, surveyIR, surveyLOI, surveyClass, rewardName, rewardValue, remainingCompletes] UTF8String];
    
    UnitySendMessage([gameObj UTF8String], "surveyCompleted", result);
}

@end

//Unity-to-iOS, will get called when Unity method is called.
extern "C"
{

PollfishUnityBridge *delegate = nil;

    void PollfishInitWith(int position,
                          int padding,
                          const char *apiKey,
                          bool releaseMode,
                          bool rewardMode,
                          const char *requestUUID,
                          const char *attributes,
                          bool offerwallMode,
                          const char *clickId,
                          const char *signature,
                          const char *rewardInfo) {
        
        PollfishParams *params = [[PollfishParams alloc] init: [NSString stringWithUTF8String: apiKey ? apiKey : ""]];
        
        [params indicatorPosition: (IndicatorPosition) position];
        [params indicatorPadding: padding];
        [params releaseMode: releaseMode];
        [params offerwallMode: offerwallMode];
        [params rewardMode: rewardMode];
        [params requestUUID: [NSString stringWithUTF8String :requestUUID ? requestUUID : ""]];
        [params platform: PlatformUnity];
        [params clickId: [NSString stringWithUTF8String: clickId ? clickId : ""]];
        [params signature: [NSString stringWithUTF8String: signature ? signature : ""]];
        
        NSString *rewardInfoString = [NSString stringWithUTF8String: rewardInfo ? rewardInfo : ""];
        
        if (![rewardInfoString  isEqual: @""]) {
            NSArray *rewardInfoArray = [rewardInfoString componentsSeparatedByString:@"\n"];
            
            NSString *rewardNameKeyValuePair = [rewardInfoArray objectAtIndex: 0];
            
            NSRange range = [rewardNameKeyValuePair rangeOfString:@"="];
            
            NSString *rewardName = @"";
            
            if (range.location != NSNotFound) {
                rewardName = [rewardNameKeyValuePair substringFromIndex:range.location + 1];
            }
            
            NSString *rewardConversionKeyValuePair = [rewardInfoArray objectAtIndex: 1];
            range = [rewardConversionKeyValuePair rangeOfString:@"="];
            
            double rewardConversion = 0;
            
            if (range.location != NSNotFound) {
                rewardConversion = [[rewardConversionKeyValuePair substringFromIndex:range.location + 1] doubleValue];
            }
        
            [params rewardInfo: [[RewardInfo alloc]
                                 initWithRewardName:rewardName
                                 rewardConversion:rewardConversion]];
        }
        
        NSString *userPropertiesString = [NSString stringWithUTF8String: attributes ? attributes : ""];
        
        if (![userPropertiesString isEqual: @""]) {
            UserProperties *userProperties = [[UserProperties alloc] init];
            NSArray *attributesArray = [userPropertiesString componentsSeparatedByString:@"\n"];
            
            for (int i=0; i < [attributesArray count]; i++) {
                
                NSString *keyValuePair = [attributesArray objectAtIndex:i];
                NSRange range = [keyValuePair rangeOfString:@"="];
                
                if (range.location != NSNotFound) {
                    
                    NSString *key = [keyValuePair substringToIndex:range.location];
                    NSString *value = [keyValuePair substringFromIndex:range.location + 1];
                    
                    [userProperties customAttribute:value forKey:key];
                }
            }
            
            [params userProperties:userProperties];
        }
        
        if (delegate == nil) {
            delegate = [[PollfishUnityBridge alloc] init];
        }
        
        [Pollfish initWith:params delegate:delegate];
    }
    
    void ShowPollfishFunction(){
        [Pollfish show];
    }
    
    void HidePollfishFunction(){
        [Pollfish hide];
    }
    
    bool IsPollfishPresentFunction() {
        return [Pollfish isPollfishPresent];
    }

    bool IsPollfishPanelOpenFunction() {
        return [Pollfish isPollfishPanelOpen];
    }
    
    void SetEventObjectNameFunction(const char * gameObjName) {
        gameObj = [[NSString stringWithUTF8String: gameObjName ? gameObjName : ""] copy];
    }

}
