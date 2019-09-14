using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerRoomDetector : MonoBehaviour {
    private IInputProvider _inputProvider;

    [SerializeField] 
    private Transform _checkFrom;
    private Room _currentRoom;

    [Inject]
    private void Init(IInputProvider inputProvider) {
        _inputProvider = inputProvider;
    }

    private void Update() {
        if (_inputProvider.ProvidePrimaryActionInput())
            CheckIfIsInRoom();
    }

    private void CheckIfIsInRoom() {
        var didHit = Physics.Raycast(
            _checkFrom.position,
            Vector3.down,
            out var hit,
            1
        );
        
        if (!didHit)
            return;

        var room = hit.collider.GetComponent<Room>();
        _currentRoom = room;
    }
}