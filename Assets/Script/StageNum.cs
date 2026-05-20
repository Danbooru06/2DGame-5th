using UnityEngine;
using UnityEngine.UI;

public class StageNum : MonoBehaviour
{
    private Text stageText = null;
    private int oldStageNum = 0;
    void Start()
    {
        stageText = GetComponent<Text>();
        if(GManager.instance != null)
        {
            stageText.text = "Stage" + GManager.instance.stageNum;
        }
        else
        {
            Debug.Log("ゲームマネージャーを置き忘れている");
            Destroy(this);
        }
    }

    void Update()
    {
        if(oldStageNum != GManager.instance.stageNum)
        {
            stageText.text = "Stage"+ GManager.instance.stageNum;
            oldStageNum = GManager.instance.stageNum;
        }
    }
}
