using UnityEngine;
using System.Collections;

public class FadeBegin : MonoBehaviour
{
    //name of the scene you want to load
    public string MainScene = "Assets/KinectView/MainScene";
    public Color loadToColor = Color.white;

    public void OnTriggerEnter()
    {
        Debug.Log(MainScene);

        Initiate.Fade(MainScene, loadToColor, 2f);
    }
}