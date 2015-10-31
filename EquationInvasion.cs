using System;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;

namespace EquationInvasion
{
	class EquasionInvasion
	{
		RenderWindow _window;
		private FloatRect screenRect;
		private Direction _arrowKeyDown = Direction.NONE;
		private List<Target> _targets;
		private Player _player;
		private int _wave;
		private Direction _waveOscilatingDir = Direction.NONE;

		public EquasionInvasion() {
			SetupWindow();

			screenRect = new FloatRect (_window.Position.X, _window.Position.Y, _window.Size.X, _window.Size.Y);
			_player = new Player(screenRect);

			_targets = new List<Target> ();
		}
		public void SetupWindow()
		{
			// create window
			_window = new RenderWindow (new VideoMode (1000, 800), "Equation Invasion! - Tainted Mustard", Styles.Close);
			_window.SetVerticalSyncEnabled (true);
			_window.SetFramerateLimit(60);
	
			// setup SFML event handlers
			_window.Closed += new EventHandler(OnClosed);
			_window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);
			_window.KeyReleased += new EventHandler<KeyEventArgs> (OnKeyReleased);
			_window.MouseButtonPressed += new EventHandler<MouseButtonEventArgs>(OnMouseClicked);
		}

		public void StartGameLoop()
		{
			while (_window.IsOpen)
			{
				// check wave progress
				if (_targets.Count == 0)
					NextWave ();

				_window.DispatchEvents();
				_window.Clear(Color.Black);

				// draw all targets to the screen
				foreach (Target t in _targets) {
					t.Oscillate (_waveOscilatingDir);
					t.Draw (_window);
				}

				// Revert direction when outer targets hit the screen edge
				if (_targets.Count > 0 && Target.HasOuterTargetHitScreenEdge (_targets)) {
					if (_waveOscilatingDir == Direction.LEFT)
						_waveOscilatingDir = Direction.RIGHT;
					else if (_waveOscilatingDir == Direction.RIGHT)
						_waveOscilatingDir = Direction.LEFT;
				}


				if (_arrowKeyDown != Direction.NONE)
					_player.Move (_arrowKeyDown, screenRect);

				// draw player
				_player.Draw (_window);

				// refresh screen
				_window.Display();
			}
		}
		void OnClosed(object sender, EventArgs e)
		{
			_window.Close();
		}
		void OnKeyPressed(object sender, EventArgs e)
		{
			KeyEventArgs pressed = (KeyEventArgs)e;

			if (pressed.Code == Keyboard.Key.Left)
				_arrowKeyDown = Direction.LEFT;
			else if (pressed.Code == Keyboard.Key.Right)
				_arrowKeyDown = Direction.RIGHT;
			else if (pressed.Code == Keyboard.Key.PageUp)  // This is a cheat only for testing
				NextWave ();
		}

		void OnKeyReleased(object sender, EventArgs e)
		{
			KeyEventArgs pressed = (KeyEventArgs)e;

			if ((pressed.Code == Keyboard.Key.Left && _arrowKeyDown == Direction.LEFT) || 
				(pressed.Code == Keyboard.Key.Right && _arrowKeyDown == Direction.RIGHT))
				_arrowKeyDown = Direction.NONE;
		}

		void OnMouseClicked(object sender, EventArgs e)
		{
			// any mouse click input will be recieved here
			MouseButtonEventArgs pressed = (MouseButtonEventArgs)e;
		}


		private void NextWave()
		{
			_targets = Target.GenTargets (_wave * 2, _wave * 1);
			_wave++;
			_waveOscilatingDir = Direction.RIGHT;
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			EquasionInvasion app = new EquasionInvasion();
			app.StartGameLoop();
		}
	}
}
