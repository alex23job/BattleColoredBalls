using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarior : MonoBehaviour, IWarior
{
    [SerializeField] private UI_control ui_Control;
    [SerializeField] private LevelControl levelControl;
    [SerializeField] private UI_Panel panel;
    [SerializeField] private InfoPanelControl infoPanel;

    private string nameRu;
    private string nameEn;
    private int currentHP;
    private int maxHP;
    private int immunity;
    private int toxinPercent;
    private int firePercent;
    private int levelSpell;
    private int levelMagic;
    private int levelHealing;
    private int levelCian;
    private int levelBrown;

    public int MaxHP { get => maxHP; set { maxHP = (value >= 0) ? value : 0; } }

    public int CurrentHP { get => currentHP; }

    public int ToxinPercent { get => toxinPercent; }
    public int FirePercent { get => firePercent; }

    public int LevelSpell { get => levelSpell; }
    public int LevelMagic { get => levelMagic; }
    public int LevelHealing { get => levelHealing; }
    public int LevelCian { get => levelCian; }
    public int LevelBrown { get => levelBrown; }

    public int Immunity { get => immunity; }

    public int StepsToxin { get; set; }
    public int StepsFire { get; set; }
    public int BonusLine { get; set; }
    public int BonusRect { get; set; }

    private int TmpImmunity = 0; // 2 - ����� �� 1 ���, 1 - ����� �� 1 ���

    private int Tmp2x = 0; //    1 - 2x �����, 2 - 2x �����, 4 - 2x ��

    public bool StepBreak = false;

    public void BallsEffect(int zn, int col, int prc, out int znDmg)
    {
        GameManager.Instance.currentPlayer.currentScore += zn;
        ui_Control.ViewScore(1, GameManager.Instance.currentPlayer.currentScore);
        int rndPrc = Random.Range(0, 101);
        int dmg = zn;
        znDmg = zn;
        switch (col)
        {
            case 0: //  �������
                ChangeHP(zn);
                infoPanel.AddStepInfo(RndColorsControl.GetColor(0), $"+{zn}");
                break;
            case 1: //  ������
                    if ((Tmp2x & 0x04) != 0)
                    {
                        znDmg *= 2; Tmp2x ^= 0x04;
                    }
                /*if (immunity != 2)
                {
                    ChangeHP(-zn);
                    if (rndPrc <= prc) StepsToxin = 3;
                }*/
                break;
            case 2: //  �����
                if (zn == 3) { StepsFire = 0; StepsToxin = 0; }
                if ((zn == 4) && (levelSpell == 2)) Tmp2x = 7;
                if (zn == 5) StepBreak = true;
                if (zn > 5)
                { StepsFire = 0; StepsToxin = 0; Tmp2x = 7; StepBreak = true; }
                break;
            case 3: //  �����
                if (levelMagic == 2 && zn == 4) znDmg = maxHP / 10;
                if (levelMagic == 3 && zn == 5) znDmg = maxHP / 4;
                if ((Tmp2x & 0x01) != 0)
                {
                    znDmg *= 2; Tmp2x ^= 0x01;
                }
                //if ((immunity != 3) || ((TmpImmunity & 1) != 0)) ChangeHP(-dmg);
                //TmpImmunity &= 0x02;
                break;
            case 4: //  ��������� (�������)
                if (zn >= 3) TmpImmunity |= 1;
                if ((levelCian == 2) && (zn == 4)) Tmp2x |= 0x02;
                if ((levelCian == 3) && (zn >= 5)) { BonusLine++; GameManager.Instance.currentPlayer.countBonusLine++; ui_Control.ViewBonus(BonusLine, 1); }
                break;
            case 5: //  ������� - �������
                if (levelHealing == 2) dmg = maxHP / 10;
                if (levelHealing == 3) dmg = maxHP / 4;
                ChangeHP(dmg);
                infoPanel.AddStepInfo(RndColorsControl.GetColor(5), $"+{dmg}");
                break;
            case 6: //  ����������
                if (zn >= 3) TmpImmunity |= 2;
                if (levelBrown == 2 && zn == 4) Tmp2x |= 0x01;
                if (levelBrown == 3 && zn >= 5) { BonusRect++; GameManager.Instance.currentPlayer.countBonusRect++; ui_Control.ViewBonus(BonusRect, 2); }
                break;
            case 7: //  ���������
                if ((Tmp2x & 0x02) != 0)
                {
                    znDmg *= 2; Tmp2x ^= 0x02;
                }
                break;
        }
        GameManager.Instance.currentPlayer.currentExp += (col != 5) ? znDmg : dmg;
    }

    public void ChangeHP(int zn)
    {
        int tmp = currentHP + zn;
        if (tmp > maxHP) currentHP = maxHP;
        else if (tmp < 0) currentHP = 0;
        else currentHP = tmp;
        ui_Control.ViewHp(CurrentHP, MaxHP, 1);
        if (currentHP == 0)
        {   //  ����� ���� !!!
            Invoke("PlayerKilled", 1f);            
        }
    }

    private void PlayerKilled()
    {
        levelControl.PlayerKilled();
    }

    // Start is called before the first frame update
    void Start()
    {
        //SetPlayerInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerInfo()
    {
        maxHP = 50 + GameManager.Instance.currentPlayer.countRed / 100;
        currentHP = maxHP;
        ui_Control.ViewHp(CurrentHP, MaxHP, 1);

        toxinPercent = GameManager.Instance.currentPlayer.countGreen / 50;
        if (toxinPercent > 100) toxinPercent = 100;

        firePercent = GameManager.Instance.currentPlayer.countOrange / 50;
        if (firePercent > 100) firePercent = 100;

        if (GameManager.Instance.currentPlayer.countYellow > 3000) levelSpell = 2;

        if (GameManager.Instance.currentPlayer.countBlue >= 2000) levelMagic = 2;
        if (GameManager.Instance.currentPlayer.countBlue >= 5000) levelMagic = 3;

        if (GameManager.Instance.currentPlayer.countCian >= 2000) levelCian = 2;
        if (GameManager.Instance.currentPlayer.countCian >= 5000) levelCian = 3;

        if (GameManager.Instance.currentPlayer.countMagenta >= 2000) levelHealing = 2;
        if (GameManager.Instance.currentPlayer.countMagenta >= 5000) levelHealing = 3;

        if (GameManager.Instance.currentPlayer.countBrown >= 2000) levelBrown = 2;
        if (GameManager.Instance.currentPlayer.countBrown >= 5000) levelBrown = 3;

        ui_Control.ViewBonus(GameManager.Instance.currentPlayer.countBonusLine, 1);
        ui_Control.ViewBonus(GameManager.Instance.currentPlayer.countBonusRect, 2);

        immunity = GameManager.Instance.currentPlayer.immunity;
        //print($"immunity => {immunity}");
        if (immunity > 0) ui_Control.ViewImm(true, immunity - 1);
    }

    public void BallsDamage(int zn, int col, int prc)
    {
        int rndPrc = Random.Range(0, 101);
        int dmg = zn;
        switch (col)
        {
            case 0: //  �������
                if (immunity != 1) ChangeHP(-zn);
                infoPanel.AddStepInfo(RndColorsControl.GetColor(0), $"-{zn}");
                break;
            case 1: //  ������
                if (immunity != 2)
                {
                    ChangeHP(-zn);
                    if (rndPrc <= prc) StepsToxin = 3;
                    string descr = $"-{zn}" + ((StepsToxin == 3) ? $"  *{StepsToxin}" : "");
                    infoPanel.AddStepInfo(RndColorsControl.GetColor(1), descr);
                }
                break;
            case 2: //  �����
                break;
            case 3: //  �����
                if ((immunity != 3) && ((TmpImmunity & 1) == 0)) ChangeHP(-dmg);
                TmpImmunity &= 0x02;
                infoPanel.AddStepInfo(RndColorsControl.GetColor(3), $"-{dmg}");
                break;
            case 4: //  ��������� (�������)
                break;
            case 5: //  ������� - �������
                break;
            case 6: //  ����������
                break;
            case 7: //  ���������
                if ((immunity != 4) && ((TmpImmunity & 2) == 0))
                {
                    ChangeHP(-zn);   //  proba
                    if (rndPrc <= prc) StepsFire = 3;
                    string descr = $"-{zn}" + ((StepsFire == 3) ? $"  *{StepsFire}" : "");
                    infoPanel.AddStepInfo(RndColorsControl.GetColor(7), descr);
                }
                TmpImmunity &= 0x01;
                break;
        }
    }

    public void StepsEffect()
    {
        if (StepsFire > 0)
        {
            ChangeHP(-StepsFire);
            StepsFire--;
        }
        if (StepsToxin > 0)
        {
            ChangeHP(-StepsToxin);
            StepsToxin--;
        }
        //TmpImmunity = 0;
        StepBreak = false;
        ui_Control.ViewImm((TmpImmunity & 0x01) != 0, 2);
        ui_Control.ViewImm((TmpImmunity & 0x02) != 0, 3);
        if (((TmpImmunity & 0x01) == 0) && (Immunity != 3)) ui_Control.ViewImm(false, 2);
        if (((TmpImmunity & 0x02) == 0) && (Immunity != 4)) ui_Control.ViewImm(false, 3);

        panel.ViewStepToxin(StepsToxin);
        panel.ViewStepFire(StepsFire);

        panel.View2xFire((Tmp2x & 0x02) != 0);
        panel.View2xMagic((Tmp2x & 0x01) != 0);
    }
}
