//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.12.2023 21:30:28</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace Console.Fireworks
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class Program
    {
        private static void Main()
        {
            Console.Title = "Happy new Year";
            const float startPattern = 0.5f;
            const int maxFireworksOfStart = 5;

            Random random = new Random();

            Console.CursorVisible = false;

            while (!Console.KeyAvailable)
            {
                if (random.NextDouble() > startPattern)
                {
                    int numFireworks = random.Next(1, maxFireworksOfStart + 1);

                    for (int i = 0; i < numFireworks; i++)
                    {
                        float launcherX = random.Next(Console.WindowWidth);
                        float launcherY = Console.WindowHeight;

                        List<FireworkElement> fireworks = new List<FireworkElement>();

                        fireworks.Add(new FireworkElement(launcherX, launcherY, GetRandomFireworkColor(), true, 0, -1.5f));
                        Console.SetCursorPosition((int)launcherX, (int)launcherY);
                        Console.Write($" Happy new year {DateTime.Now.Year}");

                        while (fireworks.Count > 0)
                        {
                            ClearFirework(fireworks);

                            UpdateFireworkElements(ref fireworks);

                            foreach (FireworkElement element in fireworks)
                            {
                                Console.SetCursorPosition((int)element.X, (int)element.Y);
                                Console.ForegroundColor = GetRandomFireworkColor();
                                Console.Write(GetRandomFireworkCharacter());
                            }

                            Thread.Sleep(40);
                        }

                        Console.SetCursorPosition((int)launcherX, (int)launcherY);
                        Console.Write(new string(' ', 21));
                    }
                }

                Thread.Sleep(40);
            }

            Console.ResetColor();
            Console.CursorVisible = true;
        }

        private static void Clamp(ref float value, float min, float max)
        {
            if (value < min) value = min;
            if (value > max) value = max;
        }

        private static ConsoleColor GetRandomFireworkColor()
        {
            Array colorValues = Enum.GetValues(typeof(ConsoleColor));
            return (ConsoleColor)colorValues.GetValue(new Random().Next(colorValues.Length));
        }

        private static char GetRandomFireworkCharacter()
        {
            char[] fireworkCharacters = { '■', '¤', '*' };
            return fireworkCharacters[new Random().Next(fireworkCharacters.Length)];
        }

        private static void ClearFirework(List<FireworkElement> fireworks)
        {
            foreach (var particle in fireworks)
            {
                Console.SetCursorPosition((int)particle.X, (int)particle.Y);
                Console.Write(" ");
            }
        }

        private static void UpdateFireworkElements(ref List<FireworkElement> fireworks)
        {
            List<FireworkElement> newElements = new List<FireworkElement>();
            foreach (var particle in fireworks)
            {
                particle.X += particle.XVelocity;
                particle.Y += particle.YVelocity;
                particle.YVelocity += 0.1f;

                if (particle.Y > Console.WindowHeight || particle.X < 0 || particle.X >= Console.WindowWidth)
                {
                    continue;
                }

                newElements.Add(particle);

                if (particle.Exploded && particle.YVelocity >= 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        float angle = i * (360f / 8);
                        float xVelocity = (float)Math.Cos(angle * Math.PI / 180) * 1.5f;
                        float yVelocity = (float)Math.Sin(angle * Math.PI / 180) * 1.5f;
                        FireworkElement newParticle = new FireworkElement(particle.X, particle.Y, particle.Color, false, xVelocity, yVelocity);
                        newElements.Add(newParticle);
                    }
                }
            }

            fireworks = newElements;
        }

        private class FireworkElement
        {
            public float X { get; set; }
            public float Y { get; set; }
            public ConsoleColor Color { get; set; }
            public bool Exploded { get; set; }
            public float XVelocity { get; set; }
            public float YVelocity { get; set; }

            public FireworkElement(float x, float y, ConsoleColor color, bool exploded, float xVelocity, float yVelocity)
            {
                X = x;
                Y = y;
                Color = color;
                Exploded = exploded;
                XVelocity = xVelocity;
                YVelocity = yVelocity;
            }
        }
    }
}
