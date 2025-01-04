using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour, IWarior
{
    [SerializeField] private UI_control ui_Control;
    [SerializeField] private LevelControl levelControl;

    private int currentHP;
    private int maxHP;
    public int MaxHP { get => maxHP; set { maxHP = (value >= 0) ? value : 0; } }
    public int CurrentHP { get => currentHP; }

    public int ToxinPercent => throw new System.NotImplementedException();

    public int FirePercent => throw new System.NotImplementedException();

    public int LevelSpell => throw new System.NotImplementedException();

    public int LevelMagic => throw new System.NotImplementedException();

    public int LevelHealing => throw new System.NotImplementedException();

    public int LevelCian => throw new System.NotImplementedException();

    public int LevelBrown => throw new System.NotImplementedException();

    public void ChangeHP(int zn)
    {
        int tmp = currentHP + zn;
        if (tmp > maxHP) currentHP = maxHP;
        else if (tmp < 0) currentHP = 0;
        else currentHP = tmp;
        ui_Control.ViewHp(CurrentHP, MaxHP, 2);
        if (currentHP == 0)
        {   //  бот убит !!!

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

    public void SetParams()
    {   //  нужно установить параметры противника в соответствии с переданными значениями

    }

    public void BallsEffect(int zn, int col, int prc)
    {
        switch(col)
        {
            case 0: //  красный
                break;
            case 1: //  зелёный
                break;
            case 2: //  жёлтый
                break;
            case 3: //  синий
                break;
            case 4: //  бирюзовый (голубой)
                break;
            case 5: //  магента
                break;
            case 6: //  коричневый
                break;
            case 7: //  оранжевый
                ChangeHP(-zn);   //  proba
                break;
        }
    }
}
