using BoilerMachine.FileHandlers;
using BoilerMachine.Models;
using BoilerMachine.Repositories;
using BoilerMachine.Services;
using BoilerMachine.Views;

/// <summary>
/// Class program to start up the application
/// </summary>
class Program
{
    /// <summary>
    /// Entry point of the program
    /// </summary>
    /// <param name="args">Command line arguments</param>
    public static void Main(string[] args)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
        Boiler boiler = new Boiler();
        CancellationTokenSource cts = new CancellationTokenSource();
        IFileHandler fileHandler = new FileHandler(filePath);
        IEventLogRepository logRepository = new EventLogRepository(fileHandler);
        IBoilerService boilerService = new BoilerService(boiler, logRepository);
        ConsoleUI consoleUI = new ConsoleUI(boilerService);
        boilerService.OnResetRequest += ConsoleUI.DisplayResetRequest;
        boilerService.OnStartCompletion += ConsoleUI.DisplayStartCompletion;
        boilerService.OnStartInterrupted += ConsoleUI.DisplayStartInterruption;
        boilerService.OnRunning += ConsoleUI.DisplayRunningMessage;
        boilerService.OnStart += consoleUI.DisplayStarted;
        consoleUI.HandleUserChoice();
    }
}