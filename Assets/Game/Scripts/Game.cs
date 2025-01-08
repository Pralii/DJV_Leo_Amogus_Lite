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
    [SerializeField] private GameObject instanciableNameTag;
    private List<GameObject> _peopleList;
    [SerializeField] private int totalPeopleCount;
    [SerializeField] private int impostorsCount;
    private static Vector3 _spawnZone = new Vector3(0, 0, 0);
    private static Vector3 _deadCorner = new Vector3(-5, 0, -5);
    public static bool inVote = false;
     
    private static Game _instance;

    public void Awake()
    {
        if (_instance == null) _instance = this;
        
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
            Vector2 displacement = 4f * Random.insideUnitCircle;
            var newPerson = Instantiate(instanciablePerson, _spawnZone + new Vector3(displacement.x, 0, displacement.y), Quaternion.identity);
            PeopleAI newPersonAI = newPerson.GetComponent<PeopleAI>();
            newPersonAI.isImpostor = false;
            interestList[Random.Range(0, interestList.Count)].GetThisPersonInterested(newPersonAI);
            _peopleList.Add(newPerson);
            newPerson.SetActive(true);
        }
    }

    public static void StartVote()
    {
        if (inVote) return;
        inVote = true;
        List<PeopleAI> peopleAIList = new List<PeopleAI>();
        
        foreach (var people in _instance._peopleList)
        {
            var peopleAI = people.GetComponent<PeopleAI>();
            if (peopleAI.isAlive)
            {
                people.transform.position = _spawnZone;
                peopleAI.DefineNewInterest(_spawnZone);
            }
            else
            {
                if ((people.transform.position - _deadCorner).magnitude > 1f)
                {
                    Vector2 displacement = Random.insideUnitCircle;
                    people.transform.position = _deadCorner + new Vector3(displacement.x, 0, displacement.y);
                }
            }

            peopleAIList.Add(peopleAI);
        }
        ListManager.SetupList(peopleAIList);
        
    }

    public static void finishVote(PeopleAI susGuy)
    {
        inVote = false;
        if (susGuy!=null) susGuy.Unalive();

        foreach (var people in _instance._peopleList)
        {
            _instance.interestList[Random.Range(0, _instance.interestList.Count)].GetThisPersonInterested(people.GetComponent<PeopleAI>());
        }

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
