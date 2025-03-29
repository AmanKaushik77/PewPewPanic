using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CurrentScore;
    [SerializeField] private TextMeshProUGUI UpdatedScore;
    [SerializeField] private Transform coinTextContainer;
    [SerializeField] private float duration;
    [SerializeField] private Ease animationCurve;

    private float containerInitPosition;
    private float moveAmount;

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        CurrentScore.SetText("0");
        UpdatedScore.SetText("0");
        containerInitPosition = coinTextContainer.localPosition.y;
        moveAmount = CurrentScore.rectTransform.rect.height;
    }

    public void UpdateScore(int score)
    {
        UpdatedScore.SetText($"{score}");
        coinTextContainer.DOLocalMoveY(containerInitPosition + moveAmount, duration)
                         .SetEase(animationCurve);
        StartCoroutine(ResetCoinContainer(score));
    }

    private IEnumerator ResetCoinContainer(int score)
    {
        yield return new WaitForSeconds(duration);
        CurrentScore.SetText($"{score}");
        Vector3 localPosition = coinTextContainer.localPosition;
        coinTextContainer.localPosition = new Vector3(localPosition.x, containerInitPosition, localPosition.z);
    }
}
