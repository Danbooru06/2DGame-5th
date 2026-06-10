using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    #region//インスペクターで設定する
    [Header("加算スコア")] public int myScore;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;
    [Header("やられた時に鳴らすSE")] public AudioClip deadSE;
    [HideInInspector] public bool isShot = false;
    #endregion

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool rightTleftF = false;
    private bool isDead = false;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        if (oc.playerStepOn || isShot)
        {
            if (!isDead)
            {
                anim.Play("enemy1_dead");
                rb.linearVelocity = new Vector2(0, -gravity);
                isDead = true;
                col.enabled = false;
                if (GManager.instance != null)
                {
                    GManager.instance.PlaySE(deadSE);
                    GManager.instance.score += myScore;
                }
                Destroy(gameObject, 3f);
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, 5));
            }
        }
        else if (!oc.playerStepOn || !isShot)
        {
            if (sr.isVisible || nonVisibleAct)
            {
                if (checkCollision.isOn)
                {
                    rightTleftF = !rightTleftF;
                }
                int xVector = -1;
                if (rightTleftF)
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                rb.linearVelocity = new Vector2(xVector * speed, -gravity);
                
            }
            else
            {
                rb.Sleep();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Diamond" || collision.collider.tag == "Graphite")
        {
           isShot = true;
        }
    }
}
