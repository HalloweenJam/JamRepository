using Player;
using Enemy.Arena;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class MinimapTeleport : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Minimap _minimap;
    [SerializeField] private Image _circleIcon;
    [SerializeField] private LayerMask _layerMask;

    private Vector2 _localPoint;
    private Coroutine _coroutine;

    private void Awake() => _minimap.OnOpenMinimap += EnableImage;

    private void EnableImage(bool active) => _circleIcon.gameObject.SetActive(active);

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_minimap.MinimapRect, eventData.position, eventData.pressEventCamera, out _localPoint);
        var worldPosition = new Vector2(_localPoint.x, _localPoint.y / 1.777f);
        worldPosition /= _minimap.ScaleRatio;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(worldPosition, Vector2.one * 2, 0, Vector2.zero, 0, _layerMask);
        if (hits.Length == 1 && hits[0].collider != null && ((hits[0].collider.transform.root.TryGetComponent(out Arena arena) && arena.IsCompleted) || hits[0].collider.CompareTag("StartRoom")))
        {
            PlayerController.TeleportPlayer.Invoke(worldPosition);
            _minimap.OpenMinimap();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_minimap.MinimapRect, eventData.position, eventData.pressEventCamera, out _localPoint);
        _circleIcon.rectTransform.anchoredPosition = _localPoint;

        var worldPosition = new Vector2(_localPoint.x, _localPoint.y / 1.777f);
        worldPosition /= _minimap.ScaleRatio;

        RaycastHit2D[] hits;
        hits = Physics2D.BoxCastAll(worldPosition, Vector2.one * 2, 0, Vector2.zero, 0, _layerMask);
        if (hits.Length == 1 && hits[0].collider != null && ((hits[0].collider.transform.root.TryGetComponent(out Arena arena) && arena.IsCompleted) || hits[0].collider.CompareTag("StartRoom")))
        {
            _circleIcon.color = Color.green;
        }
        else
            _circleIcon.color = Color.red;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _coroutine = StartCoroutine(CircleRotation());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator CircleRotation()
    {
        while(true)
        {
            _circleIcon.rectTransform.Rotate(Vector3.forward * 50f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDisable() => _minimap.OnOpenMinimap += EnableImage;
}
