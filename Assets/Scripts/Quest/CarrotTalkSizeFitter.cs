using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrotTalkSizeFitter : MonoBehaviour
{
    [SerializeField] private RectTransform textBox;
    [SerializeField] private Text text;

    private float originX;

    private void Start()
    {
        originX = textBox.sizeDelta.x;
    }

    private void Update()
    {
        textBox.sizeDelta = new Vector2(originX, text.GetComponent<RectTransform>().sizeDelta.y + 30);
    }

    public void AssignText(string script)
    {
        text.text = script;
    }
}
