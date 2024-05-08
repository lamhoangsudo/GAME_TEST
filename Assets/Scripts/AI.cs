using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public static List<GameObject> workDeskDestinations;
    public static List<GameObject> printerDestinations;
    private NavMeshAgent agent;
    private List<GameObject> diaryCount;
    private int targetDesk;
    private int targetPrinter;
    public bool isWorking;
    private Animator animator;
    [SerializeField] private Vector3 debugVector;
    private void Awake()
    {
        workDeskDestinations = new();
        printerDestinations = new();
        diaryCount = new();
        isWorking = false;
        BuyAndImproveNPC.worker.Add(gameObject);
        GetComponent<AI>().enabled = false;
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        diaryCount = GetComponent<DiaryHolding>().diaryList;
        targetDesk = 0;
        targetPrinter = 0;
        isWorking = false;
        animator = GetComponent<Animator>();
        agent.speed = BuyAndImproveNPC.NPCStartSpeed;
    }
    private void Update()
    {
        SetDestination();
        if(agent.speed != BuyAndImproveNPC.NPCStartSpeed)
        {
            agent.speed = BuyAndImproveNPC.NPCStartSpeed;
        }
        AnimatorController();
    }
    private void SetDestination()
    {
        TryGetComponent<DiaryHolding>(out var diaryHolding);
        if (diaryCount.Count >= diaryHolding.GetMaxDiaryCount())
        {
            diaryHolding.isPrinterTarget = false;
            Vector3 target = workDeskDestinations[targetDesk].transform.position;
            target.y = transform.position.y;
            agent.SetDestination(target);
            debugVector = target;
            Vector3 moveDir = (new Vector3(workDeskDestinations[targetDesk].transform.position.x, transform.position.y, workDeskDestinations[targetDesk].transform.position.z) - transform.position).normalized;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 20f);
            Debug.Log(Vector3.Distance(transform.position, workDeskDestinations[targetDesk].transform.position));
            if (Vector3.Distance(transform.position, workDeskDestinations[targetDesk].transform.position) < 1f)
            {
                isWorking = false;
                diaryHolding.isWorkDeskTarget = true;
                targetPrinter = GetTargetPrinter();
            }
            else
            {
                isWorking = true;
            }
        }
        else if(diaryCount.Count == 0)
        {
            diaryHolding.isWorkDeskTarget = false;
            Vector3 target = printerDestinations[targetPrinter].transform.position;
            target.y = transform.position.y;
            agent.SetDestination(target);
            debugVector = target;
            Vector3 moveDir = (new Vector3(printerDestinations[targetPrinter].transform.position.x, transform.position.y, printerDestinations[targetPrinter].transform.position.z) - transform.position).normalized;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 20f);
            Debug.Log(Vector3.Distance(transform.position, printerDestinations[targetPrinter].transform.position));
            if (Vector3.Distance(transform.position, printerDestinations[targetPrinter].transform.position) < 1f)
            {
                isWorking = false;
                diaryHolding.isPrinterTarget = true;
                targetDesk = GetTargetWorkDesk();
            }
            else
            {
                isWorking = true;
            }
        }
    }
    private int GetTargetWorkDesk()
    {
        int targetDesk = 0;
        var minPaperCount = 0;
        for (int i = 0; i < workDeskDestinations.Count; i++)
        {
            workDeskDestinations[i].TryGetComponent<WorkDesk>(out WorkDesk workDesk);
            if (i == 0)
            {
                minPaperCount = workDesk.diaryList.Count;
                targetDesk = i;
            }
            if (minPaperCount > workDesk.diaryList.Count)
            {
                minPaperCount = workDesk.diaryList.Count;
                targetDesk = i;
            }
        }
        return targetDesk;
    }
    private int GetTargetPrinter()
    {
        int targetPrinter = 0;
        int maxPaperCount = 0;
        int paperCount = 0;
        for (int i = 0; i < printerDestinations.Count; i++)
        {
            printerDestinations[i].TryGetComponent<DiaryPriter>(out DiaryPriter diaryPriter);
            for (int j = 0; j < diaryPriter.diaryOnTable.Count; j++)
            {
                if (diaryPriter.diaryOnTable[j] != null)
                {
                    paperCount++;
                }
            }
            if (i == 0)
            {
                maxPaperCount = paperCount;
                targetPrinter = i;
            }
            if (maxPaperCount < paperCount)
            {
                maxPaperCount = paperCount;
                targetPrinter = i;
            }
            paperCount = 0;
        }
        return targetPrinter;
    }
    private void AnimatorController()
    {
        if (isWorking)
        {
            if (diaryCount.Count <= 0)
            {
                animator.SetBool("IsRunWithoutDiary", true);
                animator.SetBool("isCarrying", false);
                animator.SetBool("IsRunWithDiary", false);
            }
            else
            {
                animator.SetBool("IsRunWithoutDiary", false);
                animator.SetBool("isCarrying", true);
                animator.SetBool("IsRunWithDiary", true);
            }
        }
        else
        {
            if (diaryCount.Count <= 0)
            {
                animator.SetBool("IsRunWithoutDiary", false);
                animator.SetBool("isCarrying", false);
                animator.SetBool("IsRunWithDiary", false);
            }
            else
            {
                animator.SetBool("IsRunWithoutDiary", false);
                animator.SetBool("isCarrying", true);
                animator.SetBool("IsRunWithDiary", false);
            }
        }
    }
}
