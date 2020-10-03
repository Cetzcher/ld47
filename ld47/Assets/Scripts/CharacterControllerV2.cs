using UnityEngine;
using System.Collections;

public class CharacterControllerV2 : MonoBehaviour 
{

    // private members
    private Rigidbody2D rb; 
    private Vector2 input;

    // public memebers
    // Ground check stuff
    public float lastJumpTime = 0f;
    public bool grounded = false;
    public bool hasDoubleJumped = false;
    public int groundChekcCount = 4;
    public float skin = 0.1f;   // how far away the rays start
    public float groundTolerance = 0.1f;
    
    // movement stuff
    public float airMovementMult = 2f;
    public float jumpImpulse = 10f;
    public float groundMovementMult = 2f;
    public float minDoubleJumpLag = 0.250f;    // 250 ms
    public Vector2 maximumSpeed = new Vector2(30, 30);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void HandleInput() 
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        input = new Vector2(x, y);
    }

    private void Raycheck()
    {
        var x = transform.position.x;
        var y = transform.position.y;
        // get lowes point
        y -= transform.localScale.y / 2f  + 0.1f;
        var startX = x - transform.localScale.x / 2f;
        var endX = x + transform.localScale.x / 2f;
        grounded = false;

        for(int i = 0; i < groundChekcCount + 1; i++)
        {
            float t = ( (float) i) / ((float) groundChekcCount);
            var xinterp = Mathf.Lerp(startX, endX, t);
            var pos = new Vector2(xinterp, y);
            var dir = Vector2.down;
            Debug.DrawRay(pos, dir, Color.red, 2);
            var res = Physics2D.Raycast(pos, dir, groundTolerance);
            if(res.collider != null) 
            {
                grounded = true;
                hasDoubleJumped = false;
            }
        }
    }

    private void FixedUpdate()
    {
        HandleInput();
        Raycheck();
        if(grounded)
        {
            // cool!
            GorundedMovement();
        }
        else 
        {
            AirMovement();
        }
    }

    private void Jump(float imp) 
    {   
        // return the jump force
        lastJumpTime = Time.time;
        rb.AddForce(Vector2.up * imp, ForceMode2D.Impulse);
    }

    private void GorundedMovement() 
    {
        var xMove = input.x * groundMovementMult;
        if(input.y > 0.2f) 
        {
            Jump(jumpImpulse);
        }

        var move = new Vector2(input.x * groundMovementMult, 0);
        rb.AddForce(move);
    }

    private void AirMovement() 
    {
        // jump only IF has not double jumped yet, the input y is positive and the time since the jump is 
        // greate than the lag
        if(!hasDoubleJumped && input.y > 0.2f && Time.time - lastJumpTime > minDoubleJumpLag) 
        {  
            rb.velocity = new Vector2(rb.velocity.x, 0f);   // reset y movement
            Jump(jumpImpulse);
            hasDoubleJumped = true;
        }
        var move = new Vector2(input.x * airMovementMult, 0);
        rb.AddForce(move);
    }

}


