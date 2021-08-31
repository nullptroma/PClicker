using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PClicker.MVVM.Models
{
    class TelegramNotifi
    {
        private static Telegram.Bot.TelegramBotClient Client = new Telegram.Bot.TelegramBotClient("1854874903:AAEhxrwvJdj9UUdkhqHYy0b_rqC2lN133Ds");

        public void SendMessage(string message, string id="")
        {
            if(!string.IsNullOrEmpty(id))
                Client.SendTextMessageAsync(id, message);
        }
    }
}
