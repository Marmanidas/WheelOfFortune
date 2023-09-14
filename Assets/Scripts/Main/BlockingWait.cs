using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockingWait : MonoBehaviour
{

    [SerializeField] GameObject finishedPopup;
    [SerializeField] PlayfabManager playfabManager;

    private void OnEnable()
    {
        finishedPopup.SetActive(true);
        Gamemanager.instance.EnableProgressBar();
        Debug.Log("BlockingWait:OnEnable");
    }

    public void CloseBlockingScreen()
    {
        Gamemanager.instance.AddExtraCoin(1);
        Gamemanager.instance.AddCoin(1);
        playfabManager.AddExtraCoins();
        playfabManager.Reward(1, false);
        finishedPopup.SetActive(false);
        gameObject.SetActive(false);
        Debug.Log("BlockingWait:CloseBlockingScreen");
    }
}
