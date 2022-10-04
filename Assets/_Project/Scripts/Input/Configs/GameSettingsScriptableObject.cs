using UnityEngine;
using UnityEngine.Rendering;

namespace ParanoidMann.Affluenza.Input
{
	[CreateAssetMenu(fileName = "GameSettings", menuName = "Configs/Game Settings", order = 1)]
	internal class GameSettingsScriptableObject : ScriptableObject
	{
		[SerializeField]
		private RenderPipelineAsset _renderPipelineAsset;
		[SerializeField]
		private float _minVerticalCameraAngle = -90.0f;
		[SerializeField]
		private float _maxVerticalCameraAngle = 70.0f;

		public RenderPipelineAsset RenderPipelineAsset => _renderPipelineAsset;

		public float MinVerticalCameraAngle => _minVerticalCameraAngle;
		public float MaxVerticalCameraAngle => _maxVerticalCameraAngle;
	}
}