using Managers;
using Player.Controls;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : Singleton<Minimap>
{
    [SerializeField] private RectTransform _centerMinimapRect;
    [SerializeField] private RectTransform _outlineRect;
    [Space]
    [SerializeField] private RectTransform _minimapRectImage;
    [SerializeField] private RectTransform _bigMinimapSize;
    [SerializeField] private Image _blackSquarePrefab;
    [SerializeField] private RectTransform _fogOfWarMask;
    [Space]
    [SerializeField] private float _speed;
    [SerializeField] private float _offset = 1f;
    private InputReader _inputReader;

    #region variables
    private Vector2 _defaultImageSize;
    private Vector2 _defaultRectPosition;
    private Vector2 _defaultRectSize;
    private Vector2 _defaultImagePosition;
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
    public Action<bool> OnOpenMinimap;
    private bool _activeCenterMimap = false;  
    private bool _isLoaded = false;

    public RectTransform MinimapRect => _minimapRectImage;
    public Vector2 ScaleRatio => _scaleRatio;


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

        _fogOfWarMask.anchoredPosition = (_playerIcon.RectTransform.anchoredPosition * _speed);
    }

    public void OpenMinimap()
    {
        _activeCenterMimap = !_activeCenterMimap;

        _outlineRect.anchoredPosition = _activeCenterMimap ? _centerMinimapRect.anchoredPosition : _defaultRectPosition;
        _outlineRect.sizeDelta = _activeCenterMimap ? _centerMinimapRect.sizeDelta : _defaultRectSize;
        _fogOfWarMask.localScale = _activeCenterMimap ? _bigMinimapSize.localScale : _defaultImageSize;
        _fogOfWarMask.anchoredPosition = _activeCenterMimap ? _defaultImagePosition : _minimapRectImage.anchoredPosition;

        if (_activeCenterMimap)
            _cursorChanger.SetCursor(CursorData.CursorType.Default);
        else
            _cursorChanger.SetCursor(CursorData.CursorType.Aim);

        OnOpenMinimap?.Invoke(_activeCenterMimap);
    }

    public void SetMinimap(float size) 
    {
        Sprite sprite =  CreateMinimapTextureUtils.GetMinimapSprite(size, _cullingMask);  
        _mapSize = size;
        _minimapImage.sprite = sprite;

        CalculateTransformationMatrix();
        _defaultRectPosition = _outlineRect.anchoredPosition;
        _defaultRectSize = _outlineRect.sizeDelta;
        _defaultImageSize = _fogOfWarMask.localScale;
        _defaultImagePosition = _minimapRectImage.anchoredPosition;

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

    public void FogOfWarForRoom(Vector2 worldPosition)
    {
        Image fog = Instantiate(_blackSquarePrefab);
        var position = WorldToMapPosition(worldPosition);
        fog.transform.SetParent(_minimapRectImage);
        fog.rectTransform.anchoredPosition = position;
    }

    private void OnDisable() => _inputReader.OpenMinimapEvent -= OpenMinimap;
}
