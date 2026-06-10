using _Project.WorldClicking.Scripts;
using NavMeshPlus.Extensions;
using UnityEngine;

namespace _Project.Main.Animations
{
    public class AiAnimationsController : AnimationsController
    {
        [SerializeField] private AgentOverride2d navMeshAgent;
        
        private Vector2 _currentDirection;
        private IAttackable _attackable;
        private IDestroyable _destroyable;
        
        private void Update()
        {
            var targetPosition = (Vector2)navMeshAgent.Agent.destination;
            var currentSpeed = navMeshAgent.Agent.velocity.magnitude;
            
            UpdateSpeed(currentSpeed);
            
            if (_attackable != null && !_attackable.IsKilled())
            {
                _currentDirection = (_attackable.GetPosition() - (Vector2)transform.position).normalized;
                UpdateDirection(_currentDirection.x);
                return;
            }
            
            if (_destroyable != null)
            {
                _currentDirection = (_destroyable.GetPosition() - (Vector2)transform.position).normalized;
                UpdateDirection(_currentDirection.x);
                return;
            }
            
            if (currentSpeed > 0.01f)
            {
                _currentDirection = (targetPosition - (Vector2)transform.position).normalized;
                UpdateDirection(_currentDirection.x);
            }
        }

        public void SetAttackable(IAttackable attackable)
        {
            _attackable = attackable;
        }

        public void RemovAttackable()
        {
            _attackable = null;
        }
        
        public void SetDestroyable(IDestroyable destroyable)
        {
            _destroyable = destroyable;
        }

        public void RemoveDestroyable()
        {
            _destroyable = null;
        }
    }
}