using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ListManager : MonoBehaviour
{
    [SerializeField] private UIProfile _uiProfilePrefab;
    private List<UIProfile> _uiProfiles = new List<UIProfile>();
    private static ListManager _instance;

    public void Awake()
    {
        _instance = this;
    }

    public static void SetupList(List<PeopleAI> crew)
    {
        foreach (var crewmate in crew)
        {
            var crewUiProfile = Instantiate(_instance._uiProfilePrefab, _instance.transform);
            crewUiProfile.SetProfile(crewmate);
            _instance._uiProfiles.Add(crewUiProfile);
            
            
        }
    }

    public static void ClearScreen()
    {
        foreach (var crewUiProfile in _instance._uiProfiles)
        {
            Destroy(crewUiProfile.gameObject);
        }
        _instance._uiProfiles.Clear();
    }

}
