using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParsingTest : MonoBehaviour
{
    public TextAsset dialogue;

    private void Start()
    {
        List<List<List<string>>> test = new List<List<List<string>>>();
        test = Parser.DialogParse(dialogue);

        for (int j = 0; j < test.Count; j++)
        {
            for (int i = 0; i < test[j].Count; i++)
            {
                string name = test[j][i][0];
                string conversation = test[j][i][1];

                Debug.Log(j + "번째 대화 - " + name + " : " + conversation);
            }
        }
    }
}
