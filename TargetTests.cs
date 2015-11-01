using NUnit.Framework;
using System;
using SFML.Graphics;
using System.Collections.Generic;

namespace EquationInvasion
{
	[TestFixture ()]
	public class TargetTests
	{
		[Test ()]
		public void TestTargetCtor ()
		{
			Target t = new Target ();
			Assert.AreEqual (60.0f, t.Rect.Height);
			Assert.AreEqual (60.0f, t.Rect.Width);
		}

		[Test ()]
		public void TestGenTargets ()
		{
			// This should generate a 9 targets
			List<Target> targets = Target.GenTargets (1, 3, EquationDifficulty.EASY);
			Assert.IsTrue (targets.Count == 3);

			// This should generate a 24 targets
			targets = Target.GenTargets (4, 6, EquationDifficulty.EASY);
			Assert.IsTrue (targets.Count == 24);

			// This should generate a 0 targets
			targets = Target.GenTargets (0, 5, EquationDifficulty.EASY);
			Assert.IsTrue (targets.Count == 0);

			// This should generate  0 targets
			targets = Target.GenTargets (5, 0, EquationDifficulty.EASY);
			Assert.IsTrue (targets.Count == 0);
		}

		[Test ()]
		public void TestGettingOuterTargets ()
		{
			Target t1 = new Target (80, 0);
			Target t2 = new Target (10, 20);
			Target t3 = new Target (110, 0);
			Target t4 = new Target (400, 10);
			Target t5 = new Target (200, 100);

			List<Target> targets = new List<Target> () {t1, t2, t3, t4, t5};

			List<Target> outer = Target.GetOuterTargets(targets);
			Target outerLeft = outer [0];
			Target outerRight = outer [1];

			// Outer left should be t2 as it has the lowest X value
			Assert.AreEqual (t2, outerLeft);

			// Outer right should be t4 as it has the highest X value
			Assert.AreEqual (t4, outerRight);
		}

		[Test ()]
		public void TestHasOcsilattedToEdge ()
		{
			Target t1 = new Target (10, 20);
			Target t2 = new Target (510, 0);
			Target t3 = new Target (980, 10);
			Target t4 = new Target (-180, 10);

			Assert.IsTrue (Target.HasOscillatedToEdge(t1), "Test left edge");
			Assert.IsFalse (Target.HasOscillatedToEdge(t2), "Test Middle");
			Assert.IsTrue (Target.HasOscillatedToEdge(t3), "Test right edge");
			Assert.IsTrue (Target.HasOscillatedToEdge(t4), "Test negative x value (off the screen)");
		}
	}
}

