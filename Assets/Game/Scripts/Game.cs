using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    
    //Game will manage interest points, kills, vote ?
    [SerializeField] private List<InterestPoint> interestList;
    [SerializeField] private PlayerCharacter player;
    [SerializeField] private GameObject instanciablePerson;
    [SerializeField] private GameObject instanciableNameTag;
    private List<GameObject> _peopleList;
    public static int totalPeopleCount;
    public static int totalimpostorsCount;
    private static Vector3 _spawnZone = new Vector3(0, 0, 0);
    private static Vector3 _deadCorner = new Vector3(-5, 0, -5);
    public static bool inVote;
     
    private static Game _instance;

    public void Awake()
    {
        inVote = false;
        if (_instance == null) _instance = this;
        
        //default parameters 
        if (totalPeopleCount == 0 || totalimpostorsCount == 0)
        {
            totalPeopleCount = 5;
            totalimpostorsCount = 2;
        }
    }

    //Spawn everyone, assign them an Interest Point
    private void StartGame()
    {
        _peopleList.Clear();
        //add impostors
        for (int i = 0; i < totalimpostorsCount; i++)
        {
            Vector2 displacement = 4f * Random.insideUnitCircle;
            var newPerson = Instantiate(instanciablePerson, _spawnZone + new Vector3(displacement.x, 0, displacement.y), Quaternion.identity);
            PeopleAI newPersonAI = newPerson.GetComponent<PeopleAI>();
            newPersonAI.isImpostor = true;
            interestList[Random.Range(0, interestList.Count)].GetThisPersonInterested(newPersonAI); //Assign an IP
            _peopleList.Add(newPerson);
            newPerson.SetActive(true);
        }
        //add normal people
        for (int i = totalimpostorsCount; i < totalPeopleCount; i++)
        {
            Vector2 displacement = 4f * Random.insideUnitCircle;
            var newPerson = Instantiate(instanciablePerson, _spawnZone + new Vector3(displacement.x, 0, displacement.y), Quaternion.identity);
            
            PeopleAI newPersonAI = newPerson.GetComponent<PeopleAI>();
            newPersonAI.isImpostor = false;
            interestList[Random.Range(0, interestList.Count)].GetThisPersonInterested(newPersonAI); //assign an IP
            _peopleList.Add(newPerson);
            newPerson.SetActive(true);
        }
        //shuffle
        for (int i = _peopleList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = _peopleList[i];
            _peopleList[i] = _peopleList[j];
            _peopleList[j] = temp;
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
            
            //if alive, teleport them to spawn (and make them stay with a new interest point here)
            if (peopleAI.isAlive)
            {
                people.transform.position = _spawnZone;
                peopleAI.DefineNewInterest(_spawnZone);
            }
            //if dead, put them in the dead corner (it's funny)
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
        if (susGuy!=null) susGuy.Unalive();//execute in place the foolish crew who defiled our community ! Obviously an impostor (joke).

        //Reassign everyone an interest point.
        foreach (var people in _instance._peopleList)
        {
            _instance.interestList[Random.Range(0, _instance.interestList.Count)].GetThisPersonInterested(people.GetComponent<PeopleAI>());
        }
        ListManager.ClearScreen();
        EmergencyButton.ResetTimer();
        CheckState();

    }

    void Start()
    {
        _peopleList = new List<GameObject>();
        StartGame();
    }

    
    //Check for win/lose condition.
    public static void CheckState()
    {
        int alivePeople = 0;
        int impostors = 0;
        foreach (var person in _instance._peopleList)
        {
            PeopleAI peopleAI= person.GetComponent<PeopleAI>();
            if (peopleAI.isAlive)
            {
                alivePeople++;
                if (peopleAI.isImpostor) impostors++;
            }
        }

        if (impostors == 0) YouWin(alivePeople);
        else if (1+alivePeople-impostors  <= impostors) YouLose();
    }

    private static void YouWin(int savedPpl)
    {
        EndScreen.text = "You Win! All impostors were executed, you saved " + savedPpl + " people.";
        SceneManager.LoadScene("EndScreen");
    }

    private static void YouLose()
    {
        EndScreen.text = "You Lose! Impostors took over the ship..";
        SceneManager.LoadScene("EndScreen");
    }
}
