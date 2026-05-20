using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class CoollTimeMarker : MonoBehaviour
{
    public Image coolTimeImage;
    public Player player;

    [HideInInspector]public float timer = 0f;

    void Update()
    {
        if(Time.time < player.nextShotTime)
        {
            timer += Time.deltaTime;
            coolTimeImage.fillAmount =  timer / player.shotInterval;
        }
    }
}
