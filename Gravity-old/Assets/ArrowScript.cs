using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    public GameObject greenBox;
    public GameObject redBox;

    private float prevX;
    private float prevY;
    private float prevZ;

	// Use this for initialization
	void Start () {
        greenBox = GameObject.FindWithTag("GreenBox");
        redBox = GameObject.FindWithTag("RedBox");
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(greenBox.transform.position.x);
        float x = -3.6f;
        float y = 1.010147f;
        float z = -0.1028511f;
        transform.position = greenBox.transform.position;
        //new Vector3(x, y, z)
    }
}
