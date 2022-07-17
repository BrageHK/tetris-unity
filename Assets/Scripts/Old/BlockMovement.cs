using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockMovement : MonoBehaviour
{
    private float time;
    public GameObject block;
    // Start is called before the first frame update
    void Awake() {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate() {        
        time += Time.deltaTime;
        if(time > 0.7f) {
            if(transform.position.y == -3.75f) {
                SpawnNewBlock();
                Destroy(gameObject.GetComponent<BlockMovement>());
            }else {
                transform.position = new Vector3(0, -0.5f, 0) + transform.position;
                time = 0;
            }
        }
    }

    private void SpawnNewBlock() {
        Instantiate(gameObject, new Vector3(0.25f, 0.25f, 0), Quaternion.identity);
    }
    
    
    void OnMove(InputValue value) {
        if(true) {
            Vector2 direction = value.Get<Vector2>();
            if(direction.x < -0.5f) {
                transform.position = new Vector3(-0.5f, 0, 0) + transform.position;
            } else if(direction.x > 0.5f) {
                transform.position = new Vector3(0.5f, 0, 0) + transform.position;
            } else if(direction.y > 0.5f) {
                transform.Rotate(0, 0, 90f);   
            } 
        }
    }

    void OnRotate(InputValue value) {
        Debug.Log(value.Get<float>());
    }
}
