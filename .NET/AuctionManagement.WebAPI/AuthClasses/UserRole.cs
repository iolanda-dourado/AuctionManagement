namespace AuctionManagement.WebAPI.AuthClasses {
    /// <summary>
    /// Class for user role
    /// </summary>
    public class UserRole {

        /// <summary>
        /// The user's username
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The user's role
        /// </summary>
        public string Role { get; set; } = string.Empty;
    }
}
