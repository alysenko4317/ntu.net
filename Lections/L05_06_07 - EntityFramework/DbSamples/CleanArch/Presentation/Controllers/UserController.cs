using MyApp.Core.Entities;
using MyApp.Core.UseCases;

namespace MyApp.Presentation.Controllers
{
    public class UserController //: Controller
    {
        private readonly CreateUser _createUser;

        public UserController(CreateUser createUser)
        {
            _createUser = createUser;
        }

      //  [HttpPost]
      //  public IActionResult Create(User user)
      //  {
      //      _createUser.Execute(user);
      //      return Ok("User created successfully");
      //  }
    }
}
