using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIProfile : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TMP_Text profileName;
    private PeopleAI me;

    //If a profile is clicked, they are voted off. If already dead, it finishes the vote without killing anyone.
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!me.isAlive) Game.finishVote(null);
        else Game.finishVote(me);
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
            profileName.text = me.getName()+" (dead)";
        }
    }
}
