using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenerManager : MonoBehaviour {


    public Generator generatorRef;
    private int indexRelated;

    private void Start()
    {
        string name = transform.name;
        indexRelated = int.Parse(name.Substring(6));
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            generatorRef.SwitchDoors(indexRelated);
        }
    }
}
