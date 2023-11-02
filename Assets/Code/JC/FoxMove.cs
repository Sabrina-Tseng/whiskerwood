using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxMove : MonoBehaviour
{
    //game manager
    private GameManager gameManager;
    
    //player number
    string horizontalControl;
    string jumpControl;
    string fireControl;
    
    //health bar
    public int maxHealth = 10000;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject Skull;

    //audio
    public AudioClip jumpSound;
    AudioSource _audioSource;
    
    public int speed = 5;
    public int jumpForce = 10;

    private int dir = 1;

    public GameObject bulletPrefab;
    public Transform spawnPoint;
    int bulletForce = 500;

    public bool grounded;
    public LayerMask ground;
    public Transform feet;
    public float recoveryTime = 1f;
    public bool hurt = false;

    Rigidbody2D rb; 
    Animator anim;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        //player number 1
        horizontalControl = "Horizontal";
        jumpControl = "Jump";
        fireControl = "Fire1";

        //health bar
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    void Update()
    {
        float xSpeed = Input.GetAxis(horizontalControl) * speed;

        if (!hurt) {
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(xSpeed));
        }
        
        if ((xSpeed < 0 && transform.localScale.x > 0) || (xSpeed > 0 && transform.localScale.x < 0)) {
    
            transform.localScale *= new Vector2(-1,1);
            dir *= -1;
            healthBar.ChangeDirection();
        }

    

        grounded = Physics2D.OverlapCircle(feet.position, .5f, ground);
        anim.SetBool("Grounded", grounded);

        if(Input.GetButtonDown(jumpControl) && grounded){
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce));
            _audioSource.PlayOneShot(jumpSound);
        }


          if (Input.GetButtonDown(fireControl) && grounded && xSpeed == 0)
        {
            anim.SetTrigger("Attack");
            GameObject newBullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir * bulletForce, 0));
            //if the bullet has a direction
            newBullet.transform.localScale *= new Vector2(dir,1);
        }

        //dead
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
            Instantiate(Skull, spawnPoint.position, Quaternion.identity);
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
     {
        if (other.gameObject.CompareTag("Cucumber"))
        {
            // anim.SetTrigger("Attack");
            currentHealth+=2000;
            healthBar.SetHealth(currentHealth);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("FireBall"))
        {
            Destroy(other.gameObject);
            if (!hurt)
            {
                StartCoroutine(IFrames());
            }        
        }

        if (other.gameObject.CompareTag("Gem1"))
        {
            gameManager.LoadLevel("Level 2");      
        }
        if (other.gameObject.CompareTag("Gem2"))
        {
            gameManager.LoadLevel("Level 3");      
        }
        if (other.gameObject.CompareTag("Gem3"))
        {
            gameManager.LoadLevel("Win");      
        }

        // fox carry cat (didn't work)
        // if (other.gameObject.CompareTag("Cat") && transform.position.y < other.transform.position.y)
        // {
        //     other.transform.SetParent(transform);
        // }
    }

    IEnumerator IFrames ()
    {
        TakeDamage(1000);
        hurt = true;
        anim.SetBool("Hurt", hurt);
        rb.AddForce(new Vector2(dir * -500, 0));
        yield return new WaitForSeconds(recoveryTime);
        hurt = false;
        anim.SetBool("Hurt", hurt);
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    // void OnCollisionExit2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Cat"))
    //     {
    //         other.transform.SetParent(null);
    //     }
    // }

}
