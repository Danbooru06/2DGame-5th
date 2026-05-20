using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
 {
    [Header("フェード")] public FadeImage fade;
    [Header("ボタンを押したときに鳴らすSE")] public AudioClip pushSE;
    [Header("スタート時に鳴らすSE")] public AudioClip startSE;

    private bool firstPush = false; //一度だけ押せるように
    private bool goNextScene = false;

    public void SelectA() //スタートボタンを押されたら呼ばれる
     {
          if (!firstPush)
          {
            GManager.instance.stageType = GManager.StageType.A;
            firstPush = true;
            GManager.instance.PlaySE(pushSE);
            SceneManager.LoadScene("A-select");
        }
     }

    public void SelectB()
    {
        if (!firstPush)
        {
            GManager.instance.stageType = GManager.StageType.B;
            firstPush = true;
            GManager.instance.PlaySE(pushSE);
            SceneManager.LoadScene("B-select1");
        }
    }

    public void SelectC()
    {
        if (!firstPush)
        {
            GManager.instance.stageType = GManager.StageType.C;
            GManager.instance.PlaySE(startSE);
            GManager.currentElement = GManager.Element.Hydrogen;
            GManager.currentItem = GManager.ElementItem.Hydrogen;
            GManager.baseItem = GManager.ElementItem.Hydrogen;

            fade.StartFadeOut();
            GManager.instance.PlaySE(pushSE);
            firstPush = true;
        }
    }

    private void Update()
    {
        if (!goNextScene && fade.IsFadeOutComplete())
        {
            SceneManager.LoadScene("stageC-1");
            goNextScene = true;
        }
    }
}