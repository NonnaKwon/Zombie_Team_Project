using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Central : MonoBehaviour
{
    [SerializeField] Transform _emptyCard;
    List<Arranger> _arrangers;
    Arranger _workingArranger;
    int _oriIndex;

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
        _workingArranger = _arrangers.Find(t => ContainPos(t.transform as RectTransform, card.position));
        _oriIndex = card.GetSiblingIndex();
        SwapCardsHierachy(_emptyCard, card);
    }

    void Drag(Transform card)
    {
        Arranger whichArrangerCard = _arrangers.Find(t => ContainPos(t.transform as RectTransform, card.position));
        Debug.Log(whichArrangerCard);
        if (whichArrangerCard == null)
        {
            //bool updateChildren = transform != _emptyCard.parent;
            //_emptyCard.SetParent(transform);
            //if(updateChildren)
            //{
            //    _arrangers.ForEach(t => t.UpdateChildren());
            //}
        }
        else
        {
            Arranger cardArranger = card.parent.GetComponent<Arranger>();
            int invisibleCardIndex = _emptyCard.GetSiblingIndex();
            int targetIndex = whichArrangerCard.GetIndexByPosition(card, invisibleCardIndex);
            if (whichArrangerCard == cardArranger)
            {
                if (invisibleCardIndex != targetIndex)
                    whichArrangerCard.SwapCard(invisibleCardIndex, targetIndex);
            }else
            {
                SwapCardsHierachy(_emptyCard, whichArrangerCard.GetCard(targetIndex));
            }
        }
    }
    void EndDrag(Transform card)
    {
        SwapCardsHierachy(_emptyCard, card);
    }
}
