using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talk : MonoBehaviour
{
    private GameObject player;
    private bool canTalk = false;
    private GameObject interactBunny = null;

    private void Awake()
    {
        player = gameObject.transform.parent.gameObject;
    }

    private void Update()
    {
        if (canTalk == true && Input.GetKeyDown(KeyCode.Z))
        {
            //Debug.Log(interactBunny);
            UIManager.instance.StartTalk(player.transform.position, interactBunny);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "InteractArea" && canTalk == false)
        {
            //Debug.Log("대화 가능");

            canTalk = true;
            interactBunny = coll.gameObject.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "InteractArea")
        {
            //Debug.Log("대화 불가능");

            canTalk = false;
            interactBunny = null;
        }
    }
}
