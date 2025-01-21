using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreMenu : MonoBehaviour
{
    [SerializeField] private Text txtGold;
    [SerializeField] private Sprite[] arSprites;
    [SerializeField] private Button[] arBonusBTN;
    [SerializeField] private Button[] arImmunBTN;
    [SerializeField] private Text txtBonusLine;
    [SerializeField] private Text txtBonusRect;
    [SerializeField] private Image imgImmunity;
    [SerializeField] private GameObject notPanel;

    // Start is called before the first frame update
    void Start()
    {
        ViewGold();
        ViewBonusLine();
        ViewBonusRect();
        ViewImmunity();
        if (GameManager.Instance.currentPlayer.countBonusLine > 0) InteractableBonusBTN(false, 1);
        if (GameManager.Instance.currentPlayer.countBonusRect > 0) InteractableBonusBTN(false, 2);
        if (GameManager.Instance.currentPlayer.totalGold < 500) InteractableBonusBTN(false, 3);
        if (GameManager.Instance.currentPlayer.immunity > 0) InteractableImmunBTN(false);
        else if (GameManager.Instance.currentPlayer.totalGold < 1000) InteractableImmunBTN(false, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ViewGold()
    {
        txtGold.text = GameManager.Instance.currentPlayer.totalGold.ToString();
    }

    private void ViewBonusLine()
    {
        txtBonusLine.text = GameManager.Instance.currentPlayer.countBonusLine.ToString();
    }

    private void ViewBonusRect()
    {
        txtBonusRect.text = GameManager.Instance.currentPlayer.countBonusRect.ToString();
    }

    private void ViewImmunity()
    {
        imgImmunity.sprite = arSprites[GameManager.Instance.currentPlayer.immunity];
    }

    public void OnClickBonusCash(int n)
    {
        if (GameManager.Instance.currentPlayer.totalGold < 500)
        {
            notPanel.SetActive(true);
            return;
        }
        RewardedComplete(n);
        InteractableBonusBTN(false, n);
    }

    public void OnClickBonusAds(int n)
    {
        RewardedComplete(n);
        InteractableBonusBTN(false, n);
    }

    public void OnClickImmunCash(int n)
    {
        if (GameManager.Instance.currentPlayer.totalGold < 500)
        {
            notPanel.SetActive(true);
            return;
        }
        RewardedComplete(n);
        InteractableImmunBTN(false);
    }

    public void OnClickImmunAds(int n)
    {
        RewardedComplete(n);
        InteractableImmunBTN(false);
    }

    public void RewardedComplete(int n)
    {
        switch(n)
        {
            case 1:
                GameManager.Instance.currentPlayer.countBonusLine++;
                ViewBonusLine();
                break;
            case 2:
                GameManager.Instance.currentPlayer.countBonusRect++;
                ViewBonusRect();
                break;
            case 3:
                GameManager.Instance.currentPlayer.immunity = 1;
                ViewImmunity();
                break;
            case 4:
                GameManager.Instance.currentPlayer.immunity = 2;
                ViewImmunity();
                break;
            case 5:
                GameManager.Instance.currentPlayer.immunity = 3;
                ViewImmunity();
                break;
            case 6:
                GameManager.Instance.currentPlayer.immunity = 4;
                ViewImmunity();
                break;
        }
    }

    private void InteractableBonusBTN(bool zn_set, int mode = 0)
    {
        if (mode == 3)
        {
            arBonusBTN[0].interactable = false;
            arBonusBTN[2].interactable = false;
            return;
        }
        int i, start = 0, end = arBonusBTN.Length;
        if (mode == 1) end = 2;
        if (mode == 2) start = 2;
        for (i = start; i < end; i++) arBonusBTN[i].interactable = zn_set;
    }
    private void InteractableImmunBTN(bool zn_set, int mode = 0)
    {
        int i;
        if (mode == 0) for (i = 0; i < arImmunBTN.Length; i++) arImmunBTN[i].interactable = zn_set;
        if (mode == 1) for (i = 0; i < arImmunBTN.Length; i += 2) arImmunBTN[i].interactable = zn_set;
    }
}
