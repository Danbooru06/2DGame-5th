using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class Enemy2 : MonoBehaviour
{
    [Header("攻撃オブジェクト")] public GameObject attackObj;
    [Header("攻撃間隔")] public float interval;
    [Header("加算スコア")] public int myScore;
    [Header("重力")] public float gravity;
    [Header("やられた時に鳴らすSE")] public AudioClip deadSE;

    private Rigidbody2D rb = null;
    private BoxCollider2D col = null;
    private Animator anim;
    private List<GameObject> attacks = new List<GameObject>();
    private float timer;
    private bool isDead = false;
    private bool isShot;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        if(anim == null || attackObj == null)
        {
            Debug.Log("設定が足りない");
            Destroy(this.gameObject);
        }
        else
        {
            attackObj.SetActive(false);
        }
    }

    void Update()
    {
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
        if (isShot)
        {
            if (!isDead)
            {
                anim.Play("enemy2_dead");
                rb.linearVelocity = new Vector2(0, -gravity);
                isDead = true;
                col.enabled = false;
                if (GManager.instance != null)
                {
                    GManager.instance.PlaySE(deadSE);
                    GManager.instance.score += myScore;
                }
                Destroy(gameObject, 3f);
                foreach(GameObject a in attacks)
                {
                    if(a != null)
                    {
                        Destroy(a);
                    }
                }
                attacks.Clear();
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 3));
            }
        }
        //通常の状態
        if (currentState.IsName("enemy2_idle"))
        {
            if(timer > interval)
            {
                anim.SetTrigger("attack");
                timer = 0.0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    public void Attack()
    {
        GameObject g = Instantiate(attackObj);
        g.transform.SetParent(transform);
        g.transform.position = attackObj.transform.position;
        g.transform.rotation = attackObj.transform.rotation;
        g.SetActive(true);

        attacks.Add(g);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Diamond")
        {
            isShot = true;
        }
    }
}
