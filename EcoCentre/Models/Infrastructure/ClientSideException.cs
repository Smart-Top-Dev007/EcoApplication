using System;

namespace EcoCentre.Models.Infrastructure
{
    public class ClientSideException :Exception
    {
        public ClientSideException(string error,string file, string line) : base(string.Format("Error:{0},File:{1}:{2}",error,file,line))
        {
            
        }
    }
}