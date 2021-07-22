using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityController : MonoBehaviour
{
    // How long the Camera pauses in between turning
    public int waitLength;
    bool turnRight;
    bool turnLeft;
    bool wait;
    bool detected;
    public int cameraAngle;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public bool stun;

    Quaternion rightRotate;
    Quaternion leftRotate;
    Quaternion stunnedRotate;

    private float timeCount = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        turnRight = true;
        turnLeft = false;
        wait = true;
        detected = false;
        stun = false;

        rightRotate = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + cameraAngle, transform.localEulerAngles.z);
        leftRotate = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y - cameraAngle, transform.localEulerAngles.z);
        stunnedRotate = Quaternion.Euler(80, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (turnRight == true && detected == false && stun == false)
        {
            transform.rotation = Quaternion.Slerp(leftRotate, rightRotate, timeCount);
            timeCount = timeCount + Time.deltaTime;
            if (wait == true)
            {
                StartCoroutine(WaitRight());
            }
        }
        if (turnLeft == true && detected == false && stun == false)
        {
            transform.rotation = Quaternion.Slerp(rightRotate, leftRotate, timeCount);
            timeCount = timeCount + Time.deltaTime;
            if (wait == true)
            {
                StartCoroutine(WaitLeft());
            }
        }

        Vector3 inFront = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, inFront, out hit, 10))
        {
            GameController instance = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameController>();
            if (hit.collider.gameObject.CompareTag("Player") && instance.detection < 100 && stun == false)
            {
                detected = true;
                instance.AddDetection();
            }
        }
        else
        {
            detected = false;
        }

        if (detected == true && stun == false)
        {
            GameObject example = GameObject.FindGameObjectWithTag("Player");
            target = example.transform;
            transform.LookAt(target);
        }

        if (stun == true)
        {
            transform.rotation = Quaternion.Slerp(stunnedRotate, stunnedRotate, 1);
            StartCoroutine(Waiting());
        }
    }

    IEnumerator WaitRight()
    {
        wait = false;
        yield return new WaitForSeconds(waitLength);
        turnRight = false;
        timeCount = 0.0f;
        turnLeft = true;
        wait = true;
    }

    IEnumerator WaitLeft()
    {
        wait = false;
        yield return new WaitForSeconds(waitLength);
        turnLeft = false;
        timeCount = 0.0f;
        turnRight = true;
        wait = true;
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(5f);
        stun = false;
    }
}
