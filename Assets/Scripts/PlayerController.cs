using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 target;
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    private List<GameObject> diaryList;
    private void Start()
    {
        animator = GetComponent<Animator>();
        diaryList = new();
        diaryList = GetComponent<DiaryHolding>().diaryList;
    }
    private void Update()
    {
        Move();
        AnimatorController();
    }
    private void Move()
    {
        if (Input.GetMouseButton(0) && !BuyAndImproveNPC.instance.isBuy)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                target = hit.point;
                target.y = 0;
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (Vector3.Magnitude(transform.position - target) > 0.1f) transform.LookAt(target);
            }
        }
    }
    private void AnimatorController()
    {
        if (Input.GetMouseButton(0))
        {
            if (diaryList.Count <= 0)
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
            if (diaryList.Count <= 0)
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
