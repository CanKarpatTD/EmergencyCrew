using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.GlobalVariables;
using TMPro;
using TriflesGames.ManagerFramework;
using TriflesGames.Managers;
using UnityEngine;

public class PlayerManager : Manager<PlayerManager>
{
    public GameObject player;
    public Transform camera;

    public GameObject activeLevel;

    public int playerMoney;
    public bool isPlayerHasPatient;
    public GameObject childPatient;
    public Animator anim;

    public TextMeshProUGUI moneyText;
    
    
    protected override void MB_Listen(bool status)
    {
        if (status)
        {
            GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Play, TestDebug);
            GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Continue, TestDebug);
            GameManager.Instance.Subscribe(ManagerEvents.BtnClick_Retry, TestDebug);
        }
        else
        {
            GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Play, TestDebug);
            GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Continue, TestDebug);
            GameManager.Instance.Unsubscribe(ManagerEvents.BtnClick_Retry, TestDebug);
        }
    }

    private void TestDebug(object[] arguments)
    {
        player.transform.position = new Vector3(0, 0, 1.45f);
        player.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    protected override void MB_Start()
    {
        playerMoney = 1000;
        moneyText.text = playerMoney.ToString();
    }

    public void MoneyEffect()
    {
        moneyText.gameObject.transform.DOScale(1.2f, .1f).OnComplete(() =>
        {
            moneyText.gameObject.transform.DOScale(1, .1f);
        });
        
        moneyText.text = playerMoney.ToString();
    }
}
