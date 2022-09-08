using UnityEngine;
using UnityEngine.AI;

namespace ParanoidMann.Affluenza.TestBox
{
	[RequireComponent(typeof(NavMeshSurface))]
	internal class TestBoxGeometryView : MonoBehaviour
	{
		[SerializeField]
		private Transform _spawnPoint;
		[SerializeField]
		private NavMeshSurface _surface;

		public Transform SpawnPoint => _spawnPoint;
		public NavMeshSurface Surface => _surface;

		private void OnValidate()
		{
			if (_surface == null)
			{
				_surface = GetComponent<NavMeshSurface>();
			}
		}
	}
}