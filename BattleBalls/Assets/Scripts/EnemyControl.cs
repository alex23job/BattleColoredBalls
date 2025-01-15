using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour, IWarior
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
    public int BonusLine { get; set; }
    public int BonusRect { get; set; }
    public int StepsToxin { get; set; }
    public int StepsFire { get; set; }

    private int TmpImmunity = 0; // 2 - огонь на 1 ход, 1 - магия на 1 ход

    private int Tmp2x = 0; //    1 - 2x магия, 2 - 2x огонь, 4 - 2x яд

    public bool StepBreak = false;

    public void ChangeHP(int zn)
    {
        int tmp = currentHP + zn;
        if (tmp > maxHP) currentHP = maxHP;
        else if (tmp < 0) currentHP = 0;
        else currentHP = tmp;
        ui_Control.ViewHp(CurrentHP, MaxHP, 2);
        if (currentHP == 0)
        {   //  бот убит !!!
            levelControl.BotKilled();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = 100;
        maxHP = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetParams(WariorParams wp)
    {   //  нужно установить параметры противника в соответствии с переданными значениями
        nameRu = wp.nameWarior;
        nameEn = wp.nameWariorEn;
        maxHP = wp.maxHP;
        immunity = wp.immun;
        toxinPercent = wp.toxinPrc;
        firePercent = wp.firePrc;
        levelBrown = wp.lvlBrown;
        levelCian = wp.lvlCian;
        levelHealing = wp.lvlHealing;
        levelMagic = wp.lvlMagic;
        levelSpell = wp.lvlSpell;
        currentHP = maxHP;
        ui_Control.ViewName(nameRu, 2);
    }

    public void BallsEffect(int zn, int col, int prc, out int znDmg)
    {
        int rndPrc = Random.Range(0, 101);
        int dmg = zn;
        znDmg = zn;
        switch(col)
        {
            case 0: //  красный
                //if (immunity != 1) 
                ChangeHP(zn);
                infoPanel.AddStepInfo(RndColorsControl.GetColor(0), $"+{zn}");
                break;
            case 1: //  зелёный
                    if ((Tmp2x & 0x04) != 0)
                    {
                        znDmg *= 2;Tmp2x &= 0x04;
                    }
                /*if (immunity != 2)
                {
                    //ChangeHP(-zn);
                    if (rndPrc <= prc) StepsToxin = 3;
                }*/
                break;
            case 2: //  жёлтый
                if (zn == 3) { StepsFire = 0; StepsToxin = 0; }
                if ((zn == 4) && (levelSpell == 2)) Tmp2x = 7;
                if (zn == 5) StepBreak = true;
                if (zn > 5)
                { StepsFire = 0; StepsToxin = 0; Tmp2x = 7; StepBreak = true; }
                break;
            case 3: //  синий
                if (levelMagic == 2 && zn == 4) znDmg = maxHP / 10;
                if (levelMagic == 3 && zn == 5) znDmg = maxHP / 4;
                if ((Tmp2x & 0x01) != 0)
                {
                    znDmg *= 2; Tmp2x ^= 0x01;
                }
                //if ((immunity != 3) || ((TmpImmunity & 1) != 0)) ChangeHP(-dmg);
                //TmpImmunity &= 0x02;
                break;
            case 4: //  бирюзовый (голубой)
                TmpImmunity |= 1;
                if ((levelCian == 2) && (zn == 4)) Tmp2x |= 0x02;
                if (levelCian == 3) { BonusLine++; ui_Control.ViewBonus(BonusLine, 3); }
                break;
            case 5: //  магента - лечение
                if (levelHealing == 2) dmg = maxHP / 10;
                if (levelHealing == 3) dmg = maxHP / 4;
                ChangeHP(dmg);
                infoPanel.AddStepInfo(RndColorsControl.GetColor(5), $"+{dmg}");
                break;
            case 6: //  коричневый
                TmpImmunity |= 2;
                if (levelBrown == 2 && zn == 4) Tmp2x |= 0x01;
                if (levelBrown == 3) { BonusRect++; ui_Control.ViewBonus(BonusRect, 4); }
                break;
            case 7: //  оранжевый
                if ((Tmp2x & 0x02) != 0)
                {
                    znDmg *= 2; Tmp2x ^= 0x02;
                }
               //if (immunity != 4 || ((TmpImmunity & 2) != 0))
               //{
                    //ChangeHP(-zn);   //  proba
                    //if (rndPrc <= prc) StepsFire = 3;
               //}
               //TmpImmunity &= 0x01;
               break;
        }
    }

    public void BallsDamage(int zn, int col, int prc)
    {
        int rndPrc = Random.Range(0, 101);
        int dmg = zn;
        switch (col)
        {
            case 0: //  красный
                if (immunity != 1)
                {
                    ChangeHP(-zn);
                    infoPanel.AddStepInfo(RndColorsControl.GetColor(0), $"-{zn}");
                }
                break;
            case 1: //  зелёный
                if (immunity != 2)
                {
                    ChangeHP(-zn);
                    if (rndPrc <= prc) StepsToxin = 3;
                    StepsToxin = 3; //  proba
                    string descr = $"-{zn}" + ((StepsToxin == 3) ? $"  *{StepsToxin}" : "");
                    infoPanel.AddStepInfo(RndColorsControl.GetColor(1), descr);
                }
                break;
            case 2: //  жёлтый
                /*if (zn == 3) { StepsFire = 0; StepsToxin = 0; }
                if ((zn == 4) && (levelSpell == 2)) Tmp2x = 7;
                if (zn == 5) StepBreak = true;*/
                break;
            case 3: //  синий
                /*if (levelMagic == 2 && zn == 4) dmg = maxHP / 10;
                if (levelMagic == 3 && zn == 5) dmg = maxHP / 4;
                if ((Tmp2x & 0x01) != 0)
                {
                    dmg *= 2; Tmp2x &= 0x01;
                }*/
                if ((immunity != 3) && ((TmpImmunity & 1) == 0))
                {
                    ChangeHP(-dmg);
                    infoPanel.AddStepInfo(RndColorsControl.GetColor(3), $"-{dmg}");
                }
                TmpImmunity &= 0x06;
                break;
            case 4: //  бирюзовый (голубой)
                /*TmpImmunity |= 1;
                if ((levelCian == 2) && (zn == 4)) Tmp2x |= 0x02;
                if (levelCian == 3) BonusLine++;*/
                break;
            case 5: //  магента - лечение
                /*if (levelHealing == 2) dmg = maxHP / 10;
                if (levelHealing == 3) dmg = maxHP / 4;
                ChangeHP(dmg);*/
                break;
            case 6: //  коричневый
                /*TmpImmunity |= 2;
                if (levelBrown == 2 && zn == 4) Tmp2x |= 0x01;
                if (levelBrown == 3) BonusRect++;*/
                break;
            case 7: //  оранжевый
                if ((immunity != 4) && ((TmpImmunity & 2) == 0))
                {
                    /*if ((Tmp2x & 0x02) != 0)
                    {
                        zn *= 2; Tmp2x &= 0x02;
                    }*/
                    ChangeHP(-zn);   //  proba
                    if (rndPrc <= prc) StepsFire = 3;
                    //StepsFire = 3;  //  proba
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
        if (BonusRect > 0)
        {   
            BonusRect = 0;
            ui_Control.ViewBonus(BonusRect, 4);
        }
        if (BonusLine > 0)
        {
            BonusLine = 0;
            ui_Control.ViewBonus(BonusLine, 3);
        }
        StepBreak = false;
        ui_Control.ViewImm((TmpImmunity & 0x01) != 0, 6);
        ui_Control.ViewImm((TmpImmunity & 0x02) != 0, 7);
        if (((TmpImmunity & 0x01) == 0) && (Immunity != 3)) ui_Control.ViewImm(false, 6);
        if (((TmpImmunity & 0x02) == 0) && (Immunity != 4)) ui_Control.ViewImm(false, 7);

        panel.ViewStepToxin(StepsToxin);
        panel.ViewStepFire(StepsFire);

        panel.View2xFire((Tmp2x & 0x02) != 0);
        panel.View2xMagic((Tmp2x & 0x01) != 0);
    }
}
