using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using UnityEngine;
using Firebase;
//using Firebase.Unity.Editor;
using Firebase.Database;
using Firebase.Functions;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using System.Net;
using Facebook.Unity;
using Google;
using OPS.AntiCheat;
using OPS.AntiCheat.Field;
using OPS.AntiCheat.Detector;

public class NetworkScript : MonoBehaviour
{
    private static DatabaseReference reference, reference2, referenceList;
    private int serial, leaderBoardMax = 100, minWithdrawal = 10, minReferral = 2, startAmount = 750, startAmount2 = 500; 
    public static List<User> users = new List<User>();
    public static List<User> invites = new List<User>();
	public static List<User> retreivedUsers = new List<User>();
    public static List<string> transactions = new List<string>();
    public static List<string> myTournaments = new List<string>();
    public static List<int> myPlays = new List<int>();
    public static List<float> inviteTokens = new List<float>();
    public static List<Tournament> tournaments = new List<Tournament>();
    public static string email, timeZone, available ="", message="", rules2="", notif, green, unityAd, awardMessage, inviteLeftMessage, approved="", state="", accountDet="";
    private bool connected, loggenIn, closedDialogs, signed, ready, isFB, isNew, award, invited, surveyChecked;
	public static int myHighScore, myGames, cashCount, usedCashCount, watched, tournCount, curDay, sysDay, rewardDay, month, yesterday, dt;
    public static float cashIncrease,gold,silver,bronze, extra1, extra2, inviteReward,invReward, paydate;
	public static User me;
	public GameObject profileDialog, SignUpDialog, LoginDialog, progBar;
	public Text info, cashText, loginText, inviteText, inviteLeftText, tournamentSize, versionText;
	public RawImage avatar, curImage, notifImage;
	private string password, messageToast="", inviteRefID, inviteID="", idToken, id;
    public static bool isNotif, blocked, isCashLeaderboard, closeLoad, bonused, error, close, close2, winnerReady, refreshList, isMailVerfied, leaderboardEmpty, leaderboardReady, myTournamentsReady, tournLeaderboardReady, inviteReady, transactionReady, tournamentReady, isEmpty;
    private float lastUpdate, pend, surveyPend, cashout;
    private List<string> used = new List<string>();
	private List<string> retreived = new List<string>();
	private List<string> competitionIDs = new List<string>();
	private static readonly HttpClient client = new HttpClient();
	public static List<object> topScores, topCashouts, todaysScores, todaysApples, yesterdaysApples,competition,competition2,data;
	private bool validAuth, chekkk, topCashReady, totScReady,todScReady,todAppReady, yesAppReady,compReady,compReady2,transReady,isReady,invReady,newInvReady, myTournReady, tournReady, loadedTourn;
	private static string signInFailed = "Sign In Failed", prem = "false", myCountry="";
	public static int minApples, minHighScore, minTodaysScore, minBid1, x1, x2, x3, compIndex, b_strike = 0;
	private Firebase.Auth.FirebaseUser user;
	private Firebase.Auth.FirebaseAuth auth;
	public static string winner1Name, winner2Name, winner3Name, winner1Photo, winner2Photo, winner3Photo;
	public static string login_id, isSecure="", transactJsonString="", tournJsonString="", inviteJsonString="";
	public string webClientId = "308123983338-c3kq32n72e2qsg2r9kkrgkib0ck3il0n.apps.googleusercontent.com";
	private GoogleSignInConfiguration configuration;
	public static bool signedIn, levelUp, proceedLogin, proceedMigrate, manualSignIn, startLoad, isMigrate, initialiseFirebase, firebaseReady;
	public static ProtectedInt32 money, greenApples, coins;
    public static ProtectedFloat myCash;
	
	void Start(){
		SignUpDialog.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        LoginDialog.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        //email = PlayerPrefs.GetString("email");
		//password = PlayerPrefs.GetString("password");
        
		coins = new ProtectedInt32(0);
		money = new ProtectedInt32(0);
		greenApples = new ProtectedInt32(0);
		
		myCash = new ProtectedFloat(0f);
		
		FieldCheatDetector.OnFieldCheatDetected += FieldCheatDetector_OnFieldCheatDetected;
		
        serial = UnityEngine.Random.Range(1001,9999);
		
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
		
		minHighScore = PlayerPrefs.GetInt("minHighScore");
		minTodaysScore = PlayerPrefs.GetInt("minTodaysScore");
		minApples = PlayerPrefs.GetInt("minApples");
		minBid1 = PlayerPrefs.GetInt("minBid1");
		
		Debug.Log("Retreived minHighScore:"+minHighScore);
		Debug.Log("Retreived minTodaysScore:"+minTodaysScore);
		Debug.Log("Retreived minApples:"+minApples);
		Debug.Log("Retreived minBid:"+minBid1);
		
		me = new User("", "", "", "", 0);

		if(PlayerPrefs.GetInt("level") == 1)
			levelUp = true;
      
	}
	
	private void FieldCheatDetector_OnFieldCheatDetected()
    {
		Application.Quit();
        //Text.text = "Field Hack Detected! Cheater tried to modify memory!";
    }

    public void getDate(){
		
		Debug.Log("UTC Timezone for current Day: "+curDay);
		
		yesterday = curDay - 1;

                if(curDay == 1){

                    if(month == 3)
                        yesterday = 28;
                    else if(month == 10 || month == 5 || month == 7 || month == 12)
                        yesterday = 30;
                    else 
                        yesterday = 31;    
					
					month -=1;
                }
        Debug.Log ("Yesterday is "+yesterday);

    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
        Debug.Log("Received Cloud Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
        Debug.Log("Received a new Cloud message from: " + e.Message.From);
    }
	public void SuscribeNotif(string id){
		Firebase.Messaging.FirebaseMessaging.Subscribe("/topics/"+id);
		PlayerPrefs.SetString("fcm", "on");
		Debug.Log("Suscribed to push Notifications Topic: "+id);
		isNotif = true;
	}
	public void UnscribeNotif(string id){
		Firebase.Messaging.FirebaseMessaging.Unsubscribe("/topics/"+id);
		PlayerPrefs.SetString("fcm","off");
		Debug.Log("UnSuscribed from push Notifications Topic: "+id);
	}
	
    public bool CheckForInternetConnection(int num)
    {
        try
        {
            using (var client = new WebClient())
            using (client.OpenRead("http://google.com/generate_204"))
                
            Debug.Log("Internet Connection Available");  
			if(num==1)
				FindObjectOfType<FruitUI>().showMessage("Connecting to Game Server...");  
            if(!connected)
                FindObjectOfType<FruitUI>().loadPanel.SetActive(true);
            StartCoroutine(setupNetwork());
            return true;
        }
        catch
        {
                Debug.Log("No Internet Connection");
                closeLoadPanels();
                string message = "";

               if(num < 1) 
                    message = "";
               if(num < 2) 
                    message = "No Internet Connection";
               else  
                    message = "User Offline";

                FindObjectOfType<FruitUI>().showMessage(message);

                if(FruitUI.userName == "" && FindObjectOfType<FruitUI>().leaderboardPanel.activeSelf){
                    FindObjectOfType<FruitUI>().userNamePanel.SetActive(true);
                    loginText.text = "Welcome to the World of Champions!";
                }
                    
            return false;
        }
    }

    public IEnumerator CheckConnection(int update)
    {	
        try
        {
            using (var client = new WebClient())
            using (client.OpenRead("http://google.com/generate_204"))
                
            Debug.Log("Internet Connection Available"); 
			if(update == 1){
				finalizeUpdateData(null,null);
			}
			FruitSpearGameCode.giveFruits = true;
			
        }
        catch
        {	
			if(FruitUI.userName != "") 
				FindObjectOfType<FruitUI>().showMessage("No Connection");
			FruitSpearGameCode.giveFruits = false;
        }
		
		 yield return null;
    }

    IEnumerator setupNetwork(){
        
            if(!connected){
				
				connected = true;
				
				//DETECT COUNTRY
				
				//if(FruitUI.consent == null || FruitUI.consent == ""){
					UnityWebRequest request = UnityWebRequest.Get ("https://extreme-ip-lookup.com/json");
					request.chunkedTransfer = false;
					yield return request.Send ();
					Debug.Log("Locating Country...");

					if (request.isNetworkError) {
						Debug.Log("error : " + request.error);
					} else {
						if (request.isDone) {
							Country res = JsonUtility.FromJson<Country> (request.downloadHandler.text);
							Debug.Log("Continent: " + res.continent);
							Debug.Log("City: " + res.city);
							Debug.Log("Country: " + res.country);
							myCountry = res.country;
							
							if(!res.continent.Equals("Europe")){
								Debug.Log("Country Does'nt need consent");
								FruitUI.consent = "yes";
							}
							if(res.country.Contains("United") || res.country.Equals("Germany") || res.country.Equals("France") || 
							res.country.Equals("Canada") || res.country.Equals("Singapore") || res.country.Equals("Hong Kong")){
								Debug.Log("Country badass");
								//invReward = 2;
							}
							/**if(res.country.Equals("Philippines")){
								Debug.Log("Low Country");
								invReward = -1;
							}**/
						}
					}
					
				//}
				
				FruitUI.consent = "yes";

                setupFireBase();
                FindObjectOfType<FruitUI>().checkConsent();
                FB.Init(SetInit, OnHideUnity);
				initGoogle();
                
            }

            if(FruitUI.userName == "" && FindObjectOfType<FruitUI>().leaderboardPanel.activeSelf){
               FindObjectOfType<FruitUI>().userNamePanel.SetActive(true);
               loginText.text = "Welcome to the World of Champions!";
            }
            

    }
	
	private void checkUserAccount(){
		if(!email.Equals("") && !password.Equals("")){
			//Debug.Log("Previous Sign In Detected "+email+" "+password);
			SignInFirebase(email,password);
		}else{
			Debug.Log("No Email Sign In Detected");
			closeLoadPanels();
		}
	}
	
	private void checkLoginStatus(){
	  Debug.Log("Setting up Firebase Auth");
	  auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
	  auth.StateChanged += AuthStateChanged;
	  AuthStateChanged(this, null);
	  Invoke("loginTimeOut",2.5f);
	}

	void loginTimeOut(){
		if(!signedIn)
			closeLoadPanels();
	}
	
	void AuthStateChanged(object sender, System.EventArgs eventArgs) {
	  if (auth.CurrentUser != user) {
		signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
		if (!signedIn && user != null) {
		  Debug.Log("Signed Out");
		  closeLoad = true;
		}
		user = auth.CurrentUser;
		if (signedIn && !manualSignIn) {
		  Debug.Log("Previously Signed in");
		  SignInFirebase();
		}
	  }
	}
	
	void OnDestroy() {
		if(auth != null){
		  auth.StateChanged -= AuthStateChanged;
		  auth = null;
		}
	}
	
	private void SignInFirebase(){

            Debug.Log("User signed in Name:" + user.DisplayName + " Email:" + user.Email);
            
            FruitUI.userName = user.DisplayName.ToLower();
            FruitUI.email = user.Email;
            FruitUI.userID = user.UserId;
			if(user.PhotoUrl != null)
				FruitUI.photoUrl = user.PhotoUrl.ToString();
			isMailVerfied = true;
			
			if(!FruitUI.userID.Equals("")){
				Debug.Log("Prepping to upload profile");
				
				uploadProfile();
				if(!FruitUI.photoUrl.Equals(""))
					StartCoroutine(SetImage(FruitUI.photoUrl+"?type=normal", FindObjectOfType<FruitUI>().profileImage, true));
			}
			else
			  messageToast = signInFailed;
		 
    }

    private void SetInit()
    {
        Debug.Log("FB init done");

        if (FB.IsInitialized)
        {
            FB.ActivateApp();
			/**
            if(FB.IsLoggedIn){
                FB.Mobile.RefreshCurrentAccessToken();
                Debug.Log("Already Logged In");
                AuthCallback(null);
            }else{
                Debug.Log("No Facebook Login on this device");
				//checkUserAccount();
            }
			**/
        }
    }
	
	private void initGoogle(){
		 configuration = new GoogleSignInConfiguration {
            WebClientId = webClientId,
            RequestIdToken = true,
			RequestEmail = true
      };
	  Debug.Log("Google init done");
	}
	
	public void loginGoogle() {
		manualSignIn = true;
		FindObjectOfType<FruitUI>().loadPanel.SetActive(true);
		if(CheckForInternetConnection(1)){
		  GoogleSignIn.Configuration = configuration;
		  GoogleSignIn.Configuration.UseGameSignIn = false;
		  GoogleSignIn.Configuration.RequestIdToken = true;
		  GoogleSignIn.Configuration.RequestEmail = true;
		  Debug.Log("Calling Google SignIn");

		  GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
			OnAuthenticationFinished);
		}
		
    }
	
	internal void OnAuthenticationFinished(Task<GoogleSignInUser> task) {
      if (task.IsFaulted) {
        using (IEnumerator<System.Exception> enumerator =
                task.Exception.InnerExceptions.GetEnumerator()) {
          if (enumerator.MoveNext()) {
            GoogleSignIn.SignInException error =
                    (GoogleSignIn.SignInException)enumerator.Current;
            Debug.Log("Got Error: " + error.Status + " " + error.Message);
          } else {
            Debug.Log("Got Unexpected Exception?!?" + task.Exception);
          }
        }
		closeLoad = true;
		messageToast = "Google Sign in Error";
      } else if(task.IsCanceled) {
        Debug.Log("Canceled");
		closeLoad = true;
		messageToast = "Google Sign in Cancelled";
      } else  {
        Debug.Log("Welcome: " + task.Result.DisplayName + "!");
		idToken = task.Result.IdToken;
		SignInGoogleFirebase(idToken);
		
      }
    }

	void migrateData(){
		
		migrateDataFunction().ContinueWith((task) => {
			if (task.IsFaulted) {
				foreach (var inner in task.Exception.InnerExceptions) {
					if (inner is FunctionsException) {
						var e = (FunctionsException) inner;
						var code = e.ErrorCode;
						Debug.Log("Migrate Data Function Error: ");
						messageToast = "Error migrating Data (At Stage 1)";
					}
				}
			} else {
				string result = task.Result;
				Debug.Log("Migrate Data Function Success");
				Debug.Log("Result: " + result);
			}
			});
			
			
		addMigrateFunctionListener();
	}
	
	private Task<string> migrateDataFunction() {
	  
	  var functions = FirebaseFunctions.DefaultInstance;
	 		
	  Debug.Log("Calling Function - Migrate Data");
	  var data = new Dictionary<string, object>();
	  
	  data["id"] = FruitUI.userID;

	  var function = functions.GetHttpsCallable("migrateData");
	  return function.CallAsync(data).ContinueWith((task) => {
		return (string) task.Result.Data;
	  });
	}
	
	public void addMigrateFunctionListener()
    {
		Debug.Log("Adding Migrate Function Listener");
       reference.Child("users").Child(FruitUI.userID)
	   .ValueChanged += HandleOldIDChanged;
		
		Invoke("migrateTimeOut",4f);
    }
	

	void migrateTimeOut(){
		if(NetworkScript.isSecure.Equals("")){
			closeLoadPanels();
			messageToast = "Time Out Checking Migration Status";
			FruitUI.userID = "";
			FruitUI.userName = "";
		}
	}
	
	 private void HandleOldIDChanged(object sender, ValueChangedEventArgs args) {
		 
		 	 
		if(args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			close = true;
			messageToast = "Error Checking Migration Status";
			return;
		}
			DataSnapshot snapshot = args.Snapshot;
				if (snapshot.Exists){
					string isSecure = "";
					try{
						isSecure = snapshot.Child("isSecure").GetValue(false).ToString();
					}catch(Exception e){
						
					}
					if(NetworkScript.isSecure.Equals("") && isSecure.Equals("true")){
						NetworkScript.isSecure = "true";
						Debug.Log("Migrate Function Completed Successfully");
						isMigrate = true;
						SignInPostMigration();	
					}
				}
				else {
					Debug.Log("Old user database does'nt Exist");
				}	   
    }
	
	void SignInPostMigration(){
		SignInGoogleFirebase(idToken);
		//CreateFirebaseUser(FruitUI.userName,email,"12345678");
	}
	
    private void OnHideUnity(bool isGameShown)
    {

    }

    public void loginFacebook(){
		FindObjectOfType<FruitUI>().loadPanel.SetActive(true);
        if(CheckForInternetConnection(1)){
            var perms = new List<string>() { "public_profile", "email" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
    }

    private void AuthCallback(ILoginResult result)
    {
        //Debug.Log(result);

         if (FB.IsLoggedIn)
        {
            Debug.Log("User Logged in");
			
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            SignInFirebase(aToken);
        }
        else
        {
           closeLoadPanels();
           Debug.Log("User cancelled login");
        }
    }

    private void SignInFirebase(AccessToken aToken){
        
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        
        Firebase.Auth.Credential credential =  Firebase.Auth.FacebookAuthProvider.GetCredential(aToken.TokenString);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if(task.IsCanceled){
                Debug.Log("Sign in Cancelled");
				messageToast = "Sign in Cancelled";
				closeLoad = true;
                return;
            }

            if(task.IsFaulted){
                Debug.Log("Sign in error: " + task.Exception);
				messageToast = "Sign in error";
				closeLoad = true;
                return;
            }

            Firebase.Auth.FirebaseUser user = task.Result;
            Debug.Log("User signed in Name:" + user.DisplayName + " Email:" + user.Email);
            
            FruitUI.userName = user.DisplayName.ToLower();
            FruitUI.email = user.Email;
            FruitUI.userID = user.UserId;
            FruitUI.photoUrl = user.PhotoUrl.ToString();
            isFB = true;
			isSecure = "true";
			isMailVerfied = true;
			
			if(!FruitUI.userID.Equals("")){
				Debug.Log("Prepping to upload profile");
				
				uploadProfile();
				
				StartCoroutine(SetImage(FruitUI.photoUrl, FindObjectOfType<FruitUI>().profileImage, true));
			}
			else
			  messageToast = signInFailed;
			
        });
        
    }
	
	public void signOut(){
		Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.SignOut();
	}
	
	
	   private void SignInGoogleFirebase(string idToken){
        
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
       
		Firebase.Auth.Credential credential =
		Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);
		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
		  if (task.IsCanceled) {
			Debug.Log("Sign in Cancelled");
			messageToast = "Sign in Cancelled";
			closeLoad = true;
			return;
		  }
		  if (task.IsFaulted) {
			Debug.Log("Sign in error: " + task.Exception);
			messageToast = "Sign in error";
			closeLoad = true;
			return;
		  }
			  
			Firebase.Auth.FirebaseUser user = task.Result;
            Debug.Log("User signed in Name:" + user.DisplayName + " Email:" + user.Email);
            
            FruitUI.userName = user.DisplayName.ToLower();
            FruitUI.email = user.Email;
            FruitUI.userID = user.UserId;
            FruitUI.photoUrl = user.PhotoUrl.ToString();
			isMailVerfied = true;
			isSecure = "true";
			
			if(!FruitUI.userID.Equals("")){
				Debug.Log("Prepping to upload profile");
				
				uploadProfile();
				
				StartCoroutine(SetImage(FruitUI.photoUrl, FindObjectOfType<FruitUI>().profileImage, true));
			}
			else
			  messageToast = signInFailed;  
		});
        
    }
	
	 public void CreateFirebaseUser(string username, string email, string password){
        
		FindObjectOfType<FruitUI>().loadPanel.SetActive(true);
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
		  if (task.IsCanceled) {
			closeLoad = true;
			//Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
			messageToast = "CreateUserWithEmailAndPasswordAsync was canceled.";
			return;
		  }
		  if (task.IsFaulted) { 
			//Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
			closeLoad = true; 
			messageToast = "The email address is invalid or has already been used by another account";
			return;
		  }

		  // Firebase user has been created.
		  Firebase.Auth.FirebaseUser user = task.Result;
		  this.user = user;
		  
		  //Debug.Log("User verification : " + user.IsEmailVerified());
		    Debug.Log("UserName: "+ username.ToLower());
            //Debug.Log("Email: "+ user.Email);
            //Debug.Log("ID: "+ user.UserId);
            
			FruitUI.userName = username.ToLower();
            FruitUI.email = user.Email;
            FruitUI.userID = user.UserId;
			FruitUI.photoUrl = "";
           
            reference.Child("usernames").Child(FruitUI.userName).Child("name").SetValueAsync(FruitUI.userName);
            
			if(!FruitUI.userID.Equals("")){
				Debug.Log("Prepping to upload profile");
				uploadProfile();
			}
			else
			  messageToast = signInFailed;
            
		});
		
	 }
	 
	  public void SignInFirebase(string email, string password){
        
		manualSignIn = true;
		FindObjectOfType<FruitUI>().loadPanel.SetActive(true);
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		
		auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
		  if (task.IsCanceled) {
			Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
			messageToast = "SignIn was cancelled";
			closeLoad = true;
			return;
		  }
		  if (task.IsFaulted) {
			closeLoad = true;			  
			Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
			messageToast = "incorrect login details";
			return;
		  }

		  Firebase.Auth.FirebaseUser user = task.Result;
		  this.user = user;
		    
		  FruitUI.email = user.Email;
          FruitUI.userID = user.UserId;
	      FruitUI.photoUrl = "";
           
		  if(!FruitUI.userID.Equals("")){
			  Debug.Log("Prepping to upload profile");
			  uploadProfile();
		  }
		  else
			  messageToast = signInFailed;
		  
		});
	 
	 }
	 
	 private void getUserToken(Firebase.Auth.FirebaseUser user, int isNew){
		 
		 user.TokenAsync(true).ContinueWith(task2 => {

			  if (task2.IsCanceled) {
				//Debug.LogError("TokenAsync was canceled.");
			   return;
			  }

			  if (task2.IsFaulted) {
				//Debug.LogError("TokenAsync encountered an error: " + task2.Exception);
				return;
			  }

			  idToken = task2.Result;
			  
			  Debug.Log("Retreived toks");
			  
			  if(isNew == 1)
				sendVerificationEmail();
			  else 
				checkVerification();
		  
		});
		
	 }
	 
	 public void resendVerification(){
		 
		 if(CheckForInternetConnection(1)){
			 sendVerificationEmail();
			 FindObjectOfType<FruitUI>().closeVerDialog();
		 }
	 }
	 
	 private async void sendVerificationEmail(){
		 
		 Debug.Log("Sending Verification Email");
		 
		  var values = new Dictionary<string, string>
			  {
				  { "requestType" , "VERIFY_EMAIL" },
				  { "idToken" , idToken }
			  };
			  
			  var content = new FormUrlEncodedContent(values);
			  
			  var response = await client.PostAsync("https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=AIzaSyB8ugh1jNKaaMjXz-mLOSwZGKu2mXEZub4",content);
			  
			  var responseString = await response.Content.ReadAsStringAsync();
			 // Debug.Log("Response : " + responseString);	

			  messageToast = "A verification link has been sent to your email";
		  
	 }
	 
	 
	 private async void checkVerification(){
		 
		 Debug.Log("Checking Verification");
		 
		  var values = new Dictionary<string, string>
			  {
				  { "idToken" , idToken }
			  };
			  
			  var content = new FormUrlEncodedContent(values);
			  
			  var response = await client.PostAsync("https://identitytoolkit.googleapis.com/v1/accounts:lookup?key=AIzaSyB8ugh1jNKaaMjXz-mLOSwZGKu2mXEZub4",content);
			  
			  var responseString = await response.Content.ReadAsStringAsync();
			 // Debug.Log("Response : " + responseString);
			  
			  //PlayerInfo playerx = PlayerInfo.createFromJson(responseString);
			  
			  string testing = responseString.Replace("\"","");
			  //Debug.Log("My Response : " + testing);
			  
			  if(testing.Contains("emailVerified: true")){
				  isMailVerfied = true;
			  }
			  
			  Debug.Log("Verified " + isMailVerfied);
		  
	 }
	 
	 public class PlayerInfo{
		 
		 public string localId;
		 public string email;
		 public string passwordHash;
		 public bool emailVerified;
		 public int passwordUpdatedAt;
		 public Dictionary<string, string> providerUserInfo;
		 public string validSince;
		 public string lastLoginAt;
	     public string createdAt;
		 public string lastRefreshAt;
		 
		 public static PlayerInfo createFromJson(string json){
			 
			 return JsonUtility.FromJson<PlayerInfo>(json);
		 }
		 
	 }
	 
	public void checkMessage(){
		if(!messageToast.Equals("")){
			FindObjectOfType<FruitUI>().showMessage(messageToast);
			messageToast = "";
			if(messageToast.Equals(signInFailed))
			{
				PlayerPrefs.SetString("email","");
				PlayerPrefs.SetString("password","");
			}
		}
        if(award){
            FindObjectOfType<FruitUI>().showFunds(awardMessage);
            FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Coins);
            award = false;                
        }
        inviteLeftText.text = inviteLeftMessage;
	} 

    void generateRefID(){

        Debug.Log("Generating New Ref ID...");
		//int ser = + UnityEngine.Random.Range(10001,99999);
		string ser = FruitUI.userID.Substring(2,5);
		
        if(me.name.Trim().Length > 5)
            me.refID = me.name.Substring(0,5).Trim() + ser;
        else
            me.refID = me.name + ser;
        
		me.refID = me.refID.ToLower().Trim();
        FruitUI.refID = me.refID;     

        coins = new ProtectedInt32(500);
                               
        Debug.Log("New Ref ID: " + me.refID);
		
		pend = 0;
		surveyPend = 0;
		cashout = 0;

        if(isFB)
            FindObjectOfType<FruitUI>().openInvitePanel();
        else if(!bonused){
            giveCash(300f, " for  registeration bonus");  
            bonused = true;
        }else
			finalizeUpdateData(null,null);

    }

    public void activateInvite()
    {
        inviteRefID = inviteText.text.ToLower().Trim();

        if(inviteRefID.Equals("")){
            FindObjectOfType<FruitUI>().showMessage("No Invite? Close the dialog");
        }
        else if(inviteRefID.Equals(FruitUI.refID))
            FindObjectOfType<FruitUI>().showMessage("You cannot use your own refferal code");
        else if(CheckForInternetConnection(1)){
            //Debug.Log("Checking for Invite ID");
            FindObjectOfType<FruitUI>().loadPanel.SetActive(true);
            reference.Child("users").OrderByChild("refID").EqualTo(inviteRefID).LimitToFirst(1)
            .GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
                    }
                    else if (task.IsCompleted) {

                        DataSnapshot snapshot = task.Result;
						
						if (snapshot.Exists){
						   Debug.Log("Invite Exists");
						   
                           foreach (var child in snapshot.Children){
							    Debug.Log("Attempting Ref");
                                string name = child.Child("name").GetValue(false).ToString();
                                string id = child.Child("id").GetValue(false).ToString();
								Debug.Log("Name: "+name);
								//Debug.Log("ID: "+id);
                                reference.Child("invites").Child(inviteRefID).Child(FruitUI.userID).Child("id").SetValueAsync(FruitUI.userID); 
                                inviteID = id;   
                               
								messageToast = "You were invited by "+name; 
                           }
						   
						   Debug.Log("Closing Ref Panel");
						   close = true;
						   close2 = true;
						   
					   }
					   else {
                           close = true;
						   close2 = false;
                           messageToast = "User not found, ensure the referral code is correct";
					   }
                       
                    }
             });
        }
    }
	
	private void closeUp(){
		close = false;
		closeLoadPanels();
		if(close2)
			FindObjectOfType<FruitUI>().closeInvitePanel();
		
	}
	
	public void uploadProfile(){
		
		if(connected){
				  
			me.id = FruitUI.userID;
			me.email = FruitUI.email;
			me.photoUrl = FruitUI.photoUrl;
			me.name = FruitUI.userName.ToLower();
			me.score = myHighScore;
			
			
			
			 reference.Child("users").Child(FruitUI.userID)
			  .GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
				   Debug.Log("No User Data");
				}
				else if (task.IsCompleted) {
					
				  DataSnapshot snapshot = task.Result;
				   
				  if(!snapshot.Exists){
					 
					 if(myCash < 5){
						 Debug.Log("New User Data"); 
						 isNew = true;
						 me.cash = 5f;
						 myCash  = new ProtectedFloat(5f);
					 }else
						 Debug.Log("Migrated User Data"); 
					 ready = true;
				  }else{
					  
					  Debug.Log("User Data found");
                      bonused = true;
					  bool proceed = true;
					  
					  try{	
						  float cash = float.Parse(snapshot.Child("cash").GetValue(false).ToString());
						  myCash = new ProtectedFloat(cash);
						  me.cash = cash;
						  Debug.Log("Ca$h: "+cash);
					  }catch(Exception e){
						 Debug.Log("User data is null, creating new Data");
						 proceed = false;
						 isNew = true;
						 bonused = false;
						 me.cash = 5f;
						 myCash  = new ProtectedFloat(5f);
						 ready = true;
					  }  
						  
					if(proceed){

						 try{
							  string isSecure = snapshot.Child("isSecure").GetValue(false).ToString();
							  if(isSecure.Equals("true")){
								  NetworkScript.isSecure = "true";
								  Debug.Log("User Account is Secured (Google/FB)");
							  }	
						  }catch(Exception e){
							  Debug.Log("Account Yet to be Secured");
						  }
						  try{		
							  me.coins = Convert.ToInt32(snapshot.Child("apples").GetValue(false).ToString());
							  coins = new ProtectedInt32(me.coins);
							  Debug.Log("Todays Apples: "+coins);
							  
						  }catch(Exception e){
							  Debug.Log("No apples located");
						  }
						    
						  try{		
							  money = new ProtectedInt32(Convert.ToInt32(snapshot.Child("money").GetValue(false).ToString()));
							  Debug.Log("Todays Cash: "+money);
						  }catch(Exception e){
							  Debug.Log("No Todays Cash located");
						  }
						  
						  try{
							  greenApples = new ProtectedInt32(Convert.ToInt32(snapshot.Child("greenXApples").GetValue(false).ToString())); 
							  Debug.Log("Green: "+greenApples);
						  }catch(Exception e){
							  Debug.Log("No green apples located");
						  }
						  
						  try{
							  sysDay = Convert.ToInt32(snapshot.Child("today").GetValue(false).ToString()); 
							  Debug.Log("Retreived Day " + sysDay);
						  }catch(Exception e){
							  Debug.Log("No previous day located");
						  }
						  
						  if(sysDay != curDay){
								  Debug.Log("New Date");
								  greenApples = new ProtectedInt32(0);
								  money = new ProtectedInt32(0);
						  }
						  try{
							  rewardDay = Convert.ToInt32(snapshot.Child("rewardDayXY").GetValue(false).ToString()); 
							  Debug.Log("Reward Day " + rewardDay);
						  }catch(Exception e){
							  Debug.Log("No reward day located");
						  }
						  
						  try{
							  watched = Convert.ToInt32(snapshot.Child("watched").GetValue(false).ToString()); 
							  Debug.Log("Watchzz " + watched);
						  }catch(Exception e){
							  Debug.Log("No watchzz");
						  }
						  
						  try{
							  string refID = snapshot.Child("refID").GetValue(false).ToString();
							  
							  FruitUI.refID = refID;
							  me.refID = refID;
							  Debug.Log("Retreived Ref ID: " + FruitUI.refID);
							  me.refID = refID;
						  }catch(Exception e){
							  Debug.Log("No Ref ID found!");
							  
							  if(myCash <200)
								bonused = false;
						  }
						  /**
						  try{
							  FruitUI.userID = "";
							  Debug.Log("MY ID - "+FruitUI.userID);
						  }catch(Exception e){
							  Debug.Log("No ID Set");
						  }
						  **/
						  try{
							  inviteID = snapshot.Child("referredBy").GetValue(false).ToString();
							  
						  }catch(Exception e){
							  Debug.Log("No Referral Set");
						  }
						  
						  try{
							  string name = snapshot.Child("name").GetValue(false).ToString();
							 
							if((name.Trim().Equals("") || name == null) && FruitUI.email.Trim().Length > 5){
								FruitUI.userName = FruitUI.email.Substring(0,5);
								Debug.Log("Setting userName to " + FruitUI.userName);
							}else{
							    FruitUI.userName = name.ToLower();
								Debug.Log("Retreived Name: "+FruitUI.userName);
							}
							me.name = FruitUI.userName;
						  }catch(Exception e){
							  Debug.Log("No userName located");
						  }
						  
						  try{
							  pend = float.Parse(snapshot.Child("pendingCash").GetValue(false).ToString());
							  Debug.Log("Pend: "+pend);
							  if(pend == 0)
								pend = 5;
							
						  }catch(Exception e){
							  Debug.Log("No pends located");
						  }
						  
						  try{
							  surveyPend = float.Parse(snapshot.Child("pendSurvey").GetValue(false).ToString());
							  Debug.Log("surveyPend: "+surveyPend);
							  if(surveyPend == 0)
								  surveyPend = 5;
						  }catch(Exception e){
							  Debug.Log("No surveyPend located");
						  }
						  
						//   try{
						// 	  prem = snapshot.Child("prem").GetValue(false).ToString();
							  
						//   }catch(Exception e){
						// 	  Debug.Log("No purch made");
						//   }
						  
						  try{
							  b_strike = Convert.ToInt32(snapshot.Child("b_strike").GetValue(false).ToString());
							  if(b_strike == curDay || b_strike == yesterday)
								  blocked = true;
							  Debug.Log("Strike: "+blocked);
						  }catch(Exception e){
							  Debug.Log("No Strike");
						  }
						  
						  try{
							approved = snapshot.Child("coins").GetValue(false).ToString();
							Debug.Log("Appv: "+approved);
						  }catch(Exception e){
							  Debug.Log("No Appv located");
						  }
						  
						  try{
							  cashout = float.Parse(snapshot.Child("cashx_out").GetValue(false).ToString());
							  Debug.Log("Cashout: "+cashout);
						  }catch(Exception e){
							  Debug.Log("No withdraw history");
						  }
						  
						  int highScore = Convert.ToInt32(snapshot.Child("score").GetValue(false).ToString());
						  int games = Convert.ToInt32(snapshot.Child("games").GetValue(false).ToString());
						  
						  Debug.Log("Score: "+highScore);
						  Debug.Log("Games Played Database: "+games);
						  
						  
						  myGames = FruitSpearGameCode.gamesPlayed + games;
						  me.games = myGames;
						  FruitSpearGameCode.gamesPlayed = 0;
						  Debug.Log("Games Played Updated Database: "+myGames);
						  
						  if(highScore > me.score){
								me.score = highScore;
								myHighScore = highScore;
						  }else
							  myHighScore = me.score;
						  
						  ready = true;
					}    	
					  
				  }
				  
				  loggenIn = true;
				}
			  });
		}else
			Debug.Log("User Offline Cant upload Profile");
		
	}
	public void botStrike(){
		b_strike = curDay;
		blocked = true;
		updateApples();
	}

    public void updateApples(){
        
        StartCoroutine(CheckConnection(1));
		Debug.Log("Updating Apples yassss");
    }
	
	private void finalizeUpdateData(string childDate, string info){
				
		if(childDate == null || info == null){
			childDate = System.DateTime.Now.ToString("yyyy-MM-dd") + " (Token)";
			info = money + " Cash Tokens collected. Bal: "+myCash;
		}
		
		if(isMigrate){
			isMigrate = false;
			info = transactJsonString;
			childDate = tournJsonString;
			
			if(!inviteJsonString.Equals(""))
				updateInvites(inviteJsonString);
		}
				
		if(!FruitUI.userID.Equals("") && myCash > 0){

			int tempCash = 0;

			StartCoroutine(GetTotalAmountOfCashFromTransactionHistory((myReturnValue) => {
				tempCash = myReturnValue;

                if (tempCash != -1)
                {
					ProtectedFloat temp = tempCash + money;

					if (temp != myCash)
                    {
                        if (temp < myCash)
                        {
							Debug.Log("total balance is greater in amount than in transaction history, syncing...");
							myCash = temp;
                        }
                    }
                    else
                    {
                        Debug.Log("transactions records and total balance is synced!");
                    }
                }

				updateDataFunction(childDate, info).ContinueWith((task) => {
					if (task.IsFaulted)
					{
						foreach (var inner in task.Exception.InnerExceptions)
						{
							if (inner is FunctionsException)
							{
								var e = (FunctionsException)inner;
								var code = e.ErrorCode;
								//var message = e.ErrorMessage;
								Debug.Log("Update Data Function Error: ");
							}
						}
					}
					else
					{
						string result = task.Result;
						Debug.Log("Update Data Function Success: " + result);
					}
				});


				PlayerPrefs.SetInt("green", 1);
				Debug.Log("Finished Deploying Update Data Function");
			}));
		
        }else
			checkError();	
	}
	
	private Task<string> updateDataFunction(string childDate, string info) {
	  
	  var functions = FirebaseFunctions.DefaultInstance;
	  string date = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
			
	  Debug.Log("Calling Function - Update Data");
	  var data = new Dictionary<string, object>();

	  float a = myCash;
	  int b = money;
	  int c = coins;
	  int d = greenApples;
	  
	  data["name"] = FruitUI.userName;
	  data["apples"] = c;
	  data["greenApples"] = d;
	  data["cash"] = a;
	  data["money"] = b;
	  data["refID"] = me.refID;
	  data["referredBy"] = inviteID;
	  data["score"] = me.score;
	  data["games"] = me.games;
	  data["today"] = curDay;
	  data["rewardDayXY"] = rewardDay;
	  data["country"] = myCountry;
	  data["b_strike"] = b_strike;
	  data["watched"] = watched;
	  data["security"] = 2;
	  data["prem"] = prem;
	  data["pendingCash"] = pend;
	  data["pendSurvey"] = surveyPend;
	  data["cashx_out"] = cashout;
	  data["login_id"] = login_id;
	  data["isSecure"] = isSecure;
	  
	  data["childDate"] = childDate;
	  data["date"] = date;
	  data["info"] = info;
	  data["info2"] = accountDet;
		
	  if(pend == 0)
		pend = 5;
	if(surveyPend == 0)
		surveyPend = 5;
	
	  var function = functions.GetHttpsCallable("uploadData");
	  return function.CallAsync(data).ContinueWith((task) => {
		Debug.Log("Result: " + (string)task.Result.Data);
		return (string) task.Result.Data;
	  });
	}

	public static IEnumerator GetTotalAmountOfCashFromTransactionHistory(System.Action<int> callback)
	{

		int cashSumFromTransactionHistory = 0;

		var DBTask = reference.Child("users").Child(FruitUI.userID).Child("transactions").GetValueAsync();

		yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

		if (DBTask.Exception != null)
		{
			Debug.Log("Error retreiving Transaction List");
			callback(-1);
		}
		else
		{
			DataSnapshot snapshot = DBTask.Result;

			if (snapshot.Exists)
			{

				//Debug.Log("There are " + snapshot.ChildrenCount + " previous Transactions");

				foreach (var child in snapshot.Children)
				{
					string info = child.Child("info").GetValue(false).ToString();

					string[] split = info.Split(' ');

					if (info.Contains("Withdrawal"))
					{
						int cashFromCurrentTransaction = int.Parse(split[2]);

						cashSumFromTransactionHistory -= cashFromCurrentTransaction;
					}
					else
					{
						int cashFromCurrentTransaction = int.Parse(split[0]);

						cashSumFromTransactionHistory += cashFromCurrentTransaction;
					}
				}

				callback(cashSumFromTransactionHistory);

			}
            else
            {
				callback(0);
			}

        }

	}

	private void updateTournament(string id, string status, int plays){
		updateTournamentFunction(id, status, plays).ContinueWith((task) => {
			if (task.IsFaulted) {
				foreach (var inner in task.Exception.InnerExceptions) {
					if (inner is FunctionsException) {
						var e = (FunctionsException) inner;
						var code = e.ErrorCode;
						//var message = e.ErrorMessage;
						Debug.Log("Update Tournament Function Error: ");
					}
				}
			} else {
				string result = task.Result;
				Debug.Log("Update Tournament Function Success: " + result);
			}
			});
	}
	
	private Task<string> updateTournamentFunction(string id, string status, int plays) {
	  
	  var functions = FirebaseFunctions.DefaultInstance;
	  string date = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
			
	  Debug.Log("Calling Function - Update Tournaments");
	  var data = new Dictionary<string, object>();
	  
	  data["id"] = id;
	  data["stat"] = status;
	  data["plays"] = plays;
	  data["date"] = date;

	  var function = functions.GetHttpsCallable("uploadTournaments");
	  return function.CallAsync(data).ContinueWith((task) => {
		Debug.Log("Result: " + (string)task.Result.Data);
		return (string) task.Result.Data;
	  });
	}
	
	private void updateInvites(string jsonList){
		updateInviteFunction(jsonList).ContinueWith((task) => {
			if (task.IsFaulted) {
				foreach (var inner in task.Exception.InnerExceptions) {
					if (inner is FunctionsException) {
						var e = (FunctionsException) inner;
						var code = e.ErrorCode;
						//var message = e.ErrorMessage;
						Debug.Log("Update Invite Function Error: ");
					}
				}
			} else {
				string result = task.Result;
				Debug.Log("Update Invite Function Success: " + result);
			}
			});
	}
	
	private Task<string> updateInviteFunction(string jsonList) {
	  
	  var functions = FirebaseFunctions.DefaultInstance;
	  	
	  Debug.Log("Calling Function - Update Invites");
	  var data = new Dictionary<string, object>();
	  
	  if(jsonList == null){
		  jsonList = "{";
		   
		  for (int i=0; i<invites.Count; i++){
			  
			  PlayerInvite inv = new PlayerInvite(invites[i].id);
			  string json = JsonUtility.ToJson(inv);
			  
			  string newChild = "\""+invites[i].id+"\"" + ":" + json;
			  string end = ",";
			  if(i == invites.Count-1)
				end = "}";
			  
			  jsonList += newChild + end;
			  //Debug.Log(json);
		  }
	  }
		  
	  data["json"] = jsonList;  
	  //Debug.Log(jsonList);
	
	  var function = functions.GetHttpsCallable("uploadInvites");
	  return function.CallAsync(data).ContinueWith((task) => {
		Debug.Log("Result: " + (string)task.Result.Data);
		return (string) task.Result.Data;
	  });
	  
	}
	
	public class PlayerInvite{
		 
		public string id;
		 
		 public PlayerInvite(string uid){
			 id = uid;
		 }
	}
	

    void closeLoadPanels(){
        FindObjectOfType<FruitUI>().loadPanel.SetActive(false);
        FindObjectOfType<FruitUI>().loadPanel1.SetActive(false);
    }
	
	public void finalizeLogin(){

		if(loggenIn){
			
            loggenIn = false;
			closeSignUpDialog();
			if(!signed)
			    FindObjectOfType<FruitUI>().showMessage(FruitUI.userName + " Signed In Successfully");
            signed = true;    
			PlayerPrefs.SetString("email",email);
		    PlayerPrefs.SetString("password",password);
			if(FruitUI.consent.Equals("yes"))
				PlayerPrefs.SetString("consent", FruitUI.consent);
            closeLoadPanels();
			Strings.setLanguage();
			
			FindObjectOfType<FruitSpearGameCode>().initPollfish();
			
			if(myGames > 1000 && myHighScore < 100)
				blocked = true;
			
			if(!blocked)
				b_strike = 0;
			
			FindObjectOfType<FruitUI>().profileName.text = FruitUI.userName;
			 
            if(isNew){
				
				FruitSpearGameCode.isAd = true;
				
				if(isFB && !invited)
					invited = true;
				/**
				if(!isFB){
					
					try{
						getUserToken(user, 1);
					}catch(Exception e){
						Debug.Log("Unable to retreive user Toks");
					}
				}
				**/
                
				SpinScripct.spins++;
                SpinScripct.maxSpins = 20;
                PlayerPrefs.SetInt("max_spins",SpinScripct.maxSpins);
            
            }else{
				/**
				if(!isFB){
					
					try{
						getUserToken(user, 0);
					}catch(Exception e){
						Debug.Log("Unable to retreive user T");
					}
				}
				**/

                if(sysDay != curDay){
                    SpinScripct.spins++;
                    SpinScripct.maxSpins = 20;
                    PlayerPrefs.SetInt("max_spins",SpinScripct.maxSpins);
                    FindObjectOfType<FruitUI>().openFreeSpinDialog();
					minApples = 0;
					minTodaysScore = 0;
					minBid1 = 0;
					PlayerPrefs.SetInt("minTodaysScore",0);
					PlayerPrefs.SetInt("minApples",0);
					PlayerPrefs.SetInt("minBid1",0);
		
                }else{
                    SpinScripct.maxSpins = PlayerPrefs.GetInt("max_spins");
                }
            }
			
			// if(prem.Equals("true"))
			// 	FindObjectOfType<FruitUI>().activatePrem();
			
			FindObjectOfType<FruitSpearGameCode>().setBest();
            FindObjectOfType<FruitSpearGameCode>().showBannerAd();      
			checkError();
              
		}

         if(ready){
            
			ready = false;
			login_id = "log_"+UnityEngine.Random.Range(10001,99999);
            	
            if(green.Equals("true")){
                FruitSpearGameCode.isGreen = true;
                Debug.Log("Green is true");
                if(sysDay != curDay && !isNew)
                    FindObjectOfType<FruitUI>().showYesterdaysApples();
                else{
                    FruitUI.yesGreen();
                    FindObjectOfType<FruitUI>().leaderboardHeadText.text = "TODAYS LEADERBOARD";
                }       
            }else{
                FruitSpearGameCode.isGreen = false;
                Debug.Log("Green is false");
                FruitUI.noGreen();
                FindObjectOfType<FruitUI>().leaderboardHeadText.text = "TODAYS HIGHSCORES";
            }
			
			if(me.refID == null || me.refID.Trim().Equals(""))
                generateRefID();
            else{
                Debug.Log("No need to generate Ref ID: "+me.refID);
				checkPend();
            }  
            
			addLoginIDListener();
			addTopic();
			getTournaments();	
         } 

	}
	
	private void addTopic(){
		Debug.Log("Checking User FCM Subscription");
		if(PlayerPrefs.GetString("fcm") == null || PlayerPrefs.GetString("fcm").Equals(""))
			SuscribeNotif("players");
		else if(PlayerPrefs.GetString("fcm").Equals("on"))
			isNotif = true;
		Debug.Log("Finished Checking User FCM Subscription");
	}
	
	public void checkPend(){
		if(pend > 10){
			float amount = pend;
			pend = 0;
			giveCash(amount, " for winning competition");	
		}else
			checkSurveyPend();
	}
	
	public void checkSurveyPend(){
		if(!surveyChecked){
			surveyChecked = true;
			if(surveyPend > 10){
				float amount = surveyPend;
				surveyPend = 0;
				giveCash(amount, " for completing Survey");	
			}else
				finalizeUpdateData(null,null);
		}
	}
	
	public void checkError(){
		
		if(!chekkk){
			
			Debug.Log("Checking for Error");
			chekkk = true;
			
			if(!isNew){
				if((myCash == 0 || FruitUI.userName.Trim().Equals(""))){
					
					FindObjectOfType<FruitUI>().showMessageDialog("ERROR!!!\n\nData Did not load correctly\nRestart the App!\n\nContact Us if problem persists");
					
					string childDate = DateTime.UtcNow.Year + "-" + DateTime.UtcNow.Month.ToString("00") + "-" + DateTime.UtcNow.Day.ToString("00");
					
					reference.Child("crashes").Child(childDate).Child(FruitUI.userID).Child("id").SetValueAsync(FruitUI.userID);
					reference.Child("crashes").Child(childDate).Child(FruitUI.userID).Child("time").SetValueAsync(System.DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"));
					reference.Child("crashes").Child(childDate).Child(FruitUI.userID).Child("status").SetValueAsync("pending");
					
				}else if(sysDay != curDay){
					
						rewardDay ++;
						Debug.Log("Get Daily Reward "+rewardDay);
						FindObjectOfType<FruitSpearGameCode>().giveDailyReward();
						//FindObjectOfType<Notification>().scheduleNotification();
					
				}
				
			}else{
				rewardDay = 1;
				Debug.Log("Get Daily Reward "+rewardDay);
				FindObjectOfType<FruitSpearGameCode>().giveDailyReward();
				//FindObjectOfType<Notification>().scheduleNotification();
			}
			
		}	
	}
	
	public void addLoginIDListener()
    {
		Debug.Log("Adding Login ID Listener");
       reference.Child("users").Child(FruitUI.userID).Child("login_id")
	   .ValueChanged += HandleLoginIDChanged;
		
    }
	
	 private void HandleLoginIDChanged(object sender, ValueChangedEventArgs args) {
		 
		 	 
		if(args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
			DataSnapshot snapshot = args.Snapshot;
				if (snapshot.Exists){
					string latestLogin = snapshot.Child("id").GetValue(false).ToString();
					//Debug.Log("Latest Login - "+latestLogin);
					if(login_id.Equals(latestLogin)){
						Debug.Log("Auth Verified");
						validAuth = true;
					}else{
						if(validAuth){
							FindObjectOfType<FruitUI>().openExpiredPanel();
							Debug.Log("Session Expired");
						}else
						{
							Debug.Log("Auth Not Yet Verified");
						}
					}
				}
				else {
					//Debug.Log("No Previous Login Exists");
				}	   
    }
	
	 public void toggleNotifications(){
        
        if(isNotif){
			UnscribeNotif("players");
            notifImage.texture = (Texture2D)Resources.Load("check_off");
        }else{
            SuscribeNotif("players");
            notifImage.texture = (Texture2D)Resources.Load("check_on");
        }
        isNotif = !isNotif;
        
    }

	public void openProfile(){
		
        FindObjectOfType<FruitUI>().playButtonSound();

			
			if(!FruitUI.userID.Trim().Equals("")){
					
				profileDialog.SetActive(true);
				
				if(isMailVerfied)
					FindObjectOfType<FruitUI>().setBadge();
			
				cashText.text = ""+myCash;
				versionText.text = "VERSION "+(double)FruitUI.version/10;
                curImage.texture = (Texture2D)Resources.Load(FruitUI.currency);
				
				if(isNotif)
					notifImage.texture = (Texture2D)Resources.Load("check_on");
				
				if(FruitUI.avatar!=null)
					avatar.texture = FruitUI.avatar;
				Debug.Log("My score1 "+ myHighScore);
				Debug.Log("My score2 "+ me.score);
				info.text = "Name: " + me.name +
				"\nEmail: " + me.email +
				"\nGames Played: " + myGames +
				"\nTop Score: " + me.score;
				
				 Debug.Log("Wallet: "+myCash);
				 Debug.Log("Total Games Played: "+myGames);
				 
			}else{
				loginText.text = "Log Into Your Account";
                FindObjectOfType<FruitUI>().userNamePanel.SetActive(true);
			}
		
	}

	public void updateDatabase(){
		
		if(FruitUI.userID != null)
			if(!FruitUI.userID.Equals("")){
				
				myGames += FruitSpearGameCode.gamesPlayed ;
				me.games = myGames;
				FruitSpearGameCode.gamesPlayed = 0;
				//Debug.Log("Games Played Updated Database: "+myGames);
							  
				if(myGames >=5 && !levelUp)
					levelUpEvent();

				me.score = FruitSpearGameCode.highScore;
							  
				if(myHighScore > me.score){
					me.score = myHighScore;
				}else
					myHighScore = me.score;
							  
				finalizeUpdateData(null,null);	 
				
			
			}
		
	}

	private void levelUpEvent(){
		Firebase.Analytics.FirebaseAnalytics.LogEvent(
			Firebase.Analytics.FirebaseAnalytics.EventLevelUp,
			new Firebase.Analytics.Parameter[] {
				new Firebase.Analytics.Parameter(
				Firebase.Analytics.FirebaseAnalytics.ParameterCharacter, "player"),
				new Firebase.Analytics.Parameter(
				Firebase.Analytics.FirebaseAnalytics.ParameterLevel, myGames),
			}
		);
		PlayerPrefs.SetInt("level",1);
		levelUp = true;
		Debug.Log("Sending Event levelUp");
	}
	
	public void closeProfile(){
        FindObjectOfType<FruitUI>().playButtonSound();
		profileDialog.SetActive(false);
		FindObjectOfType<FruitUI>().closeVerDialog();
	}
    
	
     public IEnumerator SetImage(string URLstring, RawImage image, bool profile)
    {
			System.Uri url = new System.Uri(URLstring);

			using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
			{
				yield return uwr.SendWebRequest();

				if (uwr.isNetworkError || uwr.isHttpError)
				{
					Debug.Log(uwr.error);
				}
				else
				{
					// Get downloaded asset bundle
					image.texture = DownloadHandlerTexture.GetContent(uwr);
					if(profile)
						FruitUI.avatar = DownloadHandlerTexture.GetContent(uwr);
					
				}
			}
    }
	
	public void openSignUpDialog(){
        FindObjectOfType<FruitUI>().playButtonSound();
		SignUpDialog.SetActive(true);
		LoginDialog.SetActive(false);
	}
	public void openLogInDialog(){
        FindObjectOfType<FruitUI>().playButtonSound();
		CheckForInternetConnection(1);
		LoginDialog.SetActive(true);
	}
	public void closeSignUpDialog(){
        FindObjectOfType<FruitUI>().playButtonSound();
		if(!closedDialogs){
			SignUpDialog.SetActive(false);
			LoginDialog.SetActive(false);
			FindObjectOfType<FruitUI>().userNamePanel.SetActive(false);
			closedDialogs = true;
		}
	}
	public void closeSignUpDialogAlone(){
        FindObjectOfType<FruitUI>().playButtonSound();
		SignUpDialog.SetActive(false);
	}
	public void closeLogInDialogAlone(){
		LoginDialog.SetActive(false);
	}
	public void checkLoad(){
		if(closeLoad){
			closeLoadPanels();
			closeLoad = false;
		}
	}
	
    void setupFireBase()
    {
      
#if UNITY_ANDROID

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                var app = Firebase.FirebaseApp.DefaultInstance;
				initialiseFirebase = true;
                new UnityEvent().Invoke();
				Debug.Log("Firebase Dependency valid");
				
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
				  Debug.Log("Firebase Dependency invalid");
            }
        });

#endif
				
    }
	
	void initFirebase(){
		
#if UNITY_EDITOR 
		FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false); 
#endif
		reference = FirebaseDatabase.GetInstance("https://fruit-spear-4de98.firebaseio.com/").RootReference;
		referenceList = FirebaseDatabase.GetInstance("https://players-cb43d8x.firebaseio.com/").RootReference;
				
				reference.Child("info")
                .GetValueAsync().ContinueWith(task => {
                    if (task.IsFaulted) {
						Debug.Log("Failed to Fetch Info");
                    }
                    else if (task.IsCompleted) {

                        DataSnapshot snapshot = task.Result;
                        message = snapshot.Child("message").GetValue(false).ToString();
						Debug.Log("Server Day- " +snapshot.Child("day").GetValue(false).ToString());
						curDay = Convert.ToInt32(snapshot.Child("day").GetValue(false).ToString());
						month = Convert.ToInt32(snapshot.Child("month").GetValue(false).ToString());
                        int ver = Convert.ToInt32(snapshot.Child("version").GetValue(false).ToString());
                        green = snapshot.Child("green").GetValue(false).ToString();
						string cashL = snapshot.Child("cash_leaderboard").GetValue(false).ToString();
                        rules2 = snapshot.Child("rules2").GetValue(false).ToString();
                        notif = snapshot.Child("notif").GetValue(false).ToString();
                        unityAd = snapshot.Child("unity").GetValue(false).ToString().ToLower();
						inviteReward = float.Parse(snapshot.Child("invite_reward").GetValue(false).ToString());
						paydate = float.Parse(snapshot.Child("paydate").GetValue(false).ToString());
                        gold =  float.Parse(snapshot.Child("gold").GetValue(false).ToString());
                        silver =  float.Parse(snapshot.Child("silver").GetValue(false).ToString());
                        bronze =  float.Parse(snapshot.Child("bronze").GetValue(false).ToString());
						extra1 =  float.Parse(snapshot.Child("extra1").GetValue(false).ToString());
						extra2 =  float.Parse(snapshot.Child("extra2").GetValue(false).ToString());
                        ListViewScript.gold = Convert.ToInt32(snapshot.Child("win_gold").GetValue(false).ToString());
                        ListViewScript.silver = Convert.ToInt32(snapshot.Child("win_silver").GetValue(false).ToString());
                        ListViewScript.bronze = Convert.ToInt32(snapshot.Child("win_bronze").GetValue(false).ToString());
						ListViewScript.extra1 = Convert.ToInt32(snapshot.Child("win_extra1").GetValue(false).ToString());
						ListViewScript.extra2 = Convert.ToInt32(snapshot.Child("win_extra2").GetValue(false).ToString());
                        ListViewScript.minimum = Convert.ToInt32(snapshot.Child("minimum").GetValue(false).ToString());

						if(cashL.Equals("true"))
							isCashLeaderboard = true;
						
						if(invReward > 0)
							inviteReward += 100f;
                        
                        Debug.Log("Notif: "+notif);
                        Debug.Log("Message: "+message);
                        Debug.Log("Minimum Score: "+ListViewScript.minimum);
                        Debug.Log("Gold : "+ListViewScript.gold);
                        Debug.Log("Silver : "+ListViewScript.silver);
                        Debug.Log("Bronze : "+ListViewScript.bronze);
						Debug.Log("PayDate : "+paydate);
                        Debug.Log("Unity Ads: "+unityAd);
						
                        Debug.Log("Top Earners: "+cashL);
						//Debug.Log("Invite Reward: "+inviteReward);
                        Debug.Log("Current Version: "+FruitUI.version+" Latest Version: "+ver);
						Debug.Log("Current Day " + curDay);
						
                        FruitUI.newVersion = ver;
						
						firebaseReady = true;
                    }
                });

		
	}
	
    void Update()
    {
         if (Time.time - lastUpdate >= 0.5f)
        {
            lastUpdate = Time.time;
            //Debug.Log("Update");

            tournamentSize.text = tournCount+"";
            
			if(close)
				closeUp();
			
			if(proceedLogin){
				proceedLogin = false;
				SignInPostMigration();
			}else if(proceedMigrate){
				proceedMigrate = false;
				migrateData();
			}
			if(firebaseReady){
				firebaseReady = false;
				getDate();
				checkLoginStatus();
			}
			if(initialiseFirebase){
				initialiseFirebase = false;
				initFirebase();
			}
			
			if(startLoad){
				startLoad = false;
				FindObjectOfType<FruitUI>().loadPanel.SetActive(true);
			}
			
            if(state.Equals("check"))
                checkForNewInvites();

            if(state.Equals("finalize"))
                finalizeInvites();    
			if(available.Equals("yes")){
				FindObjectOfType<SignUpScript>().proceedSignUp();
				available = "used";
			}
            if(myTournamentsReady)
                finalizeTournamentRetreive();   

            if(winnerReady)
                {
                    FruitUI.selectedTournament.showWinnerFirst();
                    winnerReady = false;
                }    
        }
    }

    public void joinTournament(string id){
		
		//Debug.Log("Joining Tourn " + id);

        if(FruitSpearGameCode.gameType != FruitSpearGameCode.GameType.Bid)
            FruitUI.selectedTournament.plays++;
       	
		updateTournament(id, "none", FruitUI.selectedTournament.plays);
            
        if(FruitSpearGameCode.gameType == FruitSpearGameCode.GameType.Bid){
			Debug.Log("Checking Minimum Bid: "+ minBid1);
			
			if(FruitUI.selectedTournament.plays > minBid1)
				StartCoroutine(uploadBid(id));
			else
				Debug.Log("Not up to Minimum Bid, Wont Bother Posting ");
			
            updateBids(id);
			SuscribeNotif(id);
			
			PlayerPrefs.SetInt("minBid1",minBid1);
            FindObjectOfType<FruitUI>().showMessage("You Bid "+FruitUI.bidAmount);
        }

    }
	
	public void retreiveLeaderboardData(string id, bool ready)
    {
        users.Clear();
        leaderboardReady = false;
        tournLeaderboardReady = false;
        leaderboardEmpty = false;

        if(id==null){
            if(FruitUI.thisLeaderboard == FruitUI.Leaderboard.All_Time_Scores)
                reference2 = referenceList.Child("leaderboard");
            else if(FruitUI.thisLeaderboard == FruitUI.Leaderboard.Todays_Scores)
                reference2 = referenceList.Child("leaderboards").Child("leaderboard_day_"+curDay);
			else if(FruitUI.thisLeaderboard == FruitUI.Leaderboard.Top_Cashouts)
				reference2 = referenceList.Child("leaderboard_cashhh");
            else if(FruitUI.thisLeaderboard == FruitUI.Leaderboard.Yesterdays_Apples)
                reference2 = referenceList.Child("leaderboards").Child("apple_leaderboard_day_"+yesterday);
            else   
                reference2 = referenceList.Child("leaderboards").Child("apple_leaderboard_day_"+curDay);
        }
        else    
            reference2 = referenceList.Child("competitors").Child(id);

        reference2.GetValueAsync().ContinueWith(task => {
               if (task.IsFaulted)
               {
                   Debug.Log("Error retreiving Spawn objects");
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;

                   if(!snapshot.Exists && !ready){
                       Debug.Log("No previous players, starting game...");
                       FruitUI.startGame = true;
                   }
                   int count = (int)snapshot.ChildrenCount;
                   Debug.Log("There are " + count + " Users on the leaderboard");

                   for (int i = 0; i < count; i++)
                   {
                       string nameX = snapshot.Child(i + "").Child("userName").GetValue(false).ToString();
                       string idX = snapshot.Child(i + "").Child("id").GetValue(false).ToString();
                       string photoX = snapshot.Child(i + "").Child("photo").GetValue(false).ToString();
                       int scoreX = Convert.ToInt32(snapshot.Child(i + "").Child("score").GetValue(false).ToString());
                       //Debug.Log("Name: "+nameX +" ID: "+idX);
                       users.Add(new User(idX, nameX, "email", photoX, scoreX));
                       
                       if(i == count-1 && ready){

                            if(id==null){
                                leaderboardReady = true;
                                Debug.Log("Leaderboard is Ready");
                            }
                            else{    
                                    tournLeaderboardReady = true;
                                    Debug.Log("tournament Leaderboard is Ready");
                                }
                        }else if(i == count-1){
                            Debug.Log("Top Score Ready");
                            users = users.OrderByDescending(j => j.score).ToList();
                            FruitSpearGameCode.topScore = users[0].score;
                            FruitUI.startGame = true;
                        }
                   }
                if(count==0 && id==null && ready){
                    leaderboardEmpty = true;
                    users.Add(new User("", "No Users On This List Yet", "", "",0));
                    leaderboardReady = true;
                    Debug.Log("Leaderboard is Ready");
                }

               }
           });
		   
		   if(curDay != System.DateTime.UtcNow.Day)
				FindObjectOfType<FruitUI>().showMessage("Restart game to reset for New Day");
    }

    public void retreivePlayers(Tournament t)
    {
       t.ready = false; 
       t.curPlayers.Clear();

        referenceList.Child("competitors").Child(t.id).GetValueAsync().ContinueWith(task => {
               if (task.IsFaulted)
               {
                   Debug.Log("Error retreiving Spawn objects");
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;

                   if(!snapshot.Exists ){
                       Debug.Log("No previous players in tournament");
                       t.ready = true;
                   }else{
                    int count = (int)snapshot.ChildrenCount;
                    for(int i=0; i<count; i++){
                        t.curPlayers.Add("Player"+i); 
                        if(i >= count-1)
                            t.ready = true;
                    }
                   }
               }
           });
    }
	

    public void retreiveTransactionHistory()
    {		
		transactions.Clear();
        transactionReady = false;
        isEmpty = false;
        int count=0;
		
			reference.Child("users").Child(FruitUI.userID).Child("transactions").GetValueAsync().ContinueWith(task => {
               if (task.IsFaulted)
               {
                   Debug.Log("Error retreiving Transaction List");
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;

				   if(snapshot.Exists){
                       
                        Debug.Log("There are " + snapshot.ChildrenCount + " previous Transactions");

                        foreach (var child in snapshot.Children)
                        {
                            string date = child.Child("date").GetValue(false).ToString();
                            string info = child.Child("info").GetValue(false).ToString();
                            
                            transactions.Add("Date: "+date+"\nTxn: "+info);
                            //Debug.Log("Date: "+date+"\nTxn: "+info);
                            count++;
                            if(count >= snapshot.ChildrenCount){
								transactions.Reverse();
                                transactionReady = true;
                                Debug.Log("Transaction List ready to proceed");
                            }
                        }

                   }else{
                       noTrans();
                   }
	  
                }
        });

    }
	
	void noTrans(){
		Debug.Log("You have no previous transactions");
        transactions.Add("You have no previous transactions");
        transactionReady = true;
        isEmpty = true;
	}

    public void retreiveTournaments()
    {
        myTournaments.Clear();
        myPlays.Clear();
        myTournamentsReady = false;
        int count1=0;

        reference.Child("users").Child(FruitUI.userID).Child("tournaments")
        .GetValueAsync().ContinueWith(task1 => {
               if (task1.IsFaulted)
               {
                   Debug.Log("Error retreiving My Tournaments");
               }
               else if (task1.IsCompleted)
               {
                   DataSnapshot snapshot1 = task1.Result;

                   if(snapshot1.Exists){
                       
                        Debug.Log("User has " + snapshot1.ChildrenCount + " previous tournaments");

                        foreach (var child in snapshot1.Children)
                        {
                            string id = child.Child("id").GetValue(false).ToString();
                            string status = child.Child("status").GetValue(false).ToString();
                            int pl = Convert.ToInt32(child.Child("plays").GetValue(false).ToString());

                            if(status.Equals("won")){
                                pl = 10;
                                Debug.Log("User has previously won");
                            }
                            myTournaments.Add(id);
                            myPlays.Add(pl);

                            count1++;
                            if(count1 >= snapshot1.ChildrenCount){
                                myTournamentsReady = true;
                                Debug.Log("My Tournaments List ready");
                            }
                        }

                   }else{
                      Debug.Log("No previous Tournaments");
                      myTournamentsReady = true;
                   }

               }
           });
        

        
    }

    public void finalizeTournamentRetreive(){

        tournaments.Clear();
        tournamentReady = false;
        myTournamentsReady = false;
        int count=0;
        tournCount = 0;
        isEmpty = false;

            reference.Child("tournaments")
           .GetValueAsync().ContinueWith(task => {
               if (task.IsFaulted)
               {
                   Debug.Log("Error retreiving Tournaments");
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;

                   if(snapshot.Exists){
                       
                        Debug.Log("There are " + snapshot.ChildrenCount + " available tournaments");
                        
                        foreach (var child in snapshot.Children)
                        {
                            try{
                                string name = child.Child("name").GetValue(false).ToString();
                                string id = child.Child("id").GetValue(false).ToString();
                                string type = child.Child("type").GetValue(false).ToString();
                                string info = child.Child("info").GetValue(false).ToString();
                                string goal = child.Child("goal").GetValue(false).ToString();
                                string valid = child.Child("valid").GetValue(false).ToString();
                                int fee = Convert.ToInt32(child.Child("fee").GetValue(false).ToString());
                                float prize = float.Parse(child.Child("prize").GetValue(false).ToString());
                                int day = Convert.ToInt32(child.Child("day").GetValue(false).ToString());
                                int hour = Convert.ToInt32(child.Child("hour").GetValue(false).ToString());
                                
                                if(valid.Equals("true")){

                                    Debug.Log("Tournament is valid");    
                                    Tournament tournament = new Tournament(name,id,type,info,goal,fee,prize,day,hour);
                                    tournaments.Add(tournament);
                                    if(myTournaments.Contains(id)){

                                        if(type.Equals("top"))
                                            tournament.used = true;
                                        else{    
                                            int index = myTournaments.IndexOf(id);
                                            tournament.plays = myPlays[index];
                                            if(tournament.plays >= 3 && !type.Equals("bid"))
                                                tournament.used = true;
                                        }
                                    }

                                    //Debug.Log("Tournament: "+tournament.name);
                                    switch(type){

                                        case "bid":
											tournament.winner1ID = child.Child("winner1").GetValue(false).ToString();
											tournament.winner2ID = child.Child("winner2").GetValue(false).ToString();
											tournament.winner3ID = child.Child("winner3").GetValue(false).ToString();
                                         break;
                                        case "top":
                                            tournament.max = Convert.ToInt32(child.Child("max").GetValue(false).ToString());
                                        break;
                                        case "target":
                                            tournament.amount = Convert.ToInt32(child.Child("amount").GetValue(false).ToString());
                                            tournament.shots = Convert.ToInt32(child.Child("shots").GetValue(false).ToString());
                                            tournament.spears = Convert.ToInt32(child.Child("decoys").GetValue(false).ToString());
                                            tournament.speed = float.Parse(child.Child("speed").GetValue(false).ToString());
                                        break;
                                        case "time":
                                            tournament.time = Convert.ToInt32(child.Child("time").GetValue(false).ToString());
                                            tournament.shots = Convert.ToInt32(child.Child("shots").GetValue(false).ToString());
                                            tournament.spears = Convert.ToInt32(child.Child("decoys").GetValue(false).ToString());
                                        break;
                                    }

                                    tournCount++;
                                }else{
                                    Debug.Log("Tournament is not valid");
                                }

                                count++;
                                if(count >= snapshot.ChildrenCount){

                                    if(tournaments.Count > 0){
                                        tournamentReady = true;
                                        Debug.Log("Tournaments List ready to proceed");
                                    }else{
                                        Debug.Log("There are no available tournaments");
                                        transactions.Clear();
                                        transactions.Add("There are no available tournaments at the moment.");
                                        transactionReady = true;
                                        isEmpty = true;
                                    }
                                }
                            }catch(Exception e){
                                Debug.Log("Invalid Tournament deleted");
                            }
                        }

                   }else{
                       Debug.Log("There are no available tournaments");
                       transactions.Clear();
                       transactions.Add("There are no available tournaments at the moment.");
                       transactionReady = true;
                       isEmpty = true;
                   }

               }
           });

    }

    public void getTournaments(){

        tournCount = 0;

            reference.Child("tournaments")
           .GetValueAsync().ContinueWith(task => {
               if (task.IsFaulted)
               {
                   Debug.Log("Error retreiving Tournaments");
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;

                   if(snapshot.Exists){
                       Debug.Log("Bitch Got "+snapshot.ChildrenCount+" Tournaments");

                        foreach (var child in snapshot.Children)
                        {
                            string valid = child.Child("valid").GetValue(false).ToString();
                            
                            if(valid.Equals("true"))
                                tournCount++;
                        }

                   }else{
                     Debug.Log("Bitch N0 Tournaments");
                   }

               }
           });

    }
	
	public IEnumerator SetWinner(string id, int index){
		
		Debug.Log("Fetching Winner Data");
		
		reference.Child("users").Child(id)
            .GetValueAsync().ContinueWith(task2 => {
            if (task2.IsFaulted) {
				Debug.Log("Error Retreiving Winner");
            }
            else if (task2.IsCompleted) {
                                            
				DataSnapshot snapshot2 = task2.Result;
											
				if(!snapshot2.Exists){
					Debug.Log("No User ID found!");
				}
				else{
                    Debug.Log("User ID found!");
                    string name = snapshot2.Child("name").GetValue(false).ToString();
                    string photo = snapshot2.Child("photoUrl").GetValue(false).ToString();
                    Debug.Log("Retreived User: " + name);
					
					if(index == 1){
						winner1Name = name;
						winner1Photo = photo;
						Debug.Log("Winner 1: "+winner1Name);
					}
					if(index == 2){
						winner2Name = name;
						winner2Photo = photo;
						Debug.Log("Winner 2: "+winner2Name);
					}
					if(index == 3){
						winner3Name = name;
						winner3Photo = photo;
						Debug.Log("Winner 3: "+winner3Name);
					}
					//displayName.text = name;
					//StartCoroutine(SetImage(photo, displayPhoto, false));
				}
                                    
            }
        }); 
		
		yield return null;
	}

     public void retreiveInvites()
    {
        isEmpty = false;

        if(state.Equals("")){

            invites.Clear();
            inviteReady = false;
            int count = 0;
            state = "";
            progBar.SetActive(true);
            //FindObjectOfType<FruitUI>().loadPanel.SetActive(true);

            Debug.Log("Checking for Previous Invites");
			
			reference.Child("users").Child(FruitUI.userID).Child("invites")
            .GetValueAsync().ContinueWith(task1 => {
                if (task1.IsFaulted)
                {
                    Debug.Log("Error retreiving previously used Invites");
                    messageToast = "Error loading Invites";
                }
                else if (task1.IsCompleted)
                {
                    DataSnapshot snapshot1 = task1.Result;

                        if(snapshot1.Exists){
                            
							foreach (var child in snapshot1.Children){
                                string id = child.Child("id").GetValue(false).ToString();
								if(!used.Contains(id))
									used.Add(id);
			
                                //Debug.Log("Retreived Previous ID: "+id);
                                count++;
                                if(count == snapshot1.ChildrenCount){
                                    state = "check";
                                }
                            }
							
							Debug.Log("You have "+used.Count+" previously used invites");
                        }else{
                            Debug.Log("You have 0 previously used invites");
                            state = "check";
                        }

                }
            });
			
		
        }
    }
	
	private void checkForNewInvites(){
        Debug.Log("Total Invites: "+invites.Count);
		cashCount = 0;
		usedCashCount = 0;
        state = "";
		int count = 0;		
		int a = 0;

        reference.Child("invites").Child(FruitUI.refID)
           .GetValueAsync().ContinueWith(task => {
               if (task.IsFaulted)
               {
                   Debug.Log("Error retreiving Invites");
                   state = "finalize";
               }
               else if (task.IsCompleted)
               {
                   DataSnapshot snapshot = task.Result;

                    if(snapshot.Exists){
                        
                        Debug.Log("You have " + snapshot.ChildrenCount + " Successful Invites");
                        
                        foreach (var child in snapshot.Children){
							
							string id = child.Child("id").GetValue(false).ToString();
							 string photo = "";
							 string name = "";
							 
							 a++;
							 Debug.Log("Child: "+a+" / "+snapshot.ChildrenCount);
							 
							
							if(retreived.Contains(id)){
								
								Debug.Log("User Previously Retreived");
								string cashed = "yes";
								
								 if(!used.Contains(id)){
										cashCount++;
										cashed = "no";
										Debug.Log("User Hasnt been cashed");
								 }else{
										Debug.Log("User Has been cashed");
										retreivedUsers[retreived.IndexOf(id)].email = "yes";
								 }
															
								invites.Add(retreivedUsers[retreived.IndexOf(id)]);
								
								count++;
								
								if(count == snapshot.ChildrenCount){
									inviteReady = true;
									state = "finalize";
									Debug.Log("Proceed to Finalize");
								}
							
							}else
							{	 
								Debug.Log("Fetching new user "+a);
								
								reference.Child("users").Child(id)
								.GetValueAsync().ContinueWith(task2 => {
									if (task2.IsFaulted) {
									Debug.Log("Error Retreiving user");
										count++;
												}
												else if (task2.IsCompleted) {
														
													DataSnapshot snapshot2 = task2.Result;
													
													if(!snapshot2.Exists){
														Debug.Log("No User ID found!");
														count++;
													}
													else{
														
														
															Debug.Log("User "+a+" found!");
														
														try{
															
															name = snapshot2.Child("name").GetValue(false).ToString();
															photo = snapshot2.Child("photoUrl").GetValue(false).ToString();
															float toks = float.Parse(snapshot2.Child("cash").GetValue(false).ToString());
														    if(toks > 9000f)
																toks = 9000f;
															
															string referredBy = snapshot2.Child("referredBy").GetValue(false).ToString();
															string cashed = "yes";
															Debug.Log("Retreived New User: " + name);

															if(referredBy.Equals(FruitUI.userID)){
																Debug.Log("Invite Validated"); //: Name: "+name +" ID: "+id);
																
																if(!used.Contains(id)){
																	cashCount++;
																	cashed = "no";
																	Debug.Log("User "+name+" Hasnt been cashed");
																}else{
																	Debug.Log("User "+name+" Has been cashed");
																	//CHECK INVITES STATS
																	if(inviteTokens.Count < 100){
																		inviteTokens.Add(toks);
																		//Debug.Log(name+" - "+toks+" Tokens");
																	}
																}
																
																
																retreived.Add(id);
																User user = new User(id, name, cashed, photo, 0);
																invites.Add(user);
																retreivedUsers.Add(user);
																
															}
														
														}catch(Exception e){
															Debug.Log("Error Parsing invite data");
														}

														count++;
														Debug.Log("Count: " + count);
														
														if(count == snapshot.ChildrenCount){
															inviteReady = true;
															state = "finalize";
															Debug.Log("Proceed to Finalize");
														}
													}
												
												}
												
												
													
											}); 
							}
											   
                        }
                    }else{
                        Debug.Log("You have 0 invites");
                        state = "finalize";
                    }

               }
           });

    }


    private void finalizeInvites(){
        state = "";
        progBar.SetActive(false);

        Debug.Log(cashCount + " invites yet to be awarded");
                        
                        if(cashCount >= minReferral){
							
							int diff = 0;
							
							float totalToks = 0;
							
							foreach(var toks in inviteTokens){
								totalToks += toks;
							}
							float averageToks = totalToks / inviteTokens.Count;
							
							//Debug.Log("Total Invite Tokens - "+totalToks+" / "+inviteTokens.Count+". Average = "+averageToks);
							updateInvites(null);
							
							/**
							int max = 100;
							
							if (used.Count > max)
								inviteReward = 100;
							else if ((used.Count + cashCount) > max){
								diff = ((used.Count + cashCount) - max) * ((int)inviteReward - 100);
							} **/
							
							if(inviteTokens.Count > 50 && used.Count > 50){
								if(averageToks > 1000)
									inviteReward = 200;
								else if(averageToks > 700)
									inviteReward = 150;
								else if(averageToks > 450)
									inviteReward = 100;
								else if(averageToks > 350)
									inviteReward = 50;
								else
									inviteReward = 20;
								
								//Debug.Log("Token Reward - "+inviteReward);
							}
                            for (int i=0; i<invites.Count; i++){
								if(!used.Contains(invites[i].id)){
									used.Add(invites[i].id);
								}
							}
							
							
							
                            float increase = (cashCount * inviteReward) - diff;
                            giveCash(increase, " for "+cashCount+" referrals");
							
							cashCount = 0;
							
							//if(diff > 0)
							//	inviteReward = 100;
						
							Strings.setLanguage();
							
                        }

                        int left = minReferral - cashCount;
                        inviteLeftMessage = "Invite " + left + Strings.invite3;
                         
    }

    public void giveCash(float increase, string message){

        myCash += increase;
        me.cash = myCash;
        string date = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        int code = UnityEngine.Random.Range(1001,9999);
        string childDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " Txn ID:"+code;
        string info = increase + " cash tokens"+ message +". Bal: "+myCash;
		
		finalizeUpdateData(childDate, info);
		
		cashIncrease = increase;
        awardMessage = "Awarded Cash Tokens"+ message + " !";
		award = true;
        
    }
     public void giveMoney(float increase){

        myCash += increase;
        me.cash = myCash;
        money ++;
        
    }
	
	public void checkNameAvailability(string newName)
    {
		Debug.Log("Checking if name is available");
		FindObjectOfType<FruitUI>().loadPanel.SetActive(true);
       reference.Child("usernames").OrderByChild("name").EqualTo(newName).LimitToFirst(1)
	   .ValueChanged += HandleValueChanged;

    }
	
	 private void HandleValueChanged(object sender, ValueChangedEventArgs args) {
		 
		 if(FruitUI.userName.Equals("")){
			 
			  if (args.DatabaseError != null) {
				Debug.LogError(args.DatabaseError.Message);
				closeLoadPanels();
				return;
			 }
			  
			  DataSnapshot snapshot = args.Snapshot;
				
					   if (snapshot.Exists){
						   Debug.Log("Exists");
						   closeLoadPanels();
						   FindObjectOfType<FruitUI>().showMessage("UserName is not available");
					   }
					   else {
							Debug.Log("Doesnt Exist");
							if(available.Equals(""))
								available = "yes";
					   }
		 }			   
    }
	
	public void updateToPrem(){
		prem = "true";
		finalizeUpdateData(null,null);
		
	}

	public void withdrawFunds(int index){
        int amount = 10000;
        string cash = FruitUI.getCurrency(FruitUI.funds1);

        if(index == 2){
            amount = 20000;
            cash = FruitUI.getCurrency(FruitUI.funds2);
        }
        if(index == 3){
            amount = 50000;
            cash = FruitUI.getCurrency(FruitUI.funds3);
        }
		if(index == 4){
            amount = 100000;
            cash = FruitUI.getCurrency(FruitUI.funds4);
        }
           

        FindObjectOfType<FruitUI>().playButtonSound();
        string details = FindObjectOfType<FruitUI>().accountDetails.text;
		
        if(!FruitUI.userName.Equals("")){

           
            if(myCash >= amount){

                if(details.Equals("")){
                    FindObjectOfType<FruitUI>().showMessage("Enter your Paypal email");
                }else{
					
					if(isMailVerfied){
						
						
							FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Coins);
							accountDet = details;
							recordWithdrawal(amount);
							
							/**
							FruitUI.bonus = 3;
							
							string emailSubject = System.Uri.EscapeUriString("Withdrawal Request (Fruit Spear)");
							string emailBody = System.Uri.EscapeUriString("Confirm your account details is correct and press send"
							+"\n\nID: " + FruitUI.userID.Substring(0,5) + "\nName: " + FruitUI.userName + " \nWallet: "+amount+ " \nWithdrawal Amount: "+cash+"\n\nAccount: "+details);
							Application.OpenURL("mailto:" + "payments@somteestudios.com.ng" + "?subject=" + emailSubject + "&body=" + emailBody);
							Debug.Log("sending Mail");
							**/
							
					}else
						FindObjectOfType<FruitUI>().showMessage("Verify your Email to Withdraw");
                }
				
               
            }else{
                FindObjectOfType<FruitUI>().showMessage("You don't have up to "+ amount +" cash tokens");
            }	
        }else{
            loginText.text = "Log Into Your Account";
            FindObjectOfType<FruitUI>().userNamePanel.SetActive(true);
        }
	}

    private void recordWithdrawal(int amount){
        myCash -= amount;
		cashout += amount;
		
		if(myCash == 0)
			myCash = new ProtectedFloat(5f);
		
		int code = UnityEngine.Random.Range(1001,9999);
        string txnID = "Txn ID:"+code;
        
        string childDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " (Withdraw) "+txnID;
		string info = "Withdrawal of "+amount+". Bal: "+myCash;
		accountDet = "Withdrawal of "+amount+" by "+FruitUI.userID+" to Acct: "+accountDet+"  - Bal: "+myCash;
        finalizeUpdateData(childDate, info);
        
		/**
		string date = System.DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
		string childDate2 = DateTime.UtcNow.Year + "-" + DateTime.UtcNow.Month.ToString("00") + "-" + DateTime.UtcNow.Day.ToString("00");
		reference.Child("withdrawals").Child(childDate2).Child(FruitUI.userID+txnID).Child("date").SetValueAsync(date);
		reference.Child("withdrawals").Child(childDate2).Child(FruitUI.userID+txnID).Child("status").SetValueAsync("pending");
        reference.Child("withdrawals").Child(childDate2).Child(FruitUI.userID+txnID).Child("info").SetValueAsync("Withdrawal of "+amount+" by "+FruitUI.userID+" to Acct: "+accountDet+"  - Bal: "+myCash);
		**/
		
		if(approved.Equals("yes"))
			StartCoroutine(uploadCashOut());
       
	    FindObjectOfType<FruitUI>().showMessage("Withdrawal request sent successfully!");
    }

    public void contactUs(){
        FindObjectOfType<FruitUI>().playButtonSound();
		float version = FruitUI.version / 10f;

        string emailSubject = System.Uri.EscapeUriString("Inquiry about Fruit Spear");
		string id = "\nEmail: " + FruitUI.email + "\nName: " + FruitUI.userName+"\nRef ID: "+FruitUI.refID+"\nApp Version: "+version+"\n\n\n";
        string emailBody = System.Uri.EscapeUriString(id);
                Application.OpenURL("mailto:" + "support@somteestudios.com.ng" + "?subject=" + emailSubject + "&body=" + emailBody);
                Debug.Log("sending Mail");
   	}

    public void uploadScore(string id, string status){
        updateTournament(id, status, FruitUI.selectedTournament.plays);
		
        int wonCount = 0;
        int lossCount = 0;

        referenceList.Child("competitors").Child(id).RunTransaction(mutableData => {
            List<object> results = mutableData.Value as List<object>;


            if (results == null)
            {
                results = new List<object>();
                Debug.Log("First On the List");
            }
            else if (mutableData.ChildrenCount > 0)
            {
                Debug.Log("There are " + results.Count + " results");

                foreach (var child in results)
                {
                    if (!(child is Dictionary<string, object>)) continue;

                    string won = (string)
                                ((Dictionary<string, object>)child)["won"];
                    string lost = (string)
                                ((Dictionary<string, object>)child)["lost"];            
                    Debug.Log("Won: "+won +", Lost:"+lost);

                    wonCount += Convert.ToInt32(won);
                    lossCount += Convert.ToInt32(lost);

                }

            }

            if(status.Equals("won"))
                wonCount++;
            if(status.Equals("lost"))
                lossCount++;

            // Add the new room
            Dictionary<string, object> data =
                             new Dictionary<string, object>();

            data["won"] = ""+wonCount;
            data["lost"] = ""+lossCount;

            if(results.Count > 0)
                results[0] = data;
            else results.Add(data);    
            

            mutableData.Value = results;

            return TransactionResult.Success(mutableData);

        });

    }

    public IEnumerator uploadTodaysHighScore()
    {

        yield return new WaitForSeconds(0);

        object room = null;
        int minScore = 5000;
		bool exists = false;
        string name = FruitUI.userName;
        string id = FruitUI.userID;
		List<int> scoresX = new List<int>();
		
		
        Debug.Log("Checking Todays Leaderboard");

        referenceList.Child("leaderboards").Child("leaderboard_day_"+curDay).RunTransaction(mutableData => {
            List<object> highScores = mutableData.Value as List<object>;


            if (highScores == null)
            {
                highScores = new List<object>();
                Debug.Log("Testing First On the Todays List");
				minTodaysScore = FruitSpearGameCode.score;
            }
            else if (mutableData.ChildrenCount > 0)
            {
                Debug.Log("There are " + highScores.Count + " names on todays list");

                foreach (var child in highScores)
                {
                    if (!(child is Dictionary<string, object>)) continue;

                    string score = "" + (long)
                                ((Dictionary<string, object>)child)["score"];
                    string childID = (string)
                                ((Dictionary<string, object>)child)["id"];
                   //Debug.Log("Name: "+name +"childID:"+childID);
                    
                    if (id.Equals(childID))
                    {
                        exists = true;
                        room = null;

                        if (FruitSpearGameCode.score > Convert.ToInt32(score))
                        {
                            room = child;
                            Debug.Log("New HighScore");
                        }
                        else
                        {
                            Debug.Log("Name: " +  "Your HighScore is already on todays Leaderboard");
                        }
                    }

                    if (Convert.ToInt32(score) < minScore && !exists)
                    {
                        minScore = Convert.ToInt32(score);
                        room = child;
                    }
					
					scoresX.Add(Convert.ToInt32(score));
                }
				
				
				Debug.Log("Testing Total todays score is "+scoresX.Count);	
				
				scoresX.Sort();
				minTodaysScore = scoresX[0];
				if(FruitSpearGameCode.score > minTodaysScore)
					minTodaysScore = FruitSpearGameCode.score;
						
				Debug.Log("Testing Minimum todays score is "+minTodaysScore);
				Debug.Log("Testing Maximum todays score is "+scoresX[scoresX.Count-1]);	
				
				

                if (FruitSpearGameCode.score > minScore && !exists)
                {
                    if (highScores.Count >= leaderBoardMax)
                        highScores.Remove(room);
                }
                else if (exists && room != null)
                    highScores.Remove(room);

            }
				
            FruitSpearGameCode.todaysScore = FruitSpearGameCode.score;

            // Add the new room
            Dictionary<string, object> newHighScore =
                             new Dictionary<string, object>();

            newHighScore["userName"] = name;
            newHighScore["id"] = id;
            newHighScore["photo"] = FruitUI.photoUrl;
            newHighScore["score"] = (long)FruitSpearGameCode.score;

            if (!exists)
            {
                if (highScores.Count < leaderBoardMax)
                {
                    highScores.Add(newHighScore);
                    Debug.Log(name + " your Highscore has been added to todays leaderboard");
                }
                else
                {
                    Debug.Log(name + " your Highscore not up to todays minimum");
                }
            }
            else if (exists && room != null)
            {
                highScores.Add(newHighScore);
                Debug.Log(name + " your Highscore has been updated on todays leaderboard");
            }

            mutableData.Value = highScores;

            return TransactionResult.Success(mutableData);

        });

    }   
	
	public IEnumerator uploadHighScore(string tournID)
    {

        yield return new WaitForSeconds(0);

        object room = null;
        int minScore = 5000;
        bool exists = false;
        string name = FruitUI.userName;
        string id = FruitUI.userID;
        int leaderBoardMax;
		List<int> scoresX = new List<int>();


        if(tournID==null){
            reference2 = referenceList.Child("leaderboard");
            leaderBoardMax = this.leaderBoardMax;
             Debug.Log("Checking Leaderboard");

        }
        else{    

            reference2 = referenceList.Child("competitors").Child(tournID);
            leaderBoardMax = FruitUI.selectedTournament.max;
            updateTournament(tournID, "none", FruitSpearGameCode.score);
            Debug.Log("Checking Tournament Leaderboard ");

        }

        reference2.RunTransaction(mutableData => {
            List<object> highScores = mutableData.Value as List<object>;


            if (highScores == null)
            {
                highScores = new List<object>();
				Debug.Log("Testing First On the List");
				if(tournID == null)
					minHighScore = FruitSpearGameCode.score;
            }
            else if (mutableData.ChildrenCount > 0)
            {
                Debug.Log("There are " + highScores.Count + " names on the list");

                foreach (var child in highScores)
                {
                    if (!(child is Dictionary<string, object>)) continue;

                    string score = "" + (long)
                                ((Dictionary<string, object>)child)["score"];
                    string childID = (string)
                                ((Dictionary<string, object>)child)["id"];
                   //Debug.Log("Name: "+name +"childID:"+childID);
                    
                    if (id.Equals(childID))
                    {
                        exists = true;
                        room = null;

                        if (FruitSpearGameCode.score > Convert.ToInt32(score))
                        {
                            room = child;
                            Debug.Log("New HighScore");
                        }
                        else
                        {
                            Debug.Log("Name: " +  "Your HighScore is already on Leaderboard");
                        }
                    }

                    if (Convert.ToInt32(score) < minScore && !exists)
                    {
                        minScore = Convert.ToInt32(score);
                        room = child;
                    }
					
					scoresX.Add(Convert.ToInt32(score));
					  
                }
					
					if(tournID==null){
						
						Debug.Log("Testing Total Top score is "+scoresX.Count);	
						
						scoresX.Sort();
						minHighScore = scoresX[0];
						if(FruitSpearGameCode.score > minHighScore)
							minHighScore = FruitSpearGameCode.score;
				
						Debug.Log("Testing Minimum Top score is "+minHighScore);
						Debug.Log("Testing Maximum Top score is "+scoresX[scoresX.Count-1]);	
				
					} 

                if (FruitSpearGameCode.score > minScore && !exists)
                {
                    if (highScores.Count >= leaderBoardMax)
                        highScores.Remove(room);
                }
                else if (exists && room != null)
                    highScores.Remove(room);

            }
			     

            // Add the new room
            Dictionary<string, object> newHighScore =
                             new Dictionary<string, object>();

            newHighScore["userName"] = name;
            newHighScore["id"] = id;
            newHighScore["photo"] = FruitUI.photoUrl;
            newHighScore["score"] = (long)FruitSpearGameCode.score;

            if (!exists)
            {
                if (highScores.Count < leaderBoardMax)
                {
                    highScores.Add(newHighScore);
                    Debug.Log(name + " your Highscore has been added to leaderboard");
                }
                else
                {
                    Debug.Log(name + " your Highscore not up to minimum");
                }
            }
            else if (exists && room != null)
            {
                highScores.Add(newHighScore);
                Debug.Log(name + " your Highscore has been updated on leaderboard");
            }

            mutableData.Value = highScores;

            return TransactionResult.Success(mutableData);

        });

    }
	
	 public IEnumerator uploadTodaysGreenApples()
    {

        yield return new WaitForSeconds(0);

        object room = null;
        int minScore = 50000;
        bool exists = false;
        string name = FruitUI.userName;
        string id = FruitUI.userID;
		List<int> scoresX = new List<int>();

        Debug.Log("Checking Todays Green Apple Leaderboard");
		
		if(curDay == System.DateTime.UtcNow.Day){
			
			referenceList.Child("leaderboards").Child("apple_leaderboard_day_"+curDay).RunTransaction(mutableData => {
				List<object> highScores = mutableData.Value as List<object>;


				if (highScores == null)
				{
					highScores = new List<object>();
					minApples = greenApples;
					Debug.Log("Testing First On the Todays Apples List");
				}
				else if (mutableData.ChildrenCount > 0)
				{
					Debug.Log("There are " + highScores.Count + " names on todays apple list");

					foreach (var child in highScores)
					{
						if (!(child is Dictionary<string, object>)) continue;

						string score = "" + (long)
									((Dictionary<string, object>)child)["score"];
						string childID = (string)
									((Dictionary<string, object>)child)["id"];
					   //Debug.Log("Name: "+name +"childID:"+childID);
						
						if (id.Equals(childID))
						{
							exists = true;
							room = null;

							if (greenApples > Convert.ToInt32(score))
							{
								room = child;
								Debug.Log("New HighScore");
							}
							else
							{
								Debug.Log("Name: " +  "Your HighScore is already on todays Leaderboard");
							}
						}

						if (Convert.ToInt32(score) < minScore && !exists)
						{
							minScore = Convert.ToInt32(score);
							room = child;
						}
						
						scoresX.Add(Convert.ToInt32(score));
					}
					
					
					Debug.Log("Testing Total Todays Apples is "+scoresX.Count);	
				
					scoresX.Sort();
					minApples = scoresX[0];
					if(greenApples > minApples)
						minApples = greenApples;
							
					Debug.Log("Testing Minimum todays apples is "+minApples);
					Debug.Log("Testing Maximum todays apples is "+scoresX[scoresX.Count-1]);	
					
					
					if (greenApples > minScore && !exists)
					{
						if (highScores.Count >= leaderBoardMax)
							highScores.Remove(room);
					}
					else if (exists && room != null)
						highScores.Remove(room);

				}
				
					
				// Add the new room
				Dictionary<string, object> newHighScore =
								 new Dictionary<string, object>();

				newHighScore["userName"] = name;
				newHighScore["id"] = id;
				newHighScore["photo"] = FruitUI.photoUrl;
				newHighScore["score"] = (long)greenApples;

				if (!exists)
				{
					if (highScores.Count < leaderBoardMax)
					{
						highScores.Add(newHighScore);
						Debug.Log(name + " your Green Apples has been added to todays leaderboard");
					}
					else
					{
						Debug.Log(name + " your Green Apples not up to todays minimum");
					}
				}
				else if (exists && room != null)
				{
					highScores.Add(newHighScore);
					Debug.Log(name + " your Green Apples has been updated on todays leaderboard");
				}

				mutableData.Value = highScores;

				return TransactionResult.Success(mutableData);

			});
		}
			

    } 
     
	 public IEnumerator uploadCashOut()
    {

        yield return new WaitForSeconds(0);

        object room = null;
        int minScore = 1000000;
        bool exists = false;
        string name = FruitUI.userName;
        string id = FruitUI.userID;


        Debug.Log("Checking Todays Top Withdrawals");
			if(isMailVerfied)
			referenceList.Child("leaderboard_cashhh").RunTransaction(mutableData => {
				List<object> highScores = mutableData.Value as List<object>;


				if (highScores == null)
				{
					highScores = new List<object>();
					Debug.Log("First On the Todays Cash List");
				}
				else if (mutableData.ChildrenCount > 0)
				{
					Debug.Log("There are " + highScores.Count + " names on todays cash list");

					foreach (var child in highScores)
					{
						if (!(child is Dictionary<string, object>)) continue;

						string score = "" + (long)
									((Dictionary<string, object>)child)["score"];
						string childID = (string)
									((Dictionary<string, object>)child)["id"];
					   //Debug.Log("Name: "+name +"childID:"+childID);
						
						if (id.Equals(childID))
						{
							exists = true;
							room = null;

							if (cashout > Convert.ToInt32(score))
							{
								room = child;
								Debug.Log("New Top Withdrawal");
							}
							else
							{
								Debug.Log("Name: " +  "Your Withdrawal is already on todays Leaderboard");
							}
						}

						if (Convert.ToInt32(score) < minScore && !exists)
						{
							minScore = Convert.ToInt32(score);
							room = child;
						}
					}

					if (cashout > minScore && !exists)
					{
						if (highScores.Count >= leaderBoardMax)
							highScores.Remove(room);
					}
					else if (exists && room != null)
						highScores.Remove(room);

				}

				// Add the new room
				Dictionary<string, object> newHighScore =
								 new Dictionary<string, object>();

				newHighScore["userName"] = name;
				newHighScore["id"] = id;
				newHighScore["photo"] = FruitUI.photoUrl;
				newHighScore["score"] = (long) cashout;

				if (!exists)
				{
					if (highScores.Count < leaderBoardMax)
					{
						highScores.Add(newHighScore);
						Debug.Log(name + " your Withdrawal has been added to todays leaderboard");
					}
					else
					{
						Debug.Log(name + " your Withdrawal is not up to todays minimum");
					}
				}
				else if (exists && room != null)
				{
					highScores.Add(newHighScore);
					Debug.Log(name + " your Withdrawal has been updated on todays leaderboard");
				}

				mutableData.Value = highScores;

				return TransactionResult.Success(mutableData);

			});
		
			

    } 
	 
	 
    public IEnumerator uploadBid(string tournID)
    {

        yield return new WaitForSeconds(0);

        object room = null;
        int minBid = 500000;
        bool exists = false;
        string name = FruitUI.userName;
        string id = FruitUI.userID;
        int leaderBoardMax = 10;
		List<int> scoresX = new List<int>();
		minBid1 = FruitUI.selectedTournament.plays;
		
        Debug.Log("Uploading Top Bid");

        referenceList.Child("competitors").Child(tournID).Child("top").RunTransaction(mutableData => {
            List<object> highBids = mutableData.Value as List<object>;

            if (highBids == null)
            {
                highBids = new List<object>();
                Debug.Log("First On the Top List");
            }
            else if (mutableData.ChildrenCount > 0)
            {
                Debug.Log("There are " + highBids.Count + " bids on the Top list");

                foreach (var child in highBids)
                {
                    if (!(child is Dictionary<string, object>)) continue;

                    string bid = "" + (long)
                                ((Dictionary<string, object>)child)["bid"];
                    string childID = (string)
                                ((Dictionary<string, object>)child)["id"];
                   //Debug.Log("Name: "+name +"childID:"+childID);
                    
                    if (id.Equals(childID))
                    {
                        exists = true;
                        room = null;

                        if (FruitUI.selectedTournament.plays > Convert.ToInt32(bid))
                        {
                            room = child;
                            Debug.Log("New HighBid");
                        }
                        else
                        {
                            Debug.Log("Name: " +  "Your HighBid is already on Top Leaderboard");
                        }
                    }

                    if (Convert.ToInt32(bid) < minBid && !exists)
                    {
                        minBid = Convert.ToInt32(bid);
                        room = child;
                    }
					
					scoresX.Add(Convert.ToInt32(bid));
                }
				
					Debug.Log("Testing Total Bs is "+scoresX.Count);	
				
					scoresX.Sort();
					if(scoresX.Count >= 5)
						minBid1 = scoresX[0];
							
					Debug.Log("Testing Min Bd is "+minBid1);
					Debug.Log("Testing Max Bd is "+scoresX[scoresX.Count-1]);
					

                if (FruitUI.selectedTournament.plays > minBid && !exists)
                {
                    if (highBids.Count >= leaderBoardMax)
                        highBids.Remove(room);
                }
                else if (exists && room != null)
                    highBids.Remove(room);

            }

            // Add the new room
            Dictionary<string, object> newHighBid =
                             new Dictionary<string, object>();
			
            newHighBid["userName"] = name;
            newHighBid["id"] = id;
            newHighBid["photo"] = FruitUI.photoUrl;
            newHighBid["bid"] = (long)FruitUI.selectedTournament.plays;

            if (!exists)
            {
                if (highBids.Count < leaderBoardMax)
                {
                    highBids.Add(newHighBid);
                    Debug.Log(name + " your Bid has been added to Top Tournament");
                }
                else
                {
                    Debug.Log(name + " your Bid is not up to Top minimum");
                }
            }
            else if (exists && room != null)
            {
                highBids.Add(newHighBid);
                Debug.Log(name + " your Bid has been updated on Top Tournament");
            }

            mutableData.Value = highBids;

            return TransactionResult.Success(mutableData);

        });

    }

     public void updateBids(string tournID){
        
        int count = 1;
        
         referenceList.Child("competitors").Child(tournID).Child("bids").RunTransaction(mutableData => {
            List<object> results = mutableData.Value as List<object>;

            if (results == null)
            {
                results = new List<object>();
                Debug.Log("First On the List");
            }
            else if (mutableData.ChildrenCount > 0)
            {
                Debug.Log("There are " + results.Count + " results");

                foreach (var child in results)
                {
                    if (!(child is Dictionary<string, object>)) continue;

                    string prev = (string)
                                ((Dictionary<string, object>)child)["count"];
                    Debug.Log("Prev Count: "+prev);

                    count = Convert.ToInt32(prev) + 1;

                }

            }

            // Add the new room
            Dictionary<string, object> data =
                             new Dictionary<string, object>();

            data["count"] = ""+count;

            if(results.Count > 0)
                results[0] = data;
            else results.Add(data);               

            mutableData.Value = results;

            return TransactionResult.Success(mutableData);

        });

    }


    
}
