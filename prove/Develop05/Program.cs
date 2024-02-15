using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

abstract class Goal
{
    private string _shortName;
    protected string _description;
    protected int _points;

    public Goal(string shortName, string description, int points)
    {
        _shortName = shortName;
        _description = description;
        _points = points;
    }

    public string ShortName { get { return _shortName; } }
    public int Points { get { return _points; } }

    public abstract void RecordEvent();
    public abstract bool IsComplete();
    public abstract string GetDetailsString();
    public abstract string GetStringRepresentation();
}

class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string shortName, string description, int points)
        : base(shortName, description, points)
    {
        _isComplete = false;
    }

    public override void RecordEvent()
    {
        _isComplete = true;
    }

    public override bool IsComplete()
    {
        return _isComplete;
    }

    public override string GetDetailsString()
    {
        return $"{ShortName} - {_description} ({(_isComplete ? "Completed" : "Not Completed")})";
    }

    public override string GetStringRepresentation()
    {
        return $"SimpleGoal:{ShortName},{_description},{_points},{(_isComplete ? 1 : 0)}";
    }
}

class EternalGoal : Goal
{
    public EternalGoal(string shortName, string description, int points)
        : base(shortName, description, points)
    {
        // No hay estado específico para EternalGoal
    }

    public override void RecordEvent()
    {
        // No hay eventos específicos para EternalGoal
    }

    public override bool IsComplete()
    {
        // Las metas eternas nunca se completan
        return false;
    }

    public override string GetDetailsString()
    {
        return $"{ShortName} - {_description} (Eternal)";
    }

    public override string GetStringRepresentation()
    {
        return $"EternalGoal:{ShortName},{_description},{_points}";
    }
}

class ChecklistGoal : Goal
{
    private int _amountCompleted;
    private int _target;
    private int _bonus;

    public ChecklistGoal(string shortName, string description, int points, int target, int bonus)
        : base(shortName, description, points)
    {
        _amountCompleted = 0;
        _target = target;
        _bonus = bonus;
    }

    // Agrega este constructor adicional
    public ChecklistGoal(string shortName, string description, int points, int target, int bonus, int amountCompleted)
        : this(shortName, description, points, target, bonus)
    {
        _amountCompleted = amountCompleted;
    }

    public override void RecordEvent()
    {
        _amountCompleted++;
        if (_amountCompleted >= _target)
        {
            // Aplicar bonificación al completar la lista de verificación
            // Se debe cambiar score a una variable global o pasarlo como parámetro
            // en función de tu implementación
            // score += _bonus;
        }
    }

    public override bool IsComplete()
    {
        return _amountCompleted >= _target;
    }

    public override string GetDetailsString()
    {
        return $"{ShortName} - {_description} (Completed {_amountCompleted}/{_target} times)";
    }

    public override string GetStringRepresentation()
    {
        return $"ChecklistGoal:{ShortName},{_description},{_points},{_target},{_bonus},{_amountCompleted}";
    }
}

class GoalManager
{
    private List<Goal> _goals;
    private int _score;

    public GoalManager()
    {
        _goals = new List<Goal>();
        _score = 0;
    }

    public void Start()
    {
        // Implementa la lógica del programa aquí
        LoadGoals();
        MenuLoop();
    }

    private void DisplayPlayerInfo()
    {
        Console.WriteLine($"You have {_score} points.");
    }

    private void ListGoalNames()
    {
        Console.WriteLine("The goals are:");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].ShortName}");
        }
    }

    private void ListGoalDetails()
    {
        Console.WriteLine("The types of Goals are:");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
        }
    }

    private void CreateGoal()
    {
        Console.WriteLine("Select the type of goal:");
        Console.WriteLine("1. Simple Goal");
        Console.WriteLine("2. Eternal Goal");
        Console.WriteLine("3. Checklist Goal");

        int choice = GetUserChoice(1, 3);

        Console.Write("Enter the short name of the goal: ");
        string shortName = Console.ReadLine();

        Console.Write("Enter the description of the goal: ");
        string description = Console.ReadLine();

        Console.Write("Enter the points for the goal: ");
        int points = GetUserInput<int>();

        switch (choice)
        {
            case 1:
                _goals.Add(new SimpleGoal(shortName, description, points));
                break;
            case 2:
                _goals.Add(new EternalGoal(shortName, description, points));
                break;
            case 3:
                Console.Write("Enter the target for the checklist goal: ");
                int target = GetUserInput<int>();
                Console.Write("Enter the bonus for completing the checklist: ");
                int bonus = GetUserInput<int>();
                _goals.Add(new ChecklistGoal(shortName, description, points, target, bonus));
                break;
            default:
                break;
        }
    }

    private void RecordEvent()
    {
        Console.WriteLine("Select a goal to record an event for:");
        ListGoalNames();

        int choice = GetUserChoice(1, _goals.Count);

        Goal selectedGoal = _goals[choice - 1];
        selectedGoal.RecordEvent();

        Console.WriteLine($"Event recorded for {selectedGoal.ShortName}!");
    }

    private void SaveGoals()
    {
        using (StreamWriter writer = new StreamWriter("goals.txt"))
        {
            foreach (Goal goal in _goals)
            {
                writer.WriteLine(goal.GetStringRepresentation());
            }
        }

        Console.WriteLine("Goals have been saved successfully!");
    }

    private void LoadGoals()
    {
        if (File.Exists("goals.txt"))
        {
            _goals.Clear();

            using (StreamReader reader = new StreamReader("goals.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Goal loadedGoal = CreateGoalFromString(line);
                    if (loadedGoal != null)
                    {
                        _goals.Add(loadedGoal);
                    }
                }
            }

            Console.WriteLine("Goals have been loaded successfully!");
        }
    }

    private void MenuLoop()
    {
        int choice;

        do
        {
            Console.WriteLine("Menu Options:");
            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. List Goals");
            Console.WriteLine("3. Save Goals");
            Console.WriteLine("4. Load Goals");
            Console.WriteLine("5. Record Event");
            Console.WriteLine("6. Quit");

            Console.Write("Select a choice from the menu: ");
            choice = GetUserChoice(1, 6);

            switch (choice)
            {
                case 1:
                    CreateGoal();
                    break;
                case 2:
                    ListGoalDetails();
                    break;
                case 3:
                    SaveGoals();
                    break;
                case 4:
                    LoadGoals();
                    break;
                case 5:
                    RecordEvent();
                    break;
                case 6:
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    break;
            }

        } while (choice != 6);
    }

    private int GetUserChoice(int min, int max)
    {
        int choice;

        while (!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max)
        {
            Console.Write($"Invalid input. Please enter a number between {min} and {max}: ");
        }

        return choice;
    }

    private T GetUserInput<T>()
    {
        while (true)
        {
            try
            {
                return (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
            }
            catch (Exception)
            {
                Console.Write("Invalid input. Please try again: ");
            }
        }
    }

    private Goal CreateGoalFromString(string goalString)
    {
        string[] parts = goalString.Split(':');
        if (parts.Length == 2)
        {
            string[] details = parts[1].Split(',');

            if (details.Length > 2)
            {
                string type = details[0];
                string shortName = details[1];
                string description = details[2];
                int points = int.Parse(details[3]);

                switch (type)
                {
                    case "SimpleGoal":
                        return new SimpleGoal(shortName, description, points);
                    case "EternalGoal":
                        return new EternalGoal(shortName, description, points);
                    case "ChecklistGoal":
                        int target = int.Parse(details[4]);
                        int bonus = int.Parse(details[5]);
                        int amountCompleted = int.Parse(details[6]);
                        return new ChecklistGoal(shortName, description, points, target, bonus, amountCompleted);
                    default:
                        break;
                }
            }
        }

        return null;
    }
}

class Program
{
    static void Main()
    {
        GoalManager goalManager = new GoalManager();
        goalManager.Start();
    }
}
