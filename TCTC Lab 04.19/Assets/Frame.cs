using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable()]
public class Frame {
    private DateTime dt;

    public Frame(DateTime dt)
    {
        this.dt = dt;
    }
}
