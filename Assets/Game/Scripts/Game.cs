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
    [SerializeField] private int totalPeopleCount;
    [SerializeField] private int impostorsCount;
    private Vector3 _spawnZone = new Vector3(0, 0, 0);

    public static Game Instance;
    private void ShowMenuUI()
    {
    }

    public void Awake()
    {
        if (Instance == null) Instance = this;
    }

    //Spawn everyone, assign them an IP.
    private void StartGame()
    {
        //add impostors
        for (int i = 0; i < impostorsCount; i++)
        {
            Vector2 displacement = 4f * Random.insideUnitCircle;
            var newPerson = Instantiate(instanciablePerson, _spawnZone + new Vector3(displacement.x, 0, displacement.y), Quaternion.identity);
            PeopleAI newPersonAI = newPerson.GetComponent<PeopleAI>();
            newPersonAI.isImpostor = true;
            interestList[Random.Range(0, interestList.Count)].GetThisPersonInterested(newPersonAI);
            _peopleList.Add(newPerson);
            newPerson.SetActive(true);
        }
        //add normal people
        for (int i = impostorsCount; i < totalPeopleCount; i++)
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
    
    

    public static void getsVoted(PeopleAI susGuy)
    {
        susGuy.Unalive();
        ListManager.ClearScreen();
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
