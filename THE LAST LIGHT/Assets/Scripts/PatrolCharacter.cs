using System.Collections;
using UnityEngine;

public class PatrolCharacter : MonoBehaviour
{
    public float walkDistance = 4f;
    public float walkSpeed = 1.5f;
    public float waitTime = 2f;

    private Animator animator;
    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPos = transform.position;
        endPos = startPos + transform.forward * walkDistance;
        StartCoroutine(PatrolLoop());
    }

    IEnumerator PatrolLoop()
    {
        while (true)
        {
            yield return StartCoroutine(WalkTo(endPos));
            animator.SetBool("isWalking", false);
            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(WalkTo(startPos));
            animator.SetBool("isWalking", false);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator WalkTo(Vector3 target)
    {
        animator.SetBool("isWalking", true);
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, target, walkSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }
}