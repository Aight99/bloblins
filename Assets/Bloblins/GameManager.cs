using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Field field;

    private GameStore store;

    private void Awake()
    {
        var initialState = new GameState();

        store = new GameStore(initialState);

        store.OnStateChanged += OnStateChanged;

        field.Initialize(store);
    }

    private void Start()
    {
        store.Send(new LoadLevelAction("Green Hills"));
    }

    private void OnStateChanged(GameState state)
    {
        // Здесь можно добавить логику, реагирующую на изменение состояния
        // Например, проверку условий победы, сохранение прогресса и т.д.
    }
}
