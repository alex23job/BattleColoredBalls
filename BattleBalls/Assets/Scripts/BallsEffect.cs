using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsEffect : MonoBehaviour
{
    private Animator anim;
    private ParticleSystem ballEmpl;
    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        ballEmpl = transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveToRight()
    {
        anim.Play("ToRight");
        ballEmpl.Play();
    }

    public void MoveToLeft()
    {
        anim.Play("ToLeft");
        ballEmpl.Play();
    }
}
