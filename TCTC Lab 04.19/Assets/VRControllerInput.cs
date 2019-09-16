using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRControllerInput : MonoBehaviour
{
    public Skeleton skeleton;
    public Skeleton instructorSkeleton;
    [SerializeField] protected SteamVR_TrackedController controllerRight;
    [SerializeField] protected SteamVR_TrackedController controllerLeft;
    private bool prevPressedRight;
    private bool prevPressedLeft;
    private bool prevScaling;
    private float startRx = 0.0f;
    private float startRy = 0.0f;
    private float startRz = 0.0f;
    private float startLx = 0.0f;
    private float startLy = 0.0f;
    private float startLz = 0.0f;
    private Vector3 startOrigin;
    private Vector3 startInstructorOrigin;

    private bool PLAY_RECORDING = true;

    private float startRotation;
    private Vector2 startJoystick;

    private bool isRecording;
    private bool isPlayingRecording;
    private bool finishedPlayingRecording = false;
    private int recordingIndex;

    private Camera cam;
    private GameObject camera;
    private bool flyingCamera = false;
    private float camVel = 0.0f;
    private float alpha = 0.0f;

    private RecordingManager recordingManager;
    private RecordingData recordingData;

    private void Awake()
    {
        skeleton = new Skeleton();
        instructorSkeleton = new Skeleton();

        // Play music
        AudioManager.singleton.PlayEvent(AudioManager.play_taichi, gameObject); // 26 seconds
        Invoke("StartRoutine", 26);
        //AudioManager.singleton.PlayEvent(AudioManager.play_piano, gameObject); // 38 seconds

        recordingManager = new RecordingManager(skeleton);
        recordingData = recordingManager.ReadFromFile();

        camera = GameObject.FindWithTag("CameraRig");
    }

    private void FlyOut()
    {
        flyingCamera = true;
        camVel = 0.0f;
    }

    private void Update()
    {
        if (controllerRight == null || controllerLeft == null)
        {
            return;
        }

        if (flyingCamera)
        {
            camVel += 0.001f * Time.deltaTime / (1 / 80f);
            camera.transform.position += new Vector3(0, camVel, 0);
        }

        var rightDevice = SteamVR_Controller.Input((int)controllerRight.controllerIndex);
    
        bool menuPressed = rightDevice.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu);
        if (menuPressed)
        {
            if (!isRecording)
            {
                isRecording = true;
                Debug.Log("STARTED RECORDING");
                cam = PrimaryCameraIndicator.cam;
                cam.backgroundColor = new Color32(0xD2, 0x6B, 0x6B, 0x05);

                recordingManager = new RecordingManager(skeleton);
            }
            else
            {
                isRecording = false;
                Debug.Log("STOPPED RECORDING");
                cam = PrimaryCameraIndicator.cam;
                cam.backgroundColor = new Color32(0x31, 0x4D, 0x79, 0x05);

                recordingManager.WriteToFile();
            }
        }
        if (isRecording)
        {
            recordingManager.Poll();
        }

        bool gripPressed = rightDevice.GetPressDown(EVRButtonId.k_EButton_Grip);
        if (gripPressed)
        {
            StartPlayingRecording();
        }

        if (isPlayingRecording)
        {
            if (recordingIndex < recordingData.GetSize())
            {
                Frame frame = recordingData.GetFrame(recordingIndex);
                
                FrameSkeleton frameSkeleton = recordingData.GetFrameSkeleton(recordingIndex);
                instructorSkeleton.SetToFrameSkeleton(frameSkeleton);

                recordingIndex += 1;
            }
            else
            {
                isPlayingRecording = false;
                finishedPlayingRecording = true;
            }
        }
        else
        {
            Frame frame;
            if (finishedPlayingRecording)
            {
                frame = recordingData.GetFrame(recordingData.GetSize() - 1);
            }
            else
            {
                frame = recordingData.GetFrame(0);
            }

            FrameSkeleton frameSkeleton = recordingData.GetFrameSkeleton(0);
            instructorSkeleton.SetToFrameSkeleton(frameSkeleton);
        }

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
            startOrigin = skeleton.GetOrigin();
            startInstructorOrigin = instructorSkeleton.GetHumanoidOrigin();

            if (controllerRight.triggerPressed)
            {
                prevPressedRight = true;
            }
            if (controllerLeft.triggerPressed)
            {
                prevPressedLeft = true;
            }
        }

        if ((controllerRight.triggerPressed || controllerLeft.triggerPressed) && !prevScaling)
        {
            float moveXR = (rx - startRx);
            float moveYR = (ry - startRy);
            float moveZR = (rz - startRz);
            float moveXL = (lx - startLx);
            float moveYL = (ly - startLy);
            float moveZL = (lz - startLz);

            moveXR *= 15;
            moveYR *= 15;
            moveZR *= 15;
            moveXL *= 15;
            moveYL *= 15;
            moveZL *= 15;

            if (controllerRight.triggerPressed || !controllerLeft.triggerPressed)
            {
                skeleton.UpdateOrigin(startOrigin + new Vector3(moveXR, moveYR, moveZR));
            }
            if (!controllerRight.triggerPressed || controllerLeft.triggerPressed)
            {
                instructorSkeleton.UpdateHumanoidOrigin(startInstructorOrigin + new Vector3(moveXL, moveYL, moveZL));
            }
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

        if (rightDevice.GetTouchDown(EVRButtonId.k_EButton_Axis0))
        {
            startJoystick = rightDevice.GetAxis(EVRButtonId.k_EButton_Axis0);
            startRotation = skeleton.GetRotation();
        }

        if (rightDevice.GetTouch(EVRButtonId.k_EButton_Axis0))
        {
            Vector2 rot = rightDevice.GetAxis(EVRButtonId.k_EButton_Axis0);
            Debug.Log(Mathf.Atan2((rot.y - startJoystick.y), (rot.x - startJoystick.x)));
            skeleton.UpdateRotation(startRotation + Mathf.Atan2((rot.y - startJoystick.y), (rot.x - startJoystick.x)));
        }
    }

    private void StartRoutine()
    {
        AudioManager.singleton.PlayEvent(AudioManager.play_piano, gameObject); // 38 seconds
        Invoke("FlyOut", 38);
        if (PLAY_RECORDING)
        {
            StartPlayingRecording();
        }
    }

    private void StartPlayingRecording()
    {
        isPlayingRecording = true;
        recordingIndex = 0;

        recordingManager = new RecordingManager(skeleton);
        recordingData = recordingManager.ReadFromFile();
    }
}
