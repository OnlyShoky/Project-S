using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : MonoBehaviour
{

    public static HeroCombat instance;
    public bool canReceiveInput;
    public bool inputReceived;

    private void Awake()
    {
        instance = this;
        canReceiveInput = true;
        inputReceived = false;

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
}
