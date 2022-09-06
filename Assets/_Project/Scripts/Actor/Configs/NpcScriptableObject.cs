using UnityEngine;

namespace ParanoidMann.Affluenza.Actor
{
	[CreateAssetMenu(fileName = "NPC", menuName = "Configs/Actor/NPC", order = 3)]
	public class NpcScriptableObject : ActorScriptableObject
	{
		internal override ActorType ActorType => ActorType.Npc;
	}
}