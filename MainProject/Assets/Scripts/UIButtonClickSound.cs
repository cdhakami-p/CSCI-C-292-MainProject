using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonClickSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        if (AudioManager.Instance != null && clickSound != null)
        {
            AudioManager.Instance.PlaySFX(clickSound);
        }
    }
}
