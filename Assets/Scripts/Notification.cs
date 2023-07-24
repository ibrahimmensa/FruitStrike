using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class Notification : MonoBehaviour
{
	
	public static string[] titles = {"Today's Leaderboard ending soon!⭐️",
                                "Few Hours Left to Win!💫",
								"You can Win Today's Reward!💸"};
	public static string[] daily_titles = {"Claim your reward!💞",
                                "Daily Reward 🎁",
								"Today's Reward is Ready 💝"};							
								
	public static string[] texts = {"You have just a few hours to claim your leaderboard position and win todays ca$h money!💸",
                                "You still have a chance to claim your spot and win todays prize Money!💰, Lets Go!💫",
								"Collect Green apples and Win today's prize Money!💰, 3 hours Left!"};


    public void scheduleNotification(){

        string raw = "chan"+UnityEngine.Random.Range(10001,99999);
        string channelId = raw.Trim();
		int hour = System.DateTime.UtcNow.Hour;
		int left = 24-hour;
		if(left == 0)
			left = 24;

         var c = new AndroidNotificationChannel()
        {
            Id = channelId,
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(c);
		
		int notNum = UnityEngine.Random.Range(0,daily_titles.Length);

        AndroidNotification notification = new AndroidNotification()
        {
            Title = daily_titles[notNum],
            Text = " Your Daily Reward is Ready🤩, Claim it Now!",
            SmallIcon = "icon_small",
            LargeIcon = "icon_big",
            FireTime = System.DateTime.Now.AddHours(left),
        };

        Debug.Log("Fire Daily Notification in "+left+" Hours");
		
        AndroidNotificationCenter.SendNotification(notification, channelId);
        
       // PlayerPrefs.SetInt("day", DateTime.Now.Day);
       // PlayerPrefs.SetInt("hour", DateTime.Now.Hour);
        scheduleGameNotification();

    }

    public void scheduleGameNotification(){

        string raw = "game"+UnityEngine.Random.Range(10001,99999);
        string channelId = raw.Trim();

         var c = new AndroidNotificationChannel()
        {
            Id = channelId,
            Name = "Default Channel_Game",
            Importance = Importance.High,
            Description = "Generic notifications",
        };

        AndroidNotificationCenter.RegisterNotificationChannel(c);

        int curHour = System.DateTime.UtcNow.Hour;
        

        int showHour = 0;

        if(curHour < 21)
            showHour = 21 - curHour;
        else   
            showHour = 21 - curHour + 24; 
		
		
		
		int notNum = UnityEngine.Random.Range(0,titles.Length);

        AndroidNotification notification = new AndroidNotification()
        {
            Title = titles[notNum],
            Text = texts[notNum],
            SmallIcon = "icon_small",
            LargeIcon = "icon_big",
            FireTime = System.DateTime.Now.AddHours(showHour),
        };

        Debug.Log("Fire Leaderboard Notification in "+showHour+" Hours");
        
        AndroidNotificationCenter.SendNotification(notification, channelId);
     
    }

}
