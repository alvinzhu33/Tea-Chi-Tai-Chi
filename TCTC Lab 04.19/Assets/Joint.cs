using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint {
    private string jointName;
    private Transform jointTransform;

    public Joint(string jointName, Transform jointTransform)
    {
        this.jointName = jointName;
        this.jointTransform = jointTransform;
    }

    public string GetJointName()
    {
        return jointName;
    }

    public Transform GetJointTransform()
    {
        return jointTransform;
    }

    public Vector3 GetJointTransformPosition()
    {
        Vector3 pos = new Vector3(-jointTransform.position.x, jointTransform.position.y, jointTransform.position.z);
        return pos;
    }
}
