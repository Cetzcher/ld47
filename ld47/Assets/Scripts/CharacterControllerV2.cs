using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterControllerV2 : MonoBehaviour 
{

    public UnityEvent walkStartEvent = new UnityEvent();
    public UnityEvent walkEndEvent = new UnityEvent();
    public UnityEvent jumpEvent = new UnityEvent();
    public UnityEvent doubleJumpEvent = new UnityEvent();

    // private members
    private Rigidbody2D rb; 
    private Vector2 input;
    private Animator animator;


    // public memebers
    // Ground check stuff
    public float lastJumpTime = 0f;
    public bool grounded = false;
    public bool hasDoubleJumped = false;
    public int groundChekcCount = 4;
    public float groundTolerance = 0.1f;
    
    // movement stuff
    public float airMovementMult = 2f;
    public float jumpImpulse = 10f;
    public float groundMovementMult = 2f;
    public float minDoubleJumpLag = 0.250f;    // 250 ms

    private AxisHelper yAxis;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        yAxis = new AxisHelper("Vertical");
        animator = GetComponent<Animator>();
    }

    private IEnumerator TurnToBlock()
    {
        /*while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }*/
        yield return null;
        LoopHandler.Instance.Reset();
        rb.velocity = Vector3.zero;
    }

    private void HandleInput() 
    {
        yAxis.UpdateAxis();
        var x = Input.GetAxis("Horizontal");
        var y = yAxis.Value;
        input = new Vector2(x, y);
        if( Input.GetKeyDown("space")) 
        {
            if(LoopHandler.Instance.CanReset)
            {
                //animator.SetBool("blockify", true);
                StartCoroutine(TurnToBlock());
            }
        }

    }

    private void Raycheck()
    {
        var x = transform.position.x;
        var y = transform.position.y;
        // get lowes point
        y -= transform.localScale.y / 1.8f  + 0.1f;
        var startX = x - transform.localScale.x / 3.5f;
        var endX = x + transform.localScale.x / 3.5f;
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
                if(res.collider.isTrigger)
                    continue;
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
            jumpEvent.Invoke();
        }

        var move = new Vector2(input.x * groundMovementMult, 0);
        if(Mathf.Abs(move.x) > 0f)
        {
            walkStartEvent.Invoke();
        }
        else
        {
            walkEndEvent.Invoke();
        }
        rb.AddForce(move);
    }

    private void AirMovement() 
    {
        // going airborne stops the walk since we are now 'gliding'
        walkEndEvent.Invoke();
        // jump only IF has not double jumped yet, the input y is positive and the time since the jump is 
        // greate than the lag
        if(!hasDoubleJumped && yAxis.IsDown && Time.time - lastJumpTime > minDoubleJumpLag) 
        {  
            rb.velocity = new Vector2(rb.velocity.x, 0f);   // reset y movement
            Jump(jumpImpulse);
            hasDoubleJumped = true;
            doubleJumpEvent.Invoke();
        }
        var move = new Vector2(input.x * airMovementMult, 0);
        rb.AddForce(move);
    }

}


