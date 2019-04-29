using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogType {Default, Quest};

[System.Serializable]
public class Dialog
{
    public DialogType type;
    public string Speaker;
    public string Text;
    public int questNum;
}

public class Dialogue : MonoBehaviour
{
    public List<Dialog> dialogue;
}
