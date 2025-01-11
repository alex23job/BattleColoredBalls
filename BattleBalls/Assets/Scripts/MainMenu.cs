using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;

    [SerializeField] private Text txtGold;
    // Start is called before the first frame update
    void Start()
    {
        
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

    public void ViewGold()
    {
        txtGold.text = GameManager.Instance.currentPlayer.totalGold.ToString();
    }
}
