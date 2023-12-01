using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;

    public GameObject projectilePrefab;
    public GameObject damageRubyPrefab;
    public GameObject healRubyPrefab;

    public AudioClip throwSound;
    public AudioClip hitSound;
    //public AudioClip poisonSound; //unused as it serves no purpose and also all of this is my code


    public int health { get { return currentHealth; } }
    int currentHealth;

    public int score;
    //public GameObject scoreText;
    public TMP_Text scoreText;
    public GameObject gameOverText;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    bool gameOver = false;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;


    //public int poisonCounter = 0;






    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene
            }
        }



    }




    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {


        if (amount < 0 && amount != -2) //MODIFIED THIS TO INCREMENT A POISON DAMAGE OVER TIME EFFECT
        {
            

            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            GameObject damageRubyObject = Instantiate(damageRubyPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity); //damage particle code
            PlaySound(hitSound);
        }
        else if (amount > 0) //for heal particles
        {
            //heal particle thinggggggg
            GameObject healRubyObject = Instantiate(healRubyPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }
        else if (amount <= -2) //POISON DOT EFFECT *attempt
        {

            
              /*while(poisonCounter <= 3)
              {
               isInvincible = false;
               poisonCounter += 1;
              }*/
            
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
            GameObject damageRubyObject = Instantiate(damageRubyPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity); //damage particle code
            PlaySound(hitSound);//temporary
            //PlaySound(poisonSound); TURN THIS ON LATER
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if (currentHealth == 0) //note to self: if this doesnt work try currentHealth = 0
        {
            gameOverText.SetActive(true);
            gameOverText.GetComponent<TMP_Text>().text = "You lost! Press R to Restart!";
            speed = 0.0f;
            gameOver = true;
        }
    }

    public void ChangeScore(int scoreAmount)
    {
        
        if (scoreAmount > 0) //increasing score
        {
            score += scoreAmount;
        }

        scoreText.text = "Fixed Robots: " + score.ToString();
        //UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if (score == 2) //note to self: if this doesnt work try score = 2
        {
            gameOverText.SetActive(true);
            gameOverText.GetComponent<TMP_Text>().text = "You Win! Created by Zachary Templin of Group 10";
            speed = 0.0f;
            gameOver = true;
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwSound);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}

