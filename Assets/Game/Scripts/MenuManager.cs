using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
    private void StartGame(int totalCrew, int impostors)
    {
        Game.totalPeopleCount = totalCrew;
        Game.totalimpostorsCount = impostors;
        SceneManager.LoadScene("MainScene");
    }

    public void VeryFew()
    {
        StartGame(3, 1);
    }

    public void LowCount()
    {
        StartGame(5, 2);
    }

    public void MediumCount()
    {
        StartGame(8, 3);
    }

    public void HighCount()
    {
        StartGame(12, 5);
    }

    public void TooMany()
    {
        StartGame(16, 7);
    }
}
