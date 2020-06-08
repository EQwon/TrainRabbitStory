using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    private GameObject player;
    public bool canTalk = false;
    private GameObject interactBunny = null;
    private Joystick joystick = null;

    public GameObject InteractBunny { get { return interactBunny; } }

    private void Update()
    {
        if (joystick == null) joystick = Player.instance.joystick;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "InteractArea" && canTalk == false)
        {
            canTalk = true;
            interactBunny = coll.gameObject.transform.parent.gameObject;
            UIManager.instance.CurrentInteractBunny = interactBunny;
            UIManager.instance.SetTalkButtonState(true);
            //Debug.Log(interactBunny.name + "와 대화 가능");
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "InteractArea")
        {
            //Debug.Log("대화 불가능");

            canTalk = false;
            interactBunny = null;
            UIManager.instance.CurrentInteractBunny = null;
            UIManager.instance.SetTalkButtonState(false);
        }
    }
}
