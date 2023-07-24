using System.Collections;
using System.Collections.Generic;
using PollfishUnity;
using UnityEngine;
using System.Runtime.InteropServices;

namespace PollfishUnity
{
#if UNITY_IPHONE
    public partial class IOSPollfishHelper : IPollfishHelper
    {
        public void Hide()
        {
            HidePollfishFunction();
        }

        public void Init(Pollfish.Params pollfishParams)
        {
            string attributesString = "";

            if (pollfishParams.userProperties != null)
            {

                foreach (KeyValuePair<string, string> kvp in pollfishParams.userProperties)
                {
                    attributesString += kvp.Key + "=" + kvp.Value + "\n";
                }

            }

            string rewardInfoDict = "";

            if (pollfishParams.rewardInfo != null)
            {
                rewardInfoDict = "rewardName=" + pollfishParams.rewardInfo.rewardName + "\n";
                rewardInfoDict += "rewardConversion=" + pollfishParams.rewardInfo.rewardConversion;
            }

            PollfishInitWith((int) pollfishParams.indicatorPosition,
                pollfishParams.indicatorPadding,
                pollfishParams.apiKey,
                pollfishParams.releaseMode,
                pollfishParams.rewardMode,
                pollfishParams.requestUUID,
                attributesString,
                pollfishParams.offerwallMode,
                pollfishParams.clickId,
                pollfishParams.signature,
                rewardInfoDict);

            SetEventObjectNameFunction("PollfishSDK");
        }

        public bool IsPollfishPanelOpen()
        {
            return IsPollfishPanelOpenFunction();
        }

        public bool IsPollfishPresent()
        {
            return IsPollfishPresentFunction();
        }

        public void ShouldQuit()
        {
            // NO OP
        }

        public void Show()
        {
            ShowPollfishFunction();
        }

    #region API

        [DllImport("__Internal")]
        private static extern void PollfishInitWith(int position, int padding, string api_key, bool releaseMode, bool rewardMode, string request_uuid, string attrDict, bool offerwallMode, string clickId, string signature, string rewardInfoDict);

        [DllImport("__Internal")]
        private static extern void ShowPollfishFunction();

        [DllImport("__Internal")]
        private static extern void HidePollfishFunction();

        [DllImport("__Internal")]
        private static extern void SetEventObjectNameFunction(string gameObjName);

        [DllImport("__Internal")]
        private static extern bool IsPollfishPresentFunction();

        [DllImport("__Internal")]
        private static extern bool IsPollfishPanelOpenFunction();

    #endregion

    }
#endif
}