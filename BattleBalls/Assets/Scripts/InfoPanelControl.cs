using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelControl : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
     struct StepInfo
    {
        public Color col;
        public string descr;
        public StepInfo(Color c, string s) { col = c; descr = s; }
    }

    private List<StepInfo> infos = new List<StepInfo>();

    private void Start()
    {
        ViewItems();
    }
    public void AddStepInfo(Color col, string descr)
    {
        if (infos.Count > 4) infos.RemoveAt(0);
        infos.Add(new StepInfo(col, descr));
        ViewItems();
    }

    public void ClearHistory()
    {
        infos.Clear();
        ViewItems();
    }

    private void ViewItems()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < infos.Count)
            {
                panels[i].SetActive(true);
                panels[i].transform.GetChild(0).GetComponent<Text>().text = infos[i].descr;
                panels[i].transform.GetChild(1).GetComponent<Image>().color = infos[i].col;
            }
            else
            {
                panels[i].SetActive(false);
            }
        }
    }
}
