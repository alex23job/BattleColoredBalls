using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private GameObject infoPanelPlayer;
    [SerializeField] private GameObject infoPanelBot;
    [SerializeField] private RndColorsControl rndColors;
    [SerializeField] private UI_control ui_Control;
    [SerializeField] private BoardControl boardControl;

    [SerializeField] private Material[] arMats;
    [SerializeField] private GameObject tailPrefab;

    [SerializeField] private EnemyControl enemyControl;
    [SerializeField] private EnemyAI enemyLogic;

    [SerializeField] private PlayerWarior player;

    [SerializeField] private HeartControl heartControl;
    [SerializeField] private MedicControl medicControl;
    [SerializeField] private ToxinControl toxinControl;
    [SerializeField] private BallsEffect magicEffect;
    [SerializeField] private BallsEffect fireEffect;

    /// <summary>
    /// ����������� ������������� ������: 1 - Vert, 2 - Hor, 3 - Rect
    /// </summary>
    private int bonusMode = 0;
    /// <summary>
    /// ����� ����� ( ������������ ��� �������������� 0 - 7 ) ��� �������� ( 0 - 3 )
    /// � ����������� �� ���������� ������
    /// </summary>
    private int numLineOrRect = -1;
    private int modeSteps = 0;
    private int[] pole64;
    private GameObject[] poleGO;
    private int currentCol1 = -1, currentCol2 = -1;
    private GameObject selectTail = null;

    private int[] cntColBalls;

    private Vector3 rndPosL, rndPosR;

    public bool isClick = true;
    // Start is called before the first frame update
    void Start()
    {
        rndPosL = new Vector3(-6.5f, 1.5f, -4f);
        rndPosR = new Vector3(6.5f, 1.5f, -4f);
        GeneratePole();
        GetNextStep();
        int numWarior = GameManager.Instance.currentPlayer.currentLevel;
        if (numWarior > WariorParams.arrWariorParams.Length - 1) numWarior = WariorParams.arrWariorParams.Length - 1;
        enemyControl.SetParams(WariorParams.arrWariorParams[numWarior]);
        ui_Control.ViewBonus(0, 3); ui_Control.ViewBonus(0, 4); //  � ���� ��� �������
        ui_Control.ViewBonus(0, 1); ui_Control.ViewBonus(0, 2); //  � ������ ����� ���� ��������� ������
        switch (enemyControl.Immunity)
        {
            case 1: ui_Control.ViewImm(true, 4); break;
            case 2: ui_Control.ViewImm(true, 5); break;
            case 3: ui_Control.ViewImm(true, 6); break;
            case 4: ui_Control.ViewImm(true, 7); break;
        }
        ui_Control.ViewName(GameManager.Instance.currentPlayer.playerName, 1);
        player.SetPlayerInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (bonusMode > 0)
        {
            if (Input.GetMouseButtonUp(0) && numLineOrRect != -1)
            {
                DeletingBonusTails();
                bonusMode = 0;
            }
        }
    }

    private void DeletingBonusTails()
    {
        List<int> ar = new List<int>();
        cntColBalls = new int[8];
        int i;
        if (bonusMode == 1)
        {
            for (i = numLineOrRect; i < 64; i += 8) ar.Add(i);
            GameManager.Instance.currentPlayer.countBonusLine = 0;
            ui_Control.ViewBonus(0, 1);
        }
        if (bonusMode == 2)
        {
            for (i = 0; i < 8; i++) ar.Add(i + 8 * numLineOrRect);
            GameManager.Instance.currentPlayer.countBonusLine = 0;
            ui_Control.ViewBonus(0, 1);
        }
        if (bonusMode == 3)
        {
            bool sel;
            for (i = 0; i < 64; i++)
            {
                sel = false;
                if (((i % 8) < 4) && ((i / 8) < 4) && (numLineOrRect == 0)) sel = true;
                if (((i % 8) < 4) && ((i / 8) >= 4) && (numLineOrRect == 2)) sel = true;
                if (((i % 8) >= 4) && ((i / 8) < 4) && (numLineOrRect == 1)) sel = true;
                if (((i % 8) >= 4) && ((i / 8) >= 4) && (numLineOrRect == 3)) sel = true;
                if (sel) ar.Add(i);
            }
            GameManager.Instance.currentPlayer.countBonusRect = 0;
            ui_Control.ViewBonus(0, 2);
        }
        DelTiles(ar);
        DownTiles();
        UseBalls();
        CikleTest();
        numLineOrRect = -1;
        boardControl.SetIsOver(false);
        currentCol1 = -1; ui_Control.ViewCross(0, true);
        currentCol2 = -1; ui_Control.ViewCross(1, true);
        GetNextStep();
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

    private void GenerateTail(int numPos, bool testAllColors = false)
    {
        int numCol = -1;
        if (testAllColors)
        {
            int mask = 0, i;
            for (i = 0; i < 64; i++)
            {
                if (pole64[i] != -1)
                {
                    mask |= 1 << pole64[i];
                    if (mask == 0xff) break;
                }
            }
            if (mask != 0xff)
            {
                for (i = 0; i < 8; i++)
                {
                    if ((mask & (1 << i)) == 0)
                    {
                        numCol = i;break;
                    }
                }
            }
        }
        if (numCol == -1)
        {
            do
            {
                numCol = Random.Range(0, 8);
            } while (false == TestColorTails(numPos, numCol));
        }
        Vector3 pos = Vector3.zero;
        pos.x = (numPos % 8) - 3.5f;
        pos.z = (numPos / 8) - 3.5f;
        GameObject go = Instantiate(tailPrefab, pos, Quaternion.identity);
        go.GetComponent<TailControl>().SetColor(numCol, arMats[numCol]);
        pole64[numPos] = numCol;
        poleGO[numPos] = go;
    }

    public void PlayerKilled()
    {
        GameManager.Instance.currentPlayer.immunity = 0;
        GameManager.Instance.currentPlayer.LevelLoss();
        GameManager.Instance.SaveGame();
        ui_Control.ViewLoss(enemyControl.BotName);
    }

    public void BotKilled()
    {
        GameManager.Instance.currentPlayer.immunity = 0;
        GameManager.Instance.currentPlayer.LevelComplete();
        GameManager.Instance.SaveGame();
        ui_Control.ViewWin(enemyControl.BotName);
    }

    private bool TestColorTails(int n, int c)
    {
        int x = n % 8, y = n / 8;
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
        //  �������� ��� ������ ���� ����� ���� � ����� ������ ��� !!!
        if (modeSteps == 0 || modeSteps == 2) { currentCol1 = numCol; ui_Control.ViewCross(modeSteps, false); }
        if (modeSteps == 1 || modeSteps == 3) { currentCol2 = numCol; ui_Control.ViewCross(modeSteps, false); }
        SelectCurrentCol(numCol, true);
        
        ui_Control.ViewBalls(modeSteps++, col);
        modeSteps %= 4;
        
        if ((modeSteps % 2) > 0) GetNextStep();
        else Invoke("DeactiveRndColors", 1f);
        
        if (modeSteps == 0 /*3*/)
        {   //  ��������� ������ ������� �� �����
            //  ��������� ������� ��� ����
            //enemyLogic.GenerateNextStep(currentCol1, currentCol2, pole64);
            if (enemyControl.StepBreak)
            {   //  ������� ���� ?!
                enemyControl.StepBreak = false;
                currentCol1 = -1; ui_Control.ViewCross(2, true);
                currentCol2 = -1; ui_Control.ViewCross(3, true);
                OffAllTile();
                GetNextStep();
            }
            else
            {
                enemyLogic.GenerateNextStep(currentCol1, currentCol2, pole64);
            }
        }
        if (modeSteps == 2)
        {   //  ��������� ������ ������� �� �����
            isClick = true;
            //print("Set isClick = true");
        }
        else
        {
            isClick = false;
            //print("Set isClick = false");
        }
    }

    private void GetNextStep()
    {
        if (modeSteps == 0) { rndColors.transform.position = rndPosL; infoPanelPlayer.SetActive(false); }
        if (modeSteps == 2) { rndColors.transform.position = rndPosR; infoPanelBot.SetActive(false); }
        rndColors.gameObject.SetActive(true);
        rndColors.SetCast();
    }

    private void DeactiveRndColors()
    {
        rndColors.gameObject.SetActive(false);
        if (modeSteps == 2) { infoPanelPlayer.SetActive(true); }
        if (modeSteps == 0) { infoPanelBot.SetActive(true); }
        //if ((modeSteps == 0)
    }

    private void SelectCurrentCol(int numCol, bool zn)
    {
        for (int i = 0; i < 64; i++)
        {
            if (pole64[i] == numCol)
            {
                poleGO[i].GetComponent<TailControl>().SetHint(zn);
            }
        }
    }

    private void OffAllTile()
    {
        for (int i = 0; i < 64; i++) poleGO[i].GetComponent<TailControl>().SetHint(false);
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

    /// <summary>
    /// ������������ ���� ������ �� "�������" ���� � ������� �� ���� ���
    /// </summary>
    /// <param name="numTile1">����� tile ������</param>
    /// <param name="numTile2">����� tile ����</param>
    public void TranslateStep(int numTile1, int numTile2)
    {
        Vector3 tg1;
        tg1.x = (numTile1 % 8) - 3.5f; tg1.z = (numTile1 / 8) - 3.5f;
        tg1.y = 0;
        poleGO[numTile2].GetComponent<TailControl>().SetTarget(tg1);
        tg1.x = (numTile2 % 8) - 3.5f; tg1.z = (numTile2 / 8) - 3.5f;
        tg1.y = 0;
        poleGO[numTile1].GetComponent<TailControl>().SetTarget(tg1);
        if (pole64[numTile1] == currentCol1)
        {
            if (currentCol1 != currentCol2) SelectCurrentCol(currentCol1, false);
            currentCol1 = -1; ui_Control.ViewCross(2, true);
        }
        else if (pole64[numTile1] == currentCol2)
        {
            if (currentCol1 != currentCol2) SelectCurrentCol(currentCol2, false);
            currentCol2 = -1; ui_Control.ViewCross(3, true);
        }
        SwapTile(numTile1, numTile2);
        selectTail = null;

        Invoke("CikleTest", 0.5f);

        if (currentCol1 == -1 && currentCol2 == -1)
        {   //  ��� ���� ������ - ������ ��� ������
            OffAllTile();
            Invoke("GetNextStep", 0.8f);
            //GetNextStep();
        }
        else
        {
            Invoke("BotNextStep", 0.8f);
        }
    }

    public void BotNextStep()
    {
        enemyLogic.GenerateNextStep(currentCol1, currentCol2, pole64);
    }

    public void TranslatePosition(Vector3 pos)
    {
        //if (modeSteps < 2) return;  //  ?
        //print($"isClick = {isClick}");
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
                {   //  ����� - ������ ������� �����
                    tg1.x = sx - 3.5f; tg1.z = sy - 3.5f;
                    tg1.y = 0;
                    poleGO[num].GetComponent<TailControl>().SetTarget(tg1);
                    tg1.x = x - 3.5f; tg1.z = y - 3.5f;
                    tg1.y = 0;
                    selectTail.GetComponent<TailControl>().SetTarget(tg1);
                    if (pole64[selectNum] == currentCol1) 
                    {
                        if (currentCol1 != currentCol2) SelectCurrentCol(currentCol1, false);
                        currentCol1 = -1; ui_Control.ViewCross(0, true); 
                    }
                    else if (pole64[selectNum] == currentCol2) 
                    {
                        if (currentCol1 != currentCol2) SelectCurrentCol(currentCol2, false);
                        currentCol2 = -1; ui_Control.ViewCross(1, true); 
                    }
                    SwapTile(selectNum, num);
                    selectTail = null;

                    Invoke("CikleTest", 0.5f);

                    if (currentCol1 == -1 && currentCol2 == -1)
                    {   //  ��� ������ ������ - ������ ��� ����
                        OffAllTile();
                        Invoke("GetNextStep", 1f);
                        //GetNextStep();
                    }
                }
                else
                {   //  ������ - ������ selectTail
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

    private void CikleTest()
    {
        cntColBalls = new int[8];
        #region ����� ��������� � ������� 3 � ����� ����������� ������
        //new WaitForSecondsRealtime(1f);
        List<int> ar = new List<int>();
        do
        {
            ar = TestTotalTiles();
            if (ar.Count > 0)
            {
                //new WaitForSecondsRealtime(1f);
                DelTiles(ar);
                DownTiles();
                //new WaitForSecondsRealtime(1f);
            }
            /*StringBuilder sb = new StringBuilder();
            for (int k = 0; k < 8; k++)
            {
                sb.Append($"< {pole64[8 * k]} {pole64[8 * k + 1]} {pole64[8 * k + 2]} {pole64[8 * k + 3]} {pole64[8 * k + 4]} {pole64[8 * k + 5]} {pole64[8 * k + 6]} {pole64[8 * k + 7]} > ");
            }
            print(sb.ToString());*/
        } while (ar.Count > 0);
        MoveTiles();
        //Invoke("MoveTiles", 0.7f);
        /*List<int> ar = TestTiles3(selectNum);
        if (ar.Count > 0)
        {
            DelTiles(ar);
        }
        ar = TestTiles3(num);
        if (ar.Count > 0)
        {
            DelTiles(ar);
        }
        DownTiles();*/
        #endregion
        UseBalls();
    }

    private void UseBalls()
    {
        IWarior warior = enemyControl as IWarior;
        int dmg;
        if (modeSteps == 0)
        {   //  ����� ��� -> ������ ����  
            for (int i = 0; i < 8; i++)
            {
                if (cntColBalls[i] > 0)
                {
                    switch (i)
                    {
                        case 0: //  �������
                            warior.BallsEffect(cntColBalls[i], 0, 0, out dmg);
                            player.BallsDamage(cntColBalls[i], 0, 0);
                            heartControl.gameObject.SetActive(true); heartControl.MoveLR(); Invoke("HeartHide", 1f);
                            break;
                        case 1: //  ������
                            warior.BallsEffect(cntColBalls[i], 1, 0, out dmg);
                            player.BallsDamage(dmg, 1, warior.ToxinPercent);
                            toxinControl.gameObject.SetActive(true); toxinControl.MoveToLeft(); Invoke("ToxinHide", 1f);
                            break;
                        case 2: //  �����
                            warior.BallsEffect(cntColBalls[i], 2, 0, out dmg);
                            break;
                        case 3: //  �����
                            warior.BallsEffect(cntColBalls[i], 3, 0, out dmg);
                            player.BallsDamage(dmg, 3, warior.ToxinPercent);
                            magicEffect.gameObject.SetActive(true); magicEffect.MoveToLeft(); Invoke("MagicHide", 1f);
                            break;
                        case 4: //  ��������� (�������)
                            warior.BallsEffect(cntColBalls[i], 4, 0, out dmg);
                            break;
                        case 5: //  �������
                            warior.BallsEffect(cntColBalls[i], 5, 0, out dmg);
                            medicControl.gameObject.SetActive(true); medicControl.MoveToRight(); Invoke("MedicHide", 1f);
                            break;
                        case 6: //  ����������
                            warior.BallsEffect(cntColBalls[i], 6, 0, out dmg);
                            break;
                        case 7: //  ���������
                            warior.BallsEffect(cntColBalls[i], 7, 0, out dmg);
                            player.BallsDamage(dmg, 7, 0);
                            fireEffect.gameObject.SetActive(true); fireEffect.MoveToLeft(); Invoke("FireHide", 1f);
                            break;
                    }
                }
            }
            if (warior.BonusLine > 0)
            {   //  ������� ��������� �����
                List<int> ar = new List<int>();
                cntColBalls = new int[8];
                int i, numLineOrRect = Random.Range(0, 8);
                bonusMode = 1 + Random.Range(0, 2);
                if (bonusMode == 1)
                {
                    for (i = numLineOrRect; i < 64; i += 8) ar.Add(i);
                }
                if (bonusMode == 2)
                {
                    for (i = 0; i < 8; i++) ar.Add(i + 8 * numLineOrRect);
                }
                print($"Bot bonus line bonusMode => {bonusMode}    numLineOrRect => {numLineOrRect}");
                bonusMode = 0;
                warior.BonusLine = 0;
                DelTiles(ar);
                print($"bonus line cntColBalls => {cntColBalls[0]} {cntColBalls[1]} {cntColBalls[2]} {cntColBalls[3]} {cntColBalls[4]} {cntColBalls[5]} {cntColBalls[6]} {cntColBalls[7]}");
                DownTiles();
                warior.StepsEffect();
                UseBalls();
                print("bonus line CikleTest");
                CikleTest();
            }
            if (warior.BonusRect > 0)
            {   //  ������� ��������� ������� 4�4
                List<int> ar = new List<int>();
                cntColBalls = new int[8];
                bool sel;
                int i, numLineOrRect = Random.Range(0, 4);
                print($"Bot bonus rect numLineOrRect => {numLineOrRect}");
                for (i = 0; i < 64; i++)
                {
                    sel = false;
                    if (((i % 8) < 4) && ((i / 8) < 4) && (numLineOrRect == 0)) sel = true;
                    if (((i % 8) < 4) && ((i / 8) >= 4) && (numLineOrRect == 2)) sel = true;
                    if (((i % 8) >= 4) && ((i / 8) < 4) && (numLineOrRect == 1)) sel = true;
                    if (((i % 8) >= 4) && ((i / 8) >= 4) && (numLineOrRect == 3)) sel = true;
                    if (sel) ar.Add(i);
                }
                warior.BonusRect = 0;
                DelTiles(ar);
                print($"bonus rect cntColBalls => {cntColBalls[0]} {cntColBalls[1]} {cntColBalls[2]} {cntColBalls[3]} {cntColBalls[4]} {cntColBalls[5]} {cntColBalls[6]} {cntColBalls[7]}");
                DownTiles();
                warior.StepsEffect();
                UseBalls();
                print("bonus rect CikleTest");
                CikleTest();
            }
        }
        if (modeSteps == 2)
        {   //  ����� ����� -> ���� ����
            //warior = enemyControl as IWarior;
            for(int i = 0; i < 8; i++)
            {
                if (cntColBalls[i] > 0)
                {
                    switch(i)
                    {
                        case 0: //  �������
                            GameManager.Instance.currentPlayer.countRed += cntColBalls[i];
                            player.BallsEffect(cntColBalls[i], 0, 0, out dmg);
                            warior.BallsDamage(cntColBalls[i], 0, 0);
                            heartControl.gameObject.SetActive(true); heartControl.MoveRL(); Invoke("HeartHide", 1f);
                            break;
                        case 1: //  ������
                            GameManager.Instance.currentPlayer.countGreen += cntColBalls[i];
                            player.BallsEffect(cntColBalls[i], 1, 0, out dmg);
                            warior.BallsDamage(dmg, 1, player.ToxinPercent);
                            toxinControl.gameObject.SetActive(true); toxinControl.MoveToRight(); Invoke("ToxinHide", 1f);
                            break;
                        case 2: //  �����
                            GameManager.Instance.currentPlayer.countYellow += cntColBalls[i];
                            player.BallsEffect(cntColBalls[i], 2, 0, out dmg);
                            break;
                        case 3: //  �����
                            GameManager.Instance.currentPlayer.countBlue += cntColBalls[i];
                            player.BallsEffect(cntColBalls[i], 3, 0, out dmg);
                            warior.BallsDamage(dmg, 3, player.ToxinPercent);
                            magicEffect.gameObject.SetActive(true); magicEffect.MoveToRight(); Invoke("MagicHide", 1f);
                            break;
                        case 4: //  ��������� (�������)
                            GameManager.Instance.currentPlayer.countCian += cntColBalls[i];
                            player.BallsEffect(cntColBalls[i], 4, 0, out dmg);
                            break;
                        case 5: //  �������
                            GameManager.Instance.currentPlayer.countMagenta += cntColBalls[i];
                            player.BallsEffect(cntColBalls[i], 5, 0, out dmg);
                            medicControl.gameObject.SetActive(true); medicControl.MoveToLeft(); Invoke("MedicHide", 1f);
                            break;
                        case 6: //  ����������
                            GameManager.Instance.currentPlayer.countBrown += cntColBalls[i];
                            player.BallsEffect(cntColBalls[i], 6, 0, out dmg);
                            break;
                        case 7: //  ���������
                            GameManager.Instance.currentPlayer.countOrange += cntColBalls[i];
                            player.BallsEffect(cntColBalls[i], 7, 0, out dmg);
                            warior.BallsDamage(dmg, 7, 0);   //  proba
                            fireEffect.gameObject.SetActive(true); fireEffect.MoveToRight(); Invoke("FireHide", 1f);
                            break;
                    }
                }
            }
        }
        //print($"cntColBalls => {cntColBalls[0]} {cntColBalls[1]} {cntColBalls[2]} {cntColBalls[3]} {cntColBalls[4]} {cntColBalls[5]} {cntColBalls[6]} {cntColBalls[7]}");
        warior.StepsEffect();
        player.StepsEffect();
        cntColBalls = null;
        /*if (modeSteps == 0)
        {
            if (warior.BonusLine > 0)
            {   //  ������� ��������� �����

            }
            if (warior.BonusRect > 0)
            {   //  ������� ��������� ������� 4�4

            }
        }*/
    }

    private void HeartHide()
    {
        heartControl.gameObject.SetActive(false);
    }

    private void MedicHide()
    {
        medicControl.gameObject.SetActive(false);
    }

    private void ToxinHide()
    {
        toxinControl.gameObject.SetActive(false);
    }

    private void MagicHide()
    {
        magicEffect.gameObject.SetActive(false);
    }

    private void FireHide()
    {
        fireEffect.gameObject.SetActive(false);
    }

    private List<int> TestTotalTiles()
    {
        List<int> ar = new List<int>();
        int i, j;
        for(i = 0; i < 8; i++)
        {
            for(j = 0; j < 8; j++)
            {
                List<int> tmp = TestTiles3p(8 * i + j);
                if (tmp.Count > 0)
                {                    
                    foreach(int n in tmp)
                    {
                        bool isNew = true;
                        foreach(int a in ar)
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

    private List<int> TestTiles3p(int num)
    {
        List<int> ar = new List<int>();
        int x = num % 8, y = num / 8, nc = pole64[num];
        //print($"TestTiles3p num={num} x={x} y={y} col={nc}");
        if (nc == -1) return ar;
        if (x < 6)
        {   //  ����� ��������������
            if ((pole64[num + 1] == nc) && (pole64[num + 2] == nc))
            {
                ar.Add(num);ar.Add(num + 1);ar.Add(num + 2);
                if ((x < 5) && (pole64[num + 3] == nc))
                {
                    ar.Add(num + 3); 
                    if ((x < 4) && (pole64[num + 4] == nc)) ar.Add(num + 4);
                }
            }
        }
        if (y < 6)
        {   //  ����� ������������
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
        {   //  �������
            if ((pole64[num + 1] == nc) && (pole64[num + 8] == nc) && (pole64[num + 9] == nc))
            {
                ar.Add(num); ar.Add(num + 1); ar.Add(num + 8); ar.Add(num + 9);
            }
        }
        return ar;
    }

    private List<int> TestTiles3(int num)
    {
        List<int> ar = new List<int>();
        int x = num % 8, y = num / 8, nc = pole64[num];
        //print($"TestTiles3 num={num} x={x} y={y} col={nc}");
        if (nc == -1) return ar;
        if ((x > 0) && (pole64[num - 1] == nc))
        {   //  ����� ��������
            if ((x > 1) && (pole64[num - 2] == nc))
            {   //  ����� ���
                ar.Add(num);ar.Add(num - 1);ar.Add(num - 2);
                if ((x < 7) && (pole64[num + 1] == nc))
                {
                    ar.Add(num + 1);
                    if ((x < 6) && (pole64[num + 2] == nc))
                    {
                        ar.Add(num + 2);
                        //if ((y > 0) && (pole64[num + 2 - 8] == nc)) ar.Add(num - 6);
                        //if ((y < 7) && (pole64[num + 2 + 8] == nc)) ar.Add(num + 10);
                    }
                    if ((y > 0) && (pole64[num + 1 - 8] == nc)) ar.Add(num - 7);
                    if ((y < 7) && (pole64[num + 1 + 8] == nc)) ar.Add(num + 9);
                }
            }
            if ((y > 0) && (pole64[num - 1 - 8] == nc))
            {   //  ����� � ����� ���� 
                ar.Add(num); ar.Add(num - 1); ar.Add(num - 9);
                if ((y < 7) && (pole64[num + 8] == nc))
                {   //  � ��� �����
                    ar.Add(num + 8);
                    if ((y < 6) && (pole64[num + 16] == nc)) ar.Add(num + 16);  //  � 2 ����� 
                }
            }
            if ((y < 7) && (pole64[num - 1 + 8] == nc))
            {   //  ����� � ����� ����� 
                ar.Add(num); ar.Add(num - 1); ar.Add(num + 7);
                if ((y > 0) && (pole64[num - 8] == nc))
                {   //  � ��� ����
                    ar.Add(num - 8);
                    if ((y > 1) && (pole64[num - 16] == nc)) ar.Add(num - 16);  //  � 2 ���� 
                }
            }
        }
        if ((x < 7) && (pole64[num + 1] == nc))
        {
            print("��� ������");
            if ((x < 6) && (pole64[num + 2] == nc))
            {
                print("��� ������");
                ar.Add(num); ar.Add(num + 1); ar.Add(num + 2);
                if ((x > 0) && (pole64[num - 1] == nc)) ar.Add(num - 1);    // 1 ����� � 2 ������
            }
            if ((x > 0) && (pole64[num - 1] == nc))
            {
                print("����� ������");
                ar.Add(num); ar.Add(num + 1); ar.Add(num - 1);
                if ((x > 1) && (pole64[num - 2] == nc)) ar.Add(num - 2);    // 2 ����� � 1 ������
            }
        }
        if ((y > 0) && (pole64[num - 8] == nc))
        {
            print("��� �� ��������� ����");
            if ((y > 1) && (pole64[num - 16] == nc))
            {
                print("��� �� ��������� ����");
                ar.Add(num); ar.Add(num - 8); ar.Add(num - 16);
            }
            if ((y < 7) && (pole64[num + 8] == nc))
            {
                print("����� ����");
                ar.Add(num); ar.Add(num + 8); ar.Add(num - 8);
                if ((y < 6) && (pole64[num + 16] == nc)) ar.Add(num + 16);    // 1 ���� � 2 �����
            }
        }
        if ((y < 7) && (pole64[num + 8] == nc))
        {
            print("��� �� ��������� �����");
            if ((y < 6) && (pole64[num + 16] == nc))
            {
                print("��� �� ��������� �����");
                ar.Add(num); ar.Add(num + 8); ar.Add(num + 16);
            }
            if ((y > 0) && (pole64[num - 8] == nc))
            {
                print("����� ����");
                ar.Add(num); ar.Add(num + 8); ar.Add(num - 8);
                if ((y > 1) && (pole64[num - 16] == nc)) ar.Add(num - 16);    // 2 ���� � 1 �����
            }
        }
        return ar;
    }

    private void DelTiles(List<int> ar)
    {
        int i, col;
        StringBuilder sb = new StringBuilder();
        for(i = 0; i < ar.Count; i++)
        {
            col = pole64[ar[i]];
            if (col >= 0 && col < 8) cntColBalls[col]++;
            sb.Append($"{ar[i]} ");
            pole64[ar[i]] = -1;
            if (poleGO[ar[i]] != null) poleGO[ar[i]].GetComponent<TailControl>().DeletingTail();
            poleGO[ar[i]] = null;
        }
        //print($"DelTiles   ar => {sb.ToString()}");
        //ar.Clear();
    }

    private void DownTiles()
    {
        int i, j;
        for(i = 0; i < 56; i++)
        {
            if (pole64[i] == -1)
            {
                for(j = i + 8; j < 64; j += 8)
                {
                    if (pole64[j] != -1)
                    {
                        /*Vector3 tg1;
                        tg1.x = (i % 8) - 3.5f; tg1.z = (i / 8) - 3.5f;
                        tg1.y = 0;
                        poleGO[j].GetComponent<TailControl>().SetTarget(tg1);*/
                        //poleGO[j].GetComponent<TailControl>().SetNewPosition(tg1);                        
                        //print($"Down Swap {i}({tg1}) <> {j}({poleGO[j].transform.position})");
                        //SwapTile(i, j);
                        pole64[i] = pole64[j];pole64[j] = -1;
                        poleGO[i] = poleGO[j];poleGO[j] = null;
                        break;
                    }
                }
            }
        }
        for(i = 0; i < 64; i++)
        {
            if (pole64[i] == -1)
            {
                GenerateTail(i, true);
                //print($"generate {i} => {pole64[i]} {poleGO[i].GetComponent<TailControl>().NumCol}");
            }
        }
    }

    private void MoveTiles()
    {
        for(int i = 0; i < 64; i++)
        {
            Vector3 tg1;
            tg1.x = (i % 8) - 3.5f; tg1.z = (i / 8) - 3.5f;
            tg1.y = 0;
            //poleGO[i].GetComponent<TailControl>().SetNewPosition(tg1);    //  ok
            poleGO[i].GetComponent<TailControl>().SetTarget(tg1);   //  no - ?
        }
    }

    public void SelectTile(GameObject go)
    {

    }

    public void TileMove(Vector3 pos)
    {

    }

    public void BoardOverPoint(int x, int y)
    {
        print($"BoardOverPoint x=>{x} y=>{y}");
        OffAllTile();numLineOrRect = -1;
        int i;
        if (bonusMode == 1)
        {
            for (i = 0; i < 64; i++)
            {
                poleGO[i].GetComponent<TailControl>().SetHint((i % 8) == x);
            }
            numLineOrRect = x;
        }
        if (bonusMode == 2)
        {
            for (i = 0; i < 64; i++)
            {
                poleGO[i].GetComponent<TailControl>().SetHint((i / 8) == y);
            }
            numLineOrRect = y;
        }
        if (bonusMode == 3)
        {
            bool sel;
            int tail = 0;
            if ((x < 4) && (y < 4)) tail = 0;
            if ((x < 4) && (y >= 4)) tail = 2;
            if ((x >= 4) && (y < 4)) tail = 1;
            if ((x >= 4) && (y >= 4)) tail = 3;
            numLineOrRect = tail;
            for (i = 0; i < 64; i++)
            {
                sel = false;
                if (((i % 8) < 4) && ((i / 8) < 4) && (tail == 0)) sel = true;
                if (((i % 8) < 4) && ((i / 8) >= 4) && (tail == 2)) sel = true;
                if (((i % 8) >= 4) && ((i / 8) < 4) && (tail == 1)) sel = true;
                if (((i % 8) >= 4) && ((i / 8) >= 4) && (tail == 3)) sel = true;
                poleGO[i].GetComponent<TailControl>().SetHint(sel);
            }
        }
    }

    public void OnClickLineV()
    {
        boardControl.SetIsOver(true);
        bonusMode = 2;
    }

    public void OnClickLineH()
    {
        boardControl.SetIsOver(true);
        bonusMode = 1;
    }

    public void OnClickRect()
    {
        boardControl.SetIsOver(true);
        bonusMode = 3;
    }
}

public class CountTileColors
{
    int numColor;
    int count;
    public CountTileColors() { }
    public CountTileColors(int col, int num) { numColor = col; count = num; }
    public int Count { get => count; }
    public int NumColor { get => numColor; }
    public void AddCount(int zn = 1) { if (zn > 0) count += zn; }
}
