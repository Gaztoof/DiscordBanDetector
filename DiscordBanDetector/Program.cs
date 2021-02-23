using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSockets;
using Discord;
using Discord.Gateway;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using System.Runtime.InteropServices;

namespace DiscordBanDetector
{
    class SavedServer
    {
        public string Name;
        public ulong Id;
    }
    class SavedFriend
    {
        public string FullName;
        public ulong Id;
    }
    class SavedData
    {
        public List<SavedServer> servers = new List<SavedServer>();
        public List<SavedFriend> friends = new List<SavedFriend>();
    }
    class Program
    {
        public static DiscordSocketClient client = new DiscordSocketClient();
        public static string AESKey = "PlesGaz";
        private const int STD_OUTPUT_HANDLE = -11;
        private const int MY_CODE_PAGE = 437;
        [DllImport("kernel32")]
        static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(int Handle, int showState);


        static void ShowConsole(bool showConsole)
        {
            ShowWindow((int)GetConsoleWindow(), showConsole ? 5 : 0);
        }

        static bool LogIn(string token)
        {
            if (token.Length < 10) return false;
            try
            {
                client.Login(token);
                client.GetRelationships();
                client.GetGuilds();
                return true;
            }catch{return false;}
        }
        static void Persist()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if(rk.GetValue("DiscordBanDetector") == null || (rk.GetValue("DiscordBanDetector").ToString() != Application.ExecutablePath))
            rk.SetValue("DiscordBanDetector", Application.ExecutablePath);
            using (TaskService ts = new TaskService())
            {
                foreach (var tas in ts.RootFolder.GetTasks())
                    if (tas.Name == "DiscordBanDetector") return;
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Runs the DiscordBanDetector to make sure you're not banned from discord.";
                var trigger = new DailyTrigger();
                trigger.Repetition.Interval = TimeSpan.FromHours(24);
                trigger.Repetition.Duration = TimeSpan.FromHours(24);
                td.Triggers.Add(trigger);
                td.Actions.Add(new ExecAction(Application.ExecutablePath, null, null));
                ts.RootFolder.RegisterTaskDefinition("DiscordBanDetector", td);
            }
        }
        static void Main(string[] args)
        {
            ShowConsole(false);
            Console.Title = "Discord Ban Detector - Coded by t.me/Gaztoof";
            if (Process.GetProcessesByName("DiscordBanDetector").Length > 1) Environment.Exit(0);
            
            Visuals.WriteLine("If you want to update the token, delete the \"token\" file.", Color.White);
            AESKey = Security.GetMachineGuid();
            string token = "";
            try
            {
                if (File.Exists(Application.StartupPath + "\\" + "token") && File.ReadAllText(Application.StartupPath + "\\" + "token") != "" && File.ReadAllBytes(Application.StartupPath + "\\" + "token").Length == 64)
                {
                    token = Security.Decrypt(File.ReadAllBytes("token"), AESKey);
                    if (!LogIn(token))
                    {
                        if (File.Exists(Application.StartupPath + "\\" + "saveddata.txt"))
                        {
                            Persist();
                            Visuals.WriteLine("The saved token doesn't works anymore, you might have been banned.", Color.IndianRed);
                            BannedMethod();
                        }
                    }
                }
                else
                {
                    token = getToken();
                }
            }
            catch { }
            Persist();
            Visuals.WriteLine("Successfully logged in from saved token!", Color.LimeGreen);
            SavedData data = new SavedData();
            Visuals.WriteLine("Retrieving servers...", Color.SkyBlue);
            int i = 0;
        Retry:
            try
            {
                var guilds = client.GetGuilds();
                foreach (var guild in guilds)
                {
                    data.servers.Add(new SavedServer() { Name = guild.Name, Id = guild.Id });
                }
                Visuals.WriteLine("Successfully retrieved servers!", Color.Lime);
            }
            catch(Exception ex)
            {
                Visuals.WriteLine("Well, an error occurred while retrieving servers! Retrying...", Color.IndianRed);
                Console.WriteLine(ex.Message);
                if(i < 3)
                {
                    i++;
                    goto Retry;
                }
            }

            Visuals.WriteLine("Retrieving friends...", Color.SkyBlue);
            try
            {
                var friends = client.GetRelationships();
                foreach (var friend in friends)
                {
                    if (friend.Type == RelationshipType.Friends)
                    {
                        data.friends.Add(new SavedFriend() { FullName = friend.User.ToString(), Id = friend.User.Id });
                    }
                }
                Visuals.WriteLine("Successfully retrieved friends!", Color.Lime);
            }
            catch
            {
                Visuals.WriteLine("Well, an error occurred while retrieving friends!", Color.IndianRed);
            }
            for (int l = 0; l < 5; l++)
            {
                try
                {
                    File.WriteAllText(Application.StartupPath + "\\" + "saveddata.txt", JsonConvert.SerializeObject(data, Formatting.Indented));
                    break;
                }
                catch
                {
                    Visuals.WriteLine("Couldn't save, retrying in 3 seconds...!", Color.IndianRed);
                    System.Threading.Thread.Sleep(3000);
                }
            }
            Visuals.WriteLine("Successfully saved all data!", Color.IndianRed);
                System.Threading.Thread.Sleep(3000);
        }
        public static void BannedMethod()
        {
            ShowConsole(true);
            Visuals.WriteLine("Every infos have been saved to saveddata.txt next to this program.", Color.White);
            try
            {
                var data = JsonConvert.DeserializeObject<SavedData>(File.ReadAllText("saveddata.txt"));
                Visuals.WriteLine("Retrieving servers...", Color.SkyBlue);
                foreach (var savedserver in data.servers)
                {
                    Console.WriteLine(savedserver.Name + " - " + savedserver.Id);
                }
                Visuals.WriteLine("Retrieving friends...", Color.Lime);
                foreach (var savedfriend in data.friends)
                {
                    Console.WriteLine(savedfriend.FullName + " - " + savedfriend.Id);
                }
            }catch{}
            MessageBox.Show("You might have been banned from discord! Check the console for informations.");
            Console.ReadLine();
            Environment.Exit(0);
        }
        public static string getToken()
        {
            ShowConsole(true);
            File.WriteAllText("token", "");
            string token = "";
            while (true)
            {
                token = Visuals.ReadLine("Please, enter your token: ", Color.White);
                if (LogIn(token)) break;
                Visuals.WriteLine("The token you've entered is wrong.", Color.IndianRed);
            }
            File.WriteAllBytes("token", Security.Encrypt(token, AESKey));
            return token;
        }
    }
}
