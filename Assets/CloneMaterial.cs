using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneMaterial : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material = Instantiate(meshRenderer.material);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
