using Managers;
using Player.Controls;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Minimap : Singleton<Minimap>
{
    [SerializeField] private RectTransform _centerMinimapRect;
    [SerializeField] private RectTransform _outlineRect;

    [SerializeField] private RectTransform _minimapRectImage;
    [SerializeField] private RectTransform _bigMinimapSize;

    [SerializeField] private float _speed;
    [SerializeField] private float _offset = 1f;
    private InputReader _inputReader;

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

    [Header("Rendering")]
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
}
