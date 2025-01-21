using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;

    [SerializeField] private GameObject[] colorItems;
    [SerializeField] private GameObject[] recordItems;

    [SerializeField] private Text txtGold;
    [SerializeField] private Text txtExp;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.currentPlayer.isLoad == true)
        {
            ViewGoldExp();
            ViewColors();
        }
        if (GameManager.Instance.currentPlayer.isAvatar == true)
        {
            //ViewColors();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLevelBtnClick(int n)
    {
        GameManager.Instance.currentPlayer.currentLevel = n;
        GameManager.Instance.LoadBattleScene();
    }

    public void InterableLevelButtons(int maxLevel)
    {
        for(int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = (i + 1) <= maxLevel;
        }
    }

    public void ViewGoldExp()
    {
        txtGold.text = GameManager.Instance.currentPlayer.totalGold.ToString();
        txtExp.text = GameManager.Instance.currentPlayer.totalScore.ToString();
    }

    public void ViewColors()
    {
        string lang = Language.Instance.CurrentLanguage;
        string sH = lang == "ru" ? "Здоровье" : "MaxHP";
        int red = GameManager.Instance.currentPlayer.countRed;
        colorItems[0].transform.GetChild(1).GetComponent<Text>().text = $"{red}  {sH} {50 + red / 100}";
        string sT = lang == "ru" ? "Шанс отравить" : "A chance to poison";

        int green = GameManager.Instance.currentPlayer.countGreen;
        colorItems[1].transform.GetChild(1).GetComponent<Text>().text = $"{green}  {sT} {(green / 50) % 100}%";

        string sY = lang == "ru" ? "Отмена яда и огня" : "Del of poison and fire";
        int yellow = GameManager.Instance.currentPlayer.countGreen;
        if (yellow > 3000) sY += lang == "ru" ? "; 2x урон" : "; 2x damage";
        //if (yellow > 0) sY += lang == "ru" ? "; 2x урон" : "; 2x damage";
        colorItems[2].transform.GetChild(1).GetComponent<Text>().text = $"{yellow}  {sY}";

        string sB = lang == "ru" ? "Урон магией" : "Magic damage";
        string sDmg = "1x";
        int blue = GameManager.Instance.currentPlayer.countBlue;
        if (blue >= 2000) sDmg = $"; {(50 + red / 100) / 10}";
        if (blue >= 5000) sDmg = $"; {(50 + red / 100) / 4}";
        colorItems[3].transform.GetChild(1).GetComponent<Text>().text = $"{blue}  {sB} {sDmg}";

        string sC = lang == "ru" ? "Защита от магии 1 ход" : "Magic Protection 1 turn";
        sDmg = "";
        int cian = GameManager.Instance.currentPlayer.countCian;
        if (cian >= 2000) sDmg = lang == "ru" ? "; 2x урон магией" : "; 2x magic damage";
        if (cian >= 5000) sDmg += lang == "ru" ? "; Бонус - удаление линии" : "; Bonus - line removal";
        //if (cian >= 0) sDmg = lang == "ru" ? "; 2x урон магией" : "; 2x magic damage";
        //if (cian >= 0) sDmg += lang == "ru" ? "; Бонус - удаление линии" : "; Bonus - line removal";
        colorItems[4].transform.GetChild(1).GetComponent<Text>().text = $"{cian}  {sC} {sDmg}";

        string sM = lang == "ru" ? "Исцеление" : "Healing";
        sDmg = "1x";
        int magenta = GameManager.Instance.currentPlayer.countMagenta;
        if (magenta >= 2000) sDmg = $"; {(50 + red / 100) / 10}";
        if (magenta >= 5000) sDmg = $"; {(50 + red / 100) / 4}";
        colorItems[5].transform.GetChild(1).GetComponent<Text>().text = $"{magenta}  {sM} {sDmg}";

        string sBr = lang == "ru" ? "Защита от огня 1 ход" : "Fire Protection 1 turn";
        sDmg = "";
        int brown = GameManager.Instance.currentPlayer.countBrown;
        if (brown >= 2000) sDmg = lang == "ru" ? "; 2x урон огнём" : "; 2x fire damage";
        if (brown >= 5000) sDmg += lang == "ru" ? "; Бонус - удаление квадрата" : "; Bonus - rectangle removal";
        //if (brown >= 0) sDmg = lang == "ru" ? "; 2x урон огнём" : "; 2x fire damage";
        //if (brown >= 0) sDmg += lang == "ru" ? "; Бонус - удаление квадрата" : "; Bonus - rectangle removal";
        colorItems[6].transform.GetChild(1).GetComponent<Text>().text = $"{brown}  {sBr} {sDmg}";

        string sO = lang == "ru" ? "Шанс поджечь" : "A chance to set fire to";
        int orang = GameManager.Instance.currentPlayer.countOrange;
        colorItems[7].transform.GetChild(1).GetComponent<Text>().text = $"{orang}  {sO} {(orang / 50) % 100}%";
    }
}
