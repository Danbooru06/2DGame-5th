using UnityEngine;

public class EnemyCollisionCheck : MonoBehaviour
{
    /// <summary>
    /// 判定内に敵か壁がある
    /// </summary>
    [HideInInspector] public bool isOn = false;
    public Enemy1 enemy1;

    private string groundTag = "Ground";
    private string enemyTag = "Enemy";
    private string hitAreaTag = "HitArea";

    #region//接触判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == groundTag || collision.tag == enemyTag || collision.tag == hitAreaTag)
        {
            isOn = true;
        }
        else if((collision.tag == "Graphite") || (collision.tag == "Diamond"))
        {
            enemy1.isShot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == groundTag || collision.tag == enemyTag || collision.tag == hitAreaTag)
        {
            isOn = false;
        }
    }
    #endregion
}
