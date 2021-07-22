using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtController : MonoBehaviour
{
    public Material destroyed;
    [HideInInspector]
    public bool swapped;

    // Start is called before the first frame update
    void Start()
    {
        swapped = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Collect()
    {
        Destroy(gameObject);
    }

    public void Swap()
    {
        gameObject.GetComponent<MeshRenderer>().material = destroyed;
        swapped = true;
    }
}
