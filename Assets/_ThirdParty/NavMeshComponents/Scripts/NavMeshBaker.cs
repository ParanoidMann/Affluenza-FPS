namespace UnityEngine.AI
{
	public class NavigationBaker : MonoBehaviour
	{
		[SerializeField]
		private NavMeshSurface[] _surfaces;
		[SerializeField]
		private Transform[] _objectsToRotate;

		private void Start()
		{
			foreach (Transform objectToRotate in _objectsToRotate)
			{
				objectToRotate.localRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
			}

			foreach (NavMeshSurface surface in _surfaces)
			{
				surface.BuildNavMesh();
			}
		}
	}
}