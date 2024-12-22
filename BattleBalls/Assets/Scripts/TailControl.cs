using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailControl : MonoBehaviour
{

    private int nCol;
    public int NumCol { get { return nCol; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
