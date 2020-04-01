using OpenTokSDK;
namespace TokBoxPOC.Sever
{
    public class ServerClass
    {
        private static readonly int ApiKey = 46617502; // YOUR API KEY
        private static readonly string ApiSecret = "f702511cfae0d823a12e3ee6f358befd8349875c";
        public static (string, string) StartInstance()
        {
            
            return (sessionId, token);
        }
    }
}
