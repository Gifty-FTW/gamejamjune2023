using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterSwitcher : MonoBehaviour
{
    [SerializeField] Viktor viktorScript;
    [SerializeField] Aleksander aleksanderScript;
    [SerializeField] GameObject phaseWalls;

    public bool isViktor;

    void Start()
    {
        isViktor = true;
        aleksanderScript.Disable();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (isViktor)
            {
                viktorScript.Disable();
                aleksanderScript.Enable();
                //phaseWalls.SetActive(false);
            }
            else
            {
                viktorScript.Enable();
                aleksanderScript.Disable();
                //phaseWalls.SetActive(true);
            }
            isViktor = !isViktor;
        }
    }
}
