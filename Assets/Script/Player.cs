using System.Collections;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region//インスペクターで設定する
    [Header("移動速度")] public float speed;
    [Header("重力")] public float gravity;
    [Header("ジャンプ速度")] public float jumpSpeed;
    [Header("ジャンプする高さ")] public float jumpHeight;
    [Header("ジャンプする長さ")] public float jumpLimitTime;
    [Header("接地判定")] public GroundCheck ground;
    [Header("天井判定")] public GroundCheck head;
    [Header("ダッシュの速さ表現")] public AnimationCurve dashCurve;
    [Header("ジャンプの速さ表現")] public AnimationCurve jumpCurve;
    [Header("踏みつけ判定の高さの割合")] public float stepOnRate;
    [Header("ジャンプする時に鳴らすSE")] public AudioClip jumpSE;
    [Header("やられた時に鳴らすSE")] public AudioClip downSE;
    [Header("コンティニュー時に鳴らすSE")] public AudioClip continueSE;
    [Header("アイテム射撃時に鳴らすSE")] public AudioClip shotSE;
    [Header("射撃位置")] public Transform shootPoint;
    [Header("射撃クールタイム")] public float shotInterval;
    [Header("元素の数")] public int elementCount;
    [Header("クールタイムマーカー")] public CoollTimeMarker coolTimeMarker;

    [HideInInspector]public float nextShotTime = 0.0f; //次に射撃できる時間
    [HideInInspector] public static float dir; //射撃の向き　右なら1、左なら-1
    [HideInInspector] public float verticalmove = 0; //スマホ用の縦移動の入力値
    [HideInInspector] public float horizontalmove = 0; //スマホ用の横移動の入力値

    [Header("---　元素関係 ---")]
    [Header("水")]
    public GameObject water;

    [Header("水素")]
    public GameObject hydrogen;
    public float hydrogenSpeed;
    public float hydrogengravity;
    public float hydrogenjumpSpeed;
    public float hydrogenjumpHeight;
    public float hydrogenjumpLimitTime;
    public RuntimeAnimatorController player_H;

    [Header("ヘリウム")]
    public GameObject helium;
    public float heliumSpeed;
    public float heliumgravity;
    public float heliumjumpSpeed;
    public float heliumjumpHeight;
    public float heliumjumpLimitTime;
    public RuntimeAnimatorController player_He;

    [Header("炭素")]
    public GameObject diamond;
    public GameObject graphite;
    public float diamondSpeed;
    public float graphiteSpeed;
    public float carbongravity;
    public float carbonjumpSpeed;
    public float carbonjumpHeight;
    public float carbonjumpLimitTime;
    public RuntimeAnimatorController player_C_D;
    public RuntimeAnimatorController player_C_G;

    [Header("酸素")]
    public GameObject oxygen;
    public float oxygenSpeed;
    public float oxygengravity;
    public float oxygenjumpSpeed;
    public float oxygenjumpHeight;
    public float oxygenjumpLimitTime;
    public RuntimeAnimatorController player_O;
    #endregion

    #region//プライベート変数 
    private Animator anim = null;
    private Rigidbody2D rb = null;
    private CapsuleCollider2D capcol = null;
    private SpriteRenderer sr = null;
    private MoveObject moveObj = null;
    private MoveObject2 moveObj2 = null;
    private GameObject baseItem = null;
    private GameObject currentItem = null;
    private bool isGround = false;
    private bool isJump = false;
    private bool isRun = false;
    private bool isHead = false;
    private bool isDown = false;
    private bool isOtherJump = false;
    private bool isContinue = false;
    private bool nonDownAnim = false;
    private bool isClearMotion = false;
    private bool shotButtonDown = false;
    private bool changeElementButtonDown = false;
    private bool isKnockback = false;
    private float jumpPos = 0.0f;
    private float otherJumpHeight = 0.0f;
    private float otherJumpSpeed = 0.0f;
    private float dashTime = 0.0f;
    private float jumpTime = 0.0f;
    private float beforeKey = 0.0f;
    private float continueTime = 0.0f;
    private float blinkTime = 0.0f;
    private string enemyTag = "Enemy";
    private string deadAreaTag = "DeadArea";
    private string hitAreaTag = "HitArea";
    private string moveFloorTag = "MoveFloor";
    private string moveFloor2Tag = "MoveFloor2";
    private string fallFloorTag = "FallFloor";
    private string jumpStepTag = "JumpStep";
    private string OxygenTag = "Oxygen";
    private string HydrogenTag = "Hydrogen";
    private string GraphiteTag = "Graphite";
    #endregion

    public void RightButtonDown()
    {
        horizontalmove = 1;
    }

    public void RightButtonUp()
    {
        horizontalmove = 0;
    }

    public void LeftButtonDown()
    {
        horizontalmove = -1;
    }

    public void LeftButtonUp()
    {
        horizontalmove = 0;
    }

    public void JumpButtonDown()
    {
        verticalmove = 1;
    }

    public void JumpButtonUp()
    {
        verticalmove = 0;
    }

    public void ShotButtonDown()
    {
        if(Time.time >= nextShotTime)
        {
            shotButtonDown = true;
        }
    }

    public void ChangeElementButtonDown()
    {
        changeElementButtonDown = true;
    }

    public void SetElement()
    {
        switch (GManager.currentElement)
        {
            case GManager.Element.Hydrogen:
                anim.runtimeAnimatorController = player_H;
                baseItem = hydrogen;
                currentItem = hydrogen;
                GManager.currentItem = GManager.ElementItem.Hydrogen;
                GManager.baseItem = GManager.ElementItem.Hydrogen;
                speed = hydrogenSpeed;
                gravity = hydrogengravity;
                jumpSpeed = hydrogenjumpSpeed;
                jumpHeight = hydrogenjumpHeight;
                jumpLimitTime = hydrogenjumpLimitTime;
                break;

            case GManager.Element.Helium:
                anim.runtimeAnimatorController = player_He;
                baseItem = helium;
                currentItem = helium;
                GManager.currentItem = GManager.ElementItem.Helium;
                GManager.baseItem = GManager.ElementItem.Helium;
                speed = heliumSpeed;
                gravity = heliumgravity;
                jumpSpeed = heliumjumpSpeed;
                jumpHeight = heliumjumpHeight;
                jumpLimitTime = heliumjumpLimitTime;
                break;

            case GManager.Element.Carbon_Diamond:
                anim.runtimeAnimatorController = player_C_D;
                baseItem = diamond;
                currentItem= diamond;
                GManager.currentItem = GManager.ElementItem.Diamond;
                GManager.baseItem = GManager.ElementItem.Diamond;
                speed = diamondSpeed;
                gravity = carbongravity;
                jumpSpeed = carbonjumpSpeed;
                jumpHeight = carbonjumpHeight;
                jumpLimitTime = carbonjumpLimitTime;
                break;

            case GManager.Element.Carbon_Graphite:
                anim.runtimeAnimatorController = player_C_G;
                baseItem = graphite;
                currentItem= graphite;
                GManager.currentItem = GManager.ElementItem.Graphite;
                GManager.baseItem = GManager.ElementItem.Graphite;
                speed = graphiteSpeed;
                gravity = carbongravity;
                jumpSpeed = carbonjumpSpeed;
                jumpHeight = carbonjumpHeight;
                jumpLimitTime = carbonjumpLimitTime;
                break;

            case GManager.Element.Oxygen:
                anim.runtimeAnimatorController = player_O;
                baseItem = oxygen;
                currentItem = oxygen;
                GManager.currentItem = GManager.ElementItem.Oxygen;
                GManager.baseItem = GManager.ElementItem.Oxygen;
                speed = oxygenSpeed;
                gravity = oxygengravity;
                jumpSpeed = oxygenjumpSpeed;
                jumpHeight = oxygenjumpHeight;
                jumpLimitTime = oxygenjumpLimitTime;
                break;
        }
    }
    void Start()
    {
        //コンポーネントのインスタンスを捕まえる
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capcol = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        //元素ごとのパラメータ
        SetElement();
    }

    private void Update()
    {

        //アイテム射撃
        if (!isDown && !GManager.instance.isGameOver && !GManager.instance.isStageClear)
        {
            if ((Input.GetKeyDown(KeyCode.X) && Time.time >= nextShotTime) || (shotButtonDown && Time.time >= nextShotTime))
            {
                ShootItem();
                nextShotTime = Time.time + shotInterval;
                coolTimeMarker.timer = 0f;
                shotButtonDown = false;
            }
            if ((Input.GetKeyDown(KeyCode.C) && GManager.instance.stageType == GManager.StageType.A) || (changeElementButtonDown && GManager.instance.stageType == GManager.StageType.A))
            {
                changeElementButtonDown = false;
                if(GManager.currentElement != (GManager.Element)elementCount - 1)
                {
                    GManager.currentElement++;
                    SetElement();
                }
                else
                {
                    GManager.currentElement = (GManager.Element)0;
                    SetElement();
                }
            }
        }

        if (isContinue)
        {
            //明滅　ついている時に戻る
            if (blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            //明滅　消えている時
            else if (blinkTime > 0.1f)
            {
                sr.enabled = false;
            }
            //明滅　ついている時
            else
            {
                sr.enabled = true;
            }
            //1秒たったら明滅終わり
            if(continueTime > 1.0f)
            {
                isContinue = false;
                blinkTime = 0.0f;
                continueTime = 0.0f;
                sr.enabled = true;
            }
            else
            {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDown && !GManager.instance.isGameOver && !GManager.instance.isStageClear)
        {

            //接地判定を得る
            isGround = ground.IsGround();
            isHead = head.IsGround();

            //各種座標軸の速度を求める
            float xSpeed = GetXSpeed();
            float ySpeed = GetYSpeed();

            //アニメーションを適用
            SetAnimation();

            //移動速度を設定
            Vector2 addVelocity = Vector2.zero;
            if(moveObj != null)
            {
                addVelocity = moveObj.GetVelocity();
            }
            else if(moveObj2 != null)
            {
                addVelocity = moveObj2.GetVelocity();
            }
            if (!isKnockback)
            {
                rb.linearVelocity = new Vector2(xSpeed, ySpeed) + addVelocity;
            }
        }
        else
        {
            if(!isClearMotion && GManager.instance.isStageClear)
            {
                anim.Play("player_win");
                isClearMotion = true;
            }
            rb.linearVelocity = new Vector2(0, -gravity);
        }
    }

    /// <summary> 
    /// Y成分で必要な計算をし、速度を返す。 
    /// </summary> 
    /// <returns>Y軸の速さ</returns> 
    private float GetYSpeed()
    {
        float verticalKey = Input.GetAxis("Vertical") + verticalmove;
        float ySpeed = -gravity;

        //何か踏んだ際のジャンプ
        if (isOtherJump)
        {
            //現在の高さが飛べる高さよりも下か
            bool canHeight = jumpPos + otherJumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎていないか
            bool canTime = jumpLimitTime > jumpTime;

            if (canHeight && canTime && !isHead)
            {
                ySpeed = otherJumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isOtherJump = false;
                jumpTime = 0.0f;
            }
        }
        //地面にいる時
        else if (isGround)
        {
            if (verticalKey > 0)
            {
                if (!isJump)
                {
                    GManager.instance.PlaySE(jumpSE);
                    //炭素の変形
                    if(GManager.instance.stageType == GManager.StageType.A)
                    {
                        if (GManager.currentElement == GManager.Element.Carbon_Diamond)
                        {
                            GManager.currentElement = GManager.Element.Carbon_Graphite;
                            SetElement();
                        }
                        else if (GManager.currentElement == GManager.Element.Carbon_Graphite)
                        {
                            GManager.currentElement = GManager.Element.Carbon_Diamond;
                            SetElement();
                        }
                    }
                    
                }
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //ジャンプした位置を記録する
                isJump = true;
                jumpTime = 0.0f;
            }
            else
            {
                isJump = false;
            }
        }
        //ジャンプ中
        else if (isJump)
        {
            //上方向キーを押しているか
            bool pushUpKey = verticalKey > 0;
            //現在の高さが飛べる高さより下か
            bool canHeight = jumpPos + jumpHeight > transform.position.y;
            //ジャンプ時間が長くなりすぎてないか
            bool canTime = jumpLimitTime > jumpTime;

            if (pushUpKey && canHeight && canTime && !isHead)
            {
                ySpeed = jumpSpeed;
                jumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
                jumpTime = 0.0f;
            }
        }

        if (isJump || isOtherJump)
        {
            ySpeed *= jumpCurve.Evaluate(jumpTime);
        }

        return ySpeed;
    }

    /// <summary> 
    /// X成分で必要な計算をし、速度を返す。 
    /// </summary> 
    /// <returns>X軸の速さ</returns> 
    private float GetXSpeed()
    {
        float horizontalKey = Input.GetAxis("Horizontal") + horizontalmove;
        float xSpeed = 0.0f;

        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isRun = true;
            dashTime += Time.deltaTime;
            xSpeed = speed;
        }
        else if (horizontalKey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isRun = true;
            dashTime += Time.deltaTime;
            xSpeed = -speed;
        }
        else
        {
            isRun = false;
            xSpeed = 0.0f;
            dashTime = 0.0f;
        }

        //前回の入力からダッシュの反転を判断して速度を変える
        if (horizontalKey > 0 && beforeKey < 0)
        {
            dashTime = 0.0f;
        }
        else if (horizontalKey < 0 && beforeKey > 0)
        {
            dashTime = 0.0f;
        }

        beforeKey = horizontalKey;
        xSpeed *= dashCurve.Evaluate(dashTime);
        return xSpeed;
    }

    /// <summary> 
    /// アニメーションを設定する 
    /// </summary> 
    private void SetAnimation()
    {
        anim.SetBool("jump", isJump);
        anim.SetBool("ground", isGround);
        anim.SetBool("run", isRun);
    }

    /// <summary>
    /// コンティニュー待機状態か
    /// </summary>
    /// <return></return>
    public bool IsContinueWaiting()
    {
        if (GManager.instance.isGameOver)
        {
            return false;
        }
        else
        {
            return IsDownAnimEnd() || nonDownAnim;
        }
    }

    //ダウンアニメーションが完了しているかどうか
    private bool IsDownAnimEnd()
    {
        if(isDown && anim != null)
        {
            AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
            if (currentState.IsName("player_lose"))
            {
                if(currentState.normalizedTime >= 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// コンティニューする
    /// </summary>
    /// <return></return>
    public void ContinuePlayer()
    {
        GManager.instance.PlaySE(continueSE);
        isDown = false;
        anim.Play("player_stand");
        isJump = false;
        isOtherJump = false;
        isRun = false;
        isContinue = true;
        nonDownAnim = false;
    }

    //やられた時の処理
    private void ReceiveDamage(bool downAnim)
    {
        if (isDown || GManager.instance.isStageClear)
        {
            return;
        }
        else
        {
            if (downAnim)
            {
                anim.Play("player_lose");
            }
            else
            {
                nonDownAnim = true;
            }
            isDown = true;
            GManager.instance.PlaySE(downSE);
            GManager.instance.SubHeartNum();
        }
    }

    //元素を射撃する
    public void ShootItem()
    {
        Instantiate(currentItem, shootPoint.position, Quaternion.identity);
        GManager.instance.PlaySE(shotSE);
        dir = (shootPoint.position.x >= transform.position.x) ? 1f : -1f;
        currentItem = baseItem;
        GManager.currentItem = GManager.baseItem;
    }

    //ノックバックさせる(水素の爆発など)
    public void Knockback(Vector2 direction, float force)
    {
        isKnockback = true;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        StartCoroutine(KnockbackEnd());
    }

    IEnumerator KnockbackEnd()
    {
        yield return new WaitForSeconds(0.3f);
        isKnockback = false;
    }

    #region//接触判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool enemy = (collision.collider.tag == enemyTag);
        bool moveFloor = (collision.collider.tag == moveFloorTag);
        bool moveFloor2 = (collision.collider.tag == moveFloor2Tag);
        bool fallFloor = (collision.collider.tag == fallFloorTag);
        bool jumpStep = (collision.collider.tag == jumpStepTag);

        if (enemy || moveFloor || moveFloor2 || fallFloor || jumpStep)
        {
            //踏みつけ判定になる高さ
            float stepOnHeight = (capcol.size.y * (stepOnRate / 100f));
            //踏みつけ判定のワールド座標
            float judgePos = transform.position.y - (capcol.size.y / 2f) + stepOnHeight;

            foreach (ContactPoint2D p in collision.contacts)
            {
                if (p.point.y < judgePos)
                {
                    if (enemy || fallFloor || jumpStep || moveFloor2)
                    {
                        ObjectCollision o = collision.gameObject.GetComponent<ObjectCollision>();
                        if (o != null)
                        {
                            if (enemy || jumpStep)
                            {
                                otherJumpHeight = o.boundHeight; //踏んづけたものから跳ねる高さを取得する
                                otherJumpSpeed = o.jumpSpeed; //ジャンプするスピード
                                o.playerStepOn = true; //踏んづけたものに対して踏んづけたことを通知する
                                jumpPos = transform.position.y; //ジャンプした位置を記録する
                                isOtherJump = true;
                                isJump = false;
                                jumpTime = 0.0f;
                            }
                            else if (fallFloor)
                            {
                                o.playerStepOn = true;
                            }
                            else if (moveFloor2)
                            {
                                o.playerStepOn = true;
                                moveObj2 = collision.gameObject.GetComponent<MoveObject2>();
                            }
                        }
                        else
                        {
                            Debug.Log("ObjectCollisionがついていない");
                        }
                    }
                    else if (moveFloor)
                    {
                        moveObj = collision.gameObject.GetComponent<MoveObject>();
                    }
                }
                else
                {
                    if (enemy)
                    {
                        ReceiveDamage(true);
                        break;
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.tag == moveFloorTag || collision.collider.tag == moveFloor2Tag)
        {
            //動く床から離れた
            moveObj = null;
            moveObj2 = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == deadAreaTag)
        {
            ReceiveDamage(false);
        }
        else if((collision.tag == hitAreaTag) || (collision.tag == "fire"))
        {
            ReceiveDamage(true);
        }

        if(collision.tag == OxygenTag)
        {
            if (GManager.currentItem == GManager.ElementItem.Hydrogen)
            {
                GManager.currentItem = GManager.ElementItem.Water;
                currentItem = water;
            }
        }

        if(collision.tag == HydrogenTag)
        {
            if(GManager.currentItem == GManager.ElementItem.Oxygen)
            {
                GManager.currentItem = GManager.ElementItem.Water;
                currentItem = water;
            }
        }
        if(collision.tag == GraphiteTag)
        {
            if(GManager.currentItem != GManager.ElementItem.Graphite)
            {
                GManager.currentItem = GManager.ElementItem.Graphite;
                currentItem = graphite;
            }
        }
    }
    #endregion
}