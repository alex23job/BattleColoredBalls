using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailControl : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private int nCol;
    public int NumCol { get { return nCol; } }

    private Vector3 target;
    private bool isMove = false;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            if (transform.position != target)
            {
                Vector3 pos = transform.position;
                //if (pos.x == target.x) return;
                Vector3 delta = target - pos;

                //delta.y = 0;
                if (delta.magnitude > 0.05f)
                {
                    Vector3 movement = delta.normalized * moveSpeed * Time.deltaTime;

                    if (movement.magnitude <= delta.magnitude)
                    {
                        //rb.AddForce(movement, ForceMode.Impulse);
                        //transform.Translate(movement);
                        transform.position += movement;
                        //anim.SetFloat("manSpeed", delta.magnitude);
                    }
                    else
                    {
                        transform.position = target;
                        //transform.rotation = rotate;
                        //anim.SetFloat("manSpeed", 0);
                        //audio_effect.Stop();
                        isMove = false;
                    }
                }
                else
                {
                    transform.position = target;
                    //transform.rotation = rotate;
                    //anim.SetFloat("manSpeed", 0);
                    //audio_effect.Stop();
                    isMove = false;
                }
            }
        }
    }

    public void SetTarget(Vector3 tg)
    {
        target = tg;
        isMove = true;
    }

    public void SetNewPosition(Vector3 tg)
    {
        transform.position = tg;
    }

    public void SetColor(int num, Material matColor)
    {
        nCol = num;
        MeshRenderer mr = transform.gameObject.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            Material tileMat = mr.materials[0];
            mr.materials = new Material[] { tileMat, matColor };
        }
    }

    public void DeletingTail()
    {
        Destroy(gameObject);
    }
}
