using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Created by Mattie Hilton - 03/10/2021 
 */
public class Rotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0,45*Time.deltaTime,0));
    }
}
