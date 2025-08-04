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

        store.OnTurnInfoChanged += OnTurnInfoChanged;

        field.Initialize(store);
    }

    private void Start()
    {
        store.Send(new LoadLevelAction("Green Hills"));
    }

    private void OnTurnInfoChanged()
    {
        store.Send(new HandleTurnChangeAction());
    }
}
