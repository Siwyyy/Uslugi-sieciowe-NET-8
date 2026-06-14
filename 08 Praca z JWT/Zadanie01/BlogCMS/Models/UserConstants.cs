using System.Collections.Generic;

namespace BlogCMS.Models
{
    public class UserConstants
    {
        public static List<LoginModel> Users = new()
        {
            new LoginModel(){ Username="Mikołaj",Password="TajneHaslo_1234",Role="Admin"}
        };
    }
}
