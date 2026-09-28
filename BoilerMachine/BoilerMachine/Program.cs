using BoilerMachine.Models;
using BoilerMachine.Services;
using BoilerMachine.Views;

class Program
{
    public static async Task Main(string[] args)
    {
        Boiler boiler = new Boiler();
        BoilerService boilerService = new BoilerService(boiler);
        ConsoleUI consoleUI = new ConsoleUI(boilerService);
        boilerService.OnResetRequest += consoleUI.DisplayResetRequest;
        boilerService.OnStartCompletion += consoleUI.DisplayStartCompletion;
        boilerService.OnStartInterrupted += consoleUI.DisplayStartInterruption;
        await consoleUI.HandleUserChoice();
    }
}