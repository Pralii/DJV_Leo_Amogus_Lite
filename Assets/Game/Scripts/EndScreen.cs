using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _tmpText;
    public static string text;

    public void Start()
    {
        if (text == null || text == "") text = "How did you get here?";
        _tmpText.text = text;
    }


}
