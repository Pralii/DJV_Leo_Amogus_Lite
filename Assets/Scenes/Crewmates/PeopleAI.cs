
using System.Collections;
using UnityEngine;

public class PeopleAI : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private float ipProximity;
    private Vector3 _interestPoint;
    private Vector3 _targetPlace;
    private bool _isAlive;
    public bool isImpostor;

    // Start is called before the first frame update
    void Start()
    {
        _isAlive = true;
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
        transform.position += new Vector3(0,-0.8f,0);
    }

    public IEnumerator MoveTowardsInterest()
    {
        if (_isAlive)
        {
            while ((_targetPlace - transform.position).magnitude > 0.2f)
            {
                transform.Translate(speed * Time.deltaTime * (_targetPlace - transform.position).normalized);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    Quaternion.LookRotation(_targetPlace - transform.position),
                    angularSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            DefineNewInterest(_interestPoint);
            StartCoroutine(MoveTowardsInterest());
        }

        yield return null;
    }
    
}
