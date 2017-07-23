using System;

namespace Certes.Acme
{
    /// <summary>
    /// Represents the ACME directory.
    /// </summary>
    [Obsolete("Use Resource.Directory instead.")]
    public class AcmeDirectory : Resource.Directory
    {
        /// <summary>
        /// Represents the metadata for ACME directory.
        /// </summary>
        [Obsolete("Use Resource.DirectoryMeta instead.")]
        public class AcmeDirectoryMeta : Resource.DirectoryMeta
        {
        }
    }
}
