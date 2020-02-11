using Microsoft.EntityFrameworkCore;

namespace PetData.Utils
{
    public static class DBExceptionFormatter
    {
        public static string format(DbUpdateException e) {
            if (e.InnerException != null)
            {
                return e.InnerException.Message;
            }
            else
            {
                return e.Message;
            }
        }
    }
}
