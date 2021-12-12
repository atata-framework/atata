using System;

namespace Atata
{
    public sealed class MissingComponentScopeFindResult : ComponentScopeFindResult
    {
        // TODO: Atata v3. Make constructor internal.
#pragma warning disable S3253 // Constructor and destructor declarations should not be redundant
        [Obsolete("Use ComponentScopeFindResult.Missing instead.")] // Obsolete since v2.0.0.
        public MissingComponentScopeFindResult()
        {
        }
#pragma warning restore S3253 // Constructor and destructor declarations should not be redundant
    }
}
