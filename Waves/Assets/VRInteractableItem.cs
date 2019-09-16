using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInteractableItem : MonoBehaviour {

    // http://academyofvr.com/intro-vive-development-pickup-interactions/

    protected Rigidbody rigidBody;
    protected bool originalKinematicState;
    protected Transform originalParent;

	// Use this for initialization
	void Awake () {
        rigidBody = GetComponent<Rigidbody>();

        // Capture original parent and kinematic state
        originalParent = transform.parent;
        originalKinematicState = rigidBody.isKinematic;
	}

    public void Pickup(VRControllerInput controller)
    {
        // Make object kinematic (not effected by physics, but still able to effect other objects)
        rigidBody.isKinematic = true;

        // Set parent object to hand
        transform.SetParent(controller.gameObject.transform);
    }

    public void Release(VRControllerInput controller)
    {
        // See if the hand is still the parent (and not transferred to another hand)
        if (transform.parent == controller.gameObject.transform)
        {
            // Return to previous kinematic state
            rigidBody.isKinematic = originalKinematicState;

            // Set parent to original parent
            if (originalParent != controller.gameObject.transform)
            {
                transform.SetParent(originalParent);
            }
            else
            {
                transform.SetParent(null);
            }

            // Throw object
            rigidBody.velocity = controller.device.velocity;
            rigidBody.angularVelocity = controller.device.angularVelocity;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
