using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyButton : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Renderer emergencyButtonRenderer;
    void Update()
    {
        if ((player.transform.position - transform.position).magnitude < 5f)
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

    }
}
