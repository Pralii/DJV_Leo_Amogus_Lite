using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Game : MonoBehaviour
{
    
    //Game will manage interest points, kills, vote ?
    [SerializeField] private List<InterestPoint> interestList;
    [SerializeField] private PlayerCharacter player;
    [SerializeField] private GameObject instanciablePerson;
    private List<GameObject> _peopleList;
    [SerializeField] private int _peopleCount;
    [SerializeField] private int _impostorsCount;
    private Vector3 _spawnZone = new Vector3(0, 0, 0);
    
    private void ShowMenuUI()
    {
    }

    //Spawn everyone, assign them an IP.
    private void StartGame()
    {
        //add impostors
        for (int i = 0; i < _impostorsCount; i++)
        {
            Vector2 displacement = 5f * Random.insideUnitCircle;
            var newPerson = Instantiate(instanciablePerson, _spawnZone + new Vector3(displacement.x, 0, displacement.y), Quaternion.identity);
            PeopleAI newPersonAI = newPerson.GetComponent<PeopleAI>();
            newPersonAI.isImpostor = true;
            interestList[Random.Range(0, interestList.Count)].GetThisPersonInterested(newPersonAI);
            _peopleList.Add(newPerson);
            newPerson.SetActive(true);
        }
        //add normal people
        for (int i = _impostorsCount; i < _peopleCount; i++)
        {
            Vector2 displacement = 5f * Random.insideUnitCircle;
            var newPerson = Instantiate(instanciablePerson, _spawnZone + new Vector3(displacement.x, 0, displacement.y), Quaternion.identity);
            PeopleAI newPersonAI = newPerson.GetComponent<PeopleAI>();
            newPersonAI.isImpostor = false;
            interestList[Random.Range(0, interestList.Count)].GetThisPersonInterested(newPersonAI);
            _peopleList.Add(newPerson);
            newPerson.SetActive(true);
        }
    }
    
    void Start()
    {
        _peopleList = new List<GameObject>();
        StartGame();
    }

    
    //Check for vote, menu, and win/lose condition.
    void Update()
    {
        
    }
}
