using UnityEngine;

public class WaitforActionState : State
{
    // Called when the player picks a move (CombatManager raises this)
    private void OnPlayerMoveChosenHandler()
    {
        var cm = CombatManager.Instance;
        if (cm == null) return;

        // Build the turn queue and start processing turns
        cm.BuildTurnQueue();
        cm.PlayNextTurn();

        cm.OnPlayerMoveChosen -= OnPlayerMoveChosenHandler; // Unsubscribe immediately to prevent multiple triggers
        // Note: UI was already disabled when the player chose a move.
        // We'll re-enable UI when we re-enter this state (Enter).
    }

    public override void Enter()
    {
        Debug.Log("WaitforActionState: Enter - subscribing to OnPlayerMoveChosen and enabling UI.");

        var cm = CombatManager.Instance;
        //if (cm != null)
        //{
        //    cm.OnPlayerMoveChosen += OnPlayerMoveChosenHandler;
        //}

        // Ensure the move UI is enabled so the player can choose an action
        var movesUI = Object.FindFirstObjectByType<MovesUI>();
        if (movesUI != null)
        {
            movesUI.SetButtonsInteractable(true);
        }
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
       
    }

    public override void Update()
    {
       if(CombatManager.Instance.playerChosenMove != null)
        {
            CombatManager.Instance.BuildTurnQueue();
            CombatManager.Instance.PlayNextTurn();

        }
    }
}