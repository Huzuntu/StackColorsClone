using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCubes : MonoBehaviour
{
    
    float height = 0f;
    GameObject PlayerControl;
    Color currentColor;
    Rigidbody pickUpRB;
    Collider pickUpCollider;
    [SerializeField] Transform stackObject;
    GameObject playerCube;
    TrailController trailController;
    SceneManagementy _sceneManager;
    PlayerMovement _playerMovement;
    public void Start()
    {
        trailController = GameObject.Find("Trails").GetComponent<TrailController>();
        playerCube = GameObject.FindWithTag("PlayerCube");
        _sceneManager = GameObject.FindObjectOfType<SceneManagementy>();
        PlayerControl = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
    }
    void Update()
    {
        currentColor = this.GetComponent<Renderer>().material.color;
    }
    
    private void OnTriggerExit(Collider other) {
        if(other.tag == "ColorWall")
        {
            Color wallColor = other.GetComponent<WallColor>().GetColor();
            this.GetComponent<Renderer>().material.color = wallColor; 
            trailController.TrailSetColor(wallColor);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        CheckPickUp(other);
    }


    private void CheckPickUp(Collider other)
    {
        if (other.transform.CompareTag("pickUp"))
        {
            Color pickUpColor = other.GetComponent<Renderer>().material.color;
            CheckColor(other, pickUpColor);
        }
    }

    private void CheckColor(Collider other, Color pickUpColor)
    {
        if (currentColor == pickUpColor)
        {
            SetScore(5);
            SetHeight(0.3f);
            SetColliderParent(other);
            SetStackPosition(other, playerCube);
        }
        else
        {
            SetScore(-5);
            Destroy(other.gameObject);
            if (stackObject != null)
            {
                if (stackObject.childCount > 1)
                {
                    Destroy(stackObject.GetChild(stackObject.childCount - 1).gameObject);
                    SetHeight(-0.3f);
                }
                else
                {
                    _playerMovement.DestroyEverything(0.0f);
                }
            }

        }
    }

    private void SetColliderParent(Collider other)
    {
        other.transform.parent = stackObject;
    }

    private static void SetScore(int score)
    {
        ScoreBoard.instance.IncreaseScore(score);
    }

    private void SetStackPosition(Collider other, GameObject playerCube)
    {
        other.transform.position = new Vector3(playerCube.transform.position.x, playerCube.transform.position.y + height, playerCube.transform.position.z);
    }

    private void SetHeight(float height)
    {
        this.height += height;
    }

    /*
    private void OnEnable() {
        PlayerMovement.Kick +=  MyKick;
    }

    private void OnDisable()
    {
        PlayerMovement.Kick -= MyKick;
    }

    private void MyKick(float forceToKick)
    {
        Debug.Log($"forceToKick is {forceToKick}");
        transform.parent = null;
        pickUpCollider.enabled = true;
        pickUpRB.isKinematic = false;
        pickUpRB.AddForce(new Vector3(0, 20, forceToKick), ForceMode.Impulse);
    }   
    */

}
