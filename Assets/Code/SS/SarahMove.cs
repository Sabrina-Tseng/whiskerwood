using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SarahMove : MonoBehaviour
{
    //player number
    string horizontalControl;
    string verticalControl;
    string fireControl;

    //health bar
    public int maxHealth = 30;
    public int currentHealth;
    public HealthBar healthBar;

    public int speed = 5;
    public float iFrames = .5f;
    public bool hurt;
    public float recoveryTime = .3f;

    Rigidbody2D rb;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //player number 3
        horizontalControl = "Horizontal3";
        fireControl = "Fire3";
        verticalControl = "Vertical3";

        //health bar
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        float xSpeed = Input.GetAxis(horizontalControl) * speed;
        float ySpeed = Input.GetAxis(verticalControl) * speed;

        rb.velocity = new Vector2(xSpeed, ySpeed);
        anim.SetFloat("Speed", Mathf.Abs(xSpeed));
        anim.SetFloat("ySpeed", ySpeed);


        if (xSpeed < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (xSpeed > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }


        if (Input.GetButtonDown(fireControl))
        {
            anim.SetTrigger("Shoot");
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

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}