using UnityEngine;
using System.Collections;

/// <summary>
/// Plays a brief blue flash or shield feedback when Defence blocks damage.
/// </summary>
public class BlockFeedbackView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer; // Or feedback GameObject
    [SerializeField] private float feedbackDuration = 0.15f;
    [SerializeField] private Color flashColor = Color.blue;

    private Color originalColor;
    private Coroutine feedbackCoroutine;

    private void Awake()
    {
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void PlayBlockFeedback()
    {
        if (spriteRenderer == null)
            return;
        if (feedbackCoroutine != null)
            StopCoroutine(feedbackCoroutine);
        feedbackCoroutine = StartCoroutine(FeedbackRoutine());
    }

    private IEnumerator FeedbackRoutine()
    {
        originalColor = spriteRenderer.color;
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(feedbackDuration);
        spriteRenderer.color = originalColor;
        feedbackCoroutine = null;
    }
}
