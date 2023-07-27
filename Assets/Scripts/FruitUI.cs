using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using OPS.AntiCheat.Field;

public class FruitUI : MonoBehaviour
{
    public static string userName="", email, userID="", photoUrl="", refID="";
    public GameObject userNamePanel, leaderboardPanel, leaderboardPanel2, leaderboard, marketPanel, marketPanel2, premiumPanel, extraLifePanel, 
    skipStagePanel, messageDialog, verDialog, listViewChild, listView, gridViewChild, gridView, ratePanel, sharePanel, listPanel, tournamentChild, joinPanel,
     infoPanel, updatePanel, ratePanel2, loadPanel, tutPanel, cashPanel, shareDialog, loadPanel1, refPanel, refList, refChild, transactionChild, greenRewardButton,
      invitePanel, resultPanel, tournamentLeaderboardChild, BuyPanel, premChild, infoButton, currencyPanel, spinPanel, awardPanel,greenAwardPanel, freeSpinDialog, freeSpearDialog, 
      withdrawPanel, switchPanel, leaderboardGuidePanel, tutorialPanel, freeGreen, MoneyPanel, consentPanel, bidPanel, winnerPanel, winner2Panel, winner3Panel, freeCashPanel, topCashButton,
	  shrIco, rtIco, spearView, expiredPanel, surveyDialog, preSurveyDialog;
    public Button close, tier1,tier2,tier3,buy, shareBonus, rateBonus, noAds, no, ok2, rewAd;
	public InputField accountDetails;
    public Text cash1,cash2,cash3,cash4, profileName, selectedItemprice, buyText, messageText, counterText, txt1, txt2, rulesText, rulesText2, cashText, infoText, cashTextMenu, 
    cashTextGame, shareText, tipsText, refText, refText1, listHead, listHeadb, listHead2, joinName, joinGoal, joinFee, joinPrize, resTournamentName, resGoal, resScore, resResult, 
    awardText, greenAwardText, tokenText, timeText, PriceText, leaderboardHeadText, bid, newBid, bidGoal, bidGoal2, bidName, preSurveyText,
     bidPrize,bidPlaceHolder, bidTimeLeft, winnerTitle, winner1Name, winner2Name, winner3Name, winner1Prize, winner2Prize, winner3Prize, verText, shrText, rtText;
    public RawImage curImage2, profileImage, consentPage, selectedItemImage, winner1Photo, winner2Photo, winner3Photo, peachIcon, pagImg, resIcon, freeSpear1, freeSpear2, freeSpear3, badge;
    public Text lbInfoText, inviteText, withdrawal1Text, withdrawal2Text, withdrawal3Text, menutext1, menutext2, menutext3, earnText;
	List<int> spears = new List<int>();
    public static int Tier, version=101, newVersion, award, bidAmount;
	public static float funds1 = 2f, funds2 = 4f, funds3 = 10f, funds4 = 20f;
    private float lastUpdate;
    public static int[] spearPrices = { 300, 300, 300, 300, 500, 500, 500, 800, 1000, 1000, 1000, 1000, 1200, 1200, 1200, 1500, 1800, 1800, 1800, 2000, 2000, 2000, 2000, 2500 };
    public static int equippedItem, selectedItem, currentPrice, rewardInterval, bonus;
    public static Texture2D currentSpear, avatar;
    public static bool infoed, hasRated, hasShared, leaderboardGuide, photo1Set, photo2Set, photo3Set;
    private int rewardTime;
    public static Tournament selectedTournament;
    public static bool isPrem, isNew, startGame, paid, scrollingLeft, scrollingRight;
    public static string currency = "usd", consent = "";
    private string usernameDB = "userrdb", premiumDB = "premmyodb";
    public static string[] tips = {"Stay connected to receive free apples during gameplay",
                                "Invite your freinds and earn cash rewards",
                                "Stuck on a stage?, click on the 'skip' button at the top right",
                                "Feeling lucky? Take a spin at the wheel of fortune and Win Prizes!",
                                 "Green Apples are more frequent as your stage increases"};
    
    public static Leaderboard thisLeaderboard;

    public enum Leaderboard {
		
		Top_Cashouts,
        Todays_Scores,
        Todays_Apples,
        Yesterdays_Apples,
        All_Time_Scores

    }

    public static void noGreen(){
        thisLeaderboard = Leaderboard.All_Time_Scores;
    }

    public static void yesGreen(){
        thisLeaderboard = Leaderboard.Todays_Apples;
    }
	public void openVerDialog(){
		playButtonSound();
		if(!NetworkScript.isMailVerfied){
			verText.text = "A VERIFICATION LINK WAS SENT TO YOUR EMAIL "+email.ToUpper()+"\nCHECK YOUR MAIL & VERIFY YOUR ACCOUNT";
			verDialog.SetActive(true);
		}else
			showMessage("Account Verified");
	}
	public void closeVerDialog(){
		playButtonSound();
		verDialog.SetActive(false);
	}

    void Start()
    {
		
		currency = PlayerPrefs.GetString("language");
		Strings.setLanguage();
		
        int tipNum = UnityEngine.Random.Range(0,tips.Length);
        Debug.Log("Tip "+tipNum);
        tipsText.text = "Tip: " + tips[tipNum];
        Tournament.ui = this;

        if(FruitSpearGameCode.isGreen)
            thisLeaderboard = Leaderboard.Todays_Apples;
        else
            thisLeaderboard = Leaderboard.All_Time_Scores;    

        close.onClick.AddListener(() => closeLeaderboard());
        tier1.onClick.AddListener(() => setupMarket(1));
        tier2.onClick.AddListener(() => setupMarket(2));
        tier3.onClick.AddListener(() => setupMarket(3));
        shareBonus.onClick.AddListener(() => shareButton());
        rateBonus.onClick.AddListener(() => rateButton());
        buy.onClick.AddListener(() => buyItem());
        noAds.onClick.AddListener(() => showBuyPanel());
        no.onClick.AddListener(() => closePremiumPanel());
        ok2.onClick.AddListener(() => closeDialog());
        rewAd.onClick.AddListener(() => getCoinReward());

        marketPanel.transform.localScale = new Vector3(Screen.height / 1280f * 0.9f, Screen.height / 1280f * 0.9f, 0);
        marketPanel2.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        BuyPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        leaderboardPanel.transform.localScale = new Vector3(Screen.height / 1422f, Screen.height / 1422f, 0);
        leaderboardPanel2.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        userNamePanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        premiumPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        messageDialog.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        cashPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        extraLifePanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        loadPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
		updatePanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        listPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        joinPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        bidPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        resultPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        currencyPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        awardPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        greenAwardPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        tutorialPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        winnerPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
		consentPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
		expiredPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        
		consent = PlayerPrefs.GetString("consent");
		
        PlayerPrefs.SetInt("spear1",1);
        newVersion = PlayerPrefs.GetInt("version");

        equippedItem = PlayerPrefs.GetInt("currentSpear");
		
		if(PlayerPrefs.GetInt("leaderboardGuide") == 1)
			leaderboardGuide = true;

        if (equippedItem <= 0)
            equippedItem = 1;
            
        currentSpear = (Texture2D)Resources.Load("Spears/spear" + equippedItem);
        selectedItemImage.texture = currentSpear;
        selectedItemprice.text = "EQUIPPED";
        peachIcon.texture = (Texture2D)Resources.Load("blank");
        Debug.Log("Spear " + equippedItem + " Equipped");

        if (PlayerPrefs.GetString("hasShared").Equals("true"))
        {
            hasShared = true;
            Debug.Log("User has used share bonus");
        }
        if (PlayerPrefs.GetString("hasRated").Equals("true"))
        {
            hasRated = true;
            Debug.Log("User has used rate bonus");
        }
        if (PlayerPrefs.GetString(premiumDB).Equals("true"))
        {
            activatePrem();
        }
        else
        {
            Debug.Log("regular member");
        }
		
		resetStrings();
		
        if(currency==null || currency.Trim().Equals("")){
            openCurrencyPanel();
            isNew = true;
        }
        tokenText.text = "+ 1";
    
    }
	public void activatePrem(){
		Debug.Log("premium member");
        isPrem = true;
        premChild.SetActive(false);
	}
	
	public void openConsentPanel(){
		consentPanel.SetActive(true);
	}
	
	public void consentYes(){
		if(consent == null || consent == ""){
			consent = "yes";
			consentPage.texture = (Texture2D)Resources.Load("consent_yes");
			PlayerPrefs.SetString("consent", consent);
			Debug.Log("Consent Approved");
		}else{
			
			checkConsent();
			consentPanel.SetActive(false);
		}
	}
	
	public void consentNo(){
		if(consent == ""){
			consent = "no";
			consentPage.texture = (Texture2D)Resources.Load("consent_no");
			PlayerPrefs.SetString("consent", consent);
			Debug.Log("Consent Declined");
		}
	}
	
	public void checkConsent(){
		/**
			if(consent == null || consent == "")
				openConsentPanel();
			else if(consent == "no")
				FindObjectOfType<FruitSpearGameCode>().setupAds(false);
			else
				FindObjectOfType<FruitSpearGameCode>().setupAds(true);	
			**/
			
		FindObjectOfType<FruitSpearGameCode>().setupAds();		
		
	}

    public void openWithdrawPanel(){

        StartCoroutine(NetworkScript.GetTotalAmountOfCashFromTransactionHistory((myReturnValue) =>
        {
            int tempCash = myReturnValue;

            if (tempCash != -1)
            {
                ProtectedFloat temp = tempCash + NetworkScript.money;

                if (temp != NetworkScript.myCash)
                {
                    if (temp < NetworkScript.myCash)
                    {
                        Debug.Log("total balance is greater in amount than in transaction history, syncing...");
                        NetworkScript.myCash = temp;
                    }
                }
                else
                {
                    Debug.Log("transactions records and total balance is synced!");
                }
            }
        }));

            playButtonSound();

        if(!userName.Equals("")){
            withdrawPanel.SetActive(true);
            cash1.text = getCurrency(funds1);
            cash2.text = getCurrency(funds2);
            cash3.text = getCurrency(funds3);
			cash4.text = getCurrency(funds4);
			withdrawal1Text.text = Strings.withdrawal1;
			withdrawal2Text.text = Strings.withdrawal2;
			if(NetworkScript.paydate!=0)
				withdrawal2Text.text = "(" + Strings.withdrawal4 + NetworkScript.paydate+"th)";
			withdrawal3Text.text = Strings.withdrawal3;
        }else{
            FindObjectOfType<NetworkScript>().loginText.text = "Log Into Your Account";
            userNamePanel.SetActive(true);
        }
        
    }
	
	public void setBadge(){
		badge.texture = (Texture2D)Resources.Load("ver");
	}

    public void closeWithdrawPanel(){
		withdrawPanel.SetActive(false);
		openFreeCashPanel();
    }
	public void closeFreeCashPanel(){
		playButtonSound();
		freeCashPanel.SetActive(false);
	}
	
	public void openOfferWall(){
		playButtonSound();
		freeCashPanel.SetActive(false);
		FindObjectOfType<FruitSpearGameCode>().showOfferWall();
	}
	public void openFreeCashPanel(){
		earnText.text = Strings.earn;
		playButtonSound();
		freeCashPanel.SetActive(true);
	}
    
    public void openAwardPanel(int amount){
        awardPanel.SetActive(true);
        awardText.text = "+"+amount;
        award = amount;
    }
    public void openGreenAwardPanel(int amount){
        greenAwardPanel.SetActive(true);
        greenAwardText.text = "+"+amount;
        award = amount;
    }
    public void closeAwardPanel(){
        awardPanel.SetActive(false);
        playButtonSound();
        FindObjectOfType<FruitSpearGameCode>().addCoins(award);
		showMessage("+"+award+" Apples");
        if(FruitSpearGameCode.getVIPGreen){
            FruitSpearGameCode.getVIPGreen = false;
            openGreenAwardPanel(200);
        }
    }
    public void closeGreenAwardPanel(){
        greenAwardPanel.SetActive(false);
        playButtonSound();
        NetworkScript.greenApples += award;
		NetworkScript.watched ++;
        FindObjectOfType<FruitSpearGameCode>().updateCoins();
        showMessage("+5 Green Apples");
    }
    public void openCurrencyPanel(){
        playButtonSound();
        currencyPanel.SetActive(true);
    }
    public void selectCurrency(string cur){

        if(!loadPanel.activeSelf){
            playButtonSound();
            currency = cur;
            tokenText.text = "+ 1";
            PlayerPrefs.SetString("language", cur);
            FindObjectOfType<NetworkScript>().curImage.texture = (Texture2D)Resources.Load(FruitUI.currency);
			curImage2.texture = (Texture2D)Resources.Load(FruitUI.currency);
			Strings.setLanguage();
            currencyPanel.SetActive(false);
			if(FindObjectOfType<TutorialScript>() != null)
				if(TutorialScript.loaded)
					FindObjectOfType<TutorialScript>().setStrings();

            if(isNew){
                openTutorial();
                isNew = false;
            }
        }
		
		resetStrings();
    }
	
	void resetStrings(){
		
		menutext1.text = Strings.play;
		menutext2.text = Strings.leaderboard;
		menutext3.text = Strings.exit;
		
	}

    public void openTutorial(){
		
		FindObjectOfType<FruitSpearGameCode>().hideBannerAd();
        tutorialPanel.SetActive(true);
		FindObjectOfType<TutorialScript>().setStrings();
		
    }
    public void closeTutorial(){
        tutorialPanel.SetActive(false);
		showMessageDialog(NetworkScript.message);
		FindObjectOfType<FruitSpearGameCode>().showBannerAd();
    }
    public void switchList(){
        closeLeaderboardGuidePanel();
        switchPanel.SetActive(true);
        leaderboardGuide = true;
		if(!NetworkScript.isCashLeaderboard)
			topCashButton.SetActive(false);
    }

	public void switchTopCashouts(){

        infoButton.SetActive(false);
        freeGreen.SetActive(false);
        leaderboardHeadText.text = "TOP EARNERS";
        thisLeaderboard = Leaderboard.Top_Cashouts;
        
        openLeaderboard();

    }
	
    public void switchAllTimeScores(){

        infoButton.SetActive(false);
        freeGreen.SetActive(false);
        leaderboardHeadText.text = "TOP HIGHSCORES";
        thisLeaderboard = Leaderboard.All_Time_Scores;
        
        openLeaderboard();

    }

    public void switchTodaysScores(){

        infoButton.SetActive(false);
        freeGreen.SetActive(false);
        leaderboardHeadText.text = "TODAYS HIGHSCORES";
        thisLeaderboard = Leaderboard.Todays_Scores;
        
        openLeaderboard();
    }

    public void switchTodaysApples(){

        if(FruitSpearGameCode.isGreen){

            infoButton.SetActive(true);
            freeGreen.SetActive(true);
            leaderboardHeadText.text = "TODAYS LEADERBOARD";
            thisLeaderboard = Leaderboard.Todays_Apples;
            
            openLeaderboard();
        }else
            showMessage("Green Apple Leaderboard is not Available");

    }

    public void showYesterdaysApples(){
        infoButton.SetActive(false);
        freeGreen.SetActive(false);
        leaderboardHeadText.text = "YESTERDAYS WINNERS";
        thisLeaderboard = Leaderboard.Yesterdays_Apples;
        openLeaderboard();

    }

    public void openSpinPanel(){
        playButtonSound();
        freeSpinDialog.SetActive(false);
        if(FindObjectOfType<NetworkScript>().CheckForInternetConnection(2))
            if(!userName.Equals("")){
                spinPanel.SetActive(true);
                FindObjectOfType<SpinScripct>().refreshCurrency();
            }else showMessage("You need to log in to use Wheel of Fortune.");
    }

     public void closeSpinPanel(){
         playButtonSound();
        spinPanel.SetActive(false);
    }

    public void playButtonSound(){
        FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Button);
    }
     public void checkForUpdates(){
         //Debug.Log("Checking for updates...");
         if(newVersion > version){
             Debug.Log("App Needs Update");
             PlayerPrefs.SetInt("version", newVersion);
             showUpdatePanel();
         }else{
             //Debug.Log("No Update needed");
              rulesText.text = "Gold - "+NetworkScript.gold+"   ( 1 Winner )  Silver - "+NetworkScript.silver+"   ( "+ListViewScript.silver+" Winners )  Bronze - "+NetworkScript.bronze+"   ( "+ListViewScript.bronze+" Winners )";
              rulesText2.text = NetworkScript.rules2;
                    if(!NetworkScript.message.Equals("") && !FruitSpearGameCode.isNew){
						
                        if(!PlayerPrefs.GetString("notif").Equals(NetworkScript.notif))
                        {
                            showMessageDialog(NetworkScript.message);
                            PlayerPrefs.SetString("notif",NetworkScript.notif);
                        }
                    }
         }

     }

    public void closeCashPanel(){
        playButtonSound();
        cashPanel.SetActive(false); 
		FindObjectOfType<NetworkScript>().checkSurveyPend();
    }
    public void showFunds(string message){
        cashPanel.SetActive(true);
        infoText.text = message;
        cashText.text = "+ "+NetworkScript.cashIncrease;
        Debug.Log(message);
    }

    public void showUpdatePanel(){
        updatePanel.SetActive(true);
    }

    public void actionButton(){
        
        if(FruitSpearGameCode.action == 1)
        {
            showBuyPanel();
        }else if(FruitSpearGameCode.action == 2){
            playButtonSound();
            getCoinReward();
        }else if(FruitSpearGameCode.action == 3){
            playButtonSound();
            FindObjectOfType<FruitSpearGameCode>().goToMarket();
        }

        FindObjectOfType<FruitSpearGameCode>().actionButton.SetActive(false);
    }

    public void openFreeSpinDialog(){
        freeSpinDialog.SetActive(true);
    }
    public void closeFreeSpinDialog(){
        playButtonSound();
        freeSpinDialog.SetActive(false);
    }
    public void getCoinReward()
    {
        playButtonSound();
        FruitSpearGameCode.rewardMode = FruitSpearGameCode.RewardMode.Coins;
        FindObjectOfType<FruitSpearGameCode>().showRewardAd();
    }
    public void getGreenAppleReward()
    {   
            playButtonSound();
			
			if(FindObjectOfType<FruitSpearGameCode>().isRewardAdReady()){
				
				//rewardTime = 30; 
				//greenRewardButton.SetActive(false);
				  
				FruitSpearGameCode.rewardMode = FruitSpearGameCode.RewardMode.Green;
				FindObjectOfType<FruitSpearGameCode>().showRewardAd();
				
			}else
				showMessage("Reward is not Ready");
            
                  

    }
    public void getExtraLife()
    {
        playButtonSound();
        extraLifePanel.SetActive(false);
        FruitSpearGameCode.rewardMode = FruitSpearGameCode.RewardMode.Revive;
        FindObjectOfType<FruitSpearGameCode>().showRewardAd();
    }
    public void skipStage(){
        skipStagePanel.SetActive(true);
    }
    public void closeSkipStagePanel(){
        skipStagePanel.SetActive(false);
    }
     public void openInfo(){
        playButtonSound();
		lbInfoText.text = Strings.infoText;
        infoPanel.SetActive(true);
    }
    public void closeInfo(){
        playButtonSound();
        infoPanel.SetActive(false);
        if(!leaderboardGuide)
            {
                leaderboardGuidePanel.SetActive(true);
                leaderboardGuide = true;
				PlayerPrefs.SetInt("leaderboardGuide",1);
            }
    }
    public void closeLeaderboardGuidePanel(){
        leaderboardGuidePanel.SetActive(false);
    }
    public void skipStageAd()
    {
        closeSkipStagePanel();
        playButtonSound();
        FruitSpearGameCode.rewardMode = FruitSpearGameCode.RewardMode.Skip;
        FindObjectOfType<FruitSpearGameCode>().showRewardAd();
    }
     public void skipStageCoin()
    {
        int amount = 100;

        playButtonSound();
        if(NetworkScript.coins >= amount){
            closeSkipStagePanel();
            NetworkScript.coins -= amount;
			paid = true;
            FindObjectOfType<FruitSpearGameCode>().updateCoins();
            FindObjectOfType<FruitSpearGameCode>().advance();
        }else{
            showMessage("You don't have up to "+amount+" apples");
        }
    }

    public void closeRefPanel(){
        playButtonSound();
        refPanel.SetActive(false);
    }
    public void openRefPanel(){
        playButtonSound();
		freeCashPanel.SetActive(false);
        refPanel.SetActive(true);
        refText.text = FruitUI.refID;
        refText1.text = Strings.invite1; //"Get cash for every 2 referrals !!!";
		inviteText.text = Strings.invite2;
        StartCoroutine(setupInvitesList());
    }
    public void openInvitePanel(){
        playButtonSound();
        invitePanel.SetActive(true);
    }
    public void closeInvitePanel(){
        playButtonSound();
        invitePanel.SetActive(false);
        if(!NetworkScript.bonused){
            FindObjectOfType<NetworkScript>().giveCash(300f, " for registeration bonus"); 
            NetworkScript.bonused = true;
        }
        
    }
	
	public void openExpiredPanel(){
        playButtonSound();
        expiredPanel.SetActive(true);
    }
	
    public void inviteFriend(){

            string appPackageName = Application.identifier;
            var shareSubject = "You gotta play this game!";
            var shareMessage = "Hey, So I just recently started playing this fun new game called Fruit Spear!" +
				   "\nAnd guess what? You can earn money 💸 daily just by playing the game for FREE!" +   
                   "\nYou should check it out. Use my code '"+refID+"' to get rewards."  +
                    "\n\n" +
                    "https://play.google.com/store/apps/details?id=" + appPackageName;

            NativeShare shareIntent = new NativeShare();
            shareIntent.SetSubject(shareSubject);
            shareIntent.SetText(shareMessage);
            shareIntent.SetTitle("Invite friends...");

            shareIntent.Share();
    }

    void closeDialog()
    {
        playButtonSound();
        messageDialog.SetActive(false);
    }
    public void showMessage(string message)
    {
#if UNITY_EDITOR
		Debug.Log(message);
#elif UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
		Debug.Log(message);
#endif
    }
     public void showMessageDialog(string message)
    {
        messageDialog.SetActive(true);
        messageText.text = message;
    }
    public void showratePanel2(){
        if(!hasRated)
            ratePanel2.SetActive(true);
    }
    public void closeratePanel2(){
        ratePanel2.SetActive(false);
    }
   
    void showBuyPanel(){
		playButtonSound();
        BuyPanel.SetActive(true);
    }
    public void closeBuyPanel(){
        playButtonSound();
        BuyPanel.SetActive(false);
    }
    public void updatePrem(){
        if(isPrem){
         txt1.text = "";
         txt2.text = "";
        }
    }
    public void closePremiumPanel()
    {
        playButtonSound();
        premiumPanel.SetActive(false);
    }
    void buyItem()
    {
        FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Button2);
        if (ItemScript.hasItem)
        {
            equippedItem = selectedItem;
            currentSpear = (Texture2D)Resources.Load("Spears/spear" + equippedItem);
            PlayerPrefs.SetInt("currentSpear", equippedItem);
            Debug.Log("Spear "+ equippedItem + " Equipped");
            selectedItemprice.text = "EQUIPPED";
            peachIcon.texture = (Texture2D)Resources.Load("blank");
        }
        else
        {
            if (NetworkScript.coins >= currentPrice)
            {
                equippedItem = selectedItem;
                currentSpear = (Texture2D)Resources.Load("Spears/spear" + equippedItem);
                PlayerPrefs.SetInt("spear" + equippedItem, 1);
                PlayerPrefs.SetInt("currentSpear", equippedItem);
                Debug.Log("Spear " + equippedItem + " Bought");
                NetworkScript.coins -= currentPrice;
                selectedItemprice.text = "EQUIPPED";
                FindObjectOfType<FruitUI>().selectedItemprice.text = "";
                FindObjectOfType<FruitUI>().buyText.text = "EQUIP";
                FindObjectOfType<FruitUI>().peachIcon.texture = (Texture2D)Resources.Load("blank");
                ItemScript.hasItem = true;
                peachIcon.texture = (Texture2D)Resources.Load("blank");
				paid = true;
                FindObjectOfType<FruitSpearGameCode>().updateCoins();
            }
            else
            {
                showMessage("You don't have up to "+currentPrice+" Apples");
            }
        }
        
    }

    public void giveFreeSpears(){
        
        List<int> available = new List<int>();
        for(int i=1; i<17; i++){
            if(PlayerPrefs.GetInt("spear" + i)!=1){
                available.Add(i);
                //Debug.Log("Spear "+i+" available");
            }
        }
        if(available.Count >=5 ){
            FruitSpearGameCode.running = false;
            FruitSpearGameCode.ready = false;
            spears.Clear();
            while(spears.Count < 3){
                int x = UnityEngine.Random.Range(1,17);
                if(available.Contains(x) && !spears.Contains(x)){
                    spears.Add(x); 
                    Debug.Log("Spear "+x+" added");  
                }
            }

            freeSpearDialog.SetActive(true);
            freeSpear1.texture = (Texture2D)Resources.Load("Spears/spear" + spears[0]);
            freeSpear2.texture = (Texture2D)Resources.Load("Spears/spear" + spears[1]);
            freeSpear3.texture = (Texture2D)Resources.Load("Spears/spear" + spears[2]);

        }else
            Debug.Log("Not up to 3 available spears");

    }

    public void selectSpear(int index){

                currentSpear = (Texture2D)Resources.Load("Spears/spear" + spears[index]);
                PlayerPrefs.SetInt("spear" + spears[index], 1);
                PlayerPrefs.SetInt("currentSpear", spears[index]);
                Debug.Log("Spear " + spears[index] + " Bought");
                freeSpearDialog.SetActive(false);
                FruitSpearGameCode.running = true;
                FruitSpearGameCode.ready = true;

    }

    private void shareButton(){
        playButtonSound();
        sharePanel.SetActive(true);
    }
    private void rateButton(){
        playButtonSound();
        ratePanel.SetActive(true);
    }

    public void rateBonusPressed()
    {
        ratePanel.SetActive(false);
        ratePanel2.SetActive(false);

        playButtonSound();
        if (!hasRated)
        {
            bonus = 1;
        }
        else
        {
            bonus = 11;
        }
         string appPackageName = Application.identifier;
         Application.OpenURL("https://play.google.com/store/apps/details?id=" + appPackageName);
           
    }

    public void shareBonusPressed()
    {
        sharePanel.SetActive(false);
        playButtonSound();
        if (!hasShared)
        {
            bonus = 2;
        }
        else
        {
           bonus = 22;
        }

            string appPackageName = Application.identifier;

            var shareSubject = "Yo, you have to play this game!";
            var shareMessage = "Hey, So I just recently started playing this fun new game called Fruit Spear!" +
                   "\nYou should check it out!" +
                    "\n\n" +
                    "https://play.google.com/store/apps/details?id=" + appPackageName;

            NativeShare shareIntent = new NativeShare();
            shareIntent.SetSubject(shareSubject);
            shareIntent.SetText(shareMessage);
            shareIntent.SetTitle("Share Fruit Spear with friends...");

            shareIntent.Share();
    
    }

    public void upgradeToPrem()
    {
        isPrem = true;
        PlayerPrefs.SetString(premiumDB, "true");
        Debug.Log("upgrading to premium");
        
        premChild.SetActive(false);
        FindObjectOfType<FruitSpearGameCode>().hideBannerAd();
		FindObjectOfType<NetworkScript>().updateToPrem();
        
    }

    public void upgradeToPremFailed()
    {
        Debug.Log("upgrade failed");
    }
    public void closeLeaderboardShareDialog(){
        shareDialog.SetActive(false);
    }
     public void openLeaderboardShareDialog(){
        shareDialog.SetActive(true);
    }
    void OnApplicationFocus(bool hasFocus)
    {
        //Debug.Log("Gained Focus");

        if(bonus == 1)
        {
            hasRated = true;
            PlayerPrefs.SetString("hasRated","true");
            Debug.Log("Bonus given for Rating");
            openAwardPanel(300);
        }
        if (bonus == 2)
        {
            hasShared = true;
            PlayerPrefs.SetString("hasShared", "true");
            Debug.Log("Bonus given for Sharing");
            openAwardPanel(300);
        }
		if(bonus == 3)
			showMessage("Withdrawal request sent successfully!");

        if(bonus == 11)
            showMessage("Already used Rate bonus");
        if(bonus == 22)
            showMessage("Already used Share bonus");

        bonus = 0;
		
		if(marketPanel.activeSelf){
			FindObjectOfType<FruitSpearGameCode>().setMarketIcons();
		}

    }
	
	public void scrollLeft(){
		scrollingLeft = true;
	}
	public void scrollRight(){
		scrollingRight = true;
	}

    void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("Paused");
    }

    public void setupMarket(int tier)
    {
        Debug.Log("Opening Tier " + tier);
        selectedItem = equippedItem;

        FindObjectOfType<FruitSpearGameCode>().resetMarket();

        Tier = tier;
        for (int i = 0; i < spearPrices.Length; i++)
        {	
			string grid = "GridView";
			if(i>=8)
				grid = "GridView2";
			if(i>=16)
				grid = "GridView3";
			
				Instantiate(gridViewChild.gameObject, GameObject.FindGameObjectWithTag(grid).transform);
        }

       // pagImg.texture = (Texture2D)Resources.Load("pag"+tier);
            

    }
 
    public void openLeaderboard()
    {
		//showMessage("Leaderboard Not Available");
		
        playButtonSound();
        switchPanel.SetActive(false);
        FindObjectOfType<FruitSpearGameCode>().menuPanel.SetActive(false);
        FruitSpearGameCode.gameType = FruitSpearGameCode.GameType.Normal;

        int curHour = System.DateTime.UtcNow.Hour;
		int curMin = System.DateTime.UtcNow.Minute;
		
		Debug.Log("UTC Timezone for Hour : "+curHour);

        int showHour = 24 - curHour;
		
		if(NetworkScript.curDay != System.DateTime.UtcNow.Day){
			timeText.text = "ENDED";
		}
		else if(showHour == 1){
			int showMin = 60 - curMin;
			timeText.text = showMin + " MINS LEFT";
		}
		else
			timeText.text = showHour + " HRS LEFT";

		if(thisLeaderboard == Leaderboard.Todays_Apples){
			infoButton.SetActive(true);
			if(!infoed){
				openInfo();
				infoed = true;
			}
        }else
			infoButton.SetActive(false);

        leaderboardPanel.SetActive(true);
        leaderboardPanel2.SetActive(true);
        StartCoroutine(setupLeaderboard());
		
		
    }

    IEnumerator setupLeaderboard(){

        if(FindObjectOfType<NetworkScript>().CheckForInternetConnection(1)){
            FindObjectOfType<FruitSpearGameCode>().resetLeaderboard(); 
            FindObjectOfType<NetworkScript>().retreiveLeaderboardData(null, true);
            yield return new WaitForSeconds(1f);
        }   
    }

    IEnumerator setupInvitesList(){

        if(FindObjectOfType<NetworkScript>().CheckForInternetConnection(1)){
            FindObjectOfType<FruitSpearGameCode>().resetInviteList(); 
            FindObjectOfType<NetworkScript>().retreiveInvites();
            yield return new WaitForSeconds(1f);
        }   
    }

    IEnumerator setupTransactionList(){

        if(FindObjectOfType<NetworkScript>().CheckForInternetConnection(1)){
            FindObjectOfType<FruitSpearGameCode>().resetLeaderboard(); 
            FindObjectOfType<FruitSpearGameCode>().resetList(); 
            FindObjectOfType<NetworkScript>().retreiveTransactionHistory();
            yield return new WaitForSeconds(1f);
        }   
    }

    IEnumerator setupTournamentList(){

        if(FindObjectOfType<NetworkScript>().CheckForInternetConnection(1)){
            FindObjectOfType<FruitSpearGameCode>().resetLeaderboard(); 
            FindObjectOfType<FruitSpearGameCode>().resetList();
            FindObjectOfType<NetworkScript>().retreiveTournaments();
            yield return new WaitForSeconds(1f);
        }   
    }

     IEnumerator setupTournamentLeaderboard(){

        if(FindObjectOfType<NetworkScript>().CheckForInternetConnection(1)){
            FindObjectOfType<FruitSpearGameCode>().resetLeaderboard(); 
            FindObjectOfType<FruitSpearGameCode>().resetList(); 
            FindObjectOfType<NetworkScript>().retreiveLeaderboardData(selectedTournament.id, true);
            yield return new WaitForSeconds(1f);
        }   
    }

     public void openTransactionList()
    {
        playButtonSound();
        listPanel.SetActive(true);
        listHead.text = "Transactions";
		listHeadb.text = "Transactions";
        listHead2.text = "";
		
        StartCoroutine(setupTransactionList());
    }

     public void openTournamentList()
    {
        playButtonSound();
		
        if(!FruitUI.userName.Equals("")){
            listPanel.SetActive(true);
            listHead.text = "Tournaments";
			listHeadb.text = "Tournaments";
            listHead2.text = "";
            
            StartCoroutine(setupTournamentList());
        }else
            showMessage("You need to log in to view tournaments");
    }

    public void openJoinPanel(){

        playButtonSound();

        if(selectedTournament.used){

                 switch(selectedTournament.type){
                   case "top":
                        if(selectedTournament.hasEnded)
                            showMessage("This Tournament has ended");
                        FruitSpearGameCode.gameType = FruitSpearGameCode.GameType.Top;
                        listPanel.SetActive(true);
                        listHead.text = selectedTournament.name;
						listHeadb.text = selectedTournament.name;
                        StartCoroutine(setupTournamentLeaderboard());
                    break;
                    case "target":
                        showMessage("You have reached the maximum attempt for this challenge");
                    break;
                    case "time":
                        showMessage("You have reached the maximum attempt for this challenge");
                    break;
                }

        }else if(selectedTournament.ready){

            if(selectedTournament.hasEnded){
                if(selectedTournament.type.Equals("bid")){
					FindObjectOfType<NetworkScript>().UnscribeNotif(selectedTournament.id);
                    selectedTournament.showWinner();
                }else
                    showMessage("This Tournament has ended");
            }else{

                if(selectedTournament.type.Equals("bid")){

                    bidPanel.SetActive(true);
                    bidName.text = selectedTournament.name;
                    bidGoal.text = Strings.bidText;
					bidGoal2.text = Strings.bidText2;
                    bid.text = ""+selectedTournament.plays;
                    bidPlaceHolder.text = "(Min. "+selectedTournament.fee+" Apples)";
                    bidPrize.text = ""+selectedTournament.prize;
                    bidTimeLeft.text = selectedTournament.getTime();
                    
                }else{

                    joinPanel.SetActive(true);
                    joinName.text = "Play '"+selectedTournament.name+"' ?";
                    joinGoal.text = "Rules: " + selectedTournament.goal;
                    joinFee.text = ""+selectedTournament.fee;
                    joinPrize.text = ""+selectedTournament.prize;
                    
                    int left = 3 - selectedTournament.plays;
                    showMessage("You have "+left+" attempts remaining");
                
                }
            }
            
        }
    }

    public void placeBid(){
        bidAmount = 0;
        try{
            bidAmount = Convert.ToInt32(newBid.text.Trim());

            if(bidAmount < selectedTournament.fee){
                showMessage("Minimum bid is "+selectedTournament.fee+" apples");
            }else{
                
                    if(NetworkScript.coins >= bidAmount){

                        if( FindObjectOfType<NetworkScript>().CheckForInternetConnection(2)){            

                            NetworkScript.coins -= bidAmount;
							paid = true;
                            FindObjectOfType<FruitSpearGameCode>().updateCoins();
                            FruitSpearGameCode.gameType = FruitSpearGameCode.GameType.Bid;
                            selectedTournament.plays += bidAmount;
                            this.bid.text = ""+selectedTournament.plays;
                            FindObjectOfType<NetworkScript>().joinTournament(selectedTournament.id);
                            newBid.text = "";

                            showMessage("Your Total Bid is " + selectedTournament.plays + " Apples");
                        }
                    }else{
                        showMessage("You dont have enough apples");
                        //bidPanel.SetActive(false);
                        //showBuyPanel();
                    }
            }

        }catch(Exception e){
            showMessage("Enter a valid Number");
        }

    }

    public void showRaffleWinner(string name, string[] userIDs, float[] prizes){
		
		Debug.Log("Preparing Raffle Winners UI");
		
		NetworkScript.winner1Name = "";
		NetworkScript.winner1Photo = "";
		NetworkScript.winner2Name = "";
		NetworkScript.winner2Photo = "";
		NetworkScript.winner3Name = "";
		NetworkScript.winner3Photo = "";
		
		winner1Photo.texture = (Texture2D)Resources.Load("avatar");
		winner2Photo.texture = (Texture2D)Resources.Load("blank");
		winner3Photo.texture = (Texture2D)Resources.Load("blank");
						
		winner1Name.text = "";
		winner2Name.text = "";
		winner3Name.text = "";
		
		winner1Prize.text = "";
		winner2Prize.text = "";
		winner3Prize.text = "";
		
		photo1Set = false;
		photo2Set = false;
		photo3Set = false;
		
		winner2Panel.SetActive(false);
		winner3Panel.SetActive(false);
		
		winnerTitle.text = "Winner Of " + name;
		winner1Prize.text = prizes[0]+"";
		StartCoroutine(FindObjectOfType<NetworkScript>().SetWinner(userIDs[0], 1));
		
		if(userIDs.Length > 1){
			winner2Panel.SetActive(true);
			winnerTitle.text = "Winners Of " + name;
			winner2Prize.text = prizes[1]+"";
			winner2Photo.texture = (Texture2D)Resources.Load("avatar");
			StartCoroutine(FindObjectOfType<NetworkScript>().SetWinner(userIDs[1], 2));
		}
		if(userIDs.Length > 2){
			winner3Panel.SetActive(true);
			winner3Prize.text = prizes[2]+"";
			winner3Photo.texture = (Texture2D)Resources.Load("avatar");
			StartCoroutine(FindObjectOfType<NetworkScript>().SetWinner(userIDs[2], 3));
		}
		
        winnerPanel.SetActive(true);
    }
	
	private void checkWinnerUI(){
		
		Debug.Log("Check Winner 1: "+NetworkScript.winner1Name);
		Debug.Log("Check Winner 2: "+NetworkScript.winner2Name);
		Debug.Log("Check Winner 3: "+NetworkScript.winner3Name);
		
		if(!NetworkScript.winner1Name.Trim().Equals(""))
			winner1Name.text = NetworkScript.winner1Name;
		
		if(!NetworkScript.winner1Photo.Trim().Equals("") && !photo1Set){
			StartCoroutine(FindObjectOfType<NetworkScript>().SetImage(NetworkScript.winner1Photo+"?type=normal", winner1Photo, false));
			photo1Set = true;
		}
		if(!NetworkScript.winner2Name.Trim().Equals(""))
			winner2Name.text = NetworkScript.winner2Name;
		
		if(!NetworkScript.winner2Photo.Trim().Equals("") && !photo2Set){
			StartCoroutine(FindObjectOfType<NetworkScript>().SetImage(NetworkScript.winner2Photo+"?type=normal", winner2Photo, false));
			photo2Set = true;
		}
		
		if(!NetworkScript.winner3Name.Trim().Equals(""))
			winner3Name.text = NetworkScript.winner3Name;
		
		if(!NetworkScript.winner3Photo.Trim().Equals("") && !photo3Set){
			StartCoroutine(FindObjectOfType<NetworkScript>().SetImage(NetworkScript.winner3Photo+"?type=normal", winner3Photo, false));
			photo3Set = true;
		}
		  //
		//StartCoroutine(FindObjectOfType<NetworkScript>().SetImage(photoUrl, winner2Photo, false));
		//StartCoroutine(FindObjectOfType<NetworkScript>().SetImage(photoUrl, winner3Photo, false));
	}

    public void closeRaffleWinner(){
        winnerPanel.SetActive(false);
    }

    public void closeBidPanel(){
        bidPanel.SetActive(false);
    }

    public void joinTournament(){

        if( FindObjectOfType<NetworkScript>().CheckForInternetConnection(2))
            if(NetworkScript.coins >= selectedTournament.fee){

                NetworkScript.coins -= selectedTournament.fee;
				paid = true;
                FindObjectOfType<FruitSpearGameCode>().updateCoins();

                loadPanel.SetActive(true);

                switch(selectedTournament.type){
                    case "top":
                        
                             FruitSpearGameCode.gameType = FruitSpearGameCode.GameType.Top;
                             FruitSpearGameCode.luckInterval = 0;
                             FindObjectOfType<NetworkScript>().retreiveLeaderboardData(selectedTournament.id, false);
                       
                    break;
                    case "target":
                        
                            FruitSpearGameCode.gameType = FruitSpearGameCode.GameType.Target;
                            startGame = true;
                        
                    break;
                    case "time":
                        
                            FruitSpearGameCode.gameType = FruitSpearGameCode.GameType.Time;
                            startGame = true;
                       
                    break;
                }
				
				FindObjectOfType<NetworkScript>().joinTournament(selectedTournament.id);

                Debug.Log("Starting Tournament "+selectedTournament.name);
                Debug.Log("Game Type: "+FruitSpearGameCode.gameType.ToString());

        }else{
            showMessage("You dont have enough apples to enter this tournament");
            joinPanel.SetActive(false);
            showBuyPanel();
        }

    }

    public void showResults(){
        resultPanel.SetActive(true);
        resIcon.texture  = selectedTournament.icon;
        resTournamentName.text = selectedTournament.name;

         switch(selectedTournament.type){
                case "top":
                    resScore.text = "Your Score: "+FruitSpearGameCode.score;

                    if(NetworkScript.users.Count > 0){
                        resGoal.text = "Top Score: "+NetworkScript.users[0].score;

                        if(FruitSpearGameCode.score >= NetworkScript.users[0].score){
                            resResult.color = Color.green;
                            resResult.text = "Congrats!, Your'e No 1";
                        }
                        else{   
                            resResult.color = Color.black;  
                            resResult.text = "";
                        }
                    }else{
                        resResult.color = Color.green;
                        resGoal.text = "Top Score: None";
                        resResult.text = "Congrats!, Your'e No 1";
                    }
                break;
                case "target":
                    int targets = selectedTournament.amount - FruitSpearGameCode.targets;
                    resScore.text = "Targets Hit - " + targets;
                    resGoal.text = "Remaining Targets - " + FruitSpearGameCode.targets;
                    if(FruitSpearGameCode.targets <= 0){
                        resResult.color = Color.green;
                        resResult.text = "You Won!";
                        FindObjectOfType<NetworkScript>().giveCash(selectedTournament.prize, " for Tournament '"+selectedTournament.name+"'");
                        FindObjectOfType<NetworkScript>().uploadScore(selectedTournament.id, "won");
                    }else{
                        resResult.color = Color.red;     
                        resResult.text = "You Missed...";
                        FindObjectOfType<NetworkScript>().uploadScore(selectedTournament.id, "lost");
                    }
                break;
                case "time":
                    int shots = selectedTournament.shots - FruitSpearGameCode.spearCount;
                    resScore.text = "Successful Shots - " + shots;
                    resGoal.text = "Remaining Spears - " + FruitSpearGameCode.spearCount;

                    if(FruitSpearGameCode.spearCount < 1){
                        resResult.color = Color.green;
                        resResult.text = "You Won!";
                        FindObjectOfType<NetworkScript>().giveCash(selectedTournament.prize, " for Tournament '"+selectedTournament.name+"'");
                        FindObjectOfType<NetworkScript>().uploadScore(selectedTournament.id, "won");
                    }
                    else if(FruitSpearGameCode.spearCount < 3){
                       resResult.color = Color.red;  
                       resResult.text = "You Were so Close...";
                       FindObjectOfType<NetworkScript>().uploadScore(selectedTournament.id, "lost");
                    }
                    else{
                       resResult.color = Color.red;     
                       resResult.text = "You didn't finish";
                       FindObjectOfType<NetworkScript>().uploadScore(selectedTournament.id, "lost");
                    }



                break;
         }

    }


    public void closeResult(){

        playButtonSound();
        resultPanel.SetActive(false);
        if(FruitSpearGameCode.gameType == FruitSpearGameCode.GameType.Top){
            listPanel.SetActive(true);
            listHead.text = selectedTournament.name;
			listHeadb.text = selectedTournament.name;
            StartCoroutine(setupTournamentLeaderboard());
        }else{
            FindObjectOfType<FruitSpearGameCode>().showTournamentInterstitial();
            FindObjectOfType<FruitSpearGameCode>().goToMenu();      
        }
    
    }

    public void closeJoinPanel(){
        playButtonSound();
        joinPanel.SetActive(false);
    }

     public void closeList()
    {
        playButtonSound();
        listPanel.SetActive(false);

		if(!FindObjectOfType<FruitSpearGameCode>().menuPanel.activeSelf)
            FindObjectOfType<FruitSpearGameCode>().showTournamentInterstitial();

        FindObjectOfType<FruitSpearGameCode>().goToMenu();
    }

    public void setLeaderBoardData()
    {
        
      listView.GetComponent<VerticalLayoutGroup>().padding.left = (int)(Screen.width / 2f);
   
        if (NetworkScript.leaderboardReady && leaderboardPanel.activeSelf && FindObjectOfType<FruitSpearGameCode>().leaderboardItems.Count == 0)
        {
            NetworkScript.leaderboardReady = false;
            Debug.Log("Setting Leaderboard data");
           
            NetworkScript.users =  NetworkScript.users.OrderByDescending(i => i.score).ToList(); 
			
			if(thisLeaderboard == Leaderboard.Top_Cashouts)
				 NetworkScript.users =  NetworkScript.users.Take(25).ToList();
			 
			if(thisLeaderboard != Leaderboard.Todays_Apples)
				timeText.text = "TOP "+ NetworkScript.users.Count;
        
			 
			 
            for (int i = 0; i < NetworkScript.users.Count; i++)
            {
                StartCoroutine(displayChild());
            }
            
            Debug.Log(NetworkScript.users.Count + " Names set on Leaderboard");
        }
    }

    IEnumerator displayChild(){
        Instantiate(listViewChild.gameObject, GameObject.FindGameObjectWithTag("ListView").transform);
        yield return null;
    }
    
    public void setInvitesData()
    {
        
      if (NetworkScript.inviteReady && refPanel.activeSelf && FindObjectOfType<FruitSpearGameCode>().inviteItems.Count == 0)
        {
            NetworkScript.inviteReady = false;
            Debug.Log("Setting Invites data");
           
            for (int i = 0; i < NetworkScript.invites.Count; i++)
            {
                StartCoroutine(displayInvites());
            }
            
            Debug.Log(NetworkScript.invites.Count + " Invites on the list");
        }
    }

    IEnumerator displayInvites(){
        Instantiate(refChild.gameObject, GameObject.FindGameObjectWithTag("InvitesView").transform);
        yield return null;
    }

    public static string getCurrency(float amount){

        float newAmount = amount;
        string cur = "$";
        
        string result = cur + Math.Round(newAmount,2);

        return result;

    }

    public void setTransactionsData()
    {
        
      if (NetworkScript.transactionReady && listPanel.activeSelf && FindObjectOfType<FruitSpearGameCode>().transactionItems.Count == 0)
        {
            NetworkScript.transactionReady = false;
            Debug.Log("Setting transactions data");
           
            for (int i = 0; i < NetworkScript.transactions.Count; i++)
            {
                Instantiate(transactionChild.gameObject, GameObject.FindGameObjectWithTag("ListView2").transform);
            }
            
            Debug.Log(NetworkScript.transactions.Count + " transactions on the list");
        }
    }

    public void setTournamentData()
    {
        
      if (NetworkScript.tournamentReady && listPanel.activeSelf && FindObjectOfType<FruitSpearGameCode>().listItems.Count == 0)
        {
            NetworkScript.tournamentReady = false;
            Debug.Log("Setting Tournament data");

            NetworkScript.tournaments =  NetworkScript.tournaments.OrderBy(i => i.id).ToList();
           
            for (int i = 0; i < NetworkScript.tournaments.Count; i++)
            {
                Instantiate(tournamentChild.gameObject, GameObject.FindGameObjectWithTag("ListView2").transform);
            }
            
            Debug.Log(NetworkScript.tournaments.Count + " tournaments on the list");
        }
    }

     public void setTournamentLeaderboardData()
    {
        
      if (NetworkScript.tournLeaderboardReady && FindObjectOfType<FruitSpearGameCode>().leaderboardItems.Count == 0)
        {
            NetworkScript.tournLeaderboardReady = false;
            
            Debug.Log("Setting Tournament Leaderboard data");
            NetworkScript.users =  NetworkScript.users.OrderByDescending(i => i.score).ToList();
            
            int rem = selectedTournament.max - NetworkScript.users.Count;
            listHead2.text = "("+rem+" players remaining)";
            
            for (int i = 0; i < NetworkScript.users.Count; i++)
            {
                StartCoroutine(displayTournChild());
            }
            
           Debug.Log(NetworkScript.users.Count + " players in the tournament");
        }
    }

    IEnumerator displayTournChild(){
        Instantiate(tournamentLeaderboardChild.gameObject, GameObject.FindGameObjectWithTag("ListView2").transform);
        yield return null;
    }

    public void closeLeaderboard()
    {
        playButtonSound();
        userNamePanel.SetActive(false);
        infoButton.SetActive(true);
        freeGreen.SetActive(true);

        if(FruitSpearGameCode.isGreen){
            leaderboardHeadText.text = "TODAYS LEADERBOARD";
            thisLeaderboard = Leaderboard.Todays_Apples;
        }
       
        if(leaderboardPanel.activeSelf){
            FindObjectOfType<FruitSpearGameCode>().menuPanel.SetActive(true);   
            leaderboardPanel.SetActive(false);
            leaderboardPanel2.SetActive(false);
       }
    }

    private void checkRefresh(){
        if(NetworkScript.refreshList){
                NetworkScript.refreshList = false;
                if(leaderboardPanel.activeSelf)
                    openLeaderboard();
        }
    }

    void Update()
    {
        if(Time.time - lastUpdate >= 1.5f)
        {
            rewardTime--;
			if(rewardTime <= 0)
				greenRewardButton.SetActive(true);

            if(startGame){
                startGame = false;
                loadPanel.SetActive(false);
                closeList();
                closeJoinPanel();
                FindObjectOfType<FruitSpearGameCode>().startGame();
            }
            setLeaderBoardData();
            setInvitesData();
            setTransactionsData();
            setTournamentData();
            setTournamentLeaderboardData();
            checkForUpdates();
            checkRefresh();
			
			if(winnerPanel.activeSelf)
				checkWinnerUI();
			
			FindObjectOfType<NetworkScript>().finalizeLogin();
			FindObjectOfType<NetworkScript>().checkLoad();
			FindObjectOfType<NetworkScript>().checkMessage();
			FindObjectOfType<FruitSpearGameCode>().checkBanner();
            cashTextMenu.text = ""+NetworkScript.myCash;
            cashTextGame.text = ""+NetworkScript.myCash;
            FindObjectOfType<NetworkScript>().cashText.text = ""+NetworkScript.myCash;
            if(userName.Equals(""))
                profileName.text = "Guest";
            else
                profileName.text = userName;

			PlayerPrefs.SetInt("gamesPlayed",FruitSpearGameCode.gamesPlayed);
            lastUpdate = Time.time;
        }

        if(leaderboardPanel.activeSelf)
        {
            RectTransform rectTransform = leaderboard.GetComponent<RectTransform>();
            float newTop = 0;
            float newBottom = 0;
            if(Screen.height >= 1280f){
                newTop = ((Screen.height - 1280f) / 2) + 190;
                newBottom = ((Screen.height - 1280f) / 2) + 105;
            }else{
                newTop = ((Screen.height - 800f) / 2) + 264;
                newBottom = ((Screen.height - 800f) / 2) + 100;
            }
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, newBottom);
            rectTransform.offsetMax = new Vector2(newBottom, -newTop);
            
            RectTransform rectTransform2 = leaderboardPanel.GetComponent<RectTransform>();
                
            if(Screen.width < 700f){

                float newTop2 = ((Screen.height - 1280f) / 2) - 42;
                float newBottom2 = ((Screen.height - 1280f) / 2) + 52;
                float newWidth = ((Screen.width - 720f) / 2) + 30f;
                rectTransform2.offsetMin = new Vector2(newWidth, newBottom2);
                rectTransform2.offsetMax = new Vector2(-newWidth, -newTop2);

            }else{
                
                float newTop2 = -80;
                float newBottom2 = 90;
                rectTransform2.offsetMin = new Vector2(rectTransform2.offsetMin.x, newBottom2);
                rectTransform2.offsetMax = new Vector2(rectTransform2.offsetMax.x, -newTop2);
            }
        }
		if(spearView.activeSelf)
        {
			if(scrollingLeft){
				if(spearView.GetComponent<ScrollRect>().horizontalNormalizedPosition > 0.05f)
					spearView.GetComponent<ScrollRect>().horizontalNormalizedPosition -= 0.05f;
				else if(spearView.GetComponent<ScrollRect>().horizontalNormalizedPosition > 0)
					spearView.GetComponent<ScrollRect>().horizontalNormalizedPosition -= 0.01f;
				else
					scrollingLeft = false;
			}
			
			if(scrollingRight){
				if(spearView.GetComponent<ScrollRect>().horizontalNormalizedPosition < 0.95f)
					spearView.GetComponent<ScrollRect>().horizontalNormalizedPosition += 0.05f;
				else if(spearView.GetComponent<ScrollRect>().horizontalNormalizedPosition < 1f)
					spearView.GetComponent<ScrollRect>().horizontalNormalizedPosition += 0.01f;
				else
					scrollingRight = false;
			}
		}
    }
    
}
