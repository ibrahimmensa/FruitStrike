using System.Collections.Generic;
using UnityEngine;
using System;

namespace PollfishUnity
{
#if UNITY_ANDROID
    public partial class AndroidPollfishHelper : IPollfishHelper
    {
        private readonly AndroidJavaClass mPollfishAssistantClass;

        public AndroidPollfishHelper()
        {
            if (Application.platform != RuntimePlatform.Android)
                return;

            mPollfishAssistantClass = new AndroidJavaClass("com.pollfish.unity.PollfishPlugin");
        }

        public void Hide()
        {
            mPollfishAssistantClass.CallStatic("hide");
        }

        public void Init(Pollfish.Params pollfishParams)
        {
            using (AndroidJavaObject rewardInfoObject = new AndroidJavaObject("java.util.HashMap"))
            using (AndroidJavaObject userPropertiesObject = new AndroidJavaObject("java.util.HashMap"))
            {
                if (pollfishParams.userProperties != null)
                {
                    IntPtr userPropertiesMethodPut = AndroidJNIHelper.GetMethodID(userPropertiesObject.GetRawClass(),
                        "put",
                        "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

                    object[] args = new object[2];

                    foreach (KeyValuePair<string, string> kvp in pollfishParams.userProperties)
                    {
                        using (AndroidJavaObject k = new AndroidJavaObject("java.lang.String", kvp.Key))
                        using (AndroidJavaObject v = new AndroidJavaObject("java.lang.String", kvp.Value))
                        {
                            args[0] = k;
                            args[1] = v;

                            AndroidJNI.CallObjectMethod(userPropertiesObject.GetRawObject(),
                                userPropertiesMethodPut, AndroidJNIHelper.CreateJNIArgArray(args));
                        }
                    }
                }

                if (pollfishParams.rewardInfo != null)
                {
                    IntPtr rewardInfoMethodPut = AndroidJNIHelper.GetMethodID(rewardInfoObject.GetRawClass(),
                       "put",
                       "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

                    object[] args = new object[2];

                    using (AndroidJavaObject rewardNameKey = new AndroidJavaObject("java.lang.String", "rewardName"))
                    using (AndroidJavaObject rewardNameValue = new AndroidJavaObject("java.lang.String", pollfishParams.rewardInfo.rewardName))

                    {
                        args[0] = rewardNameKey;
                        args[1] = rewardNameValue;

                        AndroidJNI.CallObjectMethod(rewardInfoObject.GetRawObject(),
                            rewardInfoMethodPut, AndroidJNIHelper.CreateJNIArgArray(args));
                    }

                    args = new object[2];

                    using (AndroidJavaObject rewardConversionKey = new AndroidJavaObject("java.lang.String", "rewardConversion"))
                    using (AndroidJavaObject rewardConversionValue = new AndroidJavaObject("java.lang.Double", pollfishParams.rewardInfo.rewardConversion))
                    {
                        args[0] = rewardConversionKey;
                        args[1] = rewardConversionValue;

                        AndroidJNI.CallObjectMethod(rewardInfoObject.GetRawObject(),
                            rewardInfoMethodPut, AndroidJNIHelper.CreateJNIArgArray(args));
                    }
                }

                mPollfishAssistantClass.CallStatic("init",
                    pollfishParams.apiKey,
                    (int)pollfishParams.indicatorPosition,
                    pollfishParams.indicatorPadding,
                    pollfishParams.requestUUID,
                    pollfishParams.releaseMode,
                    pollfishParams.rewardMode,
                    userPropertiesObject,
                    pollfishParams.offerwallMode,
                    pollfishParams.clickId,
                    pollfishParams.signature,
                    rewardInfoObject);

                mPollfishAssistantClass.CallStatic("setEventObjectPollfish", "PollfishSDK");
            }
        }

        public bool IsPollfishPanelOpen()
        {
            return mPollfishAssistantClass.CallStatic<bool>("isPollfishPanelOpen");
        }

        public bool IsPollfishPresent()
        {
            return mPollfishAssistantClass.CallStatic<bool>("isPollfishPresent");
        }

        public void ShouldQuit()
        {
            if (!mPollfishAssistantClass.CallStatic<bool>("isPollfishPanelOpen"))
            {
                Application.Quit();
            }
        }

        public void Show()
        {
            mPollfishAssistantClass.CallStatic("show");
        }
    }
#endif
}