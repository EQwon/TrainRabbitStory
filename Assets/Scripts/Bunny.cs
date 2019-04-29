using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    //public string bunnyName;    //토끼의 이름
    public bool isSitting; //토끼가 앉아있는지
    public bool isInteractive; //상호작용 할 수 있는지

    private void Update()
    {
        if(isSitting == false)
        {
            GetComponent<SpriteRenderer>().sortingOrder = (int)(-100 * transform.position.y);
        }
    }
}