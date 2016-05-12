namespace WebLedMatrix.Logic.Authentication.Models
{
    public static class CreationModelExternalMethods
    {
        public static User GetUser(this CreationModel model)
        {
            return new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.TelephoneNumber,
                UserName = model.Name,
                Email = model.Email
            };
        }
    }
}