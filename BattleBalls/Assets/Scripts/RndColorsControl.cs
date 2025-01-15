using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RndColorsControl : MonoBehaviour
{
    static public Color GetColor(int n)
    {
        if (n >= 0 && n < 8) return arCols[n];
        else return Color.white;
    }

    [SerializeField] private GameObject ball;
    [SerializeField] private float force = 15f;

    [SerializeField] private LevelControl lc;

    static Color[] arCols = { Color.red, Color.green, Color.yellow, Color.blue, Color.cyan, Color.magenta, new Color(0.6f, 0.4f, 0.1f), new Color(1f, 0.6f, 0.2f) };
    int[] arNumCols = { 0, 6, 4, 2, 5, 3, 1, 7, 7, 2, 0, 5, 1, 4, 6, 3};

    private Rigidbody rigidbodyBall;
    private float timer = 0.5f;
    private bool isRnd = false;
    private Vector3 oldPos;

    private void Awake()
    {
        rigidbodyBall = ball.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRnd)
        {
            if (timer > 0) timer -= Time.deltaTime;
            else
            {
                timer = 0.25f;
                if ((oldPos != ball.transform.position) && (ball.transform.position.y > 1.4f))
                {
                    oldPos = ball.transform.position;
                }
                else
                {   //  шарик остановился - определяем где
                    if (lc != null)
                    {
                        oldPos = ball.transform.position;
                        int x, y;
                        x = Mathf.RoundToInt(oldPos.x - transform.position.x + 1.5f);
                        y = Mathf.RoundToInt(oldPos.z - transform.position.z + 1.5f);
                        //print($"pos => {ball.transform.position}  x={x} y={y}");
                        lc.TranslateColor(arCols[arNumCols[4 * y + x]], arNumCols[4 * y + x]);
                        isRnd = false;
                    }
                }
            }
        }
    }

    public void SetCast()
    {
        //print("new SetCast");
        Invoke("SetIsRnd", 0.5f);
        Vector3 direction = Vector3.up;
        direction.x = Random.Range(-0.5f, 0.5f);
        direction.z = Random.Range(-0.5f, 0.5f);
        if ((direction.x > -0.05f) && (direction.x < 0.05f)) direction.x += Random.Range(0.2f, 0.6f);
        if ((direction.z > -0.05f) && (direction.z < 0.05f)) direction.z += Random.Range(0.2f, 0.6f);
        rigidbodyBall.AddForce(direction * force, ForceMode.Impulse);
    }

    private void SetIsRnd()
    {
        isRnd = true;
    }
}
