using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton {
    private Vector3 origin;
    private Vector3 humanoidOrigin;
    private float rotation;
    private float scale;
    Dictionary<string, Joint> kinectJoints = new Dictionary<string, Joint>();
    Dictionary<string, Joint> humanoidJoints = new Dictionary<string, Joint>();
    Dictionary<string, string> jointMap = new Dictionary<string, string>()
    {
        { "HandRight", "ElbowRight" },
        { "ElbowRight", "ShoulderRight" },
        { "ShoulderRight", "Neck" },
        { "HandLeft", "ElbowLeft" },
        { "ElbowLeft", "ShoulderLeft" },
        { "ShoulderLeft", "Neck" },
        { "Neck", "SpineMid" },
        { "SpineBase", "SpineMid" },
        { "HipRight", "SpineBase" },
        { "KneeRight", "HipRight" },
        { "AnkleRight", "KneeRight" },
        { "FootRight", "AnkleRight" },
        { "HipLeft", "SpineBase" },
        { "KneeLeft", "HipLeft" },
        { "AnkleLeft", "KneeLeft" },
        { "FootLeft", "AnkleLeft" },
        { "Head", "Neck" }
    };

    public Skeleton()
    {
        origin = new Vector3(0f, 4f, -20f);
        humanoidOrigin = new Vector3(-5f, 4f, -20f);
        scale = 0.5f;
        rotation = 0.0f;
    }

    public void AddJoint(Joint joint)
    {
        string name = joint.GetJointName();
        kinectJoints[name] = joint;
    }

    public void AddHumanoidJoint(Joint joint)
    {
        string name = joint.GetJointName();
        humanoidJoints[name] = joint;
    }

    public Vector3 GetOrigin()
    {
        return origin;
    }

    public Vector3 GetHumanoidOrigin()
    {
        return humanoidOrigin;
    }

    public void UpdateOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void UpdateHumanoidOrigin(Vector3 humanoidOrigin)
    {
        this.humanoidOrigin = humanoidOrigin;
    }

    public float GetScale()
    {
        return scale;
    }

    public void UpdateScale(float scale)
    {
        this.scale = scale;
    }

    public float GetRotation()
    {
        return rotation;
    }

    public void UpdateRotation(float rot)
    {
        this.rotation = rot;
    }

    public Dictionary<string, Joint> GetKinectJoints()
    {
        return kinectJoints;
    }

    public Quaternion GetRotation(string kinectJointName)
    {
        Transform target = kinectJoints[kinectJointName].GetJointTransform();
        Transform transform = kinectJoints[jointMap[kinectJointName]].GetJointTransform();
        Vector3 relativePos = target.localPosition - transform.localPosition;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        return rotation;
    }

    public Vector3 GetNextTransformPosition(string kinectJointName, bool isInstructor)
    {
        if (!isInstructor)
        {
            return kinectJoints[jointMap[kinectJointName]].GetJointTransformPosition() * this.GetScale() + origin;
        }
        else
        {
            return humanoidJoints[jointMap[kinectJointName]].GetJointTransform().position;
        }
    }

    public void SetToFrameSkeleton(FrameSkeleton frameSkeleton)
    {
        List<FrameSkeletonJoint> fsjs = frameSkeleton.GetFrameSkeletonJoints();

        foreach (FrameSkeletonJoint fsj in fsjs)
        {
            Vector3 position = fsj.GetPosition() * this.GetScale() + humanoidOrigin;
            humanoidJoints[fsj.GetJointName()].GetJointTransform().position = position;
        }
    }
}
