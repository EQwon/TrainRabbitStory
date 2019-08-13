using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    public float speed;
    public Vector2 moveDir;

    private float existTime = 0.2f;
    private float time;

    private void Update()
    {
        time += Time.deltaTime;

        //Move to direction
        transform.position = (Vector2)transform.position + moveDir * time * speed;

        //Fade Out
        FadeOut(gameObject);
        FadeOut(gameObject.transform.GetChild(0).gameObject);


        if (time >= existTime)
        {
            //Self Destruct
            Destroy(gameObject);
        }
    }

    private void FadeOut(GameObject thing)
    {
        Color originColor = thing.GetComponent<Image>().color;
        thing.GetComponent<Image>().color = new Color(originColor.r, originColor.g, originColor.b, 1f - time);
    }
}
