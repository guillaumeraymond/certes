namespace Certes.Acme.Resource
{
    /// <summary>
    /// Represents the status for <see cref="AuthorizationIdentifierChallenge"/>.
    /// </summary>
    public static class AuthorizationIdentifierChallengeStatus
    {
        /// <summary>
        /// The pending status.
        /// </summary>
        public const string Pending = "pending";

        /// <summary>
        /// The valid status.
        /// </summary>
        public const string Valid = "valid";

        /// <summary>
        /// The invalid status.
        /// </summary>
        public const string Invalid = "invalid";
    }
}
