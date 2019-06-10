using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Quest { None, PhoneCall };

public class QuestManager : MonoBehaviour
{
    public List<bool> isAccept = new List<bool>();
    public List<bool> isSuccess = new List<bool>();

    private void Awake()
    {
        int n = Enum.GetNames(typeof(Quest)).Length;

        for (int i = 0; i < n; i++)
        {
            isAccept.Add(false);
            isSuccess.Add(false);
        }
    }

    public bool CheckUnperformQuest()
    {
        for (int i = 0; i < isAccept.Count; i++)
        {
            if (isAccept[i] == true) return true;
        }
        return false;
    }
}