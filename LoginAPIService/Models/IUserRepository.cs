using SharedServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginAPIService.Models
{
    public interface IUserRepository
    {
        User ValidateUser(User user);

        bool RegisterUser(User userDetail);

        User UpdateLastName(User userDetail);
    }
}
