using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ReticleScript : MonoBehaviour
{
    public Camera CameraFacing;
    private Vector3 target, target2, target3, dirOfTravel, originalScale;
    private GameObject player, LoadLevel;
    public GameObject tRex, arrow1, arrow2, arrow3;
    public Canvas VO1, didYouKnow;
    RaycastHit hit;
    AudioSource DidYouKnow, Sniff;
    float smoothTime, lerpPosition, distance, speed, fillAmt;
    public AudioSource[] UISounds;
    SpriteRenderer arrowSprite, arrowSprite2, arrowSprite3;
    public Sprite speechBubble1, speechBubble2, speechBubble3, speechBubble4,
        speechBubble5, speechBubble6, speechBubble7, speechBubble8, defaultGaze;
    Image speechBubble, transmission, reticleSprite;
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
        DidYouKnow = GameObject.Find("DidYou").GetComponent<AudioSource>();
        smoothTime = 55f;
        speed = 10 * Time.deltaTime;
        target = new Vector3(200, 1, 36);
        target2 = new Vector3(200, 1, 115);
        target3 = new Vector3(90, 1, 115);
        reticleSprite = GameObject.Find("ReticleImage").GetComponent<Image>();
        UISounds = GameObject.Find("Pling").GetComponents<AudioSource>();
        Invoke("openDialogue1", 2);
        LoadLevel = GameObject.Find("LoadLevel") as GameObject;
        arrowSprite  = arrow1.GetComponent<SpriteRenderer>();
        arrowSprite2 = arrow2.GetComponent<SpriteRenderer>();
        arrowSprite3 = arrow3.GetComponent<SpriteRenderer>();
        speechBubble = VO1.GetComponentInChildren<Image>();       
        transmission = GameObject.Find("Communicator").GetComponent<Image>();
        DinoAnimator = tRex.GetComponent<Animator>();
        hitArrowFlag = hitArrowFlag2 = hitArrowFlag3 = false;
        originalScale = transform.localScale;
        fillAmt = 0;
        tRexTap = false;
        //dinoSounds = tRex.GetComponent<MoveDino>();
        //tRex = GameObject.Find("t_Rex").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        reticleSprite.fillAmount = fillAmt;
        if (hitArrowFlag)
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
                tRex.SetActive(true);
            }

        }
        if (hitArrowFlag2)
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
        if (hitArrowFlag3)
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

        if (hit.collider.tag == "trex")//tap on t-rex 
        {
            reticleSprite.color = color;
            if (Input.GetButton("Tap") && !tRexTap)
            {
                didYouKnow.enabled = true;                
                DidYouKnow.Play();
                tRexTap = true;
                if (Input.GetKey(KeyCode.Space))
                {
                    closeAll();
                }        
            }else if (Input.GetButton("Tap") && didYouKnow.enabled == false)
            {
                DinoAnimator.SetTrigger("Sniff");   
            }
        }/*else if (hit.collider.tag == "Roar") //roar when you tap the dino's head
        {
            reticleSprite.color = color;
            if (Input.GetButton("Tap"))
            {
                DinoAnimator.SetTrigger("Roar");              
              
            }

        }*/
        else if (hit.collider.tag == "Respawn" && arrowSprite.enabled == true)
        {
            //timeGaze();
            if (Input.GetButton("Tap"))
            {
                hitArrowFlag = true;
                arrow1.SetActive(false);
                Invoke("openDialogue7", 10);
                
            }
        }
        else if (hit.collider.tag == "Respawn2" && arrowSprite2.enabled == true && VO1.enabled == false)
        {
            //timeGaze();
            if (Input.GetButton("Tap"))
            {
                runningTime = 0f;
                hitArrowFlag2 = true;
                arrow2.SetActive(false); 
                arrowSprite3.enabled = true;
            }
        }
        else if (hit.collider.tag == "Respawn3" && arrowSprite3.enabled == true && VO1.enabled == false)
        {
            //timeGaze();
            if (Input.GetButton("Tap"))
            {
                runningTime = 0f;
                hitArrowFlag3 = true;
                arrow3.SetActive(false);
            }
        }
        else
        {
            runningTime = 0f;
            fillAmt = 1.0f;
            DinoAnimator.ResetTrigger("Sniff");
        
        }
    }


    public void AnimateReticle()
    {
        if (VO1.isActiveAndEnabled)
        {

            reticleSprite.enabled = false;

        }
        else
        {
            reticleSprite.enabled = true;
        }
    }

    //Functions for sequencin voice overs
    public void openDialogue1()
    {
        UISounds[2].Play();
        VO1.enabled = true;
        speechBubble.sprite = speechBubble1;
        Invoke("closeAll", 4);
        Invoke("openDialogue2", 10);
    }
    public void openDialogue2()
    {
        VO1.enabled = true;
        UISounds[2].Play();
        speechBubble.sprite = speechBubble2;
        Invoke("openDialogue5", 3);
    } 
    public void openDialogue5()
    {
        speechBubble.sprite = speechBubble5;
        Invoke("closeAll1", 3); //enable arrow sprite after this
    } 
    public void openDialogue7()
    {
        VO1.enabled = true;
        UISounds[2].Play();
        speechBubble.sprite = speechBubble7;
        Invoke("openDialogue8", 5);
    }
    public void openDialogue8()
    {
        speechBubble.sprite = speechBubble8;        
        Invoke("closeAll2", 5); //enable second arrow after this
        
    }
    public void closeAll()
    {
        VO1.enabled = false;
        didYouKnow.enabled = false;
        
    }
    public void closeAll1()
    {
        VO1.enabled = false;
        arrowSprite.enabled = true;
    }
    public void closeAll2()
    {
        VO1.enabled = false;
        arrowSprite2.enabled = true;
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