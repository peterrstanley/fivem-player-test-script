using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PlayerServer
{
    public class PlayerServer : BaseScript
    {
        public static string Motd = "Cake is a lie";

        public PlayerServer()
        {
            EventHandlers["PlayerConnecting"] += new Action<Player, string, CallbackDelegate>(OnPlayerConnecting);
            EventHandlers["player:onPlayerSpawned"] += new Action<Player, string, CallbackDelegate>(OnPlayerSpawned);
            EventHandlers["baseevents:enteredVehicle"] += new Action<Object, Int32, String, CallbackDelegate>(OnPlayerEnteredVehicle);
        }

        private void OnPlayerSpawned([FromSource]Player player, string playerName, CallbackDelegate kickReason)
        {
            TriggerClientEvent(player, "sendMotd", Motd);
            Debug.WriteLine($"{playerName} sending message to the client ");
        }

        private void OnPlayerConnecting([FromSource]Player player, string playerName, CallbackDelegate kickReason)
        {
            TriggerClientEvent(player, "sendMotd", Motd);
            Debug.WriteLine($"{playerName} sending message to the client: Connecting ");
        }

        private void OnPlayerEnteredVehicle(Object vehicle, Int32 seat, String displayName, CallbackDelegate kickReason)
        {

            //Debug.WriteLine($"{player.ToString()} Entered vehicle ");
            Debug.WriteLine($"{vehicle.ToString()} Vehicle ");
            Debug.WriteLine($"{displayName} test ");
            TriggerClientEvent("playerInVehicle", vehicle, seat, displayName);
        }
    }
}
