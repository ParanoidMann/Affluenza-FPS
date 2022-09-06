using UnityEngine;

namespace ParanoidMann.Affluenza.TestBox
{
	internal class TestBoxGeometryView : MonoBehaviour
	{
		[SerializeField]
		private Transform _spawnPoint;

		public Transform SpawnPoint => _spawnPoint;
	}
}