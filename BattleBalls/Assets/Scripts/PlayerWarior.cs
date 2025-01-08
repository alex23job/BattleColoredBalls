using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarior : MonoBehaviour, IWarior
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

    public int StepsToxin { get; set; }
    public int StepsFire { get; set; }
    public int BonusLine { get; set; }
    public int BonusRect { get; set; }

    public int TmpImmunity = 0; // 2 - огонь на 1 ход, 1 - магия на 1 ход

    public int Tmp2x = 0; //    1 - 2x магия, 2 - 2x огонь

    public bool StepBreak = false;

    public void BallsEffect(int zn, int col, int prc)
    {
        throw new System.NotImplementedException();
    }

    public void ChangeHP(int zn)
    {
        int tmp = currentHP + zn;
        if (tmp > maxHP) currentHP = maxHP;
        else if (tmp < 0) currentHP = 0;
        else currentHP = tmp;
        ui_Control.ViewHp(CurrentHP, MaxHP, 2);
        if (currentHP == 0)
        {   //  игрок убит !!!

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerInfo()
    {
        maxHP = 100 + GameManager.Instance.currentPlayer.countRed / 100;

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
    }
}
