using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class WheelOfFortune : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numberSelectedText;
    [SerializeField] Button[] wheelButtons;
    [SerializeField] RectTransform wheel;
    [SerializeField] TMP_InputField inputBet;
    [SerializeField] PlayfabManager playfabManager;

    private float rotationDuration = 5f;
    private Ease easingType = Ease.OutCubic;
    private int gameTurn;
    private int turnNumber;
    private int amountBet;
    private bool numberIsSelected;
    private bool madeBet;
    private bool isWinning;

    void Start()
    {
        DOTween.Init();
    }

    public void ButonNumber(int number)
    {
        numberIsSelected = true;
        numberSelectedText.text = number.ToString();
        turnNumber = number;
    }

    void EnableDisableWheelButtons(bool isEnabled)
    {
        foreach (var button in wheelButtons)
        {
            button.interactable = isEnabled;
        }
    }

    public void CheckTurn()
    {
        if (numberIsSelected && madeBet)
        {
            SetTurn(turnNumber);
            EnableDisableWheelButtons(false);
        }
    }

    #region BET

    public void PlaceBet()
    {
        amountBet = int.Parse(inputBet.text);

        if (amountBet != 0 && amountBet <= Gamemanager.instance.Gamecoins)
        {
            inputBet.text = "";
            madeBet = true;
        }
    }

    #endregion

    #region SET TURNS

    void SetTurn(int selectedNumber)
    {
        gameTurn++;
        Debug.Log("Game turn is " + gameTurn);
        switch (gameTurn)
        {
            case 1:
            case 3:
            case 4:
            case 5:
            case 6:
            case 9:
                UserLoses();
                break;
            case 2:
            case 7:
            case 8:
                UserWins();
                break;
            default:
                RandomResult();
                break;

        }

    }

    void UserLoses()
    {
        Debug.Log("User Loses");
        isWinning = false;
        float value = Random.value;
        if (value <= 0.5)
        {
            turnNumber -= Random.Range(1, 4);
            SpinWheel(turnNumber);
            return;
        }
        turnNumber += Random.Range(1, 4);
        SpinWheel(turnNumber);


    }

    void UserWins()
    {
        Debug.Log("User Wins");
        isWinning = true;
        SpinWheel(turnNumber);
    }

    void RandomResult()
    {
        int percentage = Random.Range(1, 101);
        if (percentage <= 5)
        {
            UserWins();
            return;
        }
        UserLoses();
    }


    #endregion

    #region SPINWHEEL

    public void SpinWheel(int targetNumber)
    {
        float targetAngle = CalculateTargetAngle(targetNumber);
        float totalRotation = 360f * rotationDuration + targetAngle;

        wheel.DORotate(new Vector3(0, 0, -totalRotation), rotationDuration, RotateMode.FastBeyond360).SetEase(easingType).OnComplete(() =>
        {
            Result();
        });
    }

    private float CalculateTargetAngle(int targetNumber)
    {
        switch (targetNumber)
        {
            case 1:
                return 0;
            case 2:
                return -45;
            case 3:
                return -90;
            case 4:
                return -135;
            case 5:
                return -180;
            case 6:
                return -225;
            case 7:
                return -270;
            case 8:
                return -315;
            default:
                return 0;


        }

    }

    #endregion

    public void Result()
    {
        numberIsSelected = false;
        madeBet = false;
        EnableDisableWheelButtons(true);

        if (!isWinning)
        {
            playfabManager.SpendCoin(amountBet);
        }
        else
        {
            Gamemanager.instance.AddCoin(amountBet * 2);
            playfabManager.Reward(amountBet * 2, false);
        }

    }


}
