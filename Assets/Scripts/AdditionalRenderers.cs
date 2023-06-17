using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalRenderers : MonoBehaviour
{
    // Start is called before the first frame update
    public Renderer[] ColorMeshes;
    public List<Renderer> SecondaryMeshes;
    void Start()
    {
        
    }

    public void ToggleMeshVisibility(bool SHOW)
    {
        Material DoodleMAT = GetComponent<TargetScript>().DoodleMAT;

        foreach (Renderer item in SecondaryMeshes)
        {
            item.enabled = SHOW;
        }

        foreach (Renderer item in ColorMeshes)
        {
            item.enabled = SHOW;
            if (SHOW)
            {
                item.material = DoodleMAT;

            }
        }



    }

    public void ToggleMaterials(Material MAT)
    {
        foreach (Renderer item in ColorMeshes)
        {
            item.material = MAT;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
