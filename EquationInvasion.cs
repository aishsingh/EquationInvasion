using System;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace EquationInvasion
{
	class EquasionInvasion
	{
		RenderWindow _window;
		public void StartSFMLProgram()
		{
			// setup window
			_window = new RenderWindow (new VideoMode (1000, 800), "Equation Invasion! - by Tainted Mustard", Styles.Close);
			_window.Closed += new EventHandler(OnClosed);
			_window.SetVerticalSyncEnabled (true);
			_window.SetFramerateLimit(60);

			// main game loop
			while (_window.IsOpen)
			{
				_window.DispatchEvents();
				_window.Clear(Color.Black);

				// single equation
				RectangleShape eq = new RectangleShape(new Vector2f(60.0f, 60.0f));
				eq.Position = new Vector2f(80.0f, 0.0f);
				eq.FillColor = Color.Cyan;
				_window.Draw (eq);

				// player
				CircleShape p = new CircleShape(30, 3);
				p.Position = new Vector2f(_window.Size.X/2 - (p.Radius), _window.Size.Y - (p.Radius*2));
				p.FillColor = Color.Green;
				_window.Draw (p);


				_window.Display();
			}
		}
		void OnClosed(object sender, EventArgs e)
		{
			_window.Close();
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			EquasionInvasion app = new EquasionInvasion();
			app.StartSFMLProgram();
		}
	}
}
