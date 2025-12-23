namespace Peeveen.Test.Test;

public class ExceptionTests {
	[Fact]
	public void TestExceptionIsCausedBy() {
		var exception = Assert.Throws<InterestingException>(() => ThrowInterestingException());
		Assert.True(exception.IsCausedBy<DivideByZeroException>());
	}

	public static int ThrowInterestingException() {
		try {
			int y = 0;
			int x = 5 / y;
			return x;
		} catch (Exception e) {
			throw new InterestingException(e);
		}
	}
}

public class InterestingException : Exception {
	public InterestingException(Exception inner) : base("Something interesting happened.", inner) { }
}