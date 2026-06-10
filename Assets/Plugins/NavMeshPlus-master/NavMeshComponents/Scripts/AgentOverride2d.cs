using UnityEngine;
using UnityEngine.AI;

namespace NavMeshPlus.Extensions
{
    public interface IAgentOverride
    {
        void UpdateAgent();
    }

    public class AgentDefaultOverride : IAgentOverride
    {
        public void UpdateAgent()
        {
        }
    }
    public class AgentOverride2d: MonoBehaviour
    {
        public NavMeshAgent Agent { get; private set; }
        public IAgentOverride agentOverride { get; set; }
        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;
        }

        private void Update()
        {
            agentOverride?.UpdateAgent();
        }
    }
}
