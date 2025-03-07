using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform Cam, cameraTrans;

    void Update(){
        cameraTrans.LookAt(Cam);
    }
}