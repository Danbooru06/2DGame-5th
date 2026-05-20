using UnityEngine;

public class JumpObject : MonoBehaviour
{
    private ObjectCollision oc;
    private Animator anim;
    void Start()
    {
        oc = GetComponent<ObjectCollision>();
        anim = GetComponent<Animator>();
        if(oc == null || anim == null)
        {
            Debug.Log("ジャンプ台の設定が足りない");
            Destroy(this);
        }
    }

    void Update()
    {
        if (oc.playerStepOn)
        {
            anim.SetTrigger("on");
            oc.playerStepOn = false;
        }
    }
}
