using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skipping : MonoBehaviour
{
    [Header("Object Holder")]
    [SerializeField] private GameObject phoneScreen;
    [SerializeField] private List<GameObject> answerButtons;

    [Header("Resource Holder")]
    [SerializeField] private GameObject teacherTextPrefab;
    [SerializeField] private GameObject myTextPrefab;
    [SerializeField] private TextAsset skippingTextAsset;

    private void Start()
    {
    }

    private void TeacherSpeak(string script)
    {
        GameObject teacherText = Instantiate(teacherTextPrefab, phoneScreen.transform);

        teacherText.GetComponent<CarrotTalkSizeFitter>().AssignText(script);
    }

    private void MySpeak(string script)
    {
        GameObject myText = Instantiate(myTextPrefab, phoneScreen.transform);

        myText.GetComponent<CarrotTalkSizeFitter>().AssignText(script);
    }

    public void Answer(int i)
    {

    }
}
