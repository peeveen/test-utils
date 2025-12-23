using System;
using System.Linq;

namespace Peeveen.Test {
	/// <summary>
	/// Some extension methods for Exception.
	/// </summary>
	public static class ExceptionExtensions {
		/// <summary>
		/// Returns true if this exception, or any of the inner exceptions, is of the given type.
		/// If this exception or any of the inner exceptions are an AggregateException, all inner
		/// exceptions of that AggregateException are checked, and true is returned if any of them
		/// are of the given type.
		/// </summary>
		/// <typeparam name="TInnerException">Type to look for.</typeparam>
		/// <param name="e">The top level exception.</param>
		/// <returns>True if any inner exception is of type TInnerException.</returns>
		public static bool IsCausedBy<TInnerException>(this Exception e) {
			if (e.GetType() == typeof(TInnerException))
				return true;
			if (e is AggregateException aggregateException)
				return aggregateException.InnerExceptions.Any(inner => inner.IsCausedBy<TInnerException>());
			return e.InnerException?.IsCausedBy<TInnerException>() ?? false;
		}
	}
}