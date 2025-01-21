using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardControl : MonoBehaviour
{
    [SerializeField] private LevelControl levelControl;

    private int x, z;
    private bool isOver = false;

    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (boxCollider != null) boxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (isOver)
        {
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //int mx = Mathf.RoundToInt((mp.x + 3.5f) / 2);
            //int mz = Mathf.RoundToInt((mp.z + 3.5f) / 2);
            int mx = Mathf.RoundToInt(mp.x + 3.5f);
            int mz = Mathf.RoundToInt(mp.z + 3.5f);
            if ((mx != x) || (mz != z))
            {
                if (((mx >= 0) && (mx < 8)) && ((mz >= 0) && (mz < 8)))
                {
                    levelControl.BoardOverPoint(mx, mz);
                }
                x = mx;z = mz;
            }
        }
    }

    public void SetIsOver(bool zn)
    {
        isOver = zn;
        if (boxCollider != null) boxCollider.enabled = isOver;
        print($" SetIsOver isOver = {isOver}");
    }
}
