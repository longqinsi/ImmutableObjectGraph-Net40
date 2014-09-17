namespace ImmutableObjectGraph.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	
	using Xunit;

	public class DeepHierarchyTests {
		[Fact]
		public void ToDerivedTypeOneStepAtATime() {
			A a = A.Create(1);

			// Step one level at a time.
			B b = a.ToB(2);
			C1 c1 = b.ToC1(3);
			Assert.Equal(1, c1.Field1);
			Assert.Equal(2, c1.Field2);
			Assert.Equal(3, c1.Field3);
		}

		[Fact]
		public void ToDerivedTypeMixedMode() {
			A a = A.Create(1);

			// Step one level at a time, but for the second step, call the method declared on the least-derived type.
			A bAsA = a.ToB(2);
			C1 c1 = bAsA.ToC1(Optional<System.Int32>.Missing, 3);
			Assert.Equal(1, c1.Field1);
			Assert.Equal(2, c1.Field2);
			Assert.Equal(3, c1.Field3);

			// Same thing, except this time override field2
			c1 = bAsA.ToC1(5, 3);
			Assert.Equal(1, c1.Field1);
			Assert.Equal(5, c1.Field2);
			Assert.Equal(3, c1.Field3);
		}

		[Fact]
		public void ToDerivedTypeTwoStepsAtOnce() {
			A a = A.Create(1);

			// Jump two levels at once.
			C1 c1 = a.ToC1(2, 3);
			Assert.Equal(1, c1.Field1);
			Assert.Equal(2, c1.Field2);
			Assert.Equal(3, c1.Field3);
		}

		[Fact]
		public void ToBaseType() {
			C1 c1 = C1.Create(1, 2, 3);
			A a = c1.ToA();
			Assert.IsType(typeof(A), a); // should not be a derived type.
			Assert.Equal(1, a.Field1);
		}
		
		[Fact]
		public void ToSiblingType() {
			C1 c1 = C1.Create(1, 2, 3);
            C2 c2 = c1.ToC2(Optional<System.Int32>.Missing);
			Assert.Equal(1, c2.Field1);
			Assert.Equal(2, c2.Field2);
			Assert.Equal(0, c2.Field3); // different field between types

			c2 = c1.ToC2(4);
			Assert.Equal(1, c2.Field1);
			Assert.Equal(2, c2.Field2);
			Assert.Equal(4, c2.Field3);
		}

		[Fact]
		public void ToDerivedOnBasePreservesDefaultInstance() {
            A bAsA = B.Create(Optional<System.Int32>.Missing);
            B result = bAsA.ToB(Optional<System.Int32>.Missing);
			Assert.Same(bAsA, result);
		}

		[Fact]
		public void ToDerivedOnBasePreserveInstanceWithSameValues() {
			A bAsA = B.Create(1, 2);
			B result = bAsA.ToB(2);
			Assert.Same(bAsA, result);
		}

		[Fact]
		public void ToDerivedOnBaseCreatesNewInstanceWithDifferentValues() {
			A bAsA = B.Create(1, 2);
			B result = bAsA.ToB(3);
			Assert.NotSame(bAsA, result);
			Assert.Equal(3, result.Field2);
		}

		[Fact]
		public void ToDerivedOnBaseDoesNotReturnDerivedInstance() {
            A c1AsA = C1.Create(Optional<System.Int32>.Missing);
            B result = c1AsA.ToB(Optional<System.Int32>.Missing);
			Assert.NotSame(c1AsA, result);
			Assert.IsType<B>(result); // should not retain C1 as its type
		}

		[Fact]
		public void ToSameTypeReturnsThis() {
            B b = B.Create(Optional<System.Int32>.Missing, Optional<System.Int32>.Missing);
            Assert.Same(b, b.ToB(Optional<System.Int32>.Missing));

			B nonDefaultB = B.Create(1, 2);
			Assert.Same(nonDefaultB, nonDefaultB.ToB(nonDefaultB.Field2));
            Assert.Same(nonDefaultB, nonDefaultB.ToB(Optional<System.Int32>.Missing));
		}
	}
}
