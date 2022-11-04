using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector3 distance = new Vector3(3, 5, -5);
    [SerializeField] Vector3 rotationDistance = new Vector3(0, -180, 0);
    
    PlayerMovement instance;

    private void Awake() {
        instance = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    private void ZoomCameraIn()
    {
        distance = new Vector3(0, 4, -8.25f);
        NormalCamera();    
    }
    void LateUpdate()
    {
        if(PlayerMovement.isCheer)
        {
            ZoomCameraIn();
        }
        
        else
        {
            distance = new Vector3(3, 5, -5);
            NormalCamera();
        }
    }

    private void NormalCamera()
    {
        if (target != null)
        {
            gameObject.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + distance, Time.deltaTime);
        }
    }
}
