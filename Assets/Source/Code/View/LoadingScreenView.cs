using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenView : BaseScreenView
{
    [SerializeField] private Image _progressBar;
    [SerializeField] private float _timeOut;
    [SerializeField] private float _minDuration;
    [SerializeField] private float _delay;

    private Task _progress;
    private bool _isCorrected;

    public void Initialize(Task progress)
    {
        _progress = progress;
    }

    public async Task Play()
    {
        float elapsed = 0;
        float startValue = _progressBar.fillAmount;
        float duration = _timeOut;

        while (elapsed < duration)
        {
            float amount = Mathf.Lerp(startValue, 1f, elapsed / duration);
            elapsed += Time.deltaTime;

            _progressBar.fillAmount = amount;

            if (_isCorrected == false)
                duration = CorrectDuration(elapsed, duration);

            await Task.Yield();
        }

        _progressBar.fillAmount = 1f;
        await Task.CompletedTask;
    }

    private float CorrectDuration(float elapsed, float duration)
    {
        if (_progress.IsCompleted)
        {
            _isCorrected = true;

            if (elapsed > _minDuration)
                return duration + _delay;
            else
                return _minDuration + _delay;
        }

        return duration;
    }
}
