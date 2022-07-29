using UnityEngine;
using UnityEngine.AI;

// Walk to a random position and repeat
public class RandomWalking 
{
    //public float m_Range = 25.0f;
    NavMeshAgent m_Agent;

    public RandomWalking(NavMeshAgent navMeshAgent)
    {
        m_Agent = navMeshAgent;
    }

    public Vector3 UpdateRandomWalk()
    {
        if (m_Agent.pathPending || m_Agent.remainingDistance > 0.1f)
            return m_Agent.velocity;

        m_Agent.destination = Utilities.GetRandomLocation();
        return m_Agent.velocity;
    }
}
