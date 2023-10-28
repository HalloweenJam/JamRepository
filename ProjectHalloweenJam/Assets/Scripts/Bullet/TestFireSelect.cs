/*
using Player.Controls;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Weapons;

public class TestFireSelect : MonoBehaviour
{
    public CreateFire fire;

    private int _selectedWeaponIndex = 0;

    Vector3 _worldMousePosition;
    private InputReader _inputReader;

    TextMeshProUGUI _textMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        _textMeshPro.SetText(_selectedWeaponIndex.ToString());
        _inputReader = InputReaderManager.Instance.GetInputReader();
        _inputReader.MouseWheelScrollEvent += ChangeWeapon;
        _inputReader.MousePosition += MousePosition;
        _inputReader.ShootingEvent += ShootingEvent;
    }


    private void ChangeWeapon(float direction)
    {
        _selectedWeaponIndex += direction > 0 ? 1 : -1;
        _textMeshPro.SetText(_selectedWeaponIndex.ToString());
    }

    private void ShootingEvent()
    {
        fire.OnShooting(_selectedWeaponIndex, new Vector3(0, 0, 0), _worldMousePosition);
    }

    private void MousePosition(Vector2 vector)
    {
        _worldMousePosition = Camera.main.ScreenToWorldPoint(vector);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
*/
