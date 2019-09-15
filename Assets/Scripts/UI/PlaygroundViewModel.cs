using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Observer = UniRx.Observer;

public class PlaygroundViewModel : MonoBehaviour {
    [SerializeField] private PlayerRoomDetector _roomDetector;
    [SerializeField] private PlayerInteractionDetector _interactionDetector;
    [SerializeField] private Goal _mainGoal;

    public string keycapSpriteName;
    
    public IObservable<bool> ShouldShowLocation { get; private set; }
    public IObservable<string> CurrentRoomUpperTitle { get; private set; }
    public IObservable<string> CurrentRoomTitle { get; private set; }
    
    public IObservable<bool> ShouldShowInteraction { get; private set; }
    public IObservable<string> CurrentInteractionUpperTitle { get; private set; }
    public IObservable<string> CurrentInteractionTitle { get; private set; }
    public IObservable<string> CurrentGoalTitle { get; private set; }
    public IObservable<Unit> OnGoalFinish { get; private set; }
    
    private void Awake() {
        ShouldShowLocation = _roomDetector.Room
            .StartWith((Room) null)
            .Select(room => room != null);

        CurrentRoomUpperTitle = _roomDetector.Room
            .Select(
                room => room == null
                    ? null
                    : $"Você está {room.preposition}"
            );

        CurrentRoomTitle = _roomDetector.Room
            .Select(
                room => room == null 
                    ? null 
                    : $"{room.title}"
            );

        ShouldShowInteraction = _interactionDetector.CurrentInteractable
            .StartWith((Interactable) null)
            .Select(interactable => interactable != null);
        
        CurrentInteractionUpperTitle = _interactionDetector.CurrentInteractable
            .Select(
                interactable => interactable == null 
                    ? null 
                    : $"Pressione <sprite name={keycapSpriteName}> para"
            );

        CurrentInteractionTitle = _interactionDetector.CurrentInteractable
            .Select(
                interactable => interactable == null
                    ? null
                    : interactable.actionTitle
            );

        CurrentGoalTitle = Observable.Return(_mainGoal.goalTitle);
        
        OnGoalFinish = _mainGoal.OnFinish;
    }

    private void OnRoomChange(Room room) {
        if (room == null)
            return;
                    
        Debug.Log($"Você está {room.preposition} {room.title}");
    }

    private void OnInteractableChange(Interactable interactable) {
        if (interactable == null)
            return;
        
        Debug.Log($"Pressione E para {interactable.actionTitle}");
    }

    private void OnGoalFinished(Unit _) {
        Debug.Log("Goal finished!");
    }
}
