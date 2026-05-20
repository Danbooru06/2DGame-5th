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

    [Header("--- 追加仕様 ---")]
    [Header("通常サイズ")] public float normalScale = 1.0f;
    [Header("大きいサイズ")] public float bigScale = 2.0f;
    [Header("最大体力(踏まれる回数)")] public int maxHp = 3;
    #endregion

    #region//プライベート変数
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private Animator anim = null;
    private ObjectCollision oc = null;
    private BoxCollider2D col = null;
    private bool rightTleftF = false;
    private bool isDead = false;

    // 追加のプライベート変数
    private int currentHp;
    private bool isBig = false;       // 現在大きいサイズかどうか
    private bool isInvincible = false; // 無敵フラグ
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        oc = GetComponent<ObjectCollision>();
        col = GetComponent<BoxCollider2D>();

        currentHp = maxHp; // 体力を初期化
    }

    void FixedUpdate()
    {
        // 死亡時の処理（完全にやられた場合）
        if (isDead)
        {
            transform.Rotate(new Vector3(0, 0, 5));
            return;
        }

        // プレイヤーに踏まれた時の判定
        if (oc.playerStepOn)
        {
            // 無敵時間中でなければダメージ処理を開始
            if (!isInvincible)
            {
                StartCoroutine(TakeDamageAndBlink());
            }
            else
            {
                // 無敵時間中に再度踏まれてもスルーするように、踏まれ判定をリセット
                oc.playerStepOn = false;
            }
        }
        else
        {
            // 生存時の移動・サイズ変更処理
            if (sr.isVisible || nonVisibleAct)
            {
                // 壁（障害物）に接触した瞬間
                if (checkCollision.isOn)
                {
                    rightTleftF = !rightTleftF; // 向きを反転
                    isBig = !isBig;             // サイズ状態を反転（大きい⇔通常）
                }

                // 向きとサイズを計算に組み込む
                int xVector = -1;
                float currentScaleX = isBig ? bigScale : normalScale;
                float currentScaleY = isBig ? bigScale : normalScale;

                if (rightTleftF)
                {
                    xVector = 1;
                    // 反転（元の向きが左の場合、右を向けるためにマイナスにする）
                    transform.localScale = new Vector3(-currentScaleX, currentScaleY, 1);
                }
                else
                {
                    transform.localScale = new Vector3(currentScaleX, currentScaleY, 1);
                }

                rb.linearVelocity = new Vector2(xVector * speed, -gravity);
            }
            else
            {
                rb.Sleep();
            }
        }
    }

    // ダメージ（踏まれた）時の処理と3秒間の点滅コルーチン
    private IEnumerator TakeDamageAndBlink()
    {
        isInvincible = true; // 無敵開始
        currentHp--;

        // 踏まれた判定を一度リセット
        oc.playerStepOn = false;

        // HPが0になったら死亡
        if (currentHp <= 0)
        {
            Die();
            yield break; // コルーチンをここで終了
        }

        // --- 3秒間の点滅処理 ---
        float blinkDuration = 3.0f; // 点滅させる合計時間
        float blinkInterval = 0.1f; // 点滅の速さ（0.1秒ごと）
        float timer = 0f;

        while (timer < blinkDuration)
        {
            if (sr != null)
            {
                // スプライトの表示・非表示を切り替える
                sr.enabled = !sr.enabled;
            }
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // ループを抜けたら必ずスプロイトを表示状態に戻す
        if (sr != null)
        {
            sr.enabled = true;
        }

        isInvincible = false; // 無敵解除
    }

    // 死亡処理
    private void Die()
    {
        isDead = true;
        anim.Play("enemy1_dead");
        rb.linearVelocity = new Vector2(0, -gravity);
        col.enabled = false;

        if (GManager.instance != null)
        {
            GManager.instance.PlaySE(deadSE);
            GManager.instance.score += myScore;
        }

        Destroy(gameObject, 3f);
    }
}
