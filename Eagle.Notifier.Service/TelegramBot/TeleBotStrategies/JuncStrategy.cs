using Telegram.Bot;
using Telegram.Bot.Args;
using Eagle.Notifier.Service.Resource;

namespace Eagle.Notifier.Service
{
    public class JuncStrategy : ITeleBotStrategy
    {
        public void ProcessRequest(TelegramBotClient botClient, object sender, MessageEventArgs eventArgs)
        {
            botClient.SendTextMessageAsync(eventArgs.Message.From.Id, ServiceMessage.JunkMessage);
        }
    }
}
