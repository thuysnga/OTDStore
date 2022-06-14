using OTDStore.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OTDStore.Application.System.Roles
{
    public interface IRoleService
    {
        Task<List<RoleVM>> GetAll();
    }
}
