using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerScript : MonoBehaviour
{
    public int speed;
    private int time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        Debug.Log(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 10) {
            transform.Rotate(0, 0, 1);
            time = 0;
        } else {
            time += speed;
        }
        
    }
}
