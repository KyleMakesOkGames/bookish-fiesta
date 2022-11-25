public enum AIStateID
{
    Idle,
    ChasePlayer,
    Death
}

public interface AIState
{
    AIStateID GetID();

    void Enter(AIAgent agent);
    void Update(AIAgent agent);
    void Exit(AIAgent agent);
}
