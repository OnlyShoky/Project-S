using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : MonoBehaviour
{

    public static HeroCombat instance;

    private Rigidbody2D m_body2d;

    [Header("Input bools")]
    public bool canReceiveInput;
    public bool inputReceived;

    public bool m_disableMovement;


    [Header("Basic Attacks Forces")]

    float basic1Force= 30.0f;
    float basic2Force = 15.0f;
    float basic3Force = 15.0f;




    private void Awake()
    {
        instance = this;
        canReceiveInput = true;
        inputReceived = false;

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
        
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Attacke detectado");
            if (canReceiveInput)
            {
                inputReceived = true;
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


}
