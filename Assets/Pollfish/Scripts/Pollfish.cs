using UnityEngine;
using System;
using PollfishUnity;
using System.Collections.Generic;

#if UNITY_ANDROID || UNITY_IPHONE

public class Pollfish : MonoBehaviour
{

    public class Params
    {

        public bool releaseMode { get; set; }
        public bool rewardMode { get; set; }
        public bool offerwallMode { get; set; }
        public int indicatorPadding { get; set; }
        public string requestUUID { get; set; }
        public Position indicatorPosition { get; set; }
        public Dictionary<string, string> userProperties { get; set; }
        public string apiKey { get; }
        public string clickId { get; set; }
        public string signature { get; set; }
        public RewardInfo rewardInfo { get; set; }

        public Params(string apiKey)
        {
            this.releaseMode = false;
            this.rewardMode = false;
            this.offerwallMode = false;
            this.indicatorPadding = 10;
            this.requestUUID = null;
            this.indicatorPosition = Position.TOP_LEFT;
            this.userProperties = null;
            this.apiKey = apiKey;
            this.clickId = null;
            this.signature = null;
            this.rewardInfo = null;
        }

        public Params OfferwallMode(bool offerwallMode)
        {
            this.offerwallMode = offerwallMode;
            return this;
        }

        public Params IndicatorPadding(int indicatorPadding)
        {
            this.indicatorPadding = indicatorPadding;
            return this;
        }

        public Params ReleaseMode(bool releaseMode)
        {
            this.releaseMode = releaseMode;
            return this;
        }

        public Params RewardMode(bool rewardMode)
        {
            this.rewardMode = rewardMode;
            return this;
        }

        public Params IndicatorPosition(Position indicatorPosition)
        {
            this.indicatorPosition = indicatorPosition;
            return this;
        }

        public Params RequestUUID(string requestUUID)
        {
            this.requestUUID = requestUUID;
            return this;
        }

        public Params UserProperties(Dictionary<string, string> userProperties)
        {
            this.userProperties = userProperties;
            return this;
        }

        public Params ClickId(string clickId)
        {
            this.clickId = clickId;
            return this;
        }

        public Params Signature(string signature)
        {
            this.signature = signature;
            return this;
        }

        public Params RewardInfo(RewardInfo rewardInfo)
        {
            this.rewardInfo = rewardInfo;
            return this;
        }

    }

    static IPollfishHelper helper;
    private static Pollfish instance;

    public static Pollfish Instance
    {
        get
        {
            return instance;
        }
    }

    static Pollfish()
    {
        try
        {

            GameObject go = new GameObject("PollfishSDK");
            instance = go.AddComponent<Pollfish>();
            DontDestroyOnLoad(go);
        }
        catch
        {
            Debug.Log("Pollfish.cs will create the Pollfish instance. A Pollfish Object already exists in your scene. Please remove the script from the scene.");
        }

#if UNITY_IOS
        helper = new IOSPollfishHelper();
#elif UNITY_ANDROID
        helper = new AndroidPollfishHelper();
#endif
    }

    /// <summary>
    /// Pollfish initialize function
    /// </summary>
    /// <param name="pollfishParams">Pollfish Configuration object</param>

    public static void Init(Params pollfishParams)
    {
        helper.Init(pollfishParams);
    }

    /// <summary>
    /// Check if Pollfish Panel is Open
    /// </summary>
    /// <returns>true if Pollfish Panel is Open</returns>

    public static bool IsPollfishPanelOpen()
    {
        return helper.IsPollfishPanelOpen();
    }

    /// <summary>
    /// Check if Pollfish Surveys are available on your device
    /// </summary>
    /// <returns>true if Pollfish Surveys are available on your device</returns>

    public static bool IsPollfishPresent()
    {
        return helper.IsPollfishPresent();
    }

    /// <summary>
    /// Manually show Pollfish.
    /// </summary>

    public static void Show()
    {
        helper.Show();
    }

    /// <summary>
    /// Manually hide Pollfish.
    /// </summary>

    public static void Hide()
    {
        helper.Hide();
    }

    /// <summary>
    /// Decide if you should quit your app on back button event (Pollfish is not open)
    /// </summary>

    public static void ShouldQuit()
    {
        helper.ShouldQuit();
    }

    #region Events

    public static event Action SurveyOpenedEvent;
    public static event Action SurveyClosedEvent;
    public static event Action<SurveyInfo> SurveyReceivedEvent;
    public static event Action<SurveyInfo> SurveyCompletedEvent;
    public static event Action SurveyNotAvailableEvent;
    public static event Action UserNotEligibleEvent;
    public static event Action UserRejectedSurveyEvent;

    public void surveyCompleted(string surveyInfo)
    {
        SurveyCompletedEvent?.Invoke(Utils.GetSurveyInfoFromString(surveyInfo));
    }

    public void surveyReceived(string surveyInfo)
    {
        SurveyReceivedEvent?.Invoke(Utils.GetSurveyInfoFromString(surveyInfo));
    }

    public void surveyOpened()
    {
        SurveyOpenedEvent?.Invoke();
    }

    public void surveyClosed()
    {
        SurveyClosedEvent?.Invoke();
    }

    public void surveyNotAvailable()
    {
        SurveyNotAvailableEvent?.Invoke();
    }

    public void userNotEligible()
    {
        UserNotEligibleEvent?.Invoke();
    }

    public void userRejectedSurvey()
    {
        UserRejectedSurveyEvent?.Invoke();
    }

    #endregion

}

#endif