using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class RecordingData {
    private List<Frame> frames = new List<Frame>();
    private List<FrameSkeleton> frameSkeletons = new List<FrameSkeleton>();

    public RecordingData()
    {

    }

    public void AddFrameAndFrameSkeleton(Frame frame, FrameSkeleton frameSkeleton)
    {
        this.frames.Add(frame);
        this.frameSkeletons.Add(frameSkeleton);
    }

    public int GetSize()
    {
        return frames.Count;
    }

    public Frame GetFrame(int index)
    {
        return frames[index];
    }

    public FrameSkeleton GetFrameSkeleton(int index)
    {
        return frameSkeletons[index];
    }
}
