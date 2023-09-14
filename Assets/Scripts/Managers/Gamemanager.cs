using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Globalization;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;
    public int Gamecoins { get; set; }
    public int ExtracoinsAmount { get; set; }
    public int SaveDay { get; set; }
    public int ThisDay { get; set; }
    public bool GetReward { get; set; }

    [Header("UI")]
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI messageExtraCoinsText;
    [SerializeField] GameObject nextRewardText;
    [SerializeField] GameObject nextExtraCoinsText;
    [SerializeField] GameObject progressBar;
    [SerializeField] Button[] gameButtons;
    [SerializeField] int extraCoinsUses;

    private void Awake()
    {
        instance = this;
    }

    #region COINS

    public void SetCurrentCoins(int coins)
    {
        Gamecoins = coins;
        ShowCoins();
    }

    public void SpendCoin(int amount)
    {
        Gamecoins -= amount;
        ShowCoins();
    }

    public void AddCoin(int amount)
    {
        Gamecoins += amount;
        ShowCoins();
    }


    void ShowCoins()
    {
        coinsText.text = $"Coins {Gamecoins.ToString()} /10";
    }

    #endregion

    #region DAILY BONUS

    public void ObtainGift()
    {
        Gamecoins++;
        ShowCoins();
        gameButtons[2].interactable = false;
    }

    public void EnableDailyRewardButton(bool usedReward)
    {
        if (GetReward && !usedReward)
        {
            gameButtons[2].interactable = true;
            EnableOrDisableRewardText(false);
        }
        else { EnableOrDisableRewardText(true); }

    }

    public void EnableOrDisableRewardText(bool isEnabled)
    {
        nextRewardText.SetActive(isEnabled);
    }

    #endregion

    #region GET EXTRA COINS

    public void SetMessage (string message)
    {
        messageExtraCoinsText.text = message;
    }

    public void EnableProgressBar()
    {
        SetMessage("Please Wait...");
        progressBar.SetActive(true);
    }

    public void EnableDisableGameButtons(bool isEnabled)
    {
        bool dailyreWardButton = gameButtons[2].interactable;

        foreach (var button in gameButtons)
        {
            button.interactable = isEnabled;
        }

        gameButtons[2].interactable = dailyreWardButton;
    }

    public void EnableExtraCoinButtonFirsTime()
    {
        gameButtons[1].interactable = true;
    }

    public void EnableDisableExtraCoinButton()
    {

        if (ExtracoinsAmount < extraCoinsUses)
        {
            gameButtons[1].interactable = true;
        }
        else
        {
            gameButtons[1].interactable = false;
            nextExtraCoinsText.SetActive(true);
        }
    }

    public void AddExtraCoin(int amount)
    {
        ExtracoinsAmount += amount;
        EnableDisableExtraCoinButton();
        Debug.Log("Extra coins count: " + ExtracoinsAmount);
    }

    #endregion

}
