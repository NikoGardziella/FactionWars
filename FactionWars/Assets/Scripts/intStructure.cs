using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intStructure : MonoBehaviour
{
    public Material redMaterialTower;
    void Start()
    {
        if (transform.parent.parent.tag == "Enemy")
        {

            gameObject.GetComponent<Renderer>().material = redMaterialTower; //(UnitColorRed);
        }

    }
}
