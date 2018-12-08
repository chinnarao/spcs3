using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class RepositoryResult
    {
        private readonly List<RepositoryError> _errors = new List<RepositoryError>();

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="RepositoryError"/>s containing an errors
        /// that occurred during the repository operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="RepositoryError"/>s.</value>
        public IEnumerable<RepositoryError> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="RepositoryResult"/> indicating a successful repository operation.
        /// </summary>
        /// <returns>An <see cref="RepositoryResult"/> indicating a successful operation.</returns>
        public static RepositoryResult Success { get; } = new RepositoryResult { Succeeded = true };

        /// <summary>
        /// Creates an <see cref="RepositoryResult"/> indicating a failed repository operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="RepositoryResult"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="RepositoryResult"/> indicating a failed repository operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static RepositoryResult Failed(params RepositoryError[] errors)
        {
            var result = new RepositoryResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }

            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="RepositoryResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="RepositoryResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned 
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Errors"/> collection, if any.
        /// </remarks>
        public override string ToString() =>
            Succeeded
                ? "Succeeded"
                : $"Failed : {string.Join(",", Errors.Select(x => x.Code).ToList())}";
    }
}