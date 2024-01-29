using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    
    public static void Main()
    {       
        do
        {
            RunGame();
            Console.WriteLine("\nDo you want to play again? (Type 'yes' to play again, or any other input to exit.)");

        } while (Console.ReadLine().ToLower() == "yes");
    }

    public static void RunGame()
    

    {
        Reference firstScripture = new Reference("1 Nephi", 3, 7);
        Scripture scripture1 = new Scripture(firstScripture, "And it came to pass that I, Nephi, said unto my father: I will go and do the things which the Lord hath commanded, for I know that the Lord giveth no commandments unto the children of men, save he shall prepare a way for them that they may accomplish the thing which he commandeth them.");

        Reference secondScripture = new Reference("2 Nephi", 31, 20);
        Scripture scripture2 = new Scripture(secondScripture, "Wherefore, ye must press forward with a asteadfastness in Christ, having a perfect brightness of bhope, and a clove of God and of all men. Wherefore, if ye shall press forward, feasting upon the word of Christ, and fendure to the end, behold, thus saith the Father: Ye shall have eeternal life.");

        Reference thirdScripture = new Reference("Proverbs", 3, 5, 6);
        Scripture scripture3 = new Scripture(thirdScripture, "Trust in the Lord with all your heart, and do not lean on your own understanding. In all your ways acknowledge him, and he will make straight your paths.");

        List<Scripture> scriptures = new List<Scripture> { scripture1, scripture2, scripture3 };

        foreach (var scripture in scriptures)
        {
            while (!scripture.IsCompletelyHidden())
            {
                Console.Clear();
                Console.WriteLine(scripture.GetDisplayText());
                Console.WriteLine("\nPress Enter to hide more words or type 'quit' to exit.");

                string userInput = Console.ReadLine();
                if (userInput.ToLower() == "quit")
                {
                    return;
                }

                scripture.HideRandomWords(3);
            }
        }

        Console.WriteLine("\nCongratulations! You have successfully memorized the scriptures. Well done!");
    }
}

public class Reference
{
    private string _book;
    private int _chapter;
    private int _verse;
    private int _endVerse;

    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _verse = verse;
        _endVerse = verse;
    }

    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _verse = startVerse;
        _endVerse = endVerse;
    }

    public string GetDisplayText()
    {
        return $"{_book} {_chapter}:{_verse}";
    }
}

public class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public void Show()
    {
        _isHidden = false;
    }

    public bool IsHidden()
    {
        return _isHidden;
    }

    public string GetDisplayText()
    {
        return _isHidden ? "_____ " : $"{_text} ";
    }
}

public class Scripture
{
    private Reference _reference;
    private List<Word> _words;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public void HideRandomWords(int numberToHide)
    {
        var random = new Random();
        var wordsToHide = _words.Where(word => !word.IsHidden()).OrderBy(x => random.Next()).Take(numberToHide);

        foreach (var word in wordsToHide)
        {
            word.Hide();
        }
    }

    public bool IsCompletelyHidden()
    {
        return _words.All(word => word.IsHidden());
    }

    public string GetDisplayText()
    {
        return $"{_reference.GetDisplayText()}\n\n{_words.Select(word => word.GetDisplayText()).Aggregate((current, next) => current + next)}";
    }
}
