using Managers;
using Player.Controls;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Enemy.Arena;

public class Minimap : Singleton<Minimap>, IPointerClickHandler
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Image _test;
    private Vector2 _world;
    [Space]
    [SerializeField] private RectTransform _centerMinimapRect;
    [SerializeField] private RectTransform _outlineRect;
    [Space]
    [SerializeField] private RectTransform _minimapRectImage;
    [SerializeField] private RectTransform _bigMinimapSize;
    [Space]
    [SerializeField] private float _speed;
    [SerializeField] private float _offset = 1f;
    private InputReader _inputReader;
    [Space]
    #region variables
    private Vector2 _defaultImageSize;
    private Vector2 _defaultRectPosition;
    private Vector2 _defaultRectSize;
    private Vector2 _scaleRatio;
    private float _mapSize;
    #endregion variables

    [Header("Agents")]
    [SerializeField] private MinimapIcon _minimapIcon;
    private MinimapWorldAgent _playerAgent;
    private MinimapIcon _playerIcon;
    [Space]
    [Header("Rendering")]
    [SerializeField] private CursorChanger _cursorChanger;
    [SerializeField] private LayerMask _cullingMask;     
    [SerializeField] private Image _minimapImage;

    public Action OnLoadedMinimap;
    private bool _activeCenterMimap = false;  
    private bool _isLoaded = false;

    private void Start()
    {
        _inputReader = InputReaderManager.Instance.GetInputReader();
        _inputReader.OpenMinimapEvent += OpenMinimap;
    }

    private void Update()
    {
        if (!_isLoaded)
            return; 

        UpdateMinimapIcons();
    }

    private void LateUpdate()
    {
        if (!_isLoaded || _activeCenterMimap)
            return;

        _minimapRectImage.anchoredPosition = (_playerIcon.RectTransform.anchoredPosition * _speed);
    }

    private void OpenMinimap()
    {
        _activeCenterMimap = !_activeCenterMimap;

        _outlineRect.anchoredPosition = _activeCenterMimap ? _centerMinimapRect.anchoredPosition : _defaultRectPosition;
        _outlineRect.sizeDelta = _activeCenterMimap ? _centerMinimapRect.sizeDelta : _defaultRectSize;
        _minimapRectImage.localScale = _activeCenterMimap ? _bigMinimapSize.localScale : _defaultImageSize; 

        if(_activeCenterMimap)
            _cursorChanger.SetCursor(CursorData.CursorType.Default);
        else
            _cursorChanger.SetCursor(CursorData.CursorType.Aim);
    }

    public void SetMinimap(float size) 
    {
        Sprite sprite =  CreateMinimapTextureUtils.GetMinimapSprite(size, _cullingMask);  
        _mapSize = size;
        _minimapImage.sprite = sprite;

        CalculateTransformationMatrix();
        _defaultRectPosition = _outlineRect.anchoredPosition;
        _defaultRectSize = _outlineRect.sizeDelta;
        _defaultImageSize = _minimapRectImage.localScale;

        OnLoadedMinimap?.Invoke();
        _isLoaded = true;
    }

    private void UpdateMinimapIcons()
    {
        var mapPosition = WorldToMapPosition(_playerAgent.transform.position);
        _playerIcon.RectTransform.anchoredPosition = mapPosition;
    }

    private Vector2 WorldToMapPosition(Vector3 worldPosition)
    {
        var position = new Vector2(worldPosition.x, (worldPosition.y * 1.777f));
        return position * _scaleRatio;
    }

    public void RegisterMinimapPlayer(MinimapWorldAgent minimapWorldAgent)
    {
        var minimapIcon = Instantiate(_minimapIcon);
        minimapIcon.transform.SetParent(_minimapRectImage);
        minimapIcon.SetIcon(minimapWorldAgent.Icon);
        minimapIcon.SetColor(minimapWorldAgent.IconColor);
        _playerAgent = minimapWorldAgent;
        _playerIcon = minimapIcon;
    }

    private void CalculateTransformationMatrix()
    {
        var minimapDimensions = _minimapRectImage.rect.size / _offset;  
        var mapDimensions = new Vector2(_mapSize, _mapSize);

        _scaleRatio = minimapDimensions / mapDimensions;
    }

    private void OnDisable() => _inputReader.OpenMinimapEvent -= OpenMinimap;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_minimapRectImage, eventData.position, eventData.pressEventCamera, out localPoint);
        var mousePosition = localPoint;
        Image go = Instantiate(_test);
        go.rectTransform.SetParent(_minimapImage.rectTransform);
        go.rectTransform.anchoredPosition = mousePosition;

        var worldPosition = new Vector2(mousePosition.x, mousePosition.y / 1.777f);
        worldPosition /= _scaleRatio;
        _world = worldPosition;

        RaycastHit2D hit =  Physics2D.BoxCast(worldPosition, Vector2.one, 0, Vector2.zero);
        if (hit.collider != null && hit.collider.transform.root.GetComponent<Arena>().IsCompleted)
        {
            Debug.Log(worldPosition);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_world, Vector2.one);
    }
}
