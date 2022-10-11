using System;

namespace ParanoidMann.Affluenza.Actor
{
	[Serializable]
	public enum ActorType
	{
		Player = 0,
		Npc = 1,
		Enemy = 2
	}

	[Serializable]
	public enum PlayerSubType
	{
		Player = 0,
		PlayerWithWeapon = 1
	}

	[Serializable]
	public enum EnemySubType
	{
		Enemy = 0
	}

	[Serializable]
	public enum NpcSubType
	{
		Npc = 0
	}
}