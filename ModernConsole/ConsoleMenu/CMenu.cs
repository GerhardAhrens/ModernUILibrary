/*
 * <copyright file="CMenu.cs" company="Lifeprojects.de">
 *     Class: CMenu
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>02.04.2023 14:24:31</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Über die Klasse kann ein einfaches Konsolen Menü erstellt werden.
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernConsole.Menu
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class CMenu
    {
        internal Dictionary<string, string> cmenuTexte = null;

        internal struct Item
        {
            public Item(string desc, Action act, int level)
            {
                this.Desc = desc;
                this.Act = act;
                this.Level = level;
            }

            internal string Desc { get; }

            internal Action Act { get; }

            internal int Level { get; set; }
        }

        internal IList<Item> Items;
        internal string Name;

        internal CMenu(string name = null)
        {
            this.Name = name;
            this.Items = new List<Item>();
        }

        internal CMenu(IList<Item> items, string name = null)
        {
            this.Name = name;
            this.Items = items;
        }

        public void Show()
        {
            this.IniteTexte();
            ConsoleKeyInfo k;

            do
            {
                Console.Clear();
                StringBuilder sb = new StringBuilder();
                if (this.Name != null)
                {
                    sb.AppendLine($"*** {this.Name} ****");
                }

                for (int i = 0; i < Items.Count; i++)
                {
                    sb.AppendFormat("{0} - {1}{2}", i, Items[i].Desc, Environment.NewLine);
                }

                if (Items.Any(a => a.Level == 0))
                {
                    sb.AppendFormat("{0}q - {1}", Environment.NewLine, this.GetText("quit"));
                }
                else
                {
                    sb.AppendFormat("{0}b - {1}", Environment.NewLine, this.GetText("back"));
                }

                Console.WriteLine(sb.ToString());

                k = Console.ReadKey(true);

                if (Char.IsNumber(k.KeyChar) && Convert.ToInt32(k.KeyChar.ToString()) < this.Items.Count)
                {
                    this.Items[Convert.ToInt32(k.KeyChar.ToString())].Act.Invoke();
                }

                if (k.Key == ConsoleKey.B)
                {
                    break;
                }
            }
            while ((k.Key != ConsoleKey.Q));
        }

        private string GetText(string key)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            string textKey = $"{cultureInfo.TwoLetterISOLanguageName}_{key}";
            return this.cmenuTexte[textKey];
        }

        private void IniteTexte()
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;

            this.cmenuTexte = new Dictionary<string, string>();
            if (cultureInfo.TwoLetterISOLanguageName == "de")
            {
                this.cmenuTexte.Add($"{cultureInfo.TwoLetterISOLanguageName}_quit", "Beenden");
                this.cmenuTexte.Add($"{cultureInfo.TwoLetterISOLanguageName}_back", "Zurück");
            }
            else
            {
                this.cmenuTexte.Add($"{cultureInfo.TwoLetterISOLanguageName}_quit", "Quit");
                this.cmenuTexte.Add($"{cultureInfo.TwoLetterISOLanguageName}_back", "Back");
            }
        }
    }

    public static class SmartMenu
    {
        public static CMenu Menu(string name = null)
        {
            return new CMenu(name);
        }

        public static CMenu Add(this CMenu menu, string description, Action action, int level = 0)
        {
            var existingMenuItems = menu.Items;
            existingMenuItems.Add(new CMenu.Item(description, action,level));

            return new CMenu(existingMenuItems, menu.Name);
        }

        public static CMenu Add(this CMenu menu, CMenu subMenu, int level = 0)
        {
            return menu.Add(subMenu.Name, () => subMenu.Show());
        }
    }
}

