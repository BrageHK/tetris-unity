using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public GameObject block1;
    public GameObject block2;
    bool alternating = true;
    // Start is called before the first frame update
    void Awake() {
        for(int x = 0; x < 10; x++) {
            for(int y = 0; y < 10; y++) {
                if(alternating) {
                    Instantiate(block1, new Vector3(x*0.5f-5f, y*0.5f, 0), Quaternion.identity);
                    alternating = false;
                } else {
                    Instantiate(block2, new Vector3(x*0.5f-5f, y*0.5f, 0), Quaternion.identity);
                    alternating = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
