using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Field field;

    private GameStore store;

    private void Awake()
    {
        // Создаем пустое начальное состояние
        var initialFieldState = new FieldState(0, 0, new Dictionary<CellPosition, IEntity>());
        var initialState = new GameState(initialFieldState, 0);

        // Инициализируем store
        store = new GameStore(initialState);

        // Подписываемся на изменения состояния
        store.OnStateChanged += OnStateChanged;

        // Передаем store в Field
        field.Initialize(store);
    }

    private void Start()
    {
        // Загружаем уровень через действие
        store.Dispatch(new LoadLevelAction(1));
    }

    private void OnStateChanged(GameState state)
    {
        // Здесь можно добавить логику, реагирующую на изменение состояния
        // Например, проверку условий победы, сохранение прогресса и т.д.
    }
}
