using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private UI_control ui_Control;
    [SerializeField] private LevelControl levelControl;

    private int[] pole;
    private int col1, col2;
    private List<int> arNumTile;
    private List<StepCase> arSteps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateNextStep(int numCol1, int numCol2, int[] pole64)
    {
        int i;
        col1 = numCol1;
        col2 = numCol2;
        pole = new int[64];
        arNumTile = new List<int>();
        for(i = 0; i < 64; i++)
        {
            pole[i] = pole64[i];
            if (pole[i] == numCol1 && numCol1 != -1) arNumTile.Add(i);
            if (pole[i] == numCol2 && numCol2 != -1) arNumTile.Add(i);
        }
        if (numCol1 != -1 && numCol2 != -1) Step2Col();
        else Step1Col();
    }

    private void Step2Col()
    {
        arSteps = new List<StepCase>();
        int[] p = new int[64];
        int i, np, x, y;
        foreach(int n in arNumTile)
        {
            for (i = 0; i < 64; i++) p[i] = pole[i];
            x = n % 8;y = n / 8;
            if (x > 0)
            {
                np = n - 1;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
            }
            if (x < 7)
            {
                np = n + 1;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
            }
            if (y > 0)
            {
                np = n - 8;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
            }
            if (y < 7)
            {
                np = n + 8;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
            }
        }
        SelectBestStep();
    }

    private void Step1Col()
    {
        arSteps = new List<StepCase>();
        int[] p = new int[64];
        int i, np, x, y;
        foreach (int n in arNumTile)
        {
            for (i = 0; i < 64; i++) p[i] = pole[i];
            x = n % 8; y = n / 8;
            if (x > 0)
            {
                np = n - 1;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
            }
            if (x < 7)
            {
                np = n + 1;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
            }
            if (y > 0)
            {
                np = n - 8;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
            }
            if (y < 7)
            {
                np = n + 8;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
            }
        }
        SelectBestStep();
    }

    private List<int> TestTotalMath3(int d, int s, int[] p)
    {
        List<int> ar = new List<int>();
        return ar;
    }

    private void SelectBestStep()
    {
        int i, numStep = 0, maxQual = 0;
        for(i = 0; i < arSteps.Count; i++)
        {
            if (arSteps[i].quality > maxQual)
            {
                numStep = i;
                maxQual = arSteps[i].quality;
            }
        }
        levelControl.TranslateStep(arSteps[numStep].dst, arSteps[numStep].src);
    }
}

public class StepCase
{
    public int dst; //  откуда
    public int src; //  куда
    public int quality = 0; //  выиграшность хода

    public StepCase() { }
    public StepCase(int d, int s, int q)
    {
        dst = d;
        src = s;
        quality = q;
    }
}
