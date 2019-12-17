using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace KnowitJulekalender13
{
   
    class Program
    {

        static void Main(string[] args)
        {

            string path = Path.GetFullPath("./maze.txt.zip");
            var maze = Maze.FromZip(path);

            var visitedArthur = maze.Solve(Maze.strategyArthur).Count();
            Console.WriteLine($"Arthur solved the maze visiting {visitedArthur} distinct rooms");

            var visitedIsaac = maze.Solve(Maze.strategyIsaac).Count();
            Console.WriteLine($"Isaac solved the maze visiting {visitedIsaac} distinct rooms");

            Console.WriteLine($"Difference in visited rooms is {Math.Abs(visitedArthur - visitedIsaac)}");
        }
    }

    public enum Direction
    {
        North,
        West,
        South,
        East
    }

    public class Neighbor
    {
        public Direction Direction { get; }
        public Room Room { get; }

        public Neighbor(Direction direction, Room room)
        {
            Direction = direction;
            Room = room;
        }

    }
    public class Room
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Nord { get; set; }
        public bool Vest { get; set; }
        public bool Syd { get; set; }
        public bool Aust { get; set; }

        public IEnumerable<Neighbor> GetNeighbors(Room[][] rooms)
        {
            if (!Nord) yield return new Neighbor(Direction.North, rooms[Y - 1][X]);
            if (!Vest) yield return new Neighbor(Direction.West, rooms[Y][X - 1]);
            if (!Syd)  yield return new Neighbor(Direction.South, rooms[Y + 1][X]);
            if (!Aust) yield return new Neighbor(Direction.East, rooms[Y][X + 1]);
        }
    }

    public class Maze
    {
        public Room[][] Rooms { get; }
        public int Width;
        public int Height;

        public static Maze FromZip(string path)
        {
            using (var archive = ZipFile.OpenRead(path))
            {
                foreach (var entry in archive.Entries)
                {
                    return new Maze(entry.Open());
                }
            }
            return null;
        }

        public Maze(Stream stream)
        {
            string json = null;
            using (var reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();              
            }
            Rooms = JsonConvert.DeserializeObject<Room[][]>(json);
            Width = Rooms[0].Length;
            Height = Rooms.Length;
        }

        public delegate Room Strategy(Room[][] rooms, HashSet<Room> visitedRooms, Room from);

        public static Strategy strategyArthur = (rooms, visitedRooms, from) =>
        {
            var neighbors = from.GetNeighbors(rooms);
            var unvisited = neighbors.Where(n => !visitedRooms.Contains(n.Room));
            if (!unvisited.Any()) return null;
            var ordered = unvisited
                .OrderByDescending(n => n.Direction == Direction.South)
                .ThenByDescending(n => n.Direction == Direction.East)
                .ThenByDescending(n => n.Direction == Direction.West)
                .ThenByDescending(n => n.Direction == Direction.North);
            return ordered.First().Room;
        };

        public static Strategy strategyIsaac = (rooms, visitedRooms, from) =>
        {
            var neighbors = from.GetNeighbors(rooms);
            var unvisited = neighbors.Where(n => !visitedRooms.Contains(n.Room));
            if (!unvisited.Any()) return null;
            var ordered = unvisited
                .OrderByDescending(n => n.Direction == Direction.East)
                .ThenByDescending(n => n.Direction == Direction.South)
                .ThenByDescending(n => n.Direction == Direction.West)
                .ThenByDescending(n => n.Direction == Direction.North);
            return ordered.First().Room;
        };

        public HashSet<Room> Solve(Strategy strategy)
        {
            var visitedRooms = new HashSet<Room>();
            var path = new Stack<Room>();

            var currentRoom = Rooms[0][0];
            path.Push(currentRoom);
            visitedRooms.Add(currentRoom);

            while (currentRoom != Rooms[Width - 1][Height - 1])
            {
                var nextRoom = strategy(Rooms, visitedRooms, currentRoom);
                if (nextRoom == null)
                {
                    currentRoom = path.Pop();
                    //Console.WriteLine($"Backtracked to {currentRoom.X},{currentRoom.Y}");
                }
                else
                {
                    path.Push(currentRoom);
                    visitedRooms.Add(nextRoom);
                    currentRoom = nextRoom;
                    //Console.WriteLine($"Moved to {currentRoom.X},{currentRoom.Y}");
                }
            }
            return visitedRooms;
        }
    }
}
