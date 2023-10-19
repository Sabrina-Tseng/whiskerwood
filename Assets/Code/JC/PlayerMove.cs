using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //player number
    string horizontalControl;
    string jumpControl;
    string fireControl;
    
    public int speed = 5;
    public int jumpForce = 10;

    public bool grounded;
    public LayerMask ground;
    public Transform feet;
    public float recoveryTime = .5f;
    public bool hurt = false;

    Rigidbody2D rb; 
    Animator anim;

    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       anim = GetComponent<Animator>();

        //player number 1
        horizontalControl = "Horizontal";
        jumpControl = "Jump";
        fireControl = "Fire1";
    }


    void Update()
    {
        float xSpeed = Input.GetAxis(horizontalControl) * speed;

        if (!hurt) {
        rb.velocity = new Vector2(xSpeed, rb.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(xSpeed));
        }
        
        if ((xSpeed < 0 && transform.localScale.x > 0) || (xSpeed > 0 && transform.localScale.x < 0)) 
        {
        transform.localScale *= new Vector2(-1,1);
        }

    

        grounded = Physics2D.OverlapCircle(feet.position, .1f, ground);
        anim.SetBool("Grounded", grounded);

        if(Input.GetButtonDown(jumpControl) && grounded){
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce));
        }


        if(Input.GetButtonDown(fireControl)){
            anim.SetTrigger("Attack");
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
     {
        if (other.gameObject.CompareTag("Enemy") && !hurt)

        {
        StartCoroutine(IFrames());
        }

    }

    IEnumerator IFrames ()
    {
        hurt = true;
        anim.SetBool("Hurt", hurt);
        yield return new WaitForSeconds(recoveryTime);
        hurt = false;
        anim.SetBool("Hurt", hurt);
    }


}
