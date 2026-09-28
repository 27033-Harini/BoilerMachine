using BoilerMachine.FileHandlers;
using BoilerMachine.Models;
using BoilerMachine.Repositories;
using BoilerMachine.Services;
using BoilerMachine.Views;

class Program
{
    public static async Task Main(string[] args)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
        Boiler boiler = new Boiler();
        CancellationTokenSource cts = new CancellationTokenSource();
        FileHandler fileHandler = new FileHandler(filePath);
        EventLogRepository logRepository = new EventLogRepository(fileHandler);
        BoilerService boilerService = new BoilerService(boiler, logRepository);
        ConsoleUI consoleUI = new ConsoleUI(boilerService);
        boilerService.OnResetRequest += ConsoleUI.DisplayResetRequest;
        boilerService.OnStartCompletion += ConsoleUI.DisplayStartCompletion;
        boilerService.OnStartInterrupted += ConsoleUI.DisplayStartInterruption;
        boilerService.OnAlreadyRunning += ConsoleUI.DisplayRunningMessage;
        boilerService.OnStart += consoleUI.DisplayStarted;
        consoleUI.HandleUserChoice();
    }
}