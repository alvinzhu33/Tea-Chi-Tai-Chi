using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerInput : MonoBehaviour {

    // Should only ever be one
    protected List<VRInteractableItem> heldObjects;

    // Controller reference
    protected SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device device
    {
        get
        {
            return SteamVR_Controller.Input((int)trackedObj.index);
        }
    }

	// Use this for initialization
	void Awake () {
        // Instantiate lists
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        heldObjects = new List<VRInteractableItem>();
	}

    void OnTriggerStay(Collider collider)
    {
        Debug.Log("Hello World!");
        // If object is an interactable item
        VRInteractableItem interactable = collider.GetComponent<VRInteractableItem>();
        if (interactable != null)
        {
            // If trigger button is down
            if (device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
            {
                // Pick up object
                interactable.Pickup(this);
                heldObjects.Add(interactable);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		if (heldObjects.Count > 0)
        {
            // If trigger is released
            if (device.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
            {
                // Release any held objects
                for (int i = 0; i < heldObjects.Count; i++)
                {
                    heldObjects[i].Release(this);
                }
                heldObjects = new List<VRInteractableItem>();
            }
        }
	}
}
