using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SarahMove : MonoBehaviour
{
    //game manager
    private GameManager gameManager;

    //player number
    string horizontalControl;
    string verticalControl;
    string fireControl;

    //health bar
    public int maxHealth = 30;
    public int currentHealth;
    public HealthBar healthBar;

    //gems
    public GameObject gem1;
    public GameObject gem2;
    public GameObject gem3;
    public Transform gemPosition;

    public int speed = 5;
    public float iFrames = .5f;
    public bool hurt;
    public float recoveryTime = .3f;
    private int dir = 1;

    //attack
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    int bulletForce = 500;
    public bool attacking;

    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //player number 3
        horizontalControl = "Horizontal3";
        fireControl = "Fire3";
        verticalControl = "Vertical3";

        //health bar
        //healthBar.SetMaxHealth(30);
        healthBar.SetHealth(currentHealth);

    }

    void Update()
    {
        float xSpeed = Input.GetAxis(horizontalControl) * speed;
        float ySpeed = Input.GetAxis(verticalControl) * speed;

        if (!hurt)
        {
            rb.velocity = new Vector2(xSpeed, ySpeed);
            anim.SetFloat("Speed", Mathf.Abs(xSpeed));
            anim.SetFloat("ySpeed", ySpeed);
        }

        //direction
        if ((xSpeed < 0 && transform.localScale.x > 0) || (xSpeed > 0 && transform.localScale.x < 0))
        {
            transform.localScale *= new Vector2(-1, 1);
            dir *= -1;
        }

        //attack
        if (Input.GetButtonDown(fireControl) && xSpeed == 0 && ySpeed == 0 && !attacking)
        {
            anim.SetTrigger("Shoot");
            StartCoroutine(Attack());
        }

        //go to next scene
        if (currentHealth == 20)
        {
            //gameManager.LoadLevel("Level 2");
            Destroy(this.gameObject);
            Instantiate(gem1, gemPosition.position, Quaternion.identity);
        }
        if (currentHealth == 10)
        {
            Destroy(this.gameObject);
            Instantiate(gem2, gemPosition.position, Quaternion.identity);
        }
        //dead
        if (currentHealth == 0)
        {
            Destroy(this.gameObject);
            Instantiate(gem3, gemPosition.position, Quaternion.identity);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("FishBone"))
        {
            Destroy(other.gameObject);
            if (!hurt)
            {
                StartCoroutine(IFrames());
            }
        }
        if (other.gameObject.CompareTag("Bone"))
        {
            Destroy(other.gameObject);
            if (!hurt)
            {
                StartCoroutine(IFrames());
            }
        }
    }

    IEnumerator IFrames()
    {
        TakeDamage(1);
        hurt = true;
        anim.SetBool("Hurt", hurt);
        yield return new WaitForSeconds(recoveryTime);
        hurt = false;
        anim.SetBool("Hurt", hurt);
    }

    IEnumerator Attack()
    {
        attacking = true;
        yield return new WaitForSeconds(0.5f);
        GameObject newBullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir * bulletForce, 0));
        //if the bullet has a direction
        newBullet.transform.localScale *= new Vector2(dir, 1);
        attacking = false;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    
}