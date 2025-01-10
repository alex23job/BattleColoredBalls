using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicControl : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    public void MoveToRight()
    {
        anim.Play("ToRight");
    }

    public void MoveToLeft()
    {
        anim.Play("ToLeft");
    }
}
