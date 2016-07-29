using System;
using System.Collections.Generic;

namespace GTmetrix.Model
{
    public class NoInstructionsRequest : IRequest
    {
        public List<KeyValuePair<string, string>> GetPostData()
        {
            return null;
        }
    }
}