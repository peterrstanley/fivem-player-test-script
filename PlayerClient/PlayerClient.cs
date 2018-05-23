using System;
using NativeUI;
using System.Drawing;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using CitizenFX.Core.Native;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PlayerClient
{
    public class PlayerClient : BaseScript
    {
        public bool hasReceivedMotd = false;
        private Ped _ped => Game.PlayerPed;
        private Vehicle _vehicle => Game.PlayerPed.CurrentVehicle;
        private Vehicle _trailer;

        private Ped PlayerPed => Game.PlayerPed;
        private Vehicle CurrentVehicle => Game.PlayerPed.CurrentVehicle;

        private static string vehInspectionPropertyName = "_Inspection_Data";

        public PlayerClient()
        {
            EventHandlers["playerSpawned"] += new Action<dynamic>(OnPlayerSpawned);
            EventHandlers["sendMotd"] += new Action<string>(ReceivedMotd);
            EventHandlers["playerInVehicle"] += new Action<Int32, Int32, String>(OnPlayerInVehicle);

            Tick += OnTick;
        }

        private void OnPlayerInVehicle(Int32 vehicle, Int32 seat, String displayName)
        {
            //Screen.ShowNotification($"User: {player.ToString()}");
            Screen.ShowNotification($"User's Current Vehicle: {vehicle.ToString()}");
        }
        
        private enum InputGroups
        {

        }

        private enum Controls
        {
            
        }

        private async Task OnTick()
        {
            int trailerHandle = 0;
            List<VehicleClass> vehCommercialClass = new List<VehicleClass>
            {
                VehicleClass.Commercial,
                VehicleClass.Industrial,
                VehicleClass.Service,
                VehicleClass.Utility,
            };

            await Delay(0);

            if (Game.IsControlJustReleased(1, Control.InteractionMenu))
            {
                Screen.DisplayHelpTextThisFrame($"Clicked the button");
                Screen.ShowNotification($"Key was pressed...");

                //CODE_HUMAN_MEDIC_TIME_OF_DEATH
                if(API.IsPedActiveInScenario(_ped.Handle))
                {
                    API.ClearPedTasks(_ped.Handle);
                } else
                {
                    API.TaskStartScenarioInPlace(_ped.Handle, "CODE_HUMAN_MEDIC_TIME_OF_DEATH", 99999, true);
                }
            }

            //Screen.ShowNotification($"User's Current Vehicle 2: {_vehicle.Handle.ToString()}");
            // Check if player's current vehicle is null (it will be null if they are in a vehicle) and ensure their vehicle matches one of our vehCommericalClass'.
            if (_vehicle != null && vehCommercialClass.IndexOf(_vehicle.ClassType) != -1)
            {
                //Screen.DisplayHelpTextThisFrame($"User's Current Truck: {_vehicle.ClassType.ToString()}");
                // Ensure Currentvehicle is attached to a trailer, and if so get the trailer and ref/out the trailers handle.
                if (API.IsVehicleAttachedToTrailer(CurrentVehicle.Handle) &&
                    API.GetVehicleTrailerVehicle(CurrentVehicle.Handle, ref trailerHandle))
                {
                    // Create a new vehicle using trailer's handle.
                    _trailer = new Vehicle(trailerHandle);

                    // Detach the vehicle from the trailer.
                    //API.DetachVehicleFromTrailer(CurrentVehicle.Handle);
                    //Screen.ShowNotification($"User's Current Trailer: {_trailer.ClassType.ToString()}");
                    Screen.DisplayHelpTextThisFrame($"User's Current Trailer: {_trailer.ClassType.ToString()}");
                }
            }
        }

        private void OnPlayerSpawned(object obj)
        {
            TriggerServerEvent("onPlayerSpawned");
            Screen.ShowNotification($"Sending Trigger to Server: ");
        }

        private void ReceivedMotd(string motd)
        {
            if (!hasReceivedMotd)
            {
                TriggerEvent("chatMessage", "SYSTEM", new[] { 255, 0, 0 }, motd);
                Debug.WriteLine($"Received message from server: " + motd);
                hasReceivedMotd = true;
            }
        }
    }
}
