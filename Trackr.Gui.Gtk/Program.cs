﻿using System;
using Gdk;
using Gtk;
using Trackr.List;
using Settings = Trackr.Core.Settings;


namespace Trackr.Gui.Gtk {
    public static class Program {
        public const string AppName = "Trackr";
        public static readonly Pixbuf AppIcon = new Pixbuf("trackr.png");
        
        private static SystemTray _tray;
        public static MainWindow Win;
        public static Settings Settings; // Reference to Core.Program.UserSettings
        public static AnimeList AnimeList;
        public static MangaList MangaList;
        
        public static void Main(string[] args) {
            Console.WriteLine("Hello world!");
            // If the settings file doesn't exist, force the user to set up account.
            var force = !Settings.Exists;
            // This will load the settings file for us or return a new one (if force is true)
            try {
                Core.Program.UserSettings = Settings.Load();
            }
            catch (InvalidOperationException) { // Wrong version of settings, most likely. For now we will recreate
                Settings.Delete();
                Core.Program.UserSettings = Settings.Load();
            }
            finally { Settings = Core.Program.UserSettings; }
                   
            Application.Init();
            if(force)
                using (var s = new SettingsWindow(true)) {
                    var res = s.Run();
                    s.Destroy();
                    if (res != (int) ResponseType.Accept)
                        Environment.Exit(0);
                }
            // we have a settings file! Spawn our notification icon and window
            _tray = new SystemTray();
            Win = new MainWindow { Visible = Settings.ShowWindowOnStart };
            Application.Run();
        }
    }
}
