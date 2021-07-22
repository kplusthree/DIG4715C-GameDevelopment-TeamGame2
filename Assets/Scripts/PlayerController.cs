using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public CharacterController controller;
    public float speed = 12f;
    [HideInInspector]
    public bool sprayed;
    [HideInInspector]
    public bool spraying;

    public Animator animator;
    public ParticleSystem paint;
    public ParticleSystem sparks;

    public AudioClip swipeSound;
    public AudioClip shakeSound;
    public AudioClip spraySound;

    public AudioSource soundSource;

    void Start()
    {
        sprayed = false;
        spraying = false;
        animator.SetBool("Shaking", false);
        animator.SetBool("Waited", false);
        animator.speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        GameController instance = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();

        if (instance.gameOver == false && spraying == false)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
        }
        
        Ray inFront = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inFront, out hit, 2))
        {
            ArtController example = hit.collider.gameObject.GetComponent<ArtController>();
            if (example != null && Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.gameObject.CompareTag("FinalArt"))
                {
                    instance.AddPoints(500);
                    instance.win = true;
                }
                else
                {
                    instance.AddPoints(50);
                }
                soundSource.clip = swipeSound;
                soundSource.Play();
                example.Collect();
            }
            if (example != null && Input.GetKeyDown(KeyCode.Q) && sprayed == false && example.swapped == false)
            {
                if (hit.collider.gameObject.CompareTag("FinalArt"))
                {
                    instance.AddPoints(1000);
                    instance.win = true;
                }
                else
                {
                    instance.AddPoints(100);
                }
                StartCoroutine(SprayingArt(example));
            }
            if (hit.collider.gameObject.CompareTag("Security") && Input.GetKeyDown(KeyCode.Q))
            {
                SecurityController occasion = hit.collider.gameObject.GetComponent<SecurityController>();
                if (occasion.stun == false && sprayed == false)
                {
                    StartCoroutine(SprayingCams(occasion));
                    sparks.Play();
                }
            }
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(10);
        sprayed = false;
    }

    IEnumerator SprayingArt(ArtController example)
    {
        spraying = true;
        animator.SetBool("Shaking", true);
        soundSource.clip = shakeSound;
        soundSource.Play();
        sprayed = true;
        StartCoroutine(Waiting());
        yield return new WaitForSeconds(1);
        animator.SetBool("Waited", true);
        paint.Play();
        soundSource.clip = spraySound;
        soundSource.Play();
        yield return new WaitForSeconds(3);
        example.Swap();
        spraying = false;
        animator.SetBool("Shaking", false);
        animator.SetBool("Waited", false);
    }

    IEnumerator SprayingCams(SecurityController occasion)
    {
        spraying = true;
        animator.SetBool("Shaking", true);
        soundSource.clip = shakeSound;
        soundSource.Play();
        sprayed = true;
        StartCoroutine(Waiting());
        yield return new WaitForSeconds(1);
        animator.SetBool("Waited", true);
        paint.Play();
        soundSource.clip = spraySound;
        soundSource.Play();
        yield return new WaitForSeconds(3);
        occasion.stun = true;
        spraying = false;
        animator.SetBool("Shaking", false);
        animator.SetBool("Waited", false);
    }
}