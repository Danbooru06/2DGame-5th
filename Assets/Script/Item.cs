using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Sprite HydrogenIcon;
    public Sprite HeliumIcon;
    public Sprite DiamondIcon;
    public Sprite GraphateIcon;
    public Sprite OxygenIcon;
    public Sprite WaterIcon;

    //private SpriteRenderer currentIcon;
    private Image currentIcon;
    private GManager.ElementItem oldItem;

    void Start()
    {
        //currentIcon = GetComponent<SpriteRenderer>();
        currentIcon = GetComponent<Image>();
        SetIcon();
    }

    void Update()
    {
        if (oldItem != GManager.currentItem)
        {
            SetIcon();
        }
    }

    void SetIcon()
    {
        switch (GManager.currentItem)
        {
            case GManager.ElementItem.Hydrogen:
                currentIcon.sprite = HydrogenIcon;
                oldItem = GManager.ElementItem.Hydrogen;
                break;

            case GManager.ElementItem.Helium:
                currentIcon.sprite = HeliumIcon;
                oldItem = GManager.ElementItem.Helium;
                break;

            case GManager.ElementItem.Diamond:
                currentIcon.sprite = DiamondIcon;
                oldItem= GManager.ElementItem.Diamond;
                break;

            case GManager.ElementItem.Graphite:
                currentIcon.sprite = GraphateIcon;
                oldItem = GManager.ElementItem.Graphite;
                break;

            case GManager.ElementItem.Oxygen:
                currentIcon.sprite = OxygenIcon;
                oldItem = GManager.ElementItem.Oxygen;
                break;

            case GManager.ElementItem.Water:
                currentIcon.sprite = WaterIcon;
                oldItem = GManager.ElementItem.Water;
                break;
        }
    }
}
