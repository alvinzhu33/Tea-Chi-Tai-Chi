using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {
    private float prevX;
    private float prevY;
    private float prevZ;

    private GameObject myself;
    private GameObject other;

    private bool largeArrow;

	// Use this for initialization
	void Start () {
        if (transform.tag == "EarthArrow")
        {
            myself = GameObject.FindWithTag("Earth");
            other = GameObject.FindWithTag("Venus");
            largeArrow = false;
        } else if (transform.tag == "VenusArrow")
        {
            other = GameObject.FindWithTag("Earth");
            myself = GameObject.FindWithTag("Venus");
            largeArrow = false;
        }
        else if (transform.tag == "MoonArrow")
        {
            other = GameObject.FindWithTag("Moon");
            myself = GameObject.FindWithTag("Jupiter");
            largeArrow = true;
        }
        else if (transform.tag == "JupiterArrow")
        {
            other = GameObject.FindWithTag("Jupiter");
            myself = GameObject.FindWithTag("Moon");
            largeArrow = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        /* POSITION */
        transform.position = myself.transform.position;

        /* SCALE */
        // Find the distance between red and green box
        float dist = Vector3.Distance(myself.transform.position, other.transform.position);
		float arrow_length = .0015f * 1.0f / Mathf.Pow(dist,2.0f);
        if (largeArrow)
        {
            arrow_length *= 2.0f;
        }

        // Set z scale proportional to distance squared
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, arrow_length);

        /* ROTATION */
        // Rotate Arrow towards red box
        Vector3 arrowToRedBox = other.transform.position - myself.transform.position;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, arrowToRedBox,1,0.0f);

        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
