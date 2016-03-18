using WebLedMatrix.Logic.Authentication.Abstract;

namespace WebLedMatrix.Hubs
{
    public static class extensions
    {
        public static bool IsCurrentUserInRole(this RoleManagingHub managingHub,State state)
        {
            return managingHub.Context.User.IsInRole(state.ToString());
        }
    }
}