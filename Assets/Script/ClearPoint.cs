using UnityEngine;

public class ClearPoint : MonoBehaviour
{
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;

    private Animator anim;
    private bool firstTouch;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerCheck.isOn && !firstTouch )
        {
            anim.Play("Clear");
            firstTouch = true;
            Destroy(gameObject, 1f);
        }
    }
}
