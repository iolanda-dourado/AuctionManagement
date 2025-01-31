namespace AuctionManagement.WebAPI.AuthClasses {
    /// <summary>
    /// Represents a user's registration credentials.
    /// </summary>
    public class Register {

        /// <summary>
        /// The username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The email of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The password of the user.
        /// </summary>
        public string Password { get; set; } = string.Empty;

    }
}
