using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Observer = UniRx.Observer;

public class CourseState : MonoBehaviour {
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _exitButton;

    [SerializeField] private PlayerRoomDetector _roomDetector;
    [SerializeField] private PlayerInteractionDetector _interactionDetector;
    [SerializeField] private Goal _mainGoal;

    public string keycapSpriteName;
    public string buttonSpriteName;

    public IObservable<string> CurrentInputType { get; private set; }

    public IObservable<Unit> OnMainMenuClick => _mainMenuButton.OnClickAsObservable();
    public IObservable<Unit> OnExitClick => _exitButton.OnClickAsObservable();

    public IObservable<bool> ShouldShowLocation { get; private set; }
    public IObservable<string> CurrentRoomUpperTitle { get; private set; }
    public IObservable<string> CurrentRoomTitle { get; private set; }

    public IObservable<bool> ShouldShowInteraction { get; private set; }
    public IObservable<string> CurrentInteractionUpperTitle { get; private set; }
    public IObservable<string> CurrentInteractionTitle { get; private set; }
    public IObservable<string> CurrentGoalTitle { get; private set; }
    public IObservable<Unit> OnGoalStart { get; private set; }
    public IObservable<Unit> OnGoalFinish { get; private set; }

    [Inject]
    private void Inject(IInputProvider inputProvider) {
        CurrentInputType = inputProvider.CurrentInputType();
    }

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

        var currentInteractableAndCurrentInputType = Observable.CombineLatest(
            _interactionDetector.CurrentInteractable.Select(interectable => interectable == null ? null : ""),
            CurrentInputType.Select(inputType => inputType == "mouse" ? keycapSpriteName : buttonSpriteName)
        );

        CurrentInteractionUpperTitle = currentInteractableAndCurrentInputType
            .Select(
                pair => pair[0] == null
                    ? null
                    : $"Pressione <sprite name={pair[1]}> para"
            );
        //CurrentInteractionUpperTitle = _interactionDetector.CurrentInteractable
        //    .Select(
        //        interactable => interactable == null
        //            ? null
        //            : $"Pressione <sprite name={keycapSpriteName}> para"
        //    );

        CurrentInteractionTitle = _interactionDetector.CurrentInteractable
            .Select(
                interactable => interactable == null
                    ? null
                    : interactable.actionTitle
            );

        CurrentGoalTitle = Observable.Return(_mainGoal.goalTitle);

        OnGoalStart = _mainGoal.OnStart;
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
