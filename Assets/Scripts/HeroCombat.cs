using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : MonoBehaviour
{

    public static HeroCombat instance;

    private Rigidbody2D m_body2d;

    public float inputX = 0.0f;
    public float inputY = 0.0f;

    [Header("Input bools")]
    public bool canReceiveInput;

    public bool inputBA;
    public bool inputUpBA;

    public bool m_disableMovement;


    [Header("Basic Attacks Forces")]

    float basic1Force= 30.0f;
    float basic2Force = 15.0f;
    float basic3Force = 15.0f;




    private void Awake()
    {
        instance = this;
        canReceiveInput = true;
        inputBA = false;

        m_body2d = GetComponent<Rigidbody2D>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        
    }

    public void Attack()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        //Basic Attack in idle o Crouch
        if (Input.GetButtonDown("Fire1") && inputY <= 0.0f)
        {
            Debug.Log("Attacke detectado");
            if (canReceiveInput)
            {
                inputBA = true;
                canReceiveInput = false;
            }
            else
            {
                return;
            }
        }

        //Up Attack in idle
        if (Input.GetButtonDown("Fire1") && inputY > 0.0f)
        {
            Debug.Log("Attacke detectado");
            if (canReceiveInput)
            {
                inputUpBA = true;
                canReceiveInput = false;
            }
            else
            {
                return;
            }
        }

    }

    public void InputManager()
    {

        if(!canReceiveInput)
        {
            canReceiveInput = true;
        }
        else
        {
            canReceiveInput = false;
        }


    }

    public void StopImpulse()
    {
        m_body2d.velocity = new Vector2(0, 0);
    }
    
    public void ImpulseBasicAttack1()
    {
        int faceDirection = Hero.instance.getFaceDirection();
        m_body2d.velocity = new Vector2(basic1Force* faceDirection, 0);
    }

    public void ImpulseBasicAttack2()
    {
        int faceDirection = Hero.instance.getFaceDirection();
        m_body2d.velocity = new Vector2(basic2Force* faceDirection, 0);
    }

     public void ImpulseBasicAttack3()
    {
        int faceDirection = Hero.instance.getFaceDirection();
        m_body2d.velocity = new Vector2(basic3Force* faceDirection, 0);
    }

    public void ImpulseUpBA()
    {
        m_body2d.velocity = new Vector2(0, basic1Force);
    }

}
