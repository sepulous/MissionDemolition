using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    public Text uitLevel;
    public Text uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;
    public GameObject restartMenu;

    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    void Start()
    {
        S = this;
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void Update()
    {
        UpdateGUI();

        if (mode == GameMode.playing && Goal.goalMet)
        {
            if (level + 1 == levelMax)
            {
                restartMenu.SetActive(true);
            }
            else
            {
                mode = GameMode.levelEnd;
                Invoke("NextLevel", 2f);
            }
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);
        }
    }

    public void StartLevel()
    {
        if (castle != null)
            Destroy(castle);

        Projectile.DESTROY_PROJECTILES();

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void NextLevel()
    {
        level++;
        StartLevel();
    }

    public void PlayAgain()
    {
        level = 0;
        shotsTaken = 0;
        restartMenu.SetActive(false);
        StartLevel();
    }

    void UpdateGUI()
    {
        uitLevel.text = $"Level: {level + 1} of {levelMax}";
        uitShots.text = $"Shots Taken: {shotsTaken}";
    }

    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }

    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }
}
