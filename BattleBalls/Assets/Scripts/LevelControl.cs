using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private RndColorsControl rndColors;
    [SerializeField] private UI_control ui_Control;

    [SerializeField] private Material[] arMats;
    [SerializeField] private GameObject tailPrefab;

    private int modeSteps = 0;
    private int[] pole64;
    private GameObject[] poleGO;
    // Start is called before the first frame update
    void Start()
    {
        GeneratePole();
        GetNextStep();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GeneratePole()
    {
        pole64 = new int[64];
        poleGO = new GameObject[64];
        int i;
        for (i = 0; i < 64; i++)
        {
            pole64[i] = -1;
        }
        for (i = 0; i < 64; i++)
        {
                GenerateTail(i);
        }
    }

    private void GenerateTail(int numPos)
    {
        int numCol;
        do
        {
            numCol = Random.Range(0, 8);
        } while (false == TestTails(numPos, numCol));
        Vector3 pos = Vector3.zero;
        pos.x = (numPos % 8) - 3.5f;
        pos.z = (numPos / 8) - 3.5f;
        GameObject go = Instantiate(tailPrefab, pos, Quaternion.identity);
        go.GetComponent<TailControl>().SetColor(numCol, arMats[numCol]);
        pole64[numPos] = numCol;
        poleGO[numPos] = go;
    }

    private bool TestTails(int n, int c)
    {
        int x = n % 8, y = n / 8;
        /*if (y < 7)
        {
            if (pole64[n + 8] == c) return false;
            if ((x > 0) && (pole64[n + 7] == c)) return false;
            if ((x < 7) && (pole64[n + 9] == c)) return false;
        }
        if (y > 0)
        {
            if (pole64[n - 8] == c) return false;
            if ((x > 0) && (pole64[n - 9] == c)) return false;
            if ((x < 7) && (pole64[n - 7] == c)) return false;
        }*/
        if ((y > 0) && (pole64[n - 8] == c)) return false;
        if ((y < 7) && (pole64[n + 8] == c)) return false;
        if ((x > 0) && (pole64[n - 1] == c)) return false;
        if ((x < 7) && (pole64[n + 1] == c)) return false;
        return true;
    }

    public void TranslateColor(Color col)
    {
        ui_Control.ViewBalls(modeSteps++, col);
        modeSteps %= 2;
        if ((modeSteps % 2) > 0) GetNextStep();
        else rndColors.gameObject.SetActive(false);
    }

    private void GetNextStep()
    {
        rndColors.gameObject.SetActive(true);
        rndColors.SetCast();
    }
}
