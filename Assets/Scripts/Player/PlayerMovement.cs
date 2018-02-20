using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Animator anim;
    private Vector3 movment;
    private Rigidbody playerRigidbody;
    private float speed;
    public bool dead;
    private bool canJump;
    private float timeBetweenJump;
    private float timer;
    private int direction;
    private int old_direction;
    public GameObject crosshair;

    // Use this for initialization
    void Start () {
        dead = !GetComponent<Player>().alive;
    }
    void Awake()
    {
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        speed = 3f;
        
        canJump = true;
        timeBetweenJump = 1.5f;
        direction = 0;
    }

    void FixedUpdate()
    {
            old_direction = direction;
            float h = 0f;
            float v = 0f;
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            Move(h, v);
            CalculDirection(h,v);
            Animating(h, v);
    }

    void Move(float h, float v)
    {
        if (dead)
        {
            Debug.LogError("Mort");
            return;
        }
        movment.Set(h, 0f, v);
        movment = movment.normalized * speed * Time.deltaTime;
        transform.Translate(movment);
        //crosshair.transform.Translate(movment);
        //playerRigidbody.MovePosition(transform.position + movment);
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        //Debug.LogError(walking);
        anim.SetBool("IsMoving", walking);
        //anim.SetInteger("direction", direction);
        if (!walking)
        {
            //anim.SetInteger("direction", 0);
        }
    }

    void CalculDirection (float h, float v)
    {

        if ( v!= 0)
        {
            if (v > 0)
            {
                direction = 1;
            }else
            {
                direction = 2;
            }
        }else
        {
            if (h > 0)
            {
                direction = 4;
            } else
            {
                direction = 3;
            }
        }
    }
    // Update is called once per frame
    void Update () {

        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("run", true);
            speed = 10;
        }else
        {
            anim.SetBool("run", false);
            speed = 3;
        }
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            Debug.LogError("Jump");
            anim.SetTrigger("Jump");
            timer = 0f;
            canJump = false;
        }
        if(!canJump && timer> timeBetweenJump)
        {
            canJump = true;
        }
	}
}
