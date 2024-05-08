using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnlockArea : MonoBehaviour
{
    [SerializeField] private Image unlockBar;
    [SerializeField] private int unlockPrice;
    private int moneyPaid;
    private int moneyRemain;
    [SerializeField] private GameObject unlockObj;
    [SerializeField] private TextMeshProUGUI text;
    private bool runOneTime;
    private NavMeshSurface surface;
    private void Awake()
    {
        moneyPaid = 0;
        unlockObj.SetActive(false);
    }
    private void Start()
    {
        surface = GetComponent<NavMeshSurface>();
        runOneTime = true;
    }
    private void Update()
    {
        moneyRemain = unlockPrice - moneyPaid;
        text.text = "$ " + (moneyRemain).ToString();
        unlockBar.fillAmount = (float)moneyPaid / unlockPrice;

        if (moneyPaid >= unlockPrice && runOneTime)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            unlockObj.SetActive(true);
            surface.BuildNavMesh();
            runOneTime = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && (PlayerPrefs.GetInt("money") > 0))
        {
            if ((PlayerPrefs.GetInt("money")) <= moneyRemain)
            {
                moneyPaid += PlayerPrefs.GetInt("money");
                PlayerPrefs.SetInt("money", 0);
            }
            else
            {
                moneyPaid = unlockPrice;
                PlayerPrefs.SetInt("money", (PlayerPrefs.GetInt("money") - moneyRemain));
            }
        }
    }
}
