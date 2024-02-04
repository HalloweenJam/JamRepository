using TMPro;
using UnityEngine;
using System.Collections;

public class LoadingText : MonoBehaviour
{
    [SerializeField] private float _speedAnimation;
    private TextMeshProUGUI _text;
    private Coroutine _loading;

    private void Awake() => _text = GetComponent<TextMeshProUGUI>();

    private void OnEnable() => _loading = StartCoroutine(LoadingCoroutine());
    
    private IEnumerator LoadingCoroutine()
    {
        string loading = _text.text;
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {             
                _text.text += ".";
                yield return new WaitForSeconds(_speedAnimation);
            }
            _text.text = loading;
            yield return null;
        }
    }

    private void OnDisable()
    {
        if (_loading != null)
           StopCoroutine( _loading );
    }
}
