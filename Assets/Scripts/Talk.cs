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
            GameManager.instance.ChangeMoveState(false);
            interactBunny.GetComponent<Bunny>().FlipBunny(player);
            UIManager.instance.StartTalk(player.transform.position, interactBunny);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "InteractArea" && canTalk == false)
        {
            canTalk = true;
            interactBunny = coll.gameObject.transform.parent.gameObject;
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
        }
    }
}
