using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject viktor;
    [SerializeField] GameObject aleksander;
    [SerializeField] characterSwitcher characterSwitcher;
    
    Camera mainCamera;

    GameObject activeCharacter;
    bool isViktor;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (characterSwitcher.isViktor != this.isViktor)
        {
            this.isViktor = characterSwitcher.isViktor;
            SwitchCamera();
        }
        Follow();
    }

    void SwitchCamera()
    {
        if (this.isViktor)
        {
            activeCharacter = viktor;
        }
        else
        {
            activeCharacter = aleksander;
        }
    }

    void Follow()
    {
        mainCamera.transform.position = new Vector3(activeCharacter.transform.position.x, activeCharacter.transform.position.y, -10.0f);
    }
}
