using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spextria.Graphics;
using Spextria.Graphics.GUI;
using Spextria.Entities;
using Spextria.Maps;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.IO;
using MonoGame.Extended.ViewportAdapters;
using Spextria.Statics;
using Microsoft.Xna.Framework.Media;
using Spextria.States;
using Spextria.Master;
using System.Diagnostics.Metrics;


namespace Spextria
{
    class SpextriaGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private MasterManager _masterManager;

        private double timer;

        private State _currentState;
        private State _nextState;
        private Song song;

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public SpextriaGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //_graphics.IsFullScreen = userRequestedFullScreen;
            //_graphics.IsFullScreen = true;
            IsMouseVisible = true;


            Window.Position = new Point(0, 30);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height - 30;
            IsFixedTimeStep = false;
            //lol _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.ApplyChanges();
            timer = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.song = Content.Load<Song>("Music/main_theme");
            Images.LoadTextureDict(Content);
            Colours.LoadColours();
            LevelInfo.LoadLevelInfo();
            Npcs.LoadNpcInfo();
            Images.LoadFont(Content, GraphicsDevice);
            Effects.LoadEffects(Content);

            _masterManager = new MasterManager(Window, GraphicsDevice, _graphics, Content);

            MediaPlayer.Play(song);
            //  Uncomment the following line will also loop the song
            MediaPlayer.IsRepeating = true;
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
            _currentState = new MenuState("titlescreen");//GameState();//
            _nextState = null;
            //Texture2D texture2 = Content.Load<Texture2D>("Images/Characters/player1");

            _spriteBatch = new SpriteBatch(GraphicsDevice);


        }

        void MediaPlayer_MediaStateChanged(object sender, System.
                                           EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0f;
            MediaPlayer.Play(song);
        }

        protected override void Update(GameTime gameTime)
        {
            MediaPlayer.Volume = _masterManager.storedDataManager.Settings.MusicVolume / 100f;
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            double framerate = (1 / gameTime.ElapsedGameTime.TotalSeconds);
            if (timer > 0.5) //rate of refresh the debug
            {
                framerate = (1 / gameTime.ElapsedGameTime.TotalSeconds);
                string colour = "green";
                if (framerate < 60)
                    colour = "yellow";
                else if (framerate < 30)
                    colour = "orange";
                else if (framerate < 10)
                    colour = "red";
                else if (framerate < 1)
                    colour = "blue";

                _masterManager.playGuiManager.ReloadFps(colour, framerate);

            }


            _masterManager.dt = (60 / framerate) * _masterManager.GameSpeed;
            _masterManager.timePassed = gameTime.ElapsedGameTime.TotalSeconds * _masterManager.GameSpeed;
            _masterManager.gameTime = gameTime;


            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }

            Images.UpdateTextures(gameTime.ElapsedGameTime.TotalSeconds);

            _currentState.Update(_masterManager, gameTime, this, GraphicsDevice);

            if (_masterManager.NpcTalking)
            {
                _masterManager.mapManager.CurrentMapDrawn = false;
                if (_masterManager.NpcTalkTime < 2)
                {
                    _masterManager.MapOpacity = (float)Math.Max(0.2, 1 - _masterManager.NpcTalkTime * 0.4);
                }
                else if (_masterManager.NpcTalkTime > 10)
                {
                    _masterManager.MapOpacity = (float)Math.Min(1, Math.Max(0.2,(_masterManager.NpcTalkTime-10) * 0.4));

                }
                if (_masterManager.entityManager.Player.Dead)
                {
                    _masterManager.GameSpeed = 1;
                    _masterManager.NpcTalking = false;
                }
            }

            if (_currentState is GameState && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ChangeState(new MenuState("pause_menu", _masterManager));
                //_masterManager.storedDataManager.SaveFile();
                //Exit();
            }
            else if (_currentState is GameState && (Keyboard.GetState().IsKeyDown(Keys.D1) || _masterManager.playGuiManager.CheckWeaponButtonClicked(_masterManager)))
            {
                ChangeState(new MenuState("pri_weapon_select", _masterManager));
                //_masterManager.storedDataManager.SaveFile();
                //Exit();
            }

            MouseState mouseState = Mouse.GetState();
            _masterManager.CurrentMouseState = mouseState;
            _masterManager.MousePos = new Point((int)Math.Round(mouseState.Position.X/4.5f), (int)Math.Round(mouseState.Position.Y / 4.5f));

            if (_masterManager.levelFailed)
            {
                _masterManager.levelFailed = false;
                ChangeState(new MenuState("level_failed", _masterManager));
            }
            else if (_masterManager.levelWon)
            {
                _masterManager.levelWon = false;
                ChangeState(new MenuState("level_won", _masterManager));
            }
            if (timer > 0.5)
                timer = 0;

            base.Update(gameTime);
        }

        private void DebugRect(SpriteBatch spriteBatch, Rectangle rect, Color colour)
        {
            Texture2D _texture;

            _texture = new Texture2D(GraphicsDevice, 1, 1);
            _texture.SetData(new Color[] { colour });
            spriteBatch.Draw(_texture, rect, colour);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            GraphicsDevice.Clear(Color.Black);

            _currentState.Draw(_masterManager, _spriteBatch, GraphicsDevice);
            base.Draw(gameTime);
        }
    }
}
