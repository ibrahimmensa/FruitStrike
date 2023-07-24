using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpScript : MonoBehaviour
{	

public InputField email, password, username;
public string emailText, passwordText, userText;
	
    public void createAccount(){
		
		userText = username.text.ToLower();
		emailText = email.text.Trim();
		passwordText = password.text.Trim();
		
			
			if(userText.Length < 3){
				FindObjectOfType<FruitUI>().showMessage("username too short");
				Debug.Log("username too short");
			}
			else if(emailText.Length < 8){
				FindObjectOfType<FruitUI>().showMessage("email is incomplete");
				Debug.Log("email is incomplete");
			}
			else if(passwordText.Length < 8){
				FindObjectOfType<FruitUI>().showMessage("password must be at least 8 characters");
				Debug.Log("password is too short");
			}
			else if(FindObjectOfType<NetworkScript>().CheckForInternetConnection(1))
				FindObjectOfType<NetworkScript>().checkNameAvailability(userText);
	}
	
	public void proceedSignUp(){
		FruitUI.userName = userText;
		NetworkScript.email = emailText;
		NetworkScript.manualSignIn = true;
		//FindObjectOfType<NetworkScript>().checkEmail();
		//FindObjectOfType<NetworkScript>().CreateFirebaseUser(userText,emailText,passwordText);
	}
	
	public void loginAccount(){
		
			string emailText = email.text.Trim();
			string passwordText = password.text.Trim();
			
			    if(emailText.Length < 8){
					FindObjectOfType<FruitUI>().showMessage("email is incomplete");
					Debug.Log("email is incomplete");
				}
				else if(passwordText.Length < 8){
					FindObjectOfType<FruitUI>().showMessage("password must be at least 8 characters");
					Debug.Log("password is too short");
				}
				else
					if(FindObjectOfType<NetworkScript>().CheckForInternetConnection(1))
						FindObjectOfType<NetworkScript>().SignInFirebase(emailText,passwordText);
			
	}

}
