using Unity.VisualScripting;
using UnityEngine;

public class Electron : MonoBehaviour
{
    [Header("加算するスコア")] public int myScore;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [Header("プレイヤー")]public Player player;
    [Header("アイテム取得時に鳴らすSE")] public AudioClip itemSE;
    [Header("増やす電子の数")] public int n;

    private GManager.Element e;
    private GManager.ElementItem i;
    void Update()
    {
        //プレイヤーが判定内に入ったら
        if (playerCheck.isOn)
        {
            if (GManager.instance != null)
            {
                GManager.instance.score += myScore;
                GManager.instance.PlaySE(itemSE);
                GManager.currentElement += n;
                GManager.currentItem += n;
                GManager.baseItem += n;
                player.SetElement();
                Destroy(this.gameObject);
            }
        }
    }
}
