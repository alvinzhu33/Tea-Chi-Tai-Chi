using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class RecordingManager {
    private Skeleton skeleton;
    private RecordingData recordingData;

    private string READ_FILE = "scott.dat";

    public RecordingManager(Skeleton skeleton)
    {
        this.skeleton = skeleton;
        recordingData = new RecordingData();
    }

	public void Poll()
    {
        Frame frame = new Frame(System.DateTime.UtcNow);
        FrameSkeleton frameSkeleton = new FrameSkeleton();
        Dictionary<string, Joint> joints = skeleton.GetKinectJoints();
        foreach (KeyValuePair<string, Joint> entry in joints)
        {
            string name = entry.Key;
            Joint joint = entry.Value;
            Vector3 pos = joint.GetJointTransformPosition();
            FrameSkeletonJoint fsj = new FrameSkeletonJoint(name, pos.x, pos.y, pos.z);
            frameSkeleton.AddJoint(fsj);
        }

        recordingData.AddFrameAndFrameSkeleton(frame, frameSkeleton);
    }

    public void WriteToFile()
    {
        FileStream fs = new FileStream("Data.dat", FileMode.Create);

        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            formatter.Serialize(fs, recordingData);
            Debug.Log("Successfully serialized");
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to serialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }

    public RecordingData ReadFromFile()
    {
        FileStream fs = new FileStream(READ_FILE, FileMode.Open);

        BinaryFormatter formatter = new BinaryFormatter();

        try
        {
            recordingData = (RecordingData)formatter.Deserialize(fs);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to deserialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }

        return recordingData;
    }
}
