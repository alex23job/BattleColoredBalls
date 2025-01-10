using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinControl : MonoBehaviour
{
    private Animator anim;
    private ParticleSystem toxinEmpl;
    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        toxinEmpl = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
    }

    public void MoveToRight()
    {
        anim.Play("ToRight");
        toxinEmpl.Play();
    }

    public void MoveToLeft()
    {
        anim.Play("ToLeft");
        toxinEmpl.Play();
    }
}
