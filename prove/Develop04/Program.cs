using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        MindfulnessApp app = new MindfulnessApp();

        while (true)
        {
            Console.WriteLine("Mindfulness App Menu:");
            Console.WriteLine("1. Start Breathing Activity");
            Console.WriteLine("2. Start Reflecting Activity");
            Console.WriteLine("3. Start Listing Activity");
            Console.WriteLine("4. Quit");

            Console.Write("Enter your choice (1-4): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    app.StartBreathingActivity();
                    break;

                case "2":
                    app.StartReflectingActivity();
                    break;

                case "3":
                    app.StartListingActivity();
                    break;

                case "4":
                    Console.WriteLine("Exiting the Mindfulness App. Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                    break;
            }
        }
    }
}

class MindfulnessApp
{
    public void StartBreathingActivity()
    {
        BreathingActivity breathingActivity = new BreathingActivity();
        breathingActivity.Run();
    }

    public void StartReflectingActivity()
    {
        ReflectingActivity reflectingActivity = new ReflectingActivity();
        reflectingActivity.Run();
    }

    public void StartListingActivity()
    {
        ListingActivity listingActivity = new ListingActivity();
        listingActivity.Run();
    }
}

class Activity
{
    protected string _name;
    protected string _description;
    protected int _duration;

    public void DisplayStartingMessage()
    {
        Console.WriteLine($"Starting {_name} Activity: {_description}");
        Console.WriteLine("Get ready to begin...");
        ShowSpinner(3);
    }

    public void DisplayEndingMessage()
    {
        Console.WriteLine($"Congratulations! You've completed the {_name} Activity.");
        Console.WriteLine($"Total time: {_duration} seconds");
        ShowSpinner(3);
    }

    public void ShowSpinner(int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            Console.Write("-");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }

    public void ShowCountDown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write($"{i} ");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }
}

class BreathingActivity : Activity
{
    public BreathingActivity()
    {
        _name = "Breathing";
        _description = "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.";
    }

    public void Run()
    {
        Console.Write("Enter the duration for the Breathing Activity in seconds: ");
        if (int.TryParse(Console.ReadLine(), out _duration))
        {
            DisplayStartingMessage();

            for (int i = 0; i < _duration; i++)
            {
                Console.WriteLine("Breathe in...");
                ShowCountDown(4);

                Console.WriteLine("Breathe out...");
                ShowCountDown(4);
            }

            DisplayEndingMessage();
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }
}

class ReflectingActivity : Activity
{
    private List<string> _prompts;
    private List<string> _questions;

    public ReflectingActivity()
    {
        _name = "Reflecting";
        _description = "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.\nWhen you have the answer please press enter to continue to the next question.";

        _prompts = new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };
        _questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        };
    }

    public void Run()
    {
        Console.Write("Enter the duration for the Reflecting Activity in seconds: ");
        if (int.TryParse(Console.ReadLine(), out _duration))
        {
            DisplayStartingMessage();

            string prompt = GetRandomPrompt();
            Console.WriteLine($"Think about the following prompt: {prompt}");
            ShowSpinner(3);

            foreach (var question in _questions)
            {
                DisplayPrompt(question);
                Console.ReadLine(); 
            }

            DisplayEndingMessage();
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    private string GetRandomPrompt()
    {
        Random random = new Random();
        int index = random.Next(_prompts.Count);
        return _prompts[index];
    }

    private void DisplayPrompt(string question)
    {
        Console.WriteLine(question);
        ShowSpinner(3);
    }
}

class ListingActivity : Activity
{
    private int _count;
    private List<string> _prompts;

    public ListingActivity()
    {
        _name = "Listing";
        _description = "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.";
        Thread.Sleep(1000);


        _prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };
    }

    public void Run()
    {
        Console.Write("Enter the duration for the Listing Activity in seconds: ");
        if (int.TryParse(Console.ReadLine(), out _duration))
        {
            DisplayStartingMessage();

            string prompt = GetRandomPrompt();
            Console.WriteLine($"Think about the following prompt: {prompt}");
            ShowSpinner(3);

            GetListFromUser();

            DisplayEndingMessage();
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    private string GetRandomPrompt()
    {
        Random random = new Random();
        int index = random.Next(_prompts.Count);
        return _prompts[index];
    }

    private void GetListFromUser()
    {
        Console.WriteLine("Begin listing items. Press Enter after each item. Type 'done' when you're finished.");
        string input;
        do
        {
            Console.Write("Item: ");
            input = Console.ReadLine();

            if (input.ToLower() != "done")
            {
                _count++;
            }

        } while (input.ToLower() != "done");

        Console.WriteLine($"You listed {_count} items.");
        ShowSpinner(3);
    }
}
