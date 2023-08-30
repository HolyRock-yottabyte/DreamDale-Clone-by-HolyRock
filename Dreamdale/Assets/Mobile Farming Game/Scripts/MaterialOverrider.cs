using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOverrider : MonoBehaviour
{
    public Material material;
    public int rq;

    // Start is called before the first frame update
    void Start()
    {
        material.renderQueue--;
    }

    // Update is called once per frame
    void Update()
    {
        material.renderQueue = rq;   
    }
}
