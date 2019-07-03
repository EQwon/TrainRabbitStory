using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostgraduateTutorial : MonoBehaviour
{
    private void Update()
    {
        if (Player.instance.gameObject.transform.position.x >= 29f)
        {
            UIManager.instance.Warning("다른 토끼에게 도움을 요청해보자!");

            float posY = Player.instance.gameObject.transform.position.y;
            Player.instance.gameObject.transform.position = new Vector3(28.5f, posY, 0);
        }
    }
}
