using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    BlockingWait blockingWaitPopup;
    [SerializeField] PlayfabManager playfabManager;
    [SerializeField] GameObject blocking;

    public void Action_SpendOneCoin()
    {
        Debug.Log("MainMenu:Action_SpendOneCoin");
        playfabManager.SpendCoin(1);
    }

    public void Action_GetExtraCoin()
    {
        Debug.Log("MainMenu:Action_GetExtraCoin");
        blocking.SetActive(true);
    }

    public void Action_ClaimFreeCoin()
    {
        Debug.Log("MainMenu:Action_ClaimFreeCoin");
        Gamemanager.instance.EnableOrDisableRewardText(true);
        playfabManager.Reward(1, true);
    }
}
