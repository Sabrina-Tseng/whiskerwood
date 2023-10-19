using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SarahMove : MonoBehaviour
{
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
    }

    void Update()
    {
        float xSpeed = Input.GetAxis("Horizontal") * speed;
        float ySpeed = Input.GetAxis("Vertical") * speed;

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


        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Shoot");
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Enemy") && !hurt)

        {
            StartCoroutine(IFrames());
        }

    }

    IEnumerator IFrames()
    {
        hurt = true;
        anim.SetBool("Hurt", hurt);
        yield return new WaitForSeconds(recoveryTime);
        hurt = false;
        anim.SetBool("Hurt", hurt);





    }


}