using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Central : MonoBehaviour
{
    [SerializeField] Transform _emptyCard;
    List<Arranger> _arrangers;

    private void Start()
    {
        _arrangers = new List<Arranger>();
        var arrs = transform.GetComponentsInChildren<Arranger>();

        for(int i=0;i<arrs.Length;i++)
        {
            _arrangers.Add(arrs[i]);
        }
    }

    public static void SwapCards(Transform sour, Transform dest)
    {
        Transform sourParent = sour.parent;
        Transform destParent = dest.parent;

        int sourIndex = sour.GetSiblingIndex();
        int destIndex = dest.GetSiblingIndex();

        sour.SetParent(destParent);
        sour.SetSiblingIndex(destIndex);

        dest.SetParent(sourParent);
        dest.SetSiblingIndex(sourIndex);
    }

    void SwapCardsHierachy(Transform sour, Transform dest)
    {
        SwapCards(sour, dest);
        _arrangers.ForEach(t => t.UpdateChildren());
    }

    bool ContainPos(RectTransform rt,Vector2 pos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rt, pos);
    }

    void BeginDrag(Transform card)
    {
        SwapCardsHierachy(_emptyCard, card);
    }
    void Drag(Transform card)
    {
        var whichArrangerCard = _arrangers.Find(t => ContainPos(t.transform as RectTransform, card.position));

        if (whichArrangerCard == null)
        {

        }
        else
        {
            int invisibleCardIndex = _emptyCard.GetSiblingIndex();
            int targetIndex = whichArrangerCard.GetIndexByPosition(card, invisibleCardIndex);
            if (invisibleCardIndex != targetIndex)
            {
                whichArrangerCard.SwapCard(invisibleCardIndex, targetIndex);
            }
        }
    }
    void EndDrag(Transform card)
    {
        SwapCardsHierachy(_emptyCard, card);
    }
}
