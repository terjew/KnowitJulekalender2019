using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KnowitJulekalender16
{
    enum CrossingDirection
    {
        NorthEast,
        SouthEast
    }

    class Crossing
    {
        public CrossingDirection Direction { get; }
        public int Length { get; }
        public Crossing(CrossingDirection direction, int length)
        {
            Direction = direction;
            Length = length;
        }
    }

    class Regatta
    {
        Dictionary<CrossingDirection, Vec2i> Moves = new Dictionary<CrossingDirection, Vec2i>()
        {
            {CrossingDirection.NorthEast, new Vec2i(1, -1) },
            {CrossingDirection.SouthEast, new Vec2i(1, 1) },
        };

        Dictionary<CrossingDirection, Vec2i> RadarDirection = new Dictionary<CrossingDirection, Vec2i>()
        {
            {CrossingDirection.NorthEast, new Vec2i(0, -1) },
            {CrossingDirection.SouthEast, new Vec2i(0, 1) },
        };

        Dictionary<CrossingDirection, char> RouteSymbols = new Dictionary<CrossingDirection, char>()
        {
            {CrossingDirection.NorthEast, '/' },
            {CrossingDirection.SouthEast, '\\' },
        };



        char[,] map;
        int width;
        int height;

        Vec2i position;
        Vec2i radarDirection;
        CrossingDirection currentDirection = CrossingDirection.NorthEast;


        private bool IsShore(Vec2i pos)
        {
            if (pos.Y < 0) return true;
            if (pos.X >= width) return false;
            return map[pos.Y, pos.X] == '#';
        }

        public Regatta(string[] lines)
        {
            width = lines[0].Length;
            height = lines.Length;

            map = new char[height, width];

            for (int y = 0; y < height; y++)
            {
                var line = lines[y];
                for (int x = 0; x < width; x++)
                {
                    var c = line[x];
                    if (c == 'B')
                    {
                        position = new Vec2i(x, y);
                    }
                    map[y, x] = c;
                }
            }
            radarDirection = RadarDirection[currentDirection];
        }

        char[,] backbuffer;

        void PrintMap(int kryssinger, int distance)
        {
            int drawOffsetX = 0;
            int drawOffsetY = 0;

            if (position.X > (Console.WindowWidth / 2))
            {
                drawOffsetX = (position.X - Console.WindowWidth / 2);
            }

            var w = Console.WindowWidth;
            var h = Console.WindowHeight;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    char c;
                    var mapx = x + drawOffsetX;
                    var mapy = y + drawOffsetY;
                    if (mapx < 0 || mapx >= width || mapy < 0 || mapy >= height) c = ' ';
                    else c = map[mapy, mapx];

                    if (backbuffer[y,x] != c)
                    {
                        Console.CursorLeft = x;
                        Console.CursorTop = y;
                        Console.Write(c);
                        backbuffer[y, x] = c;
                    }
                }
            }
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.Write($"Kryssinger: {kryssinger}, avstand: {((distance > 20) ? ">20" : distance.ToString())}            ");
        }

        void InitializeConsole()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.WindowWidth = Math.Clamp(width, 40, 80);
            Console.WindowHeight = Math.Min(Math.Max(20, height + 1), Console.LargestWindowHeight);
            Console.BufferWidth = Console.WindowWidth + 1;
            Console.BufferHeight = Console.WindowHeight + 1;
            backbuffer = new char[Console.WindowHeight, Console.WindowWidth];
        }

        int Clearance()
        {
            for (int i = 0; i < 4; i++)
            {
                if (IsShore(position + (radarDirection * i))) return (i - 1) * 10;
            }
            return 30;
        }

        public List<Crossing> Sail()
        {

            var list = new List<Crossing>();
            Vec2i forward = new Vec2i(1, 0);

            InitializeConsole();
            PrintMap(list.Count + 1, Clearance());
            Console.ReadKey();

            while (position.X < width - 1)
            {
                var turnPosition = position;
                int count = 0;
                while (Clearance() > 20)
                {
                    count++;
                    position += Moves[currentDirection];
                    if (position.X < width) map[position.Y, position.X] = RouteSymbols[currentDirection];
                    PrintMap(list.Count + 1, Clearance());
                }
                list.Add(new Crossing(currentDirection, count));
                position += forward;
                currentDirection = currentDirection == CrossingDirection.NorthEast ? CrossingDirection.SouthEast : CrossingDirection.NorthEast;
                radarDirection = RadarDirection[currentDirection];
                if (position.X < width)
                {
                    map[position.Y, position.X] = RouteSymbols[currentDirection];
                    PrintMap(list.Count + 1, Clearance());
                }
            }
            PrintMap(list.Count, Clearance());
            if (Console.KeyAvailable)
            {
                Console.ReadKey();
                Console.ReadKey();
            }
            return list;
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
//            var input =
//@"
//####################
//#    ###             
//#                    
//#                    
//#                    
//#                    
//#                    
//#                    
//#B                   
//###    ####     #   
//####################".Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            var input = File.ReadAllLines("./fjord.txt");
            var regatta = new Regatta(input);

            var crossings = regatta.Sail();
            Console.ReadKey();
        }

    }
}
