using SmortIOTThing.Desktop.Interfaces;

namespace SmortIOTThing.Desktop.Managers
{
    public class RequestManager: IRequestManager
    {
        public string GetWelcomeMessage()
        {
            return "Welcome";
        }
    }
}
