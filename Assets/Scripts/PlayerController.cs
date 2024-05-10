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
        if (Input.touchCount > 0 && !BuyAndImproveNPC.instance.isBuy)
        {
            Touch touch = Input.GetTouch(0);
            /*if (touch.phase == TouchPhase.Moved)
            {
                Vector3 touchDeltaPosition = new Vector3(touch.deltaPosition.x, 0, touch.deltaPosition.y);
                touchDeltaPosition = Camera.main.transform.TransformDirection(touchDeltaPosition);
                touchDeltaPosition.y = 0;
                transform.Translate(touchDeltaPosition * speed * Time.deltaTime, Space.World);
                if (Vector3.Magnitude(transform.position - target) > 0.1f) transform.LookAt(target);
            }*/
            if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out RaycastHit hit, 100))
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
