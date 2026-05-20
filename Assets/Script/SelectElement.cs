using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectElement : MonoBehaviour
{
    [Header("フェード")] public FadeImage fade;
    [Header("ゲームスタート時に鳴らすSE")] public AudioClip startSE;


    private bool firstPush = false; //一度だけ押せるように
    private bool goNextScene = false; //次のシーンに行くかどうか

   

    public void SelectHydrogen()
    {
        if (!firstPush)
        {
            GManager.instance.PlaySE(startSE);
            GManager.currentElement = GManager.Element.Hydrogen;
            GManager.currentItem = GManager.ElementItem.Hydrogen;
            GManager.baseItem = GManager.ElementItem.Hydrogen;
            fade.StartFadeOut();
            firstPush = true;
        }
    }
        

    public void SelectHelium()
    {
        if (!firstPush)
        {
            GManager.instance.PlaySE(startSE);
            GManager.currentElement = GManager.Element.Helium;
            GManager.currentItem= GManager.ElementItem.Helium;
            GManager.baseItem = GManager.ElementItem.Helium;
            fade.StartFadeOut();
            firstPush = true;
        }
    }

    public void SelectCarbon()
    {
        if (!firstPush)
        {
            GManager.instance.PlaySE(startSE);
            GManager.currentElement = GManager.Element.Carbon_Graphite;
            GManager.currentItem = GManager.ElementItem.Graphite;
            GManager.baseItem = GManager.ElementItem.Graphite;
            fade.StartFadeOut();
            firstPush = true;
        }
    }

    public void SelectOxygen()
    {
        if (!firstPush)
        {
            GManager.instance.PlaySE(startSE);
            GManager.currentElement = GManager.Element.Oxygen;
            GManager.currentItem = GManager.ElementItem.Oxygen;
            GManager.currentItem = GManager.ElementItem.Oxygen;
            fade.StartFadeOut();
            firstPush = true;
        }
    }

    private void Update()
    {
        if (!goNextScene && fade.IsFadeOutComplete())
        {
            SceneManager.LoadScene("stageA-1");
            goNextScene = true;
        }
    }
}
