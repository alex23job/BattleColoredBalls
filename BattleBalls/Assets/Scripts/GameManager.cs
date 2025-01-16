//using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void LoadYandex();

    [DllImport("__Internal")]
    private static extern void SaveYandex(string strJson);

    [DllImport("__Internal")]
    private static extern void SetToLeaderboard(int value);


    public PlayerInfo currentPlayer = new PlayerInfo();
    [SerializeField] private MainMenu mm_control;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Invoke("LoadGame", 0.08f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void LoadMapScene()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void FinishLevel()
    {
        currentPlayer.LevelComplete();
        SaveGame();
        //currentPlayer.currentLive = 3;
    }

    void LoadGame()
    {
#if UNITY_WEBGL
        LoadYandex();
#endif
#if UNITY_EDITOR
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            Debug.Log(Application.persistentDataPath + "/MySaveData.dat");
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            //Debug.Log(data.ToString());
            UpdateLoadingData(data);
        }
        else
        {
            Debug.Log("There is no save data!");
            GameManager.Instance.currentPlayer = PlayerInfo.FirstGame();
            if (mm_control != null)
            {
                mm_control.InterableLevelButtons(GameManager.Instance.currentPlayer.maxLevel);
                mm_control.ViewGold();
                mm_control.ViewColors();

                //mm_control.ViewScore();
                //mm_control.UpdateAudioSource();
            }
        }
#endif
    }

    public void UpdateLoadingData(SaveData data)
    {
        GameManager.Instance.currentPlayer.maxLevel = data.maxLvl;
        GameManager.Instance.currentPlayer.currentMode = data.oldMode;
        GameManager.Instance.currentPlayer.totalScore = data.score;
        GameManager.Instance.currentPlayer.totalGold = data.gold;
        GameManager.Instance.currentPlayer.countBonusLine = data.bonusLine;
        GameManager.Instance.currentPlayer.countBonusRect = data.bonusRect;

        GameManager.Instance.currentPlayer.countRed = data.cntRed;
        GameManager.Instance.currentPlayer.countGreen = data.cntGreen;
        GameManager.Instance.currentPlayer.countYellow = data.cntYellow;
        GameManager.Instance.currentPlayer.countBlue = data.cntBlue;
        GameManager.Instance.currentPlayer.countCian = data.cntCian;
        GameManager.Instance.currentPlayer.countMagenta = data.cntMagenta;
        GameManager.Instance.currentPlayer.countBrown = data.cntBrown;
        GameManager.Instance.currentPlayer.countOrange = data.cntOrange;

        GameManager.Instance.currentPlayer.isHintView = data.isHints;
        GameManager.Instance.currentPlayer.isSoundFone = data.isFone;
        GameManager.Instance.currentPlayer.isSoundEffects = data.isEffects;
        GameManager.Instance.currentPlayer.volumeFone = data.volFone;
        GameManager.Instance.currentPlayer.volumeEffects = data.volEffects;


        //Debug.Log("Game data loaded! Score=" + GameManager.Instance.currentPlayer.totalScore.ToString() + "  Gold=" + GameManager.Instance.currentPlayer.totalGold.ToString());
        Debug.Log($"Game data loaded! Score={GameManager.Instance.currentPlayer.totalScore}  MaxLevel={GameManager.Instance.currentPlayer.maxLevel}   Mode={GameManager.Instance.currentPlayer.currentMode}");

        GameManager.Instance.currentPlayer.isLoad = true;

        if (mm_control != null)
        {
            mm_control.InterableLevelButtons(GameManager.Instance.currentPlayer.maxLevel);
            mm_control.ViewGold();
            mm_control.ViewColors();

            //mm_control.ViewScore();
            //mm_control.UpdateAudioSource();
        }
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/MySaveData.dat");
        SaveData data = new SaveData();

        data.score = GameManager.Instance.currentPlayer.totalScore;
        data.maxLvl = GameManager.Instance.currentPlayer.maxLevel;
        data.gold = GameManager.Instance.currentPlayer.totalGold;
        data.bonusLine = GameManager.Instance.currentPlayer.countBonusLine;
        data.bonusRect = GameManager.Instance.currentPlayer.countBonusRect;

        data.cntRed = GameManager.Instance.currentPlayer.countRed;
        data.cntGreen = GameManager.Instance.currentPlayer.countGreen;
        data.cntYellow = GameManager.Instance.currentPlayer.countYellow;
        data.cntBlue = GameManager.Instance.currentPlayer.countBlue;
        data.cntCian = GameManager.Instance.currentPlayer.countCian;
        data.cntMagenta = GameManager.Instance.currentPlayer.countMagenta;
        data.cntBrown = GameManager.Instance.currentPlayer.countBrown;
        data.cntOrange = GameManager.Instance.currentPlayer.countOrange;


        data.oldMode = GameManager.Instance.currentPlayer.currentMode;

        data.isHints = GameManager.Instance.currentPlayer.isHintView;
        data.isFone = GameManager.Instance.currentPlayer.isSoundFone;
        data.isEffects = GameManager.Instance.currentPlayer.isSoundEffects;
        data.volFone = GameManager.Instance.currentPlayer.volumeFone;
        data.volEffects = GameManager.Instance.currentPlayer.volumeEffects;

        //DateTime dt = DateTime.Now;
        //data.timeString = $"{dt.Year:0000}-{dt.Month:00}-{dt.Day:00}-{dt.Hour:00}";

#if UNITY_WEBGL
        string jsonStr = JsonUtility.ToJson(data);
        SaveYandex(jsonStr);
        SetToLeaderboard(GameManager.Instance.currentPlayer.totalScore);
#endif

        //PlayerInfo.Instance.Save(jsonStr);
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }
}

public class PlayerInfo
{
    public bool isLoad = false;
    public bool isAvatar = false;

    public int totalScore = 0;  //  exp
    public int currentExp = 0;

    public int currentMode = 0;
    public int totalGold = 0;
    public int currentScore = 0;

    public int currentLevel = 1;
    public int maxLevel = 1;

    public int countRed = 0;
    public int countGreen = 0;
    public int countYellow = 0;
    public int countBlue = 0;
    public int countCian = 0;
    public int countMagenta = 0;
    public int countBrown = 0;
    public int countOrange = 0;

    public int countBonusLine = 0;
    public int countBonusRect = 0;


    /*public int currentPole = 0;

    public int countSecond = 0;
    public int countLine = 0;
    public int countFigure = 0;
    public int countHardFigure = 0;
    public bool isSurprise = false;

    public int maxQwScore = 0;
    public int maxHexScore = 0;
    public int maxPrismScore = 0;*/

    /*    public int contructions;
        public int decors;
        public int company;

        public int maxLevel;
        public int currentLevel;
        public int currentLive = 3;
        public int currentGold = 0;
        public int currentHint = 0;

        public int totalGold = 0;

        public int maxHP = 100;
        public int currentHP = 100;
        public int maxMagic = 10;
        public int currentMagic = 0;
        public int maxFire = 10;
        public int currentFire = 0;
        public int maxToxin = 10;
        public int currentToxin = 0;
    */

    public bool isHintView = true;
    public bool isSoundFone = true;
    public bool isSoundEffects = true;
    public int volumeFone = 50;
    public int volumeEffects = 100;

    public string playerName = "-------";
    public Texture photo = null;


    public PlayerInfo()
    {
        //maxLevel = 0;
        //currentLevel = 0;
    }

    public static PlayerInfo FirstGame()
    {
        return new PlayerInfo();
    }

    public void LevelComplete()
    {
        //UpdateReward(currentLevel);
        currentLevel++;
        if (currentLevel > maxLevel)
        {
            maxLevel = currentLevel;
        }
        totalScore += currentExp;
        totalGold += currentScore;
    }

    public void LevelLoss()
    {
        totalScore += currentExp;
        totalGold += currentScore;
    }

    /*    public void UpdateReward(int numLevel)
        {
            LevelInfo lev = LevelLogic.arrLevels[numLevel];
            switch (lev.rw.type)
            {
                case 0:
                    maxHP += lev.rw.value;
                    break;
                case 1:
                    maxFire += lev.rw.value;
                    break;
                case 2:
                    maxMagic += lev.rw.value;
                    break;
                case 3:
                    maxToxin += lev.rw.value;
                    break;
            }
            Debug.Log($"UpdateReward : maxHP={maxHP} maxFire={maxFire} maxMagic={maxMagic} maxToxin={maxToxin} reward: << {lev.rw.ToString()} >>  lev={lev}");
        }*/

    public void ClearCurrentParam()
    {
        currentScore = 0;
        currentExp = 0;
        /*currentGold = 0;
        currentHP = maxHP;
        currentMagic = 0;
        currentFire = 0;
        currentToxin = 0;*/
    }

    public void AddBalls(int zn, int numCol)
    {
        switch(numCol)
        {
            case 0:
                countRed += zn;
                break;
            case 1:
                countGreen += zn;
                break;
            case 2:
                countYellow += zn;
                break;
            case 3:
                countBlue += zn;
                break;
            case 4:
                countCian += zn;
                break;
            case 5:
                countMagenta += zn;
                break;
            case 6:
                countBrown += zn;
                break;
            case 7:
                countOrange += zn;
                break;
        }
    }

    public void AddBonus(int bonusID)
    {
        switch (bonusID)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
}

[Serializable]
public class SaveData
{
    public int maxLvl = 1;
    public int score = 0;
    public int gold = 0;
    public int bonusLine = 0;
    public int bonusRect = 0;

    public int cntRed = 0;
    public int cntGreen = 0;
    public int cntYellow = 0;
    public int cntBlue = 0;
    public int cntCian = 0;
    public int cntMagenta = 0;
    public int cntBrown = 0;
    public int cntOrange = 0;

    public int oldMode;

    /*public int oldPole;
    public int score;
    public int qwMaxScore;
    public int hexMaxScore;
    public int prismMaxScore;*/

    public bool isFone;
    public bool isEffects;
    public bool isHints;
    public int volFone;
    public int volEffects;
    public override string ToString()
    {
        return "SaveData: maxLvl=" + maxLvl + " score=" + score;
    }
}

public class SimpleStat
{
    public string NameStat = "";
    private int count = 0;
    private int score = 0;

    public int Count { get { return count; } }
    public int Score { get { return score; } }

    public SimpleStat() { }
    public SimpleStat(string nm, int cnt = 0, int sc = 0)
    {
        NameStat = nm;
        count = cnt;
        score = sc;
    }

    public void AddCnt(int n)
    {
        if (n > 0) count += n;
    }

    public void AddScore(int sc)
    {
        //count++;
        score += sc;
    }

    public string StrCount(string pref)
    {
        return $"{pref}{count}";
    }
}

public class Statistic
{
    private List<SimpleStat> stats = new List<SimpleStat>();

    public int Count { get { return stats.Count; } }
    public void AddSimpleStat(SimpleStat ss)
    {
        foreach (SimpleStat s in stats)
        {
            if (s.NameStat == ss.NameStat)
            {
                s.AddCnt(ss.Count);
                return;
            }
        }
        stats.Add(ss);
    }
    public void AddScoreStat(SimpleStat ss)
    {
        foreach (SimpleStat s in stats)
        {
            if (s.NameStat == ss.NameStat)
            {
                s.AddCnt(ss.Count);
                s.AddScore(ss.Score);
                return;
            }
        }
        stats.Add(ss);
    }

    public string GetStrStat(string nm, string pref)
    {
        foreach (SimpleStat ss in stats)
        {
            if (ss.NameStat == nm)
            {
                return ss.StrCount(pref);
            }
        }
        return "";
    }

    public SimpleStat GetSimpleStat(string nm)
    {
        foreach(SimpleStat ss in stats)
        {
            if (ss.NameStat == nm) return ss;
        }
        return null;
    }
}

