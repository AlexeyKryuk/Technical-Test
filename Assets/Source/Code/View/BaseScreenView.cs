using System.Collections;
using UnityEngine;

public abstract class BaseScreenView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    public void Show()
    {
        StartCoroutine(LerpAlpha(1f, 0.3f));
    }

    public void Hide()
    {
        StartCoroutine(LerpAlpha(0f, 0.3f));
    }

    private IEnumerator LerpAlpha(float alpha, float duration)
    {
        float elapsed = 0;
        float startValue = _canvasGroup.alpha;

        while (elapsed < duration)
        {
            float amount = Mathf.Lerp(startValue, alpha, elapsed / duration);
            elapsed += Time.deltaTime;

            _canvasGroup.alpha = amount;

            yield return null;
        }

        _canvasGroup.alpha = alpha;
        _canvasGroup.interactable = alpha == 1;
    }
}
