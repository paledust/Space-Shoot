using System;
using System.Collections.Generic;

namespace SimpleManager
{
    /////////////////////////////////////////////////
    // BASE CLASSES
    /////////////////////////////////////////////////
    public interface IManaged
    {
        void OnCreated();
        void OnDestroyed();
    }

    public abstract class Manager<T> where T : IManaged
    {
        protected readonly List<T> ManagedObjects = new List<T>();

        public abstract T Create();

        public abstract void Destroy(T o);

        public T Find(Predicate<T> predicate)
        {
            return ManagedObjects.Find(predicate);
        }

        public List<T> FindAll(Predicate<T> predicate)
        {
            return ManagedObjects.FindAll(predicate);
        }
    }


    /////////////////////////////////////////////////
    // GEMS
    /////////////////////////////////////////////////
    public enum GemColor
    {
        Red,
        Green,
        Blue
    }

    public class Gem : IManaged
    {
        public GemColor Color { get; private set; }

        public void Init(GemColor color)
        {
            Color = color;
        }

        public void OnCreated()
        {
            Console.WriteLine("Created " + Color + " gem");
        }

        public void OnDestroyed()
        {
            Console.WriteLine("Destroyed " + Color + " gem");
        }
    }

    public class Gems : Manager<Gem>
    {
        private static readonly Array Colors = Enum.GetValues(typeof(GemColor));
        private readonly Random _rng = new Random();
        
        public override Gem Create()
        {
            var gem = new Gem();
            gem.Init(GetRandomColor());
            ManagedObjects.Add(gem);
            gem.OnCreated();
            return gem;
        }

        public override void Destroy(Gem gem)
        {
            ManagedObjects.Remove(gem);
            gem.OnDestroyed();
        }

        public List<Gem> Create(uint n)
        {
            var gems = new List<Gem>();
            for (var i = 0; i < n; i++)
            {
                gems.Add(Create());
            }
            return gems;
        }

        private GemColor GetRandomColor()
        {
            return (GemColor)(_rng.Next() % Colors.Length);
        }

        public void DestroyAllWithColor(GemColor color)
        {
            foreach (var gem in FindAll(g => g.Color == color))
            {
                Destroy(gem);
            }
        }

        public int NumWithColor(GemColor color)
        {
            return FindAll(g => g.Color == color).Count;
        }

        public void PrintCounts()
        {
            foreach (GemColor color in Colors)
            {
                Console.WriteLine("There are {0} {1} gems", NumWithColor(color), color);
            }
        }
    }

    internal class Program
    {
        public static void Main(string[] args)
        {
            var gems = new Gems();
            gems.Create(5);
            gems.PrintCounts();

            gems.DestroyAllWithColor(GemColor.Green);
            gems.PrintCounts();

            gems.DestroyAllWithColor(GemColor.Blue);
            gems.PrintCounts();
        }
    }
}