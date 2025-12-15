using UnityEngine;

public class GameShopState : State
{
    private float shopTime;               
    private GameManager _gameManager;
    GameObject shop;

    public GameShopState(StateMachine stateMachine) : base(stateMachine)
    {
        _gameManager = stateMachine as GameManager;
        shopTime = _gameManager.GetShopTime(); 
    }

    public override void Enter()
    {
        shop = GameObject.Instantiate(_gameManager.GetShopPrefab());
    }

    public override void Update()
    {
        shopTime -= Time.deltaTime;

        if (shopTime <= 0f)
        {
            shopTime = 0f;
            GameObject.Destroy(shop);
            _gameManager.SetState(new GameWaveState(_gameManager));
        }
    }

    public override void Exit()
    {
        
    }
}
