using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicControl : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
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
