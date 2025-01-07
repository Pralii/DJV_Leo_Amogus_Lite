
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleAI : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private float ipProximity;
    [SerializeField] private GameObject originalCorpse;
    private Vector3 _interestPoint;
    private Vector3 _targetPlace;
    private bool _isAlive;
    public bool isImpostor;

    // Start is called before the first frame update
    void Start()
    {
        _isAlive = true;
        GetComponentInChildren<Renderer>().material.color = Color.HSVToRGB(Random.value, 1-Random.value*0.5f, 1-Random.value*0.5f);
        StartCoroutine(MoveTowardsInterest());
    }

    public void DefineNewInterest(Vector3 interestPoint)
    {
        _interestPoint = interestPoint;
    Vector2 displacement = Random.value*ipProximity* Random.insideUnitCircle;
    _targetPlace = interestPoint + new Vector3(displacement.x, 0, displacement.y);
    }

    public void Unalive()
    {
        _isAlive = false;
        //Change model
        transform.position = new Vector3(transform.position.x , 0.25f, transform.position.z);
        transform.Rotate(Vector3.forward, 90);;
    }

    public IEnumerator MoveTowardsInterest()
    {
        if (_isAlive)
        {
            while (_isAlive && (_targetPlace - transform.position).magnitude > 0.2f)
            {
                transform.LookAt(_targetPlace);
                transform.position += (speed * Time.deltaTime * (_targetPlace - transform.position).normalized);
                
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            DefineNewInterest(_interestPoint);
            StartCoroutine(MoveTowardsInterest());
        }

        yield return null;
    }
    
}
