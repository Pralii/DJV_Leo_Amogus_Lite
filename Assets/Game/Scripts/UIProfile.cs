using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIProfile : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshPro profileName;
    private PeopleAI me;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!me.isAlive) return;
        Game.getsVoted(me);
    }


    public void SetProfile(PeopleAI people)
    {
        me = people;
        profileImage.color = me.getColor();
        if (me.isAlive)
        {
            profileName.text = me.getName();
        }
        else
        {
            profileName.text = "DED";
        }
    }
}
