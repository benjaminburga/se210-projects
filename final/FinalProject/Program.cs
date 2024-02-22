using System;
using System.Collections.Generic;
using System.Linq;

public class Movie
{
    public int MovieId { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public decimal TicketPrice { get; set; }

    public Movie(int movieId, string title, string genre, decimal ticketPrice)
    {
        MovieId = movieId;
        Title = title;
        Genre = genre;
        TicketPrice = ticketPrice;
    }

    public string GetDetails()
    {
        return $"Movie: {Title}, Genre: {Genre}, Ticket Price: {TicketPrice} USD";
    }
}

public class User
{
    public string Name { get; set; }
    public List<string> Preferences { get; set; }

    public User(string name, List<string> preferences)
    {
        Name = name;
        Preferences = preferences;
    }

    public string GetUserData()
    {
        return $"User: {Name}, Preferences: {string.Join(", ", Preferences)}";
    }
}

public class BookingSystem
{
    private List<Movie> movies = new List<Movie>();
    private Dictionary<int, User> reservations = new Dictionary<int, User>();
    private Dictionary<int, string> preferenceOptions = new Dictionary<int, string>();
    private User user = new User("", new List<string>());
    private decimal totalAmount = 0;

    public BookingSystem()
    {
        preferenceOptions.Add(1, "Action");
        preferenceOptions.Add(2, "Romance");
        preferenceOptions.Add(3, "Sci-Fi");
        preferenceOptions.Add(4, "Comedy");

        AddMovie(new Movie(1, "Deadpool", "Action", 8));
        AddMovie(new Movie(2, "John Wick", "Action", 8));
        AddMovie(new Movie(3, "The Notebook", "Romance", 8));
        AddMovie(new Movie(4, "Pride and Prejudice", "Romance", 8));
        AddMovie(new Movie(5, "Interstellar", "Sci-Fi", 8));
        AddMovie(new Movie(6, "Blade Runner", "Sci-Fi", 8));
        AddMovie(new Movie(7, "Dumb and Dumber", "Comedy", 8));
        AddMovie(new Movie(8, "Anchorman", "Comedy", 8));
    }

    public void AddMovie(Movie movie)
    {
        movies.Add(movie);
    }

   public void ReserveSeats(User user, Movie movie, int numberOfSeats)
{
    Console.WriteLine($"\nSeats reserved successfully for {user.Name} - {movie.Title}");

    int reservationKey = reservations.Count + 1;
    reservations.Add(reservationKey, user);
    Console.WriteLine("\n-----------------------\nReserving seats \n-------------------------");

    for (int i = 0; i < 3; i++) 
    {
        Console.Write(".");
        System.Threading.Thread.Sleep(1000); 
    }

    Console.WriteLine("\n------------------------");
    Console.WriteLine($"Number of seats: {numberOfSeats}");
    Console.WriteLine($"Subtotal: {movie.TicketPrice * numberOfSeats} USD");
    Console.WriteLine($"Ticket Numbers: {GenerateTicketNumbers(numberOfSeats)}");
    Console.WriteLine($"Total Amount to Pay: {totalAmount} USD");
    Console.WriteLine("Thank you for using the Online Movie Booking System!");
}


    private string GenerateTicketNumbers(int numberOfSeats)
    {
        List<string> ticketNumbers = new List<string>();

        for (int i = 1; i <= numberOfSeats; i++)
        {
            ticketNumbers.Add($"FCFA{i + reservations.Count}");
        }

        return string.Join(", ", ticketNumbers);
    }

    public void WelcomeMessage()
    {
        Console.WriteLine("Welcome to the Online Movie Booking System!");

        Console.Write("Enter your name: ");
        user.Name = Console.ReadLine();
        Console.WriteLine($"\nWelcome {user.Name}!");

        Console.WriteLine("Select your preferences:");

        foreach (var option in preferenceOptions)
        {
            var genre = movies.FirstOrDefault(m => m.Genre == option.Value);
            if (genre != null)
            {
                Console.WriteLine($"{option.Key}. {option.Value} - Ticket Price: {genre.TicketPrice} USD");
            }
        }

        Console.Write("Enter your choice: ");
        if (int.TryParse(Console.ReadLine(), out int preferenceChoice) && preferenceOptions.ContainsKey(preferenceChoice))
        {
            user.Preferences = new List<string> { preferenceOptions[preferenceChoice] };

            bool explorePreferences = true;

            while (explorePreferences)
            {
                var genre = movies.FirstOrDefault(m => m.Genre == user.Preferences.First());

                if (genre != null)
                {
                    Console.WriteLine($"We have these movies for you based on your preferences ({user.Preferences.First()}):");

                    int movieIndex = 1;
                    foreach (var movie in movies.Where(m => m.Genre == user.Preferences.First()))
                    {
                        Console.WriteLine($"{movieIndex}. {movie.GetDetails()}");
                        movieIndex++;
                    }

                    Console.WriteLine("Would you like to explore other preferences?");
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");

                    Console.Write("Enter your choice: ");
                    if (int.TryParse(Console.ReadLine(), out int exploreChoice) && exploreChoice == 1)
                    {
                        Console.WriteLine($"Select your preferences:");

                        foreach (var option in preferenceOptions)
                        {
                            var movieGenre = movies.FirstOrDefault(m => m.Genre == option.Value);
                            if (movieGenre != null)
                            {
                                Console.WriteLine($"{option.Key}. {option.Value} - Ticket Price: {movieGenre.TicketPrice} USD");
                            }
                        }

                        Console.Write("Enter your choice: ");
                        if (int.TryParse(Console.ReadLine(), out preferenceChoice) && preferenceOptions.ContainsKey(preferenceChoice))
                        {
                            user.Preferences = new List<string> { preferenceOptions[preferenceChoice] };
                        }
                        else
                        {
                            explorePreferences = false;
                        }
                    }
                    else
                    {
                        explorePreferences = false;

                        Console.Write("Enter the number of the movie you want to watch or press enter to exit: ");
                        if (int.TryParse(Console.ReadLine(), out int selectedMovieNumber) &&
                            selectedMovieNumber > 0 && selectedMovieNumber <= movies.Count)
                        {
                            Console.Write("Enter the number of seats you want to reserve: ");
                            if (int.TryParse(Console.ReadLine(), out int numberOfSeats) && numberOfSeats > 0)
                            {
                                totalAmount = genre.TicketPrice * numberOfSeats;
                                ReserveSeats(user, genre, numberOfSeats);
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid number of seats.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid movie number.");
                        }
                    }
                }
                else
                {
                    explorePreferences = false;
                    Console.WriteLine("Invalid input. Please enter a valid preference number.");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid preference number.");
        }
    }
}

public class Program
{
    public static void Main()
    {
        BookingSystem bookingSystem = new BookingSystem();
        bookingSystem.WelcomeMessage();
    }
}
