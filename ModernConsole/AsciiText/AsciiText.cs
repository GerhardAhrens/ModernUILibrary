//-----------------------------------------------------------------------
// <copyright file="AsciiText.cs" company="Lifeprojects.de">
//     Class: AsciiText
//     Copyright � Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>18.07.2025 07:28:35</date>
//
// <summary>
// Klasse f�r 
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    using System.Text;

    public partial class MConsole
    {
        public static void AsciiText(string message, ConsoleColor color = ConsoleColor.White)
        {
            MConsole.WriteLine(AsciiChars.GetAsciiArt(message), color);
        }
    }

    internal static class AsciiChars
    {
        static string[] allCharsAscii = [
    @"
    _    
   / \   
  / _ \  
 / ___ \ 
/_/   \_\",
@"
 ____  
| __ ) 
|  _ \ 
| |_) |
|____/ ",
@"
  ____ 
 / ___|
| |    
| |___ 
 \____|",
@"
 ____  
|  _ \ 
| | | |
| |_| |
|____/ ",
@"
 _____ 
| ____|
|  _|  
| |___ 
|_____|",
@"
 _____ 
|  ___|
| |_   
|  _|  
|_|    ",
@"
  ____ 
 / ___|
| |  _ 
| |_| |
 \____|",
@"
 _   _ 
| | | |
| |_| |
|  _  |
|_| |_|",
@"
 ___ 
|_ _|
 | | 
 | | 
|___|",
@"
     _ 
    | |
 _  | |
| |_| |
 \___/ ",
@"
 _  __
| |/ /
| ' / 
| . \ 
|_|\_\",
@"
 _     
| |    
| |    
| |___ 
|_____|",
@"
 __  __ 
|  \/  |
| |\/| |
| |  | |
|_|  |_|",
@"
 _   _ 
| \ | |
|  \| |
| |\  |
|_| \_|",
@"
  ___  
 / _ \ 
| | | |
| |_| |
 \___/ ",
@"
 ____  
|  _ \ 
| |_) |
|  __/ 
|_|    ",
@"
  ___  
 / _ \ 
| | | |
| |_| |
 \__\_\",
@"
 ____  
|  _ \ 
| |_) |
|  _ < 
|_| \_\",
@"
 ____  
/ ___| 
\___ \ 
 ___) |
|____/ ",
@"
 _____ 
|_   _|
  | |  
  | |  
  |_|  ",
@"
 _   _ 
| | | |
| | | |
| |_| |
 \___/ ",
@"
__     __
\ \   / /
 \ \ / / 
  \ V /  
   \_/   ",
@"
__        __
\ \      / /
 \ \ /\ / / 
  \ V  V /  
   \_/\_/   ",
@"
__  __
\ \/ /
 \  / 
 /  \ 
/_/\_\",
@"
__   __
\ \ / /
 \ V / 
  | |  
  |_|  ",
@"
 _____
|__  /
  / / 
 / /_ 
/____|",
@"
 __ 
| _| 
| | 
| | 
|__|",
@" 
__    
\ \   
 \ \  
  \ \ 
   \_\",
@"
 __ 
|_ |
 | |
 | |
|__|
",
@"
 /\ 
|/\|
    ",
@"
       
       

 _____ 
|_____|
",
@"
 _ 
( )
 \|
   

",
@"
       
  __ _ 
 / _` |
| (_| |
 \__,_|",
@"
 _     
| |__  
| '_ \ 
| |_) |
|_.__/ ",
@"
      
  ___ 
 / __|
| (__ 
 \___|",
@"
     _ 
  __| |
 / _` |
| (_| |
 \__,_|
",
@"
      
  ___ 
 / _ \
|  __/
 \___|
",
@"
  __ 
 / _|
| |_ 
|  _|
|_|  
",
@"   
  __ _ 
 / _` |
| (_| |
 \__, |
 |___/ 
",
@" 
 _     
| |__  
| '_ \ 
| | | |
|_| |_|
",
@" 
 _ 
(_)
| |
| |
|_|
",
@"
 
   _ 
  (_)
  | |
 _/ |
|__/ 
",
@" 
 _    
| | __
| |/ /
|   < 
|_|\_\
",
@"
 _ 
| |
| |
| |
|_|
",
@"
           
 _ __ ___  
| '_ ` _ \ 
| | | | | |
|_| |_| |_|
",
@"
       
 _ __  
| '_ \ 
| | | |
|_| |_|
",
@"
       
  ___  
 / _ \ 
| (_) |
 \___/ 
",
@"     
 _ __  
| '_ \ 
| |_) |
| .__/ 
|_|    
",
@" 
  __ _ 
 / _` |
| (_| |
 \__, |
    |_|
",
@"
      
 _ __ 
| '__|
| |   
|_|   
",
@"
     
 ___ 
/ __|
\__ \
|___/
",
@"  
 _   
| |_ 
| __|
| |_ 
 \__|
",
@"
       
 _   _ 
| | | |
| |_| |
 \__,_|
",
@"
       
__   __
\ \ / /
 \ V / 
  \_/  
",
@"
          
__      __
\ \ /\ / /
 \ V  V / 
  \_/\_/  
",
@"
      
__  __
\ \/ /
 >  < 
/_/\_\
",
@"
       
 _   _ 
| | | |
| |_| |
 \__, |
 |___/ 
",
@"
     
 ____
|_  /
 / / 
/___|
",
];

        public static string GetAsciiArt(string text)
        {
            var resultList = new List<string>();
            foreach (var ch in text.ToCharArray())
            {
                var index = ch - 'A';
                if (index >= 0 && index <= allCharsAscii.Length)
                {
                    resultList.Add(allCharsAscii[index]);
                }
            }

            return ToSingleLine(resultList);
        }

        private static string ToSingleLine(List<string> asciiChars)
        {
            var sb = new StringBuilder();
            var rowsCount = asciiChars[0].Split(Environment.NewLine).Length;
            for (var i = 0; i < rowsCount; i++)
            {
                for (var j = 0; j < asciiChars.Count; j++)
                {
                    var splitted = asciiChars[j].Split(Environment.NewLine);
                    sb.Append(splitted[i]);

                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
