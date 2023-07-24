using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
   public string name;
   public string id;
   public string refID;
   public string referredBy;
   public string email;
   public string photoUrl;
   public int score;
   public int coins;
   public float cash;
   public int games;
   
   public User(string id, string name, string email, string photoUrl, int score){
       this.id = id;
       this.name = name;
	   this.email = email;
       this.photoUrl = photoUrl;
       this.score = score;
   }
}
