using BoilerMachine.Enums;
using BoilerMachine.Services;
using System.Threading.Channels;

namespace BoilerMachine.Views
{
    public class ConsoleUI
    {
        private readonly BoilerService _boilerService;
        public ConsoleUI(BoilerService boilerService)
        {
            this._boilerService = boilerService;
        }
        public static void DisplayMenu()
        {
            Console.WriteLine("Boiler Controller Initialized");
            Console.WriteLine("1. Toggle Run Interlock Switch");
            Console.WriteLine("2. Reset lockout");
            Console.WriteLine("Enter your choice: ");
        }

        public void HandleUserChoice()
        {
            while (true)
            {
                Console.Clear();
                DisplayMenu();
                switch (Console.ReadLine()?.Trim())
                {
                    case "1":
                        this.DisplayToggleRequest();
                        Console.ReadKey();
                        break;
                    case "2":
                        this.DisplayResetOperations();
                        Console.ReadKey();
                        break;
                }
            }
        }

        public void DisplayStopRequestToToggle()
        {
            Console.WriteLine("The current sequence is running and hence it should be stopped in order to toggle the interlock switch.");
        }

        public void DisplayToggleRequest()
        {
            Console.WriteLine($"The current state of the toggle: {this._boilerService.GetCurrentToggleStatus()}");
            Console.WriteLine("Toggling the switch...");
            if (this._boilerService.Toggle())
            {
                Console.WriteLine($"The current state of the toggle: {this._boilerService.GetCurrentToggleStatus()}");
            }
        }

        public void DisplayResetOperations()
        {
            InterlockSwitchStatus status = this._boilerService.GetCurrentToggleStatus();
            bool isStatusUpdated = this._boilerService.UpdateSystemStatus();
            if (!isStatusUpdated)
            {
                Console.WriteLine("The interlock switch needs to be closed first in order for a reset to occur.");
            }
            else
            {
                Console.WriteLine("Reset done successfully.");
                Console.WriteLine($"System Status: {this._boilerService.GetSystemStatus()}");
                Console.WriteLine($"Interlock Switch Status: {this._boilerService.GetCurrentToggleStatus()}");
            }
        }
    }
}
