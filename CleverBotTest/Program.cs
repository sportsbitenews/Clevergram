using System;
using ChatterBotAPI;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Linq;
using System.IO;

namespace CleverBotTest
{

    class MainClass
    {

        public static void Main(string[] args)
        {
            Run().Wait();
        }
        static async Task Run()
        {
            var Bot = new Api("Your API key here.");

            var me = await Bot.GetMe();

            Console.WriteLine("Hello my name is {0}", me.Username);

            var offset = 0;


            while (true)
            {
                var updates = await Bot.GetUpdates(offset);

                foreach (var update in updates)
                {
                    if (update.Message.Type == MessageType.TextMessage)
                    {
                        FileStream fs = new FileStream("log.txt", FileMode.Append, FileAccess.Write);
                        StreamWriter writer = new StreamWriter(fs);
                        {
                            await Bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                            await Task.Delay(1000);
                            string s = update.Message.Text;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("@{1} ({2}): {0}", s, update.Message.From.Username, update.Message.From.FirstName);
                            writer.WriteLine("@{1} ({2}): {0}\r\n", s, update.Message.From.Username, update.Message.From.FirstName);
                            ChatterBotFactory factory = new ChatterBotFactory();
                            ChatterBot bot1 = factory.Create(ChatterBotType.CLEVERBOT);
                            ChatterBotSession bot1session = bot1.CreateSession();
                            s = bot1session.Think(s);
                            var t = await Bot.SendTextMessage(update.Message.Chat.Id, s);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Bot: {0}", s);
                            writer.WriteLine("Bot: {0}\r\n", s);
                            s = update.Message.Text;
                            writer.Close();
                            fs.Close();
                        }

                        offset = update.Id + 1;
                    }
                }
                await Task.Delay(500);



            }


        }
    }
}