using System.Collections.Generic;

namespace GTmetrix.Model
{
    public interface IRequest
    {
        List<KeyValuePair<string, string>> GetPostData();
    }
}