using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class FrameSkeleton {
    private List<FrameSkeletonJoint> joints = new List<FrameSkeletonJoint>();

    public FrameSkeleton()
    {

    }

    public void AddJoint(FrameSkeletonJoint fsj)
    {
        joints.Add(fsj);
    }

    public List<FrameSkeletonJoint> GetFrameSkeletonJoints()
    {
        return joints;
    }
}
