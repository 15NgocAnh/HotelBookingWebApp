namespace HotelBooking.Data.Utils.Variable
{
    public static class RegexUtils
    {
        public const string PHONE_NUMBER = "([84|0])([3|5|7|8|9])+([0-9]{8})";
        public const string EMAIL = "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$";
        public const string PASSWORD = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
    }
}
