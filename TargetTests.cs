using NUnit.Framework;
using System;
using SFML.Graphics;

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
			Assert.AreEqual(60.0f, t.Rect.Width);
		}

		[Test ()]
		public void TestTargetCollision ()
		{
		}
	}
}

