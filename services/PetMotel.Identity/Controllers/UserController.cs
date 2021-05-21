using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetMotel.Identity.Entity;

namespace PetMotel.Identity.Controllers
{
    //or
    //[Authorize(Roles = "HRManager,Finance")]
    
    //and
    //[Authorize(Roles = "PowerUser")]
    //[Authorize(Roles = "ControlPanelUser")]
    //[Authorize(Roles = Roles.Root)]
    public class UserManagerController : ControllerBase
    {
        
    }
}