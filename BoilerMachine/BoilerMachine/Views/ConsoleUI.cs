using BoilerMachine.Enums;
using BoilerMachine.Models;
using BoilerMachine.Services;
using System.Threading.Channels;
using System.Threading.Tasks;

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
            Console.WriteLine(new string('-', 19));
            Console.WriteLine("Welcome! Boiler Controller Initialized!");
            Console.WriteLine("1. Start Boiler Sequence");
            Console.WriteLine("2. Stop Boiler Sequence");
            Console.WriteLine("3. Simulate Boiler Error");
            Console.WriteLine("4. Toggle Run Interlock Switch");
            Console.WriteLine("5. Reset lockout");
            Console.WriteLine("6. View Event log");
            Console.WriteLine("7. Exit Application");
            Console.WriteLine(new string('-', 19));
            Console.WriteLine("Enter your choice: ");
            Console.WriteLine(new string('-', 19));
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
                        this.Start();
                        Console.ReadKey();
                        break;
                    case "2":
                        this._boilerService.StopSequence();
                        Console.ReadKey();
                        break;
                    case "3":
                        this.SimulateError();
                        Console.ReadKey();
                        break;
                    case "4":
                        this.DisplayToggleRequest();
                        Console.ReadKey();
                        break;
                    case "5":
                        this.DisplayResetOperations();
                        Console.ReadKey();
                        break;
                    case "6":
                        this.DisplayEventLogs();
                        Console.ReadKey();
                        break;
                    case "7":
                        this.DisplayExitMessage();
                        return;
                    default:
                        this.DisplayInvalidInputMessage();
                        break;
                }
            }
        }

        public void DisplayExitMessage()
        {
            Console.WriteLine("Exiting...");
        }

        public void DisplayInvalidInputMessage()
        {
            Console.WriteLine("The input is invalid. Should be between 1 - 7");
        }

        public void DisplayEventLogs()
        {
            var logs = this._boilerService.GetAll();
            foreach (var log in logs)
            {
                Console.WriteLine($"{log.TimeStamp}\t{log.Event}\t{log.EventData}");
            }
        }

        public void DisplayStopRequestToToggle()
        {
            Console.SetCursorPosition(60, 1);
            Console.WriteLine("The current sequence is running and hence it should be stopped in order to toggle the interlock switch.");
        }

        public void DisplayToggleRequest()
        {
            Console.WriteLine($"The current state of the toggle: {this._boilerService.GetCurrentToggleStatus()}");
            Console.WriteLine("Toggling the switch...");
            OperationResult toggleResult = this._boilerService.Toggle();
            if (toggleResult.IsOperationCompleted)
            {
                Console.WriteLine("The toggle is updated.");
                Console.WriteLine($"The current state of the toggle: {this._boilerService.GetCurrentToggleStatus()}");
            }
            else
            {
                Console.WriteLine($"{toggleResult.ErrorMessage}");
            }
        }

        public void DisplayResetOperations()
        {
            InterlockSwitchStatus status = this._boilerService.GetCurrentToggleStatus();
            bool isStatusUpdated = this._boilerService.ResetMachine();
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
        public void DisplayStarted()
        {
            Console.SetCursorPosition(60, 5);
            Console.WriteLine("The boiler sequence has started.");
        }
        public void Start()
        {
            _ = this._boilerService.StartSequence();
        }

        public void DisplayStopRequest()
        {
            bool result = this._boilerService.StopSequence();
            if (result)
            {
                Console.WriteLine("The start sequence is stopped.");
            }
            else
            {
                Console.WriteLine("The start sequence is not running to be stopped.");
            }
        }

        public static void DisplayStartInterruption()
        {
            Console.SetCursorPosition(60, 7);
            Console.WriteLine("The start operation was interrupted due to cancellation");
        }

        public static void DisplayResetRequest()
        {
            Console.SetCursorPosition(60, 9);
            Console.WriteLine("The system needs to be reset since starting requires the status to be ready.");
        }

        public static void DisplayStartCompletion()
        {
            Console.SetCursorPosition(60, 12);
            Console.WriteLine("Start operation is completed successfully.");
        }

        public static void DisplayRunningMessage()
        {
            Console.WriteLine("The start sequence is already running.");
        }

        public void SimulateError()
        {
            bool result = this._boilerService.SimulateError();
            if (result)
            {
                Console.WriteLine("Error: System is in lockout.");
            }
            else
            {
                Console.WriteLine("The system should be in Operable status in order to smulate an error");
            }
        }

        public static void Pause()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
