using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
    [SerializeField] private BossContainer _bossContainer;
    [SerializeField] private Color _color;
    private Color _setColor;
    private Material _material;
    private Coroutine _teleportation;

    private bool _isEnter = false;

    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
        _setColor = Color.black;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (_teleportation != null)
                StopCoroutine(_teleportation);

            _isEnter = true;
            _teleportation = StartCoroutine(ActivateTeleportation(true));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_teleportation != null)
        {
            StopCoroutine(_teleportation);
            _isEnter = false;
            _teleportation = StartCoroutine(ActivateTeleportation(false));
        }
    }    

    private IEnumerator ActivateTeleportation(bool activate)
    {
        float elapsedTime = 0f;
        float teleportationTime = activate ? 3f : 1.5f;

        float value = _setColor.r;
        _setColor = Color.black;
        float toValue = activate ? _color.r : _setColor.r;

        while (elapsedTime < teleportationTime)
        {
            _setColor.r = Mathf.Lerp(value, toValue, elapsedTime / teleportationTime);
            _material.SetColor("_GlowColor", _setColor);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;

        if(_isEnter)
            _bossContainer.LoadBossScene();
    } 
}
