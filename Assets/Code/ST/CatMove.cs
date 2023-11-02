using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : MonoBehaviour
{
    //player number
    string horizontalControl;
    string jumpControl;
    string fireControl;

    //health bar
    public int maxHealth = 10000;
    public int currentHealth;
    public HealthBar healthBar;

    //move
    public int speed = 5;
    public int jumpForce = 1000;
    public bool grounded;
    private int dir = 1;

    //attack
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    int bulletForce = 500;

    //hurt
    public float recoveryTime = 1f;
    public bool hurt = false;
    public GameObject Skull;

    //audio
    public AudioClip jumpSound;
    public AudioClip catScream;
    AudioSource _audioSource;

    public LayerMask ground;
    public Transform feet;
 
    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        //player number 2
        horizontalControl = "Horizontal2";
        jumpControl = "Jump2";
        fireControl = "Fire2";

        //health bar
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        // currentHealth = 0;
        // healthBar.SetHealth(currentHealth);
    }

    void Update()
    {
        float xSpeed = Input.GetAxis(horizontalControl) * speed;

        //movement
        if (!hurt) //no movement if hurt
        {
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(xSpeed));
        }
        
        //direction
        if ((xSpeed < 0 && transform.localScale.x > 0) || (xSpeed > 0 && transform.localScale.x < 0))
        {
            transform.localScale *= new Vector2(-1, 1);
            dir *= -1;
            healthBar.ChangeDirection();
        }

        //jump
        grounded = Physics2D.OverlapCircle(feet.position, .5f, ground); 
        anim.SetBool("Grounded", grounded);

        if(Input.GetButtonDown(jumpControl) && grounded) 
        {
            _audioSource.PlayOneShot(jumpSound);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0,jumpForce));
        }

        //loaf
        if(Input.GetKey("s") && grounded && xSpeed == 0) 
        {
            anim.SetBool("Loaf", true);
            // StartCoroutine(LoafRecover());        
            currentHealth += 1;
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            anim.SetBool("Loaf", false);
        }

        //fire
        if (Input.GetButtonDown(fireControl) && grounded && xSpeed == 0)
        {
            anim.SetTrigger("Shoot");
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

    //hurt
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Cucumber") && !hurt )
        {
            StartCoroutine(IFrames1());
        }

        if (other.gameObject.CompareTag("FireBall"))
        {
            Destroy(other.gameObject);
            if (!hurt)
            {
                StartCoroutine(IFrames2());
            }
        }
    }

    IEnumerator IFrames1()
    {
        TakeDamage(500);
        hurt = true;
        anim.SetBool("Hurt",hurt);
        _audioSource.PlayOneShot(catScream);
        rb.AddForce(new Vector2(dir * -500, 1000));
        yield return new WaitForSeconds(recoveryTime);
        hurt = false;
        anim.SetBool("Hurt",hurt);
    }
    IEnumerator IFrames2()
    {
        TakeDamage(2000);
        hurt = true;
        anim.SetBool("Hurt",hurt);
        _audioSource.PlayOneShot(catScream);
        rb.AddForce(new Vector2(dir * -500, 0));
        yield return new WaitForSeconds(recoveryTime);
        hurt = false;
        anim.SetBool("Hurt",hurt);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
