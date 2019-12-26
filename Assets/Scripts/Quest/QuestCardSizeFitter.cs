using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCardSizeFitter : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Text description;

    private float titleHeight;
    private float descriptionHeight;
    private RectTransform cardRect;

    private void Start()
    {
        cardRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        titleHeight = title.rectTransform.sizeDelta.y;
        descriptionHeight = description.rectTransform.sizeDelta.y;
        cardRect.sizeDelta = new Vector2(cardRect.sizeDelta.x, titleHeight + descriptionHeight + 35);
    }

    public void SetCard(string title, string description)
    {
        this.title.text = title;
        this.description.text = description;
    }
}
