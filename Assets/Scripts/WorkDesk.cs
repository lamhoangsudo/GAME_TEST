using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkDesk : MonoBehaviour
{
    [SerializeField] private int maxDiaryCount;
    [SerializeField] private float processTime;
    [SerializeField] private GameObject money;
    public List<GameObject> diaryList { get; private set; } = new();
    [SerializeField] private List<Transform> moneyPlaces;
    private float YOffset;
    [SerializeField] private Animator animator;
    private List<GameObject> moneys;
    [SerializeField] private GameObject moneyCollectArea;
    private Transform player;
    private int moneyPlaceIndex;
    private bool isWorking;
    private void Awake()
    {
        diaryList = new();
        moneys = new();
        moneyPlaceIndex = 0;
    }
    private void Start()
    {
        AI.workDeskDestinations.Add(gameObject);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        BuyAndImproveNPC.instance.OnImproveWorkDeskSpeed += ImproveWorkDeskSpeed;
    }

    private void ImproveWorkDeskSpeed(object sender, int e)
    {
        ImproveWorkDeskSpeed(e);
    }

    private void Update()
    {
        StartCoroutine(DiaryWork());
        MoneyCollect();
        WorkerAnimation();
    }
    private void WorkerAnimation()
    {
        if(diaryList.Count > 0)
        {
            animator.SetBool("IsWorking", true);
        }
        else
        {
            animator.SetBool("IsWorking", false);
        }
    }
    private void MoneyCollect()
    {
        if(Vector3.Magnitude(moneyCollectArea.GetComponent<Collider>().bounds.center - player.position) < 1f)
        {
            for(int i = moneys.Count - 1; i >= 0; i--)
            {
                moneys[i].GetComponent<Money>().isCollected = true;
                moneys.RemoveAt(i);
            }
        }
    }
    private IEnumerator DiaryWork()
    {
        if(!isWorking && diaryList.Count > 0)
        {
            isWorking = true;
            diaryList[diaryList.Count - 1].gameObject.SetActive(false);
            diaryList.RemoveAt(diaryList.Count - 1);
            if(moneys.Count <= 0)
            {
                moneyPlaceIndex = 0;
                YOffset = 0;
            }
            Vector3 target = moneyPlaces[moneyPlaceIndex].position;
            target.y = moneyPlaces[moneyPlaceIndex].position.y + YOffset;
            GameObject newMoney = Instantiate(money, target, Quaternion.Euler(-90, 0, 0));
            moneys.Add(newMoney);
            if(moneyPlaceIndex < (moneyPlaces.Count - 1))
            {
                moneyPlaceIndex++;
            }
            else
            {
                moneyPlaceIndex = 0;
                YOffset += 0.05f;
            }
            yield return new WaitForSeconds(processTime);
            isWorking = false;
        }
    }
    public int GetMaxDiaryCount()
    {
        return maxDiaryCount;
    }
    private void ImproveWorkDeskSpeed(int improveCount)
    {
        processTime -= ((processTime / 10) * improveCount);
    }
}
