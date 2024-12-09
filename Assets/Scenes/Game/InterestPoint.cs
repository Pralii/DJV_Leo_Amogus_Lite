using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InterestPoint : MonoBehaviour
{
    //Interest point does the kills and redirections: It is aware of the people that must gather around it, who is close or not. When everyone is gathered, it waits a lil' and then tries to get a murder going. Successful or not it redirects everyone towards another interest point.
    private Collider[] _colliderList = new Collider[128];
    [SerializeField] private List<InterestPoint> otherInterestPoints;
    [SerializeField] private float ipRadius;
    private List<PeopleAI> _interestedPeople;
    private float _killCooldown = 5f;
    private float _killCharge = 5f;
    private Transform _transform;

    void Awake()
    {
        _transform = GetComponent<Transform>();
        _interestedPeople = new List<PeopleAI>();
    }
    private bool isVisible()
    {
        //This is Code with missing elements ? (Camera?)
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(transform.position) <= -ipRadius)
            {
                return false;
            }
        }

        return true;
    }
    
    //check the number of impostors/innocent in the people present, and if the player is not here, UNALIVE an innocent
    private IEnumerator TryKilling()
    {

        foreach (var personAI in _interestedPeople)
        {
            Debug.Log("tried changing interest.");
            otherInterestPoints[Random.Range(0, otherInterestPoints.Count)].GetThisPersonInterested(personAI);
                
        }
        _interestedPeople.Clear();
        yield return null;
    }

    public void GetThisPersonInterested(PeopleAI personAI)
    {
        _interestedPeople.Add(personAI);
        personAI.DefineNewInterest(_transform.position);
    }

    //Check if everyonen is here, then try killing, with a cooldown. Then redirect everyone to other places.
    void Update()
    {
        if (_interestedPeople.Count != 0)
        {
            int colliderNbr = Physics.OverlapSphereNonAlloc(transform.position, ipRadius, _colliderList);
            int closePeopleNbr = 0;
            for (int i = 0; i < colliderNbr; i++)
            {

                try
                {
                    PeopleAI personAI = _colliderList[i].GetComponentInParent<PeopleAI>();
                    if (_interestedPeople.Contains(personAI))
                    {
                        closePeopleNbr++;
                    }
                }
                catch (System.NullReferenceException)
                {


                }
            }

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
