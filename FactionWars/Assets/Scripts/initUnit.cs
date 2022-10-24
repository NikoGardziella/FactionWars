using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initUnit : MonoBehaviour
{
    public Material redMaterial;
    void Start()
    {
        if(transform.parent.parent.parent.parent.CompareTag("Enemy"))
        {
            gameObject.GetComponent<Renderer>().material = redMaterial; //(UnitColorRed);
        }

    }
}
