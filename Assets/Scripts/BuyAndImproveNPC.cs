using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyAndImproveNPC : MonoBehaviour
{
    public static List<GameObject> worker;
    public static BuyAndImproveNPC instance;
    public static float NPCStartSpeed;
    private int NPCCount;

    [SerializeField] private int costNPC;
    [SerializeField] private Button buyNPCButton;
    [SerializeField] private TextMeshProUGUI costNPCText;
    [SerializeField] private float maxNPCSpeed;
    [SerializeField] private int costNPCSpeed;
    [SerializeField] private Button improveNPCSpeedButton;
    [SerializeField] private TextMeshProUGUI costNPCSpeedText;
    [SerializeField] private int maxNPCDiaryCapacity;
    [SerializeField] private int costNPCDiaryCapacity;
    [SerializeField] private Button improveNPCDiaryCapacityButton;
    [SerializeField] private TextMeshProUGUI costNPCDiaryCapacityText;

    private int improvePrinterSpeedCount;
    [SerializeField] private int maxImprovePrinterSpeedCount;
    [SerializeField] private int costPrinterSpeed;
    [SerializeField] private Button improvePrinterSpeedButton;
    [SerializeField] private TextMeshProUGUI costPrinterSpeedText;

    private int improveWorkDeskSpeedCount;
    [SerializeField] private int maxImproveWorkDeskSpeedCount;
    [SerializeField] private int costWorkDeskSpeed;
    [SerializeField] private Button improveWorkDeskSpeedButton;
    [SerializeField] private TextMeshProUGUI costWorkDeskSpeedText;
    [SerializeField] private Canvas canvas;

    public event EventHandler<int> OnImprovePrinterSpeedCount;
    public event EventHandler<int> OnImproveWorkDeskSpeed;
    public bool isBuy;
    private void Awake()
    {
        worker = new();
        NPCCount = 0;
        NPCStartSpeed = 2f;
        improvePrinterSpeedCount = 0;
        isBuy = false;
        canvas.gameObject.SetActive(false);
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        buyNPCButton.onClick.AddListener(BuyNPC);
        costNPCText.text = "$ " + costNPC.ToString();

        improveNPCSpeedButton.onClick.AddListener(ImproveNPCSpeed);
        costNPCSpeedText.text = "$ " + costNPCSpeed.ToString();

        improveNPCDiaryCapacityButton.onClick.AddListener(ImproveNPCDiaryCapacity);
        costNPCDiaryCapacityText.text = "$ " + costNPCDiaryCapacity.ToString();

        improvePrinterSpeedButton.onClick.AddListener(ImprovePrinterSpeed);
        costPrinterSpeedText.text = "$ " + costPrinterSpeed.ToString();

        improveWorkDeskSpeedButton.onClick.AddListener(ImproveWorkDeskSpeed);
        costWorkDeskSpeedText.text = "$ " + costWorkDeskSpeed.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isBuy)
        {
            isBuy = false;
            canvas.gameObject.SetActive(false);
        }
    }
    private void BuyNPC()
    {
        if (worker.Count > NPCCount)
        {
            if (PlayerPrefs.GetInt("money") >= costNPC)
            {
                PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - costNPC);
                worker[NPCCount].GetComponent<AI>().enabled = true;
                NPCCount++;
                costNPC = costNPC * 2;
                costNPCText.text = "$ " + costNPC.ToString();
                if (worker.Count <= NPCCount)
                {
                    costNPCText.text = "- MAX -";
                }
            }
        }
    }
    private void ImproveNPCSpeed()
    {
        if (PlayerPrefs.GetInt("money") >= costNPCSpeed)
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - costNPCSpeed);
            NPCStartSpeed += 2f;
            costNPCSpeed *= 2;
            costNPCSpeedText.text = "$ " + costNPCSpeed.ToString();
            if (NPCStartSpeed >= maxNPCSpeed)
            {
                costNPCSpeedText.text = "- MAX -";
            }
        }
    }
    private void ImproveNPCDiaryCapacity()
    {
        if (PlayerPrefs.GetInt("money") >= costNPCDiaryCapacity)
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - costNPCDiaryCapacity);
            for (int i = 0; i < worker.Count; i++)
            {
                worker[i].GetComponent<DiaryHolding>().IncreaseMaxDiaryCount(10);
            }
            costNPCDiaryCapacity *= 2;
            costNPCDiaryCapacityText.text = "$ " + costNPCDiaryCapacity.ToString();
            if (worker[0].GetComponent<DiaryHolding>().GetMaxDiaryCount() >= maxNPCDiaryCapacity)
            {
                costNPCDiaryCapacityText.text = "- MAX -";
            }
        }
    }
    private void ImprovePrinterSpeed()
    {
        if (improvePrinterSpeedCount < maxImprovePrinterSpeedCount)
        {
            if (PlayerPrefs.GetInt("money") >= costPrinterSpeed)
            {
                PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - costPrinterSpeed);
                improvePrinterSpeedCount++;
                OnImprovePrinterSpeedCount?.Invoke(this, improvePrinterSpeedCount);
                costPrinterSpeed *= 2;
                costPrinterSpeedText.text = "$ " + costPrinterSpeed.ToString();
                if (improvePrinterSpeedCount >= maxImprovePrinterSpeedCount)
                {
                    costPrinterSpeedText.text = "- MAX -";
                }
            }
        }
    }
    private void ImproveWorkDeskSpeed()
    {
        if (improveWorkDeskSpeedCount < maxImproveWorkDeskSpeedCount)
        {
            if (PlayerPrefs.GetInt("money") >= costWorkDeskSpeed)
            {
                PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - costWorkDeskSpeed);
                improveWorkDeskSpeedCount++;
                OnImproveWorkDeskSpeed?.Invoke(this, improveWorkDeskSpeedCount);
                costWorkDeskSpeed *= 2;
                costWorkDeskSpeedText.text = "$ " + costWorkDeskSpeed.ToString();
                if (improveWorkDeskSpeedCount >= maxImproveWorkDeskSpeedCount)
                {
                    costWorkDeskSpeedText.text = "- MAX -";
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isBuy = true;
            canvas.gameObject.SetActive(true);
        }
    }
}
