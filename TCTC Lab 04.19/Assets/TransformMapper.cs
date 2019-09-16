using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMapper : MonoBehaviour {
    [SerializeField] BodySourceView bodySourceView;
    [SerializeField] string kinectJointName;
    [SerializeField] bool isInstructor;

    Transform source;

    Skeleton skeleton;
    Skeleton instructorSkeleton;

    private float prevRot = 0;

    LineRenderer lr;

    Joint joint;

    private void Start()
    {
        if (!isInstructor)
        {
            bodySourceView.onCreateJoint += HandleJointCreated;
        }

        VRControllerInput controllerInput = GameObject.FindWithTag("ControllerInput").GetComponent<VRControllerInput>();
        skeleton = controllerInput.skeleton;
        instructorSkeleton = controllerInput.instructorSkeleton;

        if (isInstructor)
        {
            Joint joint = new Joint(kinectJointName, this.transform);
            instructorSkeleton.AddHumanoidJoint(joint);
        }

        if (kinectJointName != "SpineMid")
        {
            lr = this.gameObject.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.useWorldSpace = true;
            lr.startWidth = 0.5f;
            lr.endWidth = 0.5f;
            if (!isInstructor)
            {
                lr.material.color = Color.magenta;
            }
            else
            {
                lr.material.color = Color.green;
            }
        }
    }

    private void HandleJointCreated(string jointName, Transform jointTransform)
    {
        if (jointName == kinectJointName && !isInstructor)
        {
            this.source = jointTransform;
            joint = new Joint(jointName, jointTransform);
            skeleton.AddJoint(joint); 
        }
    }

    // Update is called once per frame
    void LateUpdate () {
        if (source != null && !isInstructor)
        {
            //float rot = skeleton.GetRotation();

            this.transform.position = joint.GetJointTransformPosition() * skeleton.GetScale() + skeleton.GetOrigin();

            //this.transform.RotateAround(skeleton.GetOrigin(), Vector3.up, (rot - prevRot) * 180f / Mathf.PI);

            //this.transform.position += skeleton.GetOrigin();

            //prevRot = rot;
        }

        if ((source != null || isInstructor) && kinectJointName != "SpineMid")
        {
            //this.transform.rotation = skeleton.GetRotation(kinectJointName);

            lr.SetPosition(0, this.transform.position);
            if (!isInstructor)
            {
                lr.SetPosition(1, skeleton.GetNextTransformPosition(kinectJointName, isInstructor));
            }
            else
            {
                lr.SetPosition(1, instructorSkeleton.GetNextTransformPosition(kinectJointName, isInstructor));
            }
        }
    }
}
