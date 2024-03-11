using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject settingsBackground;
    // Start is called before the first frame update
    void Start()
    {
        settingsBackground.SetActive(false);
    }


    public void OpenSettings()
    {
        settingsBackground.SetActive(true);
    }
}
