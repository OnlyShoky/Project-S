using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

    [Header("Variables")]
    [SerializeField] float      m_maxSpeed = 4.5f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] bool       m_hideSword = false;
    [Header("Effects")]
    [SerializeField] GameObject m_RunStopDust;
    [SerializeField] GameObject m_JumpDust;
    [SerializeField] GameObject m_LandingDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private AudioSource         m_audioSource;
    private AudioManager_PrototypeHero m_audioManager;
    private bool                m_moving = false;
    private int                 m_facingDirection = 1;
    private float               m_disableMovementTimer = 0.0f;
    float inputX = 0.0f;
    float inputY = 0.0f;


    [Header("Grounded")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundRadius = 0.2f;
    bool isGrounded = false;

    [Header("WallJump")]
    [SerializeField] GameObject m_WallSlide;
    [SerializeField] GameObject m_WallJump;

    [SerializeField] float m_xWallForce = 7.5f;
    [SerializeField] float m_yWallForce = 7.5f;

    public Transform wallCheck;
    public float wallJumpTime = 0.2f;
    public float wallSlideSpeed = 1f;
    public float wallDistance = 0.3f;
    bool isWallSliding = false;
    RaycastHit2D WallCheckHit;
    float jumpTime;
    private Dust_DestroyEvent dustWallSlidingDestroyer;

   

    



    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();
        m_audioManager = AudioManager_PrototypeHero.instance;
        //m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Prototype>();
       
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease timer that disables input movement. Used when attacking
        m_disableMovementTimer -= Time.deltaTime;

        bool touchingGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, LayerMask.GetMask("GroundLayer","PlatformLayer")); 

        if (touchingGround)
        {
            isGrounded = true;
            m_animator.SetBool("Grounded", isGrounded);
        }
        else
        {
            isGrounded = false;
            m_animator.SetBool("Grounded", isGrounded);
        }

        // -- Handle input and movement --


        if (m_disableMovementTimer < 0.0f)
        {
            inputX = Input.GetAxis("Horizontal");
            inputY = Input.GetAxis("Vertical");

        }



        // GetAxisRaw returns either -1, 0 or 1
        float inputRaw = 0;
        if (jumpTime < Time.time)
             inputRaw = Input.GetAxisRaw("Horizontal");
        else if(isWallSliding)
             inputRaw = -Input.GetAxisRaw("Horizontal");

        // Check if current move input is larger than 0 and the move direction is equal to the characters facing direction
        if (Mathf.Abs(inputRaw) > Mathf.Epsilon && Mathf.Sign(inputRaw) == m_facingDirection)
            m_moving = true;
       else
            m_moving = false;

        // Swap direction of sprite depending on move direction
        if (inputRaw > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
            
        else if (inputRaw < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }
     
        // SlowDownSpeed helps decelerate the characters when stopping
        float SlowDownSpeed = m_moving ? 1.0f : 0.5f;

        // Set movement
        if(isGrounded || jumpTime < Time.time)
            m_body2d.velocity = new Vector2(inputX * m_maxSpeed * SlowDownSpeed, m_body2d.velocity.y);
  

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // Set Animation layer for hiding sword
        int boolInt = m_hideSword ? 1 : 0;
        m_animator.SetLayerWeight(1, boolInt);

        // -- Handle Animations --
        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded && m_disableMovementTimer < 0.0f )
        {
            m_animator.SetTrigger("Jump");
            isGrounded = false;
            m_animator.SetBool("Grounded", isGrounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            //m_groundSensor.Disable(0.2f);
        }
        else if(inputY<0 && isGrounded && !m_moving)
            m_animator.SetInteger("AnimState", 2);

        //Run
        else if(m_moving)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);


        // Wall Jump
        if (m_facingDirection == 1)
        {
            WallCheckHit = Physics2D.Raycast(wallCheck.position,  new Vector2(wallDistance, 0), wallDistance, groundLayer);
            Debug.DrawRay(wallCheck.position, new Vector2(wallDistance, 0), Color.blue);
        }
        else
        {
            WallCheckHit = Physics2D.Raycast(wallCheck.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);
            Debug.DrawRay(wallCheck.position, new Vector2(-wallDistance, 0), Color.blue);
        }

        if(WallCheckHit && !isGrounded && inputRaw != 0)
        {
            m_animator.SetBool("Walled", WallCheckHit);
            isWallSliding = true;
 
        }else 
        {

            isWallSliding = false;
            m_animator.SetBool("Walled", isWallSliding);

            //Destroy the dust effect of the gliding when we get out
            var item = transform.Find("WallSlide(Clone)");
            if (item != null)
            {
                dustWallSlidingDestroyer = item.GetComponent<Dust_DestroyEvent>();
                dustWallSlidingDestroyer.destroyEvent();
            }

        }

        if (isWallSliding)
        {
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, Mathf.Clamp(m_body2d.velocity.y, -wallSlideSpeed, float.MaxValue));
        }

        if ((isWallSliding && Input.GetButtonDown("Jump")))
        {
            m_animator.SetTrigger("Jump");
            isGrounded = false;
            m_animator.SetBool("Grounded", isGrounded);
            jumpTime = Time.time + wallJumpTime;

            m_body2d.velocity = new Vector2(m_xWallForce*-inputX, m_yWallForce);
            
        }


    }

    // Function used to spawn a dust effect
    // All dust effects spawns on the floor
    // dustXoffset controls how far from the player the effects spawns.
    // Default dustXoffset is zero
    void SpawnDustEffect(GameObject dust, float dustXOffset = 0,float dustYOffset = 0)
    {
        if (dust != null)
        {
            // Set dust spawn position
            Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset * m_facingDirection, dustYOffset, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;            
            // Turn dust in correct X direction
            newDust.transform.localScale = new Vector3(newDust.transform.localScale.x*m_facingDirection, newDust.transform.localScale.y , newDust.transform.localScale.z);
        }
    }

    void SpawnDustEffectOnHero(GameObject dust, float dustXOffset = 0, float dustYOffset = 0)
    {
        if (dust != null)
        {
            // Set dust spawn position
            Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset * m_facingDirection, dustYOffset, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
            newDust.transform.SetParent(this.transform);

            // Turn dust in correct X direction
            newDust.transform.localScale = new Vector3(newDust.transform.localScale.x * m_facingDirection, newDust.transform.localScale.y, newDust.transform.localScale.z);
        }
    }

    // Animation Events
    // These functions are called inside the animation files
    void AE_runStop()
    {
        m_audioManager.PlaySound("RunStop");
        // Spawn Dust
        float dustXOffset = 0.6f;
        SpawnDustEffect(m_RunStopDust, dustXOffset);
    }

    void AE_footstep()
    {
        m_audioManager.PlaySound("Footstep");
    }

    void AE_Jump()
    {
        m_audioManager.PlaySound("Jump");
        // Spawn Dust
        SpawnDustEffect(m_JumpDust);
    }

    void AE_Landing()
    {
        m_audioManager.PlaySound("Landing");
        // Spawn Dust
        SpawnDustEffect(m_LandingDust);
    }

    void AE_WallSlide()
    {
        m_audioManager.PlaySound("Landing");
        // Spawn Dust
        SpawnDustEffectOnHero(m_WallSlide, 0.27f , 1.36f);
    }

    

}
