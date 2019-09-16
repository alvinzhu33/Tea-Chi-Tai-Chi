using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryCameraIndicator : MonoBehaviour {
    public static Camera cam;

	void Awake () {
        cam = GetComponent<Camera>();
	}
}
