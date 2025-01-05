using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour, IWarior
{
    [SerializeField] private UI_control ui_Control;
    [SerializeField] private LevelControl levelControl;

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

    public void ChangeHP(int zn)
    {
        int tmp = currentHP + zn;
        if (tmp > maxHP) currentHP = maxHP;
        else if (tmp < 0) currentHP = 0;
        else currentHP = tmp;
        ui_Control.ViewHp(CurrentHP, MaxHP, 2);
        if (currentHP == 0)
        {   //  ��� ���� !!!

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
    {   //  ����� ���������� ��������� ���������� � ������������ � ����������� ����������
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

    public void BallsEffect(int zn, int col, int prc)
    {
        switch(col)
        {
            case 0: //  �������
                break;
            case 1: //  ������
                break;
            case 2: //  �����
                break;
            case 3: //  �����
                break;
            case 4: //  ��������� (�������)
                break;
            case 5: //  �������
                break;
            case 6: //  ����������
                break;
            case 7: //  ���������
                ChangeHP(-zn);   //  proba
                break;
        }
    }
}
