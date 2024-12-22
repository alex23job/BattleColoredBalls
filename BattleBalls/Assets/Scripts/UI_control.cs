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


    // Start is called before the first frame update
    void Start()
    {
        
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
}
