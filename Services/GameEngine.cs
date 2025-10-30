using Microsoft.EntityFrameworkCore;
using W9_assignment_template.Data;
using W9_assignment_template.Models;

namespace W9_assignment_template.Services;

public class GameEngine
{
    private readonly GameContext _context;

    public GameEngine(GameContext context)
    {
        _context = context;
    }

    public void DisplayRooms()
    {
        var rooms = _context.Rooms.Include(r => r.Characters).ToList();

        foreach (var room in rooms)
        {
            Console.WriteLine($"Room: {room.Name} - {room.Description}");
            foreach (var character in room.Characters)
            {
                Console.WriteLine($"    Character: {character.Name}, Level: {character.Level}");
            }
        }
    }

    public void DisplayCharacters()
    {
        var characters = _context.Characters.ToList();
        if (characters.Any())
        {
            Console.WriteLine("\nCharacters found:");
            foreach (var character in characters)
            {
                Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
            }
        }
        else
        {
            Console.WriteLine("No characters available.");
        }
    }

    public void FindCharacter()
    {
        Console.Write("Enter character name to search: ");
        var name = Console.ReadLine();

        var searchResults = _context.Characters.Where(character => character.Name.ToLower().Contains(name.ToLower()));

        if (searchResults.Any())
        {
            Console.WriteLine("\nCharacters found:");
            foreach (var result in searchResults)
            {
                Console.WriteLine($"Character ID: {result.Id}, Name: {result.Name}, Level: {result.Level}, Room ID: {result.RoomId}");
            }
        }
        else
        {
            Console.WriteLine("No characters available.");
        }
    }

    public void AddCharacter()
    {
        Console.Write("Enter character name: ");
        var name = Console.ReadLine();

        Console.Write("Enter character level: ");
        var level = int.Parse(Console.ReadLine());

        Console.Write("Enter room ID for the character: ");
        var roomId = int.Parse(Console.ReadLine());

        var foundRoom = _context.Rooms.Where(room => room.Id.Equals(roomId)).Count() > 0;

        if (foundRoom)
        {
            var character = new Character { Name = name, Level = level, RoomId = roomId };
            _context.Characters.Add(character);
            _context.SaveChanges();
            Console.WriteLine($"Character {name} has been created and bound to room {roomId}");
        }
        else
        {
            Console.WriteLine($"Couldn't find a room with ID: {roomId}");
            return;
        }
    }

    public void AddRoom() {
        Console.Write("Enter room name: ");
        var name = Console.ReadLine();

        Console.Write("Enter room description: ");
        var description = Console.ReadLine();

        var room = new Room
        {
            Name = name,
            Description = description
        };

        _context.Rooms.Add(room);
        _context.SaveChanges();

        Console.WriteLine($"Room '{name}' added to the game.");
    }
}