using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PollfishUnity;

public class PollfishDemo : MonoBehaviour
{
#if UNITY_ANDROID || UNITY_IPHONE

	// Pollfish configuration
#if UNITY_ANDROID

	public string apiKey = "YOUR_ANDROID_API_KEY";

#elif UNITY_IPHONE
		
	public string apiKey = "YOUR_IOS_API_KEY";
				
#endif

	public Position indicatorPosition = Position.MIDDLE_LEFT;
	public bool releaseMode = false;
	private bool offerwallMode = false;
	private bool rewardMode = false;
	public int indicatorPadding = 10;
	public string requestUUID = "REQUEST_UUID";
	public string signature = "SIGNATURE";
	public string clickId = "CLICK_ID";
	public RewardInfo rewardInfo = new RewardInfo("REWARD_NAME", 1.3);

	// Send user demographic attributes to shorten or skip demographic surveys
	Dictionary<string, string> userProperties = new Dictionary<string, string>
		{
			{ "gender", "1" },
			{ "year_of_birth", "1974" },
			{ "marital_status", "2" },
			{ "parental", "3" },
			{ "education", "1" },
			{ "employment", "1" },
			{ "career", "2" },
			{ "race", "3" },
			{ "income", "1" }
		};


	private bool isPaused = false;

	private string[] text = new string[] {
		"Show/Hide", "Rewarded Survey", "Offerwall"
	};

	private static int curGridInt;
	private static int oldGridInt = -1;

	private GUIStyle buttonStyle;
	private GUIStyle labelStyle;

	// Pollfish surveys status
	private bool surveyCompleted = false;
	private bool surveyRejected = false; 

	void OnApplicationPause(bool pause)
	{

		Debug.Log ("PollfishDemo - OnApplicationPaused: " + pause);

		if (pause) {

			// We are in background
			isPaused = true;

		} else {

			// We are in foreground again.
			isPaused = false;			

		}
	}

    public void Update()
	{		
		/* Handling Android back event */	

		if (Input.GetKeyDown(KeyCode.Escape)) {

			Pollfish.ShouldQuit();

		}
		
		if (!isPaused) { // resume
			
			Time.timeScale = 1;
				
		} else if (isPaused) { // pause
			
			Time.timeScale = 0;		
		}

	}

	public void OnEnable()
	{
		Debug.Log ("PollfishDemo - OnEnable");

		SubscribeToPollfishEvents();

		InitializePollfish();
	}

    public void OnDisable()
    {
		Debug.Log("PollfishDemo - OnDisable");

		UnsubscribeFromPollfishEvents();
	}

	private void SubscribeToPollfishEvents()
    {
		Pollfish.SurveyCompletedEvent += SurveyCompleted;
		Pollfish.SurveyOpenedEvent += SurveyOpened;
		Pollfish.SurveyClosedEvent += SurveyClosed;
		Pollfish.SurveyReceivedEvent += SurveyReceived;
		Pollfish.SurveyNotAvailableEvent += SurveyNotAvailable;
		Pollfish.UserNotEligibleEvent += UserNotEligible;
		Pollfish.UserRejectedSurveyEvent += UserRejectedSurvey;
	}

	private void UnsubscribeFromPollfishEvents()
    {
		Pollfish.SurveyCompletedEvent -= SurveyCompleted;
		Pollfish.SurveyOpenedEvent -= SurveyOpened;
		Pollfish.SurveyClosedEvent -= SurveyClosed;
		Pollfish.SurveyReceivedEvent -= SurveyReceived;
		Pollfish.SurveyNotAvailableEvent -= SurveyNotAvailable;
		Pollfish.UserNotEligibleEvent -= UserNotEligible;
		Pollfish.UserRejectedSurveyEvent -= UserRejectedSurvey;
	}

    void OnGUI ()
	{

		buttonStyle = new GUIStyle(GUI.skin.button);
		buttonStyle.fontSize = 33;

		labelStyle = new GUIStyle();

		labelStyle.fontSize = 18;
		labelStyle.normal.textColor = Color.black;
		labelStyle.alignment = TextAnchor.MiddleCenter;


		curGridInt = GUI.SelectionGrid(new Rect(10, Screen.height-200, Screen.width - 20, 60), curGridInt, text, 3,buttonStyle);

		if (curGridInt == 0) {

			if (GUI.Button (new Rect (10, 150, 100, 60), "Show", buttonStyle)) {

				Debug.Log ("PollfishDemo: Show Pollfish Button pressed.");

				Pollfish.Show();

			}

			if (GUI.Button (new Rect (10, 230, 100, 60), "Hide", buttonStyle)) {

				Debug.Log ("PollfishDemo: Hide Pollfish Button pressed.");

				Pollfish.Hide();

			}
		}

		// Rewarded mode (both Rewarded Survey & Offerwall approach)

		if ((!surveyCompleted && Pollfish.IsPollfishPresent() && (curGridInt == 1))|| (Pollfish.IsPollfishPresent() && (curGridInt == 2))) {

			if (GUI.Button (new Rect (10, 260, Screen.width - 20, 60), (curGridInt== 1) ? "Complete survey to win coins" : "Survey Offerwall", buttonStyle)) {

				Pollfish.Show();

			}

			// Rewarded mode after completion of survey

        } else if ((curGridInt == 1) && surveyRejected)
        {

            labelStyle.fontSize = 32;
            labelStyle.normal.textColor = Color.black;
            labelStyle.alignment = TextAnchor.MiddleCenter;

            GUI.Label(new Rect(10, 260, Screen.width - 20, 100), "YOU REJECTED THE SURVEY", labelStyle);

        } else if((curGridInt == 1) && surveyCompleted) {

			labelStyle.fontSize = 32;
			labelStyle.normal.textColor = Color.black;
			labelStyle.alignment = TextAnchor.MiddleCenter;

			GUI.Label(new Rect (10, 260, Screen.width - 20, 100), "YOU WON COINS", labelStyle);
        }


		labelStyle.fontSize = 30;
		labelStyle.normal.textColor = Color.red;
		labelStyle.alignment = TextAnchor.MiddleCenter;

		// Standard or Rewarded Mode of demo scene

		if (curGridInt == 0) {

			GUI.Label(new Rect(Screen.width / 2 - 200, 110, 400, 40), "Standard - Show/Hide", labelStyle);

		} else if (curGridInt == 1) {

			GUI.Label(new Rect(Screen.width / 2 - 200, 110, 400, 40), "Rewarded Survey", labelStyle);

		} else if (curGridInt == 2) {

			GUI.Label(new Rect(Screen.width / 2 - 200, 110, 400, 40), "Offerwall Mode", labelStyle);

		}

		if (GUI.changed)
		{
			if (oldGridInt != curGridInt)
			{
				Debug.Log("PollfishDemo - User changed mode");

				oldGridInt = curGridInt;
			
				InitializePollfish();
			}
		}
	}

	private void ResetPollfishSurveysStatus()
    {
		surveyCompleted = false;
		surveyRejected = false;
    }

	private void InitializePollfish()
	{ 
        rewardMode = curGridInt != 0;
		offerwallMode = curGridInt == 2;

		ResetPollfishSurveysStatus();

		Pollfish.Params pollfishParams = new Pollfish.Params(apiKey)
            .OfferwallMode(offerwallMode)
            .IndicatorPadding(indicatorPadding)
            .ReleaseMode(releaseMode)
            .RewardMode(rewardMode)
            .IndicatorPosition(indicatorPosition)
            .RequestUUID(requestUUID)
            .UserProperties(userProperties)
            .ClickId(clickId)
            .Signature(signature)
            .RewardInfo(rewardInfo);

        Pollfish.Init(pollfishParams);
	}

	// Pollfish Event Listeners

	public void SurveyCompleted(SurveyInfo surveyInfo)
	{
		Debug.Log("PollfishDemo: Survey was Completed - " + surveyInfo.ToString());

		surveyCompleted = true;
		surveyRejected = false;
	}

	public void SurveyReceived(SurveyInfo surveyInfo)
	{
		if (surveyInfo == null)
		{

			Debug.Log("PollfishDemo: Survey Offerwall received");

		}
		else
		{

			Debug.Log("PollfishDemo: Survey was completed - " + surveyInfo.ToString());

		}

		surveyCompleted = false;
		surveyRejected = false;
	}

	public void SurveyOpened()
	{
		Debug.Log("PollfishDemo: Survey was opened");

		isPaused = true; // pause scene 
	}

	public void SurveyClosed()
	{
		Debug.Log("PollfishDemo: Survey was closed");

		isPaused = false; // resume scene 
	}

	public void SurveyNotAvailable()
	{
		Debug.Log("PollfishDemo: Survey not available");

		surveyCompleted = false;
		surveyRejected = false;
	}

	public void UserNotEligible()
	{
		Debug.Log("PollfishDemo: User not eligible");

		surveyCompleted = false;
		surveyRejected = false;
	}

	public void UserRejectedSurvey()
	{
		Debug.Log("PollfishDemo: User rejected survey");

		surveyCompleted = false;
		surveyRejected = true;
	}

#endif

}