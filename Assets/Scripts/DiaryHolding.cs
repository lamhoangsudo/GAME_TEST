using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiaryHolding : MonoBehaviour
{
    public List<GameObject> diaryList { get; private set; }
    [SerializeField] private int maxDiaryCount;
    [SerializeField] private Transform posCarry;
    private int diaryCount;
    public bool isWorkDeskTarget;
    public bool isPrinterTarget;
    private void Awake()
    {
        diaryList = new();
        diaryCount = 0;
    }
    private void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            isWorkDeskTarget = true;
            isPrinterTarget = true;
        }
        else
        {
            isWorkDeskTarget = false;
            isPrinterTarget = false;
        }
    }
    private void Update()
    {
        DiaryCarry();
    }
    private void DiaryCarry()
    {
        if (diaryCount != diaryList.Count)
        {
            for (int i = 0; i < diaryList.Count; i++)
            {
                if (i == 0)
                {
                    diaryList[i].transform.position = Vector3.Slerp(diaryList[i].transform.position, posCarry.position, Time.deltaTime * 5f);
                    if (diaryList[i].transform.parent != posCarry)
                    {
                        diaryList[i].transform.parent = null;
                        diaryList[i].transform.parent = posCarry;
                        diaryList[i].transform.localRotation = Quaternion.Euler(0, Random.Range(-1f, 1f), 0);
                    }
                }
                else
                {
                    Vector3 target = Vector3.zero;
                    float yOffSet = 0.07f;
                    target.x = Mathf.Lerp(diaryList[i].transform.position.x,
                        diaryList[i - 1].transform.position.x, Time.deltaTime * 20);
                    target.y = Mathf.Lerp(diaryList[i].transform.position.y,
                        diaryList[0].transform.position.y + yOffSet * i, Time.deltaTime * 20);
                    target.z = diaryList[i - 1].transform.position.z;
                    diaryList[i].transform.position = target;
                    if (diaryList[i].transform.parent != posCarry)
                    {
                        diaryList[i].transform.parent = null;
                        diaryList[i].transform.parent = posCarry;
                        diaryList[i].transform.localRotation = Quaternion.Euler(0, Random.Range(-1f, 1f), 0);
                    }
                }
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 6 && diaryList.Count <= maxDiaryCount && isPrinterTarget)
        {
            if (collision.gameObject.TryGetComponent(out DiaryPriter diaryPriter))
            {
                for (int i = diaryPriter.diaryOnTable.Count - 1; i >= 0; i--)
                {
                    if (diaryPriter.diaryOnTable[i] == null)
                    {
                        continue;
                    }
                    if (diaryList.Count >= maxDiaryCount)
                    {
                        break;
                    }
                    diaryList.Add(diaryPriter.diaryOnTable[i]);
                    diaryPriter.diaryOnTable[i] = null;
                }
            }
        }

        if (collision.gameObject.layer == 8 && diaryList.Count > 0 && isWorkDeskTarget)
        {
            if (collision.gameObject.TryGetComponent(out WorkDesk workDesk))
            {
                collision.gameObject.TryGetComponent(out Collider collider);
                Vector3 target = collider.bounds.center;
                if (workDesk.diaryList.Count < workDesk.GetMaxDiaryCount())
                {
                    for (int i = diaryList.Count - 1; i >= 0; i--)
                    {
                        if (workDesk.GetMaxDiaryCount() <= workDesk.diaryList.Count) break;
                        target.y = 0.5f + collider.bounds.size.y + workDesk.diaryList.Count * 0.05f;
                        diaryList[i].transform.parent = null;
                        diaryList[i].transform.DOJump(target, 1.5f, 1, 0.5f).SetEase(Ease.OutQuad);
                        workDesk.diaryList.Add(diaryList[i]);
                        diaryList[i].transform.localRotation = Quaternion.Euler(0, Random.Range(60f, 90f), 0);
                        diaryList.RemoveAt(i);
                    }
                }
            }
        }
    }
    public int GetMaxDiaryCount()
    {
        return maxDiaryCount;
    }
    public void IncreaseMaxDiaryCount(int value)
    {
        maxDiaryCount += value;
    }
}
