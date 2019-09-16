using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerInput : MonoBehaviour
{
    protected SteamVR_TrackedController controllerRight;
    protected SteamVR_TrackedController controllerLeft;
    private bool prevPressedRight;
    private bool prevPressedLeft;
    private bool prevScaling;
    private float startRx = 0.0f;
    private float startRy = 0.0f;
    private float startRz = 0.0f;
    private float startLx = 0.0f;
    private float startLy= 0.0f;
    private float startLz = 0.0f;
    private float startSizeX;
    private float startSizeZ;
    private Vector3 startOrigin;

    void Update()
    {
        controllerRight = GetComponent<SteamVR_TrackedController>();
        controllerLeft = GameObject.FindWithTag("ControllerLeft").GetComponent<SteamVR_TrackedController>();
        MeshTester mesh = GameObject.FindWithTag("Mesh").GetComponent<MeshTester>();

        float rx = controllerRight.transform.position.x;
        float ry = controllerRight.transform.position.y;
        float rz = controllerRight.transform.position.z;
        float lx = controllerLeft.transform.position.x;
        float ly = controllerLeft.transform.position.y;
        float lz = controllerLeft.transform.position.z;

        if ((controllerRight.triggerPressed && !prevPressedRight) || (controllerLeft.triggerPressed && !prevPressedLeft))
        {
            startRx = rx;
            startRy = ry;
            startRz = rz;
            startLx = lx;
            startLy = ly;
            startLz = lz;
            startSizeX = mesh.sizeX;
            startSizeZ = mesh.sizeZ;
            startOrigin = mesh.origin;

            if (controllerRight.triggerPressed)
            {
                prevPressedRight = true;
            }
            if (controllerLeft.triggerPressed)
            {
                prevPressedLeft = true;
            }

        }

        if (controllerRight.triggerPressed && controllerLeft.triggerPressed)
        {
            float additionX = (rx - startRx) - (lx - startLx);
            float additionZ = (rz - startRz) - (lz - startLz);
            if (startLx > startRx)
            {
                additionX *= -1;
            }
            if (startLz > startRz)
            {
                additionZ *= -1;
            }

            additionX *= 10;
            additionZ *= 10;
           
            mesh.UpdateScale(startSizeX + additionX, startSizeZ + additionZ);

            prevScaling = true;
        }

        if (controllerRight.triggerPressed && !controllerLeft.triggerPressed && !prevScaling)
        {
            float moveX = (rx - startRx);
            float moveY = (ry - startRy);
            float moveZ = (rz - startRz);

            moveX *= 2;
            moveY *= 2;
            moveZ *= 2;

            mesh.UpdateOrigin(startOrigin + new Vector3(moveX, moveY, moveZ));
        }

        if (!controllerRight.triggerPressed)
        {
            prevPressedRight = false;
        }
        if (!controllerLeft.triggerPressed)
        {
            prevPressedLeft = false;
        }
        if (!controllerRight.triggerPressed && !controllerLeft.triggerPressed)
        {
            prevScaling = false;
        }
    }
}
