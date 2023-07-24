using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine.Serialization;
using System;
using PollfishUnity;

public class FruitSpearGameCode : MonoBehaviour, IStoreListener
{
    public GameObject fruitRB, fruitRBFront, splash, bossFightImage, peachHitImage, peachCut;
    public static Vector3 center;
    public float angle, angleMax, angleMin, angleInterval;
    public GameObject spear, peach, greenApple, moneyToken, spearObject, defPeach, defSpear;
    public GameObject gamePanel, menuPanel, gameOverPanel, UIPanel, RewardPanel, noSave, skipButton, actionButton;
    public static int stage = 1, fruitStage, score, todaysScore, topScore, highScore, highStage, spearCount = 5, peachCount, spearObjectCount, luckInterval = -1;
    public static float spearY, lastUpdate, lastUpdate2, spawnUpdate, burstUpdate, adCounter = -1f;
    public GameObject spearIco, peachCoins, greenCoins, fruitBreak, shade2, shade3, shade4, shade5, shade6, shade7, shade8, tick1, tick2, tick3, tick4, tick5, tick6, tick7, tick8;
    public List<GameObject> spearIcons = new List<GameObject>();
    public List<GameObject> leaderboardItems = new List<GameObject>();
    public List<GameObject> inviteItems = new List<GameObject>();
    public List<GameObject> transactionItems = new List<GameObject>();
    public List<GameObject> spearItems = new List<GameObject>();
    public List<GameObject> listItems = new List<GameObject>();
    public static List<GameObject> spears = new List<GameObject>();
    public static List<GameObject> peaches = new List<GameObject>();
    public static List<GameObject> spearObjects = new List<GameObject>();
    public static bool running, isAd, saved, bossFight, getVIPGreen, slowDown, spawnedObjects, turn, won, ready, giveFruits, isNew = true, isGreen, pollfishShowing;
    public Text scoreText, stageText, scoreText2, stageText2, coinsText, greenApplesText, statsText, counterText, day1Text, day1Amount;
    public Button single, multi, leaderboard, exit, restrt, share, home, market, back, rate;
    private const int stageMax = 16;
    private Texture2D blank;
    private Texture2D[] fruits = new Texture2D[stageMax];
    private Texture2D[] fruitBosses = new Texture2D[stageMax];
    private Color[] splashColor = new Color[stageMax];
    public RawImage fruitImage, splashImage, fruit1, fruit2, fruit3, fruit4, musicImage, soundImage, actionImage;
    public static int action, gamesPlayed, targets;
    private Vector3 peachPosition, peachPosition2, spearPosition;
    public static int adInterval = 0, games, appleReward = 25, time, counter=6, adCount=3, botStrike;
    public AudioSource audio, audio2, music;
    public AudioClip spearClip, fruitClip, smallfruitClip, buttonClip, buttonClip2, selectClip, clashClip, coinsClip, bossClip, spinClip, spinEndClip, m1,m2,m3,m4;
    private bool reward, isMarket, isRestart, isSound=true, isMusic=true, adLoaded, fbInterstitialLoaded, fbRewardLoaded, fbBannerShowing, didClose, didClose2,
    tutorial, bannerLoaded, givenFreeSpear, bannerShowing, facebook = true, isReset = true, offerChecked;
    public static GameType gameType = GameType.Normal;
	private int rewardAmount, rewardType, botSuspicion, noAd = 0;
	private IStoreController m_StoreController;
	IGooglePlayStoreExtensions m_GooglePlayStoreExtensions;
	public string product_noads = "no_ads";
    public string premium_monthly = "premium_monthly";
	public string product_1 = "purchase1";
	public string product_2 = "purchase2";
	public string product_3 = "purchase3";
	public string product_4 = "purchase4";
	
     public enum Sound
    {
        Hit,
        Game_Over,
        Fruit_Burst,
        Small_Fruit_Burst,
        Clash,
        Spin,
        SpinEnd,
        Coins,
        Button,
        Button2,
        Select,
        Boss
    }
    
 static string rewardAdID = "9b7bf5225720c113";
 static string interstitialAdID = "182de05da942f2f6";
 static string bannerAdID = "e53e0e63f8198fef";
 int retryAttempt, retryAttempt2;


    private string[] bossNames = {"WaterMelon",
                        "Lime",
                        "Guava",
                        "Coconut",
                        "Orange",
                        "Tomato",
                        "Apple",
                        "Kiwi",
                        "Chiko",
                        "Peach",
						"Lemon",
						"Strawberry",
						"Soursop",
						"Pineapple",
						"Pear",
						"Pomegranate",
						
						};
    public static RewardMode rewardMode;

    public enum RewardMode
    {
        Coins,
        Green,
        Revive,
        Skip,
        Spin
    }

     public enum GameType
    {
        Bid,
        Normal,
        Target,
        Time,
        Top
    }

    void Start()
    {
        for (int i = 0; i < spearCount; i++)
        {
            Instantiate(spearIco.gameObject, GameObject.FindGameObjectWithTag("GamePanel").transform);
        }

        running = false;
        single.onClick.AddListener(() => singlePlayer());
        //multi.onClick.AddListener(() => restart());
        exit.onClick.AddListener(() => exitGame());
        restrt.onClick.AddListener(() => restartListener());
        share.onClick.AddListener(() => shareScore());
        rate.onClick.AddListener(() => rateApp());
        home.onClick.AddListener(() => goToMenu());
        back.onClick.AddListener(() => closeMarket());
        market.onClick.AddListener(() => goToMarket());
        leaderboard.onClick.AddListener(() => FindObjectOfType<FruitUI>().openLeaderboard());

        gamePanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        gameOverPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        UIPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        menuPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        RewardPanel.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
       
        peachCoins.transform.localScale = new Vector3(Screen.height / 3450f, Screen.height / 3450f, 0);
        peachCoins.transform.position = new Vector3(Screen.width / 1.2f, Screen.height / 1.055f, 0);
        greenCoins.transform.localScale = new Vector3(Screen.height / 3450f, Screen.height / 3450f, 0);
        greenCoins.transform.position = new Vector3(Screen.width / 1.2f, Screen.height / 1.1f, 0);

        for (int i = 0; i < fruits.Length; i++)
        {
            int index = i + 1;
            fruits[i] = (Texture2D)Resources.Load("Fruits/fruit" + index);
            fruitBosses[i] = (Texture2D)Resources.Load("Fruits/fruitboss" + index);
        }


        blank = (Texture2D)Resources.Load("blank");

        splashColor[0] = Color.red;
        splashColor[1] = Color.green;
        splashColor[2] = Color.yellow;
        splashColor[3] = Color.white;
        splashColor[4] = Color.yellow;
        splashColor[5] = Color.red;
        splashColor[6] = Color.white;
        splashColor[7] = Color.green;
        splashColor[8] = Color.yellow;
        splashColor[9] = Color.red;
		splashColor[10] = Color.yellow;
        splashColor[11] = Color.red;
        splashColor[12] = Color.white;
        splashColor[13] = Color.yellow;
		splashColor[14] = Color.green;
        splashColor[15] = Color.red;
		
        splashImage.color = splashColor[0];

        spearY = Screen.height / 5f;
        resetAngles();

        center = fruitRB.transform.position;

        stageText.text = "";
        scoreText.text = "";

        peachPosition = defPeach.transform.position;
        spearPosition = defSpear.transform.position;

        menuPanel.transform.position = gamePanel.transform.position;

        fruitRB.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        fruitRBFront.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        fruitStage = 0;

        peachPosition2 = defPeach.transform.position;

        //Debug.Log("Y1:" + peachPosition.y + " Y2:" + peachPosition2.y);
        Destroy(defPeach);

        //Debug.Log("Spear-Y:" + spearPosition.y);
        Destroy(defSpear);
		
        Debug.Log("Country: "+System.Globalization.RegionInfo.CurrentRegion);

        string newPlayer = PlayerPrefs.GetString("Player");
		gamesPlayed = PlayerPrefs.GetInt("gamesPlayed");
		Debug.Log("Games Played Local: "+gamesPlayed);

        music.volume = 0.6f;
		int a = UnityEngine.Random.Range(0,4);
		
		if(a == 0)
			music.clip = m1;
		if(a == 1)
			music.clip = m2;
		if(a == 2)
			music.clip = m3;
		if(a == 3)
			music.clip = m4;
		
        
        highScore = PlayerPrefs.GetInt("highscore");
        highStage = PlayerPrefs.GetInt("highstage");
        statsText.text = "BEST -  STAGE " + highStage + " : SCORE " + highScore;

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) ||
            !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
#endif
		 if(newPlayer == null || newPlayer.Equals(""))
        {
            Debug.Log("New Player");
            PlayerPrefs.SetString("Player", "Granted");
            
			takeScreenshot("fruitspear_highscore.png");
            takeScreenshot("fruitspear_leaderboard.png");
            isNew = true;
           // FindObjectOfType<FruitUI>().tutorialPanel.SetActive(true);

        }else{
            isNew = false;
            Debug.Log("Old Player");
        }

        StartCoroutine(checkInternet());
    }

    IEnumerator checkInternet(){
        yield return new WaitForSeconds(3);

        FindObjectOfType<NetworkScript>().CheckForInternetConnection(1);

        if(PlayerPrefs.GetString("muzic").Equals("off")){
            music.Pause();
            musicImage.texture = (Texture2D)Resources.Load("music_off");
        }else{
            music.Play();
            musicImage.texture = (Texture2D)Resources.Load("music_on");
        }
        music.volume = 0.6f;
    }

    public void setupAds(){

        if(!adLoaded){

			Debug.Log("Setting up Ads");
			
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
				Debug.Log("Ads Ready to use");
				loadInterstitialAd();
				loadBannerAd();
				loadRewardAd();	
				
				Debug.Log("Finished Setting up Ads");
			};

			MaxSdk.SetSdkKey("wxCDcV0h4R9OzmD-WdtyS0dpa-ys6ud3txP9eOIXqeXebE94PcR-vbIf2GUXsQQONTb8AysQ9GvmblSB94uwFK");
			MaxSdk.InitializeSdk();
			
            adLoaded = true;
			
        }
		
		InitializePurchasing();
    }
	
	public void initPollfish(){
		
		Debug.Log("Setting up Pollfish");
#if UNITY_EDITOR
		Debug.Log("Pollfish Does Not work in Editor");
#else
		Pollfish.Params pollfishParams = new Pollfish.Params("d6416dd6-b256-496a-a884-7f1296017966")
			  .OfferwallMode(true)
			  .ReleaseMode(true)
			  .RewardMode(true)
			  .RequestUUID(FruitUI.userID);
  
			Pollfish.Init(pollfishParams);
#endif

	}
	
	void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			
			builder.Configure<IGooglePlayConfiguration>().SetDeferredPurchaseListener(OnDeferredPurchase);

            builder.AddProduct(product_noads, ProductType.Consumable);
            builder.AddProduct(premium_monthly, ProductType.Subscription);
			builder.AddProduct(product_1, ProductType.Consumable);
			builder.AddProduct(product_2, ProductType.Consumable);
			builder.AddProduct(product_3, ProductType.Consumable);
			builder.AddProduct(product_4, ProductType.Consumable);

            UnityPurchasing.Initialize(this, builder);
        }
	
	void OnDeferredPurchase(Product product)
        {
            Debug.Log($"Purchase of {product.definition.id} is deferred");
        }
		
	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
		m_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();

        var monthlySubscriptionProduct = m_StoreController.products.WithID(premium_monthly);
        try
        {
            var isSubscribed = IsSubscribedTo(monthlySubscriptionProduct);
            if(isSubscribed){
                Debug.Log("Subscribed to Premium Monthly");
                FindObjectOfType<FruitUI>().upgradeToPrem();
            }
        }
        catch (StoreSubscriptionInfoNotSupportedException)
        {
            var receipt = (Dictionary<string, object>)MiniJson.JsonDecode(monthlySubscriptionProduct.receipt);
            var store = receipt["Store"];
            Debug.Log("Invalid store: "+store);
        }

    }

                bool IsSubscribedTo(Product subscription)
        {
            if (subscription.receipt == null)
            {
                return false;
            }

            var subscriptionManager = new SubscriptionManager(subscription, null);

            var info = subscriptionManager.getSubscriptionInfo();

            return info.isSubscribed() == Result.True;
        }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"In-App Purchasing initialize failed: {error}");
    }	
		
	public void purchasePremiumMonthly()
	{
        m_StoreController.InitiatePurchase(premium_monthly);
    }
	public void purchase1()
	{
        m_StoreController.InitiatePurchase(product_1);
    }
	public void purchase2()
	{
        m_StoreController.InitiatePurchase(product_2);
    }
	public void purchase3()
	{
        m_StoreController.InitiatePurchase(product_3);
    }
	public void purchase4()
	{
        m_StoreController.InitiatePurchase(product_4);
    }
	
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
            var product = args.purchasedProduct;
			bool validPurchase = false;
			
			Debug.Log("Processing Purchase");
			//Debug.Log($"Processing Purchase: {product.definition.id}");
			
			var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
			AppleTangle.Data(), Application.identifier);

			try {
				// On Google Play, result has a single product ID.
				// On Apple stores, receipts contain multiple products.
				var result = validator.Validate(args.purchasedProduct.receipt);
				// For informational purposes, we list the receipt(s)
				Debug.Log("Receipt is valid. Contents:");
				foreach (IPurchaseReceipt productReceipt in result) {
					Debug.Log(productReceipt.productID);
					Debug.Log(productReceipt.purchaseDate);
					Debug.Log(productReceipt.transactionID);
					
					if(productReceipt.productID.Equals(product.definition.id))
						validPurchase = true;
				}
				
			} catch (IAPSecurityException) {
				Debug.Log("Invalid receipt, not unlocking content");
			}

            if (m_GooglePlayStoreExtensions.IsPurchasedProductDeferred(product))
            {
				Debug.Log("Purchase Pending");
				//Debug.Log($"Purchase Pending - Product: {product.definition.id}");
                return PurchaseProcessingResult.Pending;
            }
			else{
				
				if (validPurchase) {
						
					Debug.Log("Purchase Complete");
					//Debug.Log($"Purchase Complete - Product: {product.definition.id}");

					if (product.definition.id == premium_monthly)
					{
						FindObjectOfType<FruitUI>().upgradeToPrem();
                        addCoins(1500);
					}
					if (product.definition.id == product_1)
					{
						FindObjectOfType<FruitUI>().openAwardPanel(5000);
					}
					if (product.definition.id == product_2)
					{
						FindObjectOfType<FruitUI>().openAwardPanel(11000);
					}
					if (product.definition.id == product_3)
					{
						FindObjectOfType<FruitUI>().openAwardPanel(30000);
					}
					if (product.definition.id == product_4)
					{
						FindObjectOfType<FruitUI>().openAwardPanel(65000);
					}
					
				}else
					Debug.Log("Invalid Purchase");
				
				return PurchaseProcessingResult.Complete;
				
			}
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
		FindObjectOfType<FruitUI>().showMessage("Purchase Failed");
        //Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }
	
	bool IsPurchasedProductDeferred(string productId)
    {
        var product = m_StoreController.products.WithID(productId);
        return m_GooglePlayStoreExtensions.IsPurchasedProductDeferred(product);
    }
	
	public void playSound(Sound sound)
    {
        audio.volume = 1.0f;
        audio2.volume = 1.0f;
        audio2.loop = false;
        int aud=1;

        switch (sound)
        {
            case Sound.Hit:
                audio.volume = 0.7f;
                audio.clip = spearClip;
                break;
            case Sound.Fruit_Burst:
                audio2.clip = fruitClip;
                aud=2;
                break;
            case Sound.Small_Fruit_Burst:
                audio2.clip = smallfruitClip;
                aud=2;
                break;
            case Sound.Spin:
                audio2.clip = spinClip;
                audio2.loop = true;
                aud=2;
                break;   
            case Sound.SpinEnd:
                audio2.clip = spinEndClip;
                aud=2;
                break;    
            case Sound.Button:
                audio.volume = 0.5f;
                audio.clip = buttonClip;
                FindObjectOfType<FruitUI>().checkForUpdates();
                break;
            case Sound.Button2:
                audio.volume = 0.5f;
                audio.clip = buttonClip2;
                FindObjectOfType<FruitUI>().checkForUpdates();
                break;
            case Sound.Select:
                audio.volume = 0.5f;
                audio.clip = selectClip;
                break;
            case Sound.Clash:
                audio.clip = clashClip;
                break;
            case Sound.Coins:
                audio.clip = coinsClip;
                break;
            case Sound.Boss:
                audio2.clip = bossClip;
                aud=2;
                break;
        }

        if(isSound){
            if(aud==1)
                audio.Play();
            else if(aud==2)
                audio2.Play();
        }
    }
    
    public void toggleSound(){

        if(isSound){
            soundImage.texture = (Texture2D)Resources.Load("sound_off");
        }else{
            soundImage.texture = (Texture2D)Resources.Load("sound_on");
        }

        isSound = !isSound;
    }
    public void toggleMusic(){
        
        if(isMusic){
            music.Pause();
            musicImage.texture = (Texture2D)Resources.Load("music_off");
            PlayerPrefs.SetString("muzic","off");
        }else{
            music.Play();
            musicImage.texture = (Texture2D)Resources.Load("music_on");
            PlayerPrefs.SetString("muzic","on");
        }
        isMusic = !isMusic;
        
    }
    public void singlePlayer()
    {
        gameType = GameType.Normal;
        resetAngles();
        FruitUI.selectedTournament = null;
        
		FindObjectOfType<NetworkScript>().checkError();
		
        startGame();
		
		if(!tutorial){
			FindObjectOfType<FruitUI>().tutPanel.SetActive(true);
			tutorial = true;
		}
    }

    void resetAngles(){
        angle = 200f;
        angleMax = 250f;
        angleMin = 30f;
    }
    public void startGame(){

        restartListener();
		showBannerAd();
        
    }
	
	public void openPreSurveyDialog(){
		
		playSound(Sound.Button);
		if(Pollfish.IsPollfishPresent()){
			FindObjectOfType<FruitUI>().preSurveyText.text = Strings.surveyText;
			FindObjectOfType<FruitUI>().preSurveyDialog.SetActive(true);
		}else
			FindObjectOfType<FruitUI>().showMessage("Surveys not Available");
		
		FindObjectOfType<FruitUI>().surveyDialog.SetActive(false);
	}

	public void openPollfish(){
		playSound(Sound.Button);
		FindObjectOfType<FruitUI>().preSurveyDialog.SetActive(false);
		Debug.Log("Opening Pollfish");
		Pollfish.Show();
		pollfishShowing = true;
	}
	
	public void hidePollfish(){
		if(Pollfish.IsPollfishPanelOpen()){
			Pollfish.Hide();
			pollfishShowing = false;
		}
	}
	
	void Update(){
		
		if (pollfishShowing && Input.GetKeyDown (KeyCode.Escape)) {
			Pollfish.ShouldQuit();
			pollfishShowing = false;
		}
		
	}

    public void rateApp()
    {
        playSound(Sound.Button);
        string appPackageName = Application.identifier;
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + appPackageName);

    }
    public void goToFacebook()
    {
        playSound(Sound.Button);
        Application.OpenURL("https://www.facebook.com/somteestudio");

    }
    public void goToIG()
    {
        playSound(Sound.Button);
        Application.OpenURL("https://www.instagram.com/somteestudios");
    }
	public void openPrivacyPolicy()
    {
        playSound(Sound.Button);
        Application.OpenURL("https://somteestudios.com.ng/privacy-policy/");

    }
	public void openT_N_Cs()
    {
        playSound(Sound.Button);
        Application.OpenURL("https://somteestudios.com.ng/terms-conditions/");

    }
	public void goToAppPage()
    {
        playSound(Sound.Button);
        Application.OpenURL("https://play.google.com/store/apps/dev?id=6297198758873961665");
    }

    public void goToMarket()
    {
        playSound(Sound.Button);

        if (!FindObjectOfType<FruitUI>().marketPanel.activeSelf)
        {
            FindObjectOfType<FruitUI>().marketPanel.SetActive(true);
            FindObjectOfType<FruitUI>().marketPanel2.SetActive(true);
			FindObjectOfType<FruitUI>().setupMarket(1);
			
			setMarketIcons();
        }
    }
	
	public void setMarketIcons(){
		FindObjectOfType<FruitUI>().shrText.text = "+300";
			if(FruitUI.hasShared){
				FindObjectOfType<FruitUI>().shrText.text = "SHARE";
				FindObjectOfType<FruitUI>().shrIco.SetActive(false);
			}
			FindObjectOfType<FruitUI>().rtText.text = "+300";
			if(FruitUI.hasRated){
				FindObjectOfType<FruitUI>().rtText.text = "RATE";
				FindObjectOfType<FruitUI>().rtIco.SetActive(false);
			}
	}

    private void shareScore()
    {

        string screenshotName = "fruitspear_highscore.png";
        string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
        takeScreenshot(screenshotName);
		
        string appPackageName = Application.identifier;

        var shareSubject = "Fam! I challenge you to beat my high score in" +
                    " Fruit Spear";
        var shareMessage = "Fam! I challenge you to beat my high score in" +
                    " Fruit Spear" +
                    "\nIt ain't easy being the best :)" +
                     "\n\n" +
                    "https://play.google.com/store/apps/details?id=" + appPackageName;

        NativeShare shareIntent = new NativeShare();
        shareIntent.AddFile(screenShotPath, null);
        shareIntent.SetSubject(shareSubject);
        shareIntent.SetText(shareMessage);
        shareIntent.SetTitle("Share your score with friends...");

        shareIntent.Share();

    }

     public void shareLeaderboardScore()
    {
        FindObjectOfType<FruitUI>().closeLeaderboardShareDialog();
        string screenshotName = "fruitspear_leaderboard.png";
        string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
        takeScreenshot(screenshotName);
		
        string appPackageName = Application.identifier;

        var shareSubject = "Check out my leaderboard position in" +
                    " Fruit Spear";
        var shareMessage = "Yo! Check out my leaderboard position in" +
                    " Fruit Spear" +
                    "\nDo you think you can beat my score?." +
                     "\n\n" +
                    "https://play.google.com/store/apps/details?id=" + appPackageName;

        NativeShare shareIntent = new NativeShare();
        shareIntent.AddFile(screenShotPath, null);
        shareIntent.SetSubject(shareSubject);
        shareIntent.SetText(shareMessage);
        shareIntent.SetTitle("Share your leaderboard position with friends...");

        shareIntent.Share();

    }

    public void takeScreenshot(string screenshotName){       
        new WaitForEndOfFrame();
        string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
        ScreenCapture.CaptureScreenshot(screenshotName, 1);
        new WaitForSeconds(0.5f);
        Debug.Log("Screenshot Captured");
    }
    
    public void exitGame()
    {
        Application.Quit();
    }
	public void signOut()
    {
		FindObjectOfType<NetworkScript>().signOut();
    }
    public void resetLeaderboard()
    {
        for (int i = 0; i < leaderboardItems.Count; i++)
        {
            Destroy(leaderboardItems[i]);
        }
        leaderboardItems.Clear();
    }

    public void resetInviteList()
    {
        for (int i = 0; i < inviteItems.Count; i++)
        {
            Destroy(inviteItems[i]);
        }
        inviteItems.Clear();
    }

    public void resetMarket()
    {
        for (int i = 0; i < spearItems.Count; i++)
        {
            Destroy(spearItems[i]);
        }
        spearItems.Clear();
    }

    public void resetList()
    {
        for (int i = 0; i < listItems.Count; i++)
        {
            Destroy(listItems[i]);
        }
        listItems.Clear();

        for (int i = 0; i < transactionItems.Count; i++)
        {
            Destroy(transactionItems[i]);
        }
        transactionItems.Clear();
    }

    public void goToMenu()
    {
        playSound(Sound.Button);
        clearAll();

        running = false;
        menuPanel.SetActive(true);
        closeMarket();
        //gameType = GameType.Normal;
        gameOverPanel.SetActive(false);
        UIPanel.SetActive(false);

        stageText.text = "";
        scoreText.text = "";
		setBest();
    }
	
	public void setBest(){
		int x = highScore;
		if(NetworkScript.myHighScore > highScore)
			x = NetworkScript.myHighScore;
		
        statsText.text = "BEST -  STAGE " + highStage + " : SCORE " + x;
	}
	
    void closeMarket()
    {
        playSound(Sound.Button);
        FindObjectOfType<FruitUI>().marketPanel.SetActive(false);
        FindObjectOfType<FruitUI>().marketPanel2.SetActive(false);
		
    }

    void FixedUpdate()
    {
        if (coinsText.IsActive())
            coinsText.text = NetworkScript.coins + "";
        if (greenApplesText.IsActive())
            greenApplesText.text = NetworkScript.greenApples + "";    

        if (running && Time.time - burstUpdate >= 0.75f)
        {
            if (bossFight)
            {
                fruitImage.texture = fruitBosses[fruitStage];
            }
            else
            {
                fruitImage.texture = fruits[fruitStage];
            }

            fruitRB.transform.position = center;
            fruitRB.transform.Rotate(new Vector3(0, 0, angle*Time.deltaTime));
            fruitRBFront.transform.position = center;
            fruitRBFront.transform.Rotate(new Vector3(0, 0, angle*Time.deltaTime));



            if (bossFight)
            {
                fruitRB.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
                fruitRBFront.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
                fruitBreak.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
                float def = 1.875f;
                splash.transform.localScale = new Vector3(def, def, def);
            }
            else
            {
                fruitRB.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                fruitRBFront.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                fruitBreak.transform.localScale = new Vector3(1f, 1f, 1f);
                float def = 1.275f;
                splash.transform.localScale = new Vector3(def, def, def);
            }

            if (Time.time - spawnUpdate >= 0.25f)
            {


                if (!spawnedObjects)
                {
                    if (spearObjectCount > 0)
                    {
                        if (turn)
                        {
                            Vector3 pos = spearPosition;
                            Instantiate(spearObject.gameObject, pos, Quaternion.identity, GameObject.FindGameObjectWithTag("Tyre").transform);
                            spearObjectCount--;
                            //Debug.Log("Spear Object turn Spawned");
                        }
                        else
                        {
                            Vector3 pos = peachPosition;
                            dropFruit(pos);
                            // Debug.Log("Spear peach turn Spawned");
                        }

                    }
                    else if (peachCount > 0 && turn)
                    {
                        Vector3 pos = peachPosition;
                        if (bossFight)
                            pos = peachPosition2;
                        dropFruit(pos);
                        peachCount--;
                        // Debug.Log("Peach Object Spawned");
                    }


                    turn = !turn;

                }

                spawnUpdate = Time.time;
            }


            if (Time.time - lastUpdate >= 0.5f)
            {
              
			    adCounter -= 0.5f;
                if(adCounter == 0){
					Debug.Log("Ad Counter stops");
                    isRestart = false;
					FindObjectOfType<FruitUI>().loadPanel.SetActive(true);
                    showInterstitialAd();
                }
				
                if(gameType == GameType.Target){
                    if (angle < angleMax){

                        if(counter>3)
                            angle += 25f;
                        else    
                            angle += 100f;
                    }
                }else{

                    if (angle <= angleMin)
                    {
                        slowDown = false;
                        //Debug.Log("Speed Up");
                    }

                    if (slowDown && angle > angleMin)
                        angle -= 25f;
                    else if (angle < angleMax)
                        angle += 25f;

                    angleInterval++;

                    if (angleInterval >= 15f)
                    {
                        slowDown = true;
                        angleInterval = 0f;
                        //Debug.Log("Slow Down");
                    }

                    lastUpdate = Time.time;
                    //Debug.Log("Angle is " + angle);
                }

               // Debug.Log("Angle: "+angle +" Max: "+angleMax);
            }

        }else if(gameType != GameType.Target)
        {
            fruitImage.texture = blank;
        }
       

        if (Time.time - lastUpdate2 >= 1f)
        {
            SpearPhysics.counter--;

            if(!ready && (gameType == GameType.Time || gameType == GameType.Target)){
                counter--;
                counterText.text = counter+"";
                if(counter == 0){
                    ready = true;
                    counterText.text="";
                }
            }

            if(ready && running && gameType == GameType.Time){
                time --;
                stageText.text = "";
                scoreText.text = ""+time;   

                if(time <= 0){
                    Debug.Log("Time game expired");
                    running = false;
                    won = false; 
                    FindObjectOfType<FruitUI>().showResults();
                } 
            }
           

            lastUpdate2 = Time.time;
            //Debug.Log("Counter: " + SpearPhysics.counter);
        }

        if (FindObjectOfType<FruitUI>().extraLifePanel.activeSelf)
        {
            FindObjectOfType<FruitUI>().counterText.text = SpearPhysics.counter + "";
             if (SpearPhysics.counter == 6)
            {
                noSave.SetActive(true);
            }
            if (SpearPhysics.counter <= 0)
            {
                finalizeGameOver();
                Debug.Log("Counter done");
            }
            // Debug.Log("Counter: " + counter);
        }
    }

    private void dropFruit(Vector3 pos){

        if (gameType==GameType.Target || giveFruits){

                                if (gameType!=GameType.Target){

                                    int chance = 5 - fruitStage;
                                    if(chance < 2)
                                        chance = 2;
                                        
                                   
                                    int x = 3;

                                    if(NetworkScript.myCash >= 1500f)
                                        x = 4;    
                                    if(NetworkScript.myCash >= 2000f)
                                        x = 6;
									if(NetworkScript.myCash >= 2500f)
                                        x = 8;
									if(NetworkScript.myCash >= 3000f)
                                        x = 10;
									
                                    int rand = UnityEngine.Random.Range(0,chance);

                                    int moneyLuck = UnityEngine.Random.Range(0,x);
									if(FruitUI.isPrem)
										moneyLuck = 10;

                                    //Debug.Log("Ch: "+chance+" Rand: "+rand+" Score: "+score);
                                    //Debug.Log("Chance2: "+x+" Rand2: "+moneyLuck+" Time: "+System.DateTime.Now.ToString());

                                    if(moneyLuck == 0 && FruitUI.userName != "" && isAd)    
                                        Instantiate(moneyToken.gameObject, pos, Quaternion.identity, GameObject.FindGameObjectWithTag("Tyre").transform);
                                    else if(rand == 0)
                                        Instantiate(greenApple.gameObject, pos, Quaternion.identity, GameObject.FindGameObjectWithTag("Tyre").transform);
                                    else
                                        Instantiate(peach.gameObject, pos, Quaternion.identity, GameObject.FindGameObjectWithTag("Tyre").transform);
                                }else
                                    Instantiate(peach.gameObject, pos, Quaternion.identity, GameObject.FindGameObjectWithTag("Tyre").transform);
                                
                            }
                            
    }

    public void updateTarget(){
         if(gameType == GameType.Target){
                targets--;
                stageText.text = targets + " Left";
         }
    }

    public void spawnSpear()
    {
        spearCount--;

        if(gameType == GameType.Target)
            for (int i = 0; i < spears.Count; i++)
                Destroy(spears[i]);       

         if(gameType == GameType.Target && targets<=0){
            Debug.Log("Target game won");
            running = false;
            won = true; 
            FindObjectOfType<FruitUI>().showResults();
            
        }else{  
            if (spearCount > 0)
            {
                Instantiate(spear.gameObject, new Vector3((Screen.width / 2), spearY, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("GamePanel").transform);
                //Debug.Log("New Spear Spawned, Remaining " + spearCount);
            }
            else
            {
                if(running || gameType == GameType.Target){
                    //Debug.Log("Spears Finished");
                    advance();
                }
            }
        }
    }

    public void finalizeGameOver()
    {
        action = UnityEngine.Random.Range(0,4);
        Debug.Log("Action: "+action);
		if(action == 0)
			action = 1;
        
        if(gameType == GameType.Normal){

            actionButton.SetActive(true);
            if(action == 2 && !isRewardAdReady())
                    action=3;
            if(action == 1 && FruitUI.isPrem)
                actionImage.texture = (Texture2D)Resources.Load("action4");
            else        
                actionImage.texture = (Texture2D)Resources.Load("action" + action);
            gameOverPanel.SetActive(true);
            stageText2.text = "STAGE " + stage;
            scoreText2.text = "" + score;
        }else{
            if(gameType == GameType.Top)
                StartCoroutine(FindObjectOfType<NetworkScript>().uploadHighScore(FruitUI.selectedTournament.id));
            FindObjectOfType<FruitUI>().showResults();
            
        }

        FindObjectOfType<FruitUI>().extraLifePanel.SetActive(false);
        running = false;
        Debug.Log("Finalize Game Over");

		gamesPlayed++;
		PlayerPrefs.SetInt("gamesPlayed",gamesPlayed);
		//Debug.Log("Games Played Updated: "+gamesPlayed);
		
		
		if(NetworkScript.myGames > 200 && score < 10)
			botSuspicion ++;
		else 
			botSuspicion = 0;
		
		if(botSuspicion > 6)
		{
			FindObjectOfType<NetworkScript>().botStrike();
		}
		
        highScore = PlayerPrefs.GetInt("highscore");
        highStage = PlayerPrefs.GetInt("highstage");
       
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highscore", score);
        }
        if (stage > highStage)
        {
            highStage = stage;
            PlayerPrefs.SetInt("highstage", stage);
        }
		
        Debug.Log("HighScore: " + highScore + " MaxStage: " + highStage);
        Debug.Log("Attempt to post score");

        if(games >= 20){
           FindObjectOfType<FruitUI>().showratePanel2();
           games = -30;
        }
        

        if(FruitUI.userName != ""){
			
			StartCoroutine(uploadScores());
        }
        else
            Debug.Log("User Not Logged In, Highscore not uploaded");
    }

    public void updateCoins()
    {
        FindObjectOfType<NetworkScript>().updateApples();
    }
	
	IEnumerator uploadScores(){
		
		PlayerPrefs.SetInt("minHighScore",NetworkScript.minHighScore);
		PlayerPrefs.SetInt("minTodaysScore",NetworkScript.minTodaysScore);
		PlayerPrefs.SetInt("minApples",NetworkScript.minApples);
		
			if(FindObjectOfType<NetworkScript>().CheckForInternetConnection(0)){
				
				FindObjectOfType<NetworkScript>().updateDatabase();
				
				if(NetworkScript.isMailVerfied){
					
					Debug.Log("Checking: Score: "+score+" minHighScore: "+NetworkScript.minHighScore);
					if(score > todaysScore && score > NetworkScript.minHighScore)
						StartCoroutine(FindObjectOfType<NetworkScript>().uploadHighScore(null));
					/**
					Debug.Log("Checking: todaysScore: "+score+" minTodaysScore: "+NetworkScript.minTodaysScore);
					if(score > todaysScore && score > NetworkScript.minTodaysScore)
						StartCoroutine(FindObjectOfType<NetworkScript>().uploadTodaysHighScore());
					**/
					Debug.Log("Checking: Green Appls: "+NetworkScript.greenApples+" minApples: "+NetworkScript.minApples);
					if(NetworkScript.greenApples > NetworkScript.minApples){
						int min = NetworkScript.greenApples/10;
						if(NetworkScript.greenApples < 4000 || NetworkScript.watched > min || NetworkScript.myGames > min)
							StartCoroutine(FindObjectOfType<NetworkScript>().uploadTodaysGreenApples());
					}
				}
            }
			
		yield return null;	
	}
	
    private void restartListener()
    {
        playSound(Sound.Button);
		 Debug.Log("(Test) Game Restarting");
		
		StartCoroutine(FindObjectOfType<NetworkScript>().CheckConnection(0));    
        
        adInterval++;
        Debug.Log("(Test) Ad Interval:" + adInterval);
		
		saved = false;
        adCount = 3;
		
		if(NetworkScript.myCash >= 1000f){
			adCount = 2;
		}

		if(NetworkScript.blocked){
			FindObjectOfType<FruitUI>().showMessage("This action has been suspended for 2 days due to suspicion of bots");
		}else{
			
			UIPanel.SetActive(true);
			menuPanel.SetActive(false);


			if(gameType == GameType.Normal){

				if (adInterval >= adCount)
				{
					isMarket = false;
					Debug.Log("(Test) Attempting Interstitial Ad");
					
					if(adLoaded){
						isRestart = true;
						showInterstitialAd();
					}
					else {
						restart();    
					}
				}
				else
				{
					restart();
				}

				skipButton.SetActive(true);

			}else{
				skipButton.SetActive(false);
				restart();
			}
		
		}

    }

    public void showTournamentInterstitial(){
        isRestart = false;
		adInterval ++;
        if(adLoaded && adInterval >= adCount)
            showInterstitialAd();
    }

    public void buyCoins(int amt){
        FindObjectOfType<FruitUI>().showMessage("Successfully purchased "+amt+" Apples");
        addCoins(amt);
    }

    public void addCoins(int amt){
        NetworkScript.coins += amt;
        updateCoins();
        FindObjectOfType<FruitUI>().updatePrem();
		
    }
	public void giveDailyReward(){
		
		rewardAmount = 50;
		rewardType = 1;
		
		if(NetworkScript.rewardDay >= 2){
			rewardType = 2;
			shade2.SetActive(false);
			tick1.SetActive(true);
		}
		if(NetworkScript.rewardDay >= 3){
			rewardAmount = 100;
			rewardType = 1;
			shade3.SetActive(false);
			tick2.SetActive(true);
			
		}
		if(NetworkScript.rewardDay >= 4){
			rewardType = 2;
			shade4.SetActive(false);
			tick3.SetActive(true);
		}
		if(NetworkScript.rewardDay >= 5){
			rewardAmount = 200;
			rewardType = 1;
			shade5.SetActive(false);
			tick4.SetActive(true);
		}
		if(NetworkScript.rewardDay >= 6){
			rewardType = 2;
			shade6.SetActive(false);
			tick5.SetActive(true);
		}
		if(NetworkScript.rewardDay >= 7){
			rewardAmount = 400;
			rewardType = 1;
			shade7.SetActive(false);
			tick6.SetActive(true);
		}
		if(NetworkScript.rewardDay >= 8){
			rewardType = 2;
			shade8.SetActive(false);
			tick7.SetActive(true);
		}
		if(NetworkScript.rewardDay > 8){
			rewardAmount = 200;
			rewardType = 1;
            day1Text.text = "BONUS";
            if(FruitUI.isPrem){
			    getVIPGreen = true;
                rewardAmount = 500;
                day1Text.text = "VIP";
            }
			tick8.SetActive(true);
			tick1.SetActive(false);
			day1Amount.text = "+"+rewardAmount;
            
		}
			
		RewardPanel.SetActive(true);
	}
	
	public void claimReward(){
		playSound(Sound.Button);
		RewardPanel.SetActive(false);
		
		Debug.Log("Type " + rewardType);
		Debug.Log("Amount " + rewardAmount);
		
		if(rewardType == 1){
			FindObjectOfType<FruitUI>().openAwardPanel(rewardAmount);
		}else if(rewardType == 2){
			Debug.Log("Awarded: "+rewardAmount);
            FindObjectOfType<NetworkScript>().giveCash(rewardAmount," from Daily Reward");
		}
		
	}
	
    public void restart()
    {
        score = 0;
        stage = 1;
        givenFreeSpear = false;
        
        Debug.Log("Restart");
       
        if(gameType == GameType.Normal){
            stageText.text = "STAGE " + stage;
            fruitStage = 0;    
        }
        if(gameType == GameType.Top){
            stageText.text = "Top - "+ topScore;
            fruitStage = UnityEngine.Random.Range(0,10);    
            resetAngles();
        }
        if(gameType == GameType.Time){
            time = FruitUI.selectedTournament.time;
            stageText.text = "";
            scoreText.text = ""+time;
            fruitStage = UnityEngine.Random.Range(0,10);    
        }
        if(gameType == GameType.Target){
            fruitStage = UnityEngine.Random.Range(0,10);    
        }


        fruitImage.texture = fruits[fruitStage];    
        splashImage.color = splashColor[fruitStage];
        
        scoreText.text = "" + score;
        gameOverPanel.SetActive(false);
        bossFight = false;
        running = true;
        
        reset();
    }
    public void advance()
    {
        stage++; 
        games++;
        
        Debug.Log("Advance");
		Debug.Log("(Test) Ad Interval: " + adInterval+" / Ad Count: "+adCount);
        //Debug.Log("Games Played; "+games);
        stageText.text = "STAGE " + stage;
       
        int interval = 5;
        running = true;
		isReset = true;

        if(gameType == GameType.Normal){
            if (stage > 10)
                interval = 10;
        }
        if(gameType == GameType.Top){
            stageText.text = "Top - "+ topScore;
            interval = 8;
        }
        if(gameType == GameType.Target){
            if(targets > 0){
               Debug.Log("Target game lost");
               running = false;
               won = false; 
               FindObjectOfType<FruitUI>().showResults();
            }
        }   
        else if(gameType == GameType.Time){
               Debug.Log("Time game won");
               running = false;
               won = true; 
               FindObjectOfType<FruitUI>().showResults();

        }else{ 

             Debug.Log("Advance to next stage " + stage);

            if (stage % interval == 0)
            {
                if (!bossFight)
                {
                    stage--;
                    stageText.text = bossNames[fruitStage] + " BOSS";
                    fruitImage.texture = fruitBosses[fruitStage];
                    Debug.Log("BOSS FIGHT!");
                    bossFight = true;
                    bossFightImage.GetComponent<Animator>().Play("boss_fight");
                    playSound(Sound.Boss);
                   
                }
                else
                {   
                    if(stage > highStage && !givenFreeSpear){
                        givenFreeSpear = true;
                        FindObjectOfType<FruitUI>().giveFreeSpears();
                    }
                    fruitStage++;
                    if (fruitStage > 15)
                        fruitStage = 0;

                    if(gameType == GameType.Top){
                        fruitStage = UnityEngine.Random.Range(0,10);
                    }     

                    fruitImage.texture = fruits[fruitStage];
                    splashImage.color = splashColor[fruitStage];
                    bossFight = false;
                    Debug.Log("Fruit Stage Upgraded to " + fruitStage);
                                        
                    if(fruitStage > 1) 
                        adInterval+=2;
                    else
                        adInterval++;
                        
					//Debug.Log("(Test) Ad Interval: " + adInterval+" / Ad Count: "+adCount);
					
					//FindObjectOfType<FruitUI>().loadPanel.SetActive(true);
					if(adLoaded && gameType == GameType.Normal && !FruitUI.isPrem){
							
							if(adInterval >=adCount && isInterstitialReady()){
								Debug.Log("Ad Counter Starts");
								isMarket = false;
								adCounter = 0.5f;
								isReset = false;
							}
						
					}
                    
					StartCoroutine(FindObjectOfType<NetworkScript>().CheckConnection(1));    
                }


            }
			if(isReset)
				reset();
        }

    }
	
    public void clearAll()
    {
        for (int i = 0; i < spearIcons.Count; i++)
        {
            Destroy(spearIcons[i]);
        }
        for (int i = 0; i < spears.Count; i++)
        {
            Destroy(spears[i]);
        }
        for (int i = 0; i < peaches.Count; i++)
        {
            Destroy(peaches[i]);
        }
        for (int i = 0; i < spearObjects.Count; i++)
        {
            Destroy(spearObjects[i]);
        }
        spears.Clear();
        spearIcons.Clear();
        peaches.Clear();
        spearObjects.Clear();
    }

    private void reset()
    {
        clearAll();

        if (stage < 5)
            spearCount = UnityEngine.Random.Range(6, 10);
		else if (stage < 35)
			spearCount = UnityEngine.Random.Range(7, 12);
        else 
            spearCount = UnityEngine.Random.Range(7, 14);

        if(gameType == GameType.Top){
            spearCount = UnityEngine.Random.Range(8, 14);  
        }
            
        if(gameType == GameType.Time)
            spearCount = FruitUI.selectedTournament.shots;      

        if(gameType == GameType.Target)
            spearCount = FruitUI.selectedTournament.shots;      

        if (bossFight)
            spearCount = 14;

        //Debug.Log("Total Spears = " + spearCount);

        for (int i = 0; i < spearCount; i++)
        {
            Instantiate(spearIco.gameObject, GameObject.FindGameObjectWithTag("GamePanel").transform);
        }

        Instantiate(spear.gameObject, new Vector3((Screen.width / 2), spearY, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("GamePanel").transform);

        spawnedObjects = false;

        turn = true;
        spearObjectCount = 0;

        int pc1 = UnityEngine.Random.Range(-2, 4);
        if (pc1 > 0 && !bossFight)
        {
            spearObjectCount = pc1;
        }
        //Debug.Log("PC1: " + pc1);

        int pc = UnityEngine.Random.Range(1, 3);
        if (pc > 0){
            peachCount = pc;
        } 

        if (bossFight)
            peachCount = 5;

        if(gameType == GameType.Time){
            spearObjectCount = FruitUI.selectedTournament.spears;
        }   
        if(gameType == GameType.Target){
            spearObjectCount = FruitUI.selectedTournament.spears;
            peachCount = FruitUI.selectedTournament.amount;
            targets =  FruitUI.selectedTournament.amount;
            angleMax = FruitUI.selectedTournament.speed;
            stageText.text = targets + " Left";
            Debug.Log("There are "+FruitUI.selectedTournament.amount+" Targets");

            angle = 150f;
        }else{
            resetAngles();
        }

             

        // Debug.Log("PC: " + pc + ", Peach Count: " + peachCount);
         if(gameType == GameType.Normal || gameType == GameType.Top){
            ready = true;
         }else{
             ready = false;
             counter = 6;
         }
        
        // Debug.Log("Initial Spear Spawned, Remaining " + spearCount);
    }
	
	//AD EVENTS
	public void showBannerAd(){
		
		Debug.Log("Showing Banner");
		if(!bannerShowing && bannerLoaded && adLoaded && !FruitUI.isPrem){
            
            MaxSdk.ShowBanner(bannerAdID);
            bannerShowing = true;
			
		}
		
		if(adLoaded)
			if(!bannerLoaded)
				loadBannerAd();
		
	}
	
	public void hideBannerAd(){
		Debug.Log("Hiding Banner");
		if(adLoaded){
			MaxSdk.HideBanner(bannerAdID);
            bannerShowing = false;
		}
	}
    
	public void loadBannerAd(){
		Debug.Log("Loading Banner Ad");
		MaxSdk.CreateBanner(bannerAdID, MaxSdkBase.BannerPosition.BottomCenter);
	}

	public void checkBanner(){
		if(FruitUI.isPrem)
			hideBannerAd();
	}

    public void showInterstitialAd()
    {	
        if (!FruitUI.isPrem)
        {   
			if(isInterstitialReady()){
				noAd = 0;
				isAd = true;

				MaxSdk.ShowInterstitial(interstitialAdID);
				Debug.Log("Showing Video AD");
				
				adInterval = 0;
			}else{
				noAd++;
				if(noAd >= 2)
					isAd = false;
				
                if(isRestart)
				    restart();
				
				loadInterstitialAd();
			}
        }
        else
        {   
            if(isRestart)
                restart();
        }
		
    }
	
    public void showRewardAd()
    {
      if(adLoaded){

            if(isRewardAdReady()){
                Debug.Log("Showing Reward AD");
                FindObjectOfType<FruitUI>().extraLifePanel.SetActive(false);
                MaxSdk.ShowRewardedAd(rewardAdID);
				FruitUI.rewardInterval++;
				adInterval = 0;
            }
           
            else
            {
                if (rewardMode != RewardMode.Revive)
                    FindObjectOfType<FruitUI>().showMessage("Video Ad was unable to load, check internet connection and try again");

                loadRewardAd();
            }
        }else{
            FindObjectOfType<FruitUI>().showMessage(" Ad hasn't loaded");
            StartCoroutine(checkInternet2());
        }  
		
    }
	
	public IEnumerator checkInternet2(){
		FindObjectOfType<NetworkScript>().CheckForInternetConnection(1);
		yield return null;
	}

    public bool isRewardAdReady()
    {
		
      if (MaxSdk.IsRewardedAdReady(rewardAdID)){
			Debug.Log("Reward ad available");
            return true;
	   }
       else {
            Debug.Log("Reward ad not available");
            return false;
       }
    }
	
	private bool isInterstitialReady(){
		if (MaxSdk.IsInterstitialReady(interstitialAdID))
			return true;
		else
			return false;
		
	}
	
    
    public void loadRewardAd()
    {
        if(!isRewardAdReady()){
            MaxSdk.LoadRewardedAd(rewardAdID);
			Debug.Log("Loading Reward AD");
		}
       
    }


    public void loadInterstitialAd()
    {	
		if(!isInterstitialReady()){
			MaxSdk.LoadInterstitial(interstitialAdID);
			Debug.Log("Loading Interstitial AD");  
		}
		
    }

    private void revive()
    {
        Destroy(spears[spears.Count-1]);
        running = true;
        FindObjectOfType<FruitUI>().extraLifePanel.SetActive(false);
        Instantiate(spear.gameObject, new Vector3((Screen.width / 2), spearY, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("GamePanel").transform);
        Debug.Log("Revived");
    }
	
	private void OnEnable () {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
		MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
		MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
		MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
		MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
		MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

		MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
		MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
		MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
		MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
		MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
		MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
		MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
		MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

		MaxSdk.SetBannerBackgroundColor(bannerAdID, Color.clear);

		MaxSdkCallbacks.Banner.OnAdLoadedEvent      += OnBannerAdLoadedEvent;
		MaxSdkCallbacks.Banner.OnAdLoadFailedEvent  += OnBannerAdLoadFailedEvent;
		MaxSdkCallbacks.Banner.OnAdClickedEvent     += OnBannerAdClickedEvent;
		MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
		MaxSdkCallbacks.Banner.OnAdExpandedEvent    += OnBannerAdExpandedEvent;
		MaxSdkCallbacks.Banner.OnAdCollapsedEvent   += OnBannerAdCollapsedEvent;
		
		Pollfish.SurveyCompletedEvent += SurveyCompleted;
		  Pollfish.SurveyOpenedEvent += SurveyOpened;
		  Pollfish.SurveyClosedEvent += SurveyClosed;
		  Pollfish.SurveyReceivedEvent += SurveyReceived;
		  Pollfish.SurveyNotAvailableEvent += SurveyNotAvailable;
		  Pollfish.UserNotEligibleEvent += UserNotEligible;
		  Pollfish.UserRejectedSurveyEvent += UserRejectedSurvey;
	}

	private void OnDisable () {
		MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoadedEvent;
		MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialLoadFailedEvent;
		MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnInterstitialDisplayedEvent;
		MaxSdkCallbacks.Interstitial.OnAdClickedEvent -= OnInterstitialClickedEvent;
		MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialHiddenEvent;
		MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnInterstitialAdFailedToDisplayEvent;

		MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoadedEvent;
		MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdLoadFailedEvent;
		MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedAdDisplayedEvent;
		MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnRewardedAdClickedEvent;
		MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnRewardedAdRevenuePaidEvent;
		MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedAdHiddenEvent;
		MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplayEvent;
		MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;

		MaxSdkCallbacks.Banner.OnAdLoadedEvent      -= OnBannerAdLoadedEvent;
		MaxSdkCallbacks.Banner.OnAdLoadFailedEvent  -= OnBannerAdLoadFailedEvent;
		MaxSdkCallbacks.Banner.OnAdClickedEvent     -= OnBannerAdClickedEvent;
		MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnBannerAdRevenuePaidEvent;
		MaxSdkCallbacks.Banner.OnAdExpandedEvent    -= OnBannerAdExpandedEvent;
		MaxSdkCallbacks.Banner.OnAdCollapsedEvent   -= OnBannerAdCollapsedEvent;
		
		Pollfish.SurveyCompletedEvent -= SurveyCompleted;
		  Pollfish.SurveyOpenedEvent -= SurveyOpened;
		  Pollfish.SurveyClosedEvent -= SurveyClosed;
		  Pollfish.SurveyReceivedEvent -= SurveyReceived;
		  Pollfish.SurveyNotAvailableEvent -= SurveyNotAvailable;
		  Pollfish.UserNotEligibleEvent -= UserNotEligible;
		  Pollfish.UserRejectedSurveyEvent -= UserRejectedSurvey;
	}
	
	public void SurveyClosed()
	{
	  Debug.Log("PollfishDemo: Survey was closed");
	}
	public void SurveyOpened()
	{
	  Debug.Log("PollfishDemo: Survey was opened");
	}
	public void UserRejectedSurvey()
	{
	  Debug.Log("PollfishDemo: User rejected survey");
	}
	public void SurveyNotAvailable()
	{
	  Debug.Log("PollfishDemo: Survey not available");
	}
	public void UserNotEligible()
    {
        Debug.Log("PollfishDemo: User not eligible");
    }
	public void SurveyCompleted(SurveyInfo surveyInfo)
	{
	  Debug.Log("PollfishDemo: Survey was Completed - " + surveyInfo.ToString());
	  FindObjectOfType<FruitUI>().showMessage("Tokens will be received when app restarts");
	}
	public void SurveyReceived(SurveyInfo surveyInfo)
	{
	  if (surveyInfo == null)
	  {
		Debug.Log("PollfishDemo: Survey Offerwall received");
		FindObjectOfType<FruitUI>().surveyDialog.SetActive(true);
	  }
	  else
	  {
		Debug.Log("PollfishDemo: Survey was completed - " + surveyInfo.ToString());
	  }
    }

    //BANNER CALLBACKS
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
		bannerLoaded = true;
		Debug.Log("Banner Ad Ready");
		if(!FindObjectOfType<FruitUI>().tutorialPanel.activeSelf)
			showBannerAd();
	}

	private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {}

	private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

	private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

	private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)  {}

	private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

	//INTERSTITIAL CALLBACKS
	private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
	{
		Debug.Log("Interstitial AD Loaded"); 
		retryAttempt = 0;
	}

	private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
	{
		Debug.Log("Interstitial ADs Failed to Load"); 

		retryAttempt++;
		double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));
		
		Invoke("loadInterstitialAd", (float) retryDelay);
	}

	private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
		Debug.Log("Interstitial Ad Closed");
			if(!isMarket && isRestart)
				restart();
			if(!isReset){
				reset();
				isReset = true;
			}
			
			NetworkScript.closeLoad = true;
			loadInterstitialAd();
	}

	private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
	{
		Debug.Log("Interstitial ADs Failed to Display"); 
		loadInterstitialAd();
	}

	private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

	private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
	{
		Debug.Log("Interstitial ADs is Hidden"); 
		loadInterstitialAd();
	}

	//REWARD CALLBACKS
	private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
	{
		Debug.Log("Reward AD Ready"); 
		retryAttempt2 = 0;
	}

	private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
	{
		Debug.Log("Reward AD Failed to Load"); 
		retryAttempt2++;
		double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt2));
		
		Invoke("loadRewardAd", (float) retryDelay);
	}

	private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        loadRewardAd();
    }

	private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
	{
		Debug.Log("Reward AD Failed to Display"); 
		loadRewardAd();
	}

	private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {}

	private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
	{
		Debug.Log("Reward AD is Hidden"); 
		loadRewardAd();
	}

	private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
	{
		Debug.Log("Reward AD Completed"); 
		finalizeReward();
	}

	private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
	{
		
	}

    private void finalizeReward(){

        Debug.Log("User Rewarded"); 

        if(rewardMode == RewardMode.Revive){
                revive();
        }
        else if(rewardMode == RewardMode.Skip){
                advance();
        }
        else if(rewardMode == RewardMode.Coins){
				FindObjectOfType<FruitUI>().openAwardPanel(appleReward);
        }
        else if(rewardMode == RewardMode.Spin){
                FindObjectOfType<SpinScripct>().proceedSpin();
        }
        else if(rewardMode == RewardMode.Green){
               FindObjectOfType<FruitUI>().openGreenAwardPanel(5);
        }

    }
	

	
	public void showOfferWall(){
		if(offerChecked){
			Debug.Log("Showing OfferWall");
			//IronSource.Agent.showOfferwall();
		}else
			FindObjectOfType<FruitUI>().showMessage("OfferWall coming soon...");
	}
	private void getUserCredits(){
		Debug.Log("Retreiving User Credits");
		offerChecked = true;
		//IronSource.Agent.getOfferwallCredits();
	}


}
