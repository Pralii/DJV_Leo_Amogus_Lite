using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmergencyButton : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Renderer emergencyButtonRenderer;
    [SerializeField] private float emergencyCooldown;
    private float _emergencyTimer;
    [SerializeField] private TMP_Text timerText;
    private static EmergencyButton _instance;
    
    public static void ResetTimer()
    {
        _instance._emergencyTimer = _instance.emergencyCooldown; 
    }

    void Awake()
    {
        if (_instance == null) _instance = this;
    }

    void Start()
    {
        _emergencyTimer = emergencyCooldown*2;
    }

    void Update()
    {
        if (_emergencyTimer > 0) timerText.text = _emergencyTimer.ToString("00");
        else timerText.text = "";
        if ((player.transform.position - transform.position).magnitude < 5f && _emergencyTimer < 0)
        { 
            emergencyButtonRenderer.material.color = Color.red;

            if (Input.GetButtonDown("Space") )
            {
                Game.StartVote();
            }
        }
        else
        {
            emergencyButtonRenderer.material.color = Color.grey;
        }
        
        _emergencyTimer -= Time.deltaTime;
    }
}
