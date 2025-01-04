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

    public void SetParams()
    {   //  ����� ���������� ��������� ���������� � ������������ � ����������� ����������

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
