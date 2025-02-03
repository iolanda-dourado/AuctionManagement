namespace AuctionManagement.WebAPI.AuthClasses {

    /// <summary>
    /// Represents a user's login credentials.
    /// </summary>
    public class Login {

        /// <summary>
        /// The username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The password of the user.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
