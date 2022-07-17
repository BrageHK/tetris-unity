using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

//how to deactivate script:
//GameObject.Find("Cube").GetComponent<MoveObject>().enabled = false;

public class MoveBlock : MonoBehaviour
{/*

    private float time = 0f;
    public float speed = 1f;
    private Vector3 vector = new Vector3(0, -0.5f, 0);
    public GameObject block;
    private GameObject thisBlock;


    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update() {
        var keyboard = Keyboard.current;
        time += Time.deltaTime;
        
        
        if(keyboard.spaceKey.WasPerformedThisFrame) {
            transform.Rotate(0, 0, 90f);
            vector = Quaternion.AngleAxis(-45, Vector3.up) * vector;
            Debug.Log(transform.eulerAngles);
        }

        /*
        if(thisBlock.Transform.position.y < .5) {
            CreateNewBlock();
        }
    }

    public void RotateBlock() {

    }
    public void CreateNewBlock(InputAction.CallbackContext context) {
        if(context.performed) {
            thisBlock = Instantiate(block, Vector3.zero, transform.rotation);
            thisBlock.transform.Translate(vector);
        }
    }

    void FixedUpdate() {
        if(time > speed) {

            
            //thisBlock.position += new Vector3(0,-0.5f,0);
            time = 0f;
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        transform.Rotate(0, 0, 90f);
        vector = Quaternion.AngleAxis(-45, Vector3.up) * vector;

        /*
        if(direction.x>0.5f){
            transform.Translate(new Vector3(0.5f,0,0));
        }
        else if(direction.x<-0.5f){
            transform.Translate(new Vector3(-0.5f,0,0));
        }
        if(direction.y<-0.5f){
            transform.Translate(new Vector3(0,-0.5f,0));
        }
    }*/


}