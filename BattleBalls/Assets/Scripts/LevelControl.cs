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
    private int currentCol1 = -1, currentCol2 = -1;
    private GameObject selectTail = null;
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
        } while (false == TestColorTails(numPos, numCol));
        Vector3 pos = Vector3.zero;
        pos.x = (numPos % 8) - 3.5f;
        pos.z = (numPos / 8) - 3.5f;
        GameObject go = Instantiate(tailPrefab, pos, Quaternion.identity);
        go.GetComponent<TailControl>().SetColor(numCol, arMats[numCol]);
        pole64[numPos] = numCol;
        poleGO[numPos] = go;
    }

    private bool TestColorTails(int n, int c)
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

    private bool TestTails(int n1, int n2)
    {
        int x = n1 % 8, y = n1 / 8;
        if ((y > 0) && (n1 - 8 == n2)) return false;
        if ((y < 7) && (n1 + 8 == n2)) return false;
        if ((x > 0) && (n1 - 1 == n2)) return false;
        if ((x < 7) && (n1 + 1 == n2)) return false;
        return true;
    }

    public void TranslateColor(Color col, int numCol)
    {
        if (modeSteps == 0 || modeSteps == 2) { currentCol1 = numCol; ui_Control.ViewCross(modeSteps, false); }
        if (modeSteps == 1 || modeSteps == 3) { currentCol2 = numCol; ui_Control.ViewCross(modeSteps, false); }
        ui_Control.ViewBalls(modeSteps++, col);
        modeSteps %= 2;
        if ((modeSteps % 2) > 0) GetNextStep();
        else Invoke("DeactiveRndColors", 1f);
    }

    private void GetNextStep()
    {
        rndColors.gameObject.SetActive(true);
        rndColors.SetCast();
    }

    private void DeactiveRndColors()
    {
        rndColors.gameObject.SetActive(false);
    }

    private void SwapTile(int src, int dst)
    {
        GameObject tmp = poleGO[src];
        poleGO[src] = poleGO[dst];
        poleGO[dst] = tmp;
        int ind = pole64[src];
        pole64[src] = pole64[dst];
        pole64[dst] = ind;
    }

    public void TranslatePosition(Vector3 pos)
    {
        int x = Mathf.RoundToInt(pos.x + 3.5f);
        int y = Mathf.RoundToInt(pos.z + 3.5f);
        int num = 8 * y + x;
        //print($"TranslatePosition  x={x}  y={y}  num={num}");
        Vector3 tg1;
        if (selectTail == null)
        {
            if ((currentCol1 != -1 && currentCol1 == pole64[num]) || (currentCol2 != -1 && currentCol2 == pole64[num]))
            {
                selectTail = poleGO[num];
                tg1.x = x - 3.5f; tg1.z = y - 3.5f;
                tg1.y = 0.5f;
                selectTail.GetComponent<TailControl>().SetTarget(tg1);
                //print($"pos = {pos}   selectTail = null  tg1 = {tg1}");
            }
        }
        else
        {
            if (selectTail == poleGO[num])
            {
                tg1.x = x - 3.5f; tg1.z = y - 3.5f;
                tg1.y = 0;
                selectTail.GetComponent<TailControl>().SetTarget(tg1);
                selectTail = null;
                //print($"pos = {pos}   selectTail != null  tg1 = {tg1}");
                return;
            }
            else
            {
                int sx = Mathf.RoundToInt(selectTail.transform.position.x + 3.5f);
                int sy = Mathf.RoundToInt(selectTail.transform.position.z + 3.5f);
                int selectNum = 8 * sy + sx;
                if (!TestTails(selectNum, num))
                {   //  рядом - меняем местами тайлы
                    tg1.x = sx - 3.5f; tg1.z = sy - 3.5f;
                    tg1.y = 0;
                    poleGO[num].GetComponent<TailControl>().SetTarget(tg1);
                    tg1.x = x - 3.5f; tg1.z = y - 3.5f;
                    tg1.y = 0;
                    selectTail.GetComponent<TailControl>().SetTarget(tg1);
                    if (pole64[selectNum] == currentCol1) { currentCol1 = -1; ui_Control.ViewCross(0, true); }
                    if (pole64[selectNum] == currentCol2) { currentCol2 = -1; ui_Control.ViewCross(1, true); }
                    SwapTile(selectNum, num);
                    selectTail = null;
                    if (currentCol1 == -1 && currentCol2 == -1)
                    {   //  ход игрока сделан - теперь ход БОТа

                    }
                }
                else
                {   //  далеко - меняем selectTail
                    tg1.x = sx - 3.5f; tg1.z = sy - 3.5f;
                    tg1.y = 0;
                    selectTail.GetComponent<TailControl>().SetTarget(tg1);
                    selectTail = poleGO[num];
                    if ((currentCol1 != -1 && currentCol1 == pole64[num]) || (currentCol2 != -1 && currentCol2 == pole64[num]))
                    {
                        tg1.x = x - 3.5f; tg1.z = y - 3.5f;
                        tg1.y = 0.5f;
                        selectTail.GetComponent<TailControl>().SetTarget(tg1);
                    }
                }
            }
        }
    }

    public void SelectTile(GameObject go)
    {

    }

    public void TileMove(Vector3 pos)
    {

    }
}
