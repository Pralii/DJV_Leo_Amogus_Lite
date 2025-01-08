using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InterestPoint : MonoBehaviour
{
    //Interest point does the kills and redirections: It is aware of the people that must gather around it, who is close or not. When everyone is gathered, it waits a little and then tries to get a murder going. Successful or not it redirects everyone towards another interest point.
    [SerializeField] private List<InterestPoint> otherInterestPoints;
    [SerializeField] private float ipRadius;
    private List<PeopleAI> _interestedPeople;
    private List<Transform> _interestedPeopleTransforms;
    private float _killCooldown = 3f;
    private float _killCharge = 3f;
    void Awake()
    {
        _interestedPeople = new List<PeopleAI>();
        _interestedPeopleTransforms = new List<Transform>();
    }
    
    //Function to check if the interest point is visible from the camera. (Used to know if a kill is pssible)
    private bool isVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(transform.position) <= -ipRadius/2)
            {
                return false;
            }
        }
        return true;
    }
    
    //check the number of impostors/innocent in the people present, and if the player is not here, UNALIVE an innocent.
    private IEnumerator TryKilling()
    {

        var innocentPeople = new List<PeopleAI>();
        int impostorCount = 0;
        int innocentCount = 0;
        foreach (var personAI in _interestedPeople)
        {
            if (personAI.isImpostor)
            {
                impostorCount++;
            }
            else
            {
                innocentPeople.Add(personAI);
                innocentCount++;
            }
        }

        if (!isVisible() && innocentCount > 0 && impostorCount >= innocentCount)
        {
            var killedGuy = innocentPeople[Random.Range(0, innocentPeople.Count)];
            _interestedPeople.Remove(killedGuy);
            killedGuy.Unalive();
        }

        foreach (var personAI in _interestedPeople)
        {
            Debug.Log("tried changing interest.");
            otherInterestPoints[Random.Range(0, otherInterestPoints.Count)].GetThisPersonInterested(personAI);
        }
        _interestedPeople.Clear();
        _interestedPeopleTransforms.Clear();
        yield return null;
    }

    
    public void GetThisPersonInterested(PeopleAI personAI)
    {
        _interestedPeople.Add(personAI);
        _interestedPeopleTransforms.Add(personAI.GetComponent<Transform>());

        personAI.DefineNewInterest(transform.position);
    }

    //Check if everyonen is here, then try killing, with a cooldown. Then redirect everyone to other places.
    void Update()
    {

        if (_interestedPeople.Count != 0)
        {
            //Remove dead people and/or reset ourselves if a vote is ongoing.
            _interestedPeople.RemoveAll(x => !x.isAlive || Game.inVote);

            //Count close people
            int closePeopleNbr = 0;
            foreach (var t in _interestedPeopleTransforms)
            {

                if ((transform.position - t.position).magnitude < ipRadius)
                {
                    closePeopleNbr++;
                }
            }

            //if everyone is close for enough time, KIILLLL
            if (closePeopleNbr >= _interestedPeople.Count)
            {
                _killCharge -= Time.deltaTime;
                if (_killCharge <= 0f)
                {

                    StartCoroutine(TryKilling());
                    _killCharge = _killCooldown;
                }
            }
            else 
            {
                _killCharge = _killCooldown;
            }

        }
    }
}
