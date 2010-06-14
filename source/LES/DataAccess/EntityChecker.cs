using System.IO;

namespace LES.DataAccess
{
    public class EntityChecker
    {
        public static bool BrokerExists()
        {
            return File.Exists("broker.txt");
        }
        public static bool UnderwriterExists()
        {
            return File.Exists("uw.txt");
        }
    }
}
