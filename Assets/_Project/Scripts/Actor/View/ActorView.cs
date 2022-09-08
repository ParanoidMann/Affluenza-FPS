using UnityEngine;
using UnityEngine.AI;

namespace ParanoidMann.Affluenza.Actor
{
	[RequireComponent(typeof(NavMeshAgent))]
	internal class ActorView : MonoBehaviour
	{
		[SerializeField]
		private NavMeshAgent _navMeshAgent;

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