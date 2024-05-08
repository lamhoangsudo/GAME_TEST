using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyCountText;
    private void Awake()
    {
        PlayerPrefs.DeleteKey("money");
    }
    void Update()
    {
        moneyCountText.text = "$" + PlayerPrefs.GetInt("money");
    }

}
