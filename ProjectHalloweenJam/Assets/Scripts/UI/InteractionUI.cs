using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InteractionUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            PlayerInteract.OnInteractionNearby += (message) => _text.text = message;
            PlayerInteract.OnInteractionLeft += () => _text.text = "";
        }

        private void OnDisable()
        {
            PlayerInteract.OnInteractionNearby -= (message) => _text.text = message;
            PlayerInteract.OnInteractionLeft -= () => _text.text = "";
        }
    }
}