using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PEA_GameSceneUI : MonoBehaviour
{
    public static PEA_GameSceneUI instance = null;

    private float curTime = 0f;
    private int minutes = 0;
    private int seconds = 0;
    private readonly float hideTime = 10f;
    private readonly float seekTime = 11f;
    private readonly float overTime = 10f;
    private readonly float hideEmphasisTime = 5f;
    private readonly float seekEmphasisTime = 10f;

    private Coroutine coroutine = null;

    public GameObject readyUI;
    public GameObject inGameUI;
    public GameObject selectTeam;
    public GameObject witchTeam;
    public GameObject hunterTeam;
    public GameObject readyText;
    public GameObject hideText;
    public GameObject seekText;
    public GameObject fiveSeconds;
    public GameObject witchWinUI;
    public GameObject hunterWinUI;
    public GameObject menu;
    public TMP_Text overText;
    public TMP_Text witchCountText;
    public TMP_Text hunterCountText;
    public TMP_Text countDownText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 준비 단계에서 사용되는 함수들
    public void OnClickMenu()
    {
        GameManager.instance.LeaveRoom();
    }

    public void OnClickWitchTeam()
    {
        GameManager.instance.TeamToWitch();
        selectTeam.SetActive(false);
        witchTeam.SetActive(true);
        menu.SetActive(false);
        //SetTeamCountText(GameManager.instance.TeamToWitch());
    }

    public void OnClickHunterTeam()
    {
        GameManager.instance.TeamToHunter();
        selectTeam.SetActive(false);
        hunterTeam.SetActive(true);
        menu.SetActive(false);
        //SetTeamCountText(GameManager.instance.TeamToHunter());
    }

    public void SetTeamCountText(ExitGames.Client.Photon.Hashtable hash)
    {
        witchCountText.text = "Player : " + (int)hash["Witch_Count"];
        hunterCountText.text = "Player : " + (int)hash["Hunter_Count"];
    }

    public void OnClickReady()
    {
        GameManager.instance.OnClickReady();
        readyText.SetActive(GameManager.instance.IsReady);
    }

    public void OnClickChangeTeam()
    {
        selectTeam.SetActive(true);
        witchTeam.SetActive(false);
        hunterTeam.SetActive(false);

        SetTeamCountText(GameManager.instance.OnClickChangeTeam());
    }

    public void GameStart()
    {
        readyUI.SetActive(false);
        inGameUI.SetActive(true);
        CountDown(hideTime);
    }


    // 게임 중 사용되는 함수들
    public void CountDown(float countTime)
    {
        if(coroutine == null)
        {
            coroutine = StartCoroutine( ICountDown(countTime));
        }
    }

    public void GameOver()
    {
        overText.gameObject.SetActive(true);
        menu.SetActive(true);
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        CountDown(overTime);
    }

    public void HunterWin()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        inGameUI.SetActive(false);
        hunterWinUI.SetActive(true);
        GameOver();
    }

    IEnumerator ICountDown(float countTime)
    {
        curTime = countTime;

        while(curTime > 0)
        {
            curTime -= Time.deltaTime;
            minutes = (int)(curTime / 60);
            seconds = (int)(curTime % 60 + 1);
            if(seconds == 60)
            {
                minutes++;
                seconds--;
            }

            if(GameManager.instance.RoomState == GameManager.Room_State.Over)
            {
                overText.text = "Next Game starts in " + seconds;
            }
            else
            {
                countDownText.text = minutes.ToString().PadLeft(2, '0') + " : " + seconds.ToString().PadLeft(2, '0');

                if(seconds <= hideEmphasisTime && countTime == hideTime)
                {
                    fiveSeconds.GetComponent<PEA_FiveSeconds>().CountDownEmphasis((int)hideEmphasisTime);
                }
                else if(seconds <= seekEmphasisTime && countTime == seekTime)
                {
                    fiveSeconds.GetComponent<PEA_FiveSeconds>().CountDownEmphasis((int)seekEmphasisTime);
                }
            }            

            yield return new WaitForSeconds(Time.deltaTime);
        }

        if(GameManager.instance.RoomState == GameManager.Room_State.Over)
        {
            coroutine = null;
            GameManager.instance.Restart();
            yield return null;
        }
        else
        {
            if(countTime == hideTime)
            {
                hideText.SetActive(false);
                seekText.SetActive(true);
                yield return coroutine = StartCoroutine(ICountDown(seekTime));
            }
            else
            {
                GameManager.instance.WitchWin();
                //inGameUI.SetActive(false);
                seekText.SetActive(false);
                countDownText.gameObject.SetActive(false);
                witchWinUI.SetActive(true);
                coroutine = null;
                yield return null;
            }
        }
    }
}
