using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public class Authorization
    {
        public static int GetTokenId(ClaimsPrincipal claim) {
            try
            {
                return (Convert.ToInt32(claim.Claims.FirstOrDefault(s => s.Type == TokenItems.Id).Value));
            } catch (Exception)
            {
                return 0;
            }
        }
    }
}
