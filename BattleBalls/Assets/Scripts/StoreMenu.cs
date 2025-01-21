using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreMenu : MonoBehaviour
{
    [SerializeField] private Text txtGold;

    // Start is called before the first frame update
    void Start()
    {
        ViewGold();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ViewGold()
    {
        txtGold.text = GameManager.Instance.currentPlayer.totalGold.ToString();
    }
}
