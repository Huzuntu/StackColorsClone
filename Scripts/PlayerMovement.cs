using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //public static PlayerMovement _playerMovement;

    

/*
    float forwardForce;
    float forceAdder = 5f;
    float forceReducer;
    public static Action<float> Kick;
*/
    Animator anim;
    ParticleSystem[] ParticleSystems;

    [SerializeField] bool isPlaying;
    Rigidbody myRb; 
    float speedOfPlayer = 10f;
    [SerializeField] float sidewaysSpeed = 2f;
    [SerializeField] GameObject stairs;
    [SerializeField] GameObject winningArea;
    [SerializeField] Transform stackObject;
    [SerializeField] Transform endStairs;
    GameObject winningAreaa;
    TrailController trailController;
    float timeCountForForce;
    [SerializeField] float secondsToApplyForce;
    bool addingForce = false;

    float winningAreaPosition;
    public static bool isOnEndLine = false;
        
    public static bool isCheer = false;

    [SerializeField] Ease _moveEase = Ease.Linear;

    SceneManagementy _sceneManager;
    

    void Start()
    {
        trailController = GameObject.Find("Trails").GetComponent<TrailController>();
        anim = GetComponent<Animator>();
        _sceneManager = GameObject.FindObjectOfType<SceneManagementy>();
        myRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(isPlaying)
        {
            MoveForward();
        }
        if(Input.GetKey(KeyCode.R))
        {
            _sceneManager.RestartScene();
        }
        if(!isCheer)
        {
            if(Input.GetMouseButton(0))
            {   
                MoveSideways();
            }
            SidewaysMovementWithInput();
        }
    }
    
    private void FixedUpdate()
    {
        CheckIfEndLine();
        if (addingForce == true)
        {
            timeCountForForce += Time.deltaTime;
            AddTrailAndForce();
        }
    }

    private void AddTrailAndForce()
    {
        if (timeCountForForce < secondsToApplyForce)
        {
            myRb.AddForce(0, 0, 400f);
            trailController.SetTrailTime(secondsToApplyForce);
        }
        else
        {
            timeCountForForce = 0;
            addingForce = false;
        }
    }

    private void CheckIfEndLine()
    {
        if (isOnEndLine == true)
        {
            if (this.transform.position.z >= winningAreaPosition)
            {
                trailController.EnableDisableTrails(false);
                Cheer();
            }
        }
    }

    public void Cheer()
    {
        isCheer = true;
        myRb.isKinematic = true;
        if(this != null)
        {
            transform.DORotate(new Vector3(0, 180, 0), 3f).SetEase(Ease.InOutBounce);
        }
        anim.SetTrigger("Cheer");
        StartTheFirework();
        _sceneManager.InvokeNextLevel(5f);
    }

    public void StartTheFirework()
    {
        ParticleSystems = winningAreaa.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem child in ParticleSystems)
        {
            child.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckObstacle(other);
        CheckFinishLine(other);
        CheckWall(other);
    }

    private void CheckWall(Collider other)
    {
        if (other.tag == "ColorWall")
        {
            trailController.EnableDisableTrails(true);
            addingForce = true;
        }
    }

    private void CheckFinishLine(Collider other)
    {
        if (other.tag == "FinishLineEnd")
        {
            DestroyCarrierCube();
            Invoke("CloseTrigger", 0.25f);
            int i;
            for (i = 0; i < stackObject.childCount; i++)
            {
                GameObject stairss = (GameObject)Instantiate(stairs, new Vector3(0, endStairs.position.y - 0.3f, endStairs.position.z + i), Quaternion.identity);
                stairss.transform.parent = endStairs;
                stairss.GetComponent<Renderer>().material.color = stackObject.GetChild(i).gameObject.GetComponent<Renderer>().material.color;
                Destroy(stackObject.GetChild(i).gameObject);
            }
            if (i != 0)
            {
                winningAreaa = (GameObject)Instantiate(winningArea, new Vector3(0, endStairs.position.y - 0.3f, endStairs.position.z + i + 4.5f), Quaternion.identity);
                winningAreaa.transform.parent = endStairs;
                winningAreaPosition = winningAreaa.transform.localPosition.z + endStairs.position.z;
                isOnEndLine = true;
            }
        }
    }

    private void CheckObstacle(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            DestroyEverything(0.5f);
            CameraShaker.Instance.ShakeOnce(4f, 2f, 0.1f, 1f);
        }
    }

    public void DestroyEverything(float invokeSecond)
    {
        for(int i = 0; i < stackObject.childCount; i++)
        {
            Destroy(stackObject.GetChild(i).gameObject);
        }
        DestroyCarrierCube();
        myRb.isKinematic = true;
        _sceneManager.InvokeRestartScene(invokeSecond);
    }
    

    private void DestroyCarrierCube()
    {
        Destroy(GameObject.FindGameObjectWithTag("PlayerCube"));
    }
    
    private void CloseTrigger()
    {
        this.GetComponent<BoxCollider>().isTrigger = false;
    }
    private void MoveSideways()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 5000))
        {   
            if(hit.transform.tag == "ColorWall")
            {
                return;
            }
            SidewaysMovement(hit);
            if(hit.point.x > 0)
            {  
                SwingObjectsSideMovement(hit, -1);
            }
            else
            {
                SwingObjectsSideMovement(hit, +1);
            }
        }
    }

    void SwingObjectsSideMovement(RaycastHit hit, int op)
    {
        int stackCount = stackObject.childCount; // Holds the number of childs
        float slipCoefficient = 0.025f; 
        int i = 0; //This value increases as the more cubes stacked
        foreach (Transform childStack in stackObject)
            {
                if(childStack != null && (childStack.parent == stackObject) )
                {
                    i++;
                    childStack.transform.DOMoveX(GameObject.FindGameObjectWithTag("PlayerCube").transform.position.x + (op * (slipCoefficient * i)), 0.001f).SetEase(_moveEase).OnComplete(()=>GoBack(childStack));    
                }
            }
    }

    void SidewaysMovement(RaycastHit hit)
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(hit.point.x, transform.position.y, transform.position.z), sidewaysSpeed * Time.deltaTime);
    }

    void SidewaysMovementWithInput()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float sidewaysSpeedWithInput = 9f;
        Vector3 MoveDirection = new Vector3(xDirection, 0.0f, 0.0f);  
        if(Input.GetKey(KeyCode.D))
            transform.position += MoveDirection * sidewaysSpeedWithInput * Time.deltaTime;
        if(Input.GetKey(KeyCode.A))
        {
            transform.position += MoveDirection * sidewaysSpeedWithInput * Time.deltaTime;
        }
    }

    void GoBack(Transform childObject)
    {
        if(childObject != null && (childObject.parent == stackObject))
        {
            childObject.transform.DOMoveX(GameObject.FindGameObjectWithTag("PlayerCube").transform.position.x, 0.02f).SetEase(Ease.OutCubic);
        }
    }

    void MoveForward()
    {
        myRb.velocity = Vector3.forward * speedOfPlayer;
    }
    
}



