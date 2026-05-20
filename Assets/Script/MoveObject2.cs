using UnityEngine;

public class MoveObject2 : MonoBehaviour
{
    [Header("移動経路")] public GameObject[] movePoint;
    [Header("速さ")] public float speed = 1.0f;


    private Rigidbody2D rb;
    private ObjectCollision oc;
    private int nowPoint = 0;
    private bool isOn = false;
    private bool returnPoint = false;
    private Vector2 oldPos = Vector2.zero;
    private Vector2 myVelocity = Vector2.zero;
    private Vector2 startPosition = Vector2.zero;
    private float timeSinceLastStep = 0.0f;
    private const float maxInactiveTime = 3.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oc = GetComponent<ObjectCollision>();
        
        if (movePoint != null && movePoint.Length > 0 && rb != null && oc != null)
        {
            rb.position = movePoint[0].transform.position;
            oldPos = rb.position;
            startPosition = rb.position;
        }
    }
    private void Update()
    {
        if (oc.playerStepOn)
        {
            isOn = true;
            timeSinceLastStep = 0.0f;
        }
        else if (isOn) // プレイヤーが乗っていない、かつ動作中
        {
            timeSinceLastStep += Time.deltaTime;
            if (timeSinceLastStep >= maxInactiveTime)
            {
                ReturnToStart();
            }
        }
    }

    private void ReturnToStart()
    {
        // isOnを先にfalseにすることで、FixedUpdate内の移動処理を即座に止める
        isOn = false;

        if (rb != null)
        {
            // 物理的な速度もリセット
            rb.linearVelocity = Vector2.zero;
            // 直接位置を書き換える
            rb.position = startPosition;
            transform.position = startPosition;
        }

        nowPoint = 0;
        returnPoint = false;
        myVelocity = Vector2.zero;
        oldPos = startPosition;
        timeSinceLastStep = 0.0f;
    }
    /*private void Update()
    {
        //プレイヤーが1回でも乗ったらフラグをオンに
        if (oc.playerStepOn)
        {
            // プレイヤーが乗っている間はオンにしてタイマーをリセット
            isOn = true;
            timeSinceLastStep = 0.0f;
        }
        else
        {
            // プレイヤーがいない状態で既に動作開始している場合はタイマーを進める
            if (isOn)
            {
                timeSinceLastStep += Time.deltaTime;
                if (timeSinceLastStep >= maxInactiveTime)
                {
                    ReturnToStart();
                }
            }
        }
    }

    private void ReturnToStart()
    {
        // 元の位置に戻して移動を止める
        if (rb != null)
        {
            rb.MovePosition(startPosition);
            rb.position = startPosition;
        }

        nowPoint = 0;
        returnPoint = false;
        isOn = false;
        myVelocity = Vector2.zero;
        oldPos = startPosition;
        timeSinceLastStep = 0.0f;
    }*/

    public Vector2 GetVelocity()
    {
        return myVelocity;
    }

    private void FixedUpdate()
    {
        if (movePoint != null && movePoint.Length > 1 && rb != null && isOn)
        {
            //通常進行
            if (!returnPoint)
            {
                int nextPoint = nowPoint + 1;

                //目標ポイントとの誤差がわずかになるまで移動
                if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                {
                    //現在地から次のポイントへのベクトルを作成
                    Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

                    //次のポイントへ移動
                    rb.MovePosition(toVector);
                }
                //次のポイントを１つ進める
                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    ++nowPoint;
                    //現在地が配列の最後だった場合
                    if (nowPoint + 1 >= movePoint.Length)
                    {
                        returnPoint = true;
                    }
                }
            }
            //折返し進行
            else
            {
                int nextPoint = nowPoint - 1;

                //目標ポイントとの誤差がわずかになるまで移動
                if (Vector2.Distance(transform.position, movePoint[nextPoint].transform.position) > 0.1f)
                {
                    //現在地から次のポイントへのベクトルを作成
                    Vector2 toVector = Vector2.MoveTowards(transform.position, movePoint[nextPoint].transform.position, speed * Time.deltaTime);

                    //次のポイントへ移動
                    rb.MovePosition(toVector);
                }
                //次のポイントを１つ戻す
                else
                {
                    rb.MovePosition(movePoint[nextPoint].transform.position);
                    --nowPoint;
                    //現在地が配列の最初だった場合
                    if (nowPoint <= 0)
                    {
                        returnPoint = false;
                    }
                }
            }
            myVelocity = (rb.position - oldPos) / Time.deltaTime;
            oldPos = rb.position;
        }
    }

    // プレイヤーが離れた瞬間に、MoveObject2側でocのフラグを無理やり折る
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            oc.playerStepOn = false;
        }
    }
}
