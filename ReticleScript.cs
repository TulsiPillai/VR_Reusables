using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ReticleScript : MonoBehaviour
{
    public Camera CameraFacing;
    public Vector3 target, target2, target3, dirOfTravel, originalScale; //enter target vectors here
    private GameObject player, LoadLevel;
    public Canvas VO1, didYouKnow;
    RaycastHit hit;
    float smoothTime, lerpPosition, distance, speed, fillAmt;
	bool hitArrowFlag, hitArrowFlag2, hitArrowFlag3, tRexTap;
    Animator DinoAnimator;
    float runningTime = 0f;
    float lockTime = 2.0f;
    //MoveDino dinoSounds;
    Vector4 color = new Vector4(0.7f, 0f, 0f, 1f);

    //public Sprite DefaultReticle, TransmissionSprite;
    // Use this for initialization

    void Start()
    {
        player = GameObject.Find("PlayerBase").gameObject;
        smoothTime = 55f;
        speed = 10 * Time.deltaTime;

        reticleSprite = GameObject.Find("ReticleImage").GetComponent<Image>();
        UISounds = GameObject.Find("Pling").GetComponents<AudioSource>();
        Invoke("openDialogue1", 2);
        LoadLevel = GameObject.Find("LoadLevel") as GameObject;
		hitArrowFlag = hitArrowFlag2 = hitArrowFlag3 = false;
        originalScale = transform.localScale;
        fillAmt = 0;
    }

    // Update is called once per frame
    void Update()
    {
		//to move player from current position to target position in a smooth translation
        reticleSprite.fillAmount = fillAmt;
        if (hitArrowFlag) //waypoint 1 
        {
            if (Vector3.Distance(player.transform.position, target) > .5f)
            {
                dirOfTravel = target - player.transform.position; //distance
                dirOfTravel.Normalize();
                player.transform.Translate(dirOfTravel.x * speed,
                        dirOfTravel.y * speed, dirOfTravel.z * speed);
            }
            else
            {
            }

        }
        if (hitArrowFlag2) //waypoint 2 
        {
            if (Vector3.Distance(player.transform.position, target2) > .5f)
            {
                hitArrowFlag = false;
                dirOfTravel = target2 - player.transform.position;
                dirOfTravel.Normalize();
                player.transform.Translate(dirOfTravel.x * speed,
                        dirOfTravel.y * speed, dirOfTravel.z * speed);
            }
        }
        if (hitArrowFlag3) //waypoint 3
        {
            if (Vector3.Distance(player.transform.position, target3) > .5f)
            {
                hitArrowFlag2 = false;
                dirOfTravel = target3 - player.transform.position;
                dirOfTravel.Normalize();
                player.transform.Translate(dirOfTravel.x * speed,
                        dirOfTravel.y * speed, dirOfTravel.z * speed);
            }
        }
        AnimateReticle();
        lerpPosition += Time.deltaTime / smoothTime;
        //
        //Raycasting starts here
        //
        if (Physics.Raycast(new Ray(CameraFacing.transform.position,
                                     CameraFacing.transform.rotation * Vector3.forward),
                             out hit))
        {
            distance = hit.distance;
            checkForCollision();

        }
        else
        {
            fillAmt = 1.0f;
            distance = CameraFacing.farClipPlane * 0.95f; //if it hits nothing
            reticleSprite.color = new Vector4(1, 1, 1, 1);
        }

        transform.position = CameraFacing.transform.position + CameraFacing.transform.rotation * Vector3.forward * distance;
        transform.LookAt(CameraFacing.transform.position);
        transform.Rotate(0.0f, 180.0f, 0.0f);
        if (distance < 10.0f)
        {
            distance *= 1 + 5 * Mathf.Exp(-distance);
        }
        transform.localScale = originalScale * distance * 0.05f; //change scale according to distance of the object hit                
    }
    //functions for triggering audio clips, navigation sprite and changing reticle sprite
    public void checkForCollision()
    {

        
        if (hit.collider.tag == "Respawn") //setflag to true when user hits waypoint1
        {
            timeGaze();
            if (Input.GetButton("Tap"))
            {
                hitArrowFlag = true;

            }
        }
        else if (hit.collider.tag == "Respawn2")
        {
            timeGaze();
            if (Input.GetButton("Tap"))
            {
                runningTime = 0f;
                hitArrowFlag2 = true;
            }
        }
        else if (hit.collider.tag == "Respawn3")
        {
            timeGaze();
            if (Input.GetButton("Tap"))
            {
                runningTime = 0f;
            }
        }
        else
        {
            runningTime = 0f;
            fillAmt = 1.0f;
        
        }
    }


    public void AnimateReticle()
    {
        if (VO1.isActiveAndEnabled)  //during dialogues/UI disable reticle
        {

            reticleSprite.enabled = false;

        }
        else
        {
            reticleSprite.enabled = true;
        }
    }

    
    public void timeGaze()
    {
        
        runningTime += Time.deltaTime * 1f;
        if (fillAmt <= 1)
        {
            fillAmt += Time.deltaTime * 1f;
        }
        else
        {
            fillAmt = 0.0f;
        }
    }
}