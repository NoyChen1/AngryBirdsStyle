using UnityEngine;
using DG.Tweening;

public class TargetMovement : MonoBehaviour
{
    [SerializeField] private Vector3 moveOffset = new Vector3(2f, 0f, 0f);
    [SerializeField] private float duration = 2f;

    private void Start()
    {
        Vector3 targetPosition = transform.position + moveOffset;

        transform.DOMove(targetPosition, duration)
                 .SetEase(Ease.InOutSine)
                 .SetLoops(-1, LoopType.Yoyo); 
    }
}
