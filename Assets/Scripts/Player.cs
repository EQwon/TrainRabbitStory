using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    public float speed = 5f;
    public int hpDamage = 10;
    public int mpDamage = 1;
    public float attackDelay = 5f;

    public Joystick joystick;

    private Rigidbody2D rb2D;
    private Animator animator;
    private float attackedTime = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (joystick == null) joystick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)(-100 * transform.position.y);

        if (GameManager.instance.State == GameManager.TrainState.normal || GameManager.instance.State == GameManager.TrainState.normalQuest)
        {
            FlippingPlayer(joystick.Horizontal);
            Move(joystick.Horizontal, joystick.Vertical);
            Attack();
        }

        UIManager.instance.AdjustStatusBar();
        attackedTime += Time.deltaTime;
    }

    private void Move(float xDir, float yDir) //플레이어 움직여!!
    {
        Vector2 moveDelta = new Vector2(xDir, yDir) * speed * Time.deltaTime;
        rb2D.MovePosition(rb2D.position + moveDelta);

        if (moveDelta.sqrMagnitude >= 0.0001f) animator.SetBool("playerWalk", true);
        else animator.SetBool("playerWalk", false);
    }

    private void Attack()
    {
        if (/*Input.GetKeyDown(KeyCode.A)*/ joystick.Attack && attackedTime >= attackDelay)
        {
            transform.GetChild(1).GetComponent<Attack>().ApplyDamage();
            animator.SetTrigger("playerAttack");
            attackedTime = 0;
        }
    }

    private void FlippingPlayer(float horizontal) //플레이어 좌우 반전
    {
        if(horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Item")
        {
            coll.gameObject.GetComponent<QuestItem>().GetItem();
        }
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bunny")
        {
            if (coll.gameObject.GetComponent<Bunny>().isInteractable == false)
            {
                StartCoroutine(HPDecrease());
            }
        }
    }

    private IEnumerator HPDecrease()
    {
        GameManager.instance.HP = GameManager.instance.HP - 0.1f;

        //Debug.Log("체력 감소");
        yield return new WaitForSeconds(0.5f);
    }
}