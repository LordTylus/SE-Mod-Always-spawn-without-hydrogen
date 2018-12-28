﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using SpaceEngineers.Game.ModAPI;
using Sandbox.ModAPI;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;

namespace AlwaysToolRemover
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class Main : MySessionComponentBase
    {
        private Logging logger;

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            base.Init(sessionComponent);

            logger = Logging.Instance;
            
            logger.WriteLine("Initialized");

            /* Add Listener */
            MyVisualScriptLogicProvider.PlayerSpawned += PlayerSpawned;
        }

        protected override void UnloadData()
        {
            base.UnloadData();

            logger.WriteLine("Unloaded");

            MyVisualScriptLogicProvider.PlayerSpawned -= PlayerSpawned;
            logger.Close();
        }

        private void PlayerSpawned(long playerId)
        {
            //logger.WriteLine("Request of Player "+ playerId);

            IMyIdentity playerIdentity = Player(playerId);

            //logger.WriteLine("Found Identity " + playerId);

            if (playerIdentity != null)
            {
                //logger.WriteLine("Player is " + playerIdentity.DisplayName);

                var playerList = new List<IMyPlayer>();
                MyAPIGateway.Players.GetPlayers(playerList, p => p != null && p.IdentityId == playerIdentity.IdentityId);

                var player = playerList.FirstOrDefault();
                if (player != null)
                {
                    MyVisualScriptLogicProvider.SetPlayersHydrogenLevel(playerIdentity.IdentityId, 0);
                }
            }
        }

        private IMyIdentity Player(long entityId)
        {
            try
            {
                List<IMyIdentity> listIdentities = new List<IMyIdentity>();

                MyAPIGateway.Players.GetAllIdentites(listIdentities, 
                    p => p != null && p.DisplayName != "" && p.IdentityId == entityId);

                if (listIdentities.Count == 1)
                    return listIdentities[0];
                
                return null;
            }
            catch (Exception e)
            {
                logger.WriteLine("Error on getting Player Identity " + e);
                return null;
            }
        }
    }
}