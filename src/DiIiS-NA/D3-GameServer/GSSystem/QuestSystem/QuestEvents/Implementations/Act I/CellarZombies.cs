﻿using DiIiS_NA.Core.Logging;
using DiIiS_NA.GameServer.GSSystem.ActorSystem;
using DiIiS_NA.GameServer.GSSystem.ActorSystem.Implementations.Hirelings;
using DiIiS_NA.GameServer.GSSystem.GameSystem;
using DiIiS_NA.GameServer.GSSystem.PlayerSystem;
using DiIiS_NA.GameServer.MessageSystem;
using System.Linq;
using System;
using System.Collections.Generic;
using DiIiS_NA.LoginServer.AccountsSystem;
using DiIiS_NA.GameServer.GSSystem.QuestSystem.QuestEvents;
using DiIiS_NA.GameServer.Core.Types.Math;
using DiIiS_NA.Core.Helpers.Math;
using DiIiS_NA.GameServer.Core.Types.TagMap;
using DiIiS_NA.GameServer.MessageSystem.Message.Definitions.Animation;
using DiIiS_NA.D3_GameServer.Core.Types.SNO;

namespace DiIiS_NA.GameServer.GSSystem.QuestSystem.QuestEvents.Implementations
{
	class CellarZombies : QuestEvent
	{
		//ActorID: 0x7A3100DD  
		//ZombieSkinny_A_LeahInn.acr (2050031837)
		//ActorSNOId: 0x00031971:ZombieSkinny_A_LeahInn.acr

		//private static readonly Logger Logger = LogManager.CreateLogger();

		private static readonly ActorSno[] spawners = new ActorSno[]
		{
			ActorSno._omninpc_tristram_male_b_blacksmith,
			ActorSno._omninpc_tristram_male_e_blacksmith,
			ActorSno._omninpc_tristram_male_d_blacksmith,
			ActorSno._omninpc_tristram_male_c_blacksmith,
			ActorSno._omninpc_tristram_male_f_blacksmith,
			ActorSno._omninpc_tristram_male_g_blacksmith,
			ActorSno._omninpc_tristram_male_f_blacksmith,
			ActorSno._omninpc_tristram_male_a_blacksmith
		};

		public CellarZombies()
			: base(151123)
		{
		}

		public override void Execute(MapSystem.World world)
		{
			if (world.Game.Empty) return;

			List<Actor> actorstotarget = new List<Actor> { };

			foreach(var sno in spawners)
            {
				var spawner = world.GetActorBySNO(sno);
				while (spawner != null)
				{
					actorstotarget.Add(world.SpawnMonster(ActorSno._zombieskinny_a_leahinn, spawner.Position));
					spawner.Destroy();
					spawner = world.GetActorBySNO(sno);
				}
			}

			foreach (var actor in actorstotarget)
			{
				actor.Attributes[GameAttribute.Quest_Monster] = true;
				actor.Attributes.BroadcastChangedIfRevealed();
			}
			StartConversation(world, 131339);
		}

		private bool StartConversation(MapSystem.World world, Int32 conversationId)
		{
			foreach (var player in world.Players)
			{
				player.Value.Conversations.StartConversation(conversationId);
			}
			return true;
		}
	}
}
