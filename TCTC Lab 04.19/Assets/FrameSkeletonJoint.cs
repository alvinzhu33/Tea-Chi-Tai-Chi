using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class FrameSkeletonJoint {
    private string jointName;
    private float x;
    private float y;
    private float z;

    public FrameSkeletonJoint(string jointName, float x, float y, float z)
    {
        this.jointName = jointName;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public string GetJointName()
    {
        return jointName;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }
}
