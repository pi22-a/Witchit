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
    private readonly float seekTime = 180f;
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
    public TMP_Text aliveWitchCountText;
    public Transform witchPlayerList;
    public Transform watchPlayerList;
    public Transform hunterPlayerList;
    public GameObject player;
    public GameObject playerList;
    public GameObject countdownUI;

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
        if(GameManager.instance.RoomState == GameManager.Room_State.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ActivePlayerList(true);
            }
            else if (Input.GetKeyUp(KeyCode.Tab))
            {
                ActivePlayerList(false);
            }
        }
    }

    // 준비 단계에서 사용되는 함수들
    public void OnClickMenu()
    {
        GameManager.instance.LeaveRoom();
        SoundManager.instance.PlayEffect(SoundManager.Effect.Button_Click);
    }

    public void OnClickWitchTeam()
    {
        GameManager.instance.TeamToWitch();
        selectTeam.SetActive(false);
        witchTeam.SetActive(true);
        menu.SetActive(false);
        SoundManager.instance.PlayEffect(SoundManager.Effect.Button_Click);
        //SetTeamCountText(GameManager.instance.TeamToWitch());
    }

    public void OnClickHunterTeam()
    {
        GameManager.instance.TeamToHunter();
        selectTeam.SetActive(false);
        hunterTeam.SetActive(true);
        menu.SetActive(false);
        SoundManager.instance.PlayEffect(SoundManager.Effect.Button_Click);
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
        SoundManager.instance.PlayEffect(SoundManager.Effect.Button_Click);
    }

    public void OnClickChangeTeam()
    {
        selectTeam.SetActive(true);
        witchTeam.SetActive(false);
        hunterTeam.SetActive(false);

        SetTeamCountText(GameManager.instance.OnClickChangeTeam());
        SoundManager.instance.PlayEffect(SoundManager.Effect.Button_Click);
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

    public void SetAliveWitchCountText(int aliveWitchCount)
    {
        aliveWitchCountText.text = "Witch : " + aliveWitchCount;
    }

    private void ActivePlayerList(bool isActive)
    {
        playerList.SetActive(isActive);
        countdownUI.SetActive(!isActive);
    }

    public void RemovePlayerList()
    {
        foreach(Transform tr in witchPlayerList)
        {
            Destroy(tr.gameObject);
        }  

        foreach(Transform tr in watchPlayerList)
        {
            Destroy(tr.gameObject);
        }

        foreach(Transform tr in hunterPlayerList)
        {
            Destroy(tr.gameObject);
        }
    }

    public void SetPlayerList(string[] players, string[] teams)
    {
        RemovePlayerList();
        for (int i = 0; i < players.Length; i++)
        {
            GameObject player = Instantiate(this.player);
            switch (teams[i])
            {
                case "Witch":
                    player.transform.SetParent(witchPlayerList);
                    break;
                case "Watch":
                    player.transform.SetParent(watchPlayerList);
                    break;
                case "Hunter":
                    player.transform.SetParent(hunterPlayerList);
                    break;
            }
            player.transform.localScale = Vector3.one;
            player.GetComponentInChildren<TMP_Text>().text = players[i];
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

                if( minutes == 0 && seconds <= hideEmphasisTime && countTime == hideTime)
                {
                    fiveSeconds.GetComponent<PEA_FiveSeconds>().CountDownEmphasis((int)hideEmphasisTime);
                }
                else if( minutes == 0 && seconds <= seekEmphasisTime && countTime == seekTime)
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
                GameManager.instance.HunterGo();
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
