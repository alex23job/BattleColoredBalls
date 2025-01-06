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
                if ((ar.Count == 0) && TestTile2(n, np, p)) arSteps.Add(new StepCase(n, np, 2));
            }
            if (x < 7)
            {
                np = n + 1;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
                if ((ar.Count == 0) && TestTile2(n, np, p)) arSteps.Add(new StepCase(n, np, 2));
            }
            if (y > 0)
            {
                np = n - 8;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
                if ((ar.Count == 0) && TestTile2(n, np, p)) arSteps.Add(new StepCase(n, np, 2));
            }
            if (y < 7)
            {
                np = n + 8;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
                if ((ar.Count == 0) && TestTile2(n, np, p)) arSteps.Add(new StepCase(n, np, 2));
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
                if ((ar.Count == 0) && TestTile2(n, np, p)) arSteps.Add(new StepCase(n, np, 2));
            }
            if (x < 7)
            {
                np = n + 1;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
                if ((ar.Count == 0) && TestTile2(n, np, p)) arSteps.Add(new StepCase(n, np, 2));
            }
            if (y > 0)
            {
                np = n - 8;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
                if ((ar.Count == 0) && TestTile2(n, np, p)) arSteps.Add(new StepCase(n, np, 2));
            }
            if (y < 7)
            {
                np = n + 8;
                List<int> ar = TestTotalMath3(n, np, p);
                arSteps.Add(new StepCase(n, np, ar.Count));
                if ((ar.Count == 0) && TestTile2(n, np, p)) arSteps.Add(new StepCase(n, np, 2));
            }
        }
        SelectBestStep();
    }

    private List<int> TestTotalMath3(int d, int s, int[] p)
    {
        List<int> ar = new List<int>();
        int[] pl = new int[64];
        int i, j;
        for (i = 0; i < 64; i++) pl[i] = p[i];
        j = pl[d];
        pl[d] = pl[s];
        pl[s] = j;

        for (i = 0; i < 8; i++)
        {
            for (j = 0; j < 8; j++)
            {
                List<int> tmp = TestTiles3p(8 * i + j, pl);
                if (tmp.Count > 0)
                {
                    foreach (int n in tmp)
                    {
                        bool isNew = true;
                        foreach (int a in ar)
                        {
                            if (a == n)
                            {
                                isNew = false;
                                break;
                            }
                        }
                        if (isNew) ar.Add(n);
                    }
                }
            }
        }

        return ar;
    }

    private List<int> TestTiles3p(int num, int[] pole64)
    {
        List<int> ar = new List<int>();
        int x = num % 8, y = num / 8, nc = pole64[num];
        //print($"TestTiles3p num={num} x={x} y={y} col={nc}");
        if (nc == -1) return ar;
        if (x < 6)
        {   //  линия горизонтальная
            if ((pole64[num + 1] == nc) && (pole64[num + 2] == nc))
            {
                ar.Add(num); ar.Add(num + 1); ar.Add(num + 2);
                if ((x < 5) && (pole64[num + 3] == nc))
                {
                    ar.Add(num + 3);
                    if ((x < 4) && (pole64[num + 4] == nc)) ar.Add(num + 4);
                }
            }
        }
        if (y < 6)
        {   //  линия вертикальная
            if ((pole64[num + 8] == nc) && (pole64[num + 16] == nc))
            {
                ar.Add(num); ar.Add(num + 8); ar.Add(num + 16);
                if ((y < 5) && (pole64[num + 24] == nc))
                {
                    ar.Add(num + 24);
                    if ((y < 4) && (pole64[num + 32] == nc)) ar.Add(num + 32);
                }
            }
        }
        if ((x < 7) && (y < 7))
        {   //  квадрат
            if ((pole64[num + 1] == nc) && (pole64[num + 8] == nc) && (pole64[num + 9] == nc))
            {
                ar.Add(num); ar.Add(num + 1); ar.Add(num + 8); ar.Add(num + 9);
            }
        }
        return ar;
    }

    private bool TestTile2(int d, int s, int[] p)
    {
        int[] pl = new int[64];
        int i, j;
        for (i = 0; i < 64; i++) pl[i] = p[i];
        j = pl[d];
        pl[d] = pl[s];
        pl[s] = j;
        //  две одного цвета рядом ?
        int x = d % 8, y = d / 8, nc = pl[d];
        if (x > 0 && pl[d - 1] == nc) return true;
        if (x < 7 && pl[d + 1] == nc) return true;
        if (y > 0 && pl[d - 8] == nc) return true;
        if (y < 7 && pl[d + 8] == nc) return true;

        x = s % 8; y = s / 8; nc = pl[s];
        if (x > 0 && pl[s - 1] == nc) return true;
        if (x < 7 && pl[s + 1] == nc) return true;
        if (y > 0 && pl[s - 8] == nc) return true;
        if (y < 7 && pl[s + 8] == nc) return true;

        return false;
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
