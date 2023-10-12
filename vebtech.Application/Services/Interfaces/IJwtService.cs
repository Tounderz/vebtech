using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vebtech.Application.Services.Interfaces;

public interface IJwtService
{
    public string GenerateJwt(string email);
}