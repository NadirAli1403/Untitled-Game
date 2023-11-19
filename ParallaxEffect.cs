using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{

    [SerializeField] private float parallaxEffectMultiplier;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }


    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition; //creates a vector that represents the small change in camera position
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, 0);
        lastCameraPosition = cameraTransform.position;
    }
}
