using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpearPhysics : MonoBehaviour
{
    public Rigidbody2D rb;
    public RawImage image;
    public bool hasShot;
    public static int counter;
    public static bool failed;
    private bool hit;

    private void Start()
    {
        FruitSpearGameCode.spears.Add(this.gameObject);
        failed = false;
    }

    void Update()
   {
        image.texture = FruitUI.currentSpear;

        if (FruitSpearGameCode.ready && FruitSpearGameCode.running && !FindObjectOfType<FruitUI>().marketPanel.activeSelf && Time.time - FruitSpearGameCode.burstUpdate >= 0.75f)
        {

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !hasShot && Input.mousePosition.y < Screen.height/1.4f 
            && !FindObjectOfType<FruitUI>().skipStagePanel.activeSelf
            && !FindObjectOfType<FruitUI>().updatePanel.activeSelf)
            {
                shoot();
            }
#elif UNITY_ANDROID
            if (Input.touchCount > 0 && !hasShot && Input.mousePosition.y < Screen.height/1.4f
            && !FindObjectOfType<FruitUI>().skipStagePanel.activeSelf
            && !FindObjectOfType<FruitUI>().updatePanel.activeSelf)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    shoot();
                }
            }
#endif
            }
    }

    void shoot(){

       // Debug.Log("Clicked");
        if(FruitSpearGameCode.gameType == FruitSpearGameCode.GameType.Target){
            FruitSpearGameCode.running = false;
            Debug.Log("Last Spear");
        }        
		FindObjectOfType<FruitUI>().tutPanel.SetActive(false);
        FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Hit);
        rb.AddForce(new Vector2(0, Screen.height/0.3f), ForceMode2D.Impulse);
        hasShot = true;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.collider.tag == "Player" && !failed && !hit)
        {
            rb.velocity = new Vector2(-rb.velocity.x*4,-rb.velocity.y);
            failed = true;
            FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Clash);
            gameOver();
        }
    }

    void gameOver()
    {
        FruitSpearGameCode.running = false;
		int rand = UnityEngine.Random.Range(0,3);

        if (((FruitSpearGameCode.gameType == FruitSpearGameCode.GameType.Top && FruitSpearGameCode.luckInterval == 0) || (FruitSpearGameCode.gameType == FruitSpearGameCode.GameType.Normal && rand == 1 && FruitSpearGameCode.adInterval == 0 && !FruitSpearGameCode.saved)) &&  FindObjectOfType<FruitSpearGameCode>().isRewardAdReady())
        {
			FruitSpearGameCode.saved = true;
            FruitSpearGameCode.luckInterval++;
			giveExtraLife();
        }
        else
            FindObjectOfType<FruitSpearGameCode>().finalizeGameOver();

    }

    private void giveExtraLife()
    {
        SpearPhysics.counter = 7;
        FindObjectOfType<FruitUI>().counterText.text = counter + "";
        FindObjectOfType<FruitUI>().extraLifePanel.SetActive(true);
        FindObjectOfType<FruitSpearGameCode>().noSave.SetActive(false);
        Debug.Log("Give Extra Life");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Tyre" && !failed)
        {
            rb.velocity = Vector3.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            this.transform.SetParent(collision.transform);
            hit = true;
            if(FruitSpearGameCode.gameType == FruitSpearGameCode.GameType.Target)
                FruitSpearGameCode.running = true;

            FindObjectOfType<FruitSpearGameCode>().splash.GetComponent<Animator>().Play("splash");
           
            //Debug.Log("Hit "+collision.tag);
           if(FruitSpearGameCode.gameType != FruitSpearGameCode.GameType.Time){
                FruitSpearGameCode.score++;
                FindObjectOfType<FruitSpearGameCode>().scoreText.text = FruitSpearGameCode.score + "";
            }
            if (FruitSpearGameCode.spearCount > 1)
            {
                FindObjectOfType<FruitSpearGameCode>().fruitRBFront.GetComponent<Animator>().Play("tyre_collide");
            }
            else
            {
                FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Fruit_Burst);
                int index = FruitSpearGameCode.fruitStage + 1;
                string anim = "f" + index;
                if (FruitSpearGameCode.bossFight)
                    anim = "fb" + index;
                
                FindObjectOfType<FruitSpearGameCode>().fruitBreak.GetComponent<Animator>().Play(anim);
               // Debug.Log("Animating " + anim);
                FruitSpearGameCode.burstUpdate = Time.time;
            }

            FindObjectOfType<FruitSpearGameCode>().spawnSpear();

        }

        if (collision.tag == "Life" && !failed)
        {
            FindObjectOfType<FruitSpearGameCode>().peachHitImage.GetComponent<Animator>().Play("boss_fight");
            FindObjectOfType<FruitSpearGameCode>().peachCut.GetComponent<Animator>().Play("pc_break");
            FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Small_Fruit_Burst);
            FindObjectOfType<FruitSpearGameCode>().updateTarget();
            
            FruitSpearGameCode.score += 2;
            NetworkScript.coins++;
            FruitSpearGameCode.peaches.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }

         if (collision.tag == "Money" && !failed)
        {
            FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Coins);
            FindObjectOfType<NetworkScript>().giveMoney(1f);
            FruitSpearGameCode.peaches.Remove(collision.gameObject);
            Destroy(collision.gameObject);
            FindObjectOfType<FruitUI>().MoneyPanel.GetComponent<Animator>().Play("money_anim");
            
        }

        if (collision.tag == "Green" && !failed)
        {
            FindObjectOfType<FruitSpearGameCode>().peachHitImage.GetComponent<Animator>().Play("boss_fight");
            FindObjectOfType<FruitSpearGameCode>().peachCut.GetComponent<Animator>().Play("pc_break");
            FindObjectOfType<FruitSpearGameCode>().playSound(FruitSpearGameCode.Sound.Small_Fruit_Burst);
           
            FruitSpearGameCode.score += 3;
            NetworkScript.greenApples++;
            FruitSpearGameCode.peaches.Remove(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }

}
