using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrotTalkSizeFitter : MonoBehaviour
{
    [SerializeField] private RectTransform textBox;
    [SerializeField] private Text text;

    private float originX;
    private RectTransform textRect;
    private RectTransform myRect;

    private void Start()
    {
        originX = textBox.sizeDelta.x;
        textRect = text.GetComponent<RectTransform>();
        myRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        textBox.sizeDelta = new Vector2(originX, textRect.sizeDelta.y + 30);
        myRect.sizeDelta = new Vector2(myRect.sizeDelta.x, Max(80, textBox.sizeDelta.y));
    }

    public void AssignText(string script)
    {
        text.text = script;
    }

    private float Max(float a, float b)
    {
        return a > b ? a : b;
    }
}
