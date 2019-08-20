using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{
    public static int timeScale { get; set; }
    public static float deltaTime { get { return timeScale > 0 ? Time.deltaTime : 0; } }
}