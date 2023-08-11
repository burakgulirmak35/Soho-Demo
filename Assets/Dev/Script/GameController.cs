using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    private enum GameState
    {
        Placement,
        Battle,
        GameOver
    }
    [SerializeField] private BattleShipSO[] battleShipsSO;

    private GameState gameState;
    private int placedShipsCount;
    private int cellCount;
    private int mapSize = 8;
    private int[] placement;

    private void Awake()
    {
        Instance = this;
        cellCount = mapSize * mapSize;
    }

    private void Start()
    {
        BeginShipPlacement();
        UIManager.Instance.Rotate += Event_RotateShip;
    }

    #region  GameState
    private void BeginShipPlacement()
    {
        gameState = GameState.Placement;
        placedShipsCount = 0;
        Map.Instance.SetMapState(MapState.Placement);
        UpdateCursor();
        UIManager.Instance.MessageText("Place Your Ships");
    }

    private void WaitForOpponentPlacement()
    {
        gameState = GameState.Placement;
        Map.Instance.SetMapState(MapState.Disabled);
        UIManager.Instance.MessageText("Enemy Turn");
    }

    private void StartTurn()
    {
        gameState = GameState.Battle;
        Map.Instance.SetMapState(MapState.Attack);
        UIManager.Instance.MessageText("Your Turn");
    }

    private void WaitForOpponentTurn()
    {
        gameState = GameState.Battle;
        Map.Instance.SetMapState(MapState.Disabled);
        UIManager.Instance.MessageText("Enemy Turn");
    }

    private void ShowResult()
    {
        gameState = GameState.GameOver;
        Map.Instance.SetMapState(MapState.Disabled);
        UIManager.Instance.MessageText("Game Over");
    }
    #endregion

    #region UIEvents

    public void PlaceShip(Vector3Int coordinate)
    {
        if (!gameState.Equals(GameState.Placement)) { return; }

        int size = Map.Instance.currentBattleShip.ShipSize;
        int shipWidth = shipWidth = _placeShipHorizontally ? size : 1;
        int shipHeight = shipHeight = _placeShipHorizontally ? 1 : size;

        if (coordinate.x < 0 || coordinate.x + (shipWidth - 1) >= mapSize || coordinate.y - (shipHeight - 1) < 0)
        {
            return;
        }

        Map.Instance.SetShip(coordinate, _placeShipHorizontally);
    }

    #endregion
    #region SeciliGemi ve Rotasyonu
    private bool _placeShipHorizontally;
    private bool PlaceShipHorizontally
    {
        get { return _placeShipHorizontally; }
        set { _placeShipHorizontally = value; UpdateCursor(); }
    }
    private void UpdateCursor()
    {
        Map.Instance.SetShipCursor(battleShipsSO[placedShipsCount], PlaceShipHorizontally);
    }
    private void Event_RotateShip(object sender, System.EventArgs e)
    {
        PlaceShipHorizontally = !PlaceShipHorizontally;
    }
    #endregion

}
