using System;
using System.IO;
using System.Linq;
using IWshRuntimeLibrary;

namespace ShortcutCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            //if(args.Length < 1)
            //{
            //    Console.WriteLine("Source directory must be provided as a parameter.");
            //    return;
            //}

            var defaultSource = @"C:\src\Javier.Valero_W7\Dev";
            var defaultTarget = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var defaultFileType = "*.sln";
            
            var source = args.Length < 1 ? defaultSource : args[0];
            var target = args.Length < 2 ? defaultTarget : args[1];
            var fileType = args.Length < 3 ? defaultFileType : args[2];

            string[] dirs = Directory.GetFiles(source, fileType, SearchOption.AllDirectories);

            //var ownPath = AppDomain.CurrentDomain.BaseDirectory;
            //string directoryList = String.Empty;
            //dirs.ToList().ForEach(i => directoryList += i.ToString() + "\n");
            //File.WriteAllText(ownPath + "files" + ".txt", directoryList);
            
            CreateAppShortcut(target, dirs[0]);

        }

        private static void CreateAppShortcut(string target, string sourceFile)
        {
            var temp = target + "\\" + Path.GetFileNameWithoutExtension(sourceFile) + ".url";

            using (StreamWriter writer = new StreamWriter(temp))
            {
                string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + app);
                writer.WriteLine("IconIndex=0");
                string icon = app.Replace('\\', '/');
                writer.WriteLine("IconFile=" + icon);
                writer.Flush();
            }
        }

        static private void CreateAppShortcutAlt()
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Notepad.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "New shortcut for a Notepad";
            shortcut.Hotkey = "Ctrl+Shift+N";
            shortcut.TargetPath = Environment.GetFolderPath(Environment.SpecialFolders.System) + @"\notepad.exe";
            shortcut.Save();
        }
    }
}
