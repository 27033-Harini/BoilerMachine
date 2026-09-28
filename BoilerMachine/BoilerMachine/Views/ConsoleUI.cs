using BoilerMachine.Enums;
using BoilerMachine.Models;
using BoilerMachine.Services;

namespace BoilerMachine.Views
{
    /// <summary>
    /// Class to handle printing and input obtaining operations
    /// </summary>
    public class ConsoleUI
    {
        private readonly IBoilerService _boilerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleUI"/> class.
        /// </summary>
        /// <param name="boilerService">Service to handle boiler operations</param>
        public ConsoleUI(IBoilerService boilerService)
        {
            this._boilerService = boilerService;
        }

        /// <summary>
        /// Displays the main meny for the user
        /// </summary>
        public static void DisplayMenu()
        {
            Console.WriteLine(new string('-', 19));
            Console.WriteLine("Welcome! Boiler Controller Initialized!");
            Console.WriteLine(new string('-', 19));
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

        /// <summary>
        /// Handles the input entered by the user.
        /// </summary>
        public void HandleUserChoice()
        {
            while (true)
            {
                Console.Clear();
                DisplayMenu();
                DisplaySegregationLines();
                DisplayEventsHeader();
                Console.SetCursorPosition(0, 13);
                string? input = Console.ReadLine();
                if (!Enum.TryParse(input, out MainMenuOptions option) || !Enum.IsDefined(option))
                {
                    DisplayInvalidInputMessage();
                    Pause();
                    continue;
                }
                switch (option)
                {
                    case MainMenuOptions.Start:
                        this.Start();
                        Pause();
                        break;
                    case MainMenuOptions.Stop:
                        this.DisplayStopRequest();
                        Pause();
                        break;
                    case MainMenuOptions.SimulateError:
                        this.SimulateError();
                        Pause();
                        break;
                    case MainMenuOptions.Toggle:
                        this.DisplayToggleState();
                        Pause();
                        break;
                    case MainMenuOptions.Reset:
                        this.DisplayResetOperations();
                        Pause();
                        break;
                    case MainMenuOptions.EventLog:
                        this.DisplayEventLogs();
                        Pause();
                        break;
                    case MainMenuOptions.Exit:
                        this.DisplayExitMessage();
                        return;
                    default:
                        this.DisplayInvalidInputMessage();
                        break;
                }
            }
        }

        /// <summary>
        /// Seperates the screen into two.
        /// </summary>
        public void DisplaySegregationLines()
        {
            for(int y = 0; y < 30; y++)
            {
                Console.SetCursorPosition(59, y);
                Console.WriteLine("|");
            }
        }

        public void ClearSpecificLeftArea()
        {
            for (int y = 13; y < 30; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.WriteLine(new string(' ', 59));
            }
        }

        /// <summary>
        /// Displays the exiting message to user
        /// </summary>
        public void DisplayExitMessage()
        {
            Console.WriteLine("Exiting...");
        }

        /// <summary>
        /// Display the input is invalid to the user.
        /// </summary>
        public void DisplayInvalidInputMessage()
        {
            Console.WriteLine("The input is invalid. Should be between 1 - 7");
        }

        /// <summary>
        /// Displays the event logs from the log.txt file if it exists else no log found displayed.
        /// </summary>
        public void DisplayEventLogs()
        {
            IReadOnlyList<EventLog> logs = this._boilerService.GetAll();
            int index = 1;
            foreach (var log in logs)
            {
                Console.WriteLine($"{index++}. {log.TimeStamp}\t{log.Event}\t{log.EventData}");
            }

            if (index == 1)
            {
                Console.WriteLine("No event logs found.");
            }
        }

        /// <summary>
        /// Displays the stop request message when tried to toggle upon boiler being in progress.
        /// </summary>
        public void DisplayStopRequestToToggle()
        {
            Console.SetCursorPosition(60, 3);
            Console.WriteLine("The current sequence is running and hence it should ");
            Console.SetCursorPosition(60, 4);
            Console.WriteLine("be stopped in order to toggle the interlock switch.");
        }

        /// <summary>
        /// Displays the toggle state before and after switch. If error, it is printed as a message.
        /// </summary>
        public void DisplayToggleState()
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

        /// <summary>
        /// Display the result of the reset operation
        /// </summary>
        public void DisplayResetOperations()
        {
            Console.SetCursorPosition(60, 15);
            InterlockSwitchStatus status = this._boilerService.GetCurrentToggleStatus();
            bool isStatusUpdated = this._boilerService.ResetMachine();
            if (!isStatusUpdated)
            {
                Console.WriteLine("* The interlock switch needs to be closed");
                Console.SetCursorPosition(60, 16);
                Console.WriteLine(" first in order for a reset to occur.");
            }
            else
            {
                Console.SetCursorPosition(60, 17);
                Console.WriteLine("Reset done successfully.");
                Console.SetCursorPosition(60, 18);
                Console.WriteLine($"System Status: {this._boilerService.GetSystemStatus()}");
                Console.SetCursorPosition(60, 19);
                Console.WriteLine($"Interlock Switch Status: {this._boilerService.GetCurrentToggleStatus()}");
            }
        }

        /// <summary>
        /// Display that the sequence has started
        /// </summary>
        public void DisplayStarted()
        {
            Console.SetCursorPosition(60, 5);
            Console.WriteLine("* The boiler sequence has started.");
        }

        /// <summary>
        /// Start the sequence in service layer.
        /// </summary>
        public void Start()
        {
            _ = this._boilerService.StartSequence();
        }

        /// <summary>
        /// Display the header for events
        /// </summary>
        public void DisplayEventsHeader()
        {
            Console.SetCursorPosition(60, 0);
            Console.WriteLine(new string('-', 40));
            Console.SetCursorPosition(60, 1);
            Console.WriteLine("Actions occured are displayed below: ");
            Console.SetCursorPosition(60, 2);
            Console.WriteLine(new string('-', 40));
        }

        /// <summary>
        /// Displays the result of the stopsequence
        /// </summary>
        public void DisplayStopRequest()
        {
            bool result = this._boilerService.StopSequence();
            if (result)
            {
                Console.SetCursorPosition(60, 6);
                Console.WriteLine("The start sequence is stopped.");
            }
            else
            {
                Console.SetCursorPosition(60, 6);
                Console.WriteLine("The start sequence is not running to be stopped.");
            }
        }

        /// <summary>
        /// Displays the start sequence being interrupted
        /// </summary>
        public static void DisplayStartInterruption()
        {
            Console.SetCursorPosition(60, 7);
            Console.WriteLine("* The start operation was interrupted due to cancellation");
        }

        /// <summary>
        /// Displays the reset request before starting a sequence
        /// </summary>
        public static void DisplayResetRequest()
        {
            Console.SetCursorPosition(60, 8);
            Console.WriteLine("* The system needs to be reset since starting");
            Console.SetCursorPosition(60, 9);
            Console.WriteLine("  requires the status to be ready.");
        }

        /// <summary>
        /// Display the completion of the start sequence
        /// </summary>
        public static void DisplayStartCompletion()
        {
            Console.SetCursorPosition(60, 10);
            Console.WriteLine("* Start operation is completed successfully.");
        }

        /// <summary>
        /// Displays that the start process is running
        /// </summary>
        public static void DisplayRunningMessage()
        {
            Console.SetCursorPosition(60, 11);
            Console.WriteLine("* The start sequence is already running.");
        }

        /// <summary>
        /// Simulates the error - If System in Operable status -> SystemStatus updated to LockOut
        /// </summary>
        public void SimulateError()
        {
            bool result = this._boilerService.SimulateError();
            if (result)
            {
                Console.SetCursorPosition(60, 12);
                Console.WriteLine("Error: System is in lockout.");
            }
            else
            {
                Console.SetCursorPosition(60, 12);
                Console.WriteLine("The system should be in Operable");
                Console.SetCursorPosition(60, 13);
                Console.WriteLine("status in order to simulate an error");
            }
        }

        /// <summary>
        /// Pauses the screen to get input from user
        /// </summary>
        public static void Pause()
        {
            Console.SetCursorPosition(0, 14);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
