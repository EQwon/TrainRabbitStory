using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceCard : MonoBehaviour
{
    
    [SerializeField] private Text choiceText;

    public void SetChoice(List<string> texts, int myNum)
    {
        choiceText.text = texts[myNum];
        int total = texts.Count;

        float myPos = (total - 1) * 50 - 100 * myNum;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, myPos);

        GetComponent<Button>().onClick.AddListener(delegate { UIManager.instance.Choose(myNum); });
        Debug.Log(myNum + "을 할당했습니다.");
    }
}
