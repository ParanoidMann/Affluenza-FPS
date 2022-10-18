using UnityEngine;
using UnityEngine.AI;

namespace ParanoidMann.Affluenza.Actor
{
	[RequireComponent(typeof(NavMeshAgent))]
	internal class ActorView : MonoBehaviour
	{
		[Header("Actor")]
		[SerializeField]
		private Animator _animator;
		[SerializeField]
		private NavMeshAgent _navMeshAgent;

		public Animator Animator => _animator;
		public NavMeshAgent NavMeshAgent => _navMeshAgent;

		private void OnValidate()
		{
			if (_navMeshAgent == null)
			{
				_navMeshAgent = GetComponent<NavMeshAgent>();
			}
		}
	}
}