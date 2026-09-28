using BoilerMachine.Models;
using BoilerMachine.Services;
using BoilerMachine.Views;

class Program
{
    public static async Task Main(string[] args)
    {
        Boiler boiler = new Boiler();
        CancellationTokenSource cts = new CancellationTokenSource();
        BoilerService boilerService = new BoilerService(boiler, cts);
        ConsoleUI consoleUI = new ConsoleUI(boilerService);
        boilerService.OnResetRequest += consoleUI.DisplayResetRequest;
        boilerService.OnStartCompletion += consoleUI.DisplayStartCompletion;
        boilerService.OnStartInterrupted += consoleUI.DisplayStartInterruption;
        boilerService.OnAlreadyRunning += consoleUI.DisplayRunningMessage;
        await consoleUI.HandleUserChoice();
    }
}