using System.Collections.Generic;

namespace GTmetrix5.Model
{
    public interface IRequest
    {
        List<KeyValuePair<string, string>> GetPostData();
    }
}
