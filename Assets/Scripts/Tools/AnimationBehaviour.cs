using UnityEngine;
using DG.Tweening;

public class AnimationBehaviour : MonoBehaviour
{
    public GameObject gameObjectToMove;
    public Transform targetPosition;
    public float moveSpeed = 1f;
    public Ease easeType = Ease.Linear;
    public bool moveOnXAxis = true;
    public bool moveOnYAxis = true;

    public GameObject gameObjectToMove2;
    public Transform targetPosition2;

    public void MoveObjectToTarget()
    {
        if (gameObjectToMove != null && targetPosition != null)
        {
            Vector3 targetPos = new Vector3(moveOnXAxis ? targetPosition.position.x : gameObjectToMove.transform.position.x,
                moveOnYAxis ? targetPosition.position.y : gameObjectToMove.transform.position.y,
                gameObjectToMove.transform.position.z);

            gameObjectToMove.transform.DOMove(targetPos, moveSpeed).SetEase(easeType);
        }
    }

    public void MoveObjectToTarget2()
    {
        if (gameObjectToMove2 != null && targetPosition2 != null)
        {
            Vector3 targetPos2 = new Vector3(moveOnXAxis ? targetPosition2.position.x : gameObjectToMove2.transform.position.x,
                moveOnYAxis ? targetPosition2.position.y : gameObjectToMove2.transform.position.y,
                gameObjectToMove2.transform.position.z);

            gameObjectToMove2.transform.DOMove(targetPos2, moveSpeed).SetEase(easeType);
        }
    }
}
