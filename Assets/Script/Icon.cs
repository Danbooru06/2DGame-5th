using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    public Sprite HydrogenIcon;
    public Sprite HeliumIcon;
    public Sprite DiamondIcon;
    public Sprite GraphateIcon;
    public Sprite OxygenIcon;

    private SpriteRenderer currentIcon;
    private GManager.Element oldElement;

    void Start()
    {
        currentIcon = GetComponent<SpriteRenderer>();
        SetIcon();
    }

    void Update()
    {
        if(oldElement != GManager.currentElement)
        {
            SetIcon();
        }
    }

    void SetIcon()
    {
        switch (GManager.currentElement)
        {
            case GManager.Element.Hydrogen:
                currentIcon.sprite = HydrogenIcon;
                oldElement = GManager.Element.Hydrogen;
                break;

            case GManager.Element.Helium:
                currentIcon.sprite = HeliumIcon;
                oldElement = GManager.Element.Helium;
                break;

            case GManager.Element.Carbon_Diamond:
                currentIcon.sprite = DiamondIcon;
                oldElement = GManager.Element.Carbon_Diamond;
                break;

            case GManager.Element.Carbon_Graphite:
                currentIcon.sprite = GraphateIcon;
                oldElement = GManager.Element.Helium;
                break;

            case GManager.Element.Oxygen:
                currentIcon.sprite = OxygenIcon;
                oldElement = GManager.Element.Oxygen;
                break;
        }
    }
}
