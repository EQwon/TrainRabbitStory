using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchEffect : MonoBehaviour
{
    public GameObject effectPrefab;
    public int effectAmount = 8;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateEffect(Input.mousePosition);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                CreateEffect(touch.position);
        }
    }

    private void CreateEffect(Vector2 pos)
    {
        for (int i = 0; i < effectAmount; i++)
        {
            float targetAngle = Mathf.Deg2Rad * (Random.Range(0f, 360f / effectAmount) + (360f / effectAmount) * i);
            Vector2 moveDir = new Vector2(Mathf.Cos(targetAngle), Mathf.Sin(targetAngle));
            Vector2 spawnPos = moveDir * Random.Range(25, 40) + pos;
            GameObject effect = Instantiate(effectPrefab, spawnPos, Quaternion.identity, transform);

            effect.GetComponent<EffectController>().moveDir = moveDir;
            effect.GetComponent<Image>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            effect.transform.localScale = new Vector2(1, 1) * Random.Range(0.4f, 1.1f);
        }
    }
}
