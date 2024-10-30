namespace System.Windows.Input
{
    public static class InputExtension
    {
       public static bool IsBackKey(this System.Windows.Input.Key keyPressed)
       {
           if(keyPressed.ToString().ToLower() == "back")
               return true;

           return false; 
       }

       public static bool IsDeleteKey(this System.Windows.Input.Key keyPressed)
       {
           if(keyPressed.ToString().ToLower() == "delete")
               return true;
           return false; 

       }

       public static bool IsArrowKey(this System.Windows.Input.Key keyPressed)
       {
           if(keyPressed.ToString().ToLower() == "left")
               return true;

           if(keyPressed.ToString().ToLower() == "right")
               return true;
           
           if(keyPressed.ToString().ToLower() == "up")
               return true;

           if (keyPressed.ToString().ToLower() == "down")
               return true; 

           return false; 
       }
    }
}
