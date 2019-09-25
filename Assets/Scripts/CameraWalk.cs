using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraWalk : MonoBehaviour
{
    private GameObject player;

    private int cellNum;

    private void Start()
    {
        player = Player.instance.gameObject;
        cellNum = ((int)player.transform.position.x + 10) / 20;
        UIManager.instance.TrainCellNumberUpdate(cellNum + 1);
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        //UIManager.instance.TrainCellNumberUpdate(10 - cellNum);
        if (cellNum != ((int)player.transform.position.x + 10) / 20)
        {
            cellNum = ((int)player.transform.position.x + 10) / 20;
            GameManager.instance.IsCellChanging = true;
            StartCoroutine(MoveCamera());
        }
    }

    public void ZoomInCamera(Vector2 playerPos, GameObject interactBunny)
    {
        float localCamPos_X, localCamPos_Y;

        localCamPos_X = ((playerPos.x + interactBunny.transform.position.x) / 2) - cellNum * 20;
        localCamPos_Y = (playerPos.y + interactBunny.transform.position.y - 2f) / 2;

        if (localCamPos_X > 2.4f)
            localCamPos_X = 2.4f;
        else if (localCamPos_X < -2.4f)
            localCamPos_X = -2.4f;

        transform.position = new Vector3(cellNum * 20 + localCamPos_X, localCamPos_Y, -10f);
        GetComponent<Camera>().orthographicSize = 4f;
    }

    public void ZoomOutCamera()
    {
        transform.position = new Vector3(cellNum * 20f, 0, -10);
        GetComponent<Camera>().orthographicSize = 5.5f;
    }

    private IEnumerator MoveCamera()
    {
        float speed = 20f;
        Vector3 targetPos = new Vector3(cellNum * 20, 0, -10);
        bool isRight = (targetPos.x > transform.position.x);

        while (Vector3.Distance(transform.position, targetPos) > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.1f);

        transform.position = targetPos;
        if (isRight == true) Player.instance.transform.position += new Vector3(0.5f, 0, 0);
        else Player.instance.transform.position += new Vector3(-0.5f, 0, 0);
        UIManager.instance.TrainCellNumberUpdate(cellNum + 1);
        GameManager.instance.IsCellChanging = false;
    }
}
