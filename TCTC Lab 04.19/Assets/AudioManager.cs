//Place this script in an empty game object anywhere. This script must be in the scene for communication between Wwise and Unity to work properly



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager singleton = null;

    uint bankID;

    /* !!Here is where you will list off your play and stop events and your RTPCs. 
    The event MUST be written here to work in the rest of your project
    EVENT AND RTPC NAMES MUST MATCH THOSE IN YOUR WWISE PROJECT!! */

	public const string play_piano = "play_piano";
	public const string play_taichi = "play_taichi";

   


    void Awake () 
	{
            DontDestroyOnLoad(transform.gameObject);

            if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }
		
        //!!replace the string in here with whatever the name of your soundbank is

		LoadBank ("soundbank");

	}

	// Loadbank function
	public void LoadBank(string _bankName) 
	{
		AkSoundEngine.LoadBank (_bankName, AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
	}


	//Wwise play event
	public void PlayEvent(string eventName, GameObject obj) 
	{
		AkSoundEngine.PostEvent(eventName, obj);
	}
	
    public void SetRTPC(string _rtpcName, float _rtpcValue)
    {
        AkSoundEngine.SetRTPCValue(_rtpcName, _rtpcValue);
    }
		

    void Start()
    {
        
    }


    
}


/*
    Line of code to put for play events in other scripts throughout the project: 
    AudioManager.singleton.PlayEvent (AudioManager.play_example, gameObject);

    Line of code to get RTPCs in other scripts:
    AudioManager.singleton.SetRTPC (AudioManager.rtpc_example, float);
*/
