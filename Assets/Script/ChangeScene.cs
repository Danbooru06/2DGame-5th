using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void changetoGuide()
    {
        SceneManager.LoadScene("guide");
    }

    public void changetoTitle()
    {
        SceneManager.LoadScene("title");
    }
    public void changetoBugList()
    {
        SceneManager.LoadScene("buglist");
    }
}
