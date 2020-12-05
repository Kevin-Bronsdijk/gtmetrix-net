using System.Collections.Generic;

namespace GTmetrix5.Model
{
    public class NoInstructionsRequest : IRequest
    {
        public List<KeyValuePair<string, string>> GetPostData()
        {
            return null;
        }
    }
}
