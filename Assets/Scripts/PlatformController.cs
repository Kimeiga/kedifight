using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlatformController : MonoBehaviour
{

    [HideInInspector] public bool facingRight = false;
    [HideInInspector] public bool jump = false;

    public bool rightPlayer; //yes for mavikedi (right controls), no for sarikedi (left controls)

    public float runSpeed = 8;
    float xMove;

    public float maxSpeed = 5f;
    public float jumpForce = 1000f;


    public bool grounded = false;
    private Rigidbody2D rb;


    public Transform topLeft;
    public Transform bottomRight;
    public LayerMask groundLayers;


    //mario jump
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public SpriteRenderer spriteRen;
    public SpriteRenderer deathSpriteRen;
    public Sprite idleSprite;
    public Sprite jumpSprite;
    public Sprite fallSprite;
    public Sprite runSprite;

    public Vector2 shotForce = Vector2.zero;
    public bool shot = false;

    public Vector2 recoilForce = Vector2.zero;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        //try jump
        if (rightPlayer)
        {
            if (Input.GetButtonDown("Right Jump") && grounded)
            {
                jump = true;
            }

            if (Input.GetAxisRaw("Right Hor") == 1 && !facingRight)
                Flip();
            else if (Input.GetAxisRaw("Right Hor") == -1 && facingRight)
                Flip();

            //mario jump
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Right Jump"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }



            
        }
        else
        {
            if (Input.GetButtonDown("Left Jump") && grounded)
            {
                jump = true;
            }

            if (Input.GetAxisRaw("Left Hor") == 1 && !facingRight)
                Flip();
            else if (Input.GetAxisRaw("Left Hor") == -1 && facingRight)
                Flip();

            //mario jump
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Left Jump"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
            

            
        }



    }

    void FixedUpdate()
    {


        spriteRen.sprite = idleSprite;
        deathSpriteRen.sprite = idleSprite;

        if (rightPlayer)
        {

            if (Mathf.Abs(Input.GetAxisRaw("Right Hor")) > 0.01f)
            {
                spriteRen.sprite = runSprite;
                deathSpriteRen.sprite = runSprite;
            }
        }
        else
        {

            if (Mathf.Abs(Input.GetAxisRaw("Left Hor")) > 0.01f)
            {
                spriteRen.sprite = runSprite;
                deathSpriteRen.sprite = runSprite;
            }

        }

        if (rb.velocity.y > 3.8f) //and jumping
        {
            spriteRen.sprite = jumpSprite;
            deathSpriteRen.sprite = jumpSprite;
        }
        else if (rb.velocity.y < -3.8f && !grounded)
        {
            spriteRen.sprite = fallSprite;
            deathSpriteRen.sprite = fallSprite;
        }


        //check if grounded
        grounded = Physics2D.OverlapArea(topLeft.position, bottomRight.position, groundLayers);

        float h;

        if (rightPlayer)
        {
            //store Right Hor input
            h = Input.GetAxis("Right Hor");

        }
        else
        {

            //store Right Hor input
            h = Input.GetAxis("Left Hor");
        }

        

        
        //store wish move Right Hor
        float xMove = h * runSpeed;

        //move Right Horly
        rb.velocity += new Vector2(xMove - rb.velocity.x, 0);

        //jump
        if (jump)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

        rb.velocity += shotForce;
        rb.velocity += recoilForce;


        //if (shot)
        //{

        //    LeanTween.value(shotForce.x, 0, 0.2f).setEase(LeanTweenType.easeOutExpo).setOnUpdate((float val) =>
        //    {
        //        shotForce.x = val;
        //    });
        //    LeanTween.value(shotForce.y, 0, 0.2f).setOnUpdate((float val) =>
        //    {
        //        shotForce.y = val;
        //    });
        //    shot = false;
        //}

        shotForce = Vector3.Lerp(shotForce, Vector3.zero, 0.35f);
        recoilForce = Vector3.Lerp(recoilForce, Vector3.zero, 0.35f);

        //limit speed by maxSpeed
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 0, 0), rb.velocity.y.ToString());
    }

}