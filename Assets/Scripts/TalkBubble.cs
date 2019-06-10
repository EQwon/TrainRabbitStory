using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBubble : MonoBehaviour
{
    private GameObject bunny;
    private Sprite originSprite;

    private void Awake()
    {
        bunny = transform.parent.gameObject;
        originSprite = GetComponent<SpriteRenderer>().sprite;

        if (bunny.GetComponent<Dialogue>().quest == Quest.None)
        {
            GetComponent<Animator>().enabled = false;
        }
    }

    public void ChangeBubbleState()
    {
        if (GameManager.instance.gameObject.GetComponent<QuestManager>().isSuccess[(int)bunny.GetComponent<Dialogue>().quest] == true)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = originSprite;
        }
    }
}
