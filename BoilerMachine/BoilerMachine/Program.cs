using BoilerMachine.Models;
using BoilerMachine.Services;
using BoilerMachine.Views;

class Program
{
    public static void Main(string[] args)
    {
        Boiler boiler = new Boiler();
        BoilerService boilerService = new BoilerService(boiler);
        ConsoleUI consoleUI = new ConsoleUI(boilerService);
        consoleUI.HandleUserChoice();
    }
}