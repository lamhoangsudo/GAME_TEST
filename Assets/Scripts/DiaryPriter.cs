using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System;
public class DiaryPriter : MonoBehaviour
{
    public List<GameObject> diaryOnTable { get; private set; }
    [SerializeField] private Transform[] diaryPlaces;
    [SerializeField] private float instantiateTime;
    [SerializeField] private int maximumDiarySize;
    [SerializeField] private GameObject diaryPrefab;
    [SerializeField] private Transform diaryPlace;
    [SerializeField] private float Yoffset;
    private bool onWork;
    private float Yposition;
    private int placePositionIndex;
    [SerializeField] private float jumpPower;
    [SerializeField] private int numJumps;
    [SerializeField] private float duration;
    [SerializeField] private bool snapping;

    private void Awake()
    {
        diaryOnTable = new();
        onWork = false;
        placePositionIndex = 0;
    }
    private void Start()
    {
        AI.printerDestinations.Add(gameObject);
        BuyAndImproveNPC.instance.OnImprovePrinterSpeedCount += ImprovePrinterSpeed;
    }

    private void ImprovePrinterSpeed(object sender, int e)
    {
        ImprovePrinterSpeed(e);
    }

    private void Update()
    {
        StartCoroutine(InstantiateDiary());
    }
    private IEnumerator InstantiateDiary()
    {
        if((diaryOnTable.Count < maximumDiarySize || diaryOnTable.Contains(null) && onWork == false) && onWork == false)
        {
            onWork = true;
            GameObject newDiary = Instantiate(diaryPrefab, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z + 0.1f), Quaternion.identity, diaryPlace);
            Vector3 target;
            if(diaryOnTable.Contains(null))
            {
                int placePositionIndexs = (diaryOnTable.IndexOf(null) % diaryPlaces.Count());
                float Yposition = ((diaryOnTable.IndexOf(null) - (diaryOnTable.IndexOf(null) % diaryPlaces.Count())) / diaryPlaces.Count()) * Yoffset;
                diaryOnTable[diaryOnTable.IndexOf(null)] = newDiary;
                target = new Vector3(diaryPlaces[placePositionIndexs].position.x, diaryPlaces[placePositionIndexs].position.y + Yposition, diaryPlaces[placePositionIndexs].position.z);
            }
            else
            {
                diaryOnTable.Add(newDiary);
                target = new Vector3(diaryPlaces[placePositionIndex].position.x, diaryPlaces[placePositionIndex].position.y + Yposition, diaryPlaces[placePositionIndex].position.z);
                if(placePositionIndex < diaryPlaces.Count() - 1) placePositionIndex++;
                else
                {
                    placePositionIndex = 0;
                    Yposition += Yoffset;
                }
            }
            newDiary.transform.DOJump(target, jumpPower, numJumps, duration, snapping).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(instantiateTime);
            onWork = false;
        }
    }
    private void ImprovePrinterSpeed(int improveCount)
    {
        instantiateTime -= ((instantiateTime / 10) * improveCount);
    }
}
