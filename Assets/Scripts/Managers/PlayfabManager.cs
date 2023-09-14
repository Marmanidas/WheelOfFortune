using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor.PackageManager;
using UnityEditor.Search;

public class PlayfabManager : MonoBehaviour
{
    public bool UsedReward { get; set; }
    public bool DailyReward { get; set; }

    [SerializeField] string idNumber;

    void Start()
    {
        AndroidLogin();
    }

    #region LOGIN

    void AndroidLogin()
    {
        var androidRequest = new LoginWithAndroidDeviceIDRequest
        {
            //AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            AndroidDeviceId = idNumber,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(androidRequest, Onsucess, OnError);
    }

    void Onsucess(LoginResult result)
    {
        Debug.Log("Login successful");
        GetCurrency();
        LoadData();
    }


    #endregion

    #region LOAD DATA

    public void LoadData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), GetData, OnError);
    }

    void GetData(GetUserDataResult result)
    {
        if (result.Data != null)
        {
            if (result.Data.ContainsKey("TakeGift"))
            {
                UsedReward = bool.Parse(result.Data["TakeGift"].Value);
            }

            Gamemanager.instance.EnableDailyRewardButton(UsedReward);

            if (Gamemanager.instance.GetReward == false)
            {
                SaveGift(false);
            }
            if (result.Data.ContainsKey("Day"))
            {
                Gamemanager.instance.SaveDay = int.Parse(result.Data["Day"].Value);
            }
            if (result.Data.ContainsKey("Extra_Coins"))
            {
                if (Gamemanager.instance.GetReward == true && Gamemanager.instance.SaveDay != Gamemanager.instance.ThisDay)
                {
                    SaveDay(TimerUtility.CurrentTime.Day);
                    Gamemanager.instance.ExtracoinsAmount = 0;
                    AddExtraCoins();
                    return;
                }
                Gamemanager.instance.AddExtraCoin(int.Parse(result.Data["Extra_Coins"].Value));
            }
            else { Gamemanager.instance.EnableExtraCoinButtonFirsTime();}

               
        }
    }

    #endregion

    #region CURRENCY

    void GetCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), CurrencyObtained, OnError);
    }

    void CurrencyObtained(GetUserInventoryResult result)
    {
        Gamemanager.instance.SetCurrentCoins(result.VirtualCurrency["CO"]);
    }

    public void SpendCoin(int amount)
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CO",
            Amount = amount
        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, result => SubtractedCoin(result, amount) , OnError);
    }

    void SubtractedCoin(ModifyUserVirtualCurrencyResult result, int amount)
    {
        Gamemanager.instance.SpendCoin(amount);
    }

    #endregion

    #region DAILY REWARD

    public void Reward(int amount, bool isGift)
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CO",
            Amount = amount
        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, result => GiftObtained (result, isGift), OnError);
    }
    void GiftObtained(ModifyUserVirtualCurrencyResult result, bool isDdailyGift)
    {
        if (isDdailyGift)
        {
            Gamemanager.instance.ObtainGift();
            UsedReward = true;
            SaveGift(UsedReward);
        }

    }

    public void SaveGift(bool takeGift)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"TakeGift", takeGift.ToString()},
            }
        };

        PlayFabClientAPI.UpdateUserData(request, GiftTaken, OnError);
    }


    void GiftTaken(UpdateUserDataResult resultado)
    {
        Debug.Log("Enable gift saved");
    }

    #endregion

    #region GET EXTRA COINS

    public void AddExtraCoins()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Extra_Coins", Gamemanager.instance.ExtracoinsAmount.ToString()},
            }
        };

        PlayFabClientAPI.UpdateUserData(request, CoinTaken, OnError);
    }

    void CoinTaken(UpdateUserDataResult resultado)
    {
        Debug.Log("Extra coin added");
    }

    public void SaveDay(int day)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Day", day.ToString()},
            }
        };

        PlayFabClientAPI.UpdateUserData(request, result => DaySaved (result, day), OnError);
    }

    void DaySaved(UpdateUserDataResult result, int day)
    {
        Debug.Log("Day saved");
    }

  

    #endregion

    void OnError(PlayFabError error)
    {
        Debug.Log("An error occurred " + error.GenerateErrorReport());
    }

}
