using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartControl : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    public void MoveLR()
    {
        //transform.position = new Vector3(-6, 2, 2);
        anim.Play("leftMoveRight");
    }

    public void MoveRL()
    {
        //transform.position = new Vector3(6, 2, 2);
        anim.Play("rightMoveLeft");
    }
}
