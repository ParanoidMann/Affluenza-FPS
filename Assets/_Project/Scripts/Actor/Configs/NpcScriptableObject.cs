using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	[CreateAssetMenu(fileName = "NPC", menuName = "Configs/Actor/NPC", order = 3)]
	internal class NpcScriptableObject : ActorScriptableObject
	{
		[Header("NPC")]
		[SerializeField]
		private NpcSubType _subType;

		public override ActorType ActorType => ActorType.Npc;
		public override int ActorSubType => (int)_subType;
	}
}