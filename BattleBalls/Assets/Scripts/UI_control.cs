using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_control : MonoBehaviour
{
    [SerializeField] private Image ballPlayerLeft;
    [SerializeField] private Image ballPlayerRight;
    [SerializeField] private Image ballBotLeft;
    [SerializeField] private Image ballBotRight;
    [SerializeField] private Image crossPlayerLeft;
    [SerializeField] private Image crossPlayerRight;
    [SerializeField] private Image crossBotLeft;
    [SerializeField] private Image crossBotRight;
    [SerializeField] private Image playerHP;
    [SerializeField] private Image botHP;
    
    [SerializeField] private Text txtPlName;
    [SerializeField] private Text txtBotName;
    [SerializeField] private Text txtPlHp;
    [SerializeField] private Text txtBotHp;

    [SerializeField] private Text txtBotLine;
    [SerializeField] private Text txtBotRect;
    [SerializeField] private Text txtPlLine;
    [SerializeField] private Text txtPlRect;
    [SerializeField] private Button[] arBonusBtn;

    [SerializeField] private Sprite sprYes;
    [SerializeField] private Sprite sprNo;
    [SerializeField] private Image[] arImgImmunity;

    // Start is called before the first frame update
    void Start()
    {
        int i;
        for (i = 0; i < 8; i++) arImgImmunity[i].sprite = sprNo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewBalls(int mode, Color col)
    {
        switch(mode)
        {
            case 0:
                ballPlayerLeft.color = col;
                break;
            case 1:
                ballPlayerRight.color = col;
                break;
            case 2:
                ballBotLeft.color = col;
                break;
            case 3:
                ballBotRight.color = col;
                break;
        }
    }

    public void ViewCross(int mode, bool zn)
    {
        switch(mode)
        {
            case 0:
                crossPlayerLeft.gameObject.SetActive(zn);
                break;
            case 1:
                crossPlayerRight.gameObject.SetActive(zn);
                break;
            case 2:
                crossBotLeft.gameObject.SetActive(zn);
                break;
            case 3:
                crossBotRight.gameObject.SetActive(zn);
                break;
        }
    }

    /// <summary>
    /// Отображение здоровья игрока или бота
    /// </summary>
    /// <param name="hp">Текущее значение здоровья</param>
    /// <param name="maxHP">Ьаксимальное значение здоровья</param>
    /// <param name="mode">Для кого отображаем : 1 - player, 2 - bot</param>
    public void ViewHp(int hp, int maxHP, int mode)
    {
        if (mode == 1)  //  player
        {
            playerHP.fillAmount = (float)hp / (float)maxHP;
            txtPlHp.text = hp.ToString();
        }
        if (mode == 2)  //  bot
        {
            botHP.fillAmount = (float)hp / (float)maxHP;
            txtBotHp.text = hp.ToString();
        }
    }

    /// <summary>
    /// Отображение имени игрока или бота
    /// </summary>
    /// <param name="nm">строка с именем</param>
    /// <param name="mode">Для кого отображаем : 1 - player, 2 - bot</param>
    public void ViewName(string nm, int mode)
    {
        if (mode == 1)  //  player
        {
            txtPlName.text = nm;
        }
        if (mode == 2)  //  bot
        {
            txtBotName.text = nm;
        }
    }

    /// <summary>
    /// Отображение количества бонусов игрока или бота
    /// </summary>
    /// <param name="zn">количество бонусов</param>
    /// <param name="mode">Где выводим: 1 - line Pl, 2 - rect Pl, 3 - line Bot, 4 - rect Bot</param>
    public void ViewBonus(int zn, int mode)
    {
        switch(mode)
        {
            case 1:
                txtPlLine.text = zn.ToString();
                arBonusBtn[0].interactable = zn > 0;
                arBonusBtn[1].interactable = zn > 0;
                break;
            case 2:
                txtPlRect.text = zn.ToString();
                arBonusBtn[2].interactable = zn > 0;
                break;
            case 3:
                txtBotLine.text = zn.ToString();
                arBonusBtn[3].interactable = zn > 0;
                arBonusBtn[4].interactable = zn > 0;
                break;
            case 4:
                txtBotRect.text = zn.ToString();
                arBonusBtn[5].interactable = zn > 0;
                break;
        }
    }

    public void ViewExp(int zn)
    {

    }

    /// <summary>
    /// Отображение иммунитетов игрока и бота
    /// </summary>
    /// <param name="zn">есть или нет</param>
    /// <param name="mode">Отчего у кого: Pl 0 - r, 1 - g, 2 - b, 3 - or, Bot 4 - r, 5 - g, 6 - b, 7 - or</param>
    public void ViewImm(bool zn, int mode)
    {
        if ((mode >= 0) && (mode < 8)) arImgImmunity[mode].sprite = (zn == true) ? sprYes : sprNo;
    }
}
