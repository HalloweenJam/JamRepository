using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DashDisplay : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private void Start()
        {
            PlayerController.OnDashRefill += ChangeAmount;
        }

        private void OnDisable()
        {
            PlayerController.OnDashRefill -= ChangeAmount;
        }

        private void ChangeAmount(float amount)
        {
            _image.fillAmount = 1 - amount;
        }
    }
}