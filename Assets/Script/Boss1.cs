using UnityEngine;
using System.Collections;

public class Boss1 : MonoBehaviour
{
    #region//インスペクターで設定する
    [Header("加算スコア")] public int myScore;
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("画面外でも行動する")] public bool nonVisibleAct;
    [Header("接触判定")] public EnemyCollisionCheck checkCollision;
    [Header("やられた時に鳴らすSE")] public AudioClip deadSE;
    [Header("最小サイズ")] public float minScale = 1.0f;
    [Header("最大サイズ")] public float maxScale = 2.0f;
    [Header("サイズが変わるスピード")] public float scaleSpeed = 2.0f;
    [Header("最大体力(踏まれる回数)")] public int maxHp = 3;
    [Header("大きくなった時の色")] public Color bigColor = Color.red;
    [HideInInspector] public bool isShot = false;

    [Header("--- 死亡時のオブジェクト連動 ---")]
    [Header("連動して【消したい】オブジェクト（複数可）")]
    public GameObject[] targetsToDestroy;

    [Header("連動して【出現させたい】プレハブ（複数可）")]
    public GameObject[] spawnPrefabs;
    #endregion

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool rightTleftF = false;
    private bool isDead = false;

    private int currentHp;
    private bool isInvincible = false; // ダメージを受けた後の無敵状態かどうか

    private bool lastCollisionState = false;
    private float timer = 0f;
    private bool canTakeShotDamage = false; // ショットが効く状態かどうかのフラグ
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();

        currentHp = maxHp;
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            transform.Rotate(new Vector3(0, 0, 5));
            return;
        }

        // プレイヤーに踏まれた、または「ショットが有効な時にショットが当たった」場合
        if (oc.playerStepOn || (isShot && canTakeShotDamage))
        {
            if (!isInvincible)
            {
                StartCoroutine(TakeDamageAndBlink());
            }
            else //無敵中
            {
                oc.playerStepOn = false;
                isShot = false; // 無敵中ならショットを無効化
            }
        }
        else
        {
            // ショットが無効な時に当たった場合は、フラグだけ戻してノーダメージ
            if (isShot && !canTakeShotDamage)
            {
                isShot = false;
            }

            if (sr.isVisible || nonVisibleAct)
            {
                // 壁（障害物）に接触した瞬間は「向きの反転」だけを行う
                if (checkCollision != null && checkCollision.isOn && !lastCollisionState)
                {
                    rightTleftF = !rightTleftF;
                }

                if (checkCollision != null)
                {
                    lastCollisionState = checkCollision.isOn;
                }

                // --- 常に一定間隔でサイズを変更する処理 ---
                timer += Time.fixedDeltaTime * scaleSpeed;
                float lerpValue = Mathf.PingPong(timer, 1.0f);
                float currentScale = Mathf.Lerp(minScale, maxScale, lerpValue);

                // --- 大きくなっている時の色変更 ＆ ショット受付判定 ---
                // 変更具合（lerpValue）が 0.7（70%以上大きくなっている時）を基準にします
                if (lerpValue > 0.7f)
                {
                    canTakeShotDamage = true;
                    if (sr != null && !isInvincible) // 点滅中は色を上書きしない
                    {
                        // 通常の色からインスペクターで設定した色（デフォルトは赤）へ滑らかに変える
                        sr.color = Color.Lerp(Color.white, bigColor, (lerpValue - 0.7f) / 0.3f);
                    }
                }
                else
                {
                    canTakeShotDamage = false;
                    if (sr != null && !isInvincible)
                    {
                        sr.color = Color.white; // 通常サイズ付近は元の色
                    }
                }

                // 移動の向きを計算
                int xVector = -1;
                if (rightTleftF)
                {
                    xVector = 1;
                    transform.localScale = new Vector3(-currentScale, currentScale, 1);
                }
                else
                {
                    transform.localScale = new Vector3(currentScale, currentScale, 1);
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
        if (collision.collider.tag == "Hydrogen" || collision.collider.tag == "Helium" || collision.collider.tag == "Diamond" || collision.collider.tag == "Graphite" || collision.collider.tag == "Oxygen")
        {
            isShot = true;
        }
    }

    private IEnumerator TakeDamageAndBlink()
    {
        isInvincible = true;
        currentHp--;

        oc.playerStepOn = false;
        isShot = false; // ダメージを受けたらショットフラグをリセット

        if (currentHp <= 0)
        {
            Die();
            yield break;
        }

        float blinkDuration = 3.0f;
        float blinkInterval = 0.1f;
        float blinkTimer = 0f;

        while (blinkTimer < blinkDuration)
        {
            if (sr != null)
            {
                sr.enabled = !sr.enabled;
            }
            yield return new WaitForSeconds(blinkInterval);
            blinkTimer += blinkInterval;
        }

        if (sr != null)
        {
            sr.enabled = true;
            // 無敵が終わったら、その時のサイズに応じた色に戻す
            sr.color = canTakeShotDamage ? bigColor : Color.white;
        }

        isInvincible = false;
    }

    private void Die()
    {
        isDead = true;
        anim.Play("boss1_dead");
        rb.linearVelocity = new Vector2(0, -gravity);
        col.enabled = false;

        if (sr != null)
        {
            sr.color = Color.white; // 死亡時は念のため色を戻す
        }

        if (GManager.instance != null)
        {
            GManager.instance.PlaySE(deadSE);
            GManager.instance.score += myScore;
        }

        // --- 死亡時のオブジェクト連動処理 ---
        //削除
        if (targetsToDestroy != null && targetsToDestroy.Length > 0)
        {
            foreach (GameObject target in targetsToDestroy)
            {
                if (target != null)
                {
                    Destroy(target);
                }
            }
        }

        //出現
        if (spawnPrefabs != null && spawnPrefabs.Length > 0)
        {
            foreach (GameObject prefab in spawnPrefabs)
            {
                if (prefab != null)
                {
                    Instantiate(prefab, transform.position, Quaternion.identity);
                }
            }
        }

        Destroy(gameObject, 3f);
    }

   
}