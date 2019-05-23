using System;
//using System.Windows.Forms;
//using System.Drawing;

namespace DCOdyssey
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new DCOdyssey())
            {
                //try
                //{
                    
                //    Form tempForm = Form.
                //    tempForm.Icon = new Icon("Icon.ico");
                //}
                //catch { }

                game.Run();
            }
        }
    }
#endif
}
