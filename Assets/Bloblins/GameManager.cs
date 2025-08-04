using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Field field;

    private GameStore store;

    private void Awake()
    {
        var initialState = new GameState(null, null, null);

        store = new GameStore(initialState);

        field.Initialize(store);
    }

    private void Start()
    {
        store.Send(new LoadLevelAction("Green Hills"));
    }
}
