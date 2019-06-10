using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance = null;

    public float speed = 5f;
    public int damage = 10;
    public float attackDelay = 5f;

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
    }

    private void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)(-100 * transform.position.y);

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (GameManager.instance.canMove == true)
        {
            FlippingPlayer(horizontal); //플레이어 좌우 반전
            Move(horizontal, vertical); //플레이어 움직여!!
            Attack();
        }

        UIManager.instance.AdjustStatusBar();
        attackedTime += Time.deltaTime;
    }

    private void Move(int xDir, int yDir)
    {
        Vector2 newPosition = rb2D.position + new Vector2(xDir, yDir) * speed * Time.deltaTime;
        rb2D.MovePosition(newPosition);

        if (xDir != 0 || yDir != 0) animator.SetBool("playerWalk", true);
        else animator.SetBool("playerWalk", false);
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.A) && attackedTime >= attackDelay)
        {
            transform.GetChild(1).GetComponent<Attack>().ApplyDamage();
            animator.SetTrigger("playerAttack");
            attackedTime = 0;
        }
    }

    private void FlippingPlayer(int horizontal)
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
        bool finishQuest = !GameManager.instance.gameObject.GetComponent<QuestManager>().CheckUnperformQuest();
        if (coll.gameObject.tag == "Door" && finishQuest == true)
        {
            GameManager.instance.canMove = false;
            Invoke("Restart", 0.1f);
        }
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Bunny")
        {
            if (coll.gameObject.GetComponent<Bunny>().isInteractive == false)
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

    private void Restart()
    {
        GameManager.instance.ChangeMoveState(false);
        GameManager.instance.NextStage();
    }
}