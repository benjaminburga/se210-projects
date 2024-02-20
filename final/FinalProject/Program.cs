using System;
using System.Collections.Generic;

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

public class Movie
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public DateTime ReleaseDate { get; set; }

    public Movie(string title, string genre, DateTime releaseDate)
    {
        Title = title;
        Genre = genre;
        ReleaseDate = releaseDate;
    }

    public string GetMovieDetails()
    {
        return $"Movie: {Title}, Genre: {Genre}, Release Date: {ReleaseDate.ToShortDateString()}";
    }
}

public class Booking
{
    private Dictionary<int, User> reservations = new Dictionary<int, User>();

    public bool ReserveSeat(User user, Movie movie, int seatNumber)
    {
        // Simulating seat reservation logic
        if (!reservations.ContainsKey(seatNumber))
        {
            reservations.Add(seatNumber, user);
            return true;
        }
        return false;
    }

    public string GenerateTicket()
    {
        // Simulating ticket generation logic
        return "Ticket: [Sample Ticket Information]";
    }

    public string ConfirmBooking()
    {
        // Simulating booking confirmation logic
        return "Booking Confirmed!";
    }
}

public class ProjectMilestone
{
    public static void Week1()
    {
        User user = new User("John Doe", new List<string> { "Action", "Drama" });
        Console.WriteLine(user.GetUserData());
    }

    public static void Week2()
    {
        Movie movie = new Movie("Inception", "Sci-Fi", new DateTime(2010, 7, 16));
        Console.WriteLine(movie.GetMovieDetails());
    }

    public static void Week3()
    {
        User user = new User("Alice Smith", new List<string> { "Comedy", "Adventure" });
        Movie movie = new Movie("The Dark Knight", "Action", new DateTime(2008, 7, 18));

        Booking bookingSystem = new Booking();
        bool reservationResult = bookingSystem.ReserveSeat(user, movie, 1);

        if (reservationResult)
        {
            Console.WriteLine("Seat reserved successfully!");
            Console.WriteLine(bookingSystem.GenerateTicket());
            Console.WriteLine($"Booking confirmed: {bookingSystem.ConfirmBooking()}");
        }
        else
        {
            Console.WriteLine("Seat reservation failed.");
        }
    }

    public static void Main()
    {
        Week1();
        Week2();
        Week3();
    }
}
