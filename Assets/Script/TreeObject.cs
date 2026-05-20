using UnityEngine;

public class TreeObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "fire")
        {
            Destroy(gameObject);
        }
    }
}
