using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Panel : MonoBehaviour
{
    [SerializeField] private Text txtStepToxin;
    [SerializeField] private Text txtStepFire;
    [SerializeField] private Image imgStepToxinFone;
    [SerializeField] private Image imgStepFireFone;
    [SerializeField] private Image img2xMagicFone;
    [SerializeField] private Image img2xFireFone;

    private void Start()
    {
        View2xFire(false);
        View2xMagic(false);
        ViewStepFire(0);
        ViewStepToxin(0);
    }

    public void View2xFire(bool zn)
    {
        img2xFireFone.gameObject.SetActive(zn);
    }

    public void View2xMagic(bool zn)
    {
        img2xMagicFone.gameObject.SetActive(zn);
    }

    public void ViewStepToxin(int zn)
    {
        imgStepToxinFone.gameObject.SetActive(zn > 0);
        txtStepToxin.text = zn.ToString();
    }

    public void ViewStepFire(int zn)
    {
        imgStepFireFone.gameObject.SetActive(zn > 0);
        txtStepFire.text = zn.ToString();
    }
}
