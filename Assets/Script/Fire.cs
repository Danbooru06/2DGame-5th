using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{

    [Header("爆発半径")]public float radius;
    [Header("爆発の強さ")]public float force;

    private bool firstGrown = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Oxygen" || collision.tag == "Graphite")
        {
            Burn();
        }
        else if(collision.tag == "Player")
        {
            if(GManager.currentElement == GManager.Element.Oxygen || GManager.currentElement == GManager.Element.Carbon_Graphite)
            {
                Burn();
            }
        }

        if(collision.tag == "Hydrogen")
        {
            Explode();
        }

        if(collision.tag == "Water")
        {
            Destroy(this.gameObject);
        }
    }

    void Burn()
    {
        if (!firstGrown)
        {
            transform.localScale *= 2.0f;
        }
        firstGrown = true;
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach(Collider2D hit in hits)
        {
            if (!hit.CompareTag("Player") && !hit.CompareTag("Enemy")) continue;

            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb == null) continue;
            
            Vector2 direction = (hit.transform.position - transform.position).normalized;

            Player player = hit.GetComponent<Player>();
            if(player != null)
            {
                player.Knockback(direction, force);
            }
            else
            {
                rb.AddForce(direction * force, ForceMode2D.Impulse);
            }
        }
    }

}
