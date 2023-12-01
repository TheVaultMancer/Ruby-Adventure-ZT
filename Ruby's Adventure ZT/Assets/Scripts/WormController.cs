using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour
{

    public int poisonCounter; //basic variables Also I made every change in this code since im the only person in my team
    public float timeForPoison = 3.0f;
    bool isPoisoned;
    float PoisonTimer;
    void OnTriggerStay2D(Collider2D other)
    {
        poisonCounter = 0;
        RubyController controller = other.GetComponent<RubyController>();
        while (poisonCounter <= 3) //Attempt at getting a poison damage function to work 
        {
            //isInvincible = false;
            if (controller != null)
            {
                controller.ChangeHealth(-2); //more damage to differentiate from other enemies
                //Destroy(gameObject); 
            }
            poisonCounter += 1; //planned to use this to bypass the timelimit of the invincibility timer
            Debug.Log(poisonCounter);
        }
        /*if (controller != null)
        {
            controller.ChangeHealth(-2);
            Destroy(gameObject);
        }*/
        //Destroy(gameObject);

        /*if (isPoisoned)
        {
            poisonTimer -= Time.deltaTime;
            if (poisonTimer < 0)
                isPoisoned = false;
        }*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}
