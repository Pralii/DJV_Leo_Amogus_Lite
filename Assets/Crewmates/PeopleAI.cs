
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PeopleAI : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private float ipProximity;
    [SerializeField] private TMP_Text nameTag;
    private Vector3 _interestPoint;
    private Vector3 _targetPlace;
    public bool isAlive;
    public bool isImpostor;
    private string name;
    private string[] _nameList = {"Alice","Bob","Charlie","David","Eve","Frank","Gabriel","Holmes","Ingrid","Joejoe","Karen","Ligma","Monica","Niko","Oscar","Pierre-Emmanuel","Quentin","Rick","Simon","Tom","Ursula","Victoriano","Williams","Yakari","Zoe"} ;
    

    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        GetComponentInChildren<Renderer>().material.color = Color.HSVToRGB(Random.value, 1-Random.value*0.5f, 1-Random.value*0.5f);
        name = _nameList[Random.Range(0, _nameList.Length)];
        nameTag.text = name;
        StartCoroutine(MoveTowardsInterest());
    }

    public Color getColor()
    {
        return GetComponentInChildren<Renderer>().material.color;
    }

    public string getName()
    {
        return name;
    }

    public void DefineNewInterest(Vector3 interestPoint)
    { 
        _interestPoint = interestPoint;
        Vector2 displacement = ipProximity* Random.insideUnitCircle;
        _targetPlace = interestPoint + new Vector3(displacement.x, 0, displacement.y);
    }

    public void Unalive()
    {
        isAlive = false;
        transform.position = new Vector3(transform.position.x , 0.25f, transform.position.z);
        transform.Rotate(Vector3.forward, 90);
        Game.CheckState();

    }

    public IEnumerator MoveTowardsInterest()
    {
        if (isAlive)
        {
            while (isAlive && (_targetPlace - transform.position).magnitude > 0.2f)
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
