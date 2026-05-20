using Unity.VisualScripting;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    [Header("加算するスコア")] public int myScore;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [Header("アイテム取得時に鳴らすSE")] public AudioClip itemSE;
    void Update()
    {
        //プレイヤーが判定内に入ったら
        if (playerCheck.isOn)
        {
            if (GManager.instance != null)
            {
                GManager.instance.score += myScore;
                GManager.instance.PlaySE(itemSE);
                Destroy(this.gameObject);
            }
        }
    }
}
